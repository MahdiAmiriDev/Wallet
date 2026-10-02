namespace Yashil.Core.CustomViewModels.SaderatPaymentModels
{
    public class SaderatVerifyMerchantTransResponseModel
    {
        /// <summary>
        /// وضعیت  تایید یا عدم تراکنش
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnId { get; set; }
        /// <summary>
        /// متن فارسی متناظر با فیلد استاتوس
        /// </summary>
        public string Message { get; set; }
    }
}
