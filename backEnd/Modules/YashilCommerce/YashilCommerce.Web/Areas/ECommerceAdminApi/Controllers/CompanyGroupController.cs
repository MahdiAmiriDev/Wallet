using System.Collections.Generic;
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
using YashilCommerce.Infrastructure.ServiceImpl;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class CompanyGroupController : BaseController<CompanyGroup ,int,CompanyGroupListViewModel, CompanyGroupEditModel,CompanyGroupSimpleViewModel>
    {
        private readonly ICompanyGroupService _companyGroupService;
        private readonly IMapper _mapper;

        public CompanyGroupController(ICompanyGroupService companyGroupService, 
        IMapper mapper, 
        IYashilLogger<CompanyGroup> logger,
        IPermissionService permissionService) : base(companyGroupService, mapper,logger, permissionService)
        {
            _companyGroupService = companyGroupService;
            _mapper = mapper;
        }
        [HttpGet("GetByCustomFilterForList")]

        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? companyGroupId)
        {
            var companyGroups = _companyGroupService.GetByCustomFilter(companyGroupId);
            return await DataSourceLoader.LoadAsync(
                companyGroups.ProjectTo<CompanyGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }
}      