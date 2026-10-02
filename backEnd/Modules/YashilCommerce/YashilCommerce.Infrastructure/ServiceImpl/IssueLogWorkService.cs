	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class IssueLogWorkService : GenericService<IssueLogWork,int>, IIssueLogWorkService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IIssueLogWorkRepository _issueLogWorkRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public IssueLogWorkService (IUnitOfWork unitOfWork, IIssueLogWorkRepository issueLogWorkRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, issueLogWorkRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_issueLogWorkRepository = issueLogWorkRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<IssueLogWork> GetByCustomFilter( int? issueId, int? issueType, int? orderId, int? orderItemId, int? paymentId)
        {
            return _issueLogWorkRepository.GetByCustomFilter(issueId, issueType,orderId, orderItemId, paymentId);
        }
           

    }
}      
 