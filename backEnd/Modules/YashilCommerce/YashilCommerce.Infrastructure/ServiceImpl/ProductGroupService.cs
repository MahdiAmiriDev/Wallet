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
    public class ProductGroupService : GenericService<ProductGroup, int>, IProductGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductGroupRepository _ProductGroupRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IEventPublisher _eventPublisher;
        public ProductGroupService(IUnitOfWork unitOfWork, IProductGroupRepository ProductGroupRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, ProductGroupRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _ProductGroupRepository = ProductGroupRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
        }


        public IQueryable<ProductGroup> GetByCustomFilter(int? parentId, int? categoryGroupId, bool? isIncludeTax)
        {
            return _ProductGroupRepository.GetByCustomFilter(parentId, categoryGroupId, isIncludeTax);
        }


        public IQueryable<ProductGroup> GetByCategoryGroupId(int categoryGroupId)
        {
            return GetAll(true).Where(pg => pg.CategoryGroupId.Value == categoryGroupId);
        }

        public IQueryable<ProductGroup> GetByMainCode(string mainCode)
        {
            return GetAll(true).Where(pg => pg.MainCode == mainCode);
        }

        public IQueryable<ProductGroup> GetByCode(string code)
        {
            return GetAll(true).Where(x => x.Code == code);
        }
    }
}
