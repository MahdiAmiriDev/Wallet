using System.Linq;
using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletProgramService
    {
        /// <summary>Programهای Application جاری را برای فهرست مدیریتی برمی‌گرداند.</summary>
        IQueryable<WalletProgramDto> GetAll();
        /// <summary>جزئیات Program متعلق به Application جاری را برمی‌گرداند.</summary>
        Task<WalletProgramDto> GetAsync(int id);
        /// <summary>Program را با وضعیت Draft و مانده‌های مالی صفر ایجاد می‌کند.</summary>
        Task<WalletProgramResultDto> CreateAsync(WalletProgramEditDto request);
        /// <summary>فیلدهای مجاز را با کنترل تعهد مالی و نسخه رکورد تغییر می‌دهد.</summary>
        Task<WalletProgramResultDto> UpdateAsync(WalletProgramEditDto request);
        /// <summary>وضعیت Program را فقط طبق گذارهای مصوب تغییر می‌دهد.</summary>
        Task<WalletProgramResultDto> ChangeStatusAsync(WalletProgramStatusDto request);
        /// <summary>اعتبار Assignmentها و وضعیت Programهای سررسیدشده را در Job با Ledger اتمیک منقضی می‌کند.</summary>
        Task<int> ExpireDueProgramsAsync();
    }
}
