namespace Yashil.Core.CustomViewModels.SaderatPaymentModels
{
    public class SaderatGenerateTokenRequestModel
    {
        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        ///  آدرس صفحه ی برگشت به سایت
        /// </summary>
        public string callbackURL { get; set; }
        /// <summary>
        /// شماره فاکتور یا سریال پذیرنده
        /// </summary>
        public string invoiceID { get; set; }
        /// <summary>
        /// شماره تریمینال از جانب درگاه
        /// </summary>
        public long terminalID { get; set; }
        /// <summary>
        /// اطلاعات تکمیلی (اختیاری
        /// </summary>
        public string Payload { get; set; }
        /// <summary>
        /// ایمیل (اختیاری
        /// </summary>
        public string email { get; set; }

    }

}
