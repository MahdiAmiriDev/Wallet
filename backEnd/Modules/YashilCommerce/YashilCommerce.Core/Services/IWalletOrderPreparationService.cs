using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletOrderPreparationService
    {
        /// <summary>Payment و Reservation تلاش ترکیبی را با کلید پایدار در یک تراکنش آماده می‌کند.</summary>
        Task<WalletOrderPreparationResultDto> PrepareAsync(WalletOrderPreparationRequestDto request);
    }
}
