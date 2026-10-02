
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IIssueParticipantService : IGenericService<IssueParticipant,int>
    {
                 
        IQueryable<IssueParticipant> GetByCustomFilter( int? issueId, int? userId);
           
		
    }
}      
 