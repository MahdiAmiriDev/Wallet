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
	public class CustomerClubPaymentMethodTypeController : BaseController<CustomerClubPaymentMethodType ,int,CustomerClubPaymentMethodTypeListViewModel, CustomerClubPaymentMethodTypeEditModel,CustomerClubPaymentMethodTypeSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerClubPaymentMethodTypeService _customerClubPaymentMethodTypeService;
		private readonly IYashilLogger<CustomerClubPaymentMethodType> _logger;
        private readonly IPermissionService _permissionService;
        public CustomerClubPaymentMethodTypeController(ICustomerClubPaymentMethodTypeService customerClubPaymentMethodTypeService, IMapper mapper, IYashilLogger<CustomerClubPaymentMethodType> logger, IPermissionService permissionService) : base(customerClubPaymentMethodTypeService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _customerClubPaymentMethodTypeService=customerClubPaymentMethodTypeService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? paymentMethodType, int? customerClubId)
        {
            var customerClubPaymentMethodTypes = _customerClubPaymentMethodTypeService.GetByCustomFilter(paymentMethodType,customerClubId);
            return await DataSourceLoader.LoadAsync(customerClubPaymentMethodTypes.ProjectTo<CustomerClubPaymentMethodTypeListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? paymentMethodType, int? customerClubId)
        {
            var customerClubPaymentMethodTypes = _customerClubPaymentMethodTypeService.GetByCustomFilter(paymentMethodType,customerClubId);
            return await DataSourceLoader.LoadAsync(customerClubPaymentMethodTypes.ProjectTo<CustomerClubPaymentMethodTypeSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      