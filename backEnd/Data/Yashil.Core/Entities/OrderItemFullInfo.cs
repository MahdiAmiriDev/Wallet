using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class OrderItemFullInfo  
    {
public int Id { get; set; }
public string ApplicationTitle { get; set; }
public long? OrderId { get; set; }
public string CardNumber { get; set; }
public string NationalCode { get; set; }
public long? OrderItemId { get; set; }
public int? CreationDateInt { get; set; }
public int? CreationDateYearInt { get; set; }
public int? CreationDateMonthInt { get; set; }
public string MonthTitle { get; set; }
public int? CreationDateDayInt { get; set; }
public int? CreationDateHour { get; set; }
public int? CreationDateMinute { get; set; }
public string ProductTitle { get; set; }
public long? ProductId { get; set; }
public string CompanyTitle { get; set; }
public int? CompanyId { get; set; }
public string RepresentationTitle { get; set; }
public long? RepresentationId { get; set; }
public string CityTitle { get; set; }
public string ProvinceTitle { get; set; }
public string PaymentMethodType { get; set; }
public string UserName { get; set; }
public int? UserId { get; set; }
public string SubsidyTypeTitle { get; set; }
public int? SubsidyTypeId { get; set; }
public int? SubsidyStatusTypeId { get; set; }
public string SubsidyStatusTypeTitle { get; set; }
public string MembershipTypeTitle { get; set; }
public int? MembershipTypeId { get; set; }
public string CustomerClubTitle { get; set; }
public int? CustomerClubId { get; set; }
public long? BasePrice { get; set; }
public long? DiscountAmountExclTax { get; set; }
public long? PriceIncDiscount { get; set; }
public long? WageAmountExcTax { get; set; }
public long? TaxPrice { get; set; }
public long? SataSubsidePrice { get; set; }
public long? VezaratSubsidyPrice { get; set; }
public long? Total { get; set; }
public int? ProfitPrice { get; set; }
public int? InsurancePercentage { get; set; }
public int? OtherDeductionsPercentage { get; set; }
public int? OtherDeductionsPrice { get; set; }
public int? InsurancePrice { get; set; }
public int? ValueAdded { get; set; }
public int? Quantity { get; set; }
public int? OrderItemIsDeleted { get; set; }
public int? OrderStatusType { get; set; }
public string OrderStatusTypeTitle { get; set; }
public int? ProfitPercantage { get; set; }
public int? ApplicationId { get; set; }
public int? AdditionalShippingCharge { get; set; }
public int? InstallmentCount { get; set; }
public int? UnitBasePrice { get; set; }
public int? AmountPaidToSupplier { get; set; }
public int? SataSubsidyPriceIncProfit { get; set; }
public int? SataSubsidyPriceIncTaxIncProfit { get; set; }
public int? VezaratSubsidyPriceIncProfit { get; set; }
public int? VezaratSubsidyPriceIncTaxIncProfit { get; set; }
public int? TotalExclTax { get; set; }
public int? TotalPerformance { get; set; }
public byte[] EncryptedNationalCode { get; set; }
public byte[] EncryptedUserName { get; set; }
public int? ProvinceId { get; set; }
public long? NetPrice { get; set; }
public int? CategoryGroupId { get; set; }
    }
 }
