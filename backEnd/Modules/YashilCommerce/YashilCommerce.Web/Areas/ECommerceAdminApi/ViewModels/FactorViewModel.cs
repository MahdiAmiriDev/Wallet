using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class FactorListViewModel : IBaseViewModel
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

        [StringValidation] public string TaxCategoryTitle { get; set; }


        [StringValidation] public string CompanyTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        public int IssueDate { get; set; }

        [StringValidation] public string FactorReviewStatusTypeTitle { get; set; }
        public int FactorReviewStatusType { get; set; }

        public int FactorReportType { get; set; }

        public bool AllowDelete { get; set; }
        [StringValidation] public string SaleConfigurationTitle { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// showInitialOrRejectStateButton
        /// </summary>
        public bool AllowInitialOrRejectState => FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload || FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Rejected;


        /// <summary>
        /// showFactorVerifierBtn
        /// </summary>
        public bool AllowFactorVerifier => (FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload
                                            && FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForConfirmAdmin);

        /// <summary>
        /// showPaymentBtn
        /// </summary>
        public bool AllowPayment => (FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed);

        public bool IsGroupedByTaxCategory { get; set; }

        /// <summary>
        /// تعداد اقساط فاکتور
        /// </summary>
        public int FactorInstallmentCount { get; set; }

    }
    public class FactorListForFactorGroupListViewModel : IBaseViewModel
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

        [StringValidation] public string TaxCategoryTitle { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        public int IssueDate { get; set; }

        [StringValidation] public string FactorReviewStatusTypeTitle { get; set; }
        public int FactorReviewStatusType { get; set; }

        public int FactorReportType { get; set; }

        public bool AllowRemoveFromGroup { get; set; }
        [StringValidation] public string SaleConfigurationTitle { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsGroupedByTaxCategory { get; set; }
        /// <summary>
        /// عنوان دسته بندی فاکتور
        /// </summary>
        public string CategoryGroupTitle { get; set; }
    }

    /// <summary>
    /// نمایش گزارش فاکتور  در فرم
    /// </summary>
    public class FactorReportSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string TaxCategoryTitle { get; set; }

        public int CompanyId { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        public int RepresentationId { get; set; }
        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string PaymentType { get; set; }


        public decimal Price { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmountInclTax { get; set; }

        public decimal? DiscountAmountExclTax { get; set; }

        public decimal PriceIncDiscount { get; set; }

        public decimal WagePercentage { get; set; }

        public decimal WageAmountIncTax { get; set; }

        public decimal WageAmountExcTax { get; set; }

        public decimal SubsidyPrice { get; set; }

        public decimal SubsidyProfitPercentage { get; set; }

        public decimal SubsidyPriceIncProfit { get; set; }

        public decimal SubsidyPriceIncTaxIncProfit { get; set; }

        public decimal TaxPercentage { get; set; }

        public decimal TaxPrice { get; set; }

        public int FactorItemCount { get; set; }

        public decimal SumTotalInclSubsidy { get; set; }
        public decimal SumPriceInclDiscount { get; set; }
        public DateTime? FactorConfirmDate { get; set; }
        public DateTime? FactorGroupConfirmDate { get; set; }

        public int FirstFactorItemDateInt { get; set; }
        public int LastFactorItemDateInt { get; set; }

        public int IssueDate { get; set; }

        [StringValidation] public string FactorReviewStatusTypeTitle { get; set; }
        public int FactorReviewStatusType { get; set; }

        public int FactorReportType { get; set; }

        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        [StringValidation] public string SaleConfigurationTitle { get; set; }

    }

    /// <summary>
    /// برای لیست همه فاکتورهای ناظر
    /// فرمی که در آن همه فاکتور ها طبق فیلتر مرکز و نمایندگی قابل مشاهده است
    /// </summary>
    public class SuperVisorFactorListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }


        [StringValidation] public string OrderTitle { get; set; }
        [StringValidation] public string TaxCategoryTitle { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        public int IssueDate { get; set; }

        [StringValidation] public string FactorReviewStatusTypeTitle { get; set; }

        public int FactorReviewStatusType { get; set; }

        public bool AllowDelete { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        [StringValidation] public string SaleConfigurationTitle { get; set; }
        /// <summary>
        /// نام گروه بندی فاکتور
        /// اگر فاکتور گروه بندی شده باشد
        /// نام گروه فاکتور و گروه پدر گروه را نشان میدهیم
        /// </summary>
        [StringValidation] public string FactorGroupTitle { get; set; }
        /// <summary>
        /// آیا فاکتور گروه بندی شده است؟
        /// </summary>
        public bool IsGroupedByFactorGroup { get; set; }
        /// <summary>
        /// نمایش یا عدم نمایش دکمه تایید کنندگان فاکتور
        /// </summary>

        public bool AllowFactorVerifierBtn => (FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForConfirmAdmin
                                               && FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload);
        /// <summary>
        /// نمایش یا عدم نمایش دکمه پرداختی های فاکتور
        /// </summary>

        public bool AllowPaymentBtn => FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed;
        /// <summary>
        /// showFactorVerifierDescriptionBtn
        /// </summary>

        /// <summary>
        /// showFactorVerifierBtn
        /// </summary>
        public bool AllowFactorVerifier => (FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload &&
                                            FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForConfirmAdmin);
        /// <summary>
        /// اجازه نمایش فایل آپلودی
        /// </summary>
        public bool AllowViewUploadedReport => (FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload &&
                                                FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForConfirmAdmin);
        /// <summary>
        /// اگر فاکتور در وضعیت ثبت اولیه باشد می توان اطلاعات فروشنده فاکتور را تغییر داد
        /// </summary>
        public bool AllowModifyFactorInfo => (FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.WaitingForConfirmAdmin);

        public bool IsGroupedByTaxCategory { get; set; }
        /// <summary>
        /// عنوان باشگاه مشتریان
        /// </summary>
        public string CustomerClubTitle { get; set; }
        /// <summary>
        /// گروه دسته بندی
        /// </summary>
        public string CategoryGroupTitle { get; set; }

    }
    public class FactorEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)][StringValidation] public string Title { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int TaxCategoryId { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int CompanyId { get; set; }
        public int? RepresentationId { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int IssueDate { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int FactorReviewStatusType { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int AccessLevelId { get; set; }

        public bool IsGroupedByTaxCategory { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        [StringValidation] public string FullAddress { get; set; }
        /// <summary>
        /// شماره تلفن
        /// </summary>
        [StringValidation] public string Telephone { get; set; }
        /// <summary>
        /// دور نگار
        /// </summary>
        [StringValidation] public string FaxNumber { get; set; }
        /// <summary>
        /// کد اقتصادی
        /// </summary>
        [StringValidation] public string EconomicCode { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// شناسه اطلاعات حساب  تامین کننده برای پرداخت فاکتور
        /// </summary>
        public int? PersonBankAccountId { get; set; }
        /// <summary>
        /// شناسه برنامه ریزی دوره
        /// </summary>
        public int[] ProductSalesPlanningIds { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FactorInformationEditModel
    {
        public int Id { get; set; }
        /// <summary>
        /// آدرس
        /// </summary>
        [Required]
        [StringValidation] public string? FullAddress { get; set; }
        /// <summary>
        /// شماره تلفن
        /// </summary>
        [Required]
        [StringValidation] public string? Telephone { get; set; }
        /// <summary>
        /// دور نگار
        /// </summary>
        [StringValidation] public string? FaxNumber { get; set; }
        /// <summary>
        /// کد اقتصادی
        /// </summary>
        [StringValidation] public string? EconomicCode { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string? NationalCode { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        [Required]
        [StringValidation] public string CompanyTitle { get; set; }

        /// <summary>
        /// شناسه اطلاعات حساب  تامین کننده برای پرداخت فاکتور
        /// </summary>
        [Required]
        public int? PersonBankAccountId { get; set; }



    }

    public class FactorSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }
    /// <summary>
    /// برای نمایش فاکتورهای گروه بندی شده
    /// </summary>
    public class GroupedFactorListViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }
        [StringValidation] public string FactorGroupTitle { get; set; }
        [StringValidation] public string FactorGroupParentTitle { get; set; }
        public int? FactorGroupParentId { get; set; }
        public int FactorGroupId { get; set; }
        [StringValidation] public string TaxCategoryTitle { get; set; }

        public int CompanyId { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }

        public int RepresentationId { get; set; }
        [StringValidation] public string RepresentationTitle { get; set; }

        public int IssueDate { get; set; }
        public DateTime? FactorConfirmDate { get; set; }

        [StringValidation] public string FactorReviewStatusTypeTitle { get; set; }

        [StringValidation] public string SaleConfigurationTitle { get; set; }

    }
    /// <summary>
    /// نمایش فاکتورهای اساین شده(نشده) به یک گروه
    /// </summary>
    public class FactorAssignListViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        /// <summary>
        /// عنوان فاکتور
        /// </summary>
        [StringValidation] public string Title { get; set; }
        /// <summary>
        /// کد فاکتور
        /// </summary>
        [StringValidation] public string Code { get; set; }
        /// <summary>
        /// دسته بندی مالیاتی
        /// </summary>
        [StringValidation] public string TaxCategoryTitle { get; set; }
        /// <summary>
        /// درصد سود
        /// </summary>
        public decimal ProfitPercentage { get; set; }
        /// <summary>
        /// درصد تخفیف
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// عنوان گروه دسته بندی محصولات فاکتور
        /// </summary>
        public string CategoryGroupTitle { get; set; }

        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }

        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }

    }
    public class FactorStatisticViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// کد فاکتور
        /// </summary>
        [StringValidation] public string FactorCode { get; set; }
        /// <summary>
        ///         /// تاریخ صدور
        ///</summary>
        public int IssueDate { get; set; }
        /// <summary>
        /// درصد تخفیف
        /// </summary>
        public decimal? DiscountPercentage { get; set; }
        /// <summary>
        /// درصد سود
        /// </summary>
        public decimal? ProfitPercentage { get; set; }
        /// <summary>
        /// دسته بندی مالیاتی
        /// </summary>
        [StringValidation] public string TaxCategoryTitle { get; set; }
        /// <summary>
        /// قرارداد
        /// </summary>
        [StringValidation] public string SaleConfigurationTitle { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// نام نمایندگی
        /// </summary>
        [StringValidation] public string RepresentationTitle { get; set; }
        /// <summary>
        /// تعداد پرداختی ها
        /// </summary>
        public int PaidFactorInstallmentCount { get; set; }
        /// <summary>
        /// وضعیت بررسی فاکتور
        /// </summary>
        [StringValidation] public string FactorReviewStatusTypeTitle { get; set; }

        /// <summary>
        /// تعداد اقساط باقیمانده جهت تسویه فاکتور
        /// تعداد اقساط جهت تسویه فاکتور - تعداد اقساط پرداختی
        /// </summary>
        public int RemainingFactorInstallmentCount;
        /// <summary>
        /// تعداد اقساط جهت تسویه فاکتور
        /// </summary>
        public int FactorInstallmentCount { get; set; }

        /// <summary>
        /// مبلغ پرداخت شده به تامین کننده
        /// </summary>
        public decimal SumPaid { get; set; }
        /// <summary>
        /// درصد مالیات فاکتور
        /// </summary>
        public decimal FactorTaxPercentage { get; set; }

        /// <summary>
        /// مبلغ با تخفیف
        /// یا مبلغ خرید انستیتو
        /// </summary>
        public decimal CompanyPurchasePrice { get; set; }
        /// <summary>
        /// هزینه ارسال
        /// </summary>
        public decimal AdditionalShippingCharge { get; set; }
        /// <summary>
        ///درصد کسورات بیمه
        ///</summary>
        public decimal? InsurancePercentage { get; set; }
        /// <summary>
        /// مبلغ کسورات بیمه
        /// </summary>
        public decimal InsurancePrice { get; set; }
        /// <summary>
        /// دارای گواهی ارزش افزوده
        /// </summary>
        public bool HasTaxCertificate { get; set; }


        /// <summary>
        /// سایر کسورات
        ///</summary>
        public decimal? OtherDeductionsPercentage { get; set; }

        public decimal? OtherDeductionsPrice { get; set; }
        /// <summary>
        /// جمع کسورات فاکتور
        /// </summary>
        public decimal? SumFactorDeduction { get; set; }
        /// <summary>
        /// تعداد کسورات فاکتور
        /// </summary>
        public int FactorDeductionCount { get; set; }
        /// <summary>
        /// مبلغ باقی مانده
        /// </summary>
        public decimal SumRemaining { get; set; }

        /// <summary>
        /// شماره حساب بانکی
        /// </summary>
        [StringValidation] public string AccountNumber { get; set; }
        /// <summary>
        /// شماره شبا
        /// </summary>
        [StringValidation] public string ShebaNumber { get; set; }
        /// <summary>
        /// نام شعبه
        /// </summary>
        [CanBeNull]
        [StringValidation] public string BranchName { get; set; }
        /// <summary>
        /// نام بانک
        /// </summary>
        [StringValidation] public string BankName { get; set; }
        /// <summary>
        /// نام صاحب حساب 
        /// </summary>
        [StringValidation] public string BankAccountOwner { get; set; }

    }
}