using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories;

public interface IProductSalesPlanningRepository : IGenericRepository<ProductSalesPlanning, int>
{
    IQueryable<ProductSalesPlanning> GetByCustomFilter(int? productId, int? representationId, int? manageInventoryMethodType, int? productAvailabilityRangeType, int? deliveryDateType, int? published, int? confirmed);

    // Task<decimal?> CalculatePriceByCompanyAndRepresentationIdAsync(int companyId, int? representationId, decimal? price);

    public IQueryable<ProductSalesPlanning> SearchProductSalesPlannings(
        int customerClubId,
        string searchTitle,
        int? applicationGroupId,
        int? applicationId,
        int? categoryId,
        List<int> company,
        int? getCategoryProducts,
        int? cityId,
        int? categoryGroupId,
        bool loadSubCategories,
        bool isEmploymentCategoryGroup,
        bool showIfHasPrice = false);

    IQueryable<Company> FindProductSalesPlanningsCompanies(int customerClubId, string searchTitle, int? applicationId,
         int? categoryId, int? representationId, int? cityId, bool loadSubCategories,
         bool showIfHasPrice = false);


    /// <summary>
    /// check the existance of sales planingof the product
    /// count sales planing and if exist sales planing return true
    /// </summary>
    /// <param name="productId"></param>
    bool CheckExistsSalePlanning(int productId);


    Task<int?> GetRepresentationIdByProductSalesPlanningIdAsync(int salesPlanningId, int applicationId);
    IQueryable<ProductSalesPlanning> FindById(int id, int? applicationId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationId">برنامه</param>
    /// <param name="companyId">مرکز</param>
    /// <param name="categoryId">دسته بندی</param>
    /// <param name="representationId">نمایندگی</param>
    /// <param name="showOnlyActiveProductSalesPlannings">دریافت برنامه ریزی دوره فعال</param>
    /// <param name="doGetProductSalePlanningByRepresentation">
    /// اگر شناسه نمایندگی هم مقدار داشته باشد و این فیلد
    /// true
    /// باشد، برنامه ریزی دوره هایی که شناسه نمایندگی آنها نال باشد را هم بیاورد</param>
    /// <param name="customerClubId">باشکاه مشتریان</param>
    /// <param name="visibleIndividually">قابل فروش</param>
    /// <param name="doGetUnFactoredProductSalesPlannings">برنامه ریزی دوره های فاکتور نشده را بر می گرداند</param>
    /// <param name="showArchives">نمایش برنامه ریزی دوره های آرشیو شده</param>
    /// <param name="filterByOrderItemRepresentationId">اگر برنامه ریزی دوره ای شناسه نمایندگی نداشته باشد لازم است
    /// در صورت فعال بودن این فیلتر و ارسال شناسه نمایندگی بین سفارش ها نیز بررسی می کند که اگر سفارشی با برای یک برناه
    /// ریزی دوره با شناسه این نمایندگی در ایتم سفارش ثبت شده باشد نیز جزو نیازمندی ما است</param>
    /// <returns></returns>
    IQueryable<ProductSalesPlanning> GetByRepresentationFilter(
        int applicationId,
        int? companyId,
        int? categoryId,
        int? representationId,
        bool showOnlyActiveProductSalesPlannings,
        bool doGetProductSalePlanningByRepresentation,
        int customerClubId,
        bool? visibleIndividually = null,
        bool? doGetUnFactoredProductSalesPlannings = null,
        bool? showArchives = false,
        bool? filterByOrderItemRepresentationId = false,
        bool? includeHubRepresentationFilter = false);
    int? GetCourseId(int id);
    Task UpdateProductSalesPlanningCalculatedPriceAsync(int? productSalesPlanningId);

    /// <summary>
    /// بررسی وجود تنظیم فروش برای یک محصوول بدون هیچ شرطی
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<bool> CheckProductHasAnyProductSalesPlanningAsync(int productId);
    /// <summary>
    /// بررسی میکن که برای آیدی نمایندگی داده شده برنامه ریزی فروشی وجود دارد یا خیر
    /// </summary>
    /// <param name="representationId"></param>
    /// <returns></returns>
    bool CheckRepresentationHasAnyProductSalesPlanning(int representationId);

    IQueryable<ProductSalesPlanning> GetAllByApplicationId(int applicationId, bool readOnly);

    /// <summary>
    /// برنامه ریزی محصول فعال را بر میگرداند
    /// درصورتی که برنامه ریزی محصول تاریخ پایان دوره داشته باشد و تایید شده باشد و تنظیم فروش فعال داشته باشد 
    /// آن را بر میگرداند
    /// </summary>
    /// <param name="id">شناسه برنامه ریزی محصول</param>
    /// <param name="applicationId">شناسه برنامه</param>
    /// <param name="customerClubId">باشگاه مشتریان</param>
    /// <returns></returns>
    IQueryable<ProductSalesPlanning> GetActiveProductSalePlanningById(int id, int? applicationId, int customerClubId);


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
    /// <param name="itemGroupSubId"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task<int> AssignItemGroupForProductSalesPlanningAsync(int itemGroupSubId, int groupId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="teacherUserId">آیدی یوزر کاربر</param>
    /// <param name="companyId"></param>
    /// <param name="categoryId"></param>
    /// <param name="representationId"></param>
    /// <param name="hierarchical"></param>
    /// <param name="showOnlyActiveProductSalesPlannings">فقط نمایش برنامه ریزی دوره فعال</param>
    /// <param name="customerClubId">باشگاه مشتریان</param>
    /// <param name="visibleIndividually">قابل خرید باشد</param>
    /// <returns></returns>
    Task<IQueryable<ProductSalesPlanning>> GetTeacherProductSalesPlanningsAsync(
        int applicationId,
        int teacherUserId,
        int? companyId,
        int? categoryId,
        int? representationId,
        bool hierarchical,
        bool showOnlyActiveProductSalesPlannings,
        int customerClubId,
        bool? visibleIndividually = null);

    /// <summary>
    ///محصولاتی که بیشترین تخفیف را دارند، بدون در نظر گرفتن مرکز بر می گرداند
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="customerClubId"></param>
    /// <returns></returns>
    IQueryable<CompanyProductSalesPlanningDomainModel> GetFullOfDiscounts(int applicationId, int customerClubId);

    /// <summary>
    /// بیشترین فروش محصولات مرکز را
    /// بر اساس تعداد درخواست شده در پارامتر بر میگرداند
    /// </summary>
    /// <param name="customerClubId">باشگاه مشتریان</param>
    /// <param name="applicationId"></param>
    /// <returns></returns>
    IQueryable<CompanyProductSalesPlanningDomainModel> GetBestSellers(int applicationId,
        int customerClubId);
    /// <summary>
    /// جدید ترین محصولات مرکز را میگرداند
    /// </summary>
    /// <param name="customerClubId">باشگاه مشتریان</param>
    /// <param name="applicationId"></param>
    /// <returns></returns>
    IQueryable<CompanyProductSalesPlanningDomainModel> GetNewest(int applicationId,
        int customerClubId);

    /// <summary>
    /// ظرفیت باقیمانده برنامه ریزی محصول را به روز رسانی می کند
    /// </summary>
    /// <param name="productSalesPlanningId">شناسه برنامه ریزی محصول</param>
    /// <param name="applicationId">شناسه برنامه</param>
    Task<int> UpdateRemainingCapacity(int productSalesPlanningId, int applicationId);

    /// <summary>
    /// آیا درصد تخفیف نسبت به قرارداد تغییر نکرده است؟
    /// </summary>
    /// <param name="productSalesPlanningId"></param>
    /// <returns></returns>
    Task<bool> HasDiscountPercentageChangedComparedToTheContractAsync(int productSalesPlanningId);

    /// <summary>
    /// آبا کاربر محصول با شناسه ارسالی شناسه را خریده است
    /// </summary>
    /// <param name="productSalesPlanningId">شناسه برنامه ریزی محصول</param>
    /// <returns></returns>
    Task<bool> CheckCurrentUserPurchasesByProductSalesPlanningIdAsync(int productSalesPlanningId);

    /// <summary>
    /// دریافت برنامه ریزی های دوره فعال براساس شناسه اپلیکیشن
    /// </summary>
    /// <param name="applicationId">شناسه اپلیکیشن</param>
    /// <param name="customerClubId">باشگاه مشتریان</param>
    /// <param name="readOnly"></param>
    /// <returns></returns>
    IQueryable<ProductSalesPlanning> GetIsActiveProductSalesPlanningsByApplicationId(int applicationId, int customerClubId, bool readOnly = true);

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
    Expression<Func<ProductSalesPlanning, bool>> GetProductSalePlanningIsActiveFilter(
        int? applicationId,
        int customerClubId,
        int? categoryGroupId,
        bool isEmploymentCategoryGroup
        );


    /// <summary>
    /// این متد برنامه ریزی دوره 
    /// productSalesPlanning
    /// مرتبط با 
    /// شناسه تگ های ارسالی را بر می گرداند
    /// </summary>
    /// <param name="tagIds">شناسه تگ ها</param>
    /// <returns>ProductSalesPlanning</returns>
    IQueryable<ProductSalesPlanning> GetProductSalesPlanningRelatedToTagId(List<int> tagIds);

    /// <summary>
    /// این متد جهت دریافت برنامه ریزی دوره بر اساس شناسه دسته بندی آزمون کاربرد دارد
    /// </summary>
    /// <param name="examCategoryId">شناسه دسته بندی آزمون</param>
    /// <returns>ProductSalesPlanning</returns>
    IQueryable<ProductSalesPlanning> GetProductSalesPlanningByExamCategoryId(int examCategoryId);

    /// <summary>
    /// این متد مقدار حذف برنامه ریزی دوره را برابر با صحیح می کند. رکورد در دریتابیس موجود است
    /// </summary>
    /// <param name="productSalesPlanningId"></param>
    /// <returns></returns>
    Task<int> ArchiveAsync(int productSalesPlanningId);

    /// <summary>
    ///  می کند false این متد مقدار حذف برنامه ریزی دوره را برابر با  
    /// </summary>
    /// <param name="productSalesPlanningId"></param>
    /// <returns></returns>
    Task<int> RestoreFromArchiveAsync(int productSalesPlanningId);

    /// <summary>
    /// برنامه ریزی دوره های یک تنظیم فروش را پیدا می کند
    /// </summary>
    /// <param name="saleConfigurationId">شناسه برنامه ریزی دوره</param>
    /// <param name="readOnly">فقظ خواندنی</param>
    /// <returns></returns>
    IQueryable<ProductSalesPlanning> FindBySaleConfigurationId(int saleConfigurationId,bool readOnly = true);
}