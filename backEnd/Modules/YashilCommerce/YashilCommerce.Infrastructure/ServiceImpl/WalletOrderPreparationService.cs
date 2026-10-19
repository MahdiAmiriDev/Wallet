using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class WalletOrderPreparationService : IWalletOrderPreparationService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;
        private readonly IOrderService _orders;
        private readonly IWalletEligibilityService _eligibility;

        public WalletOrderPreparationService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application, IOrderService orders, IWalletEligibilityService eligibility)
        {
            _context = context;
            _principal = principal;
            _application = application;
            _orders = orders;
            _eligibility = eligibility;
        }

        /// <summary>Payment و Reservation تلاش Mixed را با یک کلید، اتمیک می‌سازد یا همان تلاش را بازیابی می‌کند.</summary>
        public async Task<WalletOrderPreparationResultDto> PrepareAsync(WalletOrderPreparationRequestDto request)
        {
            if (request == null || request.OrderId <= 0 || request.WalletProgramId <= 0
                || (request.PaymentMethodType != (int)PaymentMethodTypesEnum.HekmatOnline
                    && request.PaymentMethodType != (int)PaymentMethodTypesEnum.Vibe)
                || !Guid.TryParse(request.AttemptKey, out var attemptId))
                return Failure(WalletReserveErrorCode.InvalidRequest, "درخواست یا کلید تلاش Mixed نامعتبر است.");
            var applicationId = _application.ApplicationId;
            var userId = _principal?.Id ?? 0;
            if (applicationId <= 0 || userId <= 0)
                return Failure(WalletReserveErrorCode.UnauthorizedOrder, "کاربر و Application معتبر لازم است.");
            var key = $"WalletMixed:{attemptId:N}";

            for (var retry = 0; retry < 3; retry++)
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                try
                {
                    // قفل SQL Server به Order و کلید تلاش محدود است و میان سرورها مشترک می‌ماند.
                    if (!await LockAsync(transaction, key)
                        || !await LockAsync(transaction, $"WalletMixed:Order:{applicationId}:{request.OrderId}"))
                        return Failure(WalletReserveErrorCode.Conflict, "قفل تلاش پرداخت دریافت نشد.");

                    var prior = await FindAttemptAsync(key);
                    if (prior != null)
                        return ReplayOrConflict(prior, request, applicationId, userId);

                    var order = await _context.Order.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.OrderId
                        && x.ApplicationId == applicationId && !x.Deleted);
                    if (order == null || order.UserId != userId)
                        return Failure(order == null ? WalletReserveErrorCode.OrderNotFound
                            : WalletReserveErrorCode.UnauthorizedOrder, "سفارش کاربر یافت نشد.");
                    if (!_orders.CheckOrderAllowPay((OrderStatusTypesEnum)order.OrderStatusType,
                            (SubsidyStatusTypesEnum?)order.SubsidyStatusType))
                        return Failure(WalletReserveErrorCode.Conflict, "وضعیت سفارش اجازه پرداخت نمی‌دهد.");

                    var existingReserve = await _context.WalletReservation.AsNoTracking().AnyAsync(x =>
                        x.ApplicationId == applicationId && !x.Deleted && x.Payment.OrderId == order.Id
                        && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved);
                    var pendingPayment = await _context.Payment.AsNoTracking().AnyAsync(x => x.OrderId == order.Id
                        && x.ApplicationId == applicationId && !x.Deleted
                        && (x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway
                            || x.PaymentStatusType == (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway));
                    if (existingReserve || pendingPayment)
                        return Failure(WalletReserveErrorCode.Conflict, "تلاش پرداخت دیگری برای سفارش باز است.");

                    var assignments = await _context.WalletAssignment.AsNoTracking()
                        .Include(x => x.WalletProgram)
                        .Where(x => x.ApplicationId == applicationId && !x.Deleted && x.UserId == userId
                            && x.WalletProgramId == request.WalletProgramId
                            && x.WalletAssignmentStatusType == (int)WalletAssignmentStatusTypeEnum.Active)
                        .Take(2).ToListAsync();
                    if (assignments.Count != 1)
                        return Failure(WalletReserveErrorCode.AssignmentMismatch,
                            "یک Assignment فعال و یکتا برای کاربر لازم است.");
                    var assignment = assignments[0];
                    var program = assignment.WalletProgram;
                    var now = DateTime.UtcNow;
                    if (program == null || program.ApplicationId != applicationId || program.Deleted
                        || program.WalletProgramStatusType != (int)WalletProgramStatusEnum.Active
                        || program.StartDate > now || (program.EndDate.HasValue && program.EndDate.Value <= now)
                        || assignment.StartDate > now
                        || (assignment.EndDate.HasValue && assignment.EndDate.Value <= now)
                        || !await _eligibility.IsOrderEligibleAsync(applicationId, program.Id, order.Id))
                        return Failure(WalletReserveErrorCode.ScopeMismatch,
                            "Program، Assignment یا Scope برای سفارش معتبر نیست.");

                    var payable = await _orders.CalcAmountForPayAsync(request.PaymentMethodType, order.Id);
                    var total = payable.Total;
                    var available = assignment.AssignedAmount - assignment.ConsumedAmount
                        - assignment.ReservedAmount - assignment.ExpiredAmount;
                    if (total <= 0m || decimal.Round(total, 2) != total || available <= 0m)
                        return Failure(WalletReserveErrorCode.InsufficientBalance,
                            "بدهی یا اعتبار قابل استفاده برای Mixed کافی نیست.");
                    var walletAmount = Math.Min(total, available);
                    if (walletAmount >= total)
                        return Failure(WalletReserveErrorCode.InvalidRequest,
                            "پرداخت کامل با Wallet در محدوده Mixed نیست.");
                    if ((request.PaymentMethodType == (int)PaymentMethodTypesEnum.Vibe
                            || request.PaymentMethodType == (int)PaymentMethodTypesEnum.HekmatOnline)
                        && decimal.Truncate(total - walletAmount) != total - walletAmount)
                        return Failure(WalletReserveErrorCode.InvalidRequest,
                            "مبلغ سهم درگاه باید بدون اعشار باشد.");

                    var payment = new Payment
                    {
                        Code = "1", OrderId = order.Id, ApplicationId = applicationId,
                        CreateBy = userId, CreatorOrganizationId = _principal.OrganizationId,
                        AccessLevelId = order.AccessLevelId, CreationDate = DateTime.Now,
                        PaymentMethodType = request.PaymentMethodType,
                        PaymentStatusType = (int)PaymentStatusTypesEnum.SendToPaymentGateway,
                        SaleType = (int)payable.SaleType, InstallmentCount = payable.InstallmentCount,
                        Total = total - walletAmount, ReservationNumber = string.Empty,
                        YearMonth = DateFunction.TodayShamsiYearAndMonthInt(),
                        PaidDateInt = DateFunction.TodayShamsiInt()
                    };
                    _context.Payment.Add(payment);
                    await _context.SaveChangesAsync();
                    payment.ReservationNumber = _orders.GetReservationNumber(order.Id, payment.Id);
                    await _context.SaveChangesAsync();

                    var assignmentUpdated = await _context.WalletAssignment
                        .Where(x => x.Id == assignment.Id && x.ApplicationId == applicationId && !x.Deleted
                            && x.WalletAssignmentStatusType == (int)WalletAssignmentStatusTypeEnum.Active
                            && x.StartDate <= now && (!x.EndDate.HasValue || x.EndDate.Value > now)
                            && x.AssignedAmount - x.ConsumedAmount - x.ReservedAmount - x.ExpiredAmount >= walletAmount)
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.ReservedAmount,
                            x => x.ReservedAmount + walletAmount));
                    var programUpdated = assignmentUpdated == 1 ? await _context.WalletProgram
                        .Where(x => x.Id == program.Id && x.ApplicationId == applicationId && !x.Deleted
                            && x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Active
                            && x.StartDate <= now && (!x.EndDate.HasValue || x.EndDate.Value > now))
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.ReservedAmount,
                            x => x.ReservedAmount + walletAmount)) : 0;
                    if (assignmentUpdated != 1 || programUpdated != 1)
                        return Failure(WalletReserveErrorCode.InsufficientBalance,
                            "مانده کیف پول هنگام Reserve تغییر کرده است.");

                    now = DateTime.UtcNow;
                    now = new DateTime(now.Ticks - now.Ticks % TimeSpan.TicksPerSecond, DateTimeKind.Utc);
                    var reservation = new WalletReservation
                    {
                        Code = Guid.NewGuid().ToString("N"), Title = $"Wallet reservation for order {order.Id}",
                        CreateBy = userId, CreatorOrganizationId = _principal.OrganizationId,
                        CreationDate = now, ApplicationId = applicationId,
                        AccessLevelId = assignment.AccessLevelId, WalletAssignmentId = assignment.Id,
                        UserId = userId, PaymentId = payment.Id, Amount = walletAmount,
                        WalletReservationStatusType = (int)WalletReservationStatusType.Reserved,
                        IdempotencyKey = key, ExpiresAt = now.AddMinutes(30)
                    };
                    _context.WalletReservation.Add(reservation);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Result(request, payment, reservation, false);
                }
                catch (DbUpdateException exception) when (exception.InnerException is SqlException sql
                    && (sql.Number == 2601 || sql.Number == 2627))
                {
                    await transaction.RollbackAsync();
                    await transaction.DisposeAsync();
                    _context.ChangeTracker.Clear();
                    var winner = await FindAttemptAsync(key);
                    return winner == null ? Failure(WalletReserveErrorCode.Conflict,
                        "کلید یکتا با تلاش دیگری تداخل دارد.")
                        : ReplayOrConflict(winner, request, applicationId, userId);
                }
                catch (SqlException exception) when (exception.Number == 1205)
                {
                    await transaction.RollbackAsync();
                    _context.ChangeTracker.Clear();
                }
                catch (DbUpdateException exception) when (exception.InnerException is SqlException sql
                    && sql.Number == 1205)
                {
                    await transaction.RollbackAsync();
                    _context.ChangeTracker.Clear();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return Failure(WalletReserveErrorCode.Conflict, "تعارض هم‌زمان در آماده‌سازی پرداخت رخ داد.");
        }

        private async Task<bool> LockAsync(IDbContextTransaction transaction, string resourceName)
        {
            await using var command = _context.Database.GetDbConnection().CreateCommand();
            command.Transaction = transaction.GetDbTransaction();
            command.CommandText = "DECLARE @result int; EXEC @result = sys.sp_getapplock "
                + "@Resource = @resource, @LockMode = 'Exclusive', @LockOwner = 'Transaction', "
                + "@LockTimeout = 5000; SELECT @result;";
            var resource = command.CreateParameter();
            resource.ParameterName = "@resource";
            resource.Value = resourceName;
            command.Parameters.Add(resource);
            return Convert.ToInt32(await command.ExecuteScalarAsync()) >= 0;
        }

        private Task<WalletReservation> FindAttemptAsync(string key)
            => _context.WalletReservation.AsNoTracking()
                .Include(x => x.Payment).ThenInclude(x => x.Order)
                .Include(x => x.WalletAssignment)
                .FirstOrDefaultAsync(x => x.IdempotencyKey == key);

        private static WalletOrderPreparationResultDto ReplayOrConflict(WalletReservation reservation,
            WalletOrderPreparationRequestDto request, int applicationId, int userId)
        {
            var payment = reservation.Payment;
            if (reservation.Deleted || reservation.ApplicationId != applicationId || reservation.UserId != userId
                || payment == null || payment.Deleted || payment.ApplicationId != applicationId
                || payment.OrderId != request.OrderId || payment.PaymentMethodType != request.PaymentMethodType
                || payment.Order == null || payment.Order.UserId != userId
                || reservation.WalletAssignment == null
                || reservation.WalletAssignment.ApplicationId != applicationId
                || reservation.WalletAssignment.UserId != userId
                || reservation.WalletAssignment.WalletProgramId != request.WalletProgramId
                || reservation.Amount <= 0m || payment.Total <= 0m)
                return Failure(WalletReserveErrorCode.Conflict,
                    "کلید تلاش به سفارش، کاربر، روش یا Program دیگری تعلق دارد.");
            return Result(request, payment, reservation, true);
        }

        private static WalletOrderPreparationResultDto Result(WalletOrderPreparationRequestDto request,
            Payment payment, WalletReservation reservation, bool replay)
            => new WalletOrderPreparationResultDto
            {
                OrderId = request.OrderId, WalletProgramId = request.WalletProgramId,
                PaymentId = payment.Id, PaymentMethodType = request.PaymentMethodType, IsReplay = replay,
                PaymentStatusType = payment.PaymentStatusType,
                CanRedirect = payment.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway
                    && string.IsNullOrEmpty(payment.Token)
                    && reservation.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                    && reservation.ExpiresAt > DateTime.UtcNow,
                OrderTotal = payment.Total + reservation.Amount, WalletAmount = reservation.Amount,
                RemainingPayable = payment.Total,
                Reservation = new WalletReserveResultDto
                {
                    ReservationId = reservation.Id, OrderId = request.OrderId,
                    Amount = reservation.Amount, Status = reservation.WalletReservationStatusType,
                    ExpiresAt = reservation.ExpiresAt
                }
            };

        private static WalletOrderPreparationResultDto Failure(WalletReserveErrorCode code, string message)
            => WalletOrderPreparationResultDto.Failure(code, message);
    }
}
