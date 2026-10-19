	using System.Linq;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class MerchantTerminalInfoService : GenericService<MerchantTerminalInfo,int>, IMerchantTerminalInfoService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMerchantTerminalInfoRepository _merchantTerminalInfoRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public MerchantTerminalInfoService (IUnitOfWork unitOfWork, IMerchantTerminalInfoRepository merchantTerminalInfoRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
			: base(unitOfWork, merchantTerminalInfoRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_merchantTerminalInfoRepository = merchantTerminalInfoRepository;
			_userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<MerchantTerminalInfo> GetByCustomFilter( int? merchantId)
        {
            return _merchantTerminalInfoRepository.GetByCustomFilter(merchantId);
        }
           

    }
}      
 