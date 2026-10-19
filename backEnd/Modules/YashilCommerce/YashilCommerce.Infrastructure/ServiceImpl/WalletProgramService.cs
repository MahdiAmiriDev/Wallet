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
    public class WalletProgramService : IWalletProgramService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        /// <summary>دسترسی داده و هویت مورد اعتماد درخواست را برای مدیریت Program دریافت می‌کند.</summary>
        public WalletProgramService(YashilAppDbContext context, IUserPrincipal principal, ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>فقط Programهای غیرحذف‌شده Application جاری را برای Grid برمی‌گرداند.</summary>
        public IQueryable<WalletProgramDto> GetAll()
        {
            var applicationId = _application.ApplicationId;
            return _context.WalletProgram.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId && !x.Deleted)
                .Select(x => new WalletProgramDto
                {
                    Id = x.Id, Code = x.Code, Title = x.Title, Description = x.Description,
                    WalletProgramType = x.WalletProgramType,
                    WalletProgramAllocationMethodType = x.WalletProgramAllocationMethodType,
                    TotalAmount = x.TotalAmount, TargetUserCount = x.TargetUserCount,
                    AmountPerUser = x.AmountPerUser, AllocatedAmount = x.AllocatedAmount,
                    ConsumedAmount = x.ConsumedAmount, ReservedAmount = x.ReservedAmount,
                    ExpiredAmount = x.ExpiredAmount, CurrencyCode = x.CurrencyCode,
                    StartDate = x.StartDate, EndDate = x.EndDate,
                    WalletProgramStatusType = x.WalletProgramStatusType, RowVersion = x.RowVersion
                });
        }

        /// <summary>جزئیات Program را تنها در Application جاری می‌خواند.</summary>
        public Task<WalletProgramDto> GetAsync(int id) => GetAll().FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>Program جدید را با وضعیت Draft و مبالغ تجمیعی صفر ثبت می‌کند.</summary>
        public async Task<WalletProgramResultDto> CreateAsync(WalletProgramEditDto request)
        {
            var error = ValidateInput(request);
            if (error != null) return WalletProgramResultDto.Failure(error);
            if (_principal == null || _principal.Id <= 0 || _application.ApplicationId <= 0)
                return WalletProgramResultDto.Failure("کاربر و Application معتبر لازم است.");
            if (request.StartDate.Date < DateTime.UtcNow.Date)
                return WalletProgramResultDto.Failure("تاریخ شروع جدید نمی‌تواند در گذشته باشد.");
            if (await CodeExistsAsync(request.Code.Trim(), 0))
                return WalletProgramResultDto.Failure("کد Program تکراری است.");

            var program = new WalletProgram
            {
                Code = request.Code.Trim(), Title = request.Title.Trim(), Description = request.Description?.Trim(),
                WalletProgramType = request.WalletProgramType,
                WalletProgramAllocationMethodType = request.WalletProgramAllocationMethodType,
                TotalAmount = request.TotalAmount, TargetUserCount = request.TargetUserCount,
                AmountPerUser = request.AmountPerUser,
                CurrencyCode = request.CurrencyCode.Trim().ToUpperInvariant(),
                StartDate = request.StartDate, EndDate = request.EndDate,
                WalletProgramStatusType = (int)WalletProgramStatusEnum.Draft, IsActive = false,
                AllocatedAmount = 0m, ConsumedAmount = 0m, ReservedAmount = 0m, ExpiredAmount = 0m,
                ApplicationId = _application.ApplicationId, AccessLevelId = (int)AccessLevelEnum.Normal,
                CreatorOrganizationId = _principal.OrganizationId, CreateBy = _principal.Id,
                CreationDate = DateTime.UtcNow, Deleted = false
            };
            _context.WalletProgram.Add(program);
            return await SaveAsync(program);
        }

        /// <summary>فقط فیلدهای مجاز را تغییر می‌دهد و مانع کاهش بودجه زیر AllocatedAmount می‌شود.</summary>
        public async Task<WalletProgramResultDto> UpdateAsync(WalletProgramEditDto request)
        {
            if (request == null || request.Id <= 0 || request.RowVersion == null || request.RowVersion.Length == 0)
                return WalletProgramResultDto.Failure("شناسه و نسخه Program لازم است.");
            var error = ValidateInput(request);
            if (error != null) return WalletProgramResultDto.Failure(error);
            var program = await FindAsync(request.Id);
            if (program == null) return WalletProgramResultDto.Failure("Program یافت نشد.");
            if (!program.RowVersion.SequenceEqual(request.RowVersion))
                return WalletProgramResultDto.Failure("Program هم‌زمان تغییر کرده است؛ اطلاعات را دوباره بارگذاری کنید.");
            if (request.TotalAmount < program.AllocatedAmount)
                return WalletProgramResultDto.Failure("بودجه جدید نمی‌تواند از اعتبار تخصیص‌یافته کمتر باشد.");
            var allocationChanged = program.TargetUserCount != request.TargetUserCount
                || program.AmountPerUser != request.AmountPerUser;

            var status = (WalletProgramStatusEnum)program.WalletProgramStatusType;
            var draft = status == WalletProgramStatusEnum.Draft;
            var financialEdit = draft || status == WalletProgramStatusEnum.Active
                || status == WalletProgramStatusEnum.Suspended;
            if (!draft && (program.WalletProgramType != request.WalletProgramType
                || program.WalletProgramAllocationMethodType != request.WalletProgramAllocationMethodType
                || program.CurrencyCode != request.CurrencyCode.Trim().ToUpperInvariant()))
                return WalletProgramResultDto.Failure("نوع، روش تخصیص و ارز پس از Draft قابل تغییر نیستند.");
            if (allocationChanged && (!financialEdit || program.AllocatedAmount > 0m))
                return WalletProgramResultDto.Failure("تعداد هدف و مبلغ هر کاربر پس از تخصیص یا پایان Program قابل تغییر نیستند.");
            if (!draft && program.WalletProgramAllocationMethodType == (int)WalletProgramAllocationMethodType.FixedForUser
                && !ValidFixedAllocation(request.TotalAmount, request.TargetUserCount, request.AmountPerUser))
                return WalletProgramResultDto.Failure("تعداد هدف یا مبلغ ثابت هر کاربر با بودجه Program سازگار نیست.");
            var datesChanged = program.StartDate != request.StartDate || program.EndDate != request.EndDate;
            if (!financialEdit && (program.TotalAmount != request.TotalAmount || datesChanged))
                return WalletProgramResultDto.Failure("بودجه و بازه Program نهایی قابل تغییر نیستند.");
            if (datesChanged && ((program.StartDate != request.StartDate && request.StartDate.Date < DateTime.UtcNow.Date)
                || (program.EndDate != request.EndDate && request.EndDate.HasValue
                    && request.EndDate.Value.Date < DateTime.UtcNow.Date)))
                return WalletProgramResultDto.Failure("تغییر تاریخ به گذشته مجاز نیست.");
            if (datesChanged && await HasDateConflictAsync(program.Id, request.StartDate, request.EndDate))
                return WalletProgramResultDto.Failure("بازه جدید با تخصیص یا رزرو باز ناسازگار است.");
            if (await CodeExistsAsync(request.Code.Trim(), program.Id))
                return WalletProgramResultDto.Failure("کد Program تکراری است.");

            program.Code = request.Code.Trim();
            program.Title = request.Title.Trim();
            program.Description = request.Description?.Trim();
            if (draft)
            {
                program.WalletProgramType = request.WalletProgramType;
                program.WalletProgramAllocationMethodType = request.WalletProgramAllocationMethodType;
                program.CurrencyCode = request.CurrencyCode.Trim().ToUpperInvariant();
            }
            if (financialEdit)
            {
                program.TotalAmount = request.TotalAmount;
                program.TargetUserCount = request.TargetUserCount;
                program.AmountPerUser = request.AmountPerUser;
                program.StartDate = request.StartDate;
                program.EndDate = request.EndDate;
            }
            program.ModifyBy = _principal.Id;
            program.ModificationDate = DateTime.UtcNow;
            return await SaveAsync(program);
        }

        /// <summary>گذار وضعیت را طبق چرخه مصوب اجرا می‌کند و Close با رزرو Reserved را رد می‌کند.</summary>
        public async Task<WalletProgramResultDto> ChangeStatusAsync(WalletProgramStatusDto request)
        {
            if (request == null || request.Id <= 0 || request.RowVersion == null || request.RowVersion.Length == 0)
                return WalletProgramResultDto.Failure("شناسه و نسخه Program لازم است.");
            var program = await FindAsync(request.Id);
            if (program == null) return WalletProgramResultDto.Failure("Program یافت نشد.");
            if (!program.RowVersion.SequenceEqual(request.RowVersion))
                return WalletProgramResultDto.Failure("Program هم‌زمان تغییر کرده است؛ اطلاعات را دوباره بارگذاری کنید.");

            var from = (WalletProgramStatusEnum)program.WalletProgramStatusType;
            var to = (WalletProgramStatusEnum)request.Status;
            if (!AllowedTransition(from, to))
                return WalletProgramResultDto.Failure("این گذار وضعیت مجاز نیست.");
            var now = DateTime.UtcNow;
            if (to == WalletProgramStatusEnum.Active && (program.TotalAmount <= 0
                || string.IsNullOrWhiteSpace(program.Code) || string.IsNullOrWhiteSpace(program.Title)
                || string.IsNullOrWhiteSpace(program.CurrencyCode) || program.WalletProgramType <= 0
                || program.WalletProgramAllocationMethodType <= 0 || program.StartDate > now
                || (program.EndDate.HasValue && program.EndDate.Value <= now)))
                return WalletProgramResultDto.Failure("اطلاعات ضروری، بودجه یا بازه اعتبار برای فعال‌سازی معتبر نیست.");
            if (to == WalletProgramStatusEnum.Active
                && program.WalletProgramAllocationMethodType == (int)WalletProgramAllocationMethodType.FixedForUser
                && !ValidFixedAllocation(program.TotalAmount, program.TargetUserCount, program.AmountPerUser))
                return WalletProgramResultDto.Failure("تعداد هدف یا مبلغ ثابت هر کاربر با بودجه Program سازگار نیست.");
            if (to == WalletProgramStatusEnum.Closed && await _context.WalletReservation.AnyAsync(x =>
                x.WalletAssignment.WalletProgramId == program.Id
                && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved))
                return WalletProgramResultDto.Failure("Program دارای رزرو باز است.");
            if (to == WalletProgramStatusEnum.Expired && (!program.EndDate.HasValue || program.EndDate.Value > now))
                return WalletProgramResultDto.Failure("بازه اعتبار Program هنوز پایان نیافته است.");

            program.WalletProgramStatusType = (int)to;
            program.IsActive = to == WalletProgramStatusEnum.Active;
            program.ModifyBy = _principal.Id;
            program.ModificationDate = now;
            return await SaveAsync(program);
        }

        /// <summary>ابتدا اعتبار Assignmentهای سررسیدشده را اتمیک منقضی و سپس وضعیت Programهای سررسیدشده را تغییر می‌دهد.</summary>
        public async Task<int> ExpireDueProgramsAsync()
        {
            var now = DateTime.UtcNow;
            var dueAssignmentIds = await _context.WalletAssignment.AsNoTracking()
                .Where(x => !x.Deleted && !x.WalletProgram.Deleted
                    && ((x.EndDate.HasValue && x.EndDate.Value <= now)
                        || (x.WalletProgram.EndDate.HasValue && x.WalletProgram.EndDate.Value <= now))
                    && (x.WalletAssignmentStatusType != (int)WalletAssignmentStatusTypeEnum.Expired
                        || x.AssignedAmount > x.ConsumedAmount + x.ExpiredAmount
                        || x.ReservedAmount > 0m))
                .OrderBy(x => x.Id).Select(x => x.Id).ToListAsync();

            Exception firstFailure = null;
            var firstFailedAssignmentId = 0;
            foreach (var assignmentId in dueAssignmentIds)
            {
                try { await ExpireDueAssignmentAsync(assignmentId); }
                catch (Exception exception)
                {
                    if (firstFailure == null)
                    {
                        firstFailure = exception;
                        firstFailedAssignmentId = assignmentId;
                    }
                }
            }

            var expiredPrograms = await _context.WalletProgram.Where(x => !x.Deleted && x.EndDate.HasValue && x.EndDate.Value <= now
                    && (x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Active
                        || x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Suspended))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.WalletProgramStatusType, (int)WalletProgramStatusEnum.Expired)
                    .SetProperty(x => x.IsActive, false)
                    .SetProperty(x => x.ModificationDate, now));
            if (firstFailure != null)
                throw new InvalidOperationException(
                    $"انقضای Assignment {firstFailedAssignmentId} انجام نشد؛ سایر Assignmentها مستقل پردازش شدند.",
                    firstFailure);
            return expiredPrograms;
        }

        /// <summary>برای هر Assignment، Reservationهای باز را Expired و فقط اعتبار مصرف‌نشده را با Ledger منفی منقضی می‌کند.</summary>
        private async Task ExpireDueAssignmentAsync(int assignmentId)
        {
            for (var attempt = 0; attempt < 3; attempt++)
            {
                _context.ChangeTracker.Clear();
                await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                try
                {
                    await ExpireAssignmentWithinTransactionAsync(assignmentId);
                    await transaction.CommitAsync();
                    return;
                }
                catch (SqlException exception) when (exception.Number == 1205)
                {
                    // SQL Server تراکنش قربانی deadlock را باطل می‌کند؛ Retry همه مانده‌ها را دوباره می‌خواند.
                }
                catch (DbUpdateException exception) when (exception.InnerException is SqlException sql
                    && sql.Number == 1205)
                {
                    // Ledger و مانده‌ها در تراکنش باطل‌شده باقی نمی‌مانند.
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            throw new InvalidOperationException($"Wallet expiration برای Assignment {assignmentId} به علت تعارض هم‌زمان انجام نشد.");
        }

        /// <summary>مبلغ آزاد و رزروشدهٔ مصرف‌نشده را یک‌بار از Assignment خارج و Ledger Expired واحد ثبت می‌کند.</summary>
        private async Task ExpireAssignmentWithinTransactionAsync(int assignmentId)
        {
            var assignment = await _context.WalletAssignment.AsNoTracking()
                .Include(x => x.WalletProgram)
                .FirstOrDefaultAsync(x => x.Id == assignmentId && !x.Deleted);
            if (assignment == null || assignment.WalletProgram == null || assignment.WalletProgram.Deleted)
                return;
            if (assignment.WalletProgram.ApplicationId != assignment.ApplicationId)
                throw new InvalidOperationException($"Application مربوط به Assignment {assignmentId} و Program سازگار نیست.");

            var now = DateTime.UtcNow;
            var due = (assignment.EndDate.HasValue && assignment.EndDate.Value <= now)
                || (assignment.WalletProgram.EndDate.HasValue && assignment.WalletProgram.EndDate.Value <= now);
            if (!due)
                return;

            var reservations = await _context.WalletReservation.AsNoTracking()
                .Where(x => x.WalletAssignmentId == assignmentId
                    && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved)
                .Select(x => new { x.Id, x.ApplicationId, x.UserId, x.Amount,
                    x.Deleted, x.CapturedAt, x.ReleasedAt })
                .ToListAsync();
            if (reservations.Any(x => x.ApplicationId != assignment.ApplicationId
                || x.UserId != assignment.UserId || x.Deleted || x.Amount <= 0m
                || x.CapturedAt.HasValue || x.ReleasedAt.HasValue)
                || reservations.Sum(x => x.Amount) != assignment.ReservedAmount)
                throw new InvalidOperationException($"مانده Reservationهای Assignment {assignmentId} ناسازگار است.");

            // تا زمان تعیین تکلیف نتیجه درگاه و Capture، اعتبار رزروشده نباید منقضی شود.
            // با تعویق کل Assignment، مانده و Ledger انقضا تا پایان این تلاش سازگار می‌مانند.
            if (await _context.WalletReservation.AsNoTracking().AnyAsync(x =>
                x.WalletAssignmentId == assignmentId && !x.Deleted
                && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                && x.PaymentId.HasValue
                && (x.Payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid
                    || x.Payment.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway
                    || x.Payment.PaymentStatusType == (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway)))
                return;

            var expiryAmount = assignment.AssignedAmount - assignment.ConsumedAmount - assignment.ExpiredAmount;
            if (expiryAmount < 0m || assignment.ReservedAmount < 0m
                || assignment.ReservedAmount > expiryAmount)
                throw new InvalidOperationException($"مانده Assignment {assignmentId} برای انقضا معتبر نیست.");

            if (expiryAmount == 0m && assignment.ReservedAmount == 0m
                && assignment.WalletAssignmentStatusType == (int)WalletAssignmentStatusTypeEnum.Expired)
                return;

            var updatedReservations = await _context.WalletReservation
                .Where(x => x.WalletAssignmentId == assignmentId
                    && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                    && !x.Deleted && !x.CapturedAt.HasValue && !x.ReleasedAt.HasValue)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.WalletReservationStatusType, (int)WalletReservationStatusType.Expired)
                    .SetProperty(x => x.ModificationDate, now));
            if (updatedReservations != reservations.Count)
                throw new InvalidOperationException($"Reservationهای Assignment {assignmentId} هم‌زمان تغییر کرده‌اند.");

            var updatedAssignment = await _context.WalletAssignment
                .Where(x => x.Id == assignmentId && !x.Deleted
                    && x.ApplicationId == assignment.ApplicationId
                    && x.AssignedAmount == assignment.AssignedAmount
                    && x.ConsumedAmount == assignment.ConsumedAmount
                    && x.ReservedAmount == assignment.ReservedAmount
                    && x.ExpiredAmount == assignment.ExpiredAmount)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ReservedAmount, 0m)
                    .SetProperty(x => x.ExpiredAmount, x => x.ExpiredAmount + expiryAmount)
                    .SetProperty(x => x.WalletAssignmentStatusType, (int)WalletAssignmentStatusTypeEnum.Expired)
                    .SetProperty(x => x.ModificationDate, now));
            if (updatedAssignment != 1)
                throw new InvalidOperationException($"Assignment {assignmentId} هم‌زمان تغییر کرده است.");

            if (expiryAmount > 0m || assignment.ReservedAmount > 0m)
            {
                var updatedProgram = await _context.WalletProgram
                    .Where(x => x.Id == assignment.WalletProgramId && !x.Deleted
                        && x.ApplicationId == assignment.ApplicationId
                        && x.ReservedAmount >= assignment.ReservedAmount)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.ReservedAmount, x => x.ReservedAmount - assignment.ReservedAmount)
                        .SetProperty(x => x.ExpiredAmount, x => x.ExpiredAmount + expiryAmount));
                if (updatedProgram != 1)
                    throw new InvalidOperationException($"مانده Program برای Assignment {assignmentId} ناسازگار است.");
            }

            if (expiryAmount > 0m)
            {
                var newExpiredAmount = assignment.ExpiredAmount + expiryAmount;
                var code = $"WalletExpire:{assignmentId}:{newExpiredAmount.ToString("0.00", CultureInfo.InvariantCulture)}";
                if (await _context.WalletTransaction.AsNoTracking()
                    .AnyAsync(x => x.ApplicationId == assignment.ApplicationId && x.Code == code))
                    throw new InvalidOperationException($"Ledger انقضای Assignment {assignmentId} با مانده فعلی سازگار نیست.");

                var occurredAt = new DateTime(now.Ticks - now.Ticks % TimeSpan.TicksPerSecond, DateTimeKind.Utc);
                var systemUserId = _application.SysUserId;
                var systemUserInApplication = systemUserId > 0 && await _context.User.AsNoTracking()
                    .AnyAsync(x => x.Id == systemUserId && x.ApplicationId == assignment.ApplicationId && !x.Deleted);
                _context.WalletTransaction.Add(new WalletTransaction
                {
                    Code = code, Title = $"Wallet expiration for assignment {assignmentId}",
                    Description = "System expiration of remaining wallet credit",
                    ApplicationId = assignment.ApplicationId, AccessLevelId = assignment.AccessLevelId,
                    CreatorOrganizationId = assignment.CreatorOrganizationId,
                    CreateBy = systemUserInApplication ? systemUserId : assignment.CreateBy,
                    CreationDate = now, OccurredAt = occurredAt, Deleted = false,
                    WalletAssignmentId = assignmentId, UserId = assignment.UserId,
                    WalletTransactionType = (int)WalletTransactionTypeEnum.Expired,
                    WalletTransactionDirectionType = (int)WalletTransactionDirectionTypeEnum.Debit,
                    Amount = -expiryAmount
                });
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>فیلدهای ضروری، اندازه متن، دقت بودجه و ترتیب تاریخ‌ها را کنترل می‌کند.</summary>
        private static string ValidateInput(WalletProgramEditDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Code) || request.Code.Trim().Length > 50
                || string.IsNullOrWhiteSpace(request.Title) || request.Title.Trim().Length > 200
                || (request.Description?.Length ?? 0) > 1000 || string.IsNullOrWhiteSpace(request.CurrencyCode)
                || request.CurrencyCode.Trim().Length != 3 || request.WalletProgramType <= 0
                || request.WalletProgramAllocationMethodType <= 0 || request.StartDate == default
                || request.TotalAmount < 0 || decimal.Round(request.TotalAmount, 2) != request.TotalAmount)
                return "اطلاعات Program یا بودجه نامعتبر است.";
            if (request.EndDate.HasValue && request.EndDate.Value <= request.StartDate)
                return "تاریخ پایان باید بعد از تاریخ شروع باشد.";
            if (request.TargetUserCount.HasValue && request.TargetUserCount.Value <= 0
                || request.AmountPerUser.HasValue && (request.AmountPerUser.Value <= 0m
                    || decimal.Round(request.AmountPerUser.Value, 2) != request.AmountPerUser.Value))
                return "تعداد هدف یا مبلغ هر کاربر نامعتبر است.";
            return null;
        }

        /// <summary>سهم ثابت را با تقسیم مساوی بودجه و باقی‌مانده تخصیص‌نیافته تطبیق می‌دهد.</summary>
        private static bool ValidFixedAllocation(decimal total, int? count, decimal? amount)
            => count > 0 && amount > 0m && total > 0m
                && amount == decimal.Truncate(total / count.Value * 100m) / 100m;

        /// <summary>Program قابل ویرایش را با Application جاری محدود می‌کند.</summary>
        private Task<WalletProgram> FindAsync(int id)
            => _context.WalletProgram.FirstOrDefaultAsync(x => x.Id == id
                && x.ApplicationId == _application.ApplicationId && !x.Deleted);

        /// <summary>وجود کد تکراری را بر اساس قید یکتای سراسری Program بررسی می‌کند.</summary>
        private Task<bool> CodeExistsAsync(string code, int exceptId)
            => _context.WalletProgram.AnyAsync(x => x.Code == code && x.Id != exceptId);

        /// <summary>مانع حذف بازه زمانیِ تخصیص مصرف‌نشده یا رزرو Reserved می‌شود.</summary>
        private async Task<bool> HasDateConflictAsync(int programId, DateTime start, DateTime? end)
        {
            if (await _context.WalletAssignment.AnyAsync(x => x.WalletProgramId == programId && !x.Deleted
                && x.AssignedAmount - x.ConsumedAmount - x.ExpiredAmount > 0
                && (x.StartDate < start || (end.HasValue && (!x.EndDate.HasValue || x.EndDate.Value > end.Value)))))
                return true;
            return await _context.WalletReservation.AnyAsync(x => x.WalletAssignment.WalletProgramId == programId
                && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                && (x.CreationDate < start || (end.HasValue && x.ExpiresAt > end.Value)));
        }

        /// <summary>گذارهای مجاز Draft، Active و Suspended را از وضعیت‌های نهایی جدا می‌کند.</summary>
        private static bool AllowedTransition(WalletProgramStatusEnum from, WalletProgramStatusEnum to)
            => (from == WalletProgramStatusEnum.Draft && to == WalletProgramStatusEnum.Active)
                || (from == WalletProgramStatusEnum.Active && (to == WalletProgramStatusEnum.Suspended
                    || to == WalletProgramStatusEnum.Closed || to == WalletProgramStatusEnum.Expired))
                || (from == WalletProgramStatusEnum.Suspended && (to == WalletProgramStatusEnum.Active
                    || to == WalletProgramStatusEnum.Closed || to == WalletProgramStatusEnum.Expired));

        /// <summary>ذخیره را با RowVersion کنترل کرده و تعارض هم‌زمان یا خطای داده را بازمی‌گرداند.</summary>
        private async Task<WalletProgramResultDto> SaveAsync(WalletProgram program)
        {
            try
            {
                await _context.SaveChangesAsync();
                return new WalletProgramResultDto { IsSuccess = true, EntityId = program.Id, Program = await GetAsync(program.Id) };
            }
            catch (DbUpdateConcurrencyException)
            {
                return WalletProgramResultDto.Failure("Program هم‌زمان تغییر کرده است؛ اطلاعات را دوباره بارگذاری کنید.");
            }
            catch (DbUpdateException)
            {
                return WalletProgramResultDto.Failure("ثبت Program به دلیل داده نامعتبر یا تعارض دیتابیس انجام نشد.");
            }
        }
    }
}
