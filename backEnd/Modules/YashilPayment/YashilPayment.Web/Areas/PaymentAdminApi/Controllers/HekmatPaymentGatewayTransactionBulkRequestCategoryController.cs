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

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers;
 
	public class HekmatPaymentGatewayTransactionBulkRequestCategoryController : BaseController<HekmatPaymentGatewayTransactionBulkRequestCategory 
        ,int,HekmatPaymentGatewayTransactionBulkRequestCategoryListViewModel,
        HekmatPaymentGatewayTransactionBulkRequestCategoryEditModel,HekmatPaymentGatewayTransactionBulkRequestCategorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IHekmatPaymentGatewayTransactionBulkRequestCategoryService _hekmatPaymentGatewayTransactionBulkRequestCategoryService;

        public HekmatPaymentGatewayTransactionBulkRequestCategoryController(IHekmatPaymentGatewayTransactionBulkRequestCategoryService hekmatPaymentGatewayTransactionBulkRequestCategoryService, 
        IMapper mapper, 
        IYashilLogger<HekmatPaymentGatewayTransactionBulkRequestCategory> logger,
        IPermissionService permissionService) : base(hekmatPaymentGatewayTransactionBulkRequestCategoryService, mapper,logger, permissionService)
        {
            _mapper = mapper;
            _hekmatPaymentGatewayTransactionBulkRequestCategoryService = hekmatPaymentGatewayTransactionBulkRequestCategoryService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? parentId, int? yashilHekmatTerminalId)
        {
            var hekmatPaymentGatewayTransactionBulkRequestCategorys = _hekmatPaymentGatewayTransactionBulkRequestCategoryService.GetByCustomFilter(parentId,yashilHekmatTerminalId);
            return await DataSourceLoader.LoadAsync(hekmatPaymentGatewayTransactionBulkRequestCategorys.ProjectTo<HekmatPaymentGatewayTransactionBulkRequestCategoryListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? parentId, int? yashilHekmatTerminalId)
        {
            var hekmatPaymentGatewayTransactionBulkRequestCategorys = _hekmatPaymentGatewayTransactionBulkRequestCategoryService.GetByCustomFilter(parentId,yashilHekmatTerminalId);
            return await DataSourceLoader.LoadAsync(hekmatPaymentGatewayTransactionBulkRequestCategorys.ProjectTo<HekmatPaymentGatewayTransactionBulkRequestCategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
      }
       