using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;
using YashilLight.Core.Dto;

namespace YashilCommerce.Core.Services
{
    public interface IProductSalesPlanningService : IGenericService<ProductSalesPlanning, int>
    {
        /// <summary>
        /// ابتدا بررسی می کند در صورتی که کاربر لاگین کرده دارای دسترسی تایید برنامه ریزی دوره باشد
        /// متد به روز رسانی برنامه ریزی دوره را صدا می زند
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        Task UpdateProductSalesPlanningCalculatedPriceIfHasAccessToConfirmAsync(int productSalesPlanningId);
        Task UpdateProductSalesPlanningCalculatedPriceAsync(int? productSalesPlanningId = null);
        IQueryable<ProductSalesPlanning> GetByCustomFilter(int? productId, int? representationId, int? manageInventoryMethodType, int? productAvailabilityRangeType, int? deliveryDateType, int? published, int? confirmed);

        // Task<decimal?> CalculatePriceAsync(int productId, int? representationId, decimal? price);

        //Task<decimal?> CalculatePriceByCompanyAndRepresentationIdAsync(int companyId, int? representationId,
        //    decimal? price);

        Task<IQueryable<ProductSalesPlanning>> SearchProductSalesPlanningsAsync(string searchTitle, int? applicationGroupId,
            int? applicationId,
            int? categoryId, List<int> company, int? representationId, int? cityId, int? categoryGroupId, bool loadSubCategories,
            bool showIfHasPrice = false);


        Task<IQueryable<Company>> FindProductSalesPlanningsCompaniesAsync(string searchTitle, int? applicationId,
            int? categoryId, int? representationId, int? cityId, bool loadSubCategories,
            bool showIfHasPrice = false);

        Task<int?> GetRepresentationIdByProductSalesPlanningIdAsync(int salesPlanningId, int applicationId);
        IQueryable<ProductSalesPlanning> FindById(int pSpId, int? applicationId = null);

        /// <param name="filterByOrderItemRepresentationId">اگر برنامه ریزی دوره ای شناسه نمایندگی نداشته باشد لازم است
        /// در صورت فعال بودن این فیلتر و ارسال شناسه نمایندگی بین سفارش ها نیز بررسی می کند که اگر سفارشی با برای یک برناه
        /// ریزی دوره با شناسه این نمایندگی در ایتم سفارش ثبت شده باشد نیز جزو نیازمندی ما است</param>
        Task<IQueryable<ProductSalesPlanning>> GetByRepresentationFilterAsync(
            int applicationId,
            int? companyId,
            int? categoryId,
            int? representationId,
            bool showOnlyActiveProductSalesPlannings,
            bool doGetProductSalePlanningByRepresentation,
            bool? visibleIndividually = null,
            int? customerClubId = null,
            bool? showArchive = false, bool? filterByOrderItemRepresentationId = false, bool? includeHubRepresentationFilter = false);

        /// <summary>
        /// بررسی میکند که برای آیدی محصول داده شده
        /// تاکنون برنامه ریزی فروشی وجود داشته است یا خیر
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckProductHasAnyProductSalesPlanningAsync(int productId);
        /// <summary>
        /// بررسی میکند که برای ایدی نمایندگی داده شده
        /// تا کنون برنامه ریزی فروشی وجود داشته است یا خیر
        /// </summary>
        /// <param name="representationId"></param>
        /// <returns></returns>
        bool CheckRepresentationHasAnyProductSalesPlanning(int representationId);

        /// <summary>
        /// بررسی می کند که آیا یک برنامه ریزی دوره
        /// در اپلیکیشن وجود دارد و شرایط فعال بودن جهت فروش را دارا است یا نه
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<bool> CheckExistsProductSalePlanningInApplicationAsync(int productSalesPlanningId, int applicationId);

        /// <summary>
        /// بررسی می کند که آیا یک برنامه ریزی دوره
        /// در اپلیکیشن وجود دارد و شرایط فعال بودن جهت فروش را دارا است یا نه
        /// و همچنین قیمت آن باید بیشتر از 0 باشد 
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="applicationId"></param>
        /// <param name="considerZeroOrNullPriceValidation"></param>
        /// <returns></returns>
        Task<bool> CheckExistsProductSalePlanningInApplicationAsync(int productSalesPlanningId, int applicationId, bool considerZeroOrNullPriceValidation);
        IQueryable<ProductSalesPlanning> GetAllByApplicationId(int applicationId, bool readOnly);


        Task<AppDocument> GetProductSalesPlanningPicture(int productSalesPlanningId, int? applicationId);
        IQueryable<AppDocument> GetProductSalesPlanningAttachments(int productSalesPlanningId, int? applicationId);


        /// <summary>
        /// لیست ایتم های مربوط به گروه را بر میگرداند
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<ProductSalesPlanning> GetProductSalesPlanningAssignedToGroup(int groupId, bool readOnly = false);

        /// <summary>
        /// ایتم های گروهی با مقدار ایتم گروپ وارد شده را بر می گرداند
        /// </summary>
        /// <param name="itemGroupId"></param>
        /// <returns></returns>
        Task<bool> ExistsAnyProductSalesPlanningWithThisItemGroupIdAsync(int itemGroupId);

        /// <summary>
        /// لیست ایتم گروه های گروه بندی نشده
        /// </summary>
        /// <param name="itemGroupSubId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<ProductSalesPlanning> GetNotGroupedProductSalesPlanning(int itemGroupSubId, bool readOnly = false);

        /// <summary>
        /// جهت ایتم گروپ های که تا کنون گروه بندی نشده اند به گروه نهایی و ثبت نهایی کردن آن گروه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> AssignItemGroupForProductSalesPlanningAsync(int id);

        Task<IQueryable<ProductSalesPlanning>> GetTeacherProductSalesPlanningsAsync(int applicationId, int teacherId, int? companyId, int? categoryId, int? representationId, bool hierarchical, bool showOnlyActiveProductSalesPlannings, bool? visibleIndividually = null);
        Task<bool> ConvertProductSalesPlanningImageAsync();
        Task UpdateOrderProductSalesPlanningsCapacityAsync(Order order);

        /// <summary>
        /// فیلد confirmed
        /// مربوط به productSalesPlanning را بروزرسانی میکند
        /// در واقع اگر مجوز لازم را نداشت این فیلد null  میشود
        /// تا برنامه ریزی دوره به حالت منتظر تایید برود
        ///  اگر کاربر ادمین باشد وضعیت برنامه ریزی دوره تغییری نمیکند
        /// اما اگر کاربر مجوز تغییر تایید شدن برنامه ریزی دوره را نداشت به حالت منتظر تایید
        /// یا confirmed = null
        /// می رود.
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>
        Task<ProductSalesPlanning> ChangeProductSalesPlanningConfirmationIfNeededAsync(int productSalesPlanningId,
            bool saveAfterUpdate = false);
        /// <summary>
        /// بررسی میکند که آیا کاربر مجوز تغییر وضعیت confirmed
        /// برنامه ریزی دوره را دارد یا خیر
        /// </summary>
        /// <returns></returns>
        bool CanCurrentUserConfirmProductSalesPlanning();

        /// <summary>
        /// در صورتی که برنامه ریزی محصول دارای درصد تخفیف باشد
        /// قیمت با تخفیف را با درصد تخفیف برنامه ریزی محصول محاسبه می کند.
        /// در غیر اینصورت درصد تخفیف باشگاه مشتریان کاربر لاگین کرده را دریافت
        /// و قیمت با تخفیف را با آن محاسبه می کند
        /// </summary>
        /// <param name="price">قیمت</param>
        /// <param name="productSalePlanningDiscountPercentage">درصد تخفیف برنامه ریزی دوره</param>
        /// <param name="saleConfigurationId">شناسه تنظیم فروش</param>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <returns></returns>
        Task<(decimal discountPercentage, decimal? calcedPrice)>
            GetCalcPriceInformationAsync(
            decimal? price,
            decimal? productSalePlanningDiscountPercentage,
            int saleConfigurationId,
            int customerClubId);

        /// <summary>
        /// محصولاتی که بیشترین تخفیف را دارند، بدون در نظر گرفتن مرکز بر می گرداند
        /// </summary>
        /// <param name="customerClubId"></param>
        /// <returns></returns>
        IQueryable<CompanyProductSalesPlanningDomainModel> GetFullOfDiscounts(int customerClubId);
        Task<(int? companyId, int? represenationId)> GetCompanyAndRepresentationIdAsync(int productSalePlaningId);
        /// <summary>
        /// بیشترین فروش محصولات مرکز را میگرداند
        /// </summary>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        IQueryable<CompanyProductSalesPlanningDomainModel> GetBestSellers(int customerClubId);
        /// <summary>
        /// جدید ترین محصولات مرکز را میگرداند
        /// </summary>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        IQueryable<CompanyProductSalesPlanningDomainModel> GetNewest(int customerClubId);
        /// <summary>
        /// بررسی می کند که آیا برنامه ریزی محصول ظرفیت دارد یا خیر
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی محصول</param>
        /// <returns></returns>
        Task<bool> ChecksProductSalesPlanningHaveCapacityAsync(int productSalesPlanningId);

