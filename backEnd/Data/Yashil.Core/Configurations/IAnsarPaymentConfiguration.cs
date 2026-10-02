namespace Yashil.Core.Configurations
{
    /// <summary>
    /// the configuration parameter for Ansar payment nafis IPG
    /// </summary>
    public interface IAnsarPaymentConfiguration
    {

        /// <summary>
        /// the Merchant ID
        /// </summary>
        string MID { get; set; }
        /// <summary>
        /// the TerminalId
        /// </summary>
        string TerminalId { get; set; }
        /// <summary>
        /// the redirect url
        /// </summary>
        string RequestUrl { get; set; }
        /// <summary>
        /// username for connection to nafis ansar IPG
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// password for connection to nafis ansar IPG
        /// </summary>
        string Password { get; set; }

        public string CallBackUrl { get; set; }

    }
}
