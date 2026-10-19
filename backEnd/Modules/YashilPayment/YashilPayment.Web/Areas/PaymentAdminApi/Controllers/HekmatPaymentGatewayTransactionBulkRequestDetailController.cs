using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;


namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers;

public class HekmatPaymentGatewayTransactionBulkRequestDetailController : BaseController<HekmatPaymentGatewayTransactionBulkRequestDetail 
    ,int,HekmatPaymentGatewayTransactionBulkRequestDetailListViewModel,
    HekmatPaymentGatewayTransactionBulkRequestDetailEditModel,
    HekmatPaymentGatewayTransactionBulkRequestDetailSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IHekmatPaymentGatewayTransactionBulkRequestDetailService _hekmatPaymentGatewayTransactionBulkRequestDetailService;
        private readonly IUserPrincipal _userPrincipal;

        public HekmatPaymentGatewayTransactionBulkRequestDetailController(IHekmatPaymentGatewayTransactionBulkRequestDetailService hekmatPaymentGatewayTransactionBulkRequestDetailService, 
        IMapper mapper, 
        IYashilLogger<HekmatPaymentGatewayTransactionBulkRequestDetail> logger,
        IPermissionService permissionService, IUserPrincipal userPrincipal) : base(hekmatPaymentGatewayTransactionBulkRequestDetailService, mapper,logger, permissionService)
        {
            _mapper = mapper;
            _userPrincipal = userPrincipal;
            _hekmatPaymentGatewayTransactionBulkRequestDetailService = hekmatPaymentGatewayTransactionBulkRequestDetailService;
        }

        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? appDocumentId, int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestId)
        {
            var hekmatPaymentGatewayTransactionBulkRequestDetails = _hekmatPaymentGatewayTransactionBulkRequestDetailService.GetByCustomFilter(appDocumentId,parentId,hekmatPaymentGatewayTransactionBulkRequestId);
            return await DataSourceLoader.LoadAsync(hekmatPaymentGatewayTransactionBulkRequestDetails.ProjectTo<HekmatPaymentGatewayTransactionBulkRequestDetailListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? appDocumentId, int? parentId, int? hekmatPaymentGatewayTransactionBulkRequestId)
        {
            var hekmatPaymentGatewayTransactionBulkRequestDetails = _hekmatPaymentGatewayTransactionBulkRequestDetailService.GetByCustomFilter(appDocumentId,parentId,hekmatPaymentGatewayTransactionBulkRequestId);
            return await DataSourceLoader.LoadAsync(hekmatPaymentGatewayTransactionBulkRequestDetails.ProjectTo<HekmatPaymentGatewayTransactionBulkRequestDetailSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// دریافت لیست تراکنش های Bulk
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="hekmatPaymentGatewayTransactionBulkRequestId">شناسه درخواست تراکنش انبوه</param>
        /// <param name="showInvalidItem"></param>
        /// <returns></returns>
        [HttpGet("GetHekmatBulkRequest")]
        [YashilAuthorization("GetByCustomFilterForList")]
        public async Task<LoadResult> GetHekmatBulkRequestAsync(
            CustomDataSourceLoadOptions loadOptions,
            int hekmatPaymentGatewayTransactionBulkRequestId,
            bool showInvalidItem)
        {

            var hekmatPaymentTransactionBulkRequestDetails =
                _hekmatPaymentGatewayTransactionBulkRequestDetailService.
                    GetInvalidHekmatBulkRequest(_userPrincipal.Id, hekmatPaymentGatewayTransactionBulkRequestId, showInvalidItem);
            return await DataSourceLoader.LoadAsync(
                hekmatPaymentTransactionBulkRequestDetails.ProjectTo<HekmatPaymentGatewayTransactionBulkRequestDetailListSimpleViewModel>
                    (_mapper.ConfigurationProvider), loadOptions);
        }
}
      