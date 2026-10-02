namespace Yashil.Core.Configurations
{
    /// <summary>
    /// the configuration of Hekmat payment 
    /// </summary>
    public class HekmatPaymentConfiguration: IHekmatPaymentConfiguration
    {
        // merchantId or TerminalId 
        public string MerchantId { get; set; }
        // username for connection to hekmat IPG
        public string TerminalId { get; set; }
        // password for connection to hekmat IPG
        public string PrivateKey { get; set; }
        // the url of ipg for redirecting
        public string IpgUrl { get; set; }
        public string EndpointAddress { get; set; }

    }
}
