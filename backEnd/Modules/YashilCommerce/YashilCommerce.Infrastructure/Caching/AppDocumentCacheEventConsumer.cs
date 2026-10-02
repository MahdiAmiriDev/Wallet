using System.Collections.Generic;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.AppEvents;
using Yashil.Common.Infrastructure.Caching;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.Caching
{
    public partial class AppDocumentCacheEventConsumer : CacheEventConsumer<AppDocument, int>
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;

        public AppDocumentCacheEventConsumer(IStaticCacheManager staticCacheManager, IAdditionalUserPropService additionalUserPropService, IUserPrincipal userPrincipal) : base(staticCacheManager)
        {
            _staticCacheManager = staticCacheManager;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
        }

        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="entityEventType">Entity event type</param>
        protected override async Task ClearCacheAsync(AppDocument entity, EntityEventType entityEventType)
        {
            List<Task> tasks = new List<Task>
            {
                RemoveAsync(YashilModelCacheDefaults<AppDocument>.ByIdCacheKey, $"{entity.DocTypeId}_{entity.ObjectId}")
            };
            if (entity.DocTypeId == (int)DocumentTypeEnum.ProductSalesPlanningImage)
            {
                var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                    YashilModelCacheDefaults<ProductSalesPlanning>.ImageByIdCacheKey, entity.ObjectId);
                tasks.Add(RemoveAsync(cacheKey));
            }

            if (entity.DocTypeId == (int)DocumentTypeEnum.CategoryIcon)
            {
                var cacheKey =
                    _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<Category>.ImageByIdCacheKey,
                        entity.ObjectId);
                tasks.Add(RemoveAsync(cacheKey));
            }

            //پاک کردن کش مربوط به تصاویر اسلایدر صفحه اصلی زمانی 
            //که تصویر جدیدی درج شود
            if (entity.DocTypeId == (int)DocumentTypeEnum.CmsImage || entity.DocTypeId == (int)DocumentTypeEnum.SliderMobileImage)
            {
                var sliderLevelOneCacheKey =
                    _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<YashilCms>.MainPageSliderLevelOne);
                var sliderLevelTwoCacheKey =
                    _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<YashilCms>.MainPageSliderLevelTwo);

                tasks.Add(RemoveAsync(sliderLevelOneCacheKey));
                tasks.Add(RemoveAsync(sliderLevelTwoCacheKey));
            }

            //حذف کش مربوط به مراکز فعالی که دارای تصویر حالت موبایل و دسکتاپ هستند
            if (entity.DocTypeId == (int)DocumentTypeEnum.CompanyDesktopLogo ||
                entity.DocTypeId == (int)DocumentTypeEnum.CompanyMobileLogo)
            {
                var additionalUserProp =
                    await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

                var activeCompaniesIfHasPictureCacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                    YashilModelCacheDefaults<Company>.ActiveCompaniesIfHasPictureKey, additionalUserProp.CustomerClubId);

                tasks.Add(RemoveAsync(activeCompaniesIfHasPictureCacheKey));
            }

            tasks.Add(base.ClearCacheAsync(entity, entityEventType));

            await Task.WhenAll(tasks);
        }

    }
}