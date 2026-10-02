			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IProductTypeSpecificationRepository : IGenericRepository<ProductTypeSpecification,int>
    {
		          
        IQueryable<ProductTypeSpecification> GetByCustomFilter( int? productTypeId, int? specificationId);
           
		
    }
}      
 