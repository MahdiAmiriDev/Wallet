namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SaminPaymentModels
{
    public class SaminConfirmTransactionResponseModel
    {
        /// <summary>
        /// شماره رهگیری
        /// </summary>
        public int RefNO { get; set; }
        /// <summary>
        /// کد پاسخ
        /// </summary>
        public int ResponseCode { get; set; }
        /// <summary>
        /// شرح پاسخ
        /// </summary>
        public string ResponseInformation { get; set; }

    }
}
