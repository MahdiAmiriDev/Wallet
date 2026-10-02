			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface ICategorySpecificationRepository : IGenericRepository<CategorySpecification,int>
    {
		          
        IQueryable<CategorySpecification> GetByCustomFilter( int? categoryId, int? specificationId);
           
		
    }
}      
 