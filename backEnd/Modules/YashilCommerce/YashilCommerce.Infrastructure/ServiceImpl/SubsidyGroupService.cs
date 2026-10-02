using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents; 
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class SubsidyGroupService : GenericService<SubsidyGroup,int>, ISubsidyGroupService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly ISubsidyGroupRepository _subsidyGroupRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
		public SubsidyGroupService (IUnitOfWork unitOfWork, ISubsidyGroupRepository subsidyGroupRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, subsidyGroupRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_subsidyGroupRepository = subsidyGroupRepository;
			_userPrincipal = userPrincipal;
             _eventPublisher = eventPublisher;
        }
            

    }
}      
 