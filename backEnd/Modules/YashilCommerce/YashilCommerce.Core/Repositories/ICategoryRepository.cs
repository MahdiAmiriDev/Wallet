using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category, int>
    {
        IQueryable<Category> GetByCompanyMainCategory(int companyId);
        IQueryable<Category> GetByCompanyMainCategoryId(int companyMainCategoryId);
        IQueryable<Category> GetRepresentationCategories(int representationId);
        IQueryable<Category> GetRepresentationCategories(int representationId, int categoryId);
        IQueryable<Category> GetByCustomFilter(int? parentId, int? companyId, int? representationId, bool loadLevelOneNodes);
        IQueryable<Category> GetApplicationCategories(int applicationId, int? companyId);
        IQueryable<int> GetRepresentationCategoriesId(int representationId, int categoryId);
        IEnumerable<CategoryCustomModel> GetUsedCategories(
            int? parentId,
            string searchTitle,
            int? company,
            int? representationId,
            int? cityId, int applicationId,
            bool loadLevelOneNodes,
            int customerClubId,
            string? categoryTitle,
            int? categoryGroupId,
            bool showIfProductHasPrice = true);
        IEnumerable<CategoryCustomModel> GetRootUsedCategories(
            int? parentId,
            int? company, 
            int? representationId, 
            int? cityId,
            int applicationId,
            bool loadLevelOneNodes,
            int customerClubId,
            int? categoryGroupId,
            bool showIfProductHasPrice = true);
        IQueryable<Category> GetCategoryWithParents(int categoryId);
        string GetTitle(int id);

        /// <summary>
        /// پایین ترین دسته محصول یک مرکز را به همراه دسته پدر آن بر می گرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IQueryable<LowLevelCategoryCustomModel> GetLowLevelCategoryWithParent(int companyId);
        /// <summary>
        /// دریافت لیست دسته لندی ای اصلی یک نمایندگی
        /// </summary>
        /// <param name="representationId"></param>
        /// <returns></returns>
        IQueryable<Category> GetRepresentationMainCategories(int representationId);
        ///// <summary>
        ///// برای دریافت دسته بندی هایی که با رسانه media در رابطه هستند
        ///// </summary>
        ///// <returns></returns>
        // IQueryable<Category> GetByMediaCustomFilter();

        /// <summary>
        /// بررسی میکند که همه دسته بندی های محصول مربوط به اپیلیکیشن دارای
        /// کد کالا و واحد سنجش هستند یا خیر 
        /// </summary>
        /// <returns></returns>
        bool CheckAllMainCategoriesHasUnitOfMeasurementAndCode();

        /// <summary>
        /// جهت بروزرسانی codepath
        /// </summary>
        /// <returns></returns>
        public Task UpdateCodePathAsync();

        /// <summary>
        /// دریافت دسته بندی های مرکز
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        IQueryable<Category> GetByCompanyId(int companyId, bool readOnly = true);
        /// <summary>
        /// آیا این دسته بندی به عنوان پدردسته بندی دیگر می باشد
        /// </summary>
        /// <param name="categoryId">دسته بندی</param>
        /// <returns></returns>
        Task<bool> CategoryIsParentAsync(int categoryId);
        /// <summary>
        /// دسته بندی را بر اساس شناسه بر می گرداند
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<Category> FindById(int id);
        /// <summary>
        /// نمایش دسته بندی ها برای لیست
        /// این متد دسته بندی هایی که
        /// CompanyId
        /// آنها Null
        /// باشد را به همراه
        /// دسته بندی ها کمپانی کاربر جاری بر می گرداند
        /// </summary>
        /// <returns></returns>
        Task<(IQueryable<Category> categories, int? currentUserCompanyId)> GetCurrentUserCategoriesAsync();

        /// <summary>
        /// دریافت دسته بندی ها بر اساس شناسه اپلیکیشن
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="readOnly"></param>
        /// <returns> IQueryable<Category></returns>
        IQueryable<Category> GetAllByApplicationId(int applicationId,bool readOnly = true);

        /// <summary>
        /// ردیافت
        /// CategoryTree
        /// مربوط به یک CodePath
        /// </summary>
        /// <param name="codePath"></param>
        /// <returns></returns>
        public IQueryable<Category> GetCategoryTreeByCodePath(string codePath);
    }
}
