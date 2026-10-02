namespace Yashil.Core.Configurations
{
    /// <summary>
    /// تنظیمات مربوط به ارسال پیام
    /// </summary>
    public class YashilCommerceCustomMessage
    {
        /// <summary>
        /// پیام پرداخت موفقیت آمیز سفارش
        /// </summary>
        public string PayOkMessage { get; set; }
        /// <summary>
        /// پیام پرداخت انجام نشده سفارش
        /// </summary>
        public string PayFailMessage { get; set; }
    }
}
