using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ICompanyRepository : IGenericRepository<Company, int>
    {
        /// <summary>
        /// دریافت کمپانی بر اساس پارامتر های فیلتر
        /// </summary>
        /// <param name="searchTitle">عنوان محصول</param>
        /// <param name="applicationId">برنامه</param>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <param name="cityId">شهر</param>
        /// <param name="categoryId">دسته بندی</param>
        /// <param name="categoryGroupId">گروه دسته بندی</param>
        /// <param name="orderByCompanyDisplayOrder"></param>
        /// <param name="isEmploymentCategoryGroup">آیا گروه دسته بندی اشتغال است</param>
        /// <returns></returns>
        IQueryable<Company> GetApplicationCompanies(
            string searchTitle,
            int applicationId,
            int customerClubId,
            int? cityId,
            int? categoryId,
            int? categoryGroupId,
            bool isEmploymentCategoryGroup,
            bool orderByCompanyDisplayOrder = true
            );
        IQueryable<City> GetCityByCompanyId(int? companyId);
        Company GetUserCompany(int userId);

        /// <summary>
        /// دریافت شهر ها بر اساس پارمتر های فیلتر
        /// </summary>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <param name="isEmploymentCategoryGroup">آیا گروه دسته بندی اشتغال است</param>
        /// <param name="showIfHasPrice"></param>
        /// <returns></returns>
        IQueryable<City> GetCityByFilter(int customerClubId, int? companyId, int? categoryId, int? categoryGroupId, bool isEmploymentCategoryGroup, bool showIfHasPrice = true);
        string GetTitle(int id);
        IQueryable<Company> GetCompaniesByCityId(int? cityId);
        IQueryable<Company> GetCompaniesHaveVoucherType(bool readOnly = true);
        IQueryable<Company> GetAllActiveCompanies(bool readOnly, int? applicationGroupId, int? applicationId);
        /// <summary>
        /// دریافت مرکز بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<Company> GetByProductSalePlanningAsync(int productSalePlanningId);

        IQueryable<Company> FindById(int id, bool readOnly = true);
        Task UpdateCodePathAsync();
        /// <summary>
        /// دریافت تمامی کمپانی ها بر اساس شناسه اپلیکیشن
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Company> GetAllCompanies(int applicationId, bool readOnly = false);

        /// <summary>
        /// دریافت مراکز به همراه فرزندان
        /// </summary>
        /// <param name="codePath"> codePath مرکز</param>
        /// <param name="applicationId">برنامه</param>
        /// <returns></returns>
        IQueryable<Company> GetCompaniesWithChildren(string codePath, int applicationId);

        /// <summary>
        /// دریافت پدر مرکز
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IQueryable<Company> FindParentCompany(int parentId);
    }
}
