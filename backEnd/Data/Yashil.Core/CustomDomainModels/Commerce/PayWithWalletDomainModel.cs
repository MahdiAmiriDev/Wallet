namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class PayWithWalletDomainModel
    {
        public int? CustomerClubId { get; set; } 
        public int? CustomerClubCardId { get; set; }
        public bool IsPayWithWallet { get; set; }
    }
}