        /// <summary>
        /// آیا درصد تخفیف نسبت به قرارداد تغییر نکرده است؟
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        Task<bool> HasDiscountPercentageChangedComparedToTheContractAsync(int productSalesPlanningId);
        /// <summary>
        /// برنامه ریزی محصولات فروش رفته که فاکتور نشده اند را بر می گرداند
        /// </summary>
        /// <param name="applicationId">برنامه</param>
        /// <param name="companyId">مرکز</param>
        /// <param name="representationId">نمایندگی</param>
        /// <returns></returns>
        Task<IQueryable<ProductSalesPlanning>> GetUnFactoredProductSalesPlanningsAsync
            (int applicationId, int? companyId, int? representationId);
        /// <summary>
        /// برسی می کند که آیا کاربر به برنامه ریزی دوره دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>bool</returns>
        Task<bool> ValidateUserHasAccessToProductSalesPlanningAsync(int productSalesPlanningId, int userId);
        /// <summary>
        /// بررسی می کند که کاربر دوره آزمون استغال را خریده است
        /// </summary>
        /// <returns>bool</returns>
        Task<bool> CheckUserPurchasedEmploymentTestCourse();
        /// <summary>
        /// این متد بررسی می کند آیا کاربر دوره منجر به اشتغال را خریده و در آزمون استعداد یابی شرکت کرده است؟
        /// همچنین بررسی میکند که کاربر درحال خرید آن است منجر به اشتغال است یا نه؟ و آیا یارانه اشتغال به کاربر تعلق
        /// می گیرد یا نه؟
        /// در صورتی که کاربر لاگین نباشد
        /// null
        /// بر میگرداند
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns>EmploymentSubsidyDto || null</returns>
        Task<EmploymentSubsidyDto> CheckEmploymentSubsidyIsGrantedToCurrentUserAndPspIsEmployment(int productSalesPlanningId);
        /// <summary>
        /// این متد عملیات جستجو براساس کلید در بین عنوان برنامه ریزی های دوره فعال را انجام می دهد
        /// </summary>
        /// <param name="searchKey">رشته کلید</param>
        /// <returns>List<ProductSalesPlanningSearchDto></returns>
        Task<IQueryable<ProductSalesPlanningSearchDto>> SearchInProductSalesPlanningTitleAsync(string searchKey);

        /// <summary>
        /// دریافت برنامه ریزی دوره
        /// productSalesPlanning
        /// مرتبط به شناسه تگ های ارسالی
        /// این سرویس برنامه ریزی دوره ای را که شرایط فعال بودن و نمایش در سامانه را دارا باشند 
        /// و به شناسه تگ های ارسالی مرتبط باشند را بر می گرداند
        /// </summary>
        /// <param name="tagIds">شناسه تگ ها</param>
        /// <param name="customerClubId">شناسه باشگاه مشتری</param>
        /// <returns>ProductSalesPlanning</returns>
        IQueryable<ProductSalesPlanning> GetProductSalesPlanningRelatedToTagIds(List<int> tagIds, int customerClubId);

        /// <summary>
        /// این متد جهت دریافت تعداد برنامه ریزی دوره های فعال که
        /// بر اساس شناسه مرکز و شناسه
        /// categoryGroup
        /// گروه بندی شده اند کاربرد دارد
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <param name="companyIds">شناسه مراکز</param>
        /// <param name="representationId">شناسه نمایندگی</param>
        /// <param name="cityId">شناسه شهر</param>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <param name="query"></param>
        /// <returns><List<GroupedActiveProductSalesPlanningDto>></returns>
        Task<List<GroupedActiveProductSalesPlanningDto>> GetGroupedActiveProductSalesPlanningCountAsync(
            int? applicationId,
            int? categoryId, List<int> companyIds, int? representationId, int? cityId, int? categoryGroupId, IQueryable<ProductSalesPlanning> query);

        /// <summary>
        ///  برنامه ریزی فعال با فیلتر های
        /// تاریخ انتهای برنامه ریزی منقضی نشده باشد و ..
        /// و برنامه ریزی که دارای تنظیم فروش فعال با تاریخ منقضی نشده ..  
        /// </summary>
        /// <param name="applicationId">برنامه</param>
        /// <param name="customerClubId">باشگاه مشتریان یعنی برنامه ریزی دوره باید ویژگی تنظیم فروش فعال در این باشگاه داسته باشد</param>
        /// <param name="categoryGroupId">گروه دسته بندی محصول</param>
        /// <param name="isEmploymentCategoryGroup">آیا گروه دسته بندی محصول اشتغال است؟</param>
        /// <returns></returns>
        Expression<Func<ProductSalesPlanning, bool>> GetProductSalePlanningIsActiveFilter(int? applicationId,
            int customerClubId,
            int? categoryGroupId,
            bool isEmploymentCategoryGroup);

        /// <summary>
        /// بررسی می کند که آیا دوره برگزار شده است
        /// مثلا اگر برای دوره آزمون یا کلاسی تعریف شده باشد
        /// برنامه ریزی دوره استفاده شده است
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        Task<(bool isHeld, string message)> CheckProductSalePlanningHasBeenHeld(int productSalesPlanningId);

        /// <summary>
        /// بررسی می کند که آیا کاربر به یک برنامه ریزی دوره دسترسی دارد یا نه؟
        /// باید مرکز کاربر و مرکز برنامه ریزی دوره ای که شناسه آن ارسال شده است یکی باشد.
        /// </summary>
        /// <param name="productSalesPlaningId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CheckUserHasAccessToProductSalesPlanning(int productSalesPlaningId, int userId);

        /// <summary>
        /// این متد در حالت بروزرسانی
        /// ProductSalesPlanning
        /// برای مواقعی که شاخص ثالثی با استفاده از
        /// Api Call
        /// اقدام به بروزرسانی
        /// productSalesPlanning
        /// می کند استفاده می شود و مقادیر
        /// نام Property
        /// های مدل و
        /// UpdateSelectedFieldsOrIgnoreTheme
        /// را برمی گرداند
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="editModel"></param>
        /// <returns></returns>
        ModifyPropertiesDescriptor GetPropertiesForApplyOrIgnoreUpdateProductSalesPlanning(ProductSalesPlanning entity,
            ProductSalesPlanningBaseEditModel editModel);

        /// <summary>
        /// این متد بررسی می کند که تاریخ شروع و پایان یک آزمون بعد از تاریخ پایان دوره نباشد
        /// در صورتی که دوره شامل تاریخ پایان نباشد بررسی را انجام نمی دهد
        /// </summary>
        /// <param name="examCategoryId"></param>
        /// <param name="examStartDateInt"></param>
        /// <param name="examEndDateInt"></param>
        /// <returns></returns>
        Task<(bool isValid, string message)> ValidateProductSalesPlaningEndDateByExamCategoryIdAsync(int examCategoryId, int examStartDateInt, int examEndDateInt);

        /// <summary>
        /// این متد جهت دریافت اطلاعات مربوط به مرکز و نمایندگی یک برنامه ریزی دوره کاربرد دارد
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<(int companyId, int? representationId)> GetProductSalesPlanningCompanyAndRepresentationIdAsync(int productSalesPlanningId);

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر جاری به شناسه برنامه ریزی دوره ارسال شده دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<bool> HasCurrentUserAccessToProductSalesPlanningAsync(int productSalesPlanningId);

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر جاری به ایتم سفارش ارسال شده دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        Task<bool> HasCurrentUserAccessToOrderItemProductSalesPlanningAsync(int orderItemId);

        /// <summary>
        /// آیا دوره از نوع نیاز به تایید تامین کننده قبل از پرداخت است یا نه؟
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        Task<bool> IsProductNeedToSupplierApprovalBeforePaymentAsync(int productSalesPlanningId);

        /// <summary>
        /// دریاتف برنامه ریزی دوره های از نوع
        /// نیاز به تایید تامین کننده قبل از پرداخت هستند
        /// و حداقل یک سفارش برای آن ها ثبت شده است.
        /// بر اساس شناسه مرکز و نمایندگی
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="applicationId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        Task<IQueryable<ProductSalesPlanning>> GetProductSalesPlanningThatHasAnyOrderWithWaitingForSupplierApprovalAsync(int? companyId, int applicationId, int? representationId = null);

        /// <summary>
        /// دریافت برنامه ریزی دوره های مربوط به یک مرکز یا نمایندگی که
        /// گزینه
        /// RequireSupplierApprovalBeforePayment
        /// آن ها فعال باشد
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="applicationId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        Task<IQueryable<ProductSalesPlanning>> GetProductSalesPlanningWithRequireSupplierApprovalBeforePaymentAsync(int? companyId, int applicationId, int? representationId = null);

        /// <summary>
        /// این متد مقدار حذف برنامه ریزی دوره را برابر با صحیح می کند. رکورد در دریتابیس موجود است
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        Task<int> Archive(int productSalesPlanningId);

        /// <summary>
        ///  می کند false این متد مقدار حذف برنامه ریزی دوره را برابر با  
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns></returns>
        Task<int> RestoreFromArchive(int productSalesPlanningId);

        /// <summary>
        /// دریافت لیست دوره هایی که مدیاسیشن الزامی و مشاهده نشده توسط کاربر دارد
        /// </summary>
        /// <param name="mediaSessionDtos"></param>
        /// <returns></returns>
        Task<List<ProductSalesPlanningDto>> GetUserNotViewedMediaSessionProductInfoAsync(List<MediaSessionDto> mediaSessionDtos);

        /// <summary>
        /// این متد عملیات بروزرسانی تاریخ پایان دوره را انجام میدهد در صورتی که دوره
        /// تاریخ پایانی نداشته باشد.
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="startDateInt"></param>
        /// <param name="endDateInt"></param>
        /// <returns></returns>
        Task UpdateProductSalesPlaningStartAndEndDateAsync(int productSalesPlanningId, int startDateInt, int endDateInt);

        /// <summary>
        /// دریافت برنامه ریزی دوره های درحال اجرا
        /// به شرطی که حداقل یک دانشجو داشته باشد و
        /// یک آزمون یا یک محتوا یا یک تکلیف برای آن تعریف شده باشد
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        IQueryable<ProductSalesPlanning> GetActiveProductSalesPlanningThatAtlastHasOnePsPStudent(int? companyId, int? representationId = null);


        /// <summary>
        ///  دریافت عنوان نمایندگی های مربوط به برنامه ریزی دوره های مرکز
        /// که برای استفاده در اعتبار سنجی اکسل بارگذاری دوره کاربرد دار
        /// نمایندگی ها دوره های که منتشر شده باشند مربوط به مرکز باشند
        /// و به صورت مستقل قابل نمایش باشد
        /// به صورت غیر تکراری
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<List<string>> GetProductSalesPlanningRepresentationTitleListForFillInUploadedProductItemXlsValidationAsync(int companyId);

        /// <summary>
        /// دریافت برنامه ریزی دوره های مربوط به مربی
        /// که فعال باشند یعنی حداقل یک دانشجو و یک کلاس مجازی یا آزمون یا رسانه بارگذاری شده دارد
        /// جدید ترین ها را برمیگردانیم
        /// بر اساس پیشفرض
        /// top = 5
        /// داده که 5 تای آخر را دریافت کنیم.
        /// </summary>
        /// <param name="teacherUserId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        IQueryable<ProductSalesPlanning> GetTeacherLatestActiveProductSalesPlanning(int teacherUserId,int? top);

        /// <summary>
        /// دریافت آخرین دوره های تعریف شده در سامانه که فعال هستند
        ///  یعنی حداقل یک دانشجو و یک کلاس مجازی یا آزمون یا رسانه بارگذاری شده دارد
        /// جدید ترین ها را برمیگردانیم
        /// بر اساس پیشفرض
        /// top = 5
        /// داده که 5 تای آخر را دریافت کنیم.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IQueryable<ProductSalesPlanning> GetLatestActiveProductSalesPlanning(int? companyId,
            int? representationId, int? top);


        /// <summary>
        /// دریافت تعداد برنامه ریزی دوره های فعال
        /// به ازای هر مرکز 
        /// </summary>
        /// <returns></returns>
        public Task<List<ActiveProductSalesPlanningCountGroupedDto>> GetActiveProductSalesPlanningCountGroupByCompanyAsync(int? companyId);

        /// <summary>
        /// دریافت تعداد برنامه ریزی دوره های فعال
        /// مربوط به یک مربی به ازای
        /// representation
        /// های که معلم به آن ها تخصیص داده شده است
        /// </summary>
        /// <param name="teacherUserId"></param>
        /// <returns></returns>
        public Task<List<ActiveProductSalesPlanningCountGroupedDto>> GetTeacherActiveProductSalesPlanningCountGroupByRepresentationAsync(int teacherUserId);
        /// <summary>
        /// آیا برنامه ریزی  دوره ای برای این تنظیم فروش وجود دارد
        /// </summary>
        /// <param name="saleConfigurationId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<bool> CheckExistProductSalePlanningBySaleConfigurationId(int saleConfigurationId);
    }
}
