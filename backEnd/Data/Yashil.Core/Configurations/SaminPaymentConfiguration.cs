namespace Yashil.Core.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public class SaminPaymentConfiguration : ISaminPaymentConfiguration
    {
        /// <summary>
        /// کد پذیرنده --- این کد بعد از عقد قرارداد با پذیرنده در اختیار قرار می گیرد.
        /// </summary>
        public string MerchantCode { get; set; }
        /// <summary>
        /// شماره ترمینال --- این کد بعد از عقد قرارداد با پذیرنده در اختیار قرار می گیرد
        /// </summary>
        public string TerminalCode { get; set; }
        /// <summary>
        /// رمز پذیرنده --- این رمز منحصر به هر پذیرنده است وباید در حفظ آن کوشا باشد
        /// </summary>
        public string MerchantPassword { get; set; }
        /// <summary>
        ///  آدرس بازگشتی از درگاه پرداخت 
        /// </summary>
        public string CallBackUrl { get; set; }
        /// <summary>
        ///  مسیر بازگشتی از درگاه پرداخت 
        /// </summary>
        public string CallBackPath { get; set; }
        /// <summary>
        /// آدرس تولید توکن برای ارسال به صفحه درگاه پرداخت
        /// </summary>
        public string GenerateTokenUrl { get; set; }
        /// <summary>
        /// آدرس صفحه درگاه پرداخت
        /// </summary>
        public string RedirectToIpgUrl { get; set; }
        /// <summary>
        /// آدرس تایید تراکنش پس از بازگشت از درگاه پرداخت
        /// </summary>
        public string VerifyTransactionUrl { get; set; }
        /// <summary>
        /// آدرس استعلام تراکنش
        /// </summary>
        public string InquireTransactionUrl { get; set; }
    }
}
