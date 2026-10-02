using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class SiteVisitController : BaseController<SiteVisit ,int,SiteVisitListViewModel, SiteVisitEditModel,SiteVisitSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISiteVisitService _siteVisitService;
		private readonly IYashilLogger<SiteVisit> _logger;
        private readonly IPermissionService _permissionService;
        public SiteVisitController(ISiteVisitService siteVisitService, 
        IMapper mapper, 
        IYashilLogger<SiteVisit> logger,
        IPermissionService permissionService) : base(siteVisitService, mapper,logger, permissionService)
        {
            _mapper = mapper;
            _siteVisitService = siteVisitService;
			_logger = logger;
			_permissionService = permissionService;
        }

        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? parentId)
        {
            var siteVisits = _siteVisitService.GetByCustomFilter(parentId);
            return await DataSourceLoader.LoadAsync(siteVisits.ProjectTo<SiteVisitListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? parentId)
        {
            var siteVisits = _siteVisitService.GetByCustomFilter(parentId);
            return await DataSourceLoader.LoadAsync(siteVisits.ProjectTo<SiteVisitSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        /// <summary>
        /// این متد جهت دریافت تعداد بازدید های سایت کاربرد دارد
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSiteVisitInfo")]
        [AllowAnonymous]
        public async Task<SiteVisitViewModel> GetSiteVisitInfo()
        {
            SiteVisitViewModel siteVisit = new()
            {
                PerviousDayTotalSiteVisitCount = await _siteVisitService.GetPreviousDayTotalSiteVisitCountAsync(),
                CurrentDaySiteVisitCount = await _siteVisitService.GetCurrentDaySiteVisitCountAsync(),
                TotalSiteVisitCount = await _siteVisitService.GetTotalSiteVisitCountAsync()
            };

            return siteVisit;
        }
      }
}      