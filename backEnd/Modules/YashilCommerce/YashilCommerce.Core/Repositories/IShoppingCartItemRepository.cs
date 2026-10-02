using System;
using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using System.Threading.Tasks;

namespace YashilCommerce.Core.Repositories
{
    public interface IShoppingCartItemRepository : IGenericRepository<ShoppingCartItem, int>
    {

        IQueryable<ShoppingCartItem> GetByCustomFilter(int? shoppingCartType, Guid? code, int? productSalesPlanningId, int applicationId);
        ShoppingCartItem GetShoppingCartItem(int shoppingCartType, Guid code, int productSalesPlanningId, int applicationId);
        int GetShoppingCartCount(int shoppingCartType, Guid code, int applicationId);
        void ClearShoppingCart(int shoppingCart, Guid guidCode);
        /// <summary>
        /// ?تعداد آیتم های تکراری در سبد خرید را برمیگرداند
        /// </summary>
        /// <param name="cartType"></param>
        /// <param name="guidCode"></param>
        /// <param name="value"></param>
        /// <param name="representationId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<int> GetDuplicateItemQuantityAsync(int cartType, Guid guidCode, int productSalesPlanningId,int? representationId, int applicationId);
        /// <summary>
        /// آیا در سبد خرید محصولی وجود دارد که
        /// نیاز به تایید تامین کننده برای پرداخت باشد
        /// </summary>
        /// <param name="guidCode">کد سبد خرید</param>
        /// <returns></returns>
        Task<bool> CheckExistProductRequireSupplierApprovalBeforePaymentAsync(Guid guidCode);

        /// <summary>
        /// بررسی می کند آیا ایتمی در سبد خرید وجود دارد
        /// و از نوع نیاز به تایید تامین کننده قبل از خرید نباشد
        /// </summary>
        /// <param name="guidCode"></param>
        /// <returns></returns>
        Task<bool> CheckExistAnyItemInShoppingCartAndIsNotTypeOfProductRequireSupplierApprovalBeforePaymentAsync(Guid guidCode);
    }
}
