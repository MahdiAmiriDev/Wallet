using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YashilCommerce.Web.Areas.ECommerce.DataService;


namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.HomePageSlider
{
    /// <summary>
    /// کامپوننت اسلایدر صفحه ی هوم
    /// </summary>
    public class HomePageSliderViewComponent : ViewComponent
    {
        private readonly PublicDataService _publicDataService;
        public HomePageSliderViewComponent(PublicDataService publicDataService)
        {
            _publicDataService = publicDataService;
        }
        /// <summary>
        /// دریافت اطلاعات مربوط به اسلایدر صفحه ی هوم با توجه به این که آیا در وبسایت عمومی هستیم یا نه
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliderData = await _publicDataService.GetMainPageSliderLevelOneIfHasPictureAsync(_publicDataService.IsPublicPage());

            return View(sliderData);
        }
    }
}
