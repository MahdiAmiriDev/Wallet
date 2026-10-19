using System;
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
    public class WalletAssignmentService : IWalletAssignmentService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        /// <summary>دسترسی داده و هویت معتبر مدیر را برای ساخت عضویت Wallet دریافت می‌کند.</summary>
        public WalletAssignmentService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>Assignmentهای غیرحذف‌شده Program را در Application جاری برمی‌گرداند.</summary>
        public IQueryable<WalletAssignmentDto> GetAll(int walletProgramId)
            => GetAllForApplication().Where(x => x.WalletProgramId == walletProgramId);

        /// <summary>Assignment مشخص را با همان فیلتر Application می‌خواند.</summary>
        public Task<WalletAssignmentDto> GetAsync(int id)
            => GetAllForApplication().FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>فقط کاربران فعال و غیرحذف‌شده Application جاری را برای Target ارائه می‌کند.</summary>
        public IQueryable<WalletAssignmentUserDto> GetUsers()
            => _context.User.AsNoTracking()
                .Where(x => x.ApplicationId == _application.ApplicationId && !x.Deleted && x.IsActive)
                .Select(x => new WalletAssignmentUserDto { Id = x.Id, Title = x.UserName });

        /// <summary>Assignment دستی را برای Program فعال با مانده صفر می‌سازد؛ بودجه و Ledger دست‌نخورده‌اند.</summary>
        public async Task<WalletAssignmentResultDto> CreateAsync(WalletAssignmentCreateDto request)
        {
            if (request == null || request.WalletProgramId <= 0 || request.UserId <= 0
                || string.IsNullOrWhiteSpace(request.Title) || request.Title.Trim().Length > 300)
                return WalletAssignmentResultDto.Failure("اطلاعات Assignment نامعتبر است.");
            if (_principal == null || _principal.Id <= 0 || _application.ApplicationId <= 0)
                return WalletAssignmentResultDto.Failure("کاربر و Application معتبر لازم است.");

            var applicationId = _application.ApplicationId;
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var now = DateTime.UtcNow;
            var locked = await _context.WalletProgram.Where(x => x.Id == request.WalletProgramId
                    && x.ApplicationId == applicationId && !x.Deleted
                    && x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Active
                    && x.StartDate <= now && (!x.EndDate.HasValue || x.EndDate.Value > now))
                .ExecuteUpdateAsync(update => update
                    .SetProperty(x => x.ModifyBy, _principal.Id)
                    .SetProperty(x => x.ModificationDate, now));
            if (locked != 1)
                return WalletAssignmentResultDto.Failure("Program برای ایجاد Assignment فعال یا معتبر نیست.");

            var userExists = await _context.User.AnyAsync(x => x.Id == request.UserId
                && x.ApplicationId == applicationId && !x.Deleted && x.IsActive);
            if (!userExists)
                return WalletAssignmentResultDto.Failure("کاربر فعال در Application جاری یافت نشد.");
            var duplicate = await _context.WalletAssignment.AnyAsync(x => x.WalletProgramId == request.WalletProgramId
                && x.UserId == request.UserId
                && x.WalletAssignmentStatusType == (int)WalletAssignmentStatusTypeEnum.Active);
            if (duplicate)
                return WalletAssignmentResultDto.Failure("برای این کاربر و Program یک Assignment فعال وجود دارد.");

            var programEndDate = await _context.WalletProgram.AsNoTracking()
                .Where(x => x.Id == request.WalletProgramId)
                .Select(x => x.EndDate).SingleAsync();
            var assignment = new WalletAssignment
            {
                Code = Guid.NewGuid().ToString("N"), Title = request.Title.Trim(),
                Description = request.Description?.Trim(), WalletProgramId = request.WalletProgramId,
                UserId = request.UserId, AssignedAmount = 0m, ConsumedAmount = 0m,
                ReservedAmount = 0m, ExpiredAmount = 0m, StartDate = now,
                EndDate = programEndDate, WalletAssignmentStatusType = (int)WalletAssignmentStatusTypeEnum.Active,
                WalletAssignmentSourceType = (int)WalletAssignmentSourceTypeEnum.Manual,
                ApplicationId = applicationId, AccessLevelId = (int)AccessLevelEnum.Normal,
                CreatorOrganizationId = _principal.OrganizationId, CreateBy = _principal.Id,
                CreationDate = now, Deleted = false
            };
            _context.WalletAssignment.Add(assignment);
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateException exception) when (IsUniqueViolation(exception))
            {
                return WalletAssignmentResultDto.Failure("برای این کاربر و Program یک Assignment فعال وجود دارد.");
            }
            await transaction.CommitAsync();
            return new WalletAssignmentResultDto { IsSuccess = true, Assignment = await GetAsync(assignment.Id) };
        }

        /// <summary>Assignmentها را همراه Program فقط در Application جاری به DTO خواندنی تبدیل می‌کند.</summary>
        private IQueryable<WalletAssignmentDto> GetAllForApplication()
            => _context.WalletAssignment.AsNoTracking()
                .Where(x => x.ApplicationId == _application.ApplicationId && !x.Deleted
                    && x.WalletProgram.ApplicationId == _application.ApplicationId && !x.WalletProgram.Deleted)
                .Select(x => new WalletAssignmentDto
                {
                    Id = x.Id, WalletProgramId = x.WalletProgramId, UserId = x.UserId,
                    Title = x.Title, Description = x.Description, AssignedAmount = x.AssignedAmount,
                    ConsumedAmount = x.ConsumedAmount, ReservedAmount = x.ReservedAmount,
                    ExpiredAmount = x.ExpiredAmount, AvailableAmount = x.AvailableAmount,
                    StartDate = x.StartDate, EndDate = x.EndDate,
                    WalletAssignmentStatusType = x.WalletAssignmentStatusType,
                    WalletAssignmentSourceType = x.WalletAssignmentSourceType, RowVersion = x.RowVersion
                });

        /// <summary>تعارض قید یکتای Assignment فعال را از خطاهای دیگر پایگاه داده جدا می‌کند.</summary>
        private static bool IsUniqueViolation(DbUpdateException exception)
            => exception.InnerException is SqlException sqlException
                && (sqlException.Number == 2601 || sqlException.Number == 2627);
    }
}
