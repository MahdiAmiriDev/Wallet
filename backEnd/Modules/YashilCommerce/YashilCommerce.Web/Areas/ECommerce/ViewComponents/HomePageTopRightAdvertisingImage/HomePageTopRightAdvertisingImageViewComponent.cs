using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using YashilCommerce.Web.Areas.ECommerce.DataService;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.HomePageTopRightSliderAdvertisingImage
{
    /// <summary>
    /// این کامپوننت برای دریافت تصاویر تبلیغاتی بالا سمت راست صفحه اصلی کاربرد دارد
    /// </summary>
    public class HomePageTopRightAdvertisingImageViewComponent : ViewComponent
    {
        private readonly PublicDataService _publicDataService;
        private readonly IStaticCacheManager _cacheManager;
        public HomePageTopRightAdvertisingImageViewComponent(PublicDataService publicDataService, IStaticCacheManager cacheManager = null)
        {
            _publicDataService = publicDataService;
            _cacheManager = cacheManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {            
            return View("HomePageTopRightAdvertisingImage", await _publicDataService.GetHomePageTopRightAdvertisingImageAsync());
        }
    }
}
