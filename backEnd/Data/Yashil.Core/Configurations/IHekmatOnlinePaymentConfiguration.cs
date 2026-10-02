namespace Yashil.Core.Configurations
{
    public interface IHekmatOnlinePaymentConfiguration
    {
        /// <summary>
        /// شناسه پذیرنده
        /// mid  = کد فروشنده در سند درگاه آنلاین حکمت می باشد
        /// حداکثر رشته 19 رقمی
        /// </summary>
        string MerchantId { get; set; }
        /// <summary>
        /// کلمه عبور در اختیار سایت که باید به صورت
        /// base64
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// ترمینال
        /// </summary>
        string TerminalId { get; set; }
        /// <summary>
        ///  کلید عمومی
        /// این کلید از سمت درگاه به ما داده می شود
        /// نتیجه درخواسیت هایی که از درگاه به سایت بر میگردد
        /// با این کلید
        ///  signData
        /// ارسالی از درگاه را 
        /// Verify
        ///  می کنیم
        /// </summary>
        string HekmatIpgPublicKey { get; set; }
        /// <summary>
        /// کلید خصوصی سایت
        /// با این کلید دیتا های سایت را
        /// sign
        /// می کنیم و 
        /// کلید عمومی  ساخته شده به همراه این کلید را در اختیار درگاه قرار می گیرد
        /// برای
        /// Verify
        /// کردن 
        /// Sign Data
        /// </summary>
        string SitePrivateKey { get; set; }
        /// <summary>
        /// آدری بازگشت به سایت
        /// </summary>
        string CallbackUrl { get; set; }
        /// <summary>
        /// آدرس ساخت توکن
        /// </summary>
        string GenerateTokenUrl { get; set; }
        /// <summary>
        /// آدرس ارسال به صفحه اصلی
        /// </summary>
        string RedirectToIpgUrl { get; set; }
        /// <summary>
        /// آدرس تایید تراکنش
        /// </summary>
        string VerifyTransactionUrl { get; set; }
        /// <summary>
        /// آدرس باطل کردن تراکنش
        /// </summary>
        string VoidTransactionUrl { get; set; }
        /// <summary>
        /// آدرس جستجوی تراکنش
        /// </summary>
        string InquireTransactionUrl { get; set; }
    }
}
