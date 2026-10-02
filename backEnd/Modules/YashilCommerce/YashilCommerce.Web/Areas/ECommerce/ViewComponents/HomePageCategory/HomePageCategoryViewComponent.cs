using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YashilCommerce.Web.Areas.ECommerce.DataService;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.HomePageCategory
{
    /// <summary>
    /// کامپوننت نمایش دسته بندی ها در صفحه هوم
    /// </summary>
    public class HomePageCategoryViewComponent : ViewComponent
    {
        private readonly PublicDataService _publicDataService;

        public HomePageCategoryViewComponent(PublicDataService publicDataService)
        {
            _publicDataService = publicDataService;
        }
        /// <summary>
        /// دریافت اطلاعات مربوط به دسته بندی های قابل نمایش در صفحه ی هوم
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryData = await _publicDataService.GetUsedCategories();

            return View(categoryData);
        }

    }
}
