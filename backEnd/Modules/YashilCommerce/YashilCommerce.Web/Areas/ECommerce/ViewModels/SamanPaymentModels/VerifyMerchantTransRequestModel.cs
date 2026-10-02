namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SamanPaymentModels
{
    /// <summary>
    /// بعد از بازگشت از صفحه درگاه بانک به سایت در صورتی که پرداخت موفق باشد
    /// باید پرداخت را تایید کرد با استفاده از متد
    /// VerifyTransaction
    /// یا درصورتی که بخواهیم تراکنش کاربر ره به حساب کاربر برگردانیم از متد
    /// Reverse
    /// استفاده می کنیم و
    /// برای استفاده از این متد ها باید فیلد های زیر را مقدار دهی و ارسال کنیم
    /// </summary>
    public class TransRequestModel
    {
        /// <summary>
        /// رسید دیجیتالی تراکنش
        /// </summary>
        public string RefNum { get; set; }
        /// <summary>
        /// شماره ترمینال
        /// </summary>
        public long TerminalNumber { get; set; }
    }
}
