using System.ComponentModel.DataAnnotations;

namespace YashilPayment.Web.Areas.Payment.ViewModels
{
    public class UserCardInfoViewModel
    {
        [Display(Name = @"شماره کارت")]
        [Required(ErrorMessage = "شماره کارت اجباری است ")]
        public string CardNumber { get; set; }
        [Display(Name = @"CVV2")]
        public string CVV2 { get; set; }
        [Display(Name = @"رمز دوم")]
        public string SecondPassword { get; set; }
        [Display(Name = @"ماه")]
        public string ExpireDateMonth { get; set; }
        [Display(Name = @"سال")]
        public string ExpireDateYear { get; set; }
        public string PaymentRequestToken { get; set; }
        public PurchaseInfoDto PurchaseInfoDto { get; set; }
    }
}
