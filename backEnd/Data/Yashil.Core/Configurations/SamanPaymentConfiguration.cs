namespace Yashil.Core.Configurations
{
    /// <summary>
    /// Saman Payment Configuration property for sep IPg
    /// configuration fields in appsettting
    /// </summary>
    public class SamanPaymentConfiguration : ISamanPaymentConfiguration
    {
        /// <summary>
        /// آدرس بازگشت از درگاه به سایت پذیرنده
        /// </summary>
        public string CallBackUrl { get; set; }
        /// <summary>
        /// شماره ترمینال
        /// </summary>
        public string TerminalId { get; set; }
        /// <summary>
        /// آدرس ساخت توکن
        /// </summary>
        public string GenerateTokenUrl { get; set; }
        /// <summary>
        /// آدرس صفحه درگاه پرداخت
        /// </summary>
        public string RedirectToIpgUrl { get; set; }
        /// <summary>
        /// آدرس تایید تراکنش 
        /// </summary>
        public string VerifyTransactionUrl { get; set; }
        /// <summary>
        /// آدرس برگشت به حساب تراکنش
        /// </summary>
        public string ReverseTransactionUrl { get; set; }
    }
}
