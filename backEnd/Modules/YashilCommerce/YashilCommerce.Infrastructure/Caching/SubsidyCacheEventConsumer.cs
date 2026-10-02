
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Caching;

namespace YashilCommerce.Infrastructure.Caching
{
    public class SubsidyCacheEventConsumer : CacheEventConsumer<Subsidy, int>
    {
        public SubsidyCacheEventConsumer(IStaticCacheManager staticCacheManager) : base(staticCacheManager)
        {
        }
        protected override Task ClearCacheAsync(Subsidy entity, EntityEventType entityEventType)
        {
            return Task.WhenAll(
                RemoveByPrefixAsync(YashilModelCacheDefaults<Subsidy>.GetCategoryGroupActiveSubsidyInfoPrefixCacheKey),
                base.ClearCacheAsync(entity)
            );
        }
    }
}
