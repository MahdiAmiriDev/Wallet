using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.AppEvents;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Caching;

namespace YashilCommerce.Infrastructure.Caching
{
    public class CategoryGroupCacheEventConsumer : CacheEventConsumer<CategoryGroup, int>
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ApplicationConfiguration _applicationConfiguration;
        public CategoryGroupCacheEventConsumer(IStaticCacheManager staticCacheManager, ApplicationConfiguration applicationConfiguration):base(staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
            _applicationConfiguration = applicationConfiguration;
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="entityEventType">Entity event type</param>
        protected override Task ClearCacheAsync(CategoryGroup entity, EntityEventType entityEventType)
        {
            return Task.WhenAll(_staticCacheManager.RemoveAsync(YashilModelCacheDefaults<CategoryGroup>.CategoryGroupKey,
                _applicationConfiguration.ApplicationId),
                base.ClearCacheAsync(entity, entityEventType)
            );

        }
    }
}
