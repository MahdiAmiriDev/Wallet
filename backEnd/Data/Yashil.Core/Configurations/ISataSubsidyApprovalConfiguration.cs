namespace Yashil.Core.Configurations
{
    public interface ISataSubsidyApprovalConfiguration
    {
        // username for connection 
        string UserName { get; set; }
        // password for connection
        string Password { get; set; }
        // the url of call api
        string Url { get; set; }
       
    }

}
