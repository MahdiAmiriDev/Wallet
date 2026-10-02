using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities; 
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class CustomerClubPaymentMethodTypeService : GenericService<CustomerClubPaymentMethodType,int>, ICustomerClubPaymentMethodTypeService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly ICustomerClubPaymentMethodTypeRepository _customerClubPaymentMethodTypeRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public CustomerClubPaymentMethodTypeService (IUnitOfWork unitOfWork, ICustomerClubPaymentMethodTypeRepository customerClubPaymentMethodTypeRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, customerClubPaymentMethodTypeRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_customerClubPaymentMethodTypeRepository = customerClubPaymentMethodTypeRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<CustomerClubPaymentMethodType> GetByCustomFilter( int? paymentMethodType, int? customerClubId)
        {
            return _customerClubPaymentMethodTypeRepository.GetByCustomFilter(paymentMethodType,customerClubId);
        }

        public IQueryable<PaymentMethodType> GetByUserId(int id)
        {
            return _customerClubPaymentMethodTypeRepository.GetByUserId(id);
        }

        public IQueryable<PaymentMethodType> GetByCustomerClubId(int customerClubId)
        {
            return _customerClubPaymentMethodTypeRepository.GetByCustomerClubId(customerClubId);
        }
    }
}      
 