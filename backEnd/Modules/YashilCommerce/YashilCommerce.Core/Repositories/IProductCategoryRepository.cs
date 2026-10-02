using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory, int>
    {

        IQueryable<ProductCategory> GetByCustomFilter(int? categoryId, int? productId, bool readOnly = true);
        /// <summary>
        /// برای دریافت لییست دسته بندی های محصول
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IQueryable<ProductCategory> GetProductCategories(int productId);

        void RemoveProductCategoriesByProductId(int productId, in bool saveAfterAdd);
    }
}
