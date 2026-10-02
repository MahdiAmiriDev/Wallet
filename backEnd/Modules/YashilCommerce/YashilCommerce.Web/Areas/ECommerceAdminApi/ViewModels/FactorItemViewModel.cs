using System.Collections.Generic;
using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class FactorItemListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        public int OrderId { get; set; }
        [StringValidation] public string Code { get; set; }

        [StringValidation] public string FactorTitle { get; set; }

        [StringValidation] public string OrderItemTitle { get; set; }
        [StringValidation] public string OrderItemCode { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        /// <summary>
        /// قیمت پایه
        /// </summary>
        public decimal OrderItemPrice { get; set; }
        /// <summary>
        /// تعداد
        /// </summary>
        public int OrderItemQuantity { get; set; }
        public decimal OrderItemPriceIncDiscount { get; set; }
        /// <summary>
        /// تاریخ خرید
        /// </summary>
        public int OrderDate { get; set; }
        /// <summary>
        ///  نام کاربری خریدار
        /// </summary>
        [StringValidation] public string CustomerUserName { get; set; }
        /// <summary>
        /// مرکز آیتم خریداری شده
        /// </summary>
        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// نمایندگی آیتم خریداری شده
        /// </summary>
        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string Title { get; set; }

        /// <summary>
        /// تعداد
        /// </summary>
        public int Quantity { get; set; }
        public bool IsGroupedByTaxCategory { get; set; }
    }


    public class FactorItemEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [Range(0, int.MaxValue)][Required] public int FactorId { get; set; }
        [Range(0, int.MaxValue)][Required] public int OrderItemId { get; set; }
        [StringValidation] public string OrderItemHash { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int AccessLevelId { get; set; }
    }


    public class FactorItemSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

    public class FactorItemStatisticViewModel
    {
        public int TaxCategoryId { get; set; }
        public int ProductSalesPlanningId { get; set; }
        public int Quantity { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountAmountExclTax { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DiscountedPriceSumTaxAmount { get; set; }
    }


    public class FactorItemDetailFullViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [StringValidation] public string ProductTitle { get; set; }
        public int? FactorGroupId { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }
        [StringValidation] public string RepresentationTitle { get; set; }
        [StringValidation] public string NationalCode { get; set; }
        [StringValidation] public string PaymentMethodType { get; set; }
        [StringValidation] public string InstallmentCount { get; set; }
        public int CreationDateInt { get; set; }
        public decimal? TotalIncludeTax { get; set; }
        public decimal? Price { get; set; }
        public decimal? AdditionalShippingCharge { get; set; }
        public decimal? Tax { get; set; }
        public decimal? VezaratSubsidy { get; set; }
        public decimal? VezaratSubsidyPriceIncProfit { get; set; }
        public decimal? VezaratSubsidyPriceIncTaxIncProfit { get; set; }
        public decimal? SataSubsidy { get; set; }
        public decimal? SataSubsidyPriceIncProfit { get; set; }
        public decimal? SataSubsidyPriceIncTaxIncProfit { get; set; }
        public decimal? BasePrice { get; set; }
        [StringValidation] public string FactorCode { get; set; }
        [StringValidation] public string OrderNumber { get; set; }
        [StringValidation] public string UserName { get; set; }
        public int Quantity { get; set; }
    }

    public class InvalidFactorItemViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        [StringValidation] public string OrderTitle { get; set; }
        /// <summary>
        ///         /// قیمت با تخفیف
        ///</summary>
        public decimal OrderItemPriceIncDiscount { get; set; }
        /// <summary>
        ///         /// قیمت  صحیح با تخفیف
        ///</summary>
        public decimal CorrectOrderItemPriceIncDiscount { get; set; }
        /// <summary>
        /// مبلغ تخفیف
        /// </summary>
        public decimal ProfitPrice { get; set; }
        /// <summary>
        /// مبلغ تخفیف صحیح
        /// </summary>
        public decimal CorrectProfitPrice { get; set; }
        /// <summary>
        ///         /// میزان تخفیف (بدون مالیات)
        ///</summary>
        public decimal DiscountAmountExclTax { get; set; }
        /// <summary>
        /// مبلغ صحیح  میزان تخفیف (بدون مالیات)
        /// </summary>
        public decimal CorrectDiscountAmountExclTax { get; set; }
        /// <summary>
        ///         /// میزان کارمزد  (بدون مالیات)
        ///</summary>
        public decimal WageAmountExcTax { get; set; }
        /// <summary>
        /// مبلغ صحیح میزان کارمزد  (بدون مالیات)
        /// </summary>
        public decimal CorrectWageAmountExcTax { get; set; }
    }

    public class UnFactoredProductSalePlanning
    {
        public int?[] ProductSalesPlanningIds { get; set; }
    }


    public class FactorItemPaymentsViewModel
    {
        /// <summary>
        /// لیست آیتم های پرداخت مربوط به هر آیتم فاکتور
        /// </summary>
        public List<FactorOrderItemPaymentViewModel> FactorOrderItemPaymentList { get; set; }

    }

    public class FactorOrderItemPaymentViewModel
    {
        /// <summary>
        /// کد فاکتور
        /// </summary>
        [StringValidation]
        [CanBeNull]
        public string FactorCode { get; set; }
        /// <summary>
        /// شناسه پرداخت
        /// </summary>
        public int? PaymentId { get; set; }
        /// <summary>
        ///تاریخ پرداخت 
        /// </summary>
        public int? PaidDate { get; set; }

        /// <summary>
        /// سهم پرداخت
        /// </summary>
        public decimal? OrderItemPayment { get; set; }

        /// <summary>
        /// مبلغ کل پرداخت
        /// </summary>
        public decimal? Total { get; set; }

        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int? InstallmentCount { get; set; }

        /// <summary>
        /// نوع پرداخت
        /// </summary>
        [CanBeNull]
        public string PaymentMethodTitle { get; set; }
    }

    public class FactorItemSummaryViewModel
    {
        public int? Id { get; set; }
        /// <summary>
        /// روش پرداخت
        /// </summary>
        [CanBeNull]
        public string PaymentMethodType { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int? InstallmentCount { get; set; }
        /// <summary>
        /// مبلغ پرداخت
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// مجموع مبلغ پرداخت
        /// </summary>
        public decimal? FactorItemPaymentTotal { get; set; }
        /// <summary>
        /// درصد پرداخت
        /// </summary>
        public decimal? TotalPricePercentage { get; set; }
    }

    public class FactorItemRefundedViewModel
    {
        /// <summary>
        /// شماره فاکتور
        /// </summary>
        public int? FactorId { get; set; }

        /// <summary>
        /// کانورت شده
        /// </summary>
        public bool IsConverted { get; set; }

        /// <summary>
        /// یارانه خام
        /// </summary>
        public decimal? SubsidyPrice { get; set; }
        /// <summary>
        /// یارانه باکارمزد
        /// </summary>
        public decimal? SubsidyPriceIncProfit { get; set; }
        /// <summary>
        /// یارانه با کارمزد و مالیات
        /// </summary>
        public decimal? SubsidyPriceIncTaxIncProfit { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        [CanBeNull] public string CardNumber { get; set; }
        /// <summary>
        /// کدملی
        /// </summary>
        [CanBeNull] public string NationalCode { get; set; }

        /// <summary>
        /// تاریخ خرید
        /// </summary>
        public int? PaidDateInt { get; set; }

        /// <summary>
        /// تاریخ عودت
        /// </summary>
        public int? RefundDateInt { get; set; }

    }

    /// <summary>
    /// مدل ایتم های فاکتور که بر روی قیمت فاکتور تاثیری ندارند
    /// </summary>
    public class FactorItemPropertiesThatHaveNotEffectInFactorPriceViewModel
    {
        /// <summary>
        /// شناسه فاکتور ایتم
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///         /// درصد بیمه مربوط به فاکتور تامین کننده
        ///</summary>
        public decimal OrderItemInsurancePercentage { get; set; }

        /// <summary>
        ///         /// مبلغ بیمه مربوط به فاکتور تامین کننده
        ///</summary>
        public string OrderItemInsurancePrice { get; set; }

        /// <summary>
        ///         /// مبلغ سایر کسورات از فاکتور تامین کننده
        ///</summary>
        public decimal OrderItemOtherDeductionsPrice { get; set; }

        /// <summary>
        ///         /// درصد سایر کسورات مربوط به فاکتور تامین کننده
        ///</summary>
        public decimal OrderItemOtherDeductionsPercentage { get; set; }
        /// <summary>
        ///         /// درصد کسورات بیمه
        ///</summary>
        public decimal SaleConfigurationInsurancePercentage { get; set; }

        /// <summary>
        /// آیا شامل بیمه می شود.
        /// </summary>
        public bool IsIncludeInsurance { get; set; }
    }
}