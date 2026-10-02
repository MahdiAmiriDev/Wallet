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
    public class SaleConfigurationAttributeService : GenericService<SaleConfigurationAttribute, int>,
        ISaleConfigurationAttributeService
    {
        private readonly ISaleConfigurationAttributeRepository _saleConfigurationAttributeRepository;

        public SaleConfigurationAttributeService(IUnitOfWork unitOfWork,
            ISaleConfigurationAttributeRepository saleConfigurationAttributeRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, saleConfigurationAttributeRepository, userPrincipal, eventPublisher)
        {
            _saleConfigurationAttributeRepository = saleConfigurationAttributeRepository;
        }

        public IQueryable<SaleConfigurationAttribute> GetByCustomFilter(int? discountType, int? discountLimitationType,
            int? saleConfigurationId, int? customerClubId)
        {
            return _saleConfigurationAttributeRepository.GetByCustomFilter(discountType, discountLimitationType,
                saleConfigurationId, customerClubId);
        }

        /// <summary>
        /// دریافت ویژگی های تنظیم فروش بر اساس شناسه باشگاه مشتریان
        /// </summary>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <param name="saleConfigurationId">تنظیم فروش</param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IQueryable<SaleConfigurationAttribute> GetCustomerClubSaleConfigurationAttribute
        (
            int customerClubId,
            int saleConfigurationId,
            int applicationId
        )
        {
            return _saleConfigurationAttributeRepository
                .GetCustomerClubSaleConfigurationAttribute
                    (customerClubId, saleConfigurationId, applicationId);
        }

        public IQueryable<SaleConfigurationAttribute> GetProductSalesPlanningSaleConfigurationAttribute(int productSalesPlanningId, int customerClubId)
        {
            return _saleConfigurationAttributeRepository
                .GetProductSalesPlanningSaleConfigurationAttribute(productSalesPlanningId, customerClubId);
        }
    }
}
