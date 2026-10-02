namespace Yashil.Core.Configurations
{
    /// <summary>
    /// شناسه قالب های پیامکی در سامانه
    /// sms.ir
    /// که برای ارسال پیامک شناسه این قالب ها به سرویس ارسال پیامک داده می شود
    /// </summary>
    public interface ISmsTemplatesConfiguration
    {
        /// <summary>
        /// شناسه قالب پیامک تایید
        /// </summary>
        int SuccessfulOrderTemplateId { get; set; }
        /// <summary>
        /// شناسه قالب پیامک عدم تایید
        /// </summary>
        int UnsuccessfulOrderTemplateId { get; set; }
    }
}
