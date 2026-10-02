using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YashilCommerce.Web.Areas.ECommerce.DataService;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.FullOfDiscountsComponent
{
    /// <summary>
    /// کامپوننت نمایش کروسل مربوط به صفحه ی هوم
    /// </summary>
    public class FullOfDiscountsComponentViewComponent : ViewComponent
    {
        private readonly PublicDataService _publicDataService;

        public FullOfDiscountsComponentViewComponent(PublicDataService publicDataService)
        {
            _publicDataService = publicDataService;
        }

        /// <summary>
        /// دریافت اطلاعات مربوط به کروسل دوره ها و تنظیم جزئیات نمایش آن
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
           
           
        }
    }
}
