using System.Linq;
using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletScopeService
    {
        /// <summary>Scopeهای یک Program در Application جاری را برای مدیریت برمی‌گرداند.</summary>
        IQueryable<WalletScopeDto> GetAll(int walletProgramId);
        /// <summary>Targetهای نوع Scope را با دسترسی مدیریتی Program برمی‌گرداند.</summary>
        IQueryable<WalletScopeTargetDto> GetTargets(int scopeType);
        /// <summary>Scope مشخص را در Application جاری می‌خواند.</summary>
        Task<WalletScopeDto> GetAsync(long id);
        /// <summary>Scope را فقط برای Program در وضعیت Draft ایجاد می‌کند.</summary>
        Task<WalletScopeResultDto> CreateAsync(WalletScopeEditDto request);
        /// <summary>Scope را فقط تا پیش از فعال‌سازی Program ویرایش می‌کند.</summary>
        Task<WalletScopeResultDto> UpdateAsync(WalletScopeEditDto request);
        /// <summary>Scope را فقط در Draft به صورت نرم حذف می‌کند.</summary>
        Task<WalletScopeResultDto> DeleteAsync(WalletScopeDeleteDto request);
    }
}
