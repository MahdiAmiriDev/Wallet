namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SaminPaymentModels
{
    /// <summary>
    /// پرداخت با درگاه ثمین
    /// </summary>
    public class SaminPaymentViewModel
    {
        /// <summary>
        /// شناسه تحویل گیرنده
        /// </summary>
        public int RecipientId { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int InstallmentCount { get; set; }
    }

}
