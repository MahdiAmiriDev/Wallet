using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Classes;
using YashilNews.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.NewsAndAnnouncements
{
    /// <summary>
    /// کامپوننت نمایش اخبار و اطلاعیه ها
    /// </summary>
    public class NewsAndAnnouncementsViewComponent : ViewComponent
    {
        private readonly INewsStoreService _newsStoreService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly IAppConfigService _appConfigService;
        private readonly IStaticCacheManager _staticCacheManager;

        public NewsAndAnnouncementsViewComponent(INewsStoreService newsStoreService, ApplicationConfiguration applicationConfiguration, IMapper mapper, IAppConfigService appConfigService, IStaticCacheManager staticCacheManager)
        {
            _newsStoreService = newsStoreService;
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _appConfigService = appConfigService;
            _staticCacheManager = staticCacheManager;
        }
        /// <summary>
        /// دریافت لیست اخبار و اطلاعیه ها
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<NewsStore>.NewsAndAnnouncementsKey);

            cacheKey.CacheTime = (int)CacheTime.Default;

            return await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var sliderCount = await _appConfigService.GetValueByApplicationIdAsync(
                    ApplicationConfigKey.YashilCommerceNewsSilderCount,
                    _applicationConfiguration.ApplicationId);

                var newsList = _newsStoreService
                    .GetByCustomFilter(null, null, null, true, _applicationConfiguration.ApplicationId, null,null)
                    .OrderByDescending(x => x.Id).Take(sliderCount);

                return View(await newsList.ProjectTo<NewsStorePublicPageViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync());
            });
        }
    }
}
