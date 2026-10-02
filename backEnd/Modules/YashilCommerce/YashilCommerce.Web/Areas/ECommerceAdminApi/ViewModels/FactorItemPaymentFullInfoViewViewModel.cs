using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class FactorItemPaymentFullInfoViewViewModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// شماره سفارش
        /// </summary>
        [StringValidation] public string OrderNumber { get; set; }
        /// <summary>
        /// آیدی سفارش
        /// </summary>
        public int OrderItemId { get; set; }
        /// <summary>
        /// کد رهگیری تراکنش
        /// </summary>
        public int? TrackingCode { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string? NationalCode { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int? InstallmentCount { get; set; }
        /// <summary>
        /// شناسه محصول
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// کد فاکتور
        /// </summary>
        [StringValidation] public string? FactorCode { get; set; }
        /// <summary>
        /// نام محصول
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }
        /// <summary>
        /// نام نمایندگی
        /// </summary>
        [StringValidation] public string RepresentationTitle { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// نحوه پرداخت
        /// </summary>
        [StringValidation] public string PaymentMethodTitle { get; set; }
        /// <summary>
        /// شماره حکمت یا شماره مرجع
        /// </summary>
        [StringValidation] public string? TransactionNumber { get; set; }
        /// <summary>
        /// مبلغ پایه با لحاظ تعداد
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// مبلغ کل با مالیات
        /// </summary>
        public decimal TotalIncludeTax { get; set; }
        /// <summary>
        /// مبلغ کل بدون مالیات
        /// </summary>
        public decimal? TotalExclTax { get; set; }
        /// <summary>
        /// یارانه
        /// </summary>
        public decimal? SubsidyPrice { get; set; }
        /// <summary>
        /// یارانه وزارت 
        /// </summary>
        public decimal? VezaratSubsidy { get; set; }
        /// <summary>
        /// یارانه وزارت با کارمزد
        /// </summary>
        public decimal? VezaratSubsidyPriceIncProfit { get; set; }
        /// <summary>
        /// یارانه وزارت با کارمزد و مالیات
        /// </summary>
        public decimal? VezaratSubsidyPriceIncTaxIncProfit { get; set; }
        /// <summary>
        /// یارانه ساتا
        /// </summary>
        public decimal? SataSubsidy { get; set; }
        /// <summary>
        /// یارانه ساتا با کارمزد
        /// </summary>
        public decimal? SataSubsidyPriceIncProfit { get; set; }
        /// <summary>
        /// یارانه ساتا با کارمزد و مالیات
        /// </summary>
        public decimal? SataSubsidyPriceIncTaxIncProfit { get; set; }
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public int? PaidDateInt { get; set; }
        /// <summary>
        /// کد گروه بندی فاکتور
        /// </summary>
        public int? FactorGroupId { get; set; }
        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public decimal? PaymentTotal { get; set; }
        /// <summary>
        /// مبلغ اصلاح شده تراکنش
        /// </summary>
        public decimal? NewPaymentAmount { get; set; }
    }
}