namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
    public class CardInfoDto
    {
        public int ApplicationId { get; set; }
        public int CreateBy { get; set; }
        public int CreatorOrganizationId { get; set; }
        public int OrganizationId { get; set; }
        public int CardTypeId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Gender { get; set; }
        public string NationalCode { get; set; }
        public string MobileNumber { get; set; }
        public string CardNumber { get; set; }
        public string CVV2 { get; set; }
        public string FirstPassword { get; set; }
        public string SecondPassword { get; set; }
        public int? ExpireDate { get; set; }
        public int CreditAmount { get; set; }
        public int CashAmount { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
