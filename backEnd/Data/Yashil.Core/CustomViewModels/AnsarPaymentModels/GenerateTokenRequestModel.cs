using System.Collections.Generic;

namespace Yashil.Core.CustomViewModels.AnsarPaymentModels
{
    public class ApportionmentAccountList
    {
        public string AccountIBAN { get; set; }
        public string Amount { get; set; }
        public string ApportionmentAccountType { get; set; }
    }

    public class GenerateTokenRequestModel
    {
        public WSContext WSContext { get; set; }
        public string TransType { get; set; }
        public string ReserveNum { get; set; }
        public string Amount { get; set; }
        public string RedirectUrl { get; set; }
        /// <summary>
        /// کد کاربر که در سایت فعلی شماره موبایل کاربر است
        /// </summary>
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public List<ApportionmentAccountList> ApportionmentAccountList { get; set; }
    }

    public class WSContext
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
