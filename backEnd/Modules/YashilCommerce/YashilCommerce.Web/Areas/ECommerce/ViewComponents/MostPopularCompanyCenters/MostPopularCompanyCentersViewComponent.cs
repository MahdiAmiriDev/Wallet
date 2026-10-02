using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YashilCommerce.Web.Areas.ECommerce.DataService;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.MostPopularCompanyCenters
{
    /// <summary>
    /// کامپوننت مربوط به نمایش محبوب ترین ترین مراکز آموزشی
    /// </summary>
    public class MostPopularCompanyCentersViewComponent : ViewComponent
    {
        private readonly PublicDataService _publicDataService;
        public MostPopularCompanyCentersViewComponent(PublicDataService publicDataService)
        {
            _publicDataService = publicDataService;
        }
        /// <summary>
        /// دریافت اطلاعات مربوط به محبوب ترین مراکز آموزشی
        /// در صورتی که دارای تصویر در حالت موبایل و دسکتاپ باشند
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var activeCompanies = await _publicDataService.GetActiveCompaniesIfHasPictureAsync();            

            return View(activeCompanies);
        }
    }
}
