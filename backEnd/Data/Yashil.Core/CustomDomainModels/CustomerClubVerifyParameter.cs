namespace Yashil.Core.CustomDomainModels
{
    public class CustomerClubVerifyParameter
    {
        public int? CustomerClubId { get; set; }
        public string NationalCode { get; set; }
        public string CardNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool HasCardNumber { get; set; }
    }
}
