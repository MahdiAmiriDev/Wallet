using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yashil.Core.Configurations
{
    public interface IYashilLightSmsTemplateConfiguration
    {
        /// <summary>
        /// استفاده از پیامک برای هشدار شروع کلاس
        /// </summary>
        bool UseSmsForUpcomingClass{get;set;}
        /// <summary>
        /// زمان بررسی کلاس ها
        /// </summary>
        int TimeTresholdToCheckOngoingClasses { get; set; }
        /// <summary>
        /// شناسه قالب پیامک زمان قبل از شروع کلاس
        /// </summary>
        int StartClassTresholdTemplateId { get; set; }
        /// <summary>
        ///شناسه قالب پیامک شروع کلاس
        /// </summary>
        int StartClassTemplateId { get; set; }
        /// <summary>
        ///شناسه قالب پیامک عدم برگزاری کلاس
        /// </summary>
        int CancelClassTemplateId { get; set; }       
    }
}
