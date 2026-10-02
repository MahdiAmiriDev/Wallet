using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class OrderItemPriceParametersView  
    {
public int OrderId { get; set; }
public int OrderItemId { get; set; }
public int OrderStatusType { get; set; }
public int OrderCreationDateInt { get; set; }
public int? ParentId { get; set; }
public bool Deleted { get; set; }
public int ApplicationId { get; set; }
public int ProductSalesPlanningId { get; set; }
public int RepresentationId { get; set; }
public int CompanyId { get; set; }
public int Quantity { get; set; }
public decimal Price { get; set; }
public decimal BasePrice { get; set; }
public decimal? DiscountAmountExclTax { get; set; }
public decimal? AdditionalShippingCharge { get; set; }
public decimal PriceIncDiscount { get; set; }
public decimal? PriceIncDiscountIncAdditionalShippingCharge { get; set; }
public decimal? PriceExclDiscountExclProfit { get; set; }
public decimal? Netprice { get; set; }
public decimal? DiscountWithProfitPrice { get; set; }
public decimal? WageAmountExcTax { get; set; }
public decimal? TaxAmountPaidToSupplier { get; set; }
public decimal? PurchasePriceFromSupplier { get; set; }
public decimal? AmountPaidToSupplier { get; set; }
public decimal? SataSubsidePrice { get; set; }
public decimal? SataSubsidyPriceIncProfit { get; set; }
public decimal? SataSubsidyPriceIncTaxIncProfit { get; set; }
public decimal? VezaratSubsidyPrice { get; set; }
public decimal? VezaratSubsidyPriceIncProfit { get; set; }
public decimal? VezaratSubsidyPriceIncTaxIncProfit { get; set; }
public int VezaratCount { get; set; }
public int SataCount { get; set; }
public decimal TaxPrice { get; set; }
public decimal Total { get; set; }
public decimal? TotalExclTax { get; set; }
public decimal? TaxPercentage { get; set; }
public decimal ProfitPercentage { get; set; }
public decimal ProfitPrice { get; set; }
public decimal InsurancePercentage { get; set; }
public decimal InsurancePrice { get; set; }
public decimal OtherDeductionsPercentage { get; set; }
public decimal OtherDeductionsPrice { get; set; }
public decimal ValueAddedPrice { get; set; }
public int? SubsidyCategoryGroupId { get; set; }
public string SubsidyCategoryGroupTitle { get; set; }
public int? SubsidyId { get; set; }
    }
 }
