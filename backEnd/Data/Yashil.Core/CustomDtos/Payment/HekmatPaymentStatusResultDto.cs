namespace Yashil.Core.CustomDtos.Payment
{
    /// <summary>
    ///  اطلاعات وضعیت پرداخت با حکمت
    /// </summary>
    public class HekmatPaymentStatusResultDto
    {
        /// <summary>
        /// شناسه پرداخت پذیرنده
        /// </summary>
        public string ReservationNumber { get; set; }
        /// <summary>
        ///  شماره پیگیری دارنده
        /// </summary>
        public string RefrenceNumber { get; set; }
        /// <summary>
        /// کد خطا :درصورتی که دریافت وضعیت پرداخت حکمت دارای خطا باشد کدی اعلام می شود که نشان دهنده خطای رخ داه است
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        ///  کد نتیجه پرداخت حکمت یعنی
        ///  0 : ثبت اولیه یعنی منتظر بررسی پرداخت حکمت
        ///  1 : پرداخت  حکمت موفق بود
        ///  2 :  پرداخت حکمت ناموفق بود
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// پیام نتیجه پرداخت
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public string PaidDate { get; set; }
         
    }
}
