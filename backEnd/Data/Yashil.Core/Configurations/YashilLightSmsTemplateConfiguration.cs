using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yashil.Core.Configurations
{
    public class YashilLightSmsTemplateConfiguration : IYashilLightSmsTemplateConfiguration
    {
        /// <summary>
        /// استفاده از پیامک برای هشدار شروع کلاس
        /// </summary>
        public bool UseSmsForUpcomingClass { get; set; }
        /// <summary>
        /// زمان بررسی کلاس ها حداقل 15 دقیقه حداکثر 24 ساعت
        /// </summary>
        public int TimeTresholdToCheckOngoingClasses { get; set; }
        /// <summary>
        /// شناسه قالب پیامک زمان قبل از شروع کلاس
        /// </summary>
        public int StartClassTresholdTemplateId { get; set; }
        /// <summary>
        ///شناسه قالب پیامک شروع کلاس
        /// </summary>
        public int StartClassTemplateId { get; set; }
        /// <summary>
        ///شناسه قالب پیامک عدم برگزاری کلاس
        /// </summary>
        public int CancelClassTemplateId { get; set; }
    }
}
