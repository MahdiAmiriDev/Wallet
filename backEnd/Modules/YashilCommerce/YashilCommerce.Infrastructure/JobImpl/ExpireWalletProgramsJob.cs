using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.JobImpl
{
    public class ExpireWalletProgramsJob : IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>Factory را برای اجرای سرویس Scoped در Job زمان‌بندی‌شده دریافت می‌کند.</summary>
        public ExpireWalletProgramsJob(IServiceScopeFactory scopeFactory) { _scopeFactory = scopeFactory; }

        /// <summary>اعتبار Assignmentها و وضعیت Programهای پایان‌یافته را در Scope مستقل منقضی می‌کند.</summary>
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IWalletProgramService>().ExpireDueProgramsAsync();
        }
    }
}
