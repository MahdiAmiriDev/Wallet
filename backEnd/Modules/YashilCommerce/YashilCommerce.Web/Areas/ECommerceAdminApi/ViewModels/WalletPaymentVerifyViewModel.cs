using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class WalletPaymentVerifyViewModel 
    {
     
        [StringValidation] public string ReservationNumber { get; set; }
        public int OrderId { get; set; }
        [StringValidation] public string Status { get; set; }
        [StringValidation] public string Token { get; set; }
    
    }
}
