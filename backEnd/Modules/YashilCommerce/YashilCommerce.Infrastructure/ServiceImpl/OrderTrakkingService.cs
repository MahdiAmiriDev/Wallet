	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class OrderTrakkingService : GenericService<OrderTrakking,int>, IOrderTrakkingService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IOrderTrakkingRepository _orderTrakkingRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public OrderTrakkingService (IUnitOfWork unitOfWork, IOrderTrakkingRepository orderTrakkingRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, orderTrakkingRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_orderTrakkingRepository = orderTrakkingRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<OrderTrakking> GetByCustomFilter( int? referenceTo, int? orderId, int? orderItemId)
        {
            return _orderTrakkingRepository.GetByCustomFilter(referenceTo,orderId,orderItemId);
        }
           

    }
}      
 