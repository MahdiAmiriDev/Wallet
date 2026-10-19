using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class WalletAuditService : IWalletAuditService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        /// <summary>Context خواندنی و هویت Application جاری را برای ممیزی بدون تغییر مالی دریافت می‌کند.</summary>
        public WalletAuditService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>Program را با Ledger و Reservationهای همان Application تطبیق می‌دهد و فقط مغایرت‌ها را برمی‌گرداند.</summary>
        public async Task<WalletAuditResultDto> AuditProgramAsync(int walletProgramId)
        {
            if (walletProgramId <= 0 || _principal == null || _principal.Id <= 0
                || _application.ApplicationId <= 0)
                return WalletAuditResultDto.Failure("Program، کاربر یا Application معتبر نیست.");

            var applicationId = _application.ApplicationId;
            var program = await _context.WalletProgram.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == walletProgramId
                    && x.ApplicationId == applicationId && !x.Deleted);
            if (program == null)
                return WalletAuditResultDto.Failure("WalletProgram در Application جاری یافت نشد.");

            var result = new WalletAuditResultDto
            {
                IsSuccess = true, WalletProgramId = walletProgramId, CheckedAtUtc = DateTime.UtcNow
            };
            var assignments = await _context.WalletAssignment.AsNoTracking()
                .Where(x => x.WalletProgramId == walletProgramId && x.ApplicationId == applicationId)
                .Select(x => new { x.Id, x.UserId, x.Deleted, x.AssignedAmount, x.ConsumedAmount,
                    x.ReservedAmount, x.ExpiredAmount, x.AvailableAmount })
                .ToListAsync();
            var reservations = await _context.WalletReservation.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId
                    && x.WalletAssignment.WalletProgramId == walletProgramId
                    && x.WalletAssignment.ApplicationId == applicationId)
                .Select(x => new { x.Id, x.WalletAssignmentId, x.UserId, x.PaymentId,
                    x.Amount, x.WalletReservationStatusType, x.ExpiresAt,
                    x.CapturedAt, x.ReleasedAt, x.Deleted })
                .ToListAsync();
            var transactions = await _context.WalletTransaction.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId
                    && x.WalletAssignment.WalletProgramId == walletProgramId
                    && x.WalletAssignment.ApplicationId == applicationId)
                .Select(x => new { x.Id, x.Code, x.WalletAssignmentId, x.UserId, x.PaymentId,
                    x.WalletReservationId, x.ParentId, x.Amount, x.WalletTransactionType,
                    x.WalletTransactionDirectionType, x.Deleted })
                .ToListAsync();

            var reservationById = reservations.ToDictionary(x => x.Id);
            var transactionById = transactions.ToDictionary(x => x.Id);
            var reservationGroups = reservations.ToLookup(x => x.WalletAssignmentId);
            var transactionGroups = transactions.ToLookup(x => x.WalletAssignmentId);
            var capturesByReservation = transactions
                .Where(x => x.WalletTransactionType == (int)WalletTransactionTypeEnum.Capture
                    && x.WalletReservationId.HasValue)
                .ToLookup(x => x.WalletReservationId.Value);
            var refundsByCapture = transactions
                .Where(x => x.WalletTransactionType == (int)WalletTransactionTypeEnum.Refund
                    && x.ParentId.HasValue)
                .ToLookup(x => x.ParentId.Value);

            var duplicatedCodes = await _context.WalletTransaction.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId && x.Code != null && x.Code != "")
                .GroupBy(x => x.Code).Where(x => x.Count() > 1)
                .Select(x => x.Key).ToListAsync();
            var duplicateCodeSet = new HashSet<string>(duplicatedCodes);
            var unsupportedLedger = transactions.Any(x => x.WalletTransactionType
                != (int)WalletTransactionTypeEnum.Grant
                && x.WalletTransactionType != (int)WalletTransactionTypeEnum.Capture
                && x.WalletTransactionType != (int)WalletTransactionTypeEnum.Refund
                && x.WalletTransactionType != (int)WalletTransactionTypeEnum.Expired);

            foreach (var transaction in transactions)
            {
                if (transaction.Deleted)
                    AddIssue(result, "DeletedLedger", "WalletTransaction", transaction.Id,
                        "Ledger مالی حذف‌شده علامت خورده است.");
                if (string.IsNullOrWhiteSpace(transaction.Code))
                    AddIssue(result, "MissingLedgerCode", "WalletTransaction", transaction.Id,
                        "Ledger کلید Code ندارد.");
                else if (duplicateCodeSet.Contains(transaction.Code))
                    AddIssue(result, "DuplicateLedgerCode", "WalletTransaction", transaction.Id,
                        "Code این Ledger در Application جاری تکراری است.");

                switch (transaction.WalletTransactionType)
                {
                    case (int)WalletTransactionTypeEnum.Grant:
                    case (int)WalletTransactionTypeEnum.Refund:
                        if (transaction.Amount <= 0m || transaction.WalletTransactionDirectionType
                            != (int)WalletTransactionDirectionTypeEnum.Credit)
                            AddIssue(result, "InvalidCreditLedger", "WalletTransaction", transaction.Id,
                                "Grant یا Refund باید مبلغ مثبت و جهت Credit داشته باشد.");
                        break;
                    case (int)WalletTransactionTypeEnum.Capture:
                    case (int)WalletTransactionTypeEnum.Expired:
                        if (transaction.Amount >= 0m || transaction.WalletTransactionDirectionType
                            != (int)WalletTransactionDirectionTypeEnum.Debit)
                            AddIssue(result, "InvalidDebitLedger", "WalletTransaction", transaction.Id,
                                "Capture یا Expired باید مبلغ منفی و جهت Debit داشته باشد.");
                        break;
                    default:
                        AddIssue(result, "UnreconciledLedgerType", "WalletTransaction", transaction.Id,
                            "اثر مالی این Transaction Type در قرارداد فعلی ممیزی مشخص نیست.");
                        break;
                }

                if (transaction.WalletTransactionType == (int)WalletTransactionTypeEnum.Expired
                    && transaction.WalletReservationId.HasValue)
                    AddIssue(result, "ExpiredLinkedToReservation", "WalletTransaction", transaction.Id,
                        "Expiration مالی باید در سطح Assignment ثبت شود، نه برای هر Reservation.");
                if (transaction.WalletTransactionType == (int)WalletTransactionTypeEnum.Capture)
                {
                    if (!transaction.WalletReservationId.HasValue
                        || !reservationById.TryGetValue(transaction.WalletReservationId.Value, out var sourceReservation)
                        || sourceReservation.WalletAssignmentId != transaction.WalletAssignmentId
                        || sourceReservation.UserId != transaction.UserId
                        || sourceReservation.PaymentId != transaction.PaymentId
                        || sourceReservation.Amount != -transaction.Amount
                        || sourceReservation.WalletReservationStatusType
                            != (int)WalletReservationStatusType.Captured)
                        AddIssue(result, "InvalidCaptureLink", "WalletTransaction", transaction.Id,
                            "Capture با Reservation قطعی همان Assignment و Payment سازگار نیست.");
                }
                if (transaction.WalletTransactionType == (int)WalletTransactionTypeEnum.Refund)
                {
                    if (!transaction.ParentId.HasValue
                        || !transactionById.TryGetValue(transaction.ParentId.Value, out var capture)
                        || capture.WalletTransactionType != (int)WalletTransactionTypeEnum.Capture
                        || capture.WalletAssignmentId != transaction.WalletAssignmentId
                        || capture.UserId != transaction.UserId || capture.PaymentId != transaction.PaymentId
                        || capture.WalletReservationId != transaction.WalletReservationId)
                        AddIssue(result, "InvalidRefundLink", "WalletTransaction", transaction.Id,
                            "Refund به Capture همان Assignment، کاربر و Payment متصل نیست.");
                }
            }

            foreach (var reservation in reservations)
            {
                if (reservation.Deleted && reservation.WalletReservationStatusType
                    == (int)WalletReservationStatusType.Reserved)
                    AddIssue(result, "DeletedActiveReservation", "WalletReservation", reservation.Id,
                        "Reservation دارای مانده باز، حذف‌شده علامت خورده است.");
                if (reservation.Amount <= 0m)
                    AddIssue(result, "InvalidReservationAmount", "WalletReservation", reservation.Id,
                        "مبلغ Reservation باید مثبت باشد.");
                var captures = capturesByReservation[reservation.Id].ToList();
                if (reservation.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved)
                {
                    if (reservation.CapturedAt.HasValue || reservation.ReleasedAt.HasValue)
                        AddIssue(result, "InvalidReservationDates", "WalletReservation", reservation.Id,
                            "Reservation باز نباید تاریخ Capture یا Release داشته باشد.");
                    if (reservation.ExpiresAt <= result.CheckedAtUtc)
                        AddIssue(result, "OverdueReservedReservation", "WalletReservation", reservation.Id,
                            "مهلت Reservation باز پایان یافته و هنوز تعیین تکلیف نشده است.");
                    if (captures.Count > 0)
                        AddIssue(result, "CapturedWhileReserved", "WalletReservation", reservation.Id,
                            "Reservation باز دارای Ledger Capture است.");
                }
                else if (reservation.WalletReservationStatusType == (int)WalletReservationStatusType.Captured)
                {
                    if (!reservation.CapturedAt.HasValue || reservation.ReleasedAt.HasValue)
                        AddIssue(result, "InvalidReservationDates", "WalletReservation", reservation.Id,
                            "Reservation قطعی باید فقط تاریخ Capture داشته باشد.");
                    if (captures.Count != 1)
                        AddIssue(result, "CaptureCountMismatch", "WalletReservation", reservation.Id,
                            "Reservation قطعی باید دقیقاً یک Ledger Capture داشته باشد.");
                }
                else if (reservation.WalletReservationStatusType == (int)WalletReservationStatusType.Released
                    || reservation.WalletReservationStatusType == (int)WalletReservationStatusType.Expired)
                {
                    if ((reservation.WalletReservationStatusType == (int)WalletReservationStatusType.Released
                            && (!reservation.ReleasedAt.HasValue || reservation.CapturedAt.HasValue))
                        || (reservation.WalletReservationStatusType == (int)WalletReservationStatusType.Expired
                            && (reservation.ReleasedAt.HasValue || reservation.CapturedAt.HasValue)))
                        AddIssue(result, "InvalidReservationDates", "WalletReservation", reservation.Id,
                            "تاریخ‌های Reservation با وضعیت Release یا Expired سازگار نیستند.");
                    if (captures.Count > 0)
                        AddIssue(result, "CaptureAfterReleaseOrExpiry", "WalletReservation", reservation.Id,
                            "Reservation آزاد یا منقضی‌شده دارای Ledger Capture است.");
                }
                else
                    AddIssue(result, "UnknownReservationStatus", "WalletReservation", reservation.Id,
                        "وضعیت Reservation در قرارداد فعلی Wallet شناخته‌شده نیست.");
            }

            foreach (var capture in transactions.Where(x => x.WalletTransactionType
                == (int)WalletTransactionTypeEnum.Capture))
            {
                var refunded = refundsByCapture[capture.Id].Sum(x => x.Amount);
                if (refunded < 0m || refunded > -capture.Amount)
                    AddIssue(result, "OverRefundedCapture", "WalletTransaction", capture.Id,
                        "جمع Refundهای این Capture از مبلغ مصرف قطعی بیشتر است.", -capture.Amount, refunded);
            }

            foreach (var assignment in assignments)
            {
                var ledger = transactionGroups[assignment.Id].ToList();
                foreach (var transaction in ledger.Where(x => x.UserId != assignment.UserId))
                    AddIssue(result, "LedgerUserMismatch", "WalletTransaction", transaction.Id,
                        "کاربر Ledger با Assignment آن یکسان نیست.");
                foreach (var reservation in reservationGroups[assignment.Id]
                    .Where(x => x.UserId != assignment.UserId))
                    AddIssue(result, "ReservationUserMismatch", "WalletReservation", reservation.Id,
                        "کاربر Reservation با Assignment آن یکسان نیست.");
                var activeReservations = reservationGroups[assignment.Id]
                    .Where(x => x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved)
                    .Sum(x => x.Amount);
                CheckAmount(result, "ReservedMismatch", "WalletAssignment", assignment.Id,
                    activeReservations, assignment.ReservedAmount);

                var available = assignment.AssignedAmount - assignment.ConsumedAmount
                    - assignment.ReservedAmount - assignment.ExpiredAmount;
                if (assignment.AssignedAmount < 0m || assignment.ConsumedAmount < 0m
                    || assignment.ReservedAmount < 0m || assignment.ExpiredAmount < 0m || available < 0m)
                    AddIssue(result, "InvalidAssignmentBalance", "WalletAssignment", assignment.Id,
                        "مانده‌های Assignment منفی یا مجموع مصرف، رزرو و انقضا بیش از Assigned است.");
                if (!assignment.AvailableAmount.HasValue || assignment.AvailableAmount.Value != available)
                    AddIssue(result, "AvailableMismatch", "WalletAssignment", assignment.Id,
                        "AvailableAmount با فرمول ثبت‌شده در EF سازگار نیست.", available,
                        assignment.AvailableAmount);
                if (assignment.Deleted && (available > 0m || assignment.ReservedAmount > 0m))
                    AddIssue(result, "DeletedFundedAssignment", "WalletAssignment", assignment.Id,
                        "Assignment حذف‌شده هنوز اعتبار آزاد یا Reservation باز دارد.");

                if (ledger.Any(x => x.WalletTransactionType != (int)WalletTransactionTypeEnum.Grant
                    && x.WalletTransactionType != (int)WalletTransactionTypeEnum.Capture
                    && x.WalletTransactionType != (int)WalletTransactionTypeEnum.Refund
                    && x.WalletTransactionType != (int)WalletTransactionTypeEnum.Expired))
                    continue;
                CheckAmount(result, "AssignedMismatch", "WalletAssignment", assignment.Id,
                    ledger.Where(x => x.WalletTransactionType == (int)WalletTransactionTypeEnum.Grant)
                        .Sum(x => x.Amount), assignment.AssignedAmount);
                CheckAmount(result, "ConsumedMismatch", "WalletAssignment", assignment.Id,
                    -ledger.Where(x => x.WalletTransactionType == (int)WalletTransactionTypeEnum.Capture)
                        .Sum(x => x.Amount)
                    - ledger.Where(x => x.WalletTransactionType == (int)WalletTransactionTypeEnum.Refund)
                        .Sum(x => x.Amount), assignment.ConsumedAmount);
                CheckAmount(result, "ExpiredMismatch", "WalletAssignment", assignment.Id,
                    -ledger.Where(x => x.WalletTransactionType == (int)WalletTransactionTypeEnum.Expired)
                        .Sum(x => x.Amount), assignment.ExpiredAmount);
            }

            var grants = transactions.Where(x => x.WalletTransactionType
                == (int)WalletTransactionTypeEnum.Grant).Sum(x => x.Amount);
            if (!unsupportedLedger)
                CheckAmount(result, "AllocatedLedgerMismatch", "WalletProgram", program.Id,
                    grants, program.AllocatedAmount);
            CheckAmount(result, "AllocatedAssignmentMismatch", "WalletProgram", program.Id,
                assignments.Sum(x => x.AssignedAmount), program.AllocatedAmount);
            CheckAmount(result, "ProgramConsumedMismatch", "WalletProgram", program.Id,
                assignments.Sum(x => x.ConsumedAmount), program.ConsumedAmount);
            CheckAmount(result, "ProgramReservedMismatch", "WalletProgram", program.Id,
                assignments.Sum(x => x.ReservedAmount), program.ReservedAmount);
            CheckAmount(result, "ProgramExpiredMismatch", "WalletProgram", program.Id,
                assignments.Sum(x => x.ExpiredAmount), program.ExpiredAmount);
            if (program.AllocatedAmount < 0m || program.ConsumedAmount < 0m
                || program.ReservedAmount < 0m || program.ExpiredAmount < 0m
                || program.AllocatedAmount > program.TotalAmount
                || program.ConsumedAmount + program.ReservedAmount + program.ExpiredAmount
                    > program.AllocatedAmount)
                AddIssue(result, "InvalidProgramBalance", "WalletProgram", program.Id,
                    "بودجه یا مجموع مصرف، رزرو و انقضای Program خارج از حدود Allocated است.");

            var finalVersion = await _context.WalletProgram.AsNoTracking()
                .Where(x => x.Id == walletProgramId && x.ApplicationId == applicationId && !x.Deleted)
                .Select(x => x.RowVersion).FirstOrDefaultAsync();
            if (finalVersion == null || program.RowVersion == null
                || !program.RowVersion.SequenceEqual(finalVersion))
                return WalletAuditResultDto.Failure(
                    "WalletProgram هنگام ممیزی تغییر کرد؛ برای گزارش سازگار، درخواست را تکرار کنید.");

            return result;
        }

        /// <summary>مغایرت مالی را با شناسه و مبلغ مورد انتظار و واقعی، بدون تغییر داده ثبت می‌کند.</summary>
        private static void CheckAmount(WalletAuditResultDto result, string code, string entityType,
            int entityId, decimal expected, decimal actual)
        {
            if (expected != actual)
                AddIssue(result, code, entityType, entityId,
                    "مانده ذخیره‌شده با رویدادهای مالی موجود سازگار نیست.", expected, actual);
        }

        /// <summary>یک نشانه قابل پیگیری از مغایرت را فقط به نتیجه خواندنی Audit اضافه می‌کند.</summary>
        private static void AddIssue(WalletAuditResultDto result, string code, string entityType,
            int entityId, string message, decimal? expected = null, decimal? actual = null)
            => result.Issues.Add(new WalletAuditIssueDto
            {
                Code = code, EntityType = entityType, EntityId = entityId,
                ExpectedAmount = expected, ActualAmount = actual, Message = message
            });
    }
}
