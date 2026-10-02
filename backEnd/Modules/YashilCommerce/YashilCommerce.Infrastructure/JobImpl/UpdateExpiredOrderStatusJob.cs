using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.JobImpl
{
    public class UpdateExpiredOrderStatusJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        public UpdateExpiredOrderStatusJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var orderService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IOrderService>();
            orderService.CancelExpiredOrdersAsync(true);
            return Task.CompletedTask;
        }
    }
}