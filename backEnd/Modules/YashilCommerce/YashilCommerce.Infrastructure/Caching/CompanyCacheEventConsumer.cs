using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Caching;

namespace YashilCommerce.Infrastructure.Caching
{
    class CompanyCacheEventConsumer : CacheEventConsumer<Company, int>
    {
        public CompanyCacheEventConsumer(IStaticCacheManager staticCacheManager) : base(staticCacheManager)
        {
        }

        protected override Task ClearCacheAsync(Company entity, EntityEventType entityEventType)
        {
            return Task.WhenAll(
            RemoveByPrefixAsync(YashilModelCacheDefaults<ProductSalesPlanning>.ProductSalesPlanningByFilterPrefixCacheKey),
            RemoveByPrefixAsync(YashilModelCacheDefaults<Company>.Prefix),
            RemoveByPrefixAsync(YashilModelCacheDefaults<City>.Prefix),
            RemoveByPrefixAsync(YashilModelCacheDefaults<Category>.Prefix),
            base.ClearCacheAsync(entity)
            );
        }
    }
}
