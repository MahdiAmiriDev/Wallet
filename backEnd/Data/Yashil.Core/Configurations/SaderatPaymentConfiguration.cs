namespace Yashil.Core.Configurations
{
    /// <summary>
    /// Saderat payment configuration properties
    /// </summary>
    public class SaderatPaymentConfiguration : ISaderatPaymentConfiguration 
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
        /// username for connection to Saderat IPG
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// password for connection to Saderat IPG
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// CallBackUrl For TransactionResult
        /// </summary>
        public string CallBackUrl { get; set; }
    }
}
