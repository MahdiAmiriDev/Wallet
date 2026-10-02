
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IIssueCommentService : IGenericService<IssueComment,int>
    {
                 
        IQueryable<IssueComment> GetByCustomFilter( int? issueId);
           
		
    }
}      
 