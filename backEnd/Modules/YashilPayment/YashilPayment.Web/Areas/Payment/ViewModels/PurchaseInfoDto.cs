namespace YashilPayment.Web.Areas.Payment.ViewModels
{
    public class PurchaseInfoDto
    {
        public double TotalAmount { get; set; }
        public double Amount { get; set; }
        public int InstallmentCount { get; set; }        
    }
}
