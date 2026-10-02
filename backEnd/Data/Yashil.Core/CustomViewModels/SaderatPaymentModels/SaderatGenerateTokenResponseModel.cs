namespace Yashil.Core.CustomViewModels.SaderatPaymentModels
{
    public class SaderatGenerateTokenResponseModel
    {
        /// <summary>
        /// وضعیت در صورت موفق بودن مقدار 0
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        ///  مقدار توکن تولید شده
        /// </summary>
        public string Accesstoken { get; set; }
    }
}
