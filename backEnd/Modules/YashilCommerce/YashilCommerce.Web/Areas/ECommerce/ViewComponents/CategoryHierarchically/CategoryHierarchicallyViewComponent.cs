using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Web.Areas.ECommerce.CachedData;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using Yashil.Core.Enums;
using System.Linq;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.CategoryHierarchically
{
    /// <summary>
    /// کامپوننت نمایش دسته بندی ها
    /// </summary>
    public class CategoryHierarchicallyViewComponent : ViewComponent
    {
        private readonly CacheDataService _cacheDataService;
        private readonly IStaticCacheManager _staticCacheManager;
        public CategoryHierarchicallyViewComponent(CacheDataService cacheDataService,
            IStaticCacheManager staticCacheManager)
        {
            _cacheDataService = cacheDataService;
            _staticCacheManager = staticCacheManager;
        }

        /// <summary>
        /// دریافت لیست دسته بندی ها
        /// </summary>
        /// <param name="companyIds">شناسه کمپانی ها</param>
        /// <param name="searchTitle"></param>
        /// <param name="representationId">نمایندگی</param>
        /// <param name="cityId">شناسه شهر</param>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <param name="categoryGroupId">گروه دسته بندی</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(
            List<int> companyIds,
            int? representationId,
            string searchTitle,
            int? cityId,
            int? categoryId,
            int? categoryGroupId
            )
        {
            CategoryHierarchicallyModel categoryHierarchically = new CategoryHierarchicallyModel();

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Yashil.Core.Entities.Category>.CategoryHierarchicalModelList,
                companyIds,
                representationId,
                searchTitle,
                cityId,
                categoryId,
                categoryGroupId);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                categoryHierarchically.CategoryHierarchicalList = await _cacheDataService.GetCategoryHierarchicallyModel(
                    companyIds,
                    representationId,
                    searchTitle,
                    cityId,
                    categoryGroupId);

                if (categoryId.HasValue)
                {
                    categoryHierarchically.CategoryHierarchical = FindCategoryNodeById(categoryHierarchically.CategoryHierarchicalList, categoryId.ToString());
                    return View(categoryHierarchically);
                }

                return View(categoryHierarchically);
            });

        }

        /// <summary>
        /// جستجوی دسته بندی بر اساس شناسه دسته بندی
        /// </summary>
        /// <param name="categoryHierarchicals">لیست سلسله مراتبی دسته بندی ها</param>
        /// <param name="categoryId">شناسه دسته</param>
        /// <returns></returns>
        public CategoryHierarchicalViewModel FindCategoryNodeById(List<CategoryHierarchicalViewModel> categoryHierarchicals, string categoryId)
        {
            var categoryHierarchicalViewModel = new CategoryHierarchicalViewModel();

            foreach (var item in categoryHierarchicals)
            {
                if (item.Id == categoryId)
                {
                    return item;
                }
                if (item.SubCategories != null && item.SubCategories.Any())
                {
                    var categoryHierarchical = FindCategoryNodeById(item.SubCategories, categoryId);

                    if (!string.IsNullOrEmpty(categoryHierarchical.Id))
                    {
                        return categoryHierarchical;
                    }
                }
            }
            return categoryHierarchicalViewModel;
        }
    }
}
