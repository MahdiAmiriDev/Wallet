using JetBrains.Annotations;

namespace Yashil.Core.CustomDomainModels
{
    public class FactorStatisticDomainModel
    {
        public int Id { get; set; }

        /// <summary>
        /// کد فاکتور
        /// </summary>
        public string FactorCode { get; set; }
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
        public string TaxCategoryTitle { get; set; }
        /// <summary>
        /// قرارداد
        /// </summary>
        public string SaleConfigurationTitle { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// نام نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }
        /// <summary>
        /// وضعیت بررسی فاکتور
        /// </summary>
        public string FactorReviewStatusTypeTitle { get; set; }
        /// <summary>
        /// تعداد پرداختی ها
        /// </summary>
        public int PaidFactorInstallmentCount { get; set; }

        /// <summary>
        /// تعداد اقساط باقیمانده جهت تسویه فاکتور
        /// تعداد اقساط جهت تسویه فاکتور - تعداد اقساط پرداختی
        /// </summary>
        public int RemainingFactorInstallmentCount => FactorInstallmentCount - PaidFactorInstallmentCount;
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
        public decimal InsurancePercentage { get; set; }

        /// <summary>
        /// مبلغ بیمه 
        /// </summary>
        public decimal InsurancePrice { get; set; }

        /// <summary>
        /// دارای گواهی ارزش افزوده
        /// </summary>
        public bool HasTaxCertificate { get; set; }

        /// <summary>
        /// درصد سایر کسورات یا درصد هزینه تبلیغات
        ///</summary>
        public decimal OtherDeductionsPercentage { get; set; }
        /// <summary>
        /// مبلغ سایر کسورات یا مبلغ هزینه تبلیغات
        /// </summary>
        public decimal OtherDeductionsPrice { get; set; }

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
        public decimal SumRemaining => (decimal)(CompanyPurchasePrice - InsurancePrice - OtherDeductionsPrice - (SumFactorDeduction ?? 0) - SumPaid);
        /// <summary>
        /// شماره حساب بانکی
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// شماره شبا
        /// </summary>
        public string ShebaNumber { get; set; }
        /// <summary>
        /// نام شعبه
        /// </summary>
        [CanBeNull]
        public string BranchName { get; set; }
        /// <summary>
        /// نام بانک
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// نام صاحب حساب 
        /// </summary>
        public string BankAccountOwner { get; set; }
    }
}