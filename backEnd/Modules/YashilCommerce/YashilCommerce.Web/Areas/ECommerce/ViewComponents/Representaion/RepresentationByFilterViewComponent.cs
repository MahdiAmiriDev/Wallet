using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Representaion
{
    public class RepresentationByFilterViewComponent : ViewComponent
    {

        private readonly IRepresentationService _representationService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly IStaticCacheManager _staticCacheManager;

        public RepresentationByFilterViewComponent(ApplicationConfiguration applicationConfiguration, IMapper mapper,
            IRepresentationService representationService, IStaticCacheManager staticCacheManager)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _representationService = representationService;
            _staticCacheManager = staticCacheManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(ProductFilter productFilter)
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<Representation>.GetAllByCompanyAndCityAndCategory,
                productFilter?.Company, productFilter?.CityId, productFilter?.CategoryId,productFilter?.CategoryGroupId);
            cacheKey.CacheTime = (int)CacheTime.Default;
            var representations = await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                productFilter ??= new ProductFilter();
                IQueryable<Representation> applicationRepresentations = await _representationService
                    .GetCompanyRepresentationByFilterAsync(
                        _applicationConfiguration.ApplicationId,
                        productFilter?.Company,
                        productFilter?.CityId,
                        productFilter?.CategoryId,
                        productFilter?.CategoryGroupId);
                return await applicationRepresentations.ProjectTo<RepresentationSimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            });
            return View("_RepresentationByFilter", representations);
        }
    }
}
