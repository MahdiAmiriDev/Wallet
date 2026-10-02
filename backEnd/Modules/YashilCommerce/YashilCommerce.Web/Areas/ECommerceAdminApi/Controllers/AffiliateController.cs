	 
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using  YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class AffiliateController : BaseController<Affiliate ,int,AffiliateListViewModel, AffiliateEditModel,AffiliateSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IAffiliateService _affiliateService;
		private readonly IYashilLogger<Affiliate> _logger;
        private readonly IPermissionService _permissionService;
        public AffiliateController(IAffiliateService affiliateService, IMapper mapper, IYashilLogger<Affiliate> logger, IPermissionService permissionService) : base(affiliateService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _affiliateService=affiliateService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? addressId)
        {
            var affiliates = _affiliateService.GetByCustomFilter(addressId);
            return await DataSourceLoader.LoadAsync(affiliates.ProjectTo<AffiliateListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? addressId)
        {
            var affiliates = _affiliateService.GetByCustomFilter(addressId);
            return await DataSourceLoader.LoadAsync(affiliates.ProjectTo<AffiliateSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      