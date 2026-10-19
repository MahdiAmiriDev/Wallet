namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
    public class CardChargeModel
    {
        public int OrganizationId { get; set; }
        public int CardTypeId { get; set; }
        public int Amount { get; set; }
        public bool IsCredit { get; set; }

    }
}
