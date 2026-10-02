using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Caching;

namespace YashilCommerce.Infrastructure.Caching
{
  public  partial class CategoryCacheEventConsumer : CacheEventConsumer<Category, int>
  {
      private  readonly IStaticCacheManager _staticCacheManager;
      public CategoryCacheEventConsumer(IStaticCacheManager staticCacheManager) : base(staticCacheManager)
      {
          _staticCacheManager = staticCacheManager;
      }
      /// <summary>
      /// Clear cache data
      /// </summary>
      /// <param name="entity">Entity</param>
      /// <param name="entityEventType">Entity event type</param>
      protected override Task ClearCacheAsync(Category entity, EntityEventType entityEventType)
      {
          return Task.WhenAll(
              RemoveByPrefixAsync(YashilModelCacheDefaults<Category>.FirstLevelCategoriesPrefixCacheKey),
              base.ClearCacheAsync(entity, entityEventType)
          );

      }
  }
}
