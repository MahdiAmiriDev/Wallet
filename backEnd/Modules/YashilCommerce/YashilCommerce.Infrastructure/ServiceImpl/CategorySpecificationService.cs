	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
    using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class CategorySpecificationService : GenericService<CategorySpecification,int>, ICategorySpecificationService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly ICategorySpecificationRepository _categorySpecificationRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public CategorySpecificationService (IUnitOfWork unitOfWork, ICategorySpecificationRepository categorySpecificationRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, categorySpecificationRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_categorySpecificationRepository = categorySpecificationRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<CategorySpecification> GetByCustomFilter( int? categoryId, int? specificationId)
        {
            return _categorySpecificationRepository.GetByCustomFilter(categoryId,specificationId);
        }
           

    }
}      
 