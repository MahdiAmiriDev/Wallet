using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class WalletEligibilityService : IWalletEligibilityService
    {
        private readonly YashilAppDbContext _context;

        /// <summary>دسترسی به Scopeهای Program و روابط واقعی اقلام سفارش را دریافت می‌کند.</summary>
        public WalletEligibilityService(YashilAppDbContext context) { _context = context; }

        /// <summary>برای هر قلم سفارش، شرط‌های Typeهای متفاوت را با AND و مقادیر هر Type را با OR می‌سنجد.</summary>
        public async Task<bool> IsOrderEligibleAsync(int applicationId, int walletProgramId, int orderId)
        {
            var scopes = await _context.WalletScope.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId && x.WalletProgramId == walletProgramId
                    && !x.Deleted && x.IsActive)
                .Select(x => new { x.WalletScopeType, x.CompanyId, x.RepresentationId, x.CityId, x.ProductId })
                .ToListAsync();
            if (scopes.Count == 0) return true;

            var items = await _context.OrderItem.AsNoTracking()
                .Where(x => x.OrderId == orderId && x.ApplicationId == applicationId && !x.Deleted)
                .Select(x => new
                {
                    x.RepresentationId,
                    RepresentationCompanyId = x.Representation.CompanyId,
                    RepresentationCityId = x.Representation.Address.CityId,
                    ProductId = x.ProductSalesPlanning.ProductId,
                    ProductCompanyId = x.ProductSalesPlanning.Product.CompanyId,
                    RepresentationApplicationId = x.Representation.ApplicationId,
                    PlanningApplicationId = x.ProductSalesPlanning.ApplicationId,
                    ProductApplicationId = x.ProductSalesPlanning.Product.ApplicationId,
                    RepresentationDeleted = x.Representation.Deleted,
                    ProductDeleted = x.ProductSalesPlanning.Product.Deleted,
                    PlanningDeleted = x.ProductSalesPlanning.Deleted
                })
                .ToListAsync();
            if (items.Count == 0) return false;

            var groups = scopes.GroupBy(x => x.WalletScopeType).ToList();
            return items.All(item => !item.RepresentationDeleted && !item.ProductDeleted
                && !item.PlanningDeleted && item.RepresentationApplicationId == applicationId
                && item.PlanningApplicationId == applicationId && item.ProductApplicationId == applicationId
                && groups.All(group => group.Any(scope => group.Key switch
                {
                    (int)WalletScopeTypeEnum.Company => scope.CompanyId.HasValue
                        && scope.CompanyId == item.RepresentationCompanyId
                        && scope.CompanyId == item.ProductCompanyId,
                    (int)WalletScopeTypeEnum.Representation => scope.RepresentationId.HasValue
                        && scope.RepresentationId == item.RepresentationId,
                    (int)WalletScopeTypeEnum.City => scope.CityId.HasValue
                        && scope.CityId == item.RepresentationCityId,
                    (int)WalletScopeTypeEnum.Product => scope.ProductId.HasValue
                        && scope.ProductId == item.ProductId,
                    _ => false
                })));
        }
    }
}
