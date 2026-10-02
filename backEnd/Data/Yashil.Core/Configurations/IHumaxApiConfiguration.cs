namespace Yashil.Core.Configurations
{
    public interface IHumaxApiConfiguration
    {
        /// <summary>
        /// نام همکار
        /// </summary>
        string PartnerName { get; set; }
        /// <summary>
        /// کلید
        /// </summary>
        string APIKey { get; set; }
        /// <summary>
        /// آدرس ارسالی
        /// </summary>
        string Url { get; set; }

    }
}
