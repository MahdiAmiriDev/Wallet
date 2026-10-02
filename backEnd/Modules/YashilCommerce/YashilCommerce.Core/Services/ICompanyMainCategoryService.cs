using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface ICompanyMainCategoryService : IGenericService<CompanyMainCategory, int>
    {
        Task<bool> AssignSelectedItemsToCompany(List<int> ints, int groupId, bool assign);
        IQueryable<CompanyMainCategory> GetMainCategoryByRepresentationId(int id);
        IQueryable<CompanyMainCategory> GetByCustomFilter(int? companyId, int? mainCategoryId);
        IQueryable<Category> GetCompanyCategory(int id, bool onlyRootCategories, bool assigned);
        /// <summary>
        /// بررسی می کند که آیا دسته بندی محصول از دسته بندی اصلی وجود دارد
        /// </summary>
        /// <param name="companyMainCategoryId">شناسه دسته بندی اصلی مرکز</param>
        /// <returns></returns>
        Task<bool> CheckExistsProductCategoryOfMainCategoryAsync(int companyMainCategoryId);
        /// <summary>
        /// آیا برای این مرکز دسته بندی اصلی وجود دارد
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <returns></returns>
        Task<bool> IsExistCompanyMainCategoryAsync(int companyId);

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