			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IIssueParticipantRepository : IGenericRepository<IssueParticipant,int>
    {
		          
        IQueryable<IssueParticipant> GetByCustomFilter( int? issueId, int? userId);
           
		
    }
}      
 