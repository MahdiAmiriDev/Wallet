			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IProductTypeRepository : IGenericRepository<ProductType,int>
    {
		          
        IQueryable<ProductType> GetByCustomFilter( int? parentId);
           
		
    }
}      
 