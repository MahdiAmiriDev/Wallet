	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class IssueService : GenericService<Issue,int>, IIssueService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IIssueRepository _issueRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public IssueService (IUnitOfWork unitOfWork, IIssueRepository issueRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, issueRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_issueRepository = issueRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }

        public IQueryable<User> GetAssigneeByFiler(int? orderItemId, string recipientNationalCode = null, string mobileNumber = null)
        {
            return _issueRepository.GetAssigneeByFiler(orderItemId, recipientNationalCode, mobileNumber);
        }

        public IQueryable<Issue> GetByCustomFilter( int? orderItemId,int? orderId, int? paymentId, int? issueType, int? resolutionType, int? priorityType, int? issueStateType, int? assignee, int? issueOwner , string nonRegisteredUser = null)
        {
            return _issueRepository.GetByCustomFilter(orderItemId,orderId, paymentId, issueType,resolutionType,priorityType,issueStateType,assignee,issueOwner,nonRegisteredUser);
        }
           

    }
}      
 