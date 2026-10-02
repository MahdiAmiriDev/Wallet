using Microsoft.AspNetCore.Mvc;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(int? errorId)
        {
            var message = "بروز خطا !";
            return View(errorId);
        }
    }
}
