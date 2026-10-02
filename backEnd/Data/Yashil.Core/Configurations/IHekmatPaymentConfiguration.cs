namespace Yashil.Core.Configurations
{
    /// <summary>
    /// the configuration of Hekmat payment 
    /// </summary>
    public interface IHekmatPaymentConfiguration
    {
        // merchantId or TerminalId 
        string MerchantId { get; set; }
        // username for connection to hekmat IPG
        string TerminalId { get; set; }
        // password for connection to hekmat IPG
        string PrivateKey { get; set; }
        // the url of ipg for redirecting
        string IpgUrl { get; set; }
        string EndpointAddress { get; set; }
    }
}
