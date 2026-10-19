using System.Threading.Tasks;

namespace YashilCommerce.Core.Services
{
    public interface IWalletEligibilityService
    {
        /// <summary>سازگاری همه اقلام سفارش با Scopeهای فعال Program را بررسی می‌کند.</summary>
        Task<bool> IsOrderEligibleAsync(int applicationId, int walletProgramId, int orderId);
    }
}
