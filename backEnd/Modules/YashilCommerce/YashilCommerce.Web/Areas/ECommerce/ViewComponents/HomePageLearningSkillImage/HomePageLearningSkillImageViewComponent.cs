using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Caching;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Web.Areas.ECommerce.DataService;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.HomePageLearningSkillImage
{
    /// <summary>
    /// این کامپوننت جهت دریافت و نمایش تصاویر آموزش های مهارت محور در
    /// صفحه ی اصلی سامانه کاربرد دارد
    /// محل استفاده کامپوننت در صفحه ی اصلی پس از پرفروش ترین دوره ها است
    /// </summary>
    public class HomePageLearningSkillImageViewComponent : ViewComponent
    {
        private readonly PublicDataService _publicDataService;
        private readonly IStaticCacheManager _cacheManager;
        public HomePageLearningSkillImageViewComponent(PublicDataService publicDataService, IStaticCacheManager cacheManager)
        {
            _publicDataService = publicDataService;
            _cacheManager = cacheManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var cacheKey =
                _cacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<YashilCms>
                    .HomePageLearningSkillImageInfoKey);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var homePageLearningSkillImagesInfo = await _publicDataService.GetHomePageLearningSkillImageInfoAsync();

                if (homePageLearningSkillImagesInfo == null || !homePageLearningSkillImagesInfo.Any())
                    return View("HomePageLearningSkillImage");

                return View("HomePageLearningSkillImage",
                    await _publicDataService.GetHomePageAdvertisingImagesDocumentInfoAsync(
                        homePageLearningSkillImagesInfo));

            });

        }
    }
}
