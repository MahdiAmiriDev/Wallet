using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class ProductSalesPlanningDiscountService : GenericService<ProductSalesPlanningDiscount, int>, IProductSalesPlanningDiscountService
    {
        private readonly IProductSalesPlanningDiscountRepository _productSalesPlanningDiscountRepository;

        public ProductSalesPlanningDiscountService(IUnitOfWork unitOfWork,
            IProductSalesPlanningDiscountRepository productSalesPlanningDiscountRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, productSalesPlanningDiscountRepository, userPrincipal, eventPublisher)
        {
            _productSalesPlanningDiscountRepository = productSalesPlanningDiscountRepository;
        }

        public IQueryable<ProductSalesPlanningDiscount> GetByCustomFilter(int? productSalesPlanningId, int? customerClubId)
        {
            return _productSalesPlanningDiscountRepository.GetByCustomFilter(productSalesPlanningId, customerClubId);
        }

        public Task<bool> CheckExistProductSalePlanningDiscountAsync
            (int customerClubId, int productSalePlanningId)
        {
            return _productSalesPlanningDiscountRepository
                .CheckExistProductSalePlanningDiscountAsync(customerClubId, productSalePlanningId);
        }

        public IQueryable<ProductSalesPlanningDiscount> GetAllByApplicationId(int applicationId, bool readOnly)
        {
            return _productSalesPlanningDiscountRepository.GetAllByApplicationId(applicationId, readOnly);
        }

        public IQueryable<ProductSalesPlanningDiscount> FindById(int id)
        {
            return _productSalesPlanningDiscountRepository.FindById(id);
        }

    }
}
 ;