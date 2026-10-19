	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents; 
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
	public class PaymentGatewayPaymentMethodService : GenericService<PaymentGatewayPaymentMethod,int>, IPaymentGatewayPaymentMethodService
    {
		private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGatewayPaymentMethodRepository _paymentGatewayPaymentMethodRepository;
		private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
		public PaymentGatewayPaymentMethodService (IUnitOfWork unitOfWork, IPaymentGatewayPaymentMethodRepository paymentGatewayPaymentMethodRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, paymentGatewayPaymentMethodRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_paymentGatewayPaymentMethodRepository = paymentGatewayPaymentMethodRepository;
			_userPrincipal = userPrincipal;
             _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<PaymentGatewayPaymentMethod> GetByCustomFilter( int? paymentGatewayId, int? paymentMethodType)
        {
            return _paymentGatewayPaymentMethodRepository.GetByCustomFilter(paymentGatewayId,paymentMethodType);
        }
           

    }
}      
 