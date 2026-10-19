using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Yashil.Common.Core.Classes;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class WalletBenefitsService : IWalletBenefitsService
    {
        private readonly YashilAppDbContext _db;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _app;
        private readonly IConfiguration _configuration;

        public WalletBenefitsService(YashilAppDbContext db, IUserPrincipal principal,
            ApplicationConfiguration app, IConfiguration configuration)
        {
            _db = db; _principal = principal; _app = app; _configuration = configuration;
        }

        private bool HasIdentity => _principal?.Id > 0 && _app.ApplicationId > 0;

        /// <summary>گروه‌های کارت Program را بدون شماره خام نمایش می‌دهد.</summary>
        public IQueryable<WalletCardGroupDto> GetCardGroups(int programId)
            => _db.WalletCardGroup.AsNoTracking()
                .Where(x => x.WalletProgramId == programId && x.ApplicationId == _app.ApplicationId
                    && !x.Deleted && x.WalletProgram.ApplicationId == _app.ApplicationId)
                .Select(x => new WalletCardGroupDto { Id = x.Id, WalletProgramId = x.WalletProgramId,
                    Title = x.Title, WalletCardGroupStatusType = x.WalletCardGroupStatusType,
                    MemberCount = x.WalletCardMember.Count(m => !m.Deleted) });

        /// <summary>فقط نسخه ماسک‌شده کارت‌های گروه را نمایش می‌دهد.</summary>
        public IQueryable<WalletCardMemberDto> GetCardMembers(int groupId)
            => _db.WalletCardMember.AsNoTracking()
                .Where(x => x.WalletCardGroupId == groupId && x.ApplicationId == _app.ApplicationId
                    && !x.Deleted && x.WalletCardGroup.ApplicationId == _app.ApplicationId)
                .Select(x => new WalletCardMemberDto { Id = x.Id, WalletCardGroupId = x.WalletCardGroupId,
                    MaskedCard = x.MaskedCard, WalletCardMemberStatusType = x.WalletCardMemberStatusType });

        /// <summary>Batchهای Program را بدون کد خام یا Hash نمایش می‌دهد.</summary>
        public IQueryable<WalletCouponBatchDto> GetCouponBatches(int programId)
            => _db.WalletCouponBatch.AsNoTracking()
                .Where(x => x.WalletProgramId == programId && x.ApplicationId == _app.ApplicationId
                    && !x.Deleted && x.WalletProgram.ApplicationId == _app.ApplicationId)
                .Select(x => new WalletCouponBatchDto { Id = x.Id, WalletProgramId = x.WalletProgramId,
                    AttemptKey = x.Code, Title = x.Title, Quantity = x.Quantity,
                    AmountPerCoupon = x.AmountPerCoupon,
                    WalletCouponBatchStatusType = x.WalletCouponBatchStatusType });

        /// <summary>گروه کارت را برای Program جاری در وضعیت Draft ثبت می‌کند.</summary>
        public async Task<WalletBenefitResultDto> CreateCardGroupAsync(WalletCardGroupCreateDto request)
        {
            if (!HasIdentity || request == null || request.WalletProgramId <= 0
                || string.IsNullOrWhiteSpace(request.Title) || request.Title.Trim().Length > 200
                || request.StartDate == default || request.EndDate <= request.StartDate)
                return Fail("اطلاعات گروه کارت نامعتبر است.");
            var program = await _db.WalletProgram.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == request.WalletProgramId && x.ApplicationId == _app.ApplicationId && !x.Deleted);
            if (program == null || program.WalletProgramStatusType != (int)WalletProgramStatusEnum.Draft)
                return Fail("گروه کارت فقط برای Program در Draft ساخته می‌شود.");
            var group = new WalletCardGroup
            {
                Code = Guid.NewGuid().ToString("N"), Title = request.Title.Trim(),
                WalletProgramId = program.Id, StartDate = request.StartDate, EndDate = request.EndDate,
                WalletCardGroupStatusType = (int)WalletCardGroupStatusType.Draft,
                ApplicationId = _app.ApplicationId, AccessLevelId = (int)AccessLevelEnum.Normal,
                CreateBy = _principal.Id, CreatorOrganizationId = _principal.OrganizationId,
                CreationDate = DateTime.UtcNow
            };
            _db.WalletCardGroup.Add(group);
            await _db.SaveChangesAsync();
            return Success(group.Id, program.Id);
        }

        /// <summary>شماره کارت معتبر را فقط به‌شکل HMAC و نسخه ماسک‌شده ثبت می‌کند.</summary>
        public async Task<WalletBenefitResultDto> AddCardAsync(WalletCardMemberCreateDto request)
        {
            if (!HasIdentity || request == null || request.WalletCardGroupId <= 0
                || !TryNormalizeCard(request.CardNumber, out var normalized)
                || !TryKey("WalletSecurity:CardHmacKey", out var key))
                return Fail("شماره کارت یا تنظیم امن کلید کارت نامعتبر است.");
            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            if (!await LockAsync(tx, $"WalletBenefit:CardGroup:{_app.ApplicationId}:{request.WalletCardGroupId}"))
                return Fail("گروه کارت در حال تغییر است.");
            var group = await _db.WalletCardGroup.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == request.WalletCardGroupId && x.ApplicationId == _app.ApplicationId && !x.Deleted
                && x.WalletCardGroupStatusType == (int)WalletCardGroupStatusType.Draft
                && x.WalletProgram.ApplicationId == _app.ApplicationId && !x.WalletProgram.Deleted);
            if (group == null) return Fail("گروه Draft یافت نشد.");
            var hash = Fingerprint(key, normalized);
            if (await _db.WalletCardMember.AnyAsync(x => x.WalletCardGroupId == group.Id && x.CardFingerprint == hash))
                return Fail("این کارت در گروه ثبت شده است.");
            var member = new WalletCardMember
            {
                Code = Guid.NewGuid().ToString("N"), Title = "کارت عضو",
                WalletCardGroupId = group.Id, CardFingerprint = hash,
                MaskedCard = "************" + normalized.Substring(12),
                WalletCardMemberStatusType = (int)WalletCardMemberStatusType.Active,
                ApplicationId = _app.ApplicationId, AccessLevelId = (int)AccessLevelEnum.Normal,
                CreateBy = _principal.Id, CreatorOrganizationId = _principal.OrganizationId,
                CreationDate = DateTime.UtcNow
            };
            _db.WalletCardMember.Add(member);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return Fail("ثبت کارت انجام نشد یا کارت تکراری است."); }
            await tx.CommitAsync();
            return Success(member.Id, group.WalletProgramId);
        }

        /// <summary>گروه دارای کارت را برای Match فعال می‌کند.</summary>
        public async Task<WalletBenefitResultDto> ActivateCardGroupAsync(int groupId)
        {
            if (!HasIdentity || groupId <= 0) return Fail("گروه کارت نامعتبر است.");
            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            if (!await LockAsync(tx, $"WalletBenefit:CardGroup:{_app.ApplicationId}:{groupId}"))
                return Fail("گروه کارت در حال تغییر است.");
            var group = await _db.WalletCardGroup.FirstOrDefaultAsync(x => x.Id == groupId
                && x.ApplicationId == _app.ApplicationId && !x.Deleted);
            if (group == null || group.WalletCardGroupStatusType != (int)WalletCardGroupStatusType.Draft
                || !await _db.WalletCardMember.AnyAsync(x => x.WalletCardGroupId == groupId && !x.Deleted))
                return Fail("گروه Draft دارای کارت یافت نشد.");
            group.WalletCardGroupStatusType = (int)WalletCardGroupStatusType.Active;
            group.ModifyBy = _principal.Id; group.ModificationDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Success(group.Id, group.WalletProgramId);
        }

        /// <summary>کارت گیرنده متعلق به کاربر را Match و عضویت یا Grant مجاز را ثبت می‌کند.</summary>
        public async Task<WalletBenefitResultDto> MatchRecipientCardAsync(int recipientId)
        {
            if (!HasIdentity || recipientId <= 0 || !TryKey("WalletSecurity:CardHmacKey", out var key))
                return Fail("گیرنده یا تنظیم امن کلید کارت نامعتبر است.");
            var recipient = await _db.Recipient.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == recipientId && x.CreateBy == _principal.Id
                && x.ApplicationId == _app.ApplicationId && !x.Deleted);
            if (recipient == null || !TryNormalizeCard(recipient.CardNumber, out var normalized))
                return Fail("کارت معتبر برای گیرنده متعلق به کاربر یافت نشد.");
            var hash = Fingerprint(key, normalized);
            var now = DateTime.UtcNow;
            var matches = await _db.WalletCardMember.AsNoTracking()
                .Where(x => x.CardFingerprint == hash && !x.Deleted
                    && x.ApplicationId == _app.ApplicationId
                    && x.WalletCardMemberStatusType == (int)WalletCardMemberStatusType.Active
                    && x.WalletCardGroup.ApplicationId == _app.ApplicationId
                    && !x.WalletCardGroup.Deleted
                    && x.WalletCardGroup.WalletCardGroupStatusType == (int)WalletCardGroupStatusType.Active
                    && x.WalletCardGroup.StartDate <= now
                    && (!x.WalletCardGroup.EndDate.HasValue || x.WalletCardGroup.EndDate > now)
                    && x.WalletCardGroup.WalletProgram.ApplicationId == _app.ApplicationId
                    && !x.WalletCardGroup.WalletProgram.Deleted)
                .Select(x => new { x.CardFingerprint, x.WalletCardGroup.WalletProgramId })
                .ToListAsync();
            var programIds = matches.Where(x => SameHash(x.CardFingerprint, hash))
                .Select(x => x.WalletProgramId).Distinct().ToArray();
            if (programIds.Length == 0) return Fail("کارت در Program فعالی عضو نیست.");
            var successes = 0;
            foreach (var programId in programIds)
            {
                _db.ChangeTracker.Clear();
                await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                if (!await LockAsync(tx, $"WalletBenefit:User:{_app.ApplicationId}:{programId}:{_principal.Id}"))
                    continue;
                var program = await ActiveProgramAsync(programId);
                if (program == null) continue;
                var assignment = await EnsureAssignmentAsync(program, WalletAssignmentSourceTypeEnum.Card);
                if (assignment == null) continue;
                if (program.WalletProgramAllocationMethodType == (int)WalletProgramAllocationMethodType.FixedForUser)
                {
                    if (!program.AmountPerUser.HasValue || program.AmountPerUser <= 0m
                        || !await GrantOnceAsync(program, assignment, program.AmountPerUser.Value,
                            $"WalletCard:{program.Id}:{_principal.Id}", true)) continue;
                }
                else if (program.WalletProgramAllocationMethodType != (int)WalletProgramAllocationMethodType.Manual)
                    continue;
                await tx.CommitAsync();
                _db.ChangeTracker.Clear();
                successes++;
            }
            return successes == 0 ? Fail("برای کارت، عضویت یا اعتبار قابل ثبت نبود.")
                : new WalletBenefitResultDto { IsSuccess = true, Message = "تطبیق کارت انجام شد." };
        }

        /// <summary>Batch را با کلید درخواست پایدار و کدهای تصادفی یک‌بارنمایش در Draft می‌سازد.</summary>
        public async Task<WalletCouponBatchResultDto> CreateCouponBatchAsync(WalletCouponBatchCreateDto request)
        {
            if (!HasIdentity || request == null || !Guid.TryParse(request.AttemptKey, out var attempt)
                || request.WalletProgramId <= 0 || request.Quantity <= 0 || request.Quantity > 10000
                || request.AmountPerCoupon <= 0m
                || decimal.Round(request.AmountPerCoupon, 2) != request.AmountPerCoupon
                || string.IsNullOrWhiteSpace(request.Title) || request.Title.Trim().Length > 200
                || request.StartDate == default || request.EndDate <= request.StartDate
                || !TryKey("WalletSecurity:CouponHmacKey", out var key))
                return BatchFailure("Batch یا تنظیم امن کلید کوپن نامعتبر است.");
            var batchCode = $"WalletCouponBatch:{attempt:N}";
            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            if (!await LockAsync(tx, $"WalletBenefit:Batch:{_app.ApplicationId}:{batchCode}"))
                return BatchFailure("درخواست ساخت Batch در حال پردازش است.");
            var existing = await _db.WalletCouponBatch.AsNoTracking()
                .Where(x => x.ApplicationId == _app.ApplicationId && x.Code == batchCode)
                .Take(2).ToListAsync();
            if (existing.Count > 1) return BatchFailure("کلید Batch به بیش از یک رکورد تعلق دارد.");
            if (existing.Count == 1)
            {
                var prior = existing[0];
                if (prior.WalletProgramId != request.WalletProgramId || prior.Quantity != request.Quantity
                    || prior.AmountPerCoupon != request.AmountPerCoupon || prior.Title != request.Title.Trim()
                    || prior.StartDate != request.StartDate || prior.EndDate != request.EndDate)
                    return BatchFailure("کلید Batch با درخواست قبلی تعارض دارد.");
                return new WalletCouponBatchResultDto { IsSuccess = true, IsReplay = true,
                    EntityId = prior.Id, WalletProgramId = prior.WalletProgramId,
                    RequiresCancellation = prior.WalletCouponBatchStatusType == (int)WalletCouponBatchStatusType.Draft,
                    Message = "Batch قبلاً ساخته شده است؛ کدهای خام قابل بازیابی نیستند." };
            }
            if (!await LockAsync(tx, "WalletBenefit:CouponGeneration"))
                return BatchFailure("تولید کدهای کوپن در حال پردازش است.");
            var program = await _db.WalletProgram.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == request.WalletProgramId && x.ApplicationId == _app.ApplicationId && !x.Deleted
                && (x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Draft
                    || x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Active));
            if (program == null || program.WalletProgramAllocationMethodType
                    != (int)WalletProgramAllocationMethodType.FixedForUser
                || program.AmountPerUser != request.AmountPerCoupon)
                return BatchFailure("کوپن تنها برای Program با سهم ثابت برابر AmountPerCoupon مجاز است.");
            var now = DateTime.UtcNow;
            var batch = new WalletCouponBatch
            {
                Code = batchCode, Title = request.Title.Trim(), WalletProgramId = program.Id,
                Quantity = request.Quantity, CodeLength = 8, AmountPerCoupon = request.AmountPerCoupon,
                StartDate = request.StartDate, EndDate = request.EndDate,
                WalletCouponBatchStatusType = (int)WalletCouponBatchStatusType.Draft,
                ApplicationId = _app.ApplicationId, AccessLevelId = (int)AccessLevelEnum.Normal,
                CreateBy = _principal.Id, CreatorOrganizationId = _principal.OrganizationId,
                CreationDate = now
            };
            _db.WalletCouponBatch.Add(batch);
            await _db.SaveChangesAsync();
            var rawCodes = new List<string>(request.Quantity);
            var hashes = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < request.Quantity; i++)
            {
                string code, hash;
                var attempts = 0;
                do
                {
                    if (++attempts > 100) return BatchFailure("فضای کدهای یکتا برای Batch کافی نیست.");
                    code = RandomNumberGenerator.GetInt32(0, 100000000).ToString("D8");
                    hash = Fingerprint(key, code);
                } while (!hashes.Add(hash) || await _db.WalletCoupon.AnyAsync(x => x.CodeHash == hash));
                rawCodes.Add(code);
                _db.WalletCoupon.Add(new WalletCoupon
                {
                    Code = Guid.NewGuid().ToString("N"), Title = "کوپن کیف پول",
                    WalletCouponBatchId = batch.Id, CodeHash = hash,
                    MaskedCode = "******" + code.Substring(6),
                    WalletCouponStatusType = (int)WalletCouponStatusType.Available,
                    ApplicationId = _app.ApplicationId, AccessLevelId = (int)AccessLevelEnum.Normal,
                    CreateBy = _principal.Id, CreatorOrganizationId = _principal.OrganizationId,
                    CreationDate = now
                });
            }
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return new WalletCouponBatchResultDto { IsSuccess = true, EntityId = batch.Id,
                WalletProgramId = program.Id, Codes = rawCodes,
                Message = "کدهای خام فقط در همین پاسخ نمایش داده می‌شوند." };
        }

        /// <summary>پس از تأیید دریافت کدهای خام، Batch کامل را فعال می‌کند.</summary>
        public async Task<WalletBenefitResultDto> ActivateCouponBatchAsync(int batchId)
        {
            if (!HasIdentity || batchId <= 0) return Fail("Batch نامعتبر است.");
            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            var batch = await _db.WalletCouponBatch.FirstOrDefaultAsync(x => x.Id == batchId
                && x.ApplicationId == _app.ApplicationId && !x.Deleted);
            if (batch == null || !await LockAsync(tx,
                    $"WalletBenefit:Batch:{_app.ApplicationId}:{batch.Code}"))
                return Fail("Batch یافت نشد یا در حال پردازش است.");
            await _db.Entry(batch).ReloadAsync();
            if (batch == null || batch.WalletCouponBatchStatusType != (int)WalletCouponBatchStatusType.Draft
                || batch.StartDate > DateTime.UtcNow || batch.EndDate <= DateTime.UtcNow
                || await _db.WalletCoupon.CountAsync(x => x.WalletCouponBatchId == batchId && !x.Deleted)
                    != batch.Quantity || await ActiveProgramAsync(batch.WalletProgramId) == null)
                return Fail("Batch کامل و معتبر برای فعال‌سازی یافت نشد.");
            batch.WalletCouponBatchStatusType = (int)WalletCouponBatchStatusType.Active;
            batch.ModifyBy = _principal.Id; batch.ModificationDate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Success(batch.Id, batch.WalletProgramId);
        }

        /// <summary>Batch با پاسخ گم‌شده را با همان کلید درخواست در Draft لغو می‌کند.</summary>
        public async Task<WalletBenefitResultDto> CancelCouponBatchAsync(string attemptKey)
        {
            if (attemptKey?.StartsWith("WalletCouponBatch:", StringComparison.Ordinal) == true)
                attemptKey = attemptKey.Substring("WalletCouponBatch:".Length);
            if (!HasIdentity || !Guid.TryParse(attemptKey, out var attempt)) return Fail("کلید Batch نامعتبر است.");
            var batchCode = $"WalletCouponBatch:{attempt:N}";
            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            if (!await LockAsync(tx, $"WalletBenefit:Batch:{_app.ApplicationId}:{batchCode}"))
                return Fail("Batch در حال پردازش است.");
            var batch = await _db.WalletCouponBatch.FirstOrDefaultAsync(x => x.Code == batchCode
                && x.ApplicationId == _app.ApplicationId && !x.Deleted);
            if (batch == null || batch.WalletCouponBatchStatusType != (int)WalletCouponBatchStatusType.Draft)
                return Fail("Batch قابل لغو در Draft یافت نشد.");
            batch.WalletCouponBatchStatusType = (int)WalletCouponBatchStatusType.Cancelled;
            batch.ModifyBy = _principal.Id; batch.ModificationDate = DateTime.UtcNow;
            await _db.WalletCoupon.Where(x => x.WalletCouponBatchId == batch.Id
                    && x.WalletCouponStatusType == (int)WalletCouponStatusType.Available)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.WalletCouponStatusType,
                    (int)WalletCouponStatusType.Cancelled));
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Success(batch.Id, batch.WalletProgramId);
        }

        /// <summary>Claim و Grant کوپن را در یک تراکنش ثبت و درخواست تکراری همان کاربر را بازیابی می‌کند.</summary>
        public async Task<WalletBenefitResultDto> ClaimCouponAsync(string code)
        {
            if (!HasIdentity || code == null || code.Length != 8 || code.Any(c => c < '0' || c > '9')
                || !TryKey("WalletSecurity:CouponHmacKey", out var key))
                return Fail("کد یا تنظیم امن کوپن نامعتبر است.");
            var hash = Fingerprint(key, code);
            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            if (!await LockAsync(tx, $"WalletBenefit:Coupon:{hash}"))
                return Fail("کد در حال بررسی است.");
            var coupon = await _db.WalletCoupon.AsNoTracking().FirstOrDefaultAsync(x =>
                x.CodeHash == hash && x.ApplicationId == _app.ApplicationId && !x.Deleted);
            if (coupon == null || !SameHash(coupon.CodeHash, hash)) return Fail("کد معتبر نیست.");
            var ledgerCode = $"WalletCoupon:{coupon.Id}";
            if (coupon.WalletCouponStatusType == (int)WalletCouponStatusType.Claimed)
            {
                var previous = await _db.WalletTransaction.AsNoTracking().Where(x =>
                    x.Code == ledgerCode && x.UserId == _principal.Id
                    && x.ApplicationId == _app.ApplicationId
                    && x.WalletTransactionType == (int)WalletTransactionTypeEnum.Grant)
                    .Select(x => new { x.Amount, x.WalletAssignmentId,
                        x.WalletAssignment.WalletProgramId }).Take(2).ToListAsync();
                var priorBatch = await _db.WalletCouponBatch.AsNoTracking()
                    .Where(x => x.Id == coupon.WalletCouponBatchId && x.ApplicationId == _app.ApplicationId)
                    .Select(x => new { x.WalletProgramId, x.AmountPerCoupon }).FirstOrDefaultAsync();
                return coupon.UserId == _principal.Id && previous.Count == 1 && priorBatch != null
                    && previous[0].Amount == priorBatch.AmountPerCoupon
                    && previous[0].WalletProgramId == priorBatch.WalletProgramId
                    ? new WalletBenefitResultDto { IsSuccess = true, IsReplay = true,
                        EntityId = coupon.Id, WalletProgramId = priorBatch.WalletProgramId }
                    : Fail("کد قبلاً استفاده شده یا نیازمند بررسی مالی است.");
            }
            var now = DateTime.UtcNow;
            var batch = await _db.WalletCouponBatch.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == coupon.WalletCouponBatchId && x.ApplicationId == _app.ApplicationId && !x.Deleted
                && x.WalletCouponBatchStatusType == (int)WalletCouponBatchStatusType.Active
                && x.StartDate <= now && (!x.EndDate.HasValue || x.EndDate > now));
            if (coupon.WalletCouponStatusType != (int)WalletCouponStatusType.Available || batch == null)
                return Fail("کوپن فعال یا معتبر نیست.");
            if (!await LockAsync(tx, $"WalletBenefit:User:{_app.ApplicationId}:{batch.WalletProgramId}:{_principal.Id}"))
                return Fail("تخصیص کاربر در حال پردازش است.");
            var program = await ActiveProgramAsync(batch.WalletProgramId);
            if (program == null || program.WalletProgramAllocationMethodType
                    != (int)WalletProgramAllocationMethodType.FixedForUser
                || program.AmountPerUser != batch.AmountPerCoupon)
                return Fail("Program برای Grant کوپن معتبر نیست.");
            var assignment = await EnsureAssignmentAsync(program, WalletAssignmentSourceTypeEnum.Coupon);
            if (assignment == null) return Fail("Assignment فعال کاربر قابل ایجاد نیست.");
            var claimed = await _db.WalletCoupon.Where(x => x.Id == coupon.Id
                    && x.WalletCouponStatusType == (int)WalletCouponStatusType.Available && !x.Deleted)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.WalletCouponStatusType, (int)WalletCouponStatusType.Claimed)
                    .SetProperty(x => x.UserId, _principal.Id)
                    .SetProperty(x => x.ClaimedAt, now));
            if (claimed != 1 || !await GrantOnceAsync(program, assignment,
                    batch.AmountPerCoupon, ledgerCode))
                return Fail("Claim یا Grant انجام نشد؛ کوپن مصرف نشده است.");
            await tx.CommitAsync();
            return Success(coupon.Id, program.Id);
        }

        /// <summary>Program عملیاتی و معتبر Application جاری را برای تخصیص می‌خواند.</summary>
        private Task<WalletProgram> ActiveProgramAsync(int id)
        {
            var now = DateTime.UtcNow;
            return _db.WalletProgram.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id
                && x.ApplicationId == _app.ApplicationId && !x.Deleted
                && x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Active
                && x.StartDate <= now && (!x.EndDate.HasValue || x.EndDate > now));
        }

        /// <summary>Assignment فعال موجود را بازمی‌گرداند یا عضویت صفرمانده می‌سازد.</summary>
        private async Task<WalletAssignment> EnsureAssignmentAsync(WalletProgram program,
            WalletAssignmentSourceTypeEnum source)
        {
            var assignments = await _db.WalletAssignment.AsNoTracking()
                .Where(x => x.WalletProgramId == program.Id && x.UserId == _principal.Id
                    && x.ApplicationId == _app.ApplicationId && !x.Deleted
                    && x.WalletAssignmentStatusType == (int)WalletAssignmentStatusTypeEnum.Active)
                .Take(2).ToListAsync();
            if (assignments.Count > 1) return null;
            if (assignments.Count == 1) return assignments[0];
            var now = DateTime.UtcNow;
            var assignment = new WalletAssignment
            {
                Code = Guid.NewGuid().ToString("N"), Title = program.Title,
                WalletProgramId = program.Id, UserId = _principal.Id,
                AssignedAmount = 0m, ConsumedAmount = 0m, ReservedAmount = 0m, ExpiredAmount = 0m,
                StartDate = now, EndDate = program.EndDate,
                WalletAssignmentStatusType = (int)WalletAssignmentStatusTypeEnum.Active,
                WalletAssignmentSourceType = (int)source,
                ApplicationId = _app.ApplicationId, AccessLevelId = (int)AccessLevelEnum.Normal,
                CreateBy = _principal.Id, CreatorOrganizationId = _principal.OrganizationId,
                CreationDate = now
            };
            _db.WalletAssignment.Add(assignment);
            await _db.SaveChangesAsync();
            return assignment;
        }

        /// <summary>بودجه، Assignment و Ledger را در تراکنش جاری با کلید Grant قطعی به‌روز می‌کند.</summary>
        private async Task<bool> GrantOnceAsync(WalletProgram program, WalletAssignment assignment,
            decimal amount, string ledgerCode, bool acceptExistingAllocation = false)
        {
            var existing = await _db.WalletTransaction.AsNoTracking()
                .Where(x => x.Code == ledgerCode && x.ApplicationId == _app.ApplicationId)
                .Take(2).ToListAsync();
            if (existing.Count > 0)
                return existing.Count == 1 && existing[0].WalletAssignmentId == assignment.Id
                    && existing[0].UserId == _principal.Id && existing[0].Amount == amount
                    && existing[0].WalletTransactionType == (int)WalletTransactionTypeEnum.Grant
                    && existing[0].WalletTransactionDirectionType == (int)WalletTransactionDirectionTypeEnum.Credit;
            if (program.WalletProgramAllocationMethodType != (int)WalletProgramAllocationMethodType.FixedForUser
                || program.AmountPerUser != amount || program.TargetUserCount <= 0)
                return false;
            // در روش ثابت، یک کاربر تنها یک تخصیص مالی از Program می‌گیرد.
            var priorGrants = await _db.WalletTransaction.AsNoTracking().Where(x =>
                x.UserId == _principal.Id && x.ApplicationId == _app.ApplicationId
                && x.WalletAssignment.WalletProgramId == program.Id
                && x.WalletTransactionType == (int)WalletTransactionTypeEnum.Grant)
                .Select(x => new { x.WalletAssignmentId, x.Amount,
                    x.WalletTransactionDirectionType }).Take(2).ToListAsync();
            if (priorGrants.Count > 0)
                return acceptExistingAllocation && priorGrants.Count == 1
                    && priorGrants[0].WalletAssignmentId == assignment.Id
                    && priorGrants[0].Amount == amount
                    && priorGrants[0].WalletTransactionDirectionType
                        == (int)WalletTransactionDirectionTypeEnum.Credit;
            var now = DateTime.UtcNow;
            var budget = await _db.WalletProgram.Where(x => x.Id == program.Id
                    && x.ApplicationId == _app.ApplicationId && !x.Deleted
                    && x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Active
                    && x.AllocatedAmount <= x.TotalAmount - amount)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.AllocatedAmount,
                    x => x.AllocatedAmount + amount));
            if (budget != 1) return false;
            var assigned = await _db.WalletAssignment.Where(x => x.Id == assignment.Id
                    && x.ApplicationId == _app.ApplicationId && !x.Deleted
                    && x.WalletAssignmentStatusType == (int)WalletAssignmentStatusTypeEnum.Active)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.AssignedAmount,
                    x => x.AssignedAmount + amount));
            if (assigned != 1) return false;
            _db.WalletTransaction.Add(new WalletTransaction
            {
                Code = ledgerCode, Title = "Wallet benefit grant",
                WalletAssignmentId = assignment.Id, UserId = _principal.Id,
                WalletTransactionType = (int)WalletTransactionTypeEnum.Grant,
                WalletTransactionDirectionType = (int)WalletTransactionDirectionTypeEnum.Credit,
                Amount = amount, OccurredAt = now,
                ApplicationId = _app.ApplicationId, AccessLevelId = assignment.AccessLevelId,
                CreateBy = _principal.Id, CreatorOrganizationId = _principal.OrganizationId,
                CreationDate = now
            });
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>قفل SQL Server را تا پایان Transaction برای کلید مالی نگه می‌دارد.</summary>
        private async Task<bool> LockAsync(IDbContextTransaction transaction, string name)
        {
            await using var command = _db.Database.GetDbConnection().CreateCommand();
            command.Transaction = transaction.GetDbTransaction();
            command.CommandText = "DECLARE @result int; EXEC @result = sys.sp_getapplock "
                + "@Resource=@name, @LockMode='Exclusive', @LockOwner='Transaction', "
                + "@LockTimeout=5000; SELECT @result;";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@name"; parameter.Value = name;
            command.Parameters.Add(parameter);
            return Convert.ToInt32(await command.ExecuteScalarAsync()) >= 0;
        }

        private bool TryKey(string setting, out byte[] key)
        {
            key = null;
            try
            {
                var configured = _configuration[setting];
                if (string.IsNullOrWhiteSpace(configured)) return false;
                key = Convert.FromBase64String(configured);
                return key.Length >= 32;
            }
            catch (FormatException) { return false; }
        }

        private static string Fingerprint(byte[] key, string value)
            => Convert.ToHexString(HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(value)));

        private static bool SameHash(string stored, string computed)
        {
            if (stored == null || stored.Length != 64) return false;
            try { return CryptographicOperations.FixedTimeEquals(
                Convert.FromHexString(stored), Convert.FromHexString(computed)); }
            catch (FormatException) { return false; }
        }

        private static bool TryNormalizeCard(string input, out string normalized)
        {
            normalized = null;
            if (string.IsNullOrWhiteSpace(input)) return false;
            var digits = new StringBuilder(16);
            foreach (var c in input)
            {
                if (c >= '0' && c <= '9') digits.Append(c);
                else if (c >= '۰' && c <= '۹') digits.Append((char)('0' + c - '۰'));
                else if (c >= '٠' && c <= '٩') digits.Append((char)('0' + c - '٠'));
                else if (c != ' ' && c != '-') return false;
            }
            if (digits.Length != 16) return false;
            var sum = 0;
            for (var i = 0; i < 16; i++)
            {
                var digit = digits[i] - '0';
                if (i % 2 == 0) digit = digit * 2 > 9 ? digit * 2 - 9 : digit * 2;
                sum += digit;
            }
            if (sum % 10 != 0) return false;
            normalized = digits.ToString();
            return true;
        }

        private static WalletBenefitResultDto Success(int id, int programId)
            => new WalletBenefitResultDto { IsSuccess = true, EntityId = id, WalletProgramId = programId };
        private static WalletBenefitResultDto Fail(string message)
            => WalletBenefitResultDto.Failure(message);
        private static WalletCouponBatchResultDto BatchFailure(string message)
            => new WalletCouponBatchResultDto { Message = message };
    }
}
