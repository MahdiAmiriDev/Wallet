namespace Yashil.Core.CustomDomainModels
{
    public class CalcedOrderItemPrice
    {
        public int UId { get; set; }
        public int? ShoppingCartItemId { get; set; }
        public int RepresentationId { get; set; }
        public int ProductSalesPlanningId { get; set; }
        public int? ParentProductSalePlanningId { get; set; }
        /// <summary>
        /// قیمت پایه ضربدر تعداد
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// قیمت پایه
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// تخفیف
        /// </summary>
        public decimal? Discount { get; set; }
        /// <summary>
        /// قیمت با تخفیف
        /// </summary>
        public decimal? CalcedPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal AdditionalShippingCharge { get; set; }
        /// <summary>
        /// کارمزد
        /// </summary>
        public decimal? Wage { get; set; }
        /// <summary>
        /// یارانه
        /// </summary>
        public decimal? Subsidy { get; set; }
        /// <summary>
        /// مالیات
        /// </summary>
        public decimal? Tax { get; set; }
        /// <summary>
        /// مبلغ قابل پرداخت
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// درصد مالیات
        /// </summary>
        public decimal? TaxPercentage { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// درصد کارمزد سایت
        /// </summary>
        public decimal? WagePercentage { get; set; }

        /// <summary>
        /// ضریب افزایش سقف یارانه برای شهر 
        /// </summary>
        public decimal? CitySubsidyFactor { get; set; }

        /// <summary>
        /// ضریب افزایش سقف یارانه برای نمایندگی
        /// </summary>
        public decimal? RepresentationSubsidyFactor { get; set; }

        /// <summary>
        ///ضریب افزایش سقف یارانه برای  محصول
        /// </summary>
        public decimal? ProductSubsidyFactor { get; set; }

        /// <summary>
        /// ضریب افزایش سقف یارانه برای دسته محصول
        /// </summary>
        public decimal? CategorySubsidyFactor { get; set; }

        /// <summary>
        /// سقف یارانه برای هر حکمت کارت
        /// </summary>
        public int? SubsidyCeilingPerHekmatCard { get; set; }

        public int? SaleConfigurationAttributeId { get; set; }
        public int SaleConfigurationId { get; set; }
        public decimal? ProfitPercentage { get; set; }
        public decimal? InsurancePercentage { get; set; }
        public decimal? OtherDeductionsPercentage { get; set; }
        /// <summary>
        /// شناسه یارانه فعال بر اساس دسته بندی محصول 
        /// </summary>
        public int? SubsidyId { get; set; }
        /// <summary>
        /// شناسه دسته بندی محصول
        /// </summary>
        public int TaxCategoryId { get; set; }
        /// <summary>
        ///  آیا تحویل گیرنده ورازتی می باشد یا ساتایی
        /// </summary>
        public bool? IsVezarat { get; set; }
        /// <summary>
        /// شناسه گروه دسته بندی
        /// </summary>
        public int CategoryGroupId { get; set; }
        /// <summary>
        /// آیا تنظم فروش برای این محصول معاف از مالیات است یا خیر؟
        /// </summary>
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// ایا تایمن کننده محصول دارای گواهی ارزش افزوده است ؟
        /// </summary>
        public bool HasTaxCertificate { get; set; }

        /// <summary>
        /// آیا هزینه تبلیغات در فاکتور تامین کننده لحاظ شود
        /// </summary>
        public bool IsOtherDeductionsIncludeInSupplierInvoice { get; set; }

    }
}
