	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
    using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class CardAmountHistoryService : GenericService<CardAmountHistory,int>, ICardAmountHistoryService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly ICardAmountHistoryRepository _cardAmountHistoryRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public CardAmountHistoryService (IUnitOfWork unitOfWork, ICardAmountHistoryRepository cardAmountHistoryRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, cardAmountHistoryRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_cardAmountHistoryRepository = cardAmountHistoryRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
		}

		public IQueryable<CardAmountHistory> GetByCustomFilter(int? cardInfoId)
		{
			return _cardAmountHistoryRepository.GetByCustomFilter(cardInfoId);
		}
    }
}      
 