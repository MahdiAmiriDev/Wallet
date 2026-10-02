namespace Yashil.Core.CustomDtos.Payment;

public class HybridPaymentDto
{
    public int Id { get; set; }
    public int SaleType { get; set; }
    public int? PaymentMethodType { get; set; }
    public int? HybridPaymentStatusType { get; set; }
    public long Price { get; set; }
    public string AuthorizationTransactionResult { get; set; }
    public int? PaidDateInt { get; set; }
    public string ReservationNumber { get; set; }
}