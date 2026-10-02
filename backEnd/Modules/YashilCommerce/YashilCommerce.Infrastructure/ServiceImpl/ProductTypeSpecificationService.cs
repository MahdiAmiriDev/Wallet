	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class ProductTypeSpecificationService : GenericService<ProductTypeSpecification,int>, IProductTypeSpecificationService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IProductTypeSpecificationRepository _productTypeSpecificationRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public ProductTypeSpecificationService (IUnitOfWork unitOfWork, IProductTypeSpecificationRepository productTypeSpecificationRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, productTypeSpecificationRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_productTypeSpecificationRepository = productTypeSpecificationRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<ProductTypeSpecification> GetByCustomFilter( int? productTypeId, int? specificationId)
        {
            return _productTypeSpecificationRepository.GetByCustomFilter(productTypeId,specificationId);
        }
           

    }
}      
 