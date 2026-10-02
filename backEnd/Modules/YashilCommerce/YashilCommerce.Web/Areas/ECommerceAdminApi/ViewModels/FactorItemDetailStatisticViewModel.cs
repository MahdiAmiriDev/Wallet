using System;
using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class FactorItemDetailStatisticViewModel

    {
        public int Id { get; set; }
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string OrderTitle { get; set; }
        [StringValidation] public string UserName { get; set; }
        public int Quantity { get; set; }
        public int CreationDateInt { get; set; }
        public decimal? BasePrice { get; set; }
        public int? PaidDateInt { get; set; }
        public decimal? Price { get; set; }
        public decimal PriceIncDiscountExclAdditionalShippingCharge { get; set; }
        public decimal? AdditionalShippingCharge { get; set; }
        public decimal? PriceIncDiscount { get; set; }
        public decimal? DiscountWithProfitPrice { get; set; }
        public decimal? CompanyPurchasePrice { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DiscountedPriceSumTaxAmount { get; set; }
        public int? FactorGroupId { get; set; }
        [StringValidation] public string? Code { get; set; }
        [StringValidation] public string FactorTitle { get; set; }
        public int FactorId { get; set; }
        public int CompanyId { get; set; }
        public int? RepresentationId { get; set; }
        public int FactorReviewStatusType { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public int SaleConfigurationId { get; set; }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int ProductSalesPlanningId { get; set; }
        public int TaxCategoryId { get; set; }
        /// <summary>
        ///  مبلغ کالا منهای تخفیف منهای سود + هزینه ارسال 
        /// </summary>
        public decimal? Netprice { get; set; }
        /// <summary>
        /// قیمت منهای سود منهای تخفیف
        /// </summary>
        public decimal? PriceExclDiscountExclProfit { get; set; }
        //مبلغ کارمزد سایت  
        public decimal? WageAmountExcTax { get; set; }
        /// <summary>
        /// کد محصول
        /// </summary>
        [StringValidation] public string? ProductCode { get; set; }

        /// <summary>
        /// کد نمایندگی
        /// </summary>
        [StringValidation] public string? RepresentationCode { get; set; }

        /// <summary>
        /// نام نمایندگی
        /// </summary>
        [StringValidation] public string? RepresentationTitle { get; set; }

        /// <summary>
        /// سایر کسورات : هزینه تبلیغات
        /// </summary>
        public decimal? OtherDeductionsPrice { get; set; }
        /// <summary>
        /// قیمت پایه منهای تخفیف منهای سود منهای سایر کسورات 
        /// سایر کسورات که همان هزینه تبلیغات می باشد در صورتی که در تنظیم فروش یا قرارداد 
        /// مقدار "هزینه کسورات در فاکتور آورده شود" انتخاب شده باشد  
        /// در این فیلد مبلغ سایر کسورات قرار میگیرد در غیر این صورت مبلغ سایر کسورات در این فیلد صفر می باشد 
        /// </summary>
        public decimal? PriceExclDiscountExclProfitExclOtherDeductionsPrice { get; set; }
        /// <summary>
        /// قیمت پایه منهای تخفیف منهای سود بعلاوه هزینه ارسال (NetPrice) 
        /// منهای سایر کسورات
        /// </summary>
        public decimal? NetPriceExclOtherDeductionsPrice { get; set; }
        /// <summary>
        /// مبلغ سود
        /// </summary>
        public decimal ProfitPrice { get; set; }
        /// <summary>
        /// مبلغ تخفیف
        /// </summary>
        public decimal? DiscountAmountExclTax { get; set; }
        /// <summary>
        /// کد ملی خریدار
        /// </summary>
        [StringValidation] public string? NationalCode { get; set; }

    }
}