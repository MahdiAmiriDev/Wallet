using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ICategoryGroupRepository : IGenericRepository<CategoryGroup, int>
    {
        /// <summary>
        /// دریافت گروه دسته بندی بر اساس شناسه اپلیکیشن
        /// </summary>
        /// <param name="employmentCategoryGroupId"></param>
        /// <param name="applicationId"></param>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <returns></returns>
        IQueryable<CategoryGroup> GetCategoryGroupByApplicationId(
            int employmentCategoryGroupId,
            int applicationId,
            int customerClubId);

        IQueryable<CategoryGroup> FindById(int id, int? applicationId, bool readOnly = true);
    }
}
