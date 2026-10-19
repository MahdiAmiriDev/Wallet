using System.Collections.Generic;
using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface ICustomerOrderFundingService
    {
        Task<IReadOnlyDictionary<int, CustomerOrderFundingDto>> GetCurrentUserOrdersAsync(IEnumerable<int> orderIds);
    }
}
