using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ISaleConfigurationAttributeRepository : IGenericRepository<SaleConfigurationAttribute, int>
    {

        IQueryable<SaleConfigurationAttribute> GetByCustomFilter(int? discountType, int? discountLimitationType, int? saleConfigurationId, int? customerClubId);
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
        );
        /// <summary>
        /// دریافت ویژگی های تنظیم فروش برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <returns></returns>
        IQueryable<SaleConfigurationAttribute> GetProductSalesPlanningSaleConfigurationAttribute
            (int productSalesPlanningId, int customerClubId);
    }
}
