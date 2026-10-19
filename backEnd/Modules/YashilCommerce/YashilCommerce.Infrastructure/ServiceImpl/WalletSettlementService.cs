using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class WalletSettlementService : IWalletSettlementService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        /// <summary>دسترسی داده و هویت درخواست را برای Settlement اتمیک دریافت می‌کند.</summary>
        public WalletSettlementService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>Capture کامل را فقط با Payment موفق، کلید تکرار معتبر و Ledger منفی اجرا می‌کند.</summary>
        public Task<WalletSettlementResultDto> CaptureAsync(WalletCaptureRequestDto request)
        {
            if (request == null || request.ReservationId <= 0 || request.PaymentId <= 0
                || string.IsNullOrWhiteSpace(request.IdempotencyKey)
                || request.IdempotencyKey.Trim().Length > 150)
                return Task.FromResult(WalletSettlementResultDto.Failure(
                    WalletSettlementErrorCode.InvalidRequest, "درخواست Capture نامعتبر است."));
            return SettleAsync(request.ReservationId, request.PaymentId, request.IdempotencyKey.Trim(), true);
        }

        /// <summary>Release کامل را با شناسه Reservation تکرارپذیر و بدون Ledger قطعی اجرا می‌کند.</summary>
        public Task<WalletSettlementResultDto> ReleaseAsync(WalletReleaseRequestDto request)
        {
            if (request == null || request.ReservationId <= 0)
                return Task.FromResult(WalletSettlementResultDto.Failure(
                    WalletSettlementErrorCode.InvalidRequest, "درخواست Release نامعتبر است."));
            return SettleAsync(request.ReservationId, null, null, false);
        }

        /// <summary>هر گذار وضعیت، تغییر دو مانده و Ledger احتمالی را در یک تراکنش Serializable نگه می‌دارد.</summary>
        private async Task<WalletSettlementResultDto> SettleAsync(int reservationId, int? paymentId,
            string code, bool capture)
        {
            if (_application.ApplicationId <= 0 || ((_principal?.Id ?? 0) <= 0 && _application.SysUserId <= 0))
                return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.InvalidRequest,
                    "کاربر و Application معتبر لازم است.");

            for (var attempt = 0; attempt < 3; attempt++)
            {
                _context.ChangeTracker.Clear();
                await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                try
                {
                    var result = await SettleWithinTransactionAsync(reservationId, paymentId, code, capture);
                    if (result.IsSuccess && !result.IsReplay)
                        await transaction.CommitAsync();
                    else
                        await transaction.RollbackAsync();
                    return result;
                }
                catch (SqlException exception) when (exception.Number == 1205)
                {
                    // SQL Server قربانی deadlock را برمی‌گرداند؛ تلاش بعدی وضعیت و کلید را دوباره می‌خواند.
                }
                catch (DbUpdateException exception) when (exception.InnerException is SqlException sql
                    && sql.Number == 1205)
                {
                    // Ledger در تراکنش باطل‌شده باقی نمی‌ماند و Retry بدون اثر مالی مضاعف انجام می‌شود.
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.ConcurrentConflict,
                "تعارض هم‌زمان در Settlement رخ داد؛ دوباره تلاش کنید.");
        }

        /// <summary>وضعیت و Payment را بازبینی و سپس اثر مالی کامل Capture یا Release را اعمال می‌کند.</summary>
        private async Task<WalletSettlementResultDto> SettleWithinTransactionAsync(int reservationId,
            int? paymentId, string code, bool capture)
        {
            var applicationId = _application.ApplicationId;
            var reservation = await _context.WalletReservation.AsNoTracking()
                .Include(x => x.WalletAssignment).ThenInclude(x => x.WalletProgram)
                .Include(x => x.Payment).ThenInclude(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id == reservationId && x.ApplicationId == applicationId && !x.Deleted);
            if (reservation == null || reservation.WalletAssignment == null
                || reservation.WalletAssignment.ApplicationId != applicationId
                || reservation.WalletAssignment.Deleted || reservation.WalletAssignment.UserId != reservation.UserId
                || reservation.WalletAssignment.WalletProgram == null
                || reservation.WalletAssignment.WalletProgram.ApplicationId != applicationId
                || reservation.WalletAssignment.WalletProgram.Deleted
                || reservation.Payment == null || reservation.Payment.Id != reservation.PaymentId
                || reservation.Payment.ApplicationId != applicationId || reservation.Payment.Deleted
                || reservation.Payment.Order == null || reservation.Payment.Order.ApplicationId != applicationId
                || reservation.Payment.Order.Deleted
                || reservation.Payment.Order.Id != reservation.Payment.OrderId
                || reservation.Payment.Order.UserId != reservation.UserId)
                return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.NotFound,
                    "Reservation در Application جاری یافت نشد.");

            WalletTransaction previous = null;
            var paymentReference = capture
                ? $"PaymentId:{paymentId.Value.ToString(CultureInfo.InvariantCulture)}" : null;
            if (capture)
            {
                var byKey = await _context.WalletTransaction.AsNoTracking()
                    .Where(x => x.ApplicationId == applicationId && x.Code == code)
                    .Take(2).ToListAsync();
                if (byKey.Count > 1)
                    return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.Conflict,
                        "کلید Capture برای چند تراکنش استفاده شده است.");
                previous = byKey.SingleOrDefault();
                if (previous != null)
                {
                    if (previous.WalletTransactionType != (int)WalletTransactionTypeEnum.Capture
                        || previous.WalletReservationId != reservation.Id
                        || previous.WalletAssignmentId != reservation.WalletAssignmentId
                        || previous.UserId != reservation.UserId || previous.Amount != -reservation.Amount
                        || previous.PaymentId != reservation.PaymentId
                        || previous.Description != paymentReference)
                        return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.Conflict,
                            "کلید Capture به درخواست دیگری تعلق دارد.");
                    if (reservation.WalletReservationStatusType != (int)WalletReservationStatusType.Captured)
                        return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.Conflict,
                            "Ledger و وضعیت Reservation ناسازگار هستند.");
                    return Result(reservation, previous.Id, true);
                }
            }
            else if (reservation.WalletReservationStatusType == (int)WalletReservationStatusType.Released)
            {
                return Result(reservation, null, true);
            }

            if (reservation.WalletReservationStatusType != (int)WalletReservationStatusType.Reserved
                || reservation.CapturedAt.HasValue || reservation.ReleasedAt.HasValue)
                return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.InvalidState,
                    "وضعیت Reservation اجازه Settlement نمی‌دهد.");
            if (reservation.Amount <= 0m || decimal.Round(reservation.Amount, 2) != reservation.Amount)
                return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.InvalidRequest,
                    "مبلغ Reservation نامعتبر است.");

            var now = DateTime.UtcNow;
            now = new DateTime(now.Ticks - now.Ticks % TimeSpan.TicksPerSecond, DateTimeKind.Utc);
            var actorId = (_principal?.Id ?? 0) > 0 ? _principal.Id : _application.SysUserId;
            if (capture)
            {
                if (paymentId.Value != reservation.PaymentId)
                    return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.Conflict,
                        "Payment با Reservation یکسان نیست.");
                var paid = await _context.Payment.AsNoTracking().AnyAsync(x => x.Id == reservation.PaymentId
                    && x.OrderId == reservation.Payment.OrderId && x.ApplicationId == applicationId
                    && !x.Deleted && x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid);
                if (!paid)
                    return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.PaymentNotPaid,
                        "Payment موفق برای این Order یافت نشد.");
            }
            else if (reservation.Payment.PaymentStatusType != (int)PaymentStatusTypesEnum.Unsuccessful
                && reservation.Payment.PaymentStatusType != (int)PaymentStatusTypesEnum.Cancel
                && reservation.Payment.PaymentStatusType != (int)PaymentStatusTypesEnum.Void)
            {
                return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.InvalidState,
                    "تا تعیین تکلیف قطعی درگاه و Capture نمی‌توان Reservation را آزاد کرد.");
            }

            var targetStatus = capture ? WalletReservationStatusType.Captured : WalletReservationStatusType.Released;
            var reservationQuery = _context.WalletReservation
                .Where(x => x.Id == reservation.Id && x.ApplicationId == applicationId && !x.Deleted
                    && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                    && !x.CapturedAt.HasValue && !x.ReleasedAt.HasValue);
            var updatedReservation = capture
                ? await reservationQuery.ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.WalletReservationStatusType, (int)targetStatus)
                    .SetProperty(x => x.CapturedAt, now)
                    .SetProperty(x => x.ModifyBy, actorId)
                    .SetProperty(x => x.ModificationDate, now))
                : await reservationQuery.ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.WalletReservationStatusType, (int)targetStatus)
                    .SetProperty(x => x.ReleasedAt, now)
                    .SetProperty(x => x.ModifyBy, actorId)
                    .SetProperty(x => x.ModificationDate, now));
            if (updatedReservation != 1)
                return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.ConcurrentConflict,
                    "Reservation هم‌زمان تغییر کرده است.");

            var amount = reservation.Amount;
            var assignmentQuery = _context.WalletAssignment
                .Where(x => x.Id == reservation.WalletAssignmentId && x.ApplicationId == applicationId
                    && !x.Deleted && x.ReservedAmount >= amount);
            var updatedAssignment = capture
                ? await assignmentQuery.ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ReservedAmount, x => x.ReservedAmount - amount)
                    .SetProperty(x => x.ConsumedAmount, x => x.ConsumedAmount + amount))
                : await assignmentQuery.ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ReservedAmount, x => x.ReservedAmount - amount));
            if (updatedAssignment != 1)
                return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.BalanceMismatch,
                    "مبلغ رزروشده Assignment با Reservation سازگار نیست.");

            var programQuery = _context.WalletProgram
                .Where(x => x.Id == reservation.WalletAssignment.WalletProgramId
                    && x.ApplicationId == applicationId && !x.Deleted && x.ReservedAmount >= amount);
            var updatedProgram = capture
                ? await programQuery.ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ReservedAmount, x => x.ReservedAmount - amount)
                    .SetProperty(x => x.ConsumedAmount, x => x.ConsumedAmount + amount))
                : await programQuery.ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ReservedAmount, x => x.ReservedAmount - amount));
            if (updatedProgram != 1)
                return WalletSettlementResultDto.Failure(WalletSettlementErrorCode.BalanceMismatch,
                    "مبلغ رزروشده Program با Reservation سازگار نیست.");

            int? transactionId = null;
            if (capture)
            {
                var ledger = new WalletTransaction
                {
                    Code = code, Title = $"Wallet capture for reservation {reservation.Id}",
                    Description = paymentReference,
                    ApplicationId = applicationId, AccessLevelId = reservation.AccessLevelId,
                    CreatorOrganizationId = (_principal?.Id ?? 0) > 0
                        ? _principal.OrganizationId : reservation.CreatorOrganizationId,
                    CreateBy = (_principal?.Id ?? 0) > 0 ? _principal.Id : _application.SysUserId,
                    CreationDate = now, OccurredAt = now, Deleted = false,
                    WalletAssignmentId = reservation.WalletAssignmentId, WalletReservationId = reservation.Id,
                    UserId = reservation.UserId, PaymentId = reservation.PaymentId,
                    WalletTransactionType = (int)WalletTransactionTypeEnum.Capture,
                    // ستون Direction فعلی اجباری است؛ مبلغ منفی معنای Debit را تعیین می‌کند.
                    WalletTransactionDirectionType = (int)WalletTransactionDirectionTypeEnum.Debit,
                    Amount = -amount
                };
                _context.WalletTransaction.Add(ledger);
                await _context.SaveChangesAsync();
                transactionId = ledger.Id;
            }

            reservation.WalletReservationStatusType = (int)targetStatus;
            reservation.CapturedAt = capture ? now : null;
            reservation.ReleasedAt = capture ? null : now;
            return Result(reservation, transactionId, false);
        }

        /// <summary>نتیجه Capture یا Release را از وضعیت ثبت‌شده و بدون تغییر مالی مجدد می‌سازد.</summary>
        private static WalletSettlementResultDto Result(WalletReservation reservation, int? transactionId, bool replay)
            => new WalletSettlementResultDto
            {
                ReservationId = reservation.Id, TransactionId = transactionId, Amount = reservation.Amount,
                Status = reservation.WalletReservationStatusType,
                SettledAt = reservation.CapturedAt ?? reservation.ReleasedAt, IsReplay = replay
            };
    }
}
