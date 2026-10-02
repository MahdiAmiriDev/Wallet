using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class ProductTemplateService : GenericService<ProductTemplate,int>, IProductTemplateService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IProductTemplateRepository _productTemplateRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public ProductTemplateService (IUnitOfWork unitOfWork, IProductTemplateRepository productTemplateRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, productTemplateRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_productTemplateRepository = productTemplateRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
            

    }
}      
 