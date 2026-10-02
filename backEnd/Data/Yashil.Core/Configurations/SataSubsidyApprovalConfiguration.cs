namespace Yashil.Core.Configurations
{
    public class SataSubsidyApprovalConfiguration : ISataSubsidyApprovalConfiguration
    {
        // username for connection 
        public string UserName { get; set; }
        // password for connection
        public string Password { get; set; }
        // the url of call api
        public string Url { get; set; }

    }
}
