	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class OrderNoteService : GenericService<OrderNote,int>, IOrderNoteService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IOrderNoteRepository _orderNoteRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public OrderNoteService (IUnitOfWork unitOfWork, IOrderNoteRepository orderNoteRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, orderNoteRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_orderNoteRepository = orderNoteRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<OrderNote> GetByCustomFilter( int? orderId)
        {
            return _orderNoteRepository.GetByCustomFilter(orderId);
        }
           

    }
}      
 