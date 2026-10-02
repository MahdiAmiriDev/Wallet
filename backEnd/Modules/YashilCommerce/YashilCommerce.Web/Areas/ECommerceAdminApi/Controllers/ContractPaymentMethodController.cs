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
	public class ContractPaymentMethodController : BaseController<ContractPaymentMethod ,int,ContractPaymentMethodListViewModel, ContractPaymentMethodEditModel,ContractPaymentMethodSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IContractPaymentMethodService _contractPaymentMethodService;
        private readonly IYashilLogger<ContractPaymentMethod> _logger;  private readonly IPermissionService _permissionService;
        public ContractPaymentMethodController(IContractPaymentMethodService contractPaymentMethodService, IMapper mapper, IYashilLogger<ContractPaymentMethod> logger,IPermissionService permissionService): base(contractPaymentMethodService, mapper,logger,permissionService)
        {
            _mapper=mapper;
            _logger = logger; _permissionService = permissionService;
            _contractPaymentMethodService=contractPaymentMethodService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? contractId, int? paymentMethodType)
        {
            var contractPaymentMethods = _contractPaymentMethodService.GetByCustomFilter(contractId,paymentMethodType);
            return await DataSourceLoader.LoadAsync(contractPaymentMethods.ProjectTo<ContractPaymentMethodListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? contractId, int? paymentMethodType)
        {
            var contractPaymentMethods = _contractPaymentMethodService.GetByCustomFilter(contractId,paymentMethodType);
            return await DataSourceLoader.LoadAsync(contractPaymentMethods.ProjectTo<ContractPaymentMethodSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      