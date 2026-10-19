namespace YashilPayment.Web.Areas.Payment.ViewModels
{
    public class VerifyResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public int? Amount { get; set; }
        public int? SystemTraceNo { get; set; }
        public string? OrderId { get; set; }
        public int? InstallmentCount { get; set; }
    }
}
