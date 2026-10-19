using System.Linq;
using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletGrantService
    {
        /// <summary>Grantهای ثبت‌شده Program را فقط در Application جاری می‌خواند.</summary>
        IQueryable<WalletGrantDto> GetAll(int walletProgramId);

        /// <summary>Grant را با کنترل بودجه، Idempotency و ثبت اتمیک Ledger ایجاد می‌کند.</summary>
        Task<WalletGrantResultDto> CreateAsync(WalletGrantCreateDto request);
    }
}
