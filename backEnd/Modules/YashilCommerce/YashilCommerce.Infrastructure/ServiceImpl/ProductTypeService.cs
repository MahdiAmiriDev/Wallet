	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class ProductTypeService : GenericService<ProductType,int>, IProductTypeService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IProductTypeRepository _productTypeRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public ProductTypeService (IUnitOfWork unitOfWork, IProductTypeRepository productTypeRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, productTypeRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_productTypeRepository = productTypeRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<ProductType> GetByCustomFilter( int? parentId)
        {
            return _productTypeRepository.GetByCustomFilter(parentId);
        }
           

    }
}      
 