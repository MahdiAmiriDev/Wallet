using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Caching;

namespace YashilCommerce.Infrastructure.Caching
{
    class ProductSalesPlanningCacheEventConsumer : CacheEventConsumer<ProductSalesPlanning, int>
    {
        private readonly IStaticCacheManager _staticCacheManager;

        public ProductSalesPlanningCacheEventConsumer(IStaticCacheManager staticCacheManager) : base(staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityEventType"></param>
        /// <returns></returns>
        protected override async Task ClearCacheAsync(ProductSalesPlanning entity, EntityEventType entityEventType)
        {
            await Task.WhenAll(

                RemoveByPrefixAsync(YashilModelCacheDefaults<ProductSalesPlanning>
                    .ProductSalesPlanningByFilterPrefixCacheKey),
                RemoveByPrefixAsync(YashilModelCacheDefaults<ProductSalesPlanning>
                    .ProductDetailPrefixCacheKey),
                RemoveByPrefixAsync(YashilModelCacheDefaults<City>.Prefix),
                // در هنگام بروزرسانی برنامه ریزی دوره کش دسته بندی های سامانه را پاک میکنیم زیرا ممکن است یک دسته بندی جدید باشد
                RemoveByPrefixAsync(YashilModelCacheDefaults<Category>.Prefix),
                RemoveByPrefixAsync(YashilModelCacheDefaults<Category>.FirstLevelCategoriesPrefixCacheKey)
            );
            await base.ClearCacheAsync(entity);
        }
    }
}
