using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.JobImpl
{

    /// <summary>
    ///   بر اساس مصرف یارانه سفارشات تایید شده یا منتظر تایید یا پرداخت ناقص
    ///   اگر یارانه مصرف شده از سقف یارانه در ساتا و وزارت بیشتر مساوی باشد
    ///   یارانه ساتا و وزارت برای هر گروه دسته بندی یارانه
    ///  غیر فعال می گردد
    ///  این Job
    ///  هر 12 ساعت اجرا می شود
    /// </summary>
    public class UpdateSubsidyStatusBySubsidyConsumption : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateSubsidyStatusBySubsidyConsumption(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var subsidyService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ISubsidyService>();
            subsidyService.UpdateSubsidyStatusBySubsidyConsumption();
            return Task.CompletedTask;
        }
    }
}
