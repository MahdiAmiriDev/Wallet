
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

            namespace YashilCommerce.Core.Services
{
	public interface IIssueService : IGenericService<Issue,int>
    {
                 
        IQueryable<Issue> GetByCustomFilter( int? orderItemId,int? orderId, int? paymentId, int? issueType, int? resolutionType, int? priorityType, int? issueStateType, int? assignee, int? issueOwner , string nonRegisteredUser = null);
        IQueryable<User> GetAssigneeByFiler(int? orderItemId,string recipientNationalCode = null, string mobileNumber = null);
    }
}      
 