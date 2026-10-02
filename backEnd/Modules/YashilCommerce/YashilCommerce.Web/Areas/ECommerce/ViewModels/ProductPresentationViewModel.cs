using System.Collections.Generic;
using JetBrains.Annotations;
using Yashil.Core.CustomViewModels;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class ProductPresentationViewModel
    {
        public ProductPresentationViewModel()
        {
            CityListModelViewModel = new List<CityTitleOnlyViewModel>();
            ProductSalePlanningPresentationViewModel = new ProductSalePlanningPresentationViewModel();
        }
        public List<CityTitleOnlyViewModel> CityListModelViewModel { get; set; }
        public ProductSalePlanningPresentationViewModel ProductSalePlanningPresentationViewModel { get; set; }
        public bool CourseBased { get; set; }
        public bool SubsidyBased { get; internal set; }
        /// <summary>
        /// مدل تعلق گرفتن یارانه اشتغال به کاربر و دوره
        /// </summary>
        [CanBeNull]
        public EmploymentSubsidyDto EmploymentSubsidyDto { get; set; }

        /// <summary>
        /// تگ های مرتبط محصولات
        /// </summary>
        public List<int> RelatedTagIds { get; set; }

        /// <summary>
        ///         /// جزییات تبلیغات سریع محصول های ابن دسته بندی
        ///</summary>
        public string ProductQuickPromoDetail { get; set; }

        /// <summary>
        /// آیا خرید برنامه ریزی دوره غیر فعال است؟
        /// </summary>
        public bool IsProductSalesPlanningActive { get; set; }
    }
    public class ProductSalePlanningPresentationViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public int CompanyId { get; set; }
        public string CompanyTitle { get; set; }
        public string ProductTitle { get; set; }
        public int? RepresentationId { get; set; }
        public string RepresentationTitle { get; set; }
        public string FullAddress { get; set; }
        public bool? IsEncryptedAddress { get; set; }
        public string Phone { get; set; }
        public decimal? Price { get; set; }
        public bool IsFreeShipping { get; set; }
        public decimal? OldPrice { get; set; }
        public int StockQuantity { get; set; }
        public decimal? CalcedPrice { get; set; }
        /// <summary>
        /// سقف یارانه ساتا برای این محصول
        /// </summary>
        public int? MaxSataSubsidy { get; set; }
        /// <summary>
        /// سقف یارانه وزارت برای این محصول
        /// </summary>
        public int? MaxVezaratSubsidy { get; set; }
        public decimal? Discount => Price - CalcedPrice;
        /// <summary>
        /// توضیحات کوتاه محصول
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// توضیحات کامل محصول
        /// </summary>
        public string FullDescription { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string ParentCategoryTitle { get; internal set; }
        /// <summary>
        ///         /// غیر فعال کردن دکمه خرید
        ///</summary>
        public bool DisableBuyButton { get; set; }
        /// <summary>
        /// توضیحات برنامه ریزی محصول
        /// </summary>
        public string Description { get; set; }

        public string ExternalLink { get; set; }
        /// <summary>
        /// ظرفیت باقی مانده دوره
        /// </summary>
        public int? RemainingCapacity { get; set; }

        public string ModificationDate { get; set; }
        /// <summary>
        /// درصد تخفیف
        /// </summary>
        // TODO Fix This Remove From ProductSalePlanning ******
        public decimal? DiscountPercentage { get; set; }

        public int  SaleConfigurationId { get; set; }
        /// <summary>
        /// آیا پیش ثبت نام است
        /// </summary>
        public bool IsPreRegistration { get; set; }
        /// <summary>
        /// آیا از نوع اشتغال است
        /// </summary>
        public bool IsEmployement { get; set; }
        /// <summary>
        /// آیا کاربر جاری رزومه را کامل پر کرده است
        /// </summary>
        public bool IsResumeFillOutEnough { get; set; }

        /// <summary>
        /// سقف یارانه اشتغال ساتا برای این محصول
        /// </summary>
        public int? MaxEmploymentSataSubsidy { get; set; }
        /// <summary>
        /// سقف یارانه اشتغال وزارت برای این محصول
        /// </summary>
        public int? MaxEmploymentVezaratSubsidy { get; set; }
    }

}
