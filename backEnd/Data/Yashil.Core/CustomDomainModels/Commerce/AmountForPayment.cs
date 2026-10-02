using Yashil.Core.Enums;

namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class AmountForPayment
    {
       //  public decimal? Amount { get; set; }
        public int? InstallmentCount { get; set; }
        public bool SubsidyBased { get; set; }
        public decimal Total { get; set; }
        public SaleTypesEnum SaleType { get; set; }
    }
}
