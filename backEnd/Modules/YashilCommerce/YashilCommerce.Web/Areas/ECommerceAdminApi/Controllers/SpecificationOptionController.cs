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
    public class SpecificationOptionController : BaseController<SpecificationOption, int,
        SpecificationOptionListViewModel, SpecificationOptionEditModel, SpecificationOptionSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISpecificationOptionService _specificationOptionService;
        private readonly IYashilLogger<SpecificationOption> _logger;  private readonly IPermissionService _permissionService;

        public SpecificationOptionController(ISpecificationOptionService specificationOptionService, IMapper mapper,
            IYashilLogger<SpecificationOption> logger,IPermissionService permissionService): base(specificationOptionService, mapper, logger,permissionService)
        {
            _mapper = mapper;
            _logger = logger; _permissionService = permissionService;
            _specificationOptionService = specificationOptionService;
        }

        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? specificationId)
        {
            var specificationOptions = _specificationOptionService.GetByCustomFilter(specificationId);
            return await DataSourceLoader.LoadAsync(
                specificationOptions.ProjectTo<SpecificationOptionListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,
            int? specificationId)
        {
            var specificationOptions = _specificationOptionService.GetByCustomFilter(specificationId);
            return await DataSourceLoader.LoadAsync(
                specificationOptions.ProjectTo<SpecificationOptionSimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }
    }
}