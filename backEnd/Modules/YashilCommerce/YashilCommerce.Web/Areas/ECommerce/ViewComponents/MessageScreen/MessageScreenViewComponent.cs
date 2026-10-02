using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.MessageScreen
{
    public class MessageScreenViewComponent : ViewComponent
    {
        public async  Task<IViewComponentResult> InvokeAsync(string message)
        {
             ViewModels.MessageScreen contentMessage = new ()
             {
                 Message = message
             };

             return View("_MessageScreen", contentMessage);
        }
    }
}
