using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Classes;
using System;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Enums;
using YashilDms.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class RefundController : BaseController<Refund, int, RefundListViewModel, RefundEditModel, RefundSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRefundService _refundService;
        private readonly IYashilLogger<Refund> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IHekmatService _hekmatService;
        private readonly IAppDocumentService _appDocumentService;
        public RefundController(IRefundService refundService,
        IMapper mapper,
        IYashilLogger<Refund> logger,
        IPermissionService permissionService, IHekmatService hekmatService, IAppDocumentService appDocumentService) : base(refundService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _refundService = refundService;
            _logger = logger;
            _permissionService = permissionService;
            _hekmatService = hekmatService;
            _appDocumentService = appDocumentService;
        }

        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? orderId)
        {
            var refunds = _refundService.GetByCustomFilter(orderId);
            return await DataSourceLoader.LoadAsync(refunds.ProjectTo<RefundListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? orderId)
        {
            var refunds = _refundService.GetByCustomFilter(orderId);
            return await DataSourceLoader.LoadAsync(refunds.ProjectTo<RefundSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// بارگذاری فایل در فولدر ست شده در متغیر  
        /// fileSaveFolderName
        /// در مسیر
        /// wwwroot
        /// </summary>
        /// <param name="hekmatTransactionFile"></param>
        /// <returns></returns>
        [HttpPost("uploadhekmatTransactionFile")]
        [YashilAuthorization(ActionName = "UploadHekmatApprovedTransactionFile")]
        public async Task<ResultStatus<int>> UploadHekmatTransactionFile(IFormFile hekmatTransactionFile)
        {
            var res = await _hekmatService.BulkCopyHekmatApprovedDataAsync(hekmatTransactionFile, "");
            if (res)
            {

                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "با موفقیت ذخیره شد"
                };
            }
            else
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل"
                };
            }
        }

        [HttpGet("GetHekmatUploadedTransactionFiles")]
        [YashilAuthorization(ActionName = "HekmatUploadedTransactionFiles")]
        public async Task<LoadResult> GetHekmatUploadedTransactionFiles(CustomDataSourceLoadOptions loadOptions)
        {
            var hekmatTransactionFiles = _appDocumentService.GetObjectDocuments(Enum.GetName(AppEntityEnum.HekmatApprovedTransaction), (int)DocumentTypeEnum.HekmatApprovedTransaction);

            return await DataSourceLoader.LoadAsync(hekmatTransactionFiles.ProjectTo<UploadedFileListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetHekmatUploadedRefundFiles")]
        [YashilAuthorization(ActionName = "HekmatUploadedRefundFiles")]
        public async Task<LoadResult> GetHekmatUploadedRefundFiles(CustomDataSourceLoadOptions loadOptions)
        {
            var hekmatRefundFiles = _appDocumentService.GetObjectDocuments(Enum.GetName(AppEntityEnum.HekmatApprovedTransaction), (int)DocumentTypeEnum.HekmatRefundedTransactions);

            return await DataSourceLoader.LoadAsync(hekmatRefundFiles.ProjectTo<UploadedFileListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        [HttpPost("uploadHekmatRefundFile")]
        [YashilAuthorization(ActionName = "UploadHekmatRefundFile")]
        public async Task<ResultStatus<int>> UploadHekmatRefundFile(IFormFile hekmatRefundFile)
        {
            var res = await _hekmatService.BulkCopyHekmatRefundedTransactionsDataAsync(hekmatRefundFile);
            if (res)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "با موفقیت ذخیره شد"
                };
            }
            else
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل"
                };
            }
        }

    }
}