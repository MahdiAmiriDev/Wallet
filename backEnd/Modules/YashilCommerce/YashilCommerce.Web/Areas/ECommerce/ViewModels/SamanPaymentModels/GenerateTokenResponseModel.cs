namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SamanPaymentModels
{
    public class GenerateTokenResponseModel
    {
        /// <summary>
        /// وضعیت توکن
        ///  1 : دریافت توکن موفق بود
        ///  -1 : خطا در دریافت توکن
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// مقدار توکن دریافتی
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// کد خطا
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// پیام خطا
        /// </summary>
        public string errorDesc { get; set; }


    }
}
