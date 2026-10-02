using System.Collections.Generic;

namespace Yashil.Core.CustomDtos.Payment;

public class HybridPaymentRequestDto
{
    public int Id { get; set; }
    public string ReferenceNumber { get; set; }
    public string ReservationNumber { get; set; }
    public long BasePrice { get; set; }
    public int InstallmentCount { get; set; }
    public decimal InstituteWage { get; set; }
    public decimal SupplierWage { get; set; }
    public decimal CashInstituteWage { get; set; }
    public decimal CashSupplierWage { get; set; }
    public int? CashPaymentPercentage { get; set; }
    public string AdditionalData { get; set; }
    public int HybridPaymentRequestStatusType { get; set; }
    public int? ShippingAmount { get; set; }
    public int Tax { get; set; }

    public IEnumerable<HybridPaymentDto> HybridPayments { get; set; }
}