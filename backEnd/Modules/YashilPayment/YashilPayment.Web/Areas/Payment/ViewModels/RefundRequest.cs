namespace YashilPayment.Web.Areas.Payment.ViewModels
{
    public class RefundRequest
    {
        public string MerchantId { get; set; }
        public string TerminalId { get; set; }
        public int SystemTraceNo { get; set; }
        public int Amount { get; set; }
        public byte[] SignData { get; set; }
    }
}
