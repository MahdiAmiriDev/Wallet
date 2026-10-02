using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ShoppingCartItemRepository : GenericRepository<ShoppingCartItem, int>, IShoppingCartItemRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public ShoppingCartItemRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<ShoppingCartItem> GetByCustomFilter(int? shoppingCartType, Guid? code, int? productSalesPlanningId, int applicationId)
        {
            var query = _context.ShoppingCartItem.Where(x => x.ApplicationId == applicationId);
            if (shoppingCartType.HasValue)
            {
                query = query.Where(x => x.ShoppingCartType == shoppingCartType.Value);
            }
            if (code.HasValue)
            {
                query = query.Where(x => x.Code == code.Value);
            }
            if (productSalesPlanningId.HasValue)
            {
                query = query.Where(x => x.ProductSalesPlanningId == productSalesPlanningId.Value);
            }
            return query.AsNoTracking();
        }

        public ShoppingCartItem GetShoppingCartItem(int shoppingCartType, Guid code, int productSalesPlanningId, int applicationId)
        {
            return GetByCustomFilter(shoppingCartType, code, productSalesPlanningId, applicationId).FirstOrDefault();
        }

        public int GetShoppingCartCount(int shoppingCartType, Guid code, int applicationId)
        {
            return GetByCustomFilter(shoppingCartType, code, null, applicationId).Sum(x => x.Quantity);
        }

        public void ClearShoppingCart(int shoppingCartType, Guid guidCode)
        {
            var shoppingCartItems = DbSet.Where(x => x.Code == guidCode && x.ShoppingCartType == shoppingCartType);
            _context.ShoppingCartItem.RemoveRange(shoppingCartItems);
        }

        public async Task<int> GetDuplicateItemQuantityAsync(int cartType, Guid guidCode, int productSalesPlanningId, int? representationId, int applicationId)
        {
            var query = _context.ShoppingCartItem.AsNoTracking().Where(x => x.Code == guidCode && x.ApplicationId == applicationId
             && x.ProductSalesPlanningId == productSalesPlanningId);

            if (representationId.HasValue)
            {
                query = query.Where(x => x.RepresentationId == representationId);
            }
            return await query.Select(x=>x.Quantity).FirstOrDefaultAsync();

        }

        public Task<bool> CheckExistProductRequireSupplierApprovalBeforePaymentAsync(Guid guidCode)
        {
            return DbSet.Where(x => x.Code == guidCode)
                  .AnyAsync(c =>
                      c.ProductSalesPlanning.SaleConfiguration.RequireSupplierApprovalBeforePayment ||
                      c.ProductSalesPlanning.Product.RequireSupplierApprovalBeforePayment);
        }

        public Task<bool> CheckExistAnyItemInShoppingCartAndIsNotTypeOfProductRequireSupplierApprovalBeforePaymentAsync(Guid guidCode)
        {
            return DbSet
                .AnyAsync(c =>
                    c.Code == guidCode &&
                    !c.ProductSalesPlanning.SaleConfiguration.RequireSupplierApprovalBeforePayment &&
                    !c.ProductSalesPlanning.Product.RequireSupplierApprovalBeforePayment);
        }
    }
}
