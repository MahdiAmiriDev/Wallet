using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
	public interface IProductTagsRepository : IGenericRepository<ProductTags,int>
    {
        IQueryable<ProductTags> GetByCustomFilter( int? parentId, int? productId, int? tagType);


        /// <summary>
        /// دریافت تگ های مرتبط با برنامه ریزی دوره
        /// </summary>
        /// <param name="productSsalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns>ProductTags</returns>
        IQueryable<ProductTags> GetByProductSalesPlanningId(int productSsalesPlanningId);
    }
}      
 