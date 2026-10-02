using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YashilCommerce.Core.Classes;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.RedirectToExternalSiteForReceiveProduct
{
    public class RedirectToExternalSiteForReceiveProductViewComponent : ViewComponent
    {
        /// <summary>
        /// این کامپوننت جهت نمایش لینک های مورد نیاز برای سفارش های که
        /// برای دریافت خدمات محصول خریداری شده نیاز است که کاربر به یک
        /// سامانه ی خارج از سامانه ی جاری که در آن اقدام به خرید محصول کرده است
        /// هدایت شود کاربرد دارد
        /// این کامپوننت بر اساس کلید تشخیص می دهد که باید لینک های مربوط به
        /// کدام یک از مراکز را که نیاز به هدایت به سایت خارجی دارد ایجاد کند
        /// </summary>
        /// <param name="key"></param>
        /// <param name="orderItemId">شناسه ایتم سفارش کاربر</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(string key, int orderItemId)
        {
            switch (key)
            {
                case RedirectToCompanySiteKey.MaktabKhoneRedirectKey:
                    return View("RedirectToExternalSiteForReceiveProduct", GetRedirectToMaktabkhoneApiLinks(key, orderItemId));
                default:
                    return View("Error");
            }
        }

        /// <summary>
        /// این متد جهت دریافت لینک های مورد نیاز
        /// جهت انجام فرآیند دریافت محصول مربوط به سایت مکتب خونه کاربرد دارد
        /// </summary>
        /// <returns></returns>
        private List<RedirectToExternalSiteForReceiveProductViewModel> GetRedirectToMaktabkhoneApiLinks(string key, int orderItemId)
        {
            return new List<RedirectToExternalSiteForReceiveProductViewModel>()
            {
                new RedirectToExternalSiteForReceiveProductViewModel
                {
                    ActionName = "RedirectToCompanySiteForReceiveProduct",
                    ControllerName = "Order",
                    ButtonText = "کاربر گرامی، برای دریافت خدمات بر روی",
                    ApiKey = key,
                    OrderItemId = orderItemId

                },
                new RedirectToExternalSiteForReceiveProductViewModel
                {
                    ActionName = "DoRegisterAndRedirectToHumaxServiceForCurrentUser",
                    ControllerName = "Order",
                    ButtonText = "کاربر گرامی، جهت ورود به آزمون هدایت شغلی بر روی",
                    ApiKey = key,
                    OrderItemId = orderItemId
                },
            };
        }
    }
}
