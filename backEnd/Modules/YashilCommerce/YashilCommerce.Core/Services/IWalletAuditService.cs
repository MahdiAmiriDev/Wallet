using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletAuditService
    {
        /// <summary>مانده‌های Program را با Ledger و Reservationهای همان Application تطبیق و فقط مغایرت‌ها را گزارش می‌کند.</summary>
        Task<WalletAuditResultDto> AuditProgramAsync(int walletProgramId);
    }
}
