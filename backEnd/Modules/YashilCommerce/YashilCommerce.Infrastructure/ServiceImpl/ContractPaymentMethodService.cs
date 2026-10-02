	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
    using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class ContractPaymentMethodService : GenericService<ContractPaymentMethod,int>, IContractPaymentMethodService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IContractPaymentMethodRepository _contractPaymentMethodRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public ContractPaymentMethodService (IUnitOfWork unitOfWork, IContractPaymentMethodRepository contractPaymentMethodRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, contractPaymentMethodRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_contractPaymentMethodRepository = contractPaymentMethodRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<ContractPaymentMethod> GetByCustomFilter( int? contractId, int? paymentMethodType)
        {
            return _contractPaymentMethodRepository.GetByCustomFilter(contractId,paymentMethodType);
        }
           

    }
}      
 