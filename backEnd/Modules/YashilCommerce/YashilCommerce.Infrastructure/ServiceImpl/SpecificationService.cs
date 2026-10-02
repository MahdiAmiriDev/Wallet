using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class SpecificationService : GenericService<Specification,int>, ISpecificationService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly ISpecificationRepository _specificationRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public SpecificationService (IUnitOfWork unitOfWork, ISpecificationRepository specificationRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, specificationRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_specificationRepository = specificationRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
            

    }
}      
 