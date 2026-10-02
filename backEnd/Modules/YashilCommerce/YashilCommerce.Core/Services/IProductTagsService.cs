using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IProductTagsService : IGenericService<ProductTags, int>
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
