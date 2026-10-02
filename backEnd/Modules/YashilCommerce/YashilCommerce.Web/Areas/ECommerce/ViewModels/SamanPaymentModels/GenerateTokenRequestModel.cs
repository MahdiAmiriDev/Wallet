namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SamanPaymentModels
{
    public class GenerateTokenRequestModel
    {
        /// <summary>
        /// مشخص کننده نوع عملیات درخواستی است. و برای دریافت توکن مقدار آن باید 
        /// Token
        /// باشد
        /// به حروف بزرگ و کوچک حساس نیست.
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// مبلغ خرید است که فروشنده می خواهد از خریدار دریافت نماید
        /// این مقدار باید به صورت یک عدد صحیح
        /// int باشد
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// شماره ترمینال فروشنده است که برای شناسایی فروشنده در هر تراکنش
        /// لازم است. این شماره از سوی شرکت پرداخت الکترونیک سامان کیش اعلام خواهد شد.
        /// </summary>
        public string TerminalId { get; set; }
        /// <summary>
        /// شماره یکتایی که باید از طرف فروشنده به ازای هر تراکنش خرید
        /// ارسال می شود
        /// </summary>
        public string ResNum { get; set; }
        /// <summary>
        /// شماره موبایل کاربر (خریدار – دارنده ی کارت)  جهت بازیابی و یا ذخیره سازی اطلاعات کارت
        /// </summary>
        public string CellNumber { get; set; }
        /// <summary>
        /// آدرس بازگشتی سایت
        /// </summary>
        public string RedirectUrl { get; set; }



    }
}
