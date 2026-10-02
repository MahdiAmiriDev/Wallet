namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SamanPaymentModels
{
    /// <summary>
    /// وقتی متد تایید
    /// VerifyTransaction
    /// یا متد برگشت به حساب
    /// Reverse
    /// فراخوانی می شود
    /// مقادیر خروجی به صورت فیلد های زیر می باشد
    /// </summary>
    public class TransResponseModel
    {
        /// <summary>
        /// جزییات تراکنش
        /// </summary>
        public VerifyInfo TransactionDetail { get; set; }
        /// <summary>
        /// کد تایید
        ///   0 :  درخواست موفق بوده است
        ///  -2 : تراکنش یافت نشد
        ///  -6 : بیش از نیم ساعت از زمان اجرای تراکنش گذشته است
        ///  2 : درخواست تکراری می باشد.
        /// -105 : ترمینال ارسالی در سیستم موجود نمی باشد
        /// -104 : ترمینال ارسالی غیرفعال می باشد
        /// -106 : آدرس آ ی پی درخواستی غیر مجاز می باشد
        /// </summary>
        public int ResultCode { get; set; }
        /// <summary>
        /// پیام
        /// </summary>
        public string ResultDescription { get; set; }
        /// <summary>
        ///  موفقیت آمیز بود
        /// </summary>
        public bool Success { get; set; }
    }
    /// <summary>
    /// جزییات تراکنش
    /// </summary>
    public class VerifyInfo
    {
        /// <summary>
        /// شماره مرجع
        /// </summary>
        public string RRN { get; set; }
        /// <summary>
        /// رسید دیجیتالی تراکنش
        /// </summary>
        public string RefNum { get; set; }
        /// <summary>
        /// شماره کارت به صورت mask شده
        /// </summary>
        public string MaskedPan { get; set; }
        /// <summary>
        /// HashedPan
        /// </summary>
        public string HashedPan { get; set; }
        /// <summary>
        /// شماره ترمینال
        /// </summary>
        public int TerminalNumber { get; set; }
        /// <summary>
        /// مبلغ ارسال شده به درگاه 
        /// </summary>
        public long OrginalAmount { get; set; }
        /// <summary>
        /// مبلغ کسر شده از ساب صا ب کارت
        /// </summary>
        public long AffectiveAmount { get; set; }
        /// <summary>
        /// تاریخ تراکنش
        /// </summary>
        public string StraceDate { get; set; }
        /// <summary>
        /// کد رهگیری 
        /// </summary>
        public string StraceNo { get; set; }
    }
}
