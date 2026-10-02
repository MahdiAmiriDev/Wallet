namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class FactorGroupItemReport
    {
        public int Id { get; set; }
        /// <summary>
        /// یا نمایندگی کد مرکز
        /// </summary>
        public string CompanyOrRepresentationCode { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// نام نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }
        /// <summary>
        /// کد فاکتور
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// نوع پرداخت
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// دسته بندی مالیاتی
        /// </summary>
        public string TaxCategoryTitle { get; set; }
        /// <summary>
        /// مبلغ کل
        /// </summary>
        public decimal SumTotal { get; set; }

        /// <summary>
        /// Old Name :SumPriceSupplier
        ///  قیمت منهای سود منهای تخفیف 
        /// </summary>
        public decimal SumPriceExclDiscountExclProfit { get; set; }

        /// <summary>
        /// Old Name :SumPriceSupplier
        ///  قیمت منهای سود منهای تخفیف منهای سایر کسورات (هزینه تبلیغات) در فاکتور تامین کننده قیمت کل با تخفیف میباشد 
        /// </summary>
        public decimal SumPriceExclDiscountExclProfitExclOtherDeductionsPrice { get; set; }

        /// <summary>
        /// مبلغ که باید به تامین کننده پرداخت شود
        /// </summary>
        public decimal SumAmountPaidToSupplier { get; set; }
        /// <summary>
        /// هزینه ارسال
        /// </summary>
        public decimal SumAdditionalShippingCharge { get; set; }

        /// <summary>
        /// جمع مبلغ بیمه
        /// </summary>
        public decimal? SumInsuranceAmount { get; set; }
        /// <summary>
        /// تاریخ تایید فاکتور
        /// </summary>
        public int FactorConfirmDate { get; set; }
        /// <summary>
        /// در صد تخفیف  
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// در صد سود
        /// </summary>
        public decimal ProfitPercentage { get; set; }
        /// <summary>
        /// در صد تخفیف + سود
        /// </summary>
        public decimal ProfitDiscountPercentage { get; set; }
        /// <summary>
        /// مجموع سایر کسورات: مجموع هزینه تبلیغات
        /// </summary>
        public decimal SumOtherDeductionsPrice { get; set; }
        /// <summary>
        /// مجموع  تخفیف
        /// </summary>
        public decimal SumDiscountAmountExclTax { get; set; }
        /// <summary>
        /// مجموع سود 
        /// </summary>
        public decimal SumProfitPrice { get; set; }
        /// <summary>
        /// مجموع سود + تخفیف
        /// </summary>
        public decimal SumDiscountWithProfitPrice { get; set; }
        /// <summary>
        ///  مجموع سود + تخفیف + هزینه تبلیغات
        /// </summary>
        public decimal SumDiscountWithProfitPriceWithOtherDeductionsPrice { get; set; }
        /// <summary>
        /// سهم خانوار
        /// </summary>
        public decimal FamilyPortion { get; set; }
        /// <summary>
        ///قیمت منهای تخفیف بعلاوه هزینه ارسال(قیمت فروش تامین کننده در سایت)
        /// </summary>
        public decimal SumPriceIncDiscountWithAdditionalShippingCharge { get; set; }
        /// <summary>
        /// یارانه وزارت
        /// </summary>
        public decimal SumVezaratSubsidy { get; set; }
        /// <summary>
        /// یارانه وزارت با سود
        /// </summary>
        public decimal SumVezaratSubsidyPriceIncProfit { get; set; }
        /// <summary>
        ///سهم وزارت (یارانه وزارت با سود و مالیات)
        /// </summary>
        public decimal VezaratPortion { get; set; }
        /// <summary>
        /// یارانه ساتا
        /// </summary>
        public decimal SumSataSubsidyPrice { get; set; }
        /// <summary>
        /// یارانه ساتا با سود
        /// </summary>
        public decimal SumSataSataSubsidyPriceIncProfit { get; set; }
        /// <summary>
        /// سهم ساتا(یارانه ساتا با سود و مالیات)
        /// </summary>
        public decimal SataPortion { get; set; }
        /// <summary>
        /// قیمت پایه منهای تخفیف بعلاوه هزینه ارسال (قیمت فروش تامین کننده در سایت) منهای یارانه وزارت منهای یارانه ساتا
        /// </summary>
        public decimal SumPriceIncDiscountWithAdditionalShippingChargeExclVezaratSubsidyExclSataSubsidy { get; set; }
        /// <summary>
        /// قیمت پایه منهای تخفیف بعلاوه هزینه ارسال (قیمت فروش تامین کننده در سایت) منهای یارانه وزارت منهای یارانه ساتا بعلاوه کارمزد
        /// </summary>
        public decimal SumPriceIncDiscountWithAdditionalShippingChargeExclVezaratSubsidyExclSataSubsidyWithWageAmount { get; set; }

        /// <summary>
        /// مبلغ کل منهای تخفیف منهای سود + هزینه ارسال 
        /// </summary>
        public decimal SumNetPrice { get; set; }

        /// <summary>
        /// تعداد آیتم های فاکتور
        /// </summary>
        public int FactorItemCount { get; set; }
        /// <summary>
        /// تاریخ اولین خرید
        /// </summary>
        public int FirstFactorItemDateInt { get; set; }
        /// <summary>
        /// تاریخ اخرین خرید
        /// </summary>
        public int LastFactorItemDateInt { get; set; }
        /// <summary>
        /// تاریخ تایید نهایی گروه بندی فاکتور
        /// </summary>
        public int FactorGroupConfirmDate { get; set; }
        /// <summary>
        /// سررسید اولین قسط
        /// </summary>
        public int FactorFirstInstallmentPayDate { get; set; }
        /// <summary>
        /// اطلاعات حساب شخص
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// نام صاحب حساب
        /// </summary>
        public string PersonBankAccountName { get; set; }
        /// <summary>
        /// نام خانوادگی صاحب حساب
        /// </summary>
        public string PersonBankAccountLastName { get; set; }
        /// <summary>
        /// کد ملی صاحب حساب
        /// </summary>
        public string PersonBankAccountNationalCode { get; set; }
        /// <summary>
        /// کد تفصیلی
        /// اگر شخص حقوقی باشد کد اقتصادی قرارداد نوشته میشود
        /// و اگر شخص حقیقی باشد کد ملی موسس نوشته میشود
        /// </summary>
        public string DetailedCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BankTypeTitle { get; set; }

        /// <summary>
        /// مالیات + قیمت خرید+هزینه ارسال:شرکت این مبلغ را به تامین کننده می دهد:قیمت خرید انستیتو 
        /// </summary>
        public decimal SumDiscountedPriceSumTaxAmount { get; set; }

        /// <summary>
        /// جمع مالیات خرید
        /// </summary>
        public decimal SumTaxAmount { get; set; }
        /// <summary>
        ///مجموع مبلغ کارمزد سایت 
        /// </summary>
        public decimal SumWageAmountExcTax { get; set; }

        /// <summary>
        /// آیا هزینه تبلیغات در فاکتور تامین کننده لحاظ شود
        /// </summary>
        public bool IsOtherDeductionsIncludeInSupplierInvoice { get; set; }

        /// <summary>
        /// سایر کسورات با مالیات-هزینه تبلیغات با مالیات
        /// </summary>
        public decimal SumOtherDeductionsPriceIncTax { get; set; }
    }

}
