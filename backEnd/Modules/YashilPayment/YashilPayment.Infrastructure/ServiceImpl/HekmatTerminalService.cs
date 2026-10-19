using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents; 
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class HekmatTerminalService : GenericService<HekmatTerminal,int>, IHekmatTerminalService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IHekmatTerminalRepository _hekmatTerminalRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
		public HekmatTerminalService (IUnitOfWork unitOfWork, IHekmatTerminalRepository hekmatTerminalRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, hekmatTerminalRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_hekmatTerminalRepository = hekmatTerminalRepository;
			_userPrincipal = userPrincipal;
             _eventPublisher = eventPublisher;
        }
            

    }
}      
 