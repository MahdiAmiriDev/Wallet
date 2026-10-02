using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Yashil.Common.Core.Caching;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using System.Linq;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Common.Core.Classes;
using System;
using Microsoft.Extensions.Logging;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class CompanyController : BaseController<Company, int, CompanyListViewModel,
        CompanyEditModel, CompanySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IYashilLogger<Company> _logger;
        private readonly ICompanyService _companyService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IFactorService _factorService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;


        public CompanyController(ICompanyService companyService, IMapper mapper, IYashilLogger<Company> logger,
            IPermissionService permissionService,
            IStaticCacheManager staticCacheManager, IFactorService factorService, IAdditionalUserPropService additionalUserPropService, IUserPrincipal userPrincipal) : base(companyService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _logger = logger;
            _companyService = companyService;
            _staticCacheManager = staticCacheManager;
            _factorService = factorService;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
        }

        [HttpGet("GetCities")]
        [AllowAnonymous]
        public async Task<List<CityTitleOnlyViewModel>> GetCities(int companyId)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<City>.GetCityByCompanyIdCacheKey, companyId);
            cacheKey.CacheTime = (int)CacheTime.TwentyFourHour;
            var cities = await _staticCacheManager.GetAsync(cacheKey, () =>
            {
                var cityByCompany = _companyService.GetCityByCompanyId(companyId);
                return cityByCompany.ProjectTo<CityTitleOnlyViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            });
            return cities;
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(CompanyEditModel editModel)
        {
            //بررسی میکنیم که کد ارسال شده از سمت کاربر با کد ذخیره شده از قبل یکسال هست یا خیر 
            // اگر یکسان نباشد باید بررسی کنیم که فاکتوری صادر نشده باشد
            // اگر فاکتور صادر شده باشد باید پیام خطا بدهیم
            // این کد در گزارش های کانورت کاربر دارد
            var company = _companyService.Get(editModel.Id, true);
            if (company.Code != editModel.Code || company.Title != editModel.Title)
            {
                var hasAnyFactor = _factorService.CheckExistenceFactorForCompanyAndRepresentation(company.Id, null);
                if (hasAnyFactor)
                {
                    return "کاربر گرامی باتوجه به اینکه برای این مرکز فاکتور صادر شده است امکان تغییر کد و نام آن وجود ندارد";
                }
            }

            if (editModel.ParentId.HasValue)
            {
                var checkCompanyParent = await CheckCompanyParentAsync(editModel.ParentId.Value);
                if (!checkCompanyParent.isValid)
                {
                    return checkCompanyParent.message;
                }
            }

            var additionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            await _staticCacheManager.RemoveAsync(YashilModelCacheDefaults<Company>.ActiveCompaniesIfHasPictureKey,
                additionalUserProp.CustomerClubId);

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(CompanyEditModel editModel)
        {
            if (editModel.ParentId.HasValue)
            {
                var checkCompanyParent = await CheckCompanyParentAsync(editModel.ParentId.Value);
                if (!checkCompanyParent.isValid)
                {
                    return checkCompanyParent.message;
                }
            }
            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        [HttpGet("GetForSelectHasVoucherType")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetForSelectHaVoucherType(CustomDataSourceLoadOptions loadOptions)
        {
            var companies = _companyService.GetCompaniesHaveVoucherType();
            return await DataSourceLoader.LoadAsync(
                companies.ProjectTo<CompanySimpleViewModel>(this._mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// Ù„ÛŒØ³Øª ØªÙ…Ø§Ù…ÛŒ Ù…Ø±Ø§Ú©Ø² Ø¯Ø± ØªÙ…Ø§Ù…ÛŒ Ø¨Ø±Ù†Ø§Ù…Ù‡ Ù‡Ø§ÛŒ Ø±Ø§ Ø¨Ø± Ù…ÛŒ Ú¯Ø±Ø¯Ø§Ù†Ø¯
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActiveCompaniesForAllApps")]
        [AllowAnonymous]
        public async Task<List<CompanyCustomSimpleViewModel>> GetActiveCompaniesForAllApps(int? applicationGroupId)
        {
            var companies = _companyService.GetAllActiveCompanies(true, applicationGroupId);
            return await companies.ProjectTo<CompanyCustomSimpleViewModel>(_mapper.ConfigurationProvider).OrderBy(x => x.ShortName).ToListAsync();
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            await _companyService.DeleteCompanyMainCategoryByCompanyIdAsync(id);
            var result = await _companyService.IsAllowedDeleteCompanyAsync(id);
            if (!result.isAllowed)
            {
                return result.message;
            }
            return await base.CustomValidateBeforeDeleteAsync(id);
        }
        protected override async Task<(bool isSuccess, string message)> AfterInsertAsync(CompanyEditModel editModel, Company entity)
        {
            if (entity.Id > 0)
            {
                await _companyService.AddCategoriesForCompanyAsync(entity.Id);
            }

            await _companyService.UpdateCodePathAsync();
            return (true, string.Empty);
        }

        protected override async Task AfterUpdateAsync(CompanyEditModel editModel, Company entity)
        {
            await base.AfterUpdateAsync(editModel, entity);
            await _companyService.UpdateCodePathAsync();
        }

        protected override async Task AfterDeleteAsync(int id)
        {
            await base.AfterDeleteAsync(id);
            await _companyService.UpdateCodePathAsync();

        }

        /// <summary>
        /// دریافت canUserSeeOtherCompany 
        /// مربوط به یک مرکز بر اساس شناسه مرکز
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        [HttpGet("GetCanUserSeeOtherCompany")]
        [YashilAuthorization(ActionName = "GetCanUserSeeOtherCompany")]
        public async Task<bool> GetCanUserSeeOtherCompanyAsync(int? companyId)
        {
            return await _companyService.FindById(companyId.Value)
                .Select(x => x.CanUserSeeOtherCompany).SingleOrDefaultAsync();

        }

        /// <summary>
        /// بروزرسانی فیلد CanUserSeeOtherCompany 
        /// مربوط به یک مرکز
        /// </summary>
        /// <param name="canUserSeeOtherCompanyEditModel"></param>
        /// <returns></returns>
        [HttpPost("UpdateCanUserSeeOtherCompany")]
        [YashilAuthorization(ActionName = "UpdateCanUserSeeOtherCompany")]
        public async Task<ResultStatus<int>> UpdateCanUserSeeOtherCompanyAsync(UpdateCanUserSeeOtherCompanyEditModel canUserSeeOtherCompanyEditModel)
        {
            if (canUserSeeOtherCompanyEditModel.Id == 0 || canUserSeeOtherCompanyEditModel.Id == null)
            {
                return new ResultStatus<int> { IsSuccess = false, Message = "کاربر گرامی شناسه مرکز نامعبر است" };
            }
            try
            {
                var company = await _companyService.UpdateCanUserSeeOtherCompanyAsync(canUserSeeOtherCompanyEditModel.Id.Value, canUserSeeOtherCompanyEditModel.CanUserSeeOtherCompany);
                if (company != null)
                {
                    return new ResultStatus<int> { IsSuccess = true, Message = "عملیات  بروزرسانی با موفقیت انجام شد" };
                }
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(Company), "بروز خطا در عملیات بروزرسانی CanUserSeeOtherCompany", e);
                return new ResultStatus<int> { IsSuccess = false, Message = "بروز خطا در عملیات بروزرسانی" };
            }

            return new ResultStatus<int> { IsSuccess = false, Message = "بروز خطا در عملیات بروزرسانی" };
        }

        /// <summary>
        /// دریافت مراکز به همراه فرزندان آن با امکان نمایش سایر مراکز .
        /// ابتدا مرکز کاربر جاری را پیدا می کنیم
        /// سپس در صورتی مرکز کاربر جاری Null
        /// بود مشاهده سایر مراکز برای مرکز کاربر جاری True
        /// بود تمامی مراکز بر گردانده می شود
        /// در غیر اینصورت فرزندان مرکز کاربر جاری به همراه جد آن بر می گردد 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentUserCompanyWithChildrenForSelectWithPossibleSeeOtherCompanies")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public Task<List<CompanyWithChildrenForSelectViewModel>>
            GetCurrentUserCompanyWithChildrenForSelectWithPossibleSeeOtherCompanies()
        {
            return GetCurrentUserCompanyWithChildrenForSelect(true);
        }
        [HttpGet("GetCurrentUserCompanyWithChildrenForSelect")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public Task<List<CompanyWithChildrenForSelectViewModel>>
            GetCurrentUserCompanyWithChildrenForSelect()
        {
            return GetCurrentUserCompanyWithChildrenForSelect(false);
        }
        private async Task<List<CompanyWithChildrenForSelectViewModel>>
            GetCurrentUserCompanyWithChildrenForSelect(bool possibleSeeOtherCompanies
                )
        {
            (IQueryable<Company> companies, string currentUserCompanyCodePath) =
                await _companyService.GetCurrentUserCompanyWithChildrenAsync(possibleSeeOtherCompanies);
            var companyCodePathData = new Dictionary<string, object> { { "currentUserCompanyCodePath", currentUserCompanyCodePath } };
            return await companies.ProjectTo<CompanyWithChildrenForSelectViewModel>
                (_mapper.ConfigurationProvider,
                    companyCodePathData)
                .ToListAsync();
        }
        /// <summary>
        /// دریافت مراکز به همراه زیر مجموعه های خود برای نمایش در لیست  مراکز
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentUserCompanyWithChildrenForList")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetCurrentUserCompanyWithChildrenForList(CustomDataSourceLoadOptions loadOptions)
        {
            (IQueryable<Company> companies, string currentUserCompanyCodePath) =
                await _companyService.GetCurrentUserCompanyWithChildrenAsync(false);
            var companyCodePathData = new Dictionary<string, object> { { "currentUserCompanyCodePath", currentUserCompanyCodePath } };
            return await DataSourceLoader.LoadAsync(
                companies.ProjectTo<CompanyWithChildrenForListViewModel>
                (_mapper.ConfigurationProvider,
                    companyCodePathData)
                , loadOptions);

        }
        /// <summary>
        /// بررسی می کنیم
        /// Parent
        /// امتخابی مرکز معتبر باشد یعنی
        /// comapny Parent CodePath
        /// ارسالی جز
        /// codePath
        /// های مرکز کاربر جاری با زیر مجموعه های آن
        /// باشد
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private async Task<(bool isValid, string message)> CheckCompanyParentAsync(int parentId)
        {
            var currentUserCompanyInfo = await _companyService.GetCurrentUserCompanyInfo();
            var companyParentIsValid = await _companyService.CheckCompanyParentAsync(parentId, currentUserCompanyInfo.CodePath);
            return (companyParentIsValid, companyParentIsValid ? string.Empty : "Ú©Ø§Ø±Ø¨Ø± Ú¯Ø±Ø§Ù…ÛŒ Ù…Ø±Ú©Ø² Ø§Ù†ØªØ®Ø§Ø¨ÛŒ Ù…Ø¹ØªØ¨Ø± Ù†Ù…ÛŒ Ø¨Ø§Ø´Ø¯.");
        }

        /// <summary>
        /// این متد جهت دریافت اطلاعات مربوط به یک مرکز کاربرد دارد
        /// اطلاعات دریافتی شامل
        /// آیا مرکز تصویر حالت موبایل و دسکتاب دارد یا نه
        /// آیا تیک گزینه نمایش در صفحه ی اصلی آن خورده است یا نه
        /// آیا برنامه ریزی دوره فعال دارد یا نه
        /// آیا تنطیم فروش فعال دارد یا نه
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("GetCompanyInformation")]
        [YashilAuthorization]
        public async Task<CompanyInformationViewModel> GetCompanyInformationAsync(int companyId)
        {
            var additionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var companyInformation = await _companyService.GetCompanyInformation(companyId, additionalUserProp.CustomerClubId);


            CompanyInformationViewModel companyInformationViewModel = new()
            {
                HasDesktopAndMobileModeImage = companyInformation.HasDesktopAndMobileModeImage,
                HasActiveContract = companyInformation.HasActiveContract,
                ShowInMainPage = companyInformation.ShowInMainPage,
                HasActiveProductSalesPlanning = companyInformation.HasActiveProductSalesPlanning
            };

            return companyInformationViewModel;
        }
    }
}