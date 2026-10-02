namespace Yashil.Core.CustomDtos.Payment
{
    public class PaymentRequestDto
    {
        public int MerchantId { get; set; }
        public int TerminalId { get; set; }
        public string ReservationNumber { get; set; }
        public string LocalDateTime { get; set; }
        public string ReturnUrl { get; set; }
        public int Amount { get; set; }
        public int InstallmentCount { get; set; }
        public string SignData { get; set; }
        public string AdditionalData { get; set; }
        public string MultiplexingData { get; set; }
        public string ApplicationName { get; set; }
        public string CardNumber { get; set; }
    }
}