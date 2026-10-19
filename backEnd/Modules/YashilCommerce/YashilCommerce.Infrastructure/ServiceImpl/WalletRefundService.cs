using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class WalletRefundService : IWalletRefundService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        /// <summary>Context و هویت جاری را برای Refund اتمیک Wallet دریافت می‌کند.</summary>
        public WalletRefundService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>Refund را فقط برای Capture معتبر با کلید تکرار و مبلغ مثبت، در یک تراکنش مالی اجرا می‌کند.</summary>
        public async Task<WalletRefundResultDto> RefundAsync(WalletRefundRequestDto request)
        {
            if (request == null || request.CaptureTransactionId <= 0
                || string.IsNullOrWhiteSpace(request.Code) || request.Code.Trim().Length > 150
                || request.Amount <= 0m || request.Amount > 9999999999999999.99m
                || decimal.Round(request.Amount, 2) != request.Amount)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.InvalidRequest,
                    "شناسه Capture، مبلغ یا کلید Refund نامعتبر است.");
            if (_principal == null || _principal.Id <= 0 || _application.ApplicationId <= 0)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.InvalidRequest,
                    "کاربر و Application معتبر لازم است.");

            var code = request.Code.Trim();
            for (var attempt = 0; attempt < 3; attempt++)
            {
                _context.ChangeTracker.Clear();
                await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                try
                {
                    var result = await RefundWithinTransactionAsync(request, code);
                    if (result.IsSuccess && !result.IsReplay)
                        await transaction.CommitAsync();
                    else
                        await transaction.RollbackAsync();
                    return result;
                }
                catch (SqlException exception) when (exception.Number == 1205)
                {
                    // Deadlock تراکنش را باطل می‌کند؛ تلاش بعدی Capture، کلید و مبلغ قابل Refund را دوباره می‌خواند.
                }
                catch (DbUpdateException exception) when (exception.InnerException is SqlException sql
                    && (sql.Number == 1205 || sql.Number == 2601 || sql.Number == 2627))
                {
                    // پس از تعارض درج Ledger، Retry کلید را دوباره بررسی می‌کند تا اعتبار دو بار برنگردد.
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return WalletRefundResultDto.Failure(WalletRefundErrorCode.ConcurrentConflict,
                "تعارض هم‌زمان در Refund رخ داد؛ دوباره تلاش کنید.");
        }

        /// <summary>Capture و Refundهای قبلی را کنترل و سپس کاهش مصرف و Ledger جبرانی را اتمیک ثبت می‌کند.</summary>
        private async Task<WalletRefundResultDto> RefundWithinTransactionAsync(WalletRefundRequestDto request,
            string code)
        {
            var applicationId = _application.ApplicationId;
            var capture = await _context.WalletTransaction.AsNoTracking()
                .Include(x => x.WalletAssignment).ThenInclude(x => x.WalletProgram)
                .Include(x => x.WalletReservation)
                .Include(x => x.Payment).ThenInclude(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id == request.CaptureTransactionId
                    && x.ApplicationId == applicationId && !x.Deleted);
            if (capture == null)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.NotFound,
                    "Capture در Application جاری یافت نشد.");
            if (capture.WalletTransactionType != (int)WalletTransactionTypeEnum.Capture
                || capture.WalletTransactionDirectionType != (int)WalletTransactionDirectionTypeEnum.Debit
                || capture.Amount >= 0m || capture.PaymentId <= 0
                || !capture.WalletReservationId.HasValue || capture.WalletAssignment == null
                || capture.WalletAssignment.ApplicationId != applicationId
                || capture.WalletAssignment.Deleted || capture.WalletAssignment.UserId != capture.UserId
                || capture.WalletAssignment.WalletProgram == null
                || capture.WalletAssignment.WalletProgram.ApplicationId != applicationId
                || capture.WalletAssignment.WalletProgram.Deleted
                || capture.Payment == null || capture.Payment.Id != capture.PaymentId
                || capture.Payment.ApplicationId != applicationId || capture.Payment.Deleted
                || capture.Payment.Order == null || capture.Payment.Order.ApplicationId != applicationId
                || capture.Payment.Order.Deleted
                || capture.Payment.Order.Id != capture.Payment.OrderId
                || capture.Payment.Order.UserId != capture.UserId
                || capture.WalletReservation == null
                || capture.WalletReservation.ApplicationId != applicationId
                || capture.WalletReservation.Deleted
                || capture.WalletReservation.Id != capture.WalletReservationId.Value
                || capture.WalletReservation.WalletAssignmentId != capture.WalletAssignmentId
                || capture.WalletReservation.PaymentId != capture.PaymentId
                || capture.WalletReservation.UserId != capture.UserId
                || capture.WalletReservation.WalletReservationStatusType
                    != (int)WalletReservationStatusType.Captured
                || capture.WalletReservation.Amount != -capture.Amount)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.InvalidCapture,
                    "Capture، Assignment، Reservation یا Order سازگار نیستند.");

            var byKey = await _context.WalletTransaction.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId && x.Code == code)
                .Take(2).ToListAsync();
            if (byKey.Count > 1)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.Conflict,
                    "کلید Refund برای چند تراکنش استفاده شده است.");
            if (byKey.Count == 1)
            {
                var previous = byKey[0];
                if (previous.Deleted || previous.WalletTransactionType != (int)WalletTransactionTypeEnum.Refund
                    || previous.WalletTransactionDirectionType != (int)WalletTransactionDirectionTypeEnum.Credit
                    || previous.ParentId != capture.Id
                    || previous.WalletAssignmentId != capture.WalletAssignmentId
                    || previous.WalletReservationId != capture.WalletReservationId
                    || previous.PaymentId != capture.PaymentId || previous.UserId != capture.UserId
                    || previous.Amount != request.Amount)
                    return WalletRefundResultDto.Failure(WalletRefundErrorCode.Conflict,
                        "کلید Refund به درخواست دیگری تعلق دارد.");
                return Result(previous, capture.Id, capture.Payment.OrderId, true);
            }

            // خواندن فرزندان Capture زیر Serializable، جمع Refund را در برابر درخواست‌های هم‌زمان محافظت می‌کند.
            var earlierRefunds = await _context.WalletTransaction.AsNoTracking()
                .Where(x => x.ParentId == capture.Id
                    && x.WalletTransactionType == (int)WalletTransactionTypeEnum.Refund)
                .Select(x => new { x.ApplicationId, x.Deleted, x.WalletAssignmentId, x.PaymentId,
                    x.UserId, x.WalletReservationId, x.WalletTransactionDirectionType, x.Amount })
                .ToListAsync();
            if (earlierRefunds.Any(x => x.ApplicationId != applicationId || x.Deleted
                || x.WalletAssignmentId != capture.WalletAssignmentId
                || x.PaymentId != capture.PaymentId || x.UserId != capture.UserId
                || x.WalletReservationId != capture.WalletReservationId
                || x.WalletTransactionDirectionType != (int)WalletTransactionDirectionTypeEnum.Credit
                || x.Amount <= 0m))
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.BalanceMismatch,
                    "Ledger Refund قبلی با Capture سازگار نیست.");
            var capturedAmount = -capture.Amount;
            var refundedAmount = earlierRefunds.Sum(x => x.Amount);
            if (refundedAmount > capturedAmount)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.BalanceMismatch,
                    "مجموع Refundهای قبلی از Capture بیشتر است.");
            if (request.Amount > capturedAmount - refundedAmount)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.ExceedsCapturedAmount,
                    "مبلغ Refund از مانده قابل برگشت Capture بیشتر است.");

            var now = DateTime.UtcNow;
            now = new DateTime(now.Ticks - now.Ticks % TimeSpan.TicksPerSecond, DateTimeKind.Utc);
            var updatedAssignment = await _context.WalletAssignment
                .Where(x => x.Id == capture.WalletAssignmentId && x.ApplicationId == applicationId
                    && !x.Deleted && x.UserId == capture.UserId
                    && x.WalletProgramId == capture.WalletAssignment.WalletProgramId
                    && x.ConsumedAmount >= request.Amount)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ConsumedAmount, x => x.ConsumedAmount - request.Amount));
            if (updatedAssignment != 1)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.BalanceMismatch,
                    "مانده مصرف‌شده Assignment برای Refund کافی نیست.");

            var updatedProgram = await _context.WalletProgram
                .Where(x => x.Id == capture.WalletAssignment.WalletProgramId
                    && x.ApplicationId == applicationId && !x.Deleted
                    && x.ConsumedAmount >= request.Amount)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ConsumedAmount, x => x.ConsumedAmount - request.Amount));
            if (updatedProgram != 1)
                return WalletRefundResultDto.Failure(WalletRefundErrorCode.BalanceMismatch,
                    "مانده مصرف‌شده Program برای Refund کافی نیست.");

            var refund = new WalletTransaction
            {
                Code = code, Title = $"Wallet refund for capture {capture.Id}",
                Description = $"CaptureId:{capture.Id}", ParentId = capture.Id,
                ApplicationId = applicationId, AccessLevelId = capture.AccessLevelId,
                CreatorOrganizationId = _principal.OrganizationId, CreateBy = _principal.Id,
                CreationDate = now, OccurredAt = now, Deleted = false,
                WalletAssignmentId = capture.WalletAssignmentId,
                WalletReservationId = capture.WalletReservationId,
                UserId = capture.UserId, PaymentId = capture.PaymentId,
                WalletTransactionType = (int)WalletTransactionTypeEnum.Refund,
                WalletTransactionDirectionType = (int)WalletTransactionDirectionTypeEnum.Credit,
                Amount = request.Amount
            };
            _context.WalletTransaction.Add(refund);
            await _context.SaveChangesAsync();
            return Result(refund, capture.Id, capture.Payment.OrderId, false);
        }

        /// <summary>نتیجه Ledger جبرانی را برای ثبت جدید یا Replay بدون اثر مالی دوباره می‌سازد.</summary>
        private static WalletRefundResultDto Result(WalletTransaction refund, int captureId, int orderId,
            bool replay)
            => new WalletRefundResultDto
            {
                RefundTransactionId = refund.Id, CaptureTransactionId = captureId,
                OrderId = orderId, WalletAssignmentId = refund.WalletAssignmentId,
                Amount = refund.Amount, OccurredAt = refund.OccurredAt, IsReplay = replay
            };
    }
}
