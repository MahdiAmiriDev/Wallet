using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.Core.Classes;
using System.Collections.Generic;
using Yashil.Common.Core.Caching;
using YashilBaseInfo.Core.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YashilCommerce.Core.Classes;
using YashilUserManagement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Yashil.Core.CustomDtos.Representation;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class RepresentationController : YashilMvcBaseController<object>
    {
        private readonly IMapper _mapper;
        private readonly IRepresentationService _representationService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ICompanyService _companyService;
        private readonly ICityService _cityService;
        private readonly IAppConfigService _appConfigService;

        public RepresentationController(ApplicationConfiguration applicationConfiguration,
            IMapper mapper, IYashilLogger<object> logger,
            IPermissionService permissionService,
            IRepresentationService representationService,
            IStaticCacheManager staticCacheManager,
            ICompanyService companyService,
            ICityService cityService,
            IAppConfigService appConfigService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _representationService = representationService;
            _staticCacheManager = staticCacheManager;
            _companyService = companyService;
            _cityService = cityService;
            _appConfigService = appConfigService;
        }
        /// <summary>
        /// نمایندگی های مربوط به مرکز در هر شهر
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<ActionResult> CityRepresentations(int cityId, int? companyId)
        {
            var representationList = _representationService.GetByCityIdAndCompanyId(cityId, companyId);
            var representationViewModel = await representationList.ProjectTo<RepresentationSimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return View("_Representation", representationViewModel);
        }
        /// <summary>
        /// لیست نمایندگی های مربوط به مرکز در هر شهر
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<List<RepresentationSearchViewModel>> GetRepresentationsByCityAndCompanyId(int? cityId, int? companyId)
        {
            if (cityId == 0)
            {
                cityId = null;
            }

            var representationList = _representationService.GetByCityIdAndCompanyId(cityId, companyId);
            var representationViewModel = await representationList.ProjectTo<RepresentationSearchViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return representationViewModel;
        }
        /// <summary>
        /// فرم مربوط به جستجوی نمایندگی ها
        /// </summary>
        /// <returns></returns>
        [Route("RepresentationsSearch")]
        [HttpGet]
        public IActionResult RepresentationsSearch()
        {
            return View();
        }

        /// <summary>
        /// دریافت جزیییات مربوط به یک نمایندگی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Representation/GetDetailOfLocalCompanies")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailOfLocalCompaniesAsync(int id)
        {
            var result = await _mapper.ProjectTo<RepresentationDetailDto>(_representationService.GetCompanyDetails(id), _mapper.ConfigurationProvider).FirstAsync();
            return View("DetailsInstituteRepresentatives", result);
        }

    }
}
