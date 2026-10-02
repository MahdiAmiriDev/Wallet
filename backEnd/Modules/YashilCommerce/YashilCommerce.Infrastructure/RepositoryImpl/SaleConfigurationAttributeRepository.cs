using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class SaleConfigurationAttributeRepository : GenericApplicationBasedRepository<SaleConfigurationAttribute, int>, ISaleConfigurationAttributeRepository
    {
        public SaleConfigurationAttributeRepository(YashilAppDbContext context,
            IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
        }


        public IQueryable<SaleConfigurationAttribute> GetByCustomFilter(int? discountType,
            int? discountLimitationType, int? saleConfigurationId, int? customerClubId)
        {
            var query = GetAll(true);
            if (discountType.HasValue)
            {
                query = query.Where(x => x.DiscountType == discountType.Value);
            }
            if (discountLimitationType.HasValue)
            {
                query = query.Where(x => x.DiscountLimitationType == discountLimitationType.Value);
            }
            if (saleConfigurationId.HasValue)
            {
                query = query.Where(x => x.SaleConfigurationId == saleConfigurationId.Value);
            }

            if (customerClubId.HasValue)
            {
                query = query.Where(x => x.CustomerClubId == customerClubId.Value);
            }

            return query;
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
            return GetAll(true, applicationId)
                .Where(x =>
                    x.SaleConfigurationId == saleConfigurationId &&
                    x.CustomerClubId == customerClubId).AsNoTracking();
        }

        public IQueryable<SaleConfigurationAttribute> GetProductSalesPlanningSaleConfigurationAttribute(int productSalesPlanningId, int customerClubId)
        {
            return DbSet.Where(x => 
                x.CustomerClubId == customerClubId
                                    && x.SaleConfiguration.ProductSalesPlanning
                                        .Any(sc => sc.Id == productSalesPlanningId)
            );
        }
    }
}
