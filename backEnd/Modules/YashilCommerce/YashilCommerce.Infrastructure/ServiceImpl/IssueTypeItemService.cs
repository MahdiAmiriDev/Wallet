	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class IssueTypeItemService : GenericService<IssueTypeItem,int>, IIssueTypeItemService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IIssueTypeItemRepository _issueTypeItemRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public IssueTypeItemService (IUnitOfWork unitOfWork, IIssueTypeItemRepository issueTypeItemRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, issueTypeItemRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_issueTypeItemRepository = issueTypeItemRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<IssueTypeItem> GetByCustomFilter( int? issueId, int? issueType)
        {
            return _issueTypeItemRepository.GetByCustomFilter(issueId,issueType);
        }
           

    }
}      
 