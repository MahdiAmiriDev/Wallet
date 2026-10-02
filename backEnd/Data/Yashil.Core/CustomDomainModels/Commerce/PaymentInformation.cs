using Yashil.Core.Entities;

namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class PaymentInformation
    {
        public int? InstallmentCount { get; set; }
        public decimal? Total { get; set; }
        public int PaymentId { get; set; }
        public string ReservationNumber { get; set; }

        public Payment Payment { get; set; }
    }
}
