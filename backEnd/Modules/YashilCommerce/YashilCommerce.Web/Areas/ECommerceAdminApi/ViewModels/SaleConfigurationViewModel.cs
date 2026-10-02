using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class SaleConfigurationListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        public bool IsActive { get; set; }
        public bool AllowSetInstallmentInProduct { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        public bool AllowSetTaxInProduct { get; set; }

        public bool IsTaxExempt { get; set; }

        [StringValidation] public string TaxCategoryTitle { get; set; }

        public string InstallmentExpression { get; set; }

        public int InstallmentCount { get; set; }

        public bool ExpressionBasedInstallmentCount { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

        public decimal DiscountPercentage { get; set; }
        [StringValidation] public string FactorReportTypeTitle { get; set; }
        [StringValidation] public string FounderMobileNumber { get; set; }
        [StringValidation] public string FounderPhoneNumber { get; set; }
        public int LicenseExpireDate { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
        public bool IsGroupedByTaxCategory { get; set; }

        /// <summary>
        /// آیا بیمه شامل همه محصولات می باشد
        /// </summary>
        public bool AllProductsIncludeInsurance { get; set; }

        /// <summary>
        /// آیا هزینه سایر کسورات (هزینه تبلیغات )در فاکتور تامین کننده آورده شود
        /// </summary>
        public bool IsOtherDeductionsIncludeInSupplierInvoice { get; set; }
        /// <summary>
        /// نیاز به ویرایش فاکتور بعد از تولید
        ///</summary>
        public bool NeedModifyAfterGenerateFactor { get; set; }
        /// <summary>
        /// اجبار به تولید فاکتور بر اساس شناسه باشگاه مشتریان
        ///</summary>
        public bool ForcedToGenerateFactorByCustomerClubId { get; set; }
        /// <summary>
        ///  فیلتر کردن بر اساس شناسه محصول در هنگام صدور فاکتور
        /// </summary>
        public bool AllowFilterByProductSalesPlanningIdInGenerateFactor { get; set; }

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

    public class SaleConfigurationEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)][Required][StringValidation] public string Title { get; set; }
        public bool IsActive { get; set; }

        [Range(0, int.MaxValue)][Required] public int FromDate { get; set; }

        [Range(0, int.MaxValue)][Required] public int ToDate { get; set; }

        [Range(0, int.MaxValue)][Required] public int LicenseExpireDate { get; set; }

        public int? RepresentationId { get; set; }
        [Range(0, int.MaxValue)][Required] public int CompanyId { get; set; }
        public bool AllowSetTaxInProduct { get; set; }
        public bool IsTaxExempt { get; set; }
        [Range(0, int.MaxValue)][Required] public int TaxCategoryId { get; set; }
        [Range(0, int.MaxValue)][Required] public int FounderId { get; set; }


        [Range(0, int.MaxValue)][Required] public decimal InsurancePercentage { get; set; }


        public string Description { get; set; }
        public int AccessLevelId { get; set; }
        [Required] public int FactorReportType { get; set; }

        /// <summary>
        /// زمانبندی صدور فاکتور
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int YashilSchedulerId { get; set; }

        /// <summary>
        /// اطلاعات حساب شخص جهت پرداختی های فاکتور و فاکتور
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int PersonBankAccountId { get; set; }

        /// <summary>
        /// دارای گواهی ارزش افزوده
        /// </summary>
        public bool HasTaxCertificate { get; set; }
        /// <summary>
        /// پیش عنوان نام محصول
        /// </summary>
        [StringValidation] public string FactorItemPrefix { get; set; }

        /// <summary>
        /// اجبار به صدور فاکتور با دسته بندی مالیاتی
        /// </summary>
        public bool? ForcedToGenerateFactorByTaxCategory { get; set; }

        public bool IsGroupedByTaxCategory { get; set; }

        /// <summary>
        /// آیا بیمه شامل همه محصولات می باشد
        /// </summary>
        public bool AllProductsIncludeInsurance { get; set; }

        /// <summary>
        /// آیا هزینه سایر کسورات (هزینه تبلیغات )در فاکتور تامین کننده آورده شود
        /// </summary>
        public bool IsOtherDeductionsIncludeInSupplierInvoice { get; set; }

        /// <summary>
        ///عنوان دسته با مالیات برای فاکتور
        /// </summary>
        [StringLength(300)]
        [StringValidation]
        public string TaxTitleForFactor { get; set; }

        /// <summary>
        /// کد دسته با مایلات برای فاکتور
        /// </summary>
        [StringLength(150)]
        [StringValidation] public string TaxCodeForFactor { get; set; }

        /// <summary>
        /// واحد سنجش دسته با مالیات برای فاکتور
        /// </summary>
        [StringLength(150)]
        [StringValidation]
        public string TaxUnitOfMeasurementForFactor { get; set; }
        /// <summary>
        /// عنوان دسته معاف از مالیات برای فاکتور
        /// </summary>
        [StringLength(300)]
        [StringValidation]
        public string TaxExemptTitleForFactor { get; set; }
        /// <summary>
        /// کد دسته معاف از مالیات برای فاکتور
        /// </summary>
        [StringLength(150)]
        [StringValidation] public string TaxExemptCodeForFactor { get; set; }
        /// <summary>
        /// واحد سنجش دسته معاف از مالیات برای فاکتور
        /// </summary>
        [StringLength(150)]
        [StringValidation]
        public string TaxExemptUnitOfMeasurementForFactor { get; set; }
        /// <summary>
        /// نیاز به ویرایش فاکتور بعد از تولید
        ///</summary>
        public bool NeedModifyAfterGenerateFactor { get; set; }
        /// <summary>
        /// اجبار به تولید فاکتور بر اساس شناسه باشگاه مشتریان
        ///</summary>
        public bool ForcedToGenerateFactorByCustomerClubId { get; set; }
        /// <summary>
        /// اجازه فیلتر کردن بر اساس شناسه محصول در هنگام صدور فاکتور
        /// </summary>
        public bool AllowFilterByProductSalesPlanningIdInGenerateFactor { get; set; }
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
        /// استفاده از عنوان گروه محصول برای کانورت
        /// </summary>
        public bool UseProductGroupForConvert { get; set; }
        /// <summary>
        /// استفاده از عنوان گروه محصول برای فاکتور
        /// </summary>
        public bool UseProductGroupForFactor { get; set; }
        /// <summary>
        /// حداقل مبغ خرید
        ///</summary>
        public decimal? MinimumPurchasePrice { get; set; }
        /// <summary>
        /// فرمول محاسبه هزینه ارسال
        /// </summary>
        [CanBeNull]
        public string AdditionalShippingChargeExpression { get; set; }

    }

    public class SaleConfigurationSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

    public class SaleConfigurationCustomViewModel
    {
        public int? Id { get; set; }

        /// <summary>
        /// درصد تخفیف مربوط به قرارداد 
        /// </summary>
        public decimal DiscountPercentage { get; set; }
    }
    /// <summary>
    /// مدل مربوط به دریافت تنظیم فروش فعال
    /// </summary>
    public class ActiveSaleConfigurationEditModel
    {
        /// <summary>
        /// مرکز
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// نمایندگی
        /// </summary>
        public int? RepresentationId { get; set; }
    }
}