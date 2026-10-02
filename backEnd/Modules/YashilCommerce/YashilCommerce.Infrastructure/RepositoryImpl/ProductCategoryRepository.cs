using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class ProductCategoryRepository : GenericApplicationBasedRepository<ProductCategory, int>, IProductCategoryRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public ProductCategoryRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<ProductCategory> GetByCustomFilter(int? categoryId, int? productId, bool readOnly = true)
        {
            var query = GetAll(true);
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (productId.HasValue)
            {
                query = query.Where(x => x.ProductId == productId);
            }


            return query;
        }

        public IQueryable<ProductCategory> GetProductCategories(int productId)
        {
            return DbSet.Where(x => x.ProductId == productId);
        }

        public void RemoveProductCategoriesByProductId(int productId, in bool saveAfterAdd)
        {
            var products = DbSet.Where(x => x.ProductId == productId);
            _context.RemoveRange(products);
        }
    }
}
