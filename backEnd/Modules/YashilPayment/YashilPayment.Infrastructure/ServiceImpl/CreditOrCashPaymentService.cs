	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
    using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class CreditOrCashPaymentService : GenericService<CreditOrCashPayment,int>, ICreditOrCashPaymentService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly ICreditOrCashPaymentRepository _creditOrCashPaymentRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public CreditOrCashPaymentService (IUnitOfWork unitOfWork, ICreditOrCashPaymentRepository creditOrCashPaymentRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, creditOrCashPaymentRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_creditOrCashPaymentRepository = creditOrCashPaymentRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<CreditOrCashPayment> GetByCustomFilter( int? cardId, int? merchantId, int? status)
        {
            return _creditOrCashPaymentRepository.GetByCustomFilter(cardId,merchantId,status);
        }
           

    }
}      
 