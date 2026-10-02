using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.JobImpl
{
    public class SendMessageOnOrderStateChangeJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        public SendMessageOnOrderStateChangeJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var orderService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IOrderService>();
            orderService.SendMessageForOrder();
            return Task.CompletedTask;
        }
    }
}