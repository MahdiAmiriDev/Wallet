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
	public class CreditOrCashPaymentController : BaseController<CreditOrCashPayment ,int,CreditOrCashPaymentListViewModel, CreditOrCashPaymentEditModel,CreditOrCashPaymentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICreditOrCashPaymentService _creditOrCashPaymentService;
		private readonly IYashilLogger<CreditOrCashPayment> _logger;
        private readonly IPermissionService _permissionService;
        public CreditOrCashPaymentController(ICreditOrCashPaymentService creditOrCashPaymentService, IMapper mapper, IYashilLogger<CreditOrCashPayment> logger, IPermissionService permissionService) : base(creditOrCashPaymentService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _creditOrCashPaymentService=creditOrCashPaymentService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? cardId, int? merchantId, int? status)
        {
            var creditOrCashPayments = _creditOrCashPaymentService.GetByCustomFilter(cardId,merchantId,status);
            return await DataSourceLoader.LoadAsync(creditOrCashPayments.ProjectTo<CreditOrCashPaymentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? cardId, int? merchantId, int? status)
        {
            var creditOrCashPayments = _creditOrCashPaymentService.GetByCustomFilter(cardId,merchantId,status);
            return await DataSourceLoader.LoadAsync(creditOrCashPayments.ProjectTo<CreditOrCashPaymentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      