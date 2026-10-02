using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IProductGroupRepository : IGenericRepository<ProductGroup,int>
    {
        IQueryable<ProductGroup> GetByCustomFilter( int? parentId, int? categoryGroupId, bool? isIncludeTax);
    }
}      
 