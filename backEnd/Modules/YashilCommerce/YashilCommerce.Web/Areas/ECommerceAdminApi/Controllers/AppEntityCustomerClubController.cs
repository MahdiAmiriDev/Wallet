using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class AppEntityCustomerClubController : BaseController<AppEntityCustomerClub, int,
        AppEntityCustomerClubListViewModel, AppEntityCustomerClubEditModel, AppEntityCustomerClubSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IAppEntityCustomerClubService _appEntityCustomerClubService;
        private readonly IYashilLogger<AppEntityCustomerClub> _logger;
        private readonly IPermissionService _permissionService;

        public AppEntityCustomerClubController(IAppEntityCustomerClubService appEntityCustomerClubService,
            IMapper mapper,
            IYashilLogger<AppEntityCustomerClub> logger,
            IPermissionService permissionService) : base(appEntityCustomerClubService, mapper, logger,
            permissionService)
        {
            _mapper = mapper;
            _appEntityCustomerClubService = appEntityCustomerClubService;
            _logger = logger;
            _permissionService = permissionService;
        }

        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? appEntityId, int? customerclubId)
        {
            var appEntityCustomerClubs = _appEntityCustomerClubService.GetByCustomFilter(appEntityId, customerclubId);
            return await DataSourceLoader.LoadAsync(
                appEntityCustomerClubs.ProjectTo<AppEntityCustomerClubListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? appEntityId,
            int? customerclubId)
        {
            var appEntityCustomerClubs = _appEntityCustomerClubService.GetByCustomFilter(appEntityId, customerclubId);
            return await DataSourceLoader.LoadAsync(
                appEntityCustomerClubs.ProjectTo<AppEntityCustomerClubSimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }
    }
}