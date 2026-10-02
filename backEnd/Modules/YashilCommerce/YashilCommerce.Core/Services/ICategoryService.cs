using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using Yashil.Core.CustomDomainModels.Commerce;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface ICategoryService : IGenericService<Category, int>
    {
        IQueryable<Category> GetByCompanyMainCategory(int companyId);
        IQueryable<Category> GetByCompanyMainCategoryId(int companyMainCategoryId);
        IQueryable<Category> GetRepresentationCourseCategories(int representationId);
        IQueryable<Category> GetByCustomFilter(int? parentId, int? companyId, int? representationId, bool loadLevelOneNodes = false);
        IQueryable<Category> GetApplicationCategories(int applicationId, int? companyId = null);
        Task<IEnumerable<CategoryCustomModel>> GetUsedCategoriesAsync(
            int? parentId, string searchTitle, int? companyId,
            int? representationId, int? cityId, string? categoryTitle,
            int applicationId, bool loadLevelOneNodes,
            int? categoryGroupId = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentId">category Id</param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="cityId"></param>
        /// <param name="applicationId"></param>
        /// <param name="loadLevelOneNodes"></param>
        /// <param name="customerClubId"></param>
        /// <param name="categoryGroupId"></param>
        /// <returns></returns>
        IEnumerable<CategoryCustomModel> GetRootUsedCategories(
            int? parentId,
            int? companyId,
            int? representationId,
            int? cityId,
            int applicationId,
            bool loadLevelOneNodes,
            int customerClubId,
            int? categoryGroupId = null
            );

        IEnumerable<CategoryCustomModel> GetUsedCategoriesByFilters(
            int? parentId,
            string searchTitle,
            int? companyId,
            int? representationId,
            int? cityId,
            string? categoryTitle,
            int applicationId,
            bool loadLevelOneNodes,
            int customerClubId,
            int? categoryGroupId = null);
        IQueryable<Category> GetCategoryWithParents(int categoryId);
        string GetTitle(int id);

        /// <summary>
        /// پایین ترین دسته محصول یک مرکز را به همراه دسته پدر آن بر می گرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IQueryable<LowLevelCategoryCustomModel> GetLowLevelCategoryWithParent(int companyId);

        IQueryable<Category> GetRepresentationMainCategories(int representationId);
        ///// <summary>
        ///// برای دریافت لیست دسته بندی هایی که با رسانه media در ارتباط هستند مورد استفاده قرار میگیرد
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
        /// بروزرسانی codepath Category
        /// </summary>
        public Task UpdateCodePathAsync();
        /// <summary>
        /// آیا برای مرکز دسته بندی تعریف شده است
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <returns></returns>
        Task<bool> IsExistCategoryForCompanyAsync(int companyId);
        /// <summary>
        /// بررسی می کند که اجازه حذف دسته بندی وجود دارد 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<(bool allowDelete, string message)> AllowDeleteCategoryAsync(int categoryId);

        /// <summary>
        /// اختصاص دسته بندی درج شده به مراکز و کمپانی
        /// </summary>
        /// <param name="categoryId">دسته بندی</param>
        Task AssignToCompanyAndRepresentationAsync(int categoryId);

        IQueryable<Category> FindById(int id);

        Task<bool> IsRootCategoryAsync(int categoryId);
        /// <summary>
        /// نمایش دسته بندی ها برای لیست :
        /// این متد دسته بندی هایی که
        /// CompanyId
        /// آنها Null
        /// باشد را به همراه
        /// دسته بندی ها کمپانی کاربر جاری بر می گرداند
        /// </summary>
        /// <returns></returns>
        Task<(IQueryable<Category> categories, int? currentUserCompanyId)> GetCurrentUserCategories();

        /// <summary>
        /// این متد جهت جستجو در بین عنوان دسته بندی ها بر اساس کلید داده شده
        /// را انجام می دهد
        /// </summary>
        /// <param name="searchKey">کلید جستجو</param>
        /// <param name="customerClubId">شناسه باشگاه مشتری</param>
        /// <returns>IEnumerable<CategoryCustomModel></returns>
        IEnumerable<CategoryCustomModel> SearchInCategoryTitle(string searchKey, int customerClubId);
        /// <summary>
        /// دسته بندی را از دسته های مرکز دسته بندی و های نمایندگی حذف می کنیم  
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task RemoveCategoryFromCompanyRepresentationCategoryAsync(int categoryId);


        /// <summary>
        /// دریافت
        /// codePath
        /// مربوط به یک Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Task<string> GetProductCategoryCodePathAsync(int productId);

        /// <summary>
        /// ردیافت
        /// CategoryTree
        /// مربوط به یک CodePath
        /// </summary>
        /// <param name="codePath"></param>
        /// <returns></returns>
        public IQueryable<Category> GetCategoryTreeByCodePath(string codePath);



        /// <summary>
        /// دریافت برترین دسته بندی ها 
        /// براساس تعداد دوره و ترتیب نمایش
        /// </summary>
        /// <param name="topCount">تعداد آیتم های قابل نمایش</param>
        /// <returns></returns>
        Task<List<TopCategoryDto>> GetTopCategoriesAsync(int topCount = 5);
    }
}
