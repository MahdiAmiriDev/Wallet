using System.Collections.Generic;
using System.Threading.Tasks;
using Yashil.Core.CustomViewModels.BaseViewModels;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.DataFactory
{
    public interface IPublicDataFactory
    {
        Task<CityViewModel> GetCity(int cityId);
        /// <summary>
        /// دریافت دسته بندی های استفاده شده در برنامه
        /// </summary>
        /// <param name="parentId">دسته بندی پدر</param>
        /// <param name="searchTitle">عنوان جستجو</param>
        /// <param name="companyId">مرکز</param>
        /// <param name="representationId">نمایندگی</param>
        /// <param name="cityId">شهر</param>
        /// <param name="categoryTitle">عنوان دسته بندی</param>
        /// <param name="applicationId">برنامه</param>
        /// <param name="loadLevelOneNodes">سطح اول دسته بندی</param>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <returns></returns>
        Task<List<CategorySimpleViewModel>> GetUsedCategoriesFactoryAsync(
            int? parentId,
            string searchTitle,
            int? companyId,
            int? representationId,
            int? cityId,
            string categoryTitle,
            int applicationId,
            bool loadLevelOneNodes,
            int? categoryGroupId = null);
        
        Task<List<CategoryViewModel>> GetCategoryWithParentsAsync(int categoryId);
    }


}
