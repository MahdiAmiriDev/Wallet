using Microsoft.AspNetCore.Mvc;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.FooterParallax
{
    public class FooterParallaxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
