using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class WalletReservationService : IWalletReservationService
    {
        private readonly IWalletReservationRepository _repository;
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public WalletReservationService(IWalletReservationRepository repository, YashilAppDbContext context,
            IUserPrincipal userPrincipal, ApplicationConfiguration applicationConfiguration)
        {
            _repository = repository;
            _context = context;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }

        public async Task<WalletReserveResultDto> ReserveAsync(WalletReserveRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.IdempotencyKey)
                || request.IdempotencyKey.Length > 100 || request.OrderId <= 0 || request.WalletAssignmentId <= 0)
                return WalletReserveResultDto.Failure(WalletReserveErrorCode.InvalidRequest, "Invalid wallet reserve request.");

            if (request.Amount <= 0 || decimal.Round(request.Amount, 2) != request.Amount)
                return WalletReserveResultDto.Failure(WalletReserveErrorCode.InvalidRequest, "Invalid wallet reserve amount.");

            var userId = _userPrincipal?.Id ?? 0;
            var applicationId = _applicationConfiguration.ApplicationId;
            if (userId <= 0 || applicationId <= 0)
                return WalletReserveResultDto.Failure(WalletReserveErrorCode.UnauthorizedOrder, "A current user and application are required.");

            var order = await _repository.FindOrderAsync(applicationId, request.OrderId);
            if (order == null)
                return WalletReserveResultDto.Failure(WalletReserveErrorCode.OrderNotFound, "Order not found.");
            if (order.UserId != userId)
                return WalletReserveResultDto.Failure(WalletReserveErrorCode.UnauthorizedOrder, "The current user does not own this order.");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var byKey = await _repository.FindByIdempotencyKeyAsync(applicationId, request.IdempotencyKey);
                if (byKey != null)
                {
                    await transaction.RollbackAsync();
                    return Matches(byKey, request, applicationId, userId)
                        ? Replay(byKey)
                        : WalletReserveResultDto.Failure(WalletReserveErrorCode.Conflict, "Idempotency key belongs to a different request.");
                }

                var active = await _repository.FindActiveByOrderAsync(applicationId, request.OrderId,
                    (int)WalletReservationStatusType.Reserved);
                if (active != null)
                {
                    await transaction.RollbackAsync();
                    return Matches(active, request, applicationId, userId)
                        ? Replay(active)
                        : WalletReserveResultDto.Failure(WalletReserveErrorCode.Conflict, "Order already has an active wallet reservation.");
                }

                var assignment = await _repository.FindAssignmentWithProgramAsync(request.WalletAssignmentId);
                if (assignment == null || assignment.UserId != userId || assignment.ApplicationId != applicationId
                    || assignment.WalletProgram == null || assignment.WalletProgram.Id != assignment.WalletProgramId
                    || assignment.WalletProgram.ApplicationId != applicationId || assignment.WalletProgram.Deleted)
                {
                    await transaction.RollbackAsync();
                    return WalletReserveResultDto.Failure(WalletReserveErrorCode.AssignmentMismatch, "Wallet assignment or program is invalid.");
                }

                var updatedAssignment = await _repository.TryIncreaseAssignmentReservedAsync(
                    assignment.Id, applicationId, userId, request.Amount);
                if (updatedAssignment != 1)
                {
                    await transaction.RollbackAsync();
                    return WalletReserveResultDto.Failure(WalletReserveErrorCode.InsufficientBalance, "Insufficient available wallet balance.");
                }

                var updatedProgram = await _repository.IncreaseProgramReservedAsync(
                    assignment.WalletProgramId, applicationId, request.Amount);
                if (updatedProgram != 1)
                {
                    await transaction.RollbackAsync();
                    return WalletReserveResultDto.Failure(WalletReserveErrorCode.AssignmentMismatch, "Wallet program is unavailable.");
                }

                var creationTime = DateTime.UtcNow;
                creationTime = new DateTime(creationTime.Ticks - creationTime.Ticks % TimeSpan.TicksPerSecond, DateTimeKind.Utc);
                // TODO: Replace fixed 30-minute reservation expiration with the final Checkout/Payment expiration policy.
                var reservation = new Yashil.Core.Entities.WalletReservation
                {
                    Code = Guid.NewGuid().ToString("N"),
                    Title = $"Wallet reservation for order {request.OrderId}",
                    CreateBy = userId,
                    CreationDate = creationTime,
                    ApplicationId = applicationId,
                    AccessLevelId = assignment.AccessLevelId,
                    CreatorOrganizationId = _userPrincipal.OrganizationId,
                    WalletAssignmentId = assignment.Id,
                    UserId = userId,
                    OrderId = request.OrderId,
                    OrderItemId = null,
                    Amount = request.Amount,
                    WalletReservationStatusType = (int)WalletReservationStatusType.Reserved,
                    IdempotencyKey = request.IdempotencyKey,
                    ExpiresAt = creationTime.AddMinutes(30)
                };

                await _repository.InsertAsync(reservation);
                await transaction.CommitAsync();
                return Replay(reservation);
            }
            catch (DbUpdateException exception) when (IsUniqueViolation(exception))
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                var byKey = await _repository.FindByIdempotencyKeyAsync(applicationId, request.IdempotencyKey);
                if (byKey != null)
                    return Matches(byKey, request, applicationId, userId)
                        ? Replay(byKey)
                        : WalletReserveResultDto.Failure(WalletReserveErrorCode.Conflict, "Idempotency key belongs to a different request.");

                var active = await _repository.FindActiveByOrderAsync(applicationId, request.OrderId,
                    (int)WalletReservationStatusType.Reserved);
                if (active != null)
                    return Matches(active, request, applicationId, userId)
                        ? Replay(active)
                        : WalletReserveResultDto.Failure(WalletReserveErrorCode.Conflict, "Order already has an active wallet reservation.");

                throw;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static bool Matches(Yashil.Core.Entities.WalletReservation reservation,
            WalletReserveRequestDto request, int applicationId, int userId)
            => reservation.ApplicationId == applicationId && reservation.UserId == userId
                && reservation.OrderId == request.OrderId
                && reservation.WalletAssignmentId == request.WalletAssignmentId
                && reservation.Amount == request.Amount;

        private static WalletReserveResultDto Replay(Yashil.Core.Entities.WalletReservation reservation)
            => new WalletReserveResultDto
            {
                ReservationId = reservation.Id,
                OrderId = reservation.OrderId,
                Amount = reservation.Amount,
                Status = reservation.WalletReservationStatusType,
                ExpiresAt = reservation.ExpiresAt
            };

        private static bool IsUniqueViolation(DbUpdateException exception)
            => exception.InnerException is SqlException sqlException
                && (sqlException.Number == 2601 || sqlException.Number == 2627);
    }
}
