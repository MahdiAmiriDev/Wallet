using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Logo
{
    public class LogoViewComponent : ViewComponent
    {
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAppConfigService _appConfigService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IWebHelper _webHelper;

        public LogoViewComponent(
            IStaticCacheManager staticCacheManager,
            IAppConfigService appConfigService,
            ApplicationConfiguration applicationConfiguration,
            IWebHelper webHelper)
        {
            _staticCacheManager = staticCacheManager;
            _appConfigService = appConfigService;
            _applicationConfiguration = applicationConfiguration;
            _webHelper = webHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _appConfigService.GetSiteLogoAsync();
            var model =  new LogoSimpleViewModel()
            {
                Path = result.logoPath,
                    Name = result.name
            };
            return View(model);
        }

    }
}
