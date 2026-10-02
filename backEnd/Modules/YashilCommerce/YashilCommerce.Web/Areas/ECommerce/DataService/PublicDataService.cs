using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.CustomViewModels.Cms;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.ServiceImpl;
using YashilCommerce.Web.Areas.ECommerce.CachedData;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilDms.Core.Services;
using YashilNews.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.DataService
{
    public class PublicDataService
    {
        private readonly IWebHelper _webHelper;




        private readonly ICategoryService _categoryService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IYashilCmsService _cmsService;
        private readonly IAppConfigService _appConfigService;
        private readonly ICompanyService _companyService;
        private readonly ICommonBaseDataService _commonBaseDataService;
        private readonly ISubsidyService _subsidyService;

        public PublicDataService(

            IWebHelper webHelper,
            ICategoryService categoryService,
            ApplicationConfiguration applicationConfiguration,
            IStaticCacheManager staticCacheManager,
            IMapper mapper,
            IProductSalesPlanningService productSalesPlanningService,
            IAppDocumentService appDocumentService,
            IAdditionalUserPropService additionalUserPropService,
            IUserPrincipal userPrincipal,
            IYashilCmsService yashilCmsService, IAppConfigService appConfigService, ICompanyService companyService,
            IShoppingCartItemService shoppingCartItemService, ICommonBaseDataService commonBaseDataService, ISubsidyService subsidyService)
        {
            _webHelper = webHelper;
            _categoryService = categoryService;
            _applicationConfiguration = applicationConfiguration;
            _staticCacheManager = staticCacheManager;
            _mapper = mapper;
            _productSalesPlanningService = productSalesPlanningService;
            _appDocumentService = appDocumentService;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
            _cmsService = yashilCmsService;
            _appConfigService = appConfigService;
            _companyService = companyService;
            _commonBaseDataService = commonBaseDataService;
            _subsidyService = subsidyService;
        }

        /// <summary>
        /// پر فروش ترین دوره ها را بر می گرداند به مدت 6 ساعت کش می کند
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyProductSalesPlanningViewModel>> GetBestSellers()
        {
            var additionalUserProp = await
                _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var cacheKey = _staticCacheManager
                .PrepareKeyForDefaultCache(YashilModelCacheDefaults<ProductSalesPlanning>.BestSellersKey,
                    additionalUserProp.CustomerClubId);
            cacheKey.CacheTime = (int)CacheTime.SixHour;
            return await _staticCacheManager.GetAsync(cacheKey, () =>
            {
                var bestSellers =
                    _productSalesPlanningService.GetBestSellers(additionalUserProp.CustomerClubId);
                return _mapper.Map<List<CompanyProductSalesPlanningViewModel>>(bestSellers);
            });
        }


        /// <summary>
        /// محصولات با بیشترین تخفیف را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyProductSalesPlanningViewModel>> GetFullOfDiscountsProducts()
        {
            var additionalUserProp = await
                _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var cacheKey = _staticCacheManager
                .PrepareKeyForDefaultCache(YashilModelCacheDefaults<ProductSalesPlanning>.FullOfDiscountsKey,
                    additionalUserProp.CustomerClubId);
            cacheKey.CacheTime = (int)CacheTime.TwoHour;
            return await _staticCacheManager.GetAsync(cacheKey, () =>
            {
                var bestSellers =
                    _productSalesPlanningService.GetFullOfDiscounts(additionalUserProp.CustomerClubId);
                return _mapper.Map<List<CompanyProductSalesPlanningViewModel>>(bestSellers);
            });
        }


        public async Task<List<CategorySimpleViewModel>> GetUsedCategories(int? id = null)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var cacheKey = _staticCacheManager
                .PrepareKeyForDefaultCache(YashilModelCacheDefaults<Category>
                    .FirstLevelCategoriesSimpleViewModelKey, id, userAdditionalUserProp.CustomerClubId, "");
            cacheKey.CacheTime = (int)CacheTime.TwentyFourHour;


            return await _staticCacheManager.GetAsync(cacheKey, () =>
            {
                var levelOneUsedCategories =
                    _categoryService.GetRootUsedCategories(id, null, null, null,
                        _applicationConfiguration.ApplicationId, true, userAdditionalUserProp.CustomerClubId);

                return _mapper.Map<List<CategorySimpleViewModel>>(levelOneUsedCategories);
            });
        }

        public async Task<List<CompanyProductSalesPlanningViewModel>> GetNewest()
        {
            var additionalUserProp = await
                _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var cacheKey = _staticCacheManager
                .PrepareKeyForDefaultCache(YashilModelCacheDefaults<ProductSalesPlanning>
                    .ProductSalePlanningNewestKey, additionalUserProp.CustomerClubId);

            cacheKey.CacheTime = (int)CacheTime.TwoHour;

            return await _staticCacheManager.GetAsync(cacheKey, () =>
            {
                var newest =
                    _productSalesPlanningService.GetNewest(additionalUserProp.CustomerClubId);
                return _mapper.Map<List<CompanyProductSalesPlanningViewModel>>(newest);
            });
        }

        /// <summary>
        /// مراکز فعالی را که برای حالت موبایل و دسکتاپ دارای تصویر می باشند را برمیگرداند
        /// </summary>
        /// <returns>List<CompanyViewModel></returns>
        public async Task<List<CompanyViewModel>> GetActiveCompaniesIfHasPictureAsync()
        {

            var companiesWithPicture = await
                _companyService.GetActiveCompaniesIfHasPictureAsync();

            return companiesWithPicture.Select(c => new CompanyViewModel
            {
                Id = c.Id,
                ModificationDate = c.ModificationDate,
                ShortName = c.ShortName,
                Title = c.Title
            }).ToList();
        }

        /// <summary>
        /// دریافت تصاویر اسلایدر صفحه اصلی در صورتی که برای حالت موبایل و دسکتاپ تصویر داشته باشد
        /// </summary>
        /// <param name="levelOne"></param>
        /// <returns></returns>
        public async Task<List<YashilCmsCustomViewModel>> GetMainPageSliderLevelOneIfHasPictureAsync(bool levelOne = true)
        {
            var cacheKeyParam = levelOne
                ? YashilModelCacheDefaults<YashilCms>.MainPageSliderLevelOne
                : YashilModelCacheDefaults<YashilCms>.MainPageSliderLevelTwo;
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(cacheKeyParam);


            cacheKey.CacheTime = (int)CacheTime.OneHour;


            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var sliders = _cmsService.GetByPagePositionId(_applicationConfiguration.ApplicationId,
                    levelOne
                        ? (int)PagePositionTypeEnum.MainPageSliderLevelOne
                        : (int)PagePositionTypeEnum.MainPageSliderLevelTwo,false);

                return await sliders.Join(_appDocumentService.GetAllByApplicationId(true, _applicationConfiguration.ApplicationId),
                        cms => cms.Id,
                        appDoc => appDoc.ObjectId,
                        (cms, appDoc) => new { cms, appDoc })
                    .Where(joined => joined.appDoc.DocTypeId == (int)DocumentTypeEnum.CmsImage ||
                                     joined.appDoc.DocTypeId == (int)DocumentTypeEnum.SliderMobileImage)
                    .GroupBy(x => new
                    {
                        x.cms.Id,
                        x.cms.ModificationDate,
                        x.cms.SubTitle,
                        x.cms.Title,
                        x.cms.Url,
                        x.cms.DisplayOrder

                    })
                    .Where(group => group.Select(c => c.appDoc.DocTypeId).Count() == 2)
                    .Select(x => new YashilCmsCustomViewModel
                    {
                        Id = x.Key.Id,
                        ModificationDate = x.Key.ModificationDate.Value.ToString("yyyyMMddhhmmss"),
                        SubTitle = x.Key.SubTitle,
                        Title = x.Key.Title,
                        Url = x.Key.Url,
                        DisplayOrder = x.Key.DisplayOrder
                    }).OrderBy(x => x.DisplayOrder).ToListAsync();
            });
        }

        public Task<List<CompanyCustomSimpleViewModel>> GetActiveCompaniesForAllApps(int? applicationGroupId)
        {
            var cacheKey =
                _staticCacheManager.PrepareKeyForDefaultCache(
                    YashilModelCacheDefaults<Company>.GetActiveCompaniesForAllApps, applicationGroupId);
            return _staticCacheManager.GetAsync<List<CompanyCustomSimpleViewModel>>(cacheKey, () =>
            {
                var companies = _companyService.GetAllActiveCompanies(true, applicationGroupId);
                return companies.ProjectTo<CompanyCustomSimpleViewModel>(_mapper.ConfigurationProvider)
                    .OrderBy(x => x.ShortName).ToListAsync();
            });
        }

        public async Task<LogoSimpleViewModel> GetSiteLogoAsync()
        {
            var result = await _appConfigService.GetSiteLogoAsync();
            return new LogoSimpleViewModel()
            {
                Path = result.logoPath,
                Name = result.name
            };
        }

        /// <summary>
        /// پیدا کردن عنوانی ارسال شده در پارمتر بین رشته های تحصیلی درج شده در
        /// CommonBaseData
        /// بر اساس
        /// CommonBaseType = EducationalFieldType
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Task<List<EducationalFieldViewModel>> GetFindEducationalFieldAsync(string title)
        {
            var educationalFields = _commonBaseDataService
                .FindByTitle(CommonBaseTypeEnum.EducationalFieldType, title);
            return educationalFields.ProjectTo<EducationalFieldViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        /// <summary>
        /// این متد جهت دریافت اطلاعات مربوط به تصاویر تبلیغاتی تجاری سایت
        /// در صفحه ی اصلی بعد از دسته بندی ها کاربرد دارد
        /// </summary>
        /// <returns><List<YashilCmsCustomViewModel>></returns>
        public async Task<List<YashilCmsCustomViewModel>> GetHomePageCommercialImageInfoAsync()
        {
            var homePageFirstCommercialImageInfo = await _cmsService.GetByPagePositionId(
                    _applicationConfiguration.ApplicationId
                    , (int)PagePositionTypeEnum.FirstImageOfCommercialAdvertisingInHomePage)
                .ProjectTo<YashilCmsCustomViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            var homePageSecondCommercialImageInfo = await _cmsService.GetByPagePositionId(
                    _applicationConfiguration.ApplicationId
                    , (int)PagePositionTypeEnum.SecondImageOfCommercialAdvertisingInHomePage)
                .ProjectTo<YashilCmsCustomViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return new List<YashilCmsCustomViewModel>
            {
                homePageFirstCommercialImageInfo,
                homePageSecondCommercialImageInfo
            };
        }

        /// <summary>
        /// این متد جهت دریافت اطلاعات مربوط به تصاویر تبلیغات مهارت آموزی سایت
        /// در صفحه ی اصلی بعد از پرفروش ترین دوره ها کاربرد دارد
        /// </summary>
        /// <returns><List<YashilCmsCustomViewModel>></returns>
        public async Task<List<YashilCmsCustomViewModel>> GetHomePageLearningSkillImageInfoAsync()
        {

            var homePageFirstLearningSkillImageInfo = await _cmsService
                .GetByPagePositionId(_applicationConfiguration.ApplicationId
                    , (int)PagePositionTypeEnum.FirstImageOfLearningSkillInHomePage)
                .ProjectTo<YashilCmsCustomViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            var homePageSecondLearningSkillImageInfo = await _cmsService.GetByPagePositionId(
                    _applicationConfiguration.ApplicationId
                    , (int)PagePositionTypeEnum.SecondImageOfLearningSkillInHomePage)
                .ProjectTo<YashilCmsCustomViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            var homePageThirdLearningSkillImageInfo = await _cmsService.GetByPagePositionId(
                    _applicationConfiguration.ApplicationId
                    , (int)PagePositionTypeEnum.ThirdImageOfLearningSkillInHomePage)
                .ProjectTo<YashilCmsCustomViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            var homePageFourthLearningSkillImageInfo = await _cmsService.GetByPagePositionId(
                    _applicationConfiguration.ApplicationId
                    , (int)PagePositionTypeEnum.FourthImageOfLearningSkillInHomePage)
                .ProjectTo<YashilCmsCustomViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return new List<YashilCmsCustomViewModel>
            {
                homePageFirstLearningSkillImageInfo,
                homePageSecondLearningSkillImageInfo,
                homePageThirdLearningSkillImageInfo,
                homePageFourthLearningSkillImageInfo
            };
        }

        /// <summary>
        /// این متد جهت دریافت اطلاعات سند مربوط به تصاویر تبلیغاتی سایت کاربرد دارد
        /// برای دریافت اطلاعات تصویر تبلیغاتی که پس از دسته بندی ها در صفحه ی اصلی نمایش می دهیم
        /// و تصاویر مهارت آموزی که پس از پرفروش ترین دوره ها نمایش می دهیم
        /// </summary>
        /// <param name="yashilCmsCustomViewModel"></param>
        /// <returns></returns>
        public async Task<List<HomePageAdvertisingImageViewModel>> GetHomePageAdvertisingImagesDocumentInfoAsync(List<YashilCmsCustomViewModel> yashilCmsCustomViewModel)
        {
            if (!yashilCmsCustomViewModel.Any())
                return null;

            List<HomePageAdvertisingImageViewModel> homePageAdvertisingImages = new();

            foreach (var item in yashilCmsCustomViewModel)
            {
                if (item == null) continue;

                var t = await _appDocumentService.GetObjectDocumentsByApplicationId(nameof(YashilCms), item.Id,
                    (int)DocumentCategoryEnum.MainImage, _applicationConfiguration.ApplicationId).Select(AppDocument =>
                    AppDocument.Id).ToListAsync();

                homePageAdvertisingImages.Add(new HomePageAdvertisingImageViewModel()
                {
                    DeskTopModeAppDocumentId = await _appDocumentService.GetObjectDocumentsByApplicationId(nameof(YashilCms), item.Id,
                        (int)DocumentCategoryEnum.MainImage, _applicationConfiguration.ApplicationId).Select(AppDocument =>
                        AppDocument.Id).FirstOrDefaultAsync(),
                    MobileModeAppDocumentId = await _appDocumentService.GetObjectDocuments(nameof(YashilCms), item.Id, (int)DocumentCategoryEnum.MainImage,
                        (int)DocumentTypeEnum.SliderMobileImage, _applicationConfiguration.ApplicationId)
                        .Select(appDocument => appDocument.Id).FirstOrDefaultAsync(),
                    SubTitle = item.SubTitle,
                    Title = item.Title,
                    Url = item.Url
                });
            }

            if (!homePageAdvertisingImages.Any())
                return null;

            return homePageAdvertisingImages;
        }

        /// <summary>
        /// این متد جهت دریافت
        /// AppDocument
        /// مربوط به گروه دسته بندی می باشد
        /// </summary>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <returns></returns>
        public IQueryable<AppDocument> GetCategoryGroupImageAppDocument(int categoryGroupId)
        {
            return _appDocumentService.GetObjectDocumentsByApplicationId(nameof(CategoryGroup),
                categoryGroupId,
                (int)DocumentCategoryEnum.CategoryGroupImage,
                _applicationConfiguration.ApplicationId);
        }

        /// <summary>
        /// این سرویس جهت دریافت اطلاعات تصاویر مربوط به بالا سمت راست 
        /// صفحه ی اصلی سامانه کاربرد دارد
        /// </summary>
        /// <returns>List<YashilCmsCustomViewModel></returns>
        public async Task<List<YashilCmsCustomViewModel>> GetHomePageTopRightAdvertisingImageAsync()
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<YashilCms>.HomePageTopRightAdvertisingImageInfoKey);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                return await _cmsService.GetByPagePositionId(_applicationConfiguration.ApplicationId
                 , (int)PagePositionTypeEnum.HomePageTopRightSliderAdvertisingImage)
                .OrderBy(cms => cms.DisplayOrder)
                .ProjectTo<YashilCmsCustomViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            });
        }
        /// <summary>
        /// برسی می کند که آیا در وبسایت عمومی هستیم یا نه 
        /// اگر صفحه عمومی باشد ترو اگر تی بی ای او باشد فالس بر می گرداند
        /// </summary>
        /// <returns></returns>
        public bool IsPublicPage()
        {
            var domainName = _webHelper.GetDomainName();

            if (domainName.Contains("tbao"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// دریافت اطلاعات مربوط به یارانه اشتغال
        /// دسته بندی یارانه اشتغال را دریافت می کنیم
        /// و اطلاعات یارانه فعال این دسته بندی را از سرویس مروبط به یارانه دریافت می کنیم
        /// </summary>
        /// <returns></returns>
        public async Task<EmploymentSubsidyInformationViewModel> GetEmploymentSubsidyInformationAsync()
        {
            var employmentCategoryGroupId = await _appConfigService.GetValueByApplicationIdAsync(
                ApplicationConfigKey.YashilCommerceCategoryGroupEmploymentCategoryGroupId,
            _applicationConfiguration.ApplicationId);
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Subsidy>.GetCategoryGroupActiveSubsidyInfoCacheKey,
                employmentCategoryGroupId);

            cacheKey.CacheTime = (int)CacheTime.Default;
            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var employmentSubsidy = await _subsidyService.GetCategoryGroupActiveSubsidyInfoAsync(employmentCategoryGroupId);
                return _mapper.Map<EmploymentSubsidyInformationViewModel>(employmentSubsidy);
            });

        }
    }
}
