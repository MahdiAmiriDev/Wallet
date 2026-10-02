namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class ProductSalesPlanningSaleConfiguration
    {
        public int ProductSalePlanningId { get; set; }
        public int SaleConfigurationId { get; set; }
        public bool? IsActive { get; set; }
        public int? RepresentationId { get; set; }
        public int CompanyId { get; set; }
        public bool AllowSetTaxInProduct { get; set; }
        public bool IsTaxExempt { get; set; }
        public int TaxCategoryId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal ProfitPercentage { get; set; }
        public string InstallmentExpression { get; set; }
        public bool InstallmentBased { get; set; }
        public int InstallmentCount { get; set; }
        public bool ExpressionBasedInstallmentCount { get; set; }
        public bool AllowSetInstallmentInProduct { get; set; }
    }
}
