			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IIssueTypeItemRepository : IGenericRepository<IssueTypeItem,int>
    {
		          
        IQueryable<IssueTypeItem> GetByCustomFilter( int? issueId, int? issueType);
           
		
    }
}      
 