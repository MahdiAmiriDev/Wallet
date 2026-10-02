	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class IssueCommentService : GenericService<IssueComment,int>, IIssueCommentService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IIssueCommentRepository _issueCommentRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public IssueCommentService (IUnitOfWork unitOfWork, IIssueCommentRepository issueCommentRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, issueCommentRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_issueCommentRepository = issueCommentRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<IssueComment> GetByCustomFilter( int? issueId)
        {
            return _issueCommentRepository.GetByCustomFilter(issueId);
        }
           

    }
}      
 