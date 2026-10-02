using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IProductSalesPlanningDiscountRepository : IGenericRepository<ProductSalesPlanningDiscount, int>
    {

        IQueryable<ProductSalesPlanningDiscount> GetByCustomFilter(int? productSalesPlanningId, int? customerClubId);

        /// <summary>
        /// بررسی می کند تخفیف برنامه ریزی محصول بر اساس باشگاه مشتریان وجود دارد
        /// </summary>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <param name="productSalePlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<bool> CheckExistProductSalePlanningDiscountAsync(int customerClubId, int productSalePlanningId);
        /// <summary>
        /// دریافت  تخفیف برنامه ریزی دوره
        /// </summary>
        /// <param name="applicationId">برنامه</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<ProductSalesPlanningDiscount> GetAllByApplicationId(int applicationId, bool readOnly);

        IQueryable<ProductSalesPlanningDiscount> FindById(int id);
    }
}
