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
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
	public class MerchantTerminalInfoController : BaseController<MerchantTerminalInfo ,int,MerchantTerminalInfoListViewModel, MerchantTerminalInfoEditModel,MerchantTerminalInfoSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IMerchantTerminalInfoService _merchantTerminalInfoService;
		private readonly IYashilLogger<MerchantTerminalInfo> _logger;
        private readonly IPermissionService _permissionService;
        public MerchantTerminalInfoController(IMerchantTerminalInfoService merchantTerminalInfoService, IMapper mapper, IYashilLogger<MerchantTerminalInfo> logger, IPermissionService permissionService) : base(merchantTerminalInfoService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _merchantTerminalInfoService=merchantTerminalInfoService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? merchantId)
        {
            var merchantTerminalInfos = _merchantTerminalInfoService.GetByCustomFilter(merchantId);
            return await DataSourceLoader.LoadAsync(merchantTerminalInfos.ProjectTo<MerchantTerminalInfoListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? merchantId)
        {
            var merchantTerminalInfos = _merchantTerminalInfoService.GetByCustomFilter(merchantId);
            return await DataSourceLoader.LoadAsync(merchantTerminalInfos.ProjectTo<MerchantTerminalInfoSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      