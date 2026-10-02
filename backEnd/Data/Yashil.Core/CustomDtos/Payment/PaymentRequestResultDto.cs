namespace Yashil.Core.CustomDtos.Payment
{
    /// <summary>
    /// اطلاعات درخواست پرداخت با حکمت کارت 
    /// </summary>
    public class PaymentRequestResultDto
    {
        /// <summary>
        /// پیام نتیجه درخواست پیام
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// کد وضعیت درخواست پیام
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// توکن ارسالی
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// موفقیت آمیز بود؟
        /// </summary>
        public bool Succeeded { get; set; }
    }
}
