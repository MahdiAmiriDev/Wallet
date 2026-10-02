using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UnFactoredOrderItemView  
    {
public int Id { get; set; }
public string OrderTitle { get; set; }
public int SaleConfigurationId { get; set; }
public int? RepresentationId { get; set; }
public string RepresentationTitle { get; set; }
public int CompanyId { get; set; }
public int ApplicationId { get; set; }
public int ProductId { get; set; }
public int ProductSalePlanningId { get; set; }
public string ProductTitle { get; set; }
public string SaleConfigurationTitle { get; set; }
public DateTime CreationDate { get; set; }
public int Quantity { get; set; }
public decimal Price { get; set; }
public decimal BasePrice { get; set; }
public decimal DiscountPercentage { get; set; }
public decimal ProfitPercentage { get; set; }
public decimal? DiscountAmountInclTax { get; set; }
public decimal? DiscountAmountExclTax { get; set; }
public decimal PriceIncDiscount { get; set; }
public decimal? AdditionalShippingCharge { get; set; }
public decimal? PriceIncDiscountWithAdditionalShippingCharge { get; set; }
public decimal? TaxPercentage { get; set; }
public int MainCategoryId { get; set; }
public string MainCategoryTitle { get; set; }
public string MainCategoryCode { get; set; }
public string UnitOfMeasurement { get; set; }
public int IsIncludeInsurance { get; set; }
public string NationalCode { get; set; }
public string CardNumber { get; set; }
public int? ProductTemplateId { get; set; }
public int CustomerClubId { get; set; }
public int TaxCategoryId { get; set; }
public int CategoryGroupId { get; set; }
public bool IsGroupedByTaxCategory { get; set; }
public decimal? TaxPriceExclSubsidy { get; set; }
public bool GenerateFactorIsAllowedBasedOnProductGroup { get; set; }
public string ProductGroupIdIsNullMessage { get; set; }
public bool UseProductGroupForFactor { get; set; }
public bool UseProductGroupForConvert { get; set; }
public int? ProductGroupId { get; set; }
public string ProductGroupTitle { get; set; }
public string ProductGroupMainCode { get; set; }
public string ProductGroupCode { get; set; }
    }
 }
