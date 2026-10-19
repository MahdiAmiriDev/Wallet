namespace YashilPayment.Web.Areas.Payment.ViewModels
{
    public class PurchaseResult
    {
        /// <summary>
        /// نتیحه خرید
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// کد رهگیری (شناسه پرداخت یا سفارش مشتری) مشتری به ازای هر پرداخت
        /// </summary>
        public string ReservationNumber { get; set; }
        /// <summary>
        ///  شماره پیگیری دارنده
        /// </summary>
        public string RefrenceNumber { get; set; }
        /// <summary>
        /// توکن
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// آدرس 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// پیام
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// با موفقیت انجام شد ؟؟
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// آدرس بازگشتی 
        /// </summary>
        public string CallBackUrl { get; set; }
        /// <summary>
        /// اطلاعات سفارشی که پرداخت حکمت انجام می شود  
        /// </summary>
        public string? AdditionalDate { get; set; }
        /// <summary>
        /// مبلغ پرداخت شده
        /// </summary>
        public decimal Amount { get; set; }

        public long BasePrice { get; set; }

        /// <summary>
        /// آیا از فرمول برای محاسبه قیمت استفاده کند در حالت
        /// نقدی اقساطی طرح باقرالعلوم نیاز به استفاده
        /// از فرمول برای محاسبه قیمت نقدی اقساطی نیست
        /// </summary>
        public bool UseFormula { get; set; }

        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int? InstallmentCount { get; set; }

    }
}
