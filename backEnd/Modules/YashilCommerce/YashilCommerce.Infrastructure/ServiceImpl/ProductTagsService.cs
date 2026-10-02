using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;


namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ProductTagsService : GenericService<ProductTags, int>, IProductTagsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductTagsRepository _productTagsRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        public ProductTagsService(IUnitOfWork unitOfWork, IProductTagsRepository productTagsRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, productTagsRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _productTagsRepository = productTagsRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
        }


        public IQueryable<ProductTags> GetByCustomFilter(int? parentId, int? productId, int? tagType)
        {
            return _productTagsRepository.GetByCustomFilter(parentId, productId, tagType);
        }

        /// <summary>
        /// دریافت تگ های مرتبط با برنامه ریزی دوره
        /// </summary>
        /// <param name="productSsalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns>ProductTags</returns>
        public IQueryable<ProductTags> GetByProductSalesPlanningId(int productSsalesPlanningId)
        {
            return _productTagsRepository.GetByProductSalesPlanningId(productSsalesPlanningId);
        }
    }
}
