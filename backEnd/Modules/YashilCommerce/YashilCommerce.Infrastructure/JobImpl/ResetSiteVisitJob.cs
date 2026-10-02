
using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.JobImpl
{
    /// <summary>
    /// این کلاس جهت فراخوانی سرویس ریست کردن مقدار
    /// متغییر استاتیک مربوط به تعداد بازدید های روز سایت
    /// کاربرد دارد
    /// چون این متغیر از نوع استاتیک است و تا زمانی که سرور را ریست نکینم مقدارش صفر نمی شود
    /// نیاز است که تعداد بازدید های سایت را در زمانبندی روزانه ریست کنیم
    /// </summary>
    public class ResetSiteVisitJob: IJob
    {
        private readonly IServiceProvider _serviceProvider;
        public ResetSiteVisitJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var siteVisitService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ISiteVisitService>(); 
            siteVisitService.ResetStaticSiteVisitCount();
            return Task.CompletedTask;
        }
    }
}
