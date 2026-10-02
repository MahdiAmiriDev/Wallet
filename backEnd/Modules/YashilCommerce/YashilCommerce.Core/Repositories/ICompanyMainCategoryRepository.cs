using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ICompanyMainCategoryRepository : IGenericRepository<CompanyMainCategory, int>
    {
        Task AssignSelectedItemsToCompany(List<int> mainCategories, int companyId, bool assign);
        IQueryable<CompanyMainCategory> GetMainCourseCategoriesByCompanyMainCategory(int? id);
        IQueryable<CompanyMainCategory> GetMainCategoryByRepresentationId(int representationId);
        IQueryable<CompanyMainCategory> GetMainCourseCategoriesByCategoryId(int categoryId, in bool hierarchical = true);
        IQueryable<CompanyMainCategory> GetByCustomFilter(int? companyId, int? mainCategoryId);
        IQueryable<Category> GetCompanyCategory(int id, bool onlyRootCategories, bool assigned);
        /// <summary>
        /// بررسی می کند که آیا دسته بندی محصول از دسته بندی اصلی وجود دارد
        /// </summary>
        /// <param name="companyMainCategoryId">شناسه دسته بندی اصلی مرکز</param>
        /// <returns></returns>
        Task<bool> CheckExistsProductCategoryOfMainCategoryAsync(int companyMainCategoryId);
        /// <summary>
        /// آبا این دسته بندی در دسته بندی اصلی مرکز وجود دارد
        /// </summary>
        /// <param name="categoryId">دسته بندی</param>
        /// <returns></returns>
        Task<bool> CheckExistCompanyMainCategoryByCategoryIdAsync(int categoryId);

        IQueryable<CompanyMainCategory> FindById(int id, bool readOnly = true);

        /// <summary>
        /// حذف دسته بندی مرکز بر اساس شناسه دسته بندی
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task DeleteByCategoryIdAsync(int categoryId);
        /// <summary>
        /// حذف بر اساس شناسه مرکز
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task DeleteByCompanyIdAsync(int companyId);
    }
}