using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletSettlementService
    {
        /// <summary>رزرو کامل با پرداخت موفق را یک‌بار Capture و در Ledger ثبت می‌کند.</summary>
        Task<WalletSettlementResultDto> CaptureAsync(WalletCaptureRequestDto request);

        /// <summary>رزرو باز را یک‌بار Release می‌کند، بدون ایجاد Ledger قطعی.</summary>
        Task<WalletSettlementResultDto> ReleaseAsync(WalletReleaseRequestDto request);
    }
}
