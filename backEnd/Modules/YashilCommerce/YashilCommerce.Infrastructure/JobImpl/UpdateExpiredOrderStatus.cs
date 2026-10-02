using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.JobImpl
{
    public class UpdateExpiredOrderStatus : BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;

        private readonly IServiceProvider _serviceProvider;
        private string Schedule => "*/30 * * * * *"; //Runs every 10 seconds

        public UpdateExpiredOrderStatus(IServiceProvider serviceProvider)
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var orderService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IOrderService>();
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
                if (now > _nextRun)
                {
                    Process(orderService);
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private void Process(IOrderService orderService)
        {
            try
            {
                orderService.CancelExpiredOrdersAsync(true);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
