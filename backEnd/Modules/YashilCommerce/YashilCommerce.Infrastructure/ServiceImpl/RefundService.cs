	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents; 
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class RefundService : GenericService<Refund,int>, IRefundService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IRefundRepository _refundRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
		public RefundService (IUnitOfWork unitOfWork, IRefundRepository refundRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, refundRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_refundRepository = refundRepository;
			_userPrincipal = userPrincipal;
             _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<Refund> GetByCustomFilter( int? orderId)
        {
            return _refundRepository.GetByCustomFilter(orderId);
        }
           

    }
}      
 