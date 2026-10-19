using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class WalletScopeService : IWalletScopeService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        /// <summary>دسترسی داده و هویت درخواست را برای مدیریت Scope دریافت می‌کند.</summary>
        public WalletScopeService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>Scopeهای غیرحذف‌شده Program را فقط در Application جاری برمی‌گرداند.</summary>
        public IQueryable<WalletScopeDto> GetAll(int walletProgramId)
            => _context.WalletScope.AsNoTracking()
                .Where(x => x.WalletProgramId == walletProgramId
                    && x.ApplicationId == _application.ApplicationId && !x.Deleted
                    && x.WalletProgram.ApplicationId == _application.ApplicationId && !x.WalletProgram.Deleted)
                .Select(x => new WalletScopeDto
                {
                    Id = x.Id, WalletProgramId = x.WalletProgramId, WalletScopeType = x.WalletScopeType,
                    CompanyId = x.CompanyId, RepresentationId = x.RepresentationId,
                    CityId = x.CityId, ProductId = x.ProductId, Title = x.Title,
                    Description = x.Description, IsActive = x.IsActive, RowVersion = x.RowVersion
                });

        /// <summary>Scope مشخص را با فیلتر Application می‌خواند.</summary>
        public Task<WalletScopeDto> GetAsync(long id)
            => GetAllForApplication().FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>Targetهای موجود را از Entityهای اصلی و با فیلتر Application برای انتخاب مدیر می‌خواند.</summary>
        public IQueryable<WalletScopeTargetDto> GetTargets(int scopeType)
        {
            var applicationId = _application.ApplicationId;
            return (WalletScopeTypeEnum)scopeType switch
            {
                WalletScopeTypeEnum.Company => _context.Company.AsNoTracking()
                    .Where(x => x.ApplicationId == applicationId && !x.Deleted)
                    .Select(x => new WalletScopeTargetDto { Id = x.Id, Title = x.Title }),
                WalletScopeTypeEnum.Representation => _context.Representation.AsNoTracking()
                    .Where(x => x.ApplicationId == applicationId && !x.Deleted)
                    .Select(x => new WalletScopeTargetDto { Id = x.Id, Title = x.Title }),
                WalletScopeTypeEnum.City => _context.City.AsNoTracking()
                    .Where(x => !x.Deleted)
                    .Select(x => new WalletScopeTargetDto { Id = x.Id, Title = x.Title }),
                WalletScopeTypeEnum.Product => _context.Product.AsNoTracking()
                    .Where(x => x.ApplicationId == applicationId && !x.Deleted)
                    .Select(x => new WalletScopeTargetDto { Id = x.Id, Title = x.Title }),
                _ => _context.City.AsNoTracking().Where(x => false)
                    .Select(x => new WalletScopeTargetDto { Id = x.Id, Title = x.Title })
            };
        }

        /// <summary>Scope معتبر را در Draft و در همان Transaction قفل Program ایجاد می‌کند.</summary>
        public async Task<WalletScopeResultDto> CreateAsync(WalletScopeEditDto request)
        {
            if (request == null || request.WalletProgramId <= 0 || request.Id != 0)
                return WalletScopeResultDto.Failure("درخواست Scope نامعتبر است.");
            var error = await ValidateAsync(request);
            if (error != null) return WalletScopeResultDto.Failure(error);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            if (!await LockDraftProgramAsync(request.WalletProgramId))
                return WalletScopeResultDto.Failure("تغییر Scope فقط برای Program در وضعیت Draft مجاز است.");

            var now = DateTime.UtcNow;
            var scope = new WalletScope
            {
                Code = Guid.NewGuid().ToString("N"), Title = request.Title.Trim(),
                Description = request.Description?.Trim(), WalletProgramId = request.WalletProgramId,
                WalletScopeType = request.WalletScopeType, CompanyId = request.CompanyId,
                RepresentationId = request.RepresentationId, CityId = request.CityId,
                ProductId = request.ProductId, IsActive = request.IsActive,
                ApplicationId = _application.ApplicationId, AccessLevelId = (int)AccessLevelEnum.Normal,
                CreatorOrganizationId = _principal.OrganizationId, CreateBy = _principal.Id,
                CreationDate = now, Deleted = false
            };
            _context.WalletScope.Add(scope);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return new WalletScopeResultDto { IsSuccess = true, Scope = await GetAsync(scope.Id) };
        }

        /// <summary>فیلدهای Scope را فقط در Draft و با کنترل نسخه رکورد تغییر می‌دهد.</summary>
        public async Task<WalletScopeResultDto> UpdateAsync(WalletScopeEditDto request)
        {
            if (request == null || request.Id <= 0 || request.WalletProgramId <= 0
                || request.RowVersion == null || request.RowVersion.Length == 0)
                return WalletScopeResultDto.Failure("شناسه و نسخه Scope لازم است.");
            var error = await ValidateAsync(request);
            if (error != null) return WalletScopeResultDto.Failure(error);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            if (!await LockDraftProgramAsync(request.WalletProgramId))
                return WalletScopeResultDto.Failure("تغییر Scope فقط برای Program در وضعیت Draft مجاز است.");
            var scope = await FindAsync(request.Id);
            if (scope == null || scope.WalletProgramId != request.WalletProgramId)
                return WalletScopeResultDto.Failure("Scope یافت نشد.");
            if (!scope.RowVersion.SequenceEqual(request.RowVersion))
                return WalletScopeResultDto.Failure("Scope هم‌زمان تغییر کرده است؛ اطلاعات را دوباره بارگذاری کنید.");

            scope.Title = request.Title.Trim();
            scope.Description = request.Description?.Trim();
            scope.WalletScopeType = request.WalletScopeType;
            scope.CompanyId = request.CompanyId;
            scope.RepresentationId = request.RepresentationId;
            scope.CityId = request.CityId;
            scope.ProductId = request.ProductId;
            scope.IsActive = request.IsActive;
            scope.ModifyBy = _principal.Id;
            scope.ModificationDate = DateTime.UtcNow;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                return WalletScopeResultDto.Failure("Scope هم‌زمان تغییر کرده است؛ اطلاعات را دوباره بارگذاری کنید.");
            }
            await transaction.CommitAsync();
            return new WalletScopeResultDto { IsSuccess = true, Scope = await GetAsync(scope.Id) };
        }

        /// <summary>حذف نرم Scope را فقط در وضعیت Draft و با کنترل نسخه انجام می‌دهد.</summary>
        public async Task<WalletScopeResultDto> DeleteAsync(WalletScopeDeleteDto request)
        {
            if (request == null || request.Id <= 0 || request.RowVersion == null || request.RowVersion.Length == 0)
                return WalletScopeResultDto.Failure("شناسه و نسخه Scope لازم است.");
            var scope = await FindAsync(request.Id);
            if (scope == null) return WalletScopeResultDto.Failure("Scope یافت نشد.");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            if (!await LockDraftProgramAsync(scope.WalletProgramId))
                return WalletScopeResultDto.Failure("حذف Scope فقط برای Program در وضعیت Draft مجاز است.");
            if (!scope.RowVersion.SequenceEqual(request.RowVersion))
                return WalletScopeResultDto.Failure("Scope هم‌زمان تغییر کرده است؛ اطلاعات را دوباره بارگذاری کنید.");
            scope.Deleted = true;
            scope.IsActive = false;
            scope.ModifyBy = _principal.Id;
            scope.ModificationDate = DateTime.UtcNow;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                return WalletScopeResultDto.Failure("Scope هم‌زمان تغییر کرده است؛ اطلاعات را دوباره بارگذاری کنید.");
            }
            await transaction.CommitAsync();
            return new WalletScopeResultDto { IsSuccess = true };
        }

        /// <summary>شکل Target و وجود آن در داده‌های همین Application را اعتبارسنجی می‌کند.</summary>
        private async Task<string> ValidateAsync(WalletScopeEditDto request)
        {
            if (_principal == null || _principal.Id <= 0 || _application.ApplicationId <= 0)
                return "کاربر و Application معتبر لازم است.";
            if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Trim().Length > 300)
                return "عنوان Scope باید بین ۱ تا ۳۰۰ کاراکتر باشد.";
            var type = (WalletScopeTypeEnum)request.WalletScopeType;
            var validShape = type switch
            {
                WalletScopeTypeEnum.Company => request.CompanyId > 0 && request.RepresentationId == null
                    && request.CityId == null && request.ProductId == null,
                WalletScopeTypeEnum.Representation => request.RepresentationId > 0 && request.CompanyId == null
                    && request.CityId == null && request.ProductId == null,
                WalletScopeTypeEnum.City => request.CityId > 0 && request.CompanyId == null
                    && request.RepresentationId == null && request.ProductId == null,
                WalletScopeTypeEnum.Product => request.ProductId > 0 && request.CompanyId == null
                    && request.RepresentationId == null && request.CityId == null,
                _ => false
            };
            if (!validShape) return "نوع Scope و شناسه هدف با هم سازگار نیستند.";
            var applicationId = _application.ApplicationId;
            var exists = type switch
            {
                WalletScopeTypeEnum.Company => await _context.Company.AnyAsync(x =>
                    x.Id == request.CompanyId && x.ApplicationId == applicationId && !x.Deleted),
                WalletScopeTypeEnum.Representation => await _context.Representation.AnyAsync(x =>
                    x.Id == request.RepresentationId && x.ApplicationId == applicationId && !x.Deleted),
                WalletScopeTypeEnum.City => await _context.City.AnyAsync(x =>
                    x.Id == request.CityId && !x.Deleted),
                WalletScopeTypeEnum.Product => await _context.Product.AnyAsync(x =>
                    x.Id == request.ProductId && x.ApplicationId == applicationId && !x.Deleted),
                _ => false
            };
            return exists ? null : "هدف Scope یافت نشد.";
        }

        /// <summary>Program Draft را به صورت شرطی قفل می‌کند تا Activation هم‌زمان Scope را دور نزند.</summary>
        private async Task<bool> LockDraftProgramAsync(int programId)
            => _principal != null && _principal.Id > 0 && _application.ApplicationId > 0
                && await _context.WalletProgram.Where(x => x.Id == programId
                    && x.ApplicationId == _application.ApplicationId && !x.Deleted
                    && x.WalletProgramStatusType == (int)WalletProgramStatusEnum.Draft)
                    .ExecuteUpdateAsync(x => x
                        .SetProperty(p => p.ModifyBy, _principal.Id)
                        .SetProperty(p => p.ModificationDate, DateTime.UtcNow)) == 1;

        /// <summary>Scope غیرحذف‌شده را فقط در Application جاری برای تغییر بارگذاری می‌کند.</summary>
        private Task<WalletScope> FindAsync(long id)
            => _context.WalletScope.FirstOrDefaultAsync(x => x.Id == id
                && x.ApplicationId == _application.ApplicationId && !x.Deleted
                && x.WalletProgram.ApplicationId == _application.ApplicationId && !x.WalletProgram.Deleted);

        /// <summary>پروژکشن Scopeهای Application را برای خواندن تک رکورد فراهم می‌کند.</summary>
        private IQueryable<WalletScopeDto> GetAllForApplication()
            => _context.WalletScope.AsNoTracking()
                .Where(x => x.ApplicationId == _application.ApplicationId && !x.Deleted
                    && x.WalletProgram.ApplicationId == _application.ApplicationId && !x.WalletProgram.Deleted)
                .Select(x => new WalletScopeDto
                {
                    Id = x.Id, WalletProgramId = x.WalletProgramId, WalletScopeType = x.WalletScopeType,
                    CompanyId = x.CompanyId, RepresentationId = x.RepresentationId,
                    CityId = x.CityId, ProductId = x.ProductId, Title = x.Title,
                    Description = x.Description, IsActive = x.IsActive, RowVersion = x.RowVersion
                });
    }
}
