using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class ApplicationCustomerClubController : BaseController<ApplicationCustomerClub ,int,ApplicationCustomerClubListViewModel, ApplicationCustomerClubEditModel,ApplicationCustomerClubSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationCustomerClubService _applicationCustomerClubService;
		private readonly IYashilLogger<ApplicationCustomerClub> _logger;
        private readonly IPermissionService _permissionService;
        public ApplicationCustomerClubController(IApplicationCustomerClubService applicationCustomerClubService, IMapper mapper, IYashilLogger<ApplicationCustomerClub> logger, IPermissionService permissionService) : base(applicationCustomerClubService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _applicationCustomerClubService=applicationCustomerClubService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? customerClubId)
        {
            var applicationCustomerClubs = _applicationCustomerClubService.GetByCustomFilter(customerClubId);
            return await DataSourceLoader.LoadAsync(applicationCustomerClubs.ProjectTo<ApplicationCustomerClubListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? customerClubId)
        {
            var applicationCustomerClubs = _applicationCustomerClubService.GetByCustomFilter(customerClubId);
            return await DataSourceLoader.LoadAsync(applicationCustomerClubs.ProjectTo<ApplicationCustomerClubSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      