			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IProductSpecificationOptionRepository : IGenericRepository<ProductSpecificationOption,int>
    {
		          
        IQueryable<ProductSpecificationOption> GetByCustomFilter( int? productId, int? specificationOption);
           
		
    }
}      
 