using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class GiftCardTypeService : GenericService<GiftCardType,int>, IGiftCardTypeService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IGiftCardTypeRepository _giftCardTypeRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public GiftCardTypeService (IUnitOfWork unitOfWork, IGiftCardTypeRepository giftCardTypeRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, giftCardTypeRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_giftCardTypeRepository = giftCardTypeRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
            

    }
}      
 