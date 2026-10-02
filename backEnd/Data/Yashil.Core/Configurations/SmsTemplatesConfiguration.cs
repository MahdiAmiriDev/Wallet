namespace Yashil.Core.Configurations
{
    public class SmsTemplatesConfiguration : ISmsTemplatesConfiguration
    {
        /// <summary>
        /// شناسه قالب پیامک تایید
        /// </summary>
        public int SuccessfulOrderTemplateId { get; set; }
        /// <summary>
        /// شناسه قالب پیامک عدم تایید
        /// </summary>
        public int UnsuccessfulOrderTemplateId { get; set; }
    }
}
