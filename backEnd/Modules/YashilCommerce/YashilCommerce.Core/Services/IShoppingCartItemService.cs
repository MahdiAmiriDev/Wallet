using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IShoppingCartItemService : IGenericService<ShoppingCartItem, int>
    {

        IQueryable<ShoppingCartItem> GetByCustomFilter(int? shoppingCartType, Guid? code, int? productSalesPlanningId);
        Task AddShoppingCartItemAync(ShoppingCartItem shoppingCartItem, bool saveAfterAdd = false);

        /// <summary>
        /// انجام عملیات حذف کالا از سبد خرید کاربر
        /// در صورتی که عملیات موفقیت آمیز انجام شود مقدار
        /// empty
        /// برمیگرداند و در غیر این صورت پیام خطا.
        /// </summary>
        /// <param name="cartType"></param>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="saveAfterDelete"></param>
        /// <returns></returns>
        Task<string> DeleteShoppingCartItemAsync(int cartType, int id, Guid code, bool saveAfterDelete);
        int GetShoppingCartItemsCount(int shoppingCartType, Guid code);

        bool ClearShoppingCart(int shoppingCartType, Guid guidCode, bool saveAfterClear);
        /// <summary>
        /// ?تعداد آیتم های تکراری در سبد خرید را برمیگرداند
        /// </summary>
        /// <param name="cartType"></param>
        /// <param name="guidCode"></param>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        Task<int> GetDuplicateItemQuantityAsync(int cartType, Guid guidCode, int productSalesPlanningId, int? representationId);

        Task<bool> CheckAllSaleConfigurationsHasSameInstallmentCountAsync(int shoppingCartType, Guid guidCode,
            int productSalesPlanningId,
            List<SaleConfigurationAttributeDto> saleConfigurations);
        /// <summary>
        /// آیا در سبد خرید محصولی وجود دارد که
        /// نیاز به تایید تامین کننده برای پرداخت باشد
        /// </summary>
        /// <param name="guidCode">کد سبد خرید</param>
        /// <returns></returns>
        Task<bool> CheckExistProductRequireSupplierApprovalBeforePaymentAsync(Guid guidCode);

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر محصولی در سبد خرید خود اضافه کرده است یا نه؟
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> AnyItemExistsInShoppingCartAsync(Guid code);

        /// <summary>
        /// بررسی می کند آیا ایتمی در سبد خرید وجود دارد
        /// و از نوع نیاز به تایید تامین کننده قبل از خرید نباشد
        /// </summary>
        /// <param name="guidCode"></param>
        /// <returns></returns>
        Task<bool> CheckExistAnyItemInShoppingCartAndIsNotTypeOfProductRequireSupplierApprovalBeforePaymentAsync(Guid guidCode);
    }
}
