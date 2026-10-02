namespace Yashil.Core.CustomViewModels.SaderatPaymentModels
{
    public class SaderatVerifyMerchantTransRequestModel
    {
        /// <summary>
        /// رسید دیجیتالی خریدار
        /// </summary>
        public string digitalreceipt { get; set; }
        /// <summary>
        /// شماره ترمینال
        /// </summary>
        public long Tid { get; set; }
    }
}
