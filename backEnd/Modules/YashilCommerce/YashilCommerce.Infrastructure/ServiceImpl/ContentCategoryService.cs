	using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities; using Yashil.Common.Core.AppEvents;
    using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
	public class ContentCategoryService : GenericService<ContentCategory,int>, IContentCategoryService
    {
		private readonly IUnitOfWork _unitOfWork;  private readonly IEventPublisher _eventPublisher;
        private readonly IContentCategoryRepository _contentCategoryRepository;
		private readonly IUserPrincipal _userPrincipal;
       
		public ContentCategoryService (IUnitOfWork unitOfWork, IContentCategoryRepository contentCategoryRepository, IUserPrincipal userPrincipal,IEventPublisher eventPublisher)
			: base(unitOfWork, contentCategoryRepository,userPrincipal,eventPublisher)
        {
			_unitOfWork = unitOfWork;
			_contentCategoryRepository = contentCategoryRepository;
			_userPrincipal = userPrincipal;  _eventPublisher = eventPublisher;
        }
                 
        public IQueryable<ContentCategory> GetByCustomFilter( int? parentId)
        {
            return _contentCategoryRepository.GetByCustomFilter(parentId);
        }
           

    }
}      
 