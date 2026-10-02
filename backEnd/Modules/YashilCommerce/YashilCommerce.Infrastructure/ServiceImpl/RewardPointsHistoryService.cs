using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class RewardPointsHistoryService : GenericService<RewardPointsHistory,int>, IRewardPointsHistoryService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IRewardPointsHistoryRepository _rewardPointsHistoryRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public RewardPointsHistoryService (IUnitOfWork unitOfWork, IRewardPointsHistoryRepository rewardPointsHistoryRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, rewardPointsHistoryRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_rewardPointsHistoryRepository = rewardPointsHistoryRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
            

    }
}      
 