namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SaminPaymentModels
{
    public class SaminConfirmTransactionRequestModel
    {
        /// <summary>
        /// توکن
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// شماره رهگیری
        /// </summary>
        public int RefNO { get; set; }
        /// <summary>
        /// نوع وضعیت درخواستی تراکنش
        /// 1 = Verify
        /// 2 = REVERSED
        /// </summary>
        public int CONFIRM_TRANSACTION_STATUS { get; set; }
    }
}
