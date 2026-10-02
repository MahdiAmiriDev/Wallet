using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class FactorItemsSoldToTheSataAndVezaratAndHekmatPaymentsView  
    {
public long? Id { get; set; }
public string OrderNumber { get; set; }
public int OrderItemId { get; set; }
public int OrderId { get; set; }
public int? TrackingCode { get; set; }
public string NationalCode { get; set; }
public int? InstallmentCount { get; set; }
public int ProductId { get; set; }
public string FactorCode { get; set; }
public string ProductTitle { get; set; }
public string RepresentationTitle { get; set; }
public string CompanyTitle { get; set; }
public string PaymentMethodTitle { get; set; }
public string TransactionNumber { get; set; }
public decimal Price { get; set; }
public decimal TotalIncludeTax { get; set; }
public decimal? TotalExclTax { get; set; }
public decimal? SubsidyPrice { get; set; }
public decimal? VezaratSubsidy { get; set; }
public decimal? VezaratSubsidyPriceIncProfit { get; set; }
public decimal? VezaratSubsidyPriceIncTaxIncProfit { get; set; }
public decimal? SataSubsidy { get; set; }
public decimal? SataSubsidyPriceIncProfit { get; set; }
public decimal? SataSubsidyPriceIncTaxIncProfit { get; set; }
public int? PaidDateInt { get; set; }
public int? FactorGroupId { get; set; }
public decimal? PaymentTotal { get; set; }
public decimal? NewPaymentAmount { get; set; }
public decimal? FinalTotal { get; set; }
public int? PaymentId { get; set; }
    }
 }
