namespace Yashil.Core.Configurations
{
    public interface ISaminPaymentConfiguration
    {
        string MerchantCode { get; set; }
        string TerminalCode { get; set; }
        string MerchantPassword { get; set; }
        string CallBackUrl { get; set; }
        string CallBackPath { get; set; }
        string GenerateTokenUrl { get; set; }
        string RedirectToIpgUrl { get; set; }
        string VerifyTransactionUrl { get; set; }
        string InquireTransactionUrl { get; set; }
    }
}
