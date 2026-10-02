using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using Yashil.Infrastructure.Caching;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Core.Enums;
using Yashil.Core.CustomViewModels;
using YashilBaseInfo.Core.Services;
using Microsoft.EntityFrameworkCore;
using YashilCommerce.Core.Classes;
using YashilCommerce.Web.DataFactory;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class CompanyController : YashilMvcBaseController<Company>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
        private readonly IAppDocumentService _appDocumentService;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICityService _cityService;
        private readonly IPublicDataFactory _publicDataFactory;

        public CompanyController(IMapper mapper,
            IStaticCacheManager staticCacheManager,
            IYashilLogger<Company> logger, IPermissionService permissionService, IAppDocumentService appDocumentService, ICompanyService companyService, ICityService cityService, StoreConfiguration storeConfiguration, ApplicationConfiguration applicationConfiguration, IPublicDataFactory publicDataFactory) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _appDocumentService = appDocumentService;
            _companyService = companyService;
            _mapper = mapper;
            _staticCacheManager = staticCacheManager;
            _cityService = cityService;
            _storeConfiguration = storeConfiguration;
            _publicDataFactory = publicDataFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTitle">متن جستجو</param>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <param name="cityId">شهر</param>
        /// <param name="representationId">نمایندگی</param>
        /// <param name="company">مراکز</param>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(
            string searchTitle,
            int? categoryId,
            int? cityId,
            int? representationId,
            int? categoryGroupId,
            List<int> company)
        {
            int? companyId = null;
            if (company != null && company.Count > 0)
            {
                companyId = company.FirstOrDefault();
            }
            // در حالتی که شناسه دسته بندی ندارد یعنی 
            // Null
            // می باشد لیست دسته بندی ها را بر اساس مرکز و نمایندگی و شهر دریافت می کنیم
            var levelOneUsedCategoryList = new List<CategorySimpleViewModel>();
            if (!categoryId.HasValue)
            {
                levelOneUsedCategoryList = await _publicDataFactory.GetUsedCategoriesFactoryAsync(
                        null,
                        searchTitle,
                        companyId,
                        representationId,
                        cityId,
                        null,
                        ApplicationConfiguration.ApplicationId,
                        true,
                        categoryGroupId);

            }

            bool companyRepresentationsHasProductSalesPlanning = false;
            if (companyId.HasValue)
            {
                companyRepresentationsHasProductSalesPlanning = await _companyService
                    .CheckCompanyRepresentationsHasProductSalesPlanning(companyId.Value);

            }
            var indexPageViewModel = new IndexPageViewModel
            {
                FirstLevelCategories = levelOneUsedCategoryList,
                AllowFilterByCity = _storeConfiguration.AllowFilterByCity && companyRepresentationsHasProductSalesPlanning
            };

            if (categoryId.HasValue)
            {
                indexPageViewModel.ProductFilter.SelectedCategoryWithParents =
                    await _publicDataFactory.GetCategoryWithParentsAsync(categoryId.Value);
            }

            indexPageViewModel.ProductFilter.CategoryGroupId = categoryGroupId;
            indexPageViewModel.ProductFilter.CategoryId = categoryId;
            indexPageViewModel.ProductFilter.Company = company;
            indexPageViewModel.ProductFilter.CityId = cityId;
            indexPageViewModel.ProductFilter.RepresentationId = representationId;
            indexPageViewModel.ProductListViewType = ProductListViewType.horizentalCard;
            indexPageViewModel.ProductFilter.SearchTitle = searchTitle;
            if (companyId != null)
                indexPageViewModel.BannerUrl = await GetCompanyBannerUrl(companyId.Value);

            return View(indexPageViewModel);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyLargeLogo(int cId)
        {
            return await GetFile(DocumentTypeEnum.CompanyDesktopLogo, cId);
        }

        /// <summary>
        /// دریافت
        /// url
        /// بنر
        /// AppDocument
        /// و کش کردن
        /// بنر دسکتاپ
        /// </summary>
        /// <param name="companyId">CompanyId</param>
        /// <returns></returns>
        private async Task<string> GetCompanyBannerUrl(int companyId)
        {
            var cacheKey = GetCompanyBannerCacheKey(companyId,
                DocumentTypeEnum.CompanyDesktopLogo);
            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var appDoc = await _appDocumentService.GetAppDocumentAsync(
                    companyId,
                    DocumentTypeEnum.CompanyDesktopLogo);
                if (appDoc != null)
                {
                    return appDoc.Description;
                }
                return string.Empty;
            });
        }

        /// <summary>
        /// کش کردن بنر کمپانی
        /// </summary>
        /// <param name="companyId">companyId</param>
        /// <param name="docType">docType</param>
        /// <returns></returns>
        private CacheKey GetCompanyBannerCacheKey(int companyId, DocumentTypeEnum docType)
        {
            return _staticCacheManager.
                PrepareKeyForDefaultCache(YashilModelCacheDefaults<AppDocument>.ByIdCacheKey,
                    $"{(int)docType}_{companyId}");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanySmallLogo(int cId)
        {
            return await GetFile(DocumentTypeEnum.CompanyMobileLogo, cId);
        }
        private async Task<IActionResult> GetFile(DocumentTypeEnum documentType, int cId)
        {
            var cacheKey = GetCompanyBannerCacheKey(cId, documentType);
            cacheKey.CacheTime = (int)CacheTime.TwentyFourHour;
            return await _staticCacheManager.GetAsync<IActionResult>(cacheKey, async () =>
            {
                var appDoc = _appDocumentService.GetFile(cId, (int)documentType);
                if (appDoc?.DocumentFile != null)
                {
                    return File(appDoc.DocumentFile, appDoc.ContentType);
                }

                return NotFound();
            });

        }
        /// <summary>
        /// لیست مراکز های مربوط به هر شهر
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<ActionResult> CityCompanies(int? cityId)
        {
            var companies = _companyService.GetCompaniesByCityId(cityId);
            var company = await companies.ProjectTo<CompanyViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return View("_CityCompany", company);
        }
        /// <summary>
        /// لیست شهرهایی که مرکز موردنظر، با توجه به شناسه مرکز درآن شهر فعال است
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetCityByCompanyId(int? companyId)
        {
            var provinces = _companyService.GetCityByCompanyId(companyId);
            var cityTitleOnlyViewModels = await provinces.ProjectTo<CityTitleOnlyViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return View("_Cities", cityTitleOnlyViewModels);
        }
        /// <summary>
        /// لیست تمامی شهرها
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetAllCities()
        {
            var provinces = _cityService.GetCities();
            var cityTitleOnlyViewModels = await provinces.ProjectTo<CityTitleOnlyViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return View("_Cities", cityTitleOnlyViewModels);
        }


    }
}
