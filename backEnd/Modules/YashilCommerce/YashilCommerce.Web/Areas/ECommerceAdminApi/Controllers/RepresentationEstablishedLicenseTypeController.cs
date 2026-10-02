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
	public class RepresentationEstablishedLicenseTypeController : BaseController<RepresentationEstablishedLicenseType ,int,RepresentationEstablishedLicenseTypeListViewModel, RepresentationEstablishedLicenseTypeEditModel,RepresentationEstablishedLicenseTypeSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepresentationEstablishedLicenseTypeService _representationEstablishedLicenseTypeService;
        private readonly IYashilLogger<RepresentationEstablishedLicenseType> _logger;  
        private readonly IPermissionService _permissionService;
        public RepresentationEstablishedLicenseTypeController(IRepresentationEstablishedLicenseTypeService representationEstablishedLicenseTypeService, IMapper mapper, IYashilLogger<RepresentationEstablishedLicenseType> logger,IPermissionService permissionService) 
            : base(representationEstablishedLicenseTypeService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _logger = logger; 
            _permissionService = permissionService;
            _representationEstablishedLicenseTypeService=representationEstablishedLicenseTypeService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? representationId, int? establishedLicenseType)
        {
            var representationEstablishedLicenseTypes = _representationEstablishedLicenseTypeService.GetByCustomFilter(representationId,establishedLicenseType);
            return await DataSourceLoader.LoadAsync(representationEstablishedLicenseTypes.ProjectTo<RepresentationEstablishedLicenseTypeListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? representationId, int? establishedLicenseType)
        {
            var representationEstablishedLicenseTypes = _representationEstablishedLicenseTypeService.GetByCustomFilter(representationId,establishedLicenseType);
            return await DataSourceLoader.LoadAsync(representationEstablishedLicenseTypes.ProjectTo<RepresentationEstablishedLicenseTypeSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      