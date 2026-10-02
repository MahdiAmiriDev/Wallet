using System.Collections.Generic;

namespace Yashil.Core.CustomViewModels.ECommerce
{
    public class ProductSalePlanningViewModel
    {
        public int ProductId { get; set; }
        public int ProductSalePlanningId { get; set; }
        public string Title { get; set; }
        public string CompanyTitle { get; set; }
        public string? CompanyShortName { get; set; }
        public string CategoryTitle { get; set; }
        public string RepresentationTitle { get; set; }
        public string RepresentationShortName { get; set; }
        public int CompanyId { get; set; }
        public decimal BasePrice { get; set; }
        public string CityTitle { get; set; }
        public string StateTitle { get; set; }
       
        public string? Duration { get; set; }
        public string EndDateString { get; set; }
        public string StartDateString { get; set; }

        // TODO:Check Calc Price FILL  In Loop ***
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public bool SubsidyBased { get; set; }
        public int? MaxSataSubsidy { get; set; }
        public int? MaxVezaratSubsidy { get; set; }
        public SortedList<string, string> Properties { get; set; }
        public string CoursePlanningSchedule { get; set; }
        public int? EndDate { get; set; }
        /// <summary>
        ///         /// قیمت
        ///</summary>
        public int? StartDate { get; set; }
        public int? InstallmentCount { get; set; }
        /// <summary>
        ///         /// روش اجرای دوره
        ///</summary>
        public int RunType { get; set; }
        /// <summary>
        ///         /// نوع برگزاری
        ///</summary>
        public int? ImplementationType { get; set; }
        /// <summary>
        ///         /// غیر فعال کردن دکمه خرید
        ///</summary>
        public bool DisableBuyButton { get; set; }

        public string ModificationDate { get; set; }
        /// <summary>
        /// آیا از نوع اشتغال است
        /// </summary>
        public bool IsEmployment { get; set; }
        /// <summary>
        /// سقف یارانه اشتغال ساتا برای این محصول
        /// </summary>
        public int? MaxEmploymentSataSubsidy { get; set; }
        /// <summary>
        /// سقف یارانه اشتغال وزارت برای این محصول
        /// </summary>
        public int? MaxEmploymentVezaratSubsidy { get; set; }

        /// <summary>
        /// تعداد تگ های مرتبط با برنامه ریزی محصول
        /// در کامپوننت 
        /// RelatedProduct
        /// کاربرد دارد
        /// </summary>
        public int? RelatedTagCount { get; set; }

    }


    /// <summary>
    /// مدل مورد استفاده برای زمانی که برنامه ریزی های دوره ها را
    /// بر اساس شناسه مرکز مربوطه گروه بندی کرده ایم
    /// در ProductSalePlanningFactoryViewComponent
    /// کاربرد دارد
    /// </summary>
    public class ProductSalePlanningGroupedByCompanyViewModel
    {
        public List<ProductSalePlanningViewModel> GroupedProductSalesPlanning { get; set; }
    }
}
