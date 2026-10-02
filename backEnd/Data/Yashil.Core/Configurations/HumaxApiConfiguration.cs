namespace Yashil.Core.Configurations
{
    public class HumaxApiConfiguration:IHumaxApiConfiguration
    {
        public string PartnerName { get; set; }
        public string APIKey { get; set; }
        public string Url { get; set; }
    }
}
