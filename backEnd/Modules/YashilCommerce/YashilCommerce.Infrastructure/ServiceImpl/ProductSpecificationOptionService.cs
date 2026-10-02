	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class ProductSpecificationOptionService : GenericService<ProductSpecificationOption,int>, IProductSpecificationOptionService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IProductSpecificationOptionRepository _productSpecificationRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public ProductSpecificationOptionService (IUnitOfWork unitOfWork, IProductSpecificationOptionRepository productSpecificationRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, productSpecificationRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_productSpecificationRepository = productSpecificationRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<ProductSpecificationOption> GetByCustomFilter( int? productId, int? specificationOption)
        {
            return _productSpecificationRepository.GetByCustomFilter(productId,specificationOption);
        }
           

    }
}      
 