namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
    public class ResultModel
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        
        public int Status { get; set; }
    }
}
