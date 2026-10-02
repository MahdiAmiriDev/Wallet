			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IIssueCommentRepository : IGenericRepository<IssueComment,int>
    {
		          
        IQueryable<IssueComment> GetByCustomFilter( int? issueId);
           
		
    }
}      
 