using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Caching;
using Yashil.Core.CustomDtos.Representation;
using Yashil.Core.CustomViewModels.Cms;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Core.Web;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.CachedData;
using YashilCommerce.Web.Areas.ECommerce.DataService;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilDms.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PublicApiController : ControllerBase
    {
        // GET: /<controller>/
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAppDocumentService _appDocumentService;
        private readonly PublicDataService _publicDataService;
        private readonly CacheDataService _cacheDataService;
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly IAppConfigService _appConfigService;
        private readonly IRepresentationService _representationService;
        private readonly IMapper _mapper;

        public PublicApiController(
            IStaticCacheManager staticCacheManager,
            IAppDocumentService appDocumentService,
            CacheDataService cacheDataService,
            PublicDataService publicDataService,
            IRepresentationService representationService,
            IShoppingCartItemService shoppingCartItemService,
            IAppConfigService appConfigService,
            IMapper mapper)
        {
            _staticCacheManager = staticCacheManager;
            _appDocumentService = appDocumentService;
            _publicDataService = publicDataService;
            _cacheDataService = cacheDataService;
            _shoppingCartItemService = shoppingCartItemService;
            _appConfigService = appConfigService;
            _representationService = representationService;
            _mapper = mapper;
        }

        /// <summary>
        /// لیست دسته بندی محصولات که در حال حاظر محصول فعال دارند
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUsedCategories")]
        public async Task<List<CategorySimpleViewModel>> GetUsedCategories(int? id = null)
        {
            return await _publicDataService.GetUsedCategories(id);
        }


        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600,
            Location = ResponseCacheLocation.Client,
            VaryByQueryKeys = new[] { "categoryId", "modificationDate" })]
        [HttpGet("GetCategoryIcon")]
        public async Task<ActionResult> GetCategoryIconAsync(int? categoryId)
        {
            if (categoryId == null || categoryId.Value == 0)
            {
                return NotFound("داده های ورودی نامعتبر است");
            }

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<Category>.ImageByIdCacheKey, categoryId);
            return await _staticCacheManager.GetAsync<ActionResult>(cacheKey, () =>
            {
                var appDoc = _appDocumentService.GetFile(categoryId.Value, (int)DocumentTypeEnum.CategoryIcon);
                if (appDoc?.DocumentFile != null)
                {
                    return File(appDoc.DocumentFile, appDoc.ContentType);
                }
                return File("/images/default-category.png", "image/x-png");
            });


        }


        /// <summary>
        /// دریافت ایکون کمپانی
        /// </summary>
        /// <param name="cId">objectId</param>
        /// <returns></returns>
        [HttpGet("GetCompanyIcon")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600, VaryByQueryKeys = new[] { "cId", "modificationDate" })]
        public async Task<IActionResult> GetCompanyIcon(int cId)
        {
            return await GetFile((int)DocumentTypeEnum.CompanyIcon, cId);
        }
        /// <summary>
        /// تصویر بزرگ دسته بندی جهت نمایش در منو  
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600,
            Location = ResponseCacheLocation.Client,
            VaryByQueryKeys = new[] { "categoryId", "modificationDate" })]
        [HttpGet("GetCategoryMenuIcon")]
        public Task<IActionResult> GetCategoryMenuIconAsync(int? categoryId)
        {
            if (categoryId == null || categoryId.Value == 0)
            {
                return null;
            }
            return GetFile((int)DocumentTypeEnum.CategoryMenuIcon, categoryId.Value);
        }

        /// <summary>
        /// پروفروش ترین محصولات
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBestSellers")]
        public async Task<List<CompanyProductSalesPlanningViewModel>> GetBestSellers()
        {
            return await _publicDataService.GetBestSellers();
        }

        /// <summary>
        /// جدید ترین محصولات 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNewest")]
        public async Task<List<CompanyProductSalesPlanningViewModel>> GetNewest()
        {
            return await _publicDataService.GetNewest();
        }

        /// <summary>
        /// لیست مراکز فعال که محصول برای فروش دارند
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActiveCompaniesByFilter")]
        public Task<List<CompanyViewModel>> GetActiveCompaniesByFilterAsync()
        {
            return _cacheDataService.GetActiveCompaniesByFilterAsync(null, null, null, null);
        }
        /// <summary>
        /// دریافت اطلاعات اسلایدر سطح یک
        /// فقط آن های را می دهد که دارای تصویر برای هر دور حالت
        /// موبایل و دسکتاپ باشند
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMainPageSlider")]
        public async Task<List<YashilCmsCustomViewModel>> GetMainPageSliderLevelOneAsync(bool levelOne = true)
        {
            return await _publicDataService.GetMainPageSliderLevelOneIfHasPictureAsync(levelOne);
        }

        /// <summary>
        /// لیست تمامی مراکز در تمامی برنامه های را بر می گرداند
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActiveCompaniesForAllApps")]
        [AllowAnonymous]
        public Task<List<CompanyCustomSimpleViewModel>> GetActiveCompaniesForAllApps(int? applicationGroupId)
        {
            return _publicDataService.GetActiveCompaniesForAllApps(applicationGroupId);
        }

        /// <summary>
        /// دریافت تصاویر اسلایدر صفحه ی اصلی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSliderImage")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new[] { "id", "modificationTime" })]
        public async Task<IActionResult> GetSliderImageAsync(int? id)
        {
            if (!id.HasValue) return NotFound();
            var appDoc = _appDocumentService.GetFile(id.Value, (int)DocumentTypeEnum.CmsImage);
            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }
            return NotFound();
        }

        /// <summary>
        /// دریافت تصاویر حالت موبایل اسلایدر صفحه ی اصلی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetSliderMobileImage")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new[] { "id", "modificationTime" })]
        public async Task<IActionResult> GetSliderMobileImageAsync(int? id)
        {
            if (!id.HasValue) return NotFound();
            var appDoc = _appDocumentService.GetFile(id.Value, (int)DocumentTypeEnum.SliderMobileImage);
            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }
            return NotFound();
        }

        [HttpGet("GetSiteLogo")]
        public async Task<LogoSimpleViewModel> GetSiteLogoAsync()
        {
            return await _publicDataService.GetSiteLogoAsync();
        }

        /// <summary>
        /// دریافت اسلایدر دسته بندی مربوط به باشگاه مشتریان
        /// 
        /// </summary>
        /// <param name="id">آیدی categoryCustomerClub</param>
        /// <returns></returns>
        [HttpGet("GetCategoryCustomerClubSlider")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new[] { "id", "modificationTime" })]
        public async Task<IActionResult> GetCategoryCustomerClubSlider(int id)
        {
            var docTypeId = (int)DocumentTypeEnum.CategorySlider;
            var appDoc = _appDocumentService.GetFile(id, docTypeId);
            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }
            return NotFound("فایل موزد نظر یافت نشد");
        }

        /// <summary>
        /// دریافت تصویر اسلایدر دسته بندی مربوط به باشگاه مشتریان در حالت موبایل     
        /// </summary>
        /// <param name="id">شناسه categoryCustomerClub</param>
        /// <returns></returns>
        [HttpGet("GetCategoryCustomerCustomerClubMobileSliderImage")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new[] { "id", "modificationTime" })]
        public IActionResult GetCategoryCustomerCustomerClubMobileSliderImage(int id)
        {
            var appDoc = _appDocumentService.GetFile(id, (int)DocumentTypeEnum.CategorySliderMobileMode);
            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }
            return NotFound("فایل موزد نظر یافت نشد");
        }

        /// <summary>
        /// تعداد آیتم های سبد خرید کاربر را بر اساس کد کوکی بر می گرداند
        /// </summary>
        /// <param name="cartType"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetShoppingCartItemsCount")]
        public int GetShoppingCartItemsCount(int cartType = (int)ShoppingCartTypeEnum.ShoppingCart)
        {
            string code = HttpContext.Request.Cookies[CookiKey.CartId];

            if (!string.IsNullOrWhiteSpace(code))
            {
                if (Guid.TryParse(code, out var guidCode))
                {
                    return _shoppingCartItemService.GetShoppingCartItemsCount(cartType, guidCode);
                }
            }
            return 0;
        }

        /// <summary>
        /// این متد جهت دریافت عکس محصول فراخوانی می شود
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        [HttpGet("GetProductSalesPlanningImage")]
        public async Task<ActionResult> GetProductSalesPlanningImageAsync(int? productSalesPlanningId)
        {
            if (productSalesPlanningId == null)
            {
                return BadRequest(); 
            }

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<ProductSalesPlanning>.ImageByIdCacheKey, productSalesPlanningId);
            return await _staticCacheManager.GetAsync<ActionResult>(cacheKey, async () =>
            {
                var appDoc = await _appDocumentService.GetFileAsync(productSalesPlanningId.Value, (int)DocumentTypeEnum.ProductSalesPlanningImage);
                if (appDoc?.DocumentFile != null)
                {
                    return File(appDoc.DocumentFile, appDoc.ContentType);
                }

                return File("/images/courses/EducatrionCourse.jpg", "image/jpg");
            });


        }

        private async Task<IActionResult> GetFile(int docTypeId, int objectId)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<AppDocument>.ByIdCacheKey, $"{docTypeId}_{objectId}");
            cacheKey.CacheTime = (int)CacheTime.TwentyFourHour;

            return await _staticCacheManager.GetAsync<IActionResult>(cacheKey, () =>
            {
                var appDoc = _appDocumentService.GetFile(objectId, docTypeId);
                if (appDoc?.DocumentFile != null)
                {
                    return File(appDoc.DocumentFile, appDoc.ContentType);
                }

                return NotFound();
            });

        }

        /// <summary>
        ///  23 23 23 
        /// </summary>
        /// <param name="company"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet("GetCategoryHierarchically")]
        public Task<List<CategoryHierarchicalViewModel>> GetCategoryHierarchically(List<int>? company, int? cityId)
        {

            var categoryHierarchicalViewModelList = _cacheDataService
                .GetCategoryHierarchicallyModel(company, null, null, cityId);

            return categoryHierarchicalViewModelList;
        }

        /// <summary>
        /// منوی اصلی سایت را بر می گرداند
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpGet("GetMenu")]
        public Task<List<CategoryHierarchicalViewModel>> GetMenu()
        {
            return _cacheDataService.GetCategoryHierarchicallyModel();
        }

        [AllowAnonymous]
        [HttpGet("GetFullOfDiscountsProducts")]
        public Task<List<CompanyProductSalesPlanningViewModel>> GetFullOfDiscountsProducts()
        {
            return _publicDataService.GetFullOfDiscountsProducts();
        }

        /// <summary>
        /// دریافت تصویر مربوط به اسلایدر در صفحه ی main 
        /// </summary>
        /// <param name="id"> CmsId </param>       
        /// <returns></returns>
        [HttpGet("GetMainPageSliderImage")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new string[] { "id", "modificationTime" })]
        public async Task<IActionResult> GetMainPageSliderImage(int id)
        {
            //TODO Check If Secure Request
            var docTypeId = (int)DocumentTypeEnum.CmsImage;
            var appDoc = _appDocumentService.GetFile(id, docTypeId);
            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }
            return NotFound();
        }

        /// <summary>
        /// دریافت تصویر مربوط به اسلایدر در صفحه ی main برای حالت موبایل
        /// </summary>
        /// <param name="id"> CmsId </param>       
        /// <returns></returns>
        [HttpGet("GetMainPageSliderMobileImage")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 3600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new string[] { "id", "modificationTime" })]
        public async Task<IActionResult> GetMainPageSliderMobileImage(int id)
        {
            var docTypeId = (int)DocumentTypeEnum.SliderMobileImage;
            var appDoc = _appDocumentService.GetFile(id, docTypeId);
            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }
            return NotFound();
        }
        /// <summary>
        /// پیدا کردن عنوانی ارسال شده در پارمتر بین رشته های تحصیلی درج شده در
        /// CommonBaseData
        /// بر اساس
        /// CommonBaseType = EducationalFieldType
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetFindEducationalField")]
        public async Task<List<EducationalFieldViewModel>> GetFindEducationalFieldAsync(string title)
        {
            return await _publicDataService.GetFindEducationalFieldAsync(title);
        }


        /// <summary>
        /// این متد جهت دریافت تصاویر تبلیغاتی سایت در صفحه ی اصلی کاربرد دارد
        /// مانند 2 تصویر بعد از دسته بندی ها و 4 تصویر بعد از پر فروش ترین دوره ها
        /// </summary>
        /// <param name="appDocumentId">شناسه سند</param>
        /// <returns>File</returns>
        [AllowAnonymous]
        [HttpGet("GetHomePageAdvertisingImage")]
        public async Task<IActionResult> GetHomePageAdvertisingImage(int appDocumentId)
        {
            if (appDocumentId == 0 || appDocumentId < 0)
                return NotFound();

            var cacheKey =
                _staticCacheManager.PrepareKeyForDefaultCache(
                    YashilModelCacheDefaults<AppDocument>.HomePageAdvertisingImageKey, appDocumentId);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _staticCacheManager.GetAsync<IActionResult>(cacheKey, () =>
             {

                 var appDocument = _appDocumentService.GetFile(appDocumentId);

                 if (appDocument?.DocumentFile != null && appDocument?.ContentType != null)
                 {
                     return File(appDocument.DocumentFile, appDocument.ContentType, appDocument.OrginalName);
                 }

                 return NotFound();
             });

        }

        /// <summary>
        /// این متد برای دریافت تصویر حالت دسکتاپ مربوط به
        /// categoryGroup
        /// کاربرد دارد
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetCategoryGroupSliderImage")]
        public IActionResult GetCategoryGroupSliderImage(int? objectId)
        {
            if (objectId == null || objectId.Value == 0)
                return NotFound("داده های ورودی نامعتبر است");


            var appDoc = _appDocumentService.GetFile(objectId.Value, (int)DocumentTypeEnum.CategoryGroupDesktopModeImage);

            if (appDoc?.DocumentFile != null)
                return File(appDoc.DocumentFile, appDoc.ContentType);


            return NotFound("تصویر مورد نظر یافت نشد");
        }

        /// <summary>
        /// این متد برای دریافت تصویر حالت موبایل مربوط به
        /// categoryGroup
        /// کاربرد دارد
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetCategoryGroupSliderMobileModeImage")]
        public IActionResult GetCategoryGroupSliderMobileModeImage(int? objectId)
        {
            if (objectId == null || objectId.Value == 0)
                return NotFound("داده های ورودی نامعتبر است");


            var appDoc = _appDocumentService.GetFile(objectId.Value, (int)DocumentTypeEnum.CategoryGroupMobileModeImage);

            if (appDoc?.DocumentFile != null)
                return File(appDoc.DocumentFile, appDoc.ContentType);


            return NotFound("تصویر مورد نظر یافت نشد");
        }

        /// <summary>
        /// این متد جهت دریافت تصاویر تبلیغاتی بالا سمت راست  صفحه اصلی کاربرد دارد
        /// </summary>
        /// <param name="id">شناسه تصویر</param>
        /// <returns></returns>
        [HttpGet("GetHomePageTopRightImage")]
        public async Task<IActionResult> GetHomePageTopRightImageAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<YashilCms>.HomePageTopRightSliderAdvertisingImageKey,
                id.Value);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _staticCacheManager.GetAsync<IActionResult>(cacheKey, async () =>
            {

                var appDoc = _appDocumentService.GetFile(id.Value, (int)DocumentTypeEnum.CmsImage);

                if (appDoc?.DocumentFile != null)
                {
                    return File(appDoc.DocumentFile, appDoc.ContentType);
                }

                return NotFound();
            });
        }

        /// <summary>
        //این متد برای دریافت تصاویر مربوط به شرکتی است که
        //آگهی شغلی در سامانه درج کرده است
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetCorporationImage")]
        public IActionResult GetCorporationImageAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var appDoc = _appDocumentService.GetFile(id.Value, (int)DocumentTypeEnum.CorporationImage);

            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }

            return File("/images/job-offer/building-logo.jpg", "image/jpg");
        }

        /// <summary>
        //این متد برای دریافت تصاویر پس زمینه مربوط به شرکتی است که
        //آگهی شغلی در سامانه درج کرده است
        ///مورد استفاده در کامپوننت 
        ///CorporationDetailHeader
        /// </summary>        
        /// <returns></returns>
        [HttpGet("GetCorporationBackgroundImage")]
        public IActionResult GetCorporationBackgroundImageAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var appDoc = _appDocumentService.GetFile(id.Value, (int)DocumentTypeEnum.CorporationBackgroundImage);

            if (appDoc?.DocumentFile != null)
                return File(appDoc.DocumentFile, appDoc.ContentType);

            return File("/images/job-offer/main.jpg", "image/jpg");
        }
        /// <summary>
        /// دریافت اطلاعات مربوط به یارانه اشتغال
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetEmploymentSubsidyInformation")]
        public Task<EmploymentSubsidyInformationViewModel> GetEmploymentSubsidyInformation()
        {
            return _publicDataService.GetEmploymentSubsidyInformationAsync();
        }

        /// <summary>
        /// این متد
        /// پیام اطلاع رسانی بعد از افزودن محصول به سبد خرید در سامانه ی باقر العلوم
        /// را بر میگرداند.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAddProductToBasketNoticeMessage")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAddProductToBasketNoticeMessageAsync()
        {
            var model = new
            {
                message = await _appConfigService.GetStringValueAsync(ApplicationConfigKey.YashilCommerceAddProductToBasketNoticeMessage)
            };

            return Ok(model);
        }

        /// <summary>
        /// دریافت لیست اطلاعات نمایندگی های ایزایران
        /// </summary>
        /// <returns></returns>

        [Route("GetListOfLocalCompanies")]
        [AllowAnonymous]
        public async Task<List<RepresentationLocalDto>> GetListOfLocalCompaniesAsync()
        {
            var valueString = await _appConfigService.GetStringValueAsync(
                ApplicationConfigKey.YashilCommerceRepresentationLocalCompanyId);
            if (string.IsNullOrEmpty(valueString)) return null;

            var companyIds = valueString.Split(',')
                .Select(int.Parse)
                .ToList();
            return await _mapper.ProjectTo<RepresentationLocalDto>(_representationService.GetRepresentationByParams(companyIds)).ToListAsync();
        }
    }
}
