using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Core.CustomViewModels;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.CitySelector
{
    public class CitySelectorViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICompanyService _companyService;


        public CitySelectorViewComponent(
            IMapper mapper,  IStaticCacheManager staticCacheManager,
            ICompanyService companyService)
        {
            _mapper = mapper;
            _staticCacheManager = staticCacheManager;
            _companyService = companyService;

        }

        public async Task<IViewComponentResult> InvokeAsync(ProductFilter productFilter)
        {
            int? companyId = null;
            if (productFilter?.Company != null && productFilter.Company.Any())
            {
                companyId = productFilter.Company.FirstOrDefault();
            }
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<City>.GetAllByCompanyAndCategory,
                companyId,
                productFilter?.CategoryId,
                productFilter?.CategoryGroupId
                );
            cacheKey.CacheTime = (int)CacheTime.TwentyFourHour;
            var cities = await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var cityByCompany = await _companyService.GetCityByFilterAsync(
                    companyId,
                    productFilter?.CategoryId,
                    productFilter?.CategoryGroupId);
                return await cityByCompany.ProjectTo<CityTitleOnlyViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            });
            return View("_CitySelector", cities);
        }
    }
}
