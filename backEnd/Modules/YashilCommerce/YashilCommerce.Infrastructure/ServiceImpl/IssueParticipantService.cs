	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class IssueParticipantService : GenericService<IssueParticipant,int>, IIssueParticipantService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IIssueParticipantRepository _issueParticipantRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public IssueParticipantService (IUnitOfWork unitOfWork, IIssueParticipantRepository issueParticipantRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, issueParticipantRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_issueParticipantRepository = issueParticipantRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<IssueParticipant> GetByCustomFilter( int? issueId, int? userId)
        {
            return _issueParticipantRepository.GetByCustomFilter(issueId,userId);
        }
           

    }
}      
 