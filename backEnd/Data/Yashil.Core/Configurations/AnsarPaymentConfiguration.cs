namespace Yashil.Core.Configurations
{
    /// <summary>
    /// ansar payment configuration properties
    /// </summary>
    public class AnsarPaymentConfiguration : IAnsarPaymentConfiguration
    {
        /// <summary>
        /// the Merchant ID
        /// </summary>
        public string MID { get; set; }
        /// <summary>
        /// the TerminalId
        /// </summary>
        public string TerminalId { get; set; }
        /// <summary>
        /// the ipg url for redirecting
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// username for connection to nafis ansar IPG
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// password for connection to nafis ansar IPG
        /// </summary>
        public string Password { get; set; }

        public string CallBackUrl { get; set; }
    }
}