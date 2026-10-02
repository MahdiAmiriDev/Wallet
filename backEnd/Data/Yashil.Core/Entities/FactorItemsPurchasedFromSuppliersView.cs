using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class FactorItemsPurchasedFromSuppliersView  
    {
public int Id { get; set; }
public string Title { get; set; }
public string ProductCode { get; set; }
public string RepresentationCode { get; set; }
public string RepresentationTitle { get; set; }
public string OrderTitle { get; set; }
public string NationalCode { get; set; }
public string UserName { get; set; }
public int Quantity { get; set; }
public int CreationDateInt { get; set; }
public int? PaidDateInt { get; set; }
public decimal BasePrice { get; set; }
public decimal Price { get; set; }
public decimal PriceIncDiscountExclAdditionalShippingCharge { get; set; }
public decimal? AdditionalShippingCharge { get; set; }
public decimal? PriceIncDiscountWithAdditionalShippingCharge { get; set; }
public decimal? WageAmountExcTax { get; set; }
public decimal OtherDeductionsPrice { get; set; }
public decimal? OtherDeductionsPriceWithTax { get; set; }
public decimal? PriceIncDiscountIncAdditionalShippingCharge { get; set; }
public decimal? DiscountAmountExclTax { get; set; }
public decimal ProfitPrice { get; set; }
public decimal? DiscountWithProfitPrice { get; set; }
public decimal? DiscountWithProfitPriceWithOtherDeductionsPrice { get; set; }
public decimal? PriceExclDiscountExclProfit { get; set; }
public decimal? PriceExclDiscountExclProfitExclOtherDeductionsPrice { get; set; }
public decimal? Netprice { get; set; }
public decimal? NetPriceExclOtherDeductionsPrice { get; set; }
public decimal? TaxAmount { get; set; }
public decimal? DiscountedPriceSumTaxAmount { get; set; }
public decimal? InsurancePrice { get; set; }
public decimal? AmountPaidToSupplier { get; set; }
public int? FactorGroupId { get; set; }
public string Code { get; set; }
public string FactorTitle { get; set; }
public int FactorId { get; set; }
public int CompanyId { get; set; }
public int? RepresentationId { get; set; }
public int FactorReviewStatusType { get; set; }
public DateTime? ConfirmDate { get; set; }
public int FactorItemId { get; set; }
public int SaleConfigurationId { get; set; }
public int OrderItemId { get; set; }
public int ProductId { get; set; }
public int ProductSalesPlanningId { get; set; }
public int TaxCategoryId { get; set; }
public decimal? VezaratSubsidy { get; set; }
public decimal? VezaratSubsidyPriceIncProfit { get; set; }
public decimal? VezaratSubsidyPriceIncTaxIncProfit { get; set; }
public decimal? SataSubsidyPrice { get; set; }
public decimal? SataSubsidyPriceIncProfit { get; set; }
public decimal? SataSubsidyPriceIncTaxIncProfit { get; set; }
public decimal Total { get; set; }
public string ProductGroupTitle { get; set; }
public int? ProductGroupId { get; set; }
    }
 }
