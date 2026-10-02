			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IContentCategoryRepository : IGenericRepository<ContentCategory,int>
    {
		          
        IQueryable<ContentCategory> GetByCustomFilter( int? parentId);
           
		
    }
}      
 