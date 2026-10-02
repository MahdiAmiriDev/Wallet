using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Caching;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Web.Areas.ECommerce.DataService;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.HomePageCommercialImage
{
    /// <summary>
    /// این کامپوننت جهت دریافت و نمایش تصاویر تبلیغاتی سایت در صفحه ی اصلی
    /// کاربرد دارد
    /// محل نمایش تصاویر در صفحه ی اصلی پس از دسته بندی ها است
    /// </summary>
    public class HomePageCommercialImageViewComponent: ViewComponent
    {
        private readonly PublicDataService _publicDataService;
        private readonly IStaticCacheManager _cacheManager;

        public HomePageCommercialImageViewComponent(PublicDataService publicDataService, IStaticCacheManager cacheManager)
        {
            _publicDataService = publicDataService;
            _cacheManager = cacheManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheKey =
                _cacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<YashilCms>
                    .HomePageCommercialImageInfoKey);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _cacheManager.GetAsync(cacheKey, async () => 
            {
                var homePageCommercialImagesInfo = await _publicDataService.GetHomePageCommercialImageInfoAsync();

                if (homePageCommercialImagesInfo == null || !homePageCommercialImagesInfo.Any())
                    return View("HomePageCommercialImage");

                return View("HomePageCommercialImage", await _publicDataService.GetHomePageAdvertisingImagesDocumentInfoAsync(homePageCommercialImagesInfo));
            });

        }
    }
}
