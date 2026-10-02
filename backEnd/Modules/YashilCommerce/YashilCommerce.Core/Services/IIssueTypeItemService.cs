
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IIssueTypeItemService : IGenericService<IssueTypeItem,int>
    {
                 
        IQueryable<IssueTypeItem> GetByCustomFilter( int? issueId, int? issueType);
           
		
    }
}      
 