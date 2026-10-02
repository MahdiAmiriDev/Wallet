using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomViewModels.Tms;

namespace Yashil.Core.CustomViewModels.ECommerce
{
    public class ProductListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public bool IsGroupedProduct { get; set; }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        public bool IsFreeShipping { get; set; }

        public bool IsGroupProduct { get; set; }

        public bool AllowMultiVendor { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        [StringValidation] public string AppEntityTitle { get; set; }

        public bool VisibleIndividually { get; set; }

        [StringValidation] public string ProductTemplateTitle { get; set; }


        [StringValidation] public string MetaKeywords { get; set; }

        [StringValidation] public string MetaTitle { get; set; }

        public bool AllowCustomerReviews { get; set; }

        public int ApprovedRatingSum { get; set; }

        public int NotApprovedRatingSum { get; set; }

        public bool SubjectToAcl { get; set; }

        public bool LimitedToStores { get; set; }

        [StringValidation] public string GTIN { get; set; }

        [StringValidation] public string ManufacturerPartNumber { get; set; }

        public bool IsGiftCard { get; set; }

        [StringValidation] public string GiftCardTypeTitle { get; set; }

        public decimal? OverriddenGiftCardAmount { get; set; }

        public bool RequireOtherProducts { get; set; }

        [StringValidation] public string RequiredProductIds { get; set; }

        public bool AutomaticallyAddRequiredProducts { get; set; }

        public bool IsDownload { get; set; }

        public int? DownloadId { get; set; }

        public bool UnlimitedDownloads { get; set; }

        public int MaxNumberOfDownloads { get; set; }

        public int? DownloadExpirationDays { get; set; }

        [StringValidation] public string DownloadActivationTypeTitle { get; set; }

        public bool HasSampleDownload { get; set; }

        public int? SampleDownloadId { get; set; }

        public bool HasUserAgreement { get; set; }

        public bool IsRecurring { get; set; }

        public int RecurringCycleLength { get; set; }

        [StringValidation] public string RecurringCyclePeriodTypeTitle { get; set; }

        public int RecurringTotalCycles { get; set; }

        public bool IsRental { get; set; }

        public int RentalPriceLength { get; set; }

        public int? RentalPricePeriodId { get; set; }

        public bool IsShipEnabled { get; set; }

        public bool ShipSeparately { get; set; }

        public bool IsTaxExempt { get; set; }

        [StringValidation] public string TaxCategoryTitle { get; set; }

        public bool IsTelecommunicationsOrBroadcastingOrElectronicServices { get; set; }

        public int? MinStockQuantity { get; set; }

        public int? LowStockActivityId { get; set; }

        public int? NotifyAdminForQuantityBelow { get; set; }

        public int? BackorderModeId { get; set; }

        public bool AllowBackInStockSubscriptions { get; set; }



        public bool UseMultipleWarehouses { get; set; }

        [StringValidation] public string AllowedQuantities { get; set; }

        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal? BasePriceAmount { get; set; }

        [StringValidation] public string BasePriceUnitTypeTitle { get; set; }

        public decimal? BasePriceBaseAmount { get; set; }

        [StringValidation] public string BasePriceBaseUnitTypeTitle { get; set; }

        public bool MarkAsNew { get; set; }

        public bool HasTierPrices { get; set; }

        public bool HasDiscountsApplied { get; set; }

        public decimal Weight { get; set; }

        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public DateTime? AvailableStartDate { get; set; }

        public DateTime? AvailableEndDate { get; set; }

        public int DisplayOrder { get; set; }

        public bool Published { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

        /// <summary>
        ///         /// حذف شده
        ///</summary>
        public bool Deleted { get; set; }
        [StringValidation] public string CategoryTitle { get; set; }
        public int? Duration { get; set; }
        public int? CourseId { get; set; }
        /// <summary>
        ///  ضریب سوبسید وزارت
        /// </summary>
        public decimal VezaratSubsidyFactor { get; set; }
        /// <summary>
        ///   ضریب سوبسید ساتا
        ///</summary>
        public decimal SataSubsidyFactor { get; set; }
        /// <summary>
        /// نوع محصول
        /// </summary>
        public string ProductTypeTitle { get; set; }
        /// <summary>
        /// نیاز به تایید تامین کننده قبل از پرداخت کاربر
        /// </summary>
        public bool RequireSupplierApprovalBeforePayment { get; set; }
        /// <summary>
        /// متن دکمه ای در صفحه پیش فاکتور جهت تایید توسط تامین کننده به کاربر نمایش داده می شود  
        /// </summary>
        public string ButtonTextRequireSupplierApprovalBeforePayment { get; set; }
        /// <summary>
        /// متن پیام به کاربر جهت تایید تامین کننده قبل از پرداخت  
        /// </summary>
        public string MessageToUserInRequireSupplierApprovalBeforePayment { get; set; }

    }

    public class ProductSimpleListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        public bool IsFreeShipping { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }
        [StringValidation] public string CategoryTitle { get; set; }
        public bool SalesAtAllRepresentations { get; set; }
        public bool SalesAtSelectedRepresentations { get; set; }
        public bool? AllowSelectByRepresentation { get; set; }
        public int SalesModelType { get; set; }

        public bool? AllowSetPriceByRepresentation { get; set; }
        public bool? AllowManageInventoryByRepresentation { get; set; }
        public bool? AllowManageDiscountByRepresentation { get; set; }
        public int? CourseId { get; set; }
        public int? Duration { get; set; }
    }

