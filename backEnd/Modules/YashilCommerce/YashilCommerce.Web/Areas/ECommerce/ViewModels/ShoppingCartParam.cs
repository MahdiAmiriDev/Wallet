namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class ShoppingCartParam
    {
        public int? RecipientId { get; set; }

        public string RecipientIdString { get; set; }
        public int? PaymentMethodType { get; set; }
        public int? OrderId { get; set; }
        /// <summary>
        /// نوع خرید  : اقساطی یا اعتباری یا نقدی
        /// </summary>
        public int? PurchaseType { get; set; }
        /// <summary>
        /// نیاز به تایید تایمن کننده قبل از پرداخت
        /// </summary>
        public bool RequiredSupplierApprovalBeforePayment { get; set; }
    }
}
