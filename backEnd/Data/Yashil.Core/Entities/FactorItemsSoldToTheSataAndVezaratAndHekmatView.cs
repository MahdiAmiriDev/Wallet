using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class FactorItemsSoldToTheSataAndVezaratAndHekmatView  
    {
public int Id { get; set; }
public string FactorCode { get; set; }
public string OrderNumber { get; set; }
public string UserName { get; set; }
public string NationalCode { get; set; }
public int OrderItemId { get; set; }
public int ApplicationId { get; set; }
public int ProductId { get; set; }
public int FactorId { get; set; }
public int? FactorGroupId { get; set; }
public int Quantity { get; set; }
public decimal? AdditionalShippingCharge { get; set; }
public string ProductTitle { get; set; }
public string CompanyTitle { get; set; }
public string RepresentationTitle { get; set; }
public string ProvinceTitle { get; set; }
public string CityTitle { get; set; }
public string CityShortTitle { get; set; }
public string PaymentMethodType { get; set; }
public string InstallmentCount { get; set; }
public decimal TotalIncludeTax { get; set; }
public decimal? TotalExclTax { get; set; }
public decimal Tax { get; set; }
public decimal? VezaratSubsidy { get; set; }
public decimal? VezaratSubsidyPriceIncProfit { get; set; }
public decimal? VezaratSubsidyPriceIncTaxIncProfit { get; set; }
public decimal? SataSubsidy { get; set; }
public decimal? SataSubsidyPriceIncProfit { get; set; }
public decimal? SataSubsidyPriceIncTaxIncProfit { get; set; }
public decimal BasePrice { get; set; }
public decimal Price { get; set; }
public decimal? AmountPaidToSupplier { get; set; }
public int OrderId { get; set; }
public string CardNumber { get; set; }
public string InstallmentTrackingCode { get; set; }
public string PaidDateInt { get; set; }
public string RecipientMobileNumber { get; set; }
public int CategoryGroupId { get; set; }
public int? ProductTemplateId { get; set; }
public int? FactorGroupConfirmDateInt { get; set; }
public bool? IsGroupedByTaxCategory { get; set; }
public string TaxCategoryCode { get; set; }
public string MainCategoryCode { get; set; }
public string RepresentationFullAddress { get; set; }
    }
 }
