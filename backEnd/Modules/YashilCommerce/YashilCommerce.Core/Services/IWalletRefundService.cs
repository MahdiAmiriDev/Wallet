using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletRefundService
    {
        /// <summary>Refund جزئی یا کامل Capture را با Ledger جدید و کاهش اتمیک مصرف ثبت می‌کند.</summary>
        Task<WalletRefundResultDto> RefundAsync(WalletRefundRequestDto request);
    }
}
