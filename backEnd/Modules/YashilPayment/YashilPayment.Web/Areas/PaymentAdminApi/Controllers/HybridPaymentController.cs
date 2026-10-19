using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class HybridPaymentController : BaseController<HybridPayment, int, HybridPaymentListViewModel, HybridPaymentEditModel, HybridPaymentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IHybridPaymentService _hybridPaymentService;
        private readonly IYashilLogger<HybridPaymentRequest> _logger;

        public HybridPaymentController(IHybridPaymentService hybridPaymentService,
        IMapper mapper,
        IYashilLogger<HybridPayment> logger,
        IPermissionService permissionService, IYashilLogger<HybridPaymentRequest> logger1, IPaymentGatewayService paymentGatewayService, IHttpContextAccessor httpContextAccessor) 
            : base(hybridPaymentService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _logger = logger1;
            _hybridPaymentService = hybridPaymentService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? hybridPaymentRequestId, int? hybridPaymentStatusType, int? paymentMethodType)
        {
            var hybridPayments = _hybridPaymentService.GetByCustomFilter(hybridPaymentRequestId, hybridPaymentStatusType, paymentMethodType, null);
            return await DataSourceLoader.LoadAsync(hybridPayments.ProjectTo<HybridPaymentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,
            int? hybridPaymentRequestId,
            int? hybridPaymentStatusType, 
            int? paymentMethodType)
        {
            var hybridPayments = _hybridPaymentService.GetByCustomFilter(hybridPaymentRequestId, hybridPaymentStatusType, paymentMethodType, null);
            return await DataSourceLoader.LoadAsync(hybridPayments.ProjectTo<HybridPaymentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


    }
}