namespace Yashil.Core.CustomViewModels.AnsarPaymentModels
{


    public class VerifyMerchantTransRequestModel
    {
        public WSContext WSContext { get; set; }
        public string Token { get; set; }
        public string RefNum { get; set; }
    }

    
}
