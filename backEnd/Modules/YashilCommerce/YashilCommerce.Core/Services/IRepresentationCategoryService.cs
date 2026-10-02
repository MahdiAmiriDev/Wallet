using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IRepresentationCategoryService : IGenericService<RepresentationCategory, int>
    {
        IQueryable<RepresentationCategory> GetByCustomFilter(int? representationId, int? categoryId);
        IQueryable<Category> GetCompanyCategory(int id, bool onlyRootCategories, bool assigned);
        /// <summary>
        /// دریافت دسته بندی نمایندگی بر اساس شناسه نمایندگی
        /// </summary>
        /// <param name="representationId">شناسه نمایندگی</param>
        ///  <param name="readOnly">شناسه نمایندگی</param>
        /// <returns></returns>
        IQueryable<RepresentationCategory> GetByRepresentationId(int representationId,bool readOnly);
        /// <summary>
        /// بررسی می کند که این دسته بندی در دسنه بندی نمایندگی ها وجود دارد یا خیر
        /// </summary>
        /// <param name="categoryId">دسنه بندی</param>
        /// <returns></returns>
        Task<bool> CheckExistRepresentationCategoryByCategoryIdAsync(int categoryId);

        /// <summary>
        /// حذف دسته بندی نمایندگی بر اساس شناسه دسته بندی
        /// </summary>
        /// <param name="categoryId"></param>
        Task DeleteByCategoryIdAsync(int categoryId);
        /// <summary>
        /// حذف دسته بندی نمایندگی بر اساس شناسه نمایندگی
        /// </summary>
        /// <param name="representationId"></param>
        /// <returns></returns>
        Task DeleteByRepresentationIdAsync(int representationId);
    }
}
