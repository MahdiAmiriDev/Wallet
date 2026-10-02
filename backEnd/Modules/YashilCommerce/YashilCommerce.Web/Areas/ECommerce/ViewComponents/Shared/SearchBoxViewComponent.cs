using Microsoft.AspNetCore.Mvc;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Shared
{
    public class SearchBoxViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View("_Searchbox");
        }
    }
}
