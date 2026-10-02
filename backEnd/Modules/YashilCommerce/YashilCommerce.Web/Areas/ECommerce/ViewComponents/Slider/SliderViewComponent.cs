using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Interfaces;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.DataService;
using YashilCommerce.Web.Areas.ECommerce.ViewModels.Slider;
using YashilNews.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Slider
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly IYashilCmsService _cmsService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ICategoryCustomerClubService _categoryCustomerClubService;
        private readonly PublicDataService _publicDataService;


        public SliderViewComponent(ApplicationConfiguration applicationConfiguration, IMapper mapper,
            IYashilCmsService cmsService, IStaticCacheManager staticCacheManager, IUserPrincipal userPrincipal,
            IAdditionalUserPropService additionalUserPropService,
            ICategoryCustomerClubService categoryCustomerClubService, PublicDataService publicDataService)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _cmsService = cmsService;
            _staticCacheManager = staticCacheManager;
            _userPrincipal = userPrincipal;
            _additionalUserPropService = additionalUserPropService;
            _categoryCustomerClubService = categoryCustomerClubService;
            _publicDataService = publicDataService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? categoryId , int? categoryGroupId)
        {
            if (categoryId.HasValue)
            {
                var userAdditionalUserProp =
                    await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

                CacheKey cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                    YashilModelCacheDefaults<YashilCms>.MainPageSliderLevelTwoByCategoryAndCustomerClub, categoryId,
                    userAdditionalUserProp.CustomerClubId);
                cacheKey.CacheTime = (int)CacheTime.OneHour;

                var sliderList = await _staticCacheManager.GetAsync(cacheKey, async () =>
                {
                    var categoryCustomerClubs =
                        _categoryCustomerClubService.GetByCustomFilter(userAdditionalUserProp.CustomerClubId,
                            categoryId, _applicationConfiguration.ApplicationId);
                    return await categoryCustomerClubs.ProjectTo<SliderSimpleViewModel>(_mapper.ConfigurationProvider)
                        .ToListAsync();
                });
                if (sliderList.Any())
                {
                    return View(sliderList);
                }
            }else if (categoryGroupId.HasValue)
            {
                List<SliderSimpleViewModel> sliderSimpleViewModels = new();

                var categoryGroupSliderImage = await _publicDataService.GetCategoryGroupImageAppDocument(categoryGroupId.Value)
                    .Select(appDocument => new SliderSimpleViewModel
                    {
                        CategoryGroupImageObjectId = appDocument.ObjectId,
                        Title = string.Empty,
                        Id = default(int),
                        Url = string.Empty,
                        ModificationDate = string.Empty,
                        SubTitle = string.Empty
                    }).FirstOrDefaultAsync();

                if (categoryGroupSliderImage == null)
                    return View(sliderSimpleViewModels);

                sliderSimpleViewModels.Add(categoryGroupSliderImage);

                return View(sliderSimpleViewModels);
            }

            var defaultSliderList = await GetDefaultSliderLevelTwoAsync();
            return View(defaultSliderList);
        }

        private Task<List<SliderSimpleViewModel>> GetDefaultSliderLevelTwoAsync()
        {
            CacheKey cacheKey =
                _staticCacheManager.PrepareKeyForDefaultCache(
                    YashilModelCacheDefaults<YashilCms>.MainPageSliderLevelTwo);
            cacheKey.CacheTime = (int)CacheTime.OneHour;
            return _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var sliders = _cmsService.GetByPagePositionId(_applicationConfiguration.ApplicationId,
                    (int)PagePositionTypeEnum.MainPageSliderLevelTwo);
                return await sliders.ProjectTo<SliderSimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            });
        }
    }
}