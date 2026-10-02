using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class OrderChangePermissionService : GenericService<OrderChangePermission,int>, IOrderChangePermissionService
    {
		private readonly IUnitOfWork _unitOfWork;  
		private readonly IEventPublisher _eventPublisher;
        private readonly IOrderChangePermissionRepository _orderChangePermissionRepository;
		private readonly IUserPrincipal _userPrincipal;

       
		public OrderChangePermissionService (IUnitOfWork unitOfWork, IOrderChangePermissionRepository orderChangePermissionRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, orderChangePermissionRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_orderChangePermissionRepository = orderChangePermissionRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
            

    }
}      
 