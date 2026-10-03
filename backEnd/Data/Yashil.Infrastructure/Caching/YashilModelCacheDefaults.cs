using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Infrastructure.Caching
{
    public static partial class YashilModelCacheDefaults<TEntity> where TEntity : IBaseEntity<int>
    {

        public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();
        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string Prefix => $"Yashil.{EntityTypeName}.";
        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ByIdPrefix => $"Yashil.{EntityTypeName}.byid.";
        /// <summary>
        /// Gets a key for caching entity by identifier
        /// </summary>
        /// <remarks>
        /// {0} : entity id
        /// </remarks>
        public static CacheKey ByIdCacheKey => new CacheKey($"Yashil.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);
        public static CacheKey ImageByIdCacheKey => new CacheKey($"Yashil.{EntityTypeName}.image.byid.{{0}}", ByIdPrefix, Prefix);
        public static CacheKey CategoryMenuImageByIdCacheKey => new CacheKey($"Yashil.{EntityTypeName}.categoryMenuImage.byid.{{0}}", ByIdPrefix, Prefix);

        /// <summary>
        /// Gets a key for caching entities by identifiers
        /// </summary>
        /// <remarks>
        /// {0} : entity ids
        /// </remarks>
        public static CacheKey ByIdsCacheKey => new CacheKey($"Yashil.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);
        public static CacheKey GetCityByCompanyIdCacheKey => new CacheKey($"Yashil.{EntityTypeName}.byCompanyIdCacheKey.{{0}}", ByCompanyIdPrefix, Prefix);
        public static CacheKey GetAllCitiesCacheKey => new CacheKey($"Yashil.{EntityTypeName}.allCacheKey");
        public static CacheKey GetAllAppConfigs => new CacheKey($"Yashil.{EntityTypeName}.allCacheKey", Prefix);
        public static CacheKey Logo => new CacheKey($"Yashil.{EntityTypeName}.Logo-{{0}}", Prefix);
        public static CacheKey MainPageSliderLevelOne => new CacheKey($"Yashil.{EntityTypeName}.mainPageSliderLevelOne");

        public static CacheKey MainPageGalleryImage => new CacheKey($"Yashil.{EntityTypeName}.MainPageGalleryImage");

        public static CacheKey MainPageSliderLevelTwo => new CacheKey($"Yashil.{EntityTypeName}.mainPageSliderLevelTwo");
        public static CacheKey MainPageSliderLevelTwoByCategoryAndCustomerClub => new CacheKey("{0}-{1}", MainPageSliderLevelTwo.ToString());
        public static CacheKey GetCategoryByCompanyAndCityId => new CacheKey("Yashil.GetCategoryByCompanyAndCityId-{0}-{1}");
        public static CacheKey GetAllCompanies => new CacheKey($"Yashil.{EntityTypeName}.allCacheKey");

        public static CacheKey GetAllByCityAndCategory => new CacheKey($"Yashil.{EntityTypeName}.GetAllByCityAndCategory-{{0}}-{{1}}-{{2}}-{{3}}-{{4}}", Prefix);
        public static CacheKey GetAppConfigByKey => new CacheKey($"Yashil.{EntityTypeName}.GetAllByCityAndCategory-{{0}}-{{1}}", Prefix);

        public static CacheKey GetValueAppConfigByKey => new CacheKey($"Yashil.{EntityTypeName}.GetValueAppConfigByKey-{{0}}", Prefix);

        public static CacheKey GetAllByCompanyAndCategory => new CacheKey($"Yashil.{EntityTypeName}.GetAllByCompanyAndCategory-{{0}}-{{1}}-{{2}}", Prefix);
        public static CacheKey GetAllByCompanyAndCityAndCategory => new CacheKey($"Yashil.{EntityTypeName}.GetAllByCompanyAndCityAndCategory-{{0}}-{{1}}-{{2}}-{{3}}", Prefix);
        public static CacheKey GetAllByCompanyAndCity => new CacheKey($"Yashil.{EntityTypeName}.GetAllByCompanyAndCity-{{0}}-{{1}}-{{2}}-{{3}}-{{4}}", Prefix);
        /// <summary>
        /// دریافت دسته بندی های استفاده شده در برنامه ریزی دوره های فعال
        /// </summary>
        public static CacheKey GetUsedCategories => new CacheKey($"Yashil.{EntityTypeName}.GetUsedCategories-{{0}}-{{1}}-{{2}}-{{3}}-{{4}}-{{5}}-{{6}}", Prefix);

        public static CacheKey GetUsedCategoriesFactory => new CacheKey($"Yashil.{EntityTypeName}.GetUsedCategories-{{0}}-{{1}}-{{2}}-{{3}}-{{4}}-{{5}}-{{6}}", Prefix);
        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ByCompanyIdPrefix => $"Yashil.{EntityTypeName}.byids.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ByIdsPrefix => $"Yashil.{EntityTypeName}.byids.";

        public static CacheKey ProductReviewsModelKey => new CacheKey("Yashil.commerce.Categories-{0}", HierarchicallyUsedCategoriesPrefixCacheKey);

        public static CacheKey AllUsedCategoriesModelKey => new CacheKey("Yashil.commerce.UsedCategories-{0}", AllUsedCategoriesPrefixCacheKey);
        public static CacheKey FirstLevelCategoriesSimpleViewModelKey =>
            new CacheKey("Yashil.commerce-{0}-{1}-{2}", FirstLevelCategoriesPrefixCacheKey);
        public static CacheKey CategoryProductsByFilterProductSalePlanningViewModelModelKey => new CacheKey("Yashil.commerce-{0}-{1}-{2}-{3}", CategoryProductsByFilterProductPrefixCacheKey);
        public static CacheKey ProductSalesPlanningByFilterModelKey => new CacheKey("Yashil.Commerce.Products-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", ProductSalesPlanningByFilterPrefixCacheKey);

        /// <summary>
        /// پرفروش ترین کالاها
        /// </summary>
        public static CacheKey BestSellersKey => new CacheKey("Yashil.commerce-BestSellers-{0}", GetBestSellersKey);
        public static CacheKey FullOfDiscountsKey => new CacheKey("Yashil.commerce-FullOfDiscounts-{0}", GetFullOfDiscountsKey);

        /// <summary>
        /// کلید مربوط به جدیدترین کالا ها
        /// </summary>
        public static CacheKey ProductSalePlanningNewestKey => new CacheKey("Yashil.commerce-Newest-{0}", GetProductSalePlanningNewestNewestKey);
        /// <summary>
        /// کلید مربوط به شناسخ باشگاه مشتریان پیش فرض با توجه به پارامتر دامین
        /// </summary>
        public static CacheKey DefaultCustomerClubId => new CacheKey("Yashil.commerce.CustomerClub-DefaultCustomerClubId-{0}", DefaultCustomerClubIdCacheKey);
        /// <summary>
        /// کلید مربوط به کش کردن کامپوننت اخبار و اطلاعیه های صفحه ی اصلی سامانه ی باقرالعلوم
        /// </summary>
        public static CacheKey NewsAndAnnouncementsKey => new CacheKey("Yashil.Commerce.ViewComponent-NewsAndAnnouncement");
        /// <summary>
        /// کلید مربوط به کش کردن قسمت دریاره شرکت در فوتر سامانه باقرالعلوم
        /// </summary>
        public static CacheKey CorporationAboutContentViewKey => new CacheKey($"Yashil.{EntityTypeName}.ContentViewComponent-PagePositionType-{0}");

        /// <summary>
        /// کلید مورد استفاده در کش کردن دسته بندی های گروه 
        /// </summary>
        public static CacheKey CategoryGroupKey => new CacheKey($"Yashil.{EntityTypeName}.CurrentApplicationCategoryGroup-{0}");

        /// <summary>
        /// کلید مورد استفاده در کش کردن کامپوننت دریافت تصاویر تبلیغاتی سایت
        /// نمایش داده شده در زیر دسته بندی ها
        /// </summary>
        public static CacheKey HomePageCommercialImageInfoKey => new CacheKey("Yashil.Commerce.ViewComponent-HomePageCommercialImage");

        /// <summary>
        /// کلید مورد استفاده در کش کردن کامپوننت دریافت تصاویر آموزش مهارتی سایت
        /// نمایش داده شده در زیر پرفروش ترین دوره ها
        /// </summary>
        public static CacheKey HomePageLearningSkillImageInfoKey => new CacheKey("Yashil.Commerce.ViewComponent-HomePageLearningSkillImage");

        /// <summary>
        /// کلید مورد استفاده در کش کردن تصاویر تبلیغاتی سایت که در صفحه ی اصلی نمایش داده می شوند
        /// شامل دو تصویر بعد از دسته بندی ها و  چهار تصویر بعد از پرفروش ترین دوره ها
        /// </summary>
        public static CacheKey HomePageAdvertisingImageKey => new CacheKey("Yashil.Commerce.HomePage-Advertising-Image-{0}");

        /// <summary>
        /// کلید مربوط به کش کردن تعداد بازدید های سایت
        /// برای تعداد بازدید دیروز
        /// </summary>
        public static CacheKey SiteVisitPerviousDayCountKey => new CacheKey($"Yashil.{EntityTypeName}.GetSiteVisitPerviousDayCount");
        /// <summary>
        /// کلید مربوط به کش کردن تعداد بازدید های سایت
        /// برای تعداد بازدید کل
        /// </summary>
        public static CacheKey SiteVisitTotalCountKey => new CacheKey($"Yashil.{EntityTypeName}.GetSiteVisitTotalCount");


        /// <summary>
        ///کلید کش مربوط به برنامه  ریزی های نمایندگی های مراکز 
        /// </summary>
        public static CacheKey CompanyRepresentationsHasProductSalesPlanning =>
            new CacheKey($"Yashil.{EntityTypeName}.CompanyRepresentationsHasProductSalesPlanning-{{0}}", Prefix);

        /// <summary>
        ///کلید کش مربوط به دسته بندی ها با پدر  
        /// </summary>
        public static CacheKey GetCategoryWithParents =>
            new CacheKey($"Yashil.{EntityTypeName}.GetCategoryWithParents-{{0}}", Prefix);

        /// <summary>
        /// کلید مورد استفاده در کش کردن اطلاعات تصاویر تبلیغاتی بالا سمت راست  
        ///  صفحه ی اصلی سامانه
        /// این کلید اطلاعات مربوط به تصاویر را کش می کند
        /// </summary>
        public static CacheKey HomePageTopRightAdvertisingImageInfoKey => new CacheKey($"Yashil.{EntityTypeName}.ViewComponent-HomePageTopRightSliderAdvertisingImage");
        /// <summary>
        /// کلید مورد استفاده برای کش کردن فایل تصویر  
        ///  تبلیغاتی بالا سمت راست صفحه اصلی  
        /// </summary>
        public static CacheKey HomePageTopRightSliderAdvertisingImageKey => new CacheKey("Yashil.Commerce.ViewComponent-HomePageTopRightSliderAdvertisingImage-{0}");

        /// <summary>
        /// کلید مورد استفاده برای کش کردن 
        /// مقادیر اطلاعات پایه
        /// </summary>
        public static CacheKey CommonBaseDataKey => new CacheKey("Yashil.CommonBaseData-CommonBaseTypeId-{0}");
        /// <summary>
        /// کلید مورد استفاده برای کش کردن شهر ها        
        /// </summary>
        public static CacheKey CityOptionModelKey => new CacheKey("Yashil.City-OptionModel");
        /// <summary>
        /// کلید مورد استفاده برای کش کردن دسته بندی های شغلی        
        /// </summary>
        public static CacheKey JobCoCategoryOptionModelKey => new CacheKey("Yashil.JobCoCategory-OptionModel");

        /// <summary>
        /// کلید مورد استفاده برای کش کردن تعداد برنامه ریزی های دوره
        /// که بر اساس شناسه مرکز گروه بندی شده اند
        /// </summary>
        public static CacheKey ActiveProductSalesPlanningCountGroupedByCompanyKey => 
            new CacheKey($"Yashil.{EntityTypeName}.ActiveProductSalesPlanningGroupedByCompany-{{0}}-{{1}}-{{2}}-{{3}}-{{4}}-{{5}}");

        /// <summary>
        /// کلید مربوط به کش کردن اطلاعات مراکز فعالی که
        /// دارای تصویر برای حالت موبایل و دسکتاپ هستند
        /// </summary>
        public static CacheKey ActiveCompaniesIfHasPictureKey => new
            CacheKey($"Yashil.{EntityTypeName}.ActiveCompaniesIfHasPicture-{{0}}");

        /// <summary>
        ///
        ///  (( بنابر تصمیم گرفته شده در حال حاضر این متد کاربردی ندارد چون کش را برای سوال های آزمون برداشته ایم ))
        /// 
        /// کلید مربوط به کش کردن سوال های آزمون
        /// </summary>
        public static CacheKey GetExamQuestionsKey => new CacheKey($"Yashil.{EntityTypeName}.GetExamQuestions-{{0}}");

        /// <summary>
        /// کلید مربوط به کش کردن دریافت دوره های عمومی
        /// برای صفحه ی هوم سامانه ی ال ام اس
        /// </summary>
        public static CacheKey GetLandingHomePagePublicProductSalesPlanningKey =>
            new CacheKey($"Yashil.{EntityTypeName}.GetLandingHomePagePublicProductSalesPlanning");

        /// <summary>
        /// کلید مربوط به کش کردن دریافت دوره 
        /// ها بر اساس شناسه مرکز  
        /// </summary>
        public static CacheKey GetProductSalesPlanningConsiderPublicFilteredByCompanyIdKey =>
            new CacheKey($"Yashil.{EntityTypeName}.GetProductSalesPlanningConsiderPublicFilteredByCompanyId-{{0}}");

        /// <summary>
        /// کلید مربوط به کش کردن دریافت اخبار عمومی صفحه اصلی
        /// مورد استفاده در سامانه ی
        /// LMS
        /// </summary>
        public static CacheKey PublicPageNewsListCacheKey => new CacheKey($"Yashil.{EntityTypeName}.PublicPageNewsListCacheKey");

        /// <summary>
        /// کلید مربوط به کش کردن برنامه ریزی دوره عمومی
        /// اگر برنامه ریزی دوره عمومی باشد بر اساس شناسه برنامه ریزی دوره عمومی بودن آن را کش می کنیم
        /// </summary>
        public static CacheKey PublicProductSalesPlanningCacheKey => new CacheKey($"Yashil.{EntityTypeName}.PublicProductSalesPlanning-{{0}}");



        #region MyRegion
        public static string AdditionalUserPropsPrefixCacheKey => "Yashil.Commerce.AdditionalUserPropsModelKey-";
        public static CacheKey AdditionalUserPropsModelKey => new CacheKey("Yashil.commerce.AdditionalUserPropsModelKey-{0}", AdditionalUserPropsPrefixCacheKey);

        public static string CurrentUserCustomerClubPropsPrefixCacheKey => "Yashil.Commerce.CurrentUserCustomerClubModelKey-";
        public static CacheKey CurrentUserCustomerClubPropsKey => new CacheKey("Yashil.commerce.CurrentUserCustomerClubModelKey-{0}", CurrentUserCustomerClubPropsPrefixCacheKey);

        public static CacheKey GetActiveCompaniesForAllApps => new CacheKey("Yashil.commerce.GetActiveCompaniesForAllApps-{0}", Prefix);
        public static CacheKey CategoryHierarchicalModelList => new CacheKey("Yashil.commerce.Category-{0}-{1}-{2}-{3}-{4}-{5}", CategoryHierarchicalViewModelList);

        #endregion

        public static string GetAllByCompanyAndCategoryPrefixCachKey =>
            $"Yashil.{EntityTypeName}.GetAllByCompanyAndCategory-";
        public static string ProductSalesPlanningByFilterPrefixCacheKey => "Yashil.Commerce.Products-";
        public static string HierarchicallyUsedCategoriesPrefixCacheKey => "UsedCategories.Hierarchically";
        public static string AllUsedCategoriesPrefixCacheKey => "AllUsedCategories.Hierarchically";
        public static string FirstLevelCategoriesPrefixCacheKey => "FirstLevelCategories.CategorySimpleViewModel";
        public static string CategoryProductsByFilterProductPrefixCacheKey => "CategoryProductsByFilter.SalePlanningViewModel";
        public static string CategoryHierarchicalViewModelList => "Yashil.Commerce.Category";

        public static string GetVirtualClassRoomConfigCacheKey => "GetVirtualClassRoomConfigCacheKey";


        public static string GetBestSellersKey => "GetBestSellersKey";
        public static string GetFullOfDiscountsKey => "GetFullOfDiscountsKey";

        public static string GetProductSalePlanningNewestNewestKey => "GetProductSalePlanningNewestNewestKey";

        public static string DefaultCustomerClubIdCacheKey => "DefaultCustomerClubId";
        /// <summary>
        /// کش مربوط به اطلاعات محصول 
        /// </summary>
        public static CacheKey GetProductDetailCacheKey => new CacheKey("Yashil.commerce.ProductSalesPlanning-{0}-{1}", ProductDetailPrefixCacheKey);
        /// <summary>
        ///  پریفیکس مربوط به کش اطلاعات محصول
        /// </summary>
        public static string ProductDetailPrefixCacheKey => "ProductDetail";

        /// <summary>
        /// کلید کش مریوط به مجوز عملیات منبع 
        /// </summary>
        public static CacheKey GetResourceAppActionCacheKey =>
            new CacheKey($"Yashil.{EntityTypeName}.GetResourceAppActionCacheKey-{{0}}-{{1}}", Prefix);

        public static string GetCategoryGroupActiveSubsidyInfoPrefixCacheKey => "ProductDetail";
        /// <summary>
        /// کلید اطلاعات مربوط به یارانه فعال گروه دسته بندی
        /// </summary>
        public static CacheKey GetCategoryGroupActiveSubsidyInfoCacheKey => 
            new CacheKey($"Yashil.{EntityTypeName}.GetCategoryGroupActiveSubsidyInfo-{{0}}", GetCategoryGroupActiveSubsidyInfoPrefixCacheKey);

        #region YashilCorporationUnit
        /// <summary>
        /// کلید کش مریوط دریافت همه واحد شرکت 
        /// </summary>
        public static string GetAllUnitCacheKeyPrefixCacheKey => "GetAllUnit";
        /// <summary>
        ///  کش مریوط دریافت همه واحد شرکت 
        /// </summary>
        public static CacheKey GetAllUnitCacheKey => new CacheKey($"Yashil.{EntityTypeName}.allCacheKey", GetAllUnitCacheKeyPrefixCacheKey);

        #endregion

    }
}
