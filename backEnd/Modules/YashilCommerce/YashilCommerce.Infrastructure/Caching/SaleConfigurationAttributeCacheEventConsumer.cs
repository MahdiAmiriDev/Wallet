using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Caching;

namespace YashilCommerce.Infrastructure.Caching
{
    internal class SaleConfigurationAttributeCacheEventConsumer : CacheEventConsumer<SaleConfigurationAttribute, int>
    {
        public SaleConfigurationAttributeCacheEventConsumer(IStaticCacheManager staticCacheManager) : base(staticCacheManager)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityEventType"></param>
        /// <returns></returns>
        protected override Task ClearCacheAsync(SaleConfigurationAttribute entity, EntityEventType entityEventType)
        {
            return Task.WhenAll(
                    RemoveByPrefixAsync(YashilModelCacheDefaults<ProductSalesPlanning>
                        .ProductSalesPlanningByFilterPrefixCacheKey),
                    RemoveByPrefixAsync(YashilModelCacheDefaults<ProductSalesPlanning>
                        .ProductDetailPrefixCacheKey),
                    RemoveByPrefixAsync(YashilModelCacheDefaults<City>.Prefix),
                    RemoveByPrefixAsync(YashilModelCacheDefaults<Category>.Prefix),
                    RemoveByPrefixAsync(YashilModelCacheDefaults<Company>.Prefix),
                    base.ClearCacheAsync(entity)
                );
        }

    }
}
