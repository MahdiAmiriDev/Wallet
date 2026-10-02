using System;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Enums;
using Yashil.Core.Web;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Enums;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class RedirectUrlService : IRedirectUrlService
    {
        private readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        public RedirectUrlService(IShoppingCartItemService shoppingCartItemService, IHttpContextAccessor httpContextAccessor, IAdditionalUserPropService additionalUserPropService, StoreConfiguration storeConfiguration)
        {
            _shoppingCartItemService = shoppingCartItemService;
            _httpContextAccessor = httpContextAccessor;
            _additionalUserPropService = additionalUserPropService;
            _storeConfiguration = storeConfiguration;
        }

        public string GetRedirectUrl(int? userId = null)
        {
            // اگر یوزری وجود داشته باشد و پروژه محدود به باشگاه مشتریان باشد
            // چک میشود که کاربر عضو باشگاه مشتریان باشد
            // اگر بود ادامه کار در غیر اینصورت باید به صفحه ثبت نام هدایت شود و مقدارregister را برمیگرداند
            if (userId.HasValue && _storeConfiguration.LimitedToCustomerClub)
            {
                var additionalUserProp = _additionalUserPropService.FindByUserId(userId.Value).FirstOrDefault();
                if (additionalUserProp?.CustomerClubId == null)
                {
                    return DefaultPages.Register;
                }
            }
            // در غیر این صورت کد سبد خرید دریافت میشود و اگر کاربر دارای سبد خرید باشد به سبد خرید هدایت میشود
            string code = _httpContextAccessor.HttpContext.Request.Cookies[CookiKey.CartId];
            if (!string.IsNullOrWhiteSpace(code))
            {
                if (Guid.TryParse(code, out var guidCode))
                {
                    var shoppingCartItemCount =
                        _shoppingCartItemService.GetShoppingCartItemsCount((int)ShoppingCartTypeEnum.ShoppingCart,
                            guidCode);
                    if (shoppingCartItemCount > 0)
                    {
                        return "/ShoppingCartItem";
                    }
                }
            }
            return null;
        }
    }
}
