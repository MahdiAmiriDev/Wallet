namespace Yashil.Core.Configurations
{
    /// <summary>
    /// the configuration fields of Saman Payment in AppSettings
    /// </summary>
    public interface ISamanPaymentConfiguration
    {
        string CallBackUrl { get; set; }
        string TerminalId { get; set; }
        string GenerateTokenUrl { get; set; }
        string RedirectToIpgUrl { get; set; }
        string VerifyTransactionUrl { get; set; }
        string ReverseTransactionUrl { get; set; }

    }
}
