namespace YashilPayment.Core.CustomDomainModels;

public class YashilHekmatTerminalInfoDomainModel
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string MerchantName { get; set; }
    public string Url { get; set; }
    public decimal SupplierWage {  get; set; }
    public decimal InstituteWage { get; set; }
    public decimal CashInstituteWage { get; set; }
    public decimal CashSupplierWage { get; set; }
}