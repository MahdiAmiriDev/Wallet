using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Services;
using Yashil.Common.Core.Classes;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Core.CustomViewModels.ECommerce;

namespace YashilCommerce.Web.DataFactory
{
    public class PublicDataFactory : IPublicDataFactory
    {
        // GET: /<controller>/
        private readonly IMapper _mapper;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICityService _cityService;
        private readonly ICategoryService _categoryService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;

        public PublicDataFactory(
            IStaticCacheManager staticCacheManager,
            ICityService cityService,
            IMapper mapper, ICategoryService categoryService, IAdditionalUserPropService additionalUserPropService,
            IUserPrincipal userPrincipal)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
            _staticCacheManager = staticCacheManager;
            _cityService = cityService;
        }


        public async Task<Yashil.Core.CustomViewModels.BaseViewModels.CityViewModel> GetCity(int cityId)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<City>.GetAllCitiesCacheKey);
            cacheKey.CacheTime = (int)CacheTime.TwentyFourHour;
            var cachedCities = await _staticCacheManager.GetAsync(cacheKey, async () =>
              {
                  var allCities = _cityService.GetAll();
                  return await allCities.ProjectTo<Yashil.Core.CustomViewModels.BaseViewModels.CityViewModel>(_mapper.ConfigurationProvider).ToListAsync();
              });
            return cachedCities.FirstOrDefault(x => x.Id == cityId);
        }

        public async Task<List<CategorySimpleViewModel>> GetUsedCategoriesFactoryAsync(
            int? parentId,
            string searchTitle,
            int? companyId,
            int? representationId,
            int? cityId,
            string categoryTitle,
            int applicationId,
            bool loadLevelOneNodes,
            int? categoryGroupId = null)
        {


            var additionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            //  در صورتی که متن جستجوی کاربر مقدار داشته باشد دسته بندی ها را کش نمی کنیم 
            if (!string.IsNullOrEmpty(searchTitle))
            {
                var levelOneUsedCategories = _categoryService.GetUsedCategoriesByFilters(
                    parentId,
                    searchTitle,
                    companyId,
                    representationId,
                    cityId,
                    categoryTitle,
                    applicationId,
                    loadLevelOneNodes,
                    additionalUserProp.CustomerClubId,
                    categoryGroupId);
                return _mapper.Map<List<CategorySimpleViewModel>>(levelOneUsedCategories);
            }
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Category>.GetUsedCategories,
                parentId,
                companyId,
                representationId,
                cityId,
                applicationId,
                loadLevelOneNodes,
                categoryGroupId
            );
            cacheKey.CacheTime = (int)CacheTime.Default;
            return await _staticCacheManager.GetAsync(cacheKey, () =>
            {
                var levelOneUsedCategories = _categoryService.GetUsedCategoriesByFilters(parentId,
                    searchTitle,
                    companyId,
                    representationId,
                    cityId,
                    categoryTitle,
                    applicationId,
                    loadLevelOneNodes,
                    additionalUserProp.CustomerClubId,
                    categoryGroupId);
                return _mapper.Map<List<CategorySimpleViewModel>>(levelOneUsedCategories);
            });


        }

        public Task<List<CategoryViewModel>> GetCategoryWithParentsAsync(int categoryId)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Category>.
                    GetCategoryWithParents,
                categoryId
            );
            cacheKey.CacheTime = (int)CacheTime.Default;
            return _staticCacheManager.GetAsync(cacheKey, () =>
            {
                var parents = _categoryService.GetCategoryWithParents(categoryId);
                return _mapper.Map<List<CategoryViewModel>>(parents);

                // return parents.ProjectTo<CategoryViewModel>(Mapper.ConfigurationProvider).ToList();
            });
        }


    }
}
