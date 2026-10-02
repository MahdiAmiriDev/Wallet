namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SaminPaymentModels
{
    public class SaminGenerateTokenResultModel
    {
        /// <summary>
        /// متغیر توکن با طول ثابت 16 کاراکتر
        /// که در ادامه فرآیند خرید و استعلام از آن استفاده خواهد شد
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// کد پاسخ
        /// SUCCESS	0 "عملیات موفقیت آمیز انجام شد"
        /// </summary>
        public int ResponseCode { get; set; }
        /// <summary>
        /// شرح پاسخ
        /// </summary>
        public string ResponseInformation { get; set; }
    }
}
