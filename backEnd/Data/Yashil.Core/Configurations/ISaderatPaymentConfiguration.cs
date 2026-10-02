namespace Yashil.Core.Configurations
{
    /// <summary>
    /// the configuration parameter for Saderat payment  IPG
    /// </summary>
    public interface ISaderatPaymentConfiguration
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
        /// username for connection to  Saderat IPG
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// password for connection to  Saderat IPG
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// CallBackUrl For TransactionResult
        /// </summary>
        string CallBackUrl { get; set; }
    }
}
