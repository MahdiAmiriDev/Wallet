using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Caching;

namespace YashilCommerce.Infrastructure.Caching
{
    class RepresentationCacheEventConsumer : CacheEventConsumer<Representation, int>
    {
        private readonly IStaticCacheManager _staticCacheManager;
        public RepresentationCacheEventConsumer(IStaticCacheManager staticCacheManager) : base(staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
        }

        protected override Task ClearCacheAsync(Representation entity, EntityEventType entityEventType)
        {
            return Task.WhenAll(
                  RemoveByPrefixAsync(YashilModelCacheDefaults<City>.Prefix),
                  RemoveAsync(YashilModelCacheDefaults<City>.GetCityByCompanyIdCacheKey, entity.CompanyId),
                  RemoveByPrefixAsync(YashilModelCacheDefaults<Representation>.Prefix)
             );
        }
    }
}
