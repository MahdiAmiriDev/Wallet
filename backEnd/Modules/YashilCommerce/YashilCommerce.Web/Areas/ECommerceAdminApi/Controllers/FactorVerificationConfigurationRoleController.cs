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
    public class FactorVerificationConfigurationRoleController : BaseController<FactorVerificationConfigurationRole, int, FactorVerificationConfigurationRoleListViewModel, FactorVerificationConfigurationRoleEditModel, FactorVerificationConfigurationRoleSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFactorVerificationConfigurationRoleService _factorVerificationConfigurationRoleService;
        private readonly IYashilLogger<FactorVerificationConfigurationRole> _logger;
        private readonly IPermissionService _permissionService;
        public FactorVerificationConfigurationRoleController(IFactorVerificationConfigurationRoleService factorVerificationConfigurationRoleService, IMapper mapper, IYashilLogger<FactorVerificationConfigurationRole> logger, IPermissionService permissionService) : base(factorVerificationConfigurationRoleService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorVerificationConfigurationRoleService = factorVerificationConfigurationRoleService;
            _logger = logger;
            _permissionService = permissionService;
        }
        
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? factorVerificationConfigurationId, int? roleId)
        {
            var factorVerificationConfigurationRoles = _factorVerificationConfigurationRoleService.GetByCustomFilter(factorVerificationConfigurationId, roleId);
            return await DataSourceLoader.LoadAsync(factorVerificationConfigurationRoles.ProjectTo<FactorVerificationConfigurationRoleListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? factorVerificationConfigurationId, int? roleId)
        {
            var factorVerificationConfigurationRoles = _factorVerificationConfigurationRoleService.GetByCustomFilter(factorVerificationConfigurationId, roleId);
            return await DataSourceLoader.LoadAsync(factorVerificationConfigurationRoles.ProjectTo<FactorVerificationConfigurationRoleSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}