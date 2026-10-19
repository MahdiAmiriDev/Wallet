using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
	public class PaymentGatewayPaymentMethodController : BaseController<PaymentGatewayPaymentMethod ,int,PaymentGatewayPaymentMethodListViewModel, PaymentGatewayPaymentMethodEditModel,PaymentGatewayPaymentMethodSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentGatewayPaymentMethodService _paymentGatewayPaymentMethodService;
		private readonly IYashilLogger<PaymentGatewayPaymentMethod> _logger;
        private readonly IPermissionService _permissionService;
        public PaymentGatewayPaymentMethodController(IPaymentGatewayPaymentMethodService paymentGatewayPaymentMethodService, 
        IMapper mapper, 
        IYashilLogger<PaymentGatewayPaymentMethod> logger,
        IPermissionService permissionService) : base(paymentGatewayPaymentMethodService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _paymentGatewayPaymentMethodService=paymentGatewayPaymentMethodService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? paymentGatewayId, int? paymentMethodType)
        {
            var paymentGatewayPaymentMethods = _paymentGatewayPaymentMethodService.GetByCustomFilter(paymentGatewayId,paymentMethodType);
            return await DataSourceLoader.LoadAsync(paymentGatewayPaymentMethods.ProjectTo<PaymentGatewayPaymentMethodListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? paymentGatewayId, int? paymentMethodType)
        {
            var paymentGatewayPaymentMethods = _paymentGatewayPaymentMethodService.GetByCustomFilter(paymentGatewayId,paymentMethodType);
            return await DataSourceLoader.LoadAsync(paymentGatewayPaymentMethods.ProjectTo<PaymentGatewayPaymentMethodSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      