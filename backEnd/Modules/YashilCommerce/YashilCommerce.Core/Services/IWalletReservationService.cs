using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IWalletReservationService
    {
        Task<WalletReserveResultDto> ReserveAsync(WalletReserveRequestDto request);
    }
}