    public class ProductWithCourseSimpleListViewModel
    {
        public int Id { get; set; }
        [StringValidation] public string Code { get; set; }
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }
        [StringValidation] public string CategoryTitle { get; set; }
        public int? Duration { get; set; }
        public int? CourseId { get; set; }

    }

    public class ProductEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? SpecificationCategoryId { get; set; }
        public int Id { get; set; }

        public bool SalesAtAllRepresentations { get; set; }
        public bool SalesAtSelectedRepresentations { get; set; }
        public bool? AllowSelectByRepresentation { get; set; }

        public int SalesModelType { get; set; }
        [StringValidation(ErrorMessage = "Html Injection Error")]
        public string SalesModelTypeTitle { get; set; }


        public bool? AllowSetPriceByRepresentation { get; set; }
        public bool? AllowManageInventoryByRepresentation { get; set; }
        public bool? AllowManageDiscountByRepresentation { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)]
        [StringValidation]
        [Required]
        public string Title { get; set; }
        public bool IsFreeShipping { get; set; }
        public bool IsGroupProduct { get; set; }
        public bool AllowMultiVendor { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int CompanyId { get; set; }
        [Range(0, int.MaxValue)] public int? AppEntityId { get; set; }
        public bool VisibleIndividually { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        [StringValidation]
        public string AdminComment { get; set; }
        public int? ProductTemplateId { get; set; }
        public bool ShowOnHomepage { get; set; }
        [StringLength(800)]
        [StringValidation]
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        [StringLength(800)][StringValidation] public string MetaTitle { get; set; }
        public bool AllowCustomerReviews { get; set; }
        [Range(0, int.MaxValue)][Required] public int ApprovedRatingSum { get; set; }
        [Range(0, int.MaxValue)][Required] public int NotApprovedRatingSum { get; set; }
        public bool SubjectToAcl { get; set; }
        public bool LimitedToStores { get; set; }
        [StringLength(200)][StringValidation] public string GTIN { get; set; }
        [StringLength(800)][StringValidation] public string ManufacturerPartNumber { get; set; }
        public bool IsGiftCard { get; set; }
        public int? GiftCardTypeId { get; set; }
        public decimal? OverriddenGiftCardAmount { get; set; }
        public bool RequireOtherProducts { get; set; }
        [StringLength(2000)][StringValidation] public string RequiredProductIds { get; set; }
        public bool AutomaticallyAddRequiredProducts { get; set; }
        public bool IsDownload { get; set; }
        public int? DownloadId { get; set; }
        public bool UnlimitedDownloads { get; set; }
        [Range(0, int.MaxValue)][Required] public int MaxNumberOfDownloads { get; set; }
        public int? DownloadExpirationDays { get; set; }
        public int? DownloadActivationType { get; set; }
        public bool HasSampleDownload { get; set; }
        public int? SampleDownloadId { get; set; }
        public bool HasUserAgreement { get; set; }
        [StringValidation] public string UserAgreementText { get; set; }
        public bool IsRecurring { get; set; }
        [Range(0, int.MaxValue)][Required] public int RecurringCycleLength { get; set; }
        public int? RecurringCyclePeriodType { get; set; }
        [Range(0, int.MaxValue)][Required] public int RecurringTotalCycles { get; set; }
        public bool IsRental { get; set; }
        [Range(0, int.MaxValue)][Required] public int RentalPriceLength { get; set; }
        public int? RentalPricePeriodId { get; set; }
        public bool IsShipEnabled { get; set; }
        public bool ShipSeparately { get; set; }
        public bool IsTaxExempt { get; set; }
        [Range(0, int.MaxValue)][Required] public int TaxCategoryId { get; set; }
        public bool IsTelecommunicationsOrBroadcastingOrElectronicServices { get; set; }
        public int? MinStockQuantity { get; set; }
        public int? LowStockActivityId { get; set; }
        public int? NotifyAdminForQuantityBelow { get; set; }
        public int? BackorderModeId { get; set; }
        public bool AllowBackInStockSubscriptions { get; set; }
        public bool DisplayStockQuantity { get; set; }
        public bool UseMultipleWarehouses { get; set; }
        public bool DisplayStockAvailability { get; set; }
        [Range(0, int.MaxValue)][Required] public int OrderMinimumQuantity { get; set; }
        [Range(0, int.MaxValue)][Required] public int OrderMaximumQuantity { get; set; }
        [StringLength(2000)]
        [StringValidation]
        public string AllowedQuantities { get; set; }
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }
        public bool NotReturnable { get; set; }
        public bool DisableBuyButton { get; set; }
        public bool DisableWishlistButton { get; set; }
        public bool AvailableForPreOrder { get; set; }
        public bool CallForPrice { get; set; }

        [Required] public decimal MinPrice { get; set; }

        [Required] public decimal MaxPrice { get; set; }
        public bool CustomerEntersPrice { get; set; }

        [Required] public decimal MinimumCustomerEnteredPrice { get; set; }

        [Required] public decimal MaximumCustomerEnteredPrice { get; set; }
        public bool BasePriceEnabled { get; set; }
        public decimal? BasePriceAmount { get; set; }
        public int? BasePriceUnitType { get; set; }
        public decimal? BasePriceBaseAmount { get; set; }
        public int? BasePriceBaseUnitType { get; set; }

        public bool HasTierPrices { get; set; }
        public bool HasDiscountsApplied { get; set; }

        [Required] public decimal Weight { get; set; }

        [Required] public decimal Length { get; set; }

        [Required] public decimal Width { get; set; }

        public DateTime? AvailableEndDate { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int DisplayOrder { get; set; }
        public bool Published { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int AccessLevelId { get; set; }
        public List<int> Categories { get; set; }
        public bool SalesByAllRepresentations { get; set; }
        public bool SalesBySelectedRepresentations { get; set; }
        public CourseEditModel Course { get; set; }
        public int? CourseId { get; set; }
        public int? Duration { get; set; }

        /// <summary>
        ///  ضریب سوبسید وزارت
        /// </summary>
        public decimal VezaratSubsidyFactor { get; set; }
        /// <summary>
        ///   ضریب سوبسید ساتا
        ///</summary>
        public decimal SataSubsidyFactor { get; set; }

        public int DefaultCategoryId { get; set; }
        /// <summary>
        /// نوع محصول میتواند اشتغال یا سایر باشد به طور پیش فرض سایر است
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// شناسه دسته بندی محصول
        /// </summary>
        public int CategoryGroupId { get; set; }
        /// <summary>
        /// برای دریافت محصول نیاز به ارجاع به سایت مرکز می باشد؟
        ///</summary>
        public bool? NeedRedirectToCompanySiteForReceive { get; set; }
        /// <summary>
        /// نیاز به تایید تامین کننده قبل از پرداخت کاربر
        /// </summary>
        public bool RequireSupplierApprovalBeforePayment { get; set; }
        /// <summary>
        /// متن دکمه ای در صفحه پیش فاکتور جهت تایید توسط تامین کننده به کاربر نمایش داده می شود  
        /// </summary>
        public string ButtonTextRequireSupplierApprovalBeforePayment { get; set; }
        /// <summary>
        /// متن پیام به کاربر جهت تایید تامین کننده قبل از پرداخت  
        /// </summary>
        public string MessageToUserInRequireSupplierApprovalBeforePayment { get; set; }

        /// <summary>
        /// شناسه گروه محصول
        /// </summary>
        public int? ProductGroupId { get; set; }

        /// <summary>
        /// شناسه گروه محصول شامل مالیات
        /// </summary>
        public int? ProductGroupIdIncludeTax { get; set; }

    }

    public class ProductSimpleEditModel
    {
        public int Id { get; set; }
    }

    public class ProductSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        //شناسه دوره
        public int Id { get; set; }
        //عنوان دوره
        [StringValidation] public string Title { get; set; }
    }

    public class ProductInfoViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringValidation] public string Code { get; set; }
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }
        public int CompanyId { get; set; }
        [StringValidation] public string CategoryTitle { get; set; }
        public int Categories { get; set; }
        public int? SkillType { get; set; }
        [StringValidation] public string SkillTypeTitle { get; set; }
        public int? CertificateType { get; set; }
        [StringValidation] public string CertificateTypeTitle { get; set; }
        public int? EvaluationMethod { get; set; }
        [StringValidation] public string EvaluationMethodTitle { get; set; }
        public int? Duration { get; set; }
        [StringValidation] public string ShortDescription { get; set; }
        [StringValidation] public string FullDescription { get; set; }
        /// <summary>
        /// نوع فروش
        /// </summary>
        public int SalesModelType { get; set; }
    }
}