using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface ICustomerWalletBalanceService
    {
        Task<CustomerWalletBalanceDto> GetCurrentUserBalanceAsync();
    }
}
