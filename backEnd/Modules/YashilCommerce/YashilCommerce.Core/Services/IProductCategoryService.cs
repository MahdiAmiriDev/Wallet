
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IProductCategoryService : IGenericService<ProductCategory, int>
    {

        IQueryable<ProductCategory> GetByCustomFilter(int? categoryId, int? productId = null);
        void Update(ICollection<ProductCategory> productCategory, int productId);

         


        /// <summary>
        /// برای حذف دسته بندی های محصول 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task RemoveProductCategoriesByProductId(int productId, bool saveAfterAdd = false);

        IQueryable<ProductCategory> GetProductCategories(int productId);
    }
}
