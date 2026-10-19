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
    public class WalletGrantService : IWalletGrantService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        /// <summary>دسترسی داده و هویت مدیر را برای Grant مالی دریافت می‌کند.</summary>
        public WalletGrantService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>Ledgerهای Grant با مبلغ مثبت را فقط برای Program و Application جاری نمایش می‌دهد.</summary>
        public IQueryable<WalletGrantDto> GetAll(int walletProgramId)
            => _context.WalletTransaction.AsNoTracking()
                .Where(x => x.ApplicationId == _application.ApplicationId
                    && x.WalletAssignment.WalletProgramId == walletProgramId
                    && x.WalletAssignment.ApplicationId == _application.ApplicationId
                    && x.WalletAssignment.WalletProgram.ApplicationId == _application.ApplicationId
                    && x.WalletTransactionType == (int)WalletTransactionTypeEnum.Grant
                    && x.Amount > 0m)
                .Select(x => new WalletGrantDto
                {
                    Id = x.Id, WalletProgramId = x.WalletAssignment.WalletProgramId,
                    WalletAssignmentId = x.WalletAssignmentId, UserId = x.UserId,
                    Code = x.Code, Amount = x.Amount, OccurredAt = x.OccurredAt
                });

        /// <summary>Grant را با کلید تکرار، کنترل اتمیک بودجه و به‌روزرسانی Assignment و Ledger ثبت می‌کند.</summary>
        public async Task<WalletGrantResultDto> CreateAsync(WalletGrantCreateDto request)
        {
            if (request == null || request.WalletAssignmentId <= 0
                || string.IsNullOrWhiteSpace(request.Code) || request.Code.Trim().Length > 150
                || request.Amount <= 0m || request.Amount > 9999999999999999.99m
                || decimal.Round(request.Amount, 2) != request.Amount)
                return WalletGrantResultDto.Failure("شناسه Assignment، مبلغ یا کلید Grant نامعتبر است.");
            if (_principal == null || _principal.Id <= 0 || _application.ApplicationId <= 0)
                return WalletGrantResultDto.Failure("کاربر و Application معتبر لازم است.");

            var code = request.Code.Trim();
            // Serializable خواندن کلید و درج Ledger را حتی بدون قید یکتای Code هماهنگ می‌کند.
            for (var attempt = 0; attempt < 3; attempt++)
            {
                _context.ChangeTracker.Clear();
                await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                try
                {
                    var result = await CreateWithinTransactionAsync(request, code);
                    if (result.IsSuccess && !result.IsReplay)
                        await transaction.CommitAsync();
                    else
                        await transaction.RollbackAsync();
                    return result;
                }
                catch (SqlException exception) when (exception.Number == 1205)
                {
                    // SQL Server تراکنش قربانی deadlock را خودکار برمی‌گرداند؛ تلاش بعدی با Context پاک شروع می‌شود.
                }
                catch (DbUpdateException exception) when (exception.InnerException is SqlException sql
                    && sql.Number == 1205)
                {
                    // ثبت Ledger در تراکنش باطل‌شده باقی نمی‌ماند و درخواست با همان کلید دوباره بررسی می‌شود.
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return WalletGrantResultDto.Failure("تعارض هم‌زمان در ثبت Grant رخ داد؛ دوباره تلاش کنید.");
        }

        /// <summary>ابتدا Replay را بررسی و سپس سه اثر مالی Grant را در Transaction جاری اعمال می‌کند.</summary>
        private async Task<WalletGrantResultDto> CreateWithinTransactionAsync(WalletGrantCreateDto request, string code)
        {
            var applicationId = _application.ApplicationId;
            var byKey = await _context.WalletTransaction.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId && x.Code == code)
                .Select(x => new
                {
                    x.Id, x.Code, x.WalletAssignmentId, x.UserId, x.Amount,
                    x.WalletTransactionType, x.OccurredAt,
                    WalletProgramId = x.WalletAssignment.WalletProgramId
                }).Take(2).ToListAsync();
            if (byKey.Count > 1)
                return WalletGrantResultDto.Failure("کلید Grant قبلاً برای چند تراکنش استفاده شده است.", true);
            if (byKey.Count == 1)
            {
                var previous = byKey[0];
                if (previous.WalletTransactionType != (int)WalletTransactionTypeEnum.Grant
                    || previous.WalletAssignmentId != request.WalletAssignmentId
                    || previous.Amount != request.Amount)
                    return WalletGrantResultDto.Failure("کلید Grant به درخواست دیگری تعلق دارد.", true);
                return new WalletGrantResultDto
                {
                    IsSuccess = true, IsReplay = true,
                    Grant = new WalletGrantDto
                    {
                        Id = previous.Id, WalletProgramId = previous.WalletProgramId,
                        WalletAssignmentId = previous.WalletAssignmentId, UserId = previous.UserId,
                        Amount = previous.Amount, Code = previous.Code, OccurredAt = previous.OccurredAt
                    }
                };
            }

            var now = DateTime.UtcNow;
            var assignment = await _context.WalletAssignment.AsNoTracking()
                .Where(x => x.Id == request.WalletAssignmentId && x.ApplicationId == applicationId
                    && !x.Deleted && x.WalletProgram.ApplicationId == applicationId
                    && !x.WalletProgram.Deleted)
                .Select(x => new { x.WalletProgramId, x.UserId, x.AccessLevelId,
                    x.WalletProgram.WalletProgramAllocationMethodType,
                    x.WalletProgram.TargetUserCount, x.WalletProgram.AmountPerUser })
                .FirstOrDefaultAsync();
            if (assignment == null)
                return WalletGrantResultDto.Failure("Assignment در Application جاری یافت نشد.");
            if (assignment.WalletProgramAllocationMethodType
                    == (int)WalletProgramAllocationMethodType.FixedForUser)
            {
                if (assignment.TargetUserCount <= 0 || assignment.AmountPerUser != request.Amount)
                    return WalletGrantResultDto.Failure("مبلغ Grant باید با سهم ثابت Program برابر باشد.");
                if (await _context.WalletTransaction.AsNoTracking().AnyAsync(x =>
                    x.ApplicationId == applicationId && x.UserId == assignment.UserId
                    && x.WalletAssignment.WalletProgramId == assignment.WalletProgramId
                    && x.WalletTransactionType == (int)WalletTransactionTypeEnum.Grant))
                    return WalletGrantResultDto.Failure("سهم ثابت این کاربر قبلاً تخصیص یافته است.");
            }

            var updatedProgram = await _context.WalletProgram
                .Where(x => x.Id == assignment.WalletProgramId && x.ApplicationId == applicationId
                    && !x.Deleted && x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Active
                    && x.StartDate <= now && (!x.EndDate.HasValue || x.EndDate.Value > now)
                    && x.AllocatedAmount <= x.TotalAmount - request.Amount)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.AllocatedAmount, x => x.AllocatedAmount + request.Amount)
                    .SetProperty(x => x.ModifyBy, _principal.Id)
                    .SetProperty(x => x.ModificationDate, now));
            if (updatedProgram != 1)
                return WalletGrantResultDto.Failure("Program فعال و معتبر نیست یا بودجه کافی ندارد.");

            var updatedAssignment = await _context.WalletAssignment
                .Where(x => x.Id == request.WalletAssignmentId && x.ApplicationId == applicationId
                    && x.UserId == assignment.UserId && x.WalletProgramId == assignment.WalletProgramId
                    && !x.Deleted && x.WalletAssignmentStatusType == (int)WalletAssignmentStatusTypeEnum.Active
                    && x.StartDate <= now && (!x.EndDate.HasValue || x.EndDate.Value > now))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.AssignedAmount, x => x.AssignedAmount + request.Amount)
                    .SetProperty(x => x.ModifyBy, _principal.Id)
                    .SetProperty(x => x.ModificationDate, now));
            if (updatedAssignment != 1)
                return WalletGrantResultDto.Failure("Assignment فعال و معتبر نیست.");

            var grant = new WalletTransaction
            {
                Code = code, Title = $"Wallet grant for assignment {request.WalletAssignmentId}",
                ApplicationId = applicationId, AccessLevelId = assignment.AccessLevelId,
                CreatorOrganizationId = _principal.OrganizationId, CreateBy = _principal.Id,
                CreationDate = now, OccurredAt = now, Deleted = false,
                WalletAssignmentId = request.WalletAssignmentId, UserId = assignment.UserId,
                WalletTransactionType = (int)WalletTransactionTypeEnum.Grant,
                // ستون Direction در مدل فعلی اجباری است؛ علامت Amount معنای مالی Ledger را تعیین می‌کند.
                WalletTransactionDirectionType = (int)WalletTransactionDirectionTypeEnum.Credit,
                Amount = request.Amount
            };
            _context.WalletTransaction.Add(grant);
            await _context.SaveChangesAsync();
            return new WalletGrantResultDto
            {
                IsSuccess = true,
                Grant = new WalletGrantDto
                {
                    Id = grant.Id, WalletProgramId = assignment.WalletProgramId,
                    WalletAssignmentId = grant.WalletAssignmentId, UserId = grant.UserId,
                    Code = grant.Code, Amount = grant.Amount, OccurredAt = grant.OccurredAt
                }
            };
        }
    }
}
