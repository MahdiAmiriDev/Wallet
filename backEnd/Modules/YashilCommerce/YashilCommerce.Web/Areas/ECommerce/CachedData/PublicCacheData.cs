using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Interfaces;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.CachedData
{
    public class CacheDataService
    {
        private readonly ICompanyService _companyService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly ICategoryService _categoryService;


        public CacheDataService(IStaticCacheManager staticCacheManager,
              ApplicationConfiguration applicationConfiguration,
              ICompanyService companyService,
              IMapper mapper, IAdditionalUserPropService additionalUserPropService, IUserPrincipal userPrincipal, ICategoryService categoryService)
        {
            _staticCacheManager = staticCacheManager;
            _applicationConfiguration = applicationConfiguration;
            _companyService = companyService;
            _mapper = mapper;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
            _categoryService = categoryService;
        }

        /// <summary>
        /// لیست مراکز فعال که محصول برای فروش دارند
        /// </summary>
        /// <param name="searchTitle"></param>
        /// <param name="cityId"></param>
        /// <param name="categoryId"></param>
        /// <param name="categoryGroupId">گروه دذسته بندی</param>
        /// <returns></returns>
        public async Task<List<CompanyViewModel>> GetActiveCompaniesByFilterAsync(
            string searchTitle,
            int? cityId,
            int? categoryId,
            int? categoryGroupId)
        {

            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Company>.GetAllByCityAndCategory,
                searchTitle, cityId, categoryId, categoryGroupId, userAdditionalUserProp.CustomerClubId);

            cacheKey.CacheTime = (int)CacheTime.Default;
            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var applicationCompanies = await _companyService.
                    GetApplicationCompaniesAsync(
                        searchTitle,
                        _applicationConfiguration.ApplicationId,
                        userAdditionalUserProp.CustomerClubId,
                        cityId,
                        categoryId,
                        categoryGroupId);

                return await applicationCompanies
                    .ProjectTo<CompanyViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            });
        }

        /// <summary>
        /// لیست دسته بندی هایی که در آن محصول فعال وجود دارد را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public Task<List<CategoryHierarchicalViewModel>> GetCategoryHierarchicallyModel()
        {
            return GetCategoryHierarchicallyModel(new List<int>(), null, null, null);
        }
        /// <summary>
        /// لیست دسته بندی های مربوط به هر کمپانی و نمایندگی را که در آن محصول فعال وجود دارد را بر می گرداند
        /// </summary>
        /// <returns></returns>
        public Task<List<CategoryHierarchicalViewModel>> GetCategoryHierarchicallyModel(
            List<int> company,
            int? representationId,
            string searchTitle,
            int? cityId,
            int? categoryGroupId = null)
        {
            int? companyId = null;
            if (company != null && company.Count > 0)
            {
                companyId = company.FirstOrDefault();
            }

            var nodeUrl = CreateNodeUrl(searchTitle, companyId, representationId, cityId, categoryGroupId);
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Category>.GetAllByCompanyAndCity,
                companyId,
                representationId,
                searchTitle,
                cityId,
                categoryGroupId
                );
            cacheKey.CacheTime = (int)CacheTime.Default;
            return _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var applicationCategories = await _categoryService.GetUsedCategoriesAsync(
                    null,
                    searchTitle,
                    companyId,
                    representationId,
                    cityId, null,
                    _applicationConfiguration.ApplicationId,
                    false,
                    categoryGroupId
                    );
                var applicationCategoryList = _mapper.Map<List<CategorySimpleViewModel>>(applicationCategories);
                var levelOneCategories = applicationCategories.Where(x => !x.ParentId.HasValue);
                var subCategories = levelOneCategories
                    .Select(x => new CategoryHierarchicalViewModel
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Icon = x.CssClass,
                        Href = nodeUrl + $"categoryId={x.Id}"
                    }).ToList();
                BuildCategoryTree(searchTitle, companyId, representationId, cityId, categoryGroupId, subCategories, applicationCategoryList);
                return subCategories;
            });
        }

        private void BuildCategoryTree(
            string searchTitle,
            int? companyId,
            int? representationId,
            int? cityId,
            int? categoryGroupId,
            IEnumerable<CategoryHierarchicalViewModel> subCategories,
            IReadOnlyCollection<CategorySimpleViewModel> applicationCategories)
        {
            var nodeUrl = CreateNodeUrl(searchTitle, companyId, representationId, cityId, categoryGroupId);
            foreach (var subcategory in subCategories)
            {
                subcategory.SubCategories = applicationCategories.Where(cn => cn.ParentId == int.Parse(subcategory.Id)).Select(c => new CategoryHierarchicalViewModel
                {
                    Id = c.Id.ToString(),
                    Title = c.Title,
                    Icon = c.CssClass,
                    Href = nodeUrl + $"categoryId={c.Id}"
                }).ToList();
                if (subcategory.SubCategories != null)
                {
                    if (subcategory.SubCategories.Any())
                    {
                        BuildCategoryTree(
                            searchTitle,
                            companyId,
                            representationId,
                            cityId,
                            categoryGroupId,
                            subcategory.SubCategories,
                            applicationCategories);
                    }
                    else
                    {
                        subcategory.SubCategories = null;
                    }
                }
            }
        }

        /// <summary>
        /// ساخت url
        /// با توجه به پارامتر های موجود در لینک آدرس
        /// </summary>
        /// <param name="searchTitle"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="cityId"></param>
        /// <param name="categoryGroupId"></param>
        /// <returns></returns>
        private string CreateNodeUrl(
            string searchTitle,
            int? companyId,
            int? representationId,
            int? cityId,
            int? categoryGroupId)
        {
            var nodeUrl = "Company?";
            if (!string.IsNullOrEmpty(searchTitle))
            {
                nodeUrl += $"searchTitle={searchTitle}&";
            }
            if (companyId.HasValue)
            {
                nodeUrl += $"company={companyId.Value}&";
            }
            if (representationId.HasValue)
            {
                nodeUrl += $"representationId={representationId.Value}&";
            }
            if (cityId.HasValue)
            {
                nodeUrl += $"cityId={cityId.Value}&";
            }
            if (categoryGroupId.HasValue)
            {
                nodeUrl += $"categoryGroupId={categoryGroupId.Value}&";
            }
            return nodeUrl;
        }

    }
}
