using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilDms.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class UploadedFactorPaymentController : BaseController<UploadedFactorPayment, int,
        UploadedFactorPaymentListViewModel, UploadedFactorPaymentEditModel, UploadedFactorPaymentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUploadedFactorPaymentService _uploadedFactorPaymentService;
        private readonly IYashilLogger<UploadedFactorPayment> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IUserPrincipal _userPrincipal;

        public UploadedFactorPaymentController(IUploadedFactorPaymentService uploadedFactorPaymentService,
            IMapper mapper, IYashilLogger<UploadedFactorPayment> logger, IPermissionService permissionService,
            IAppDocumentService appDocumentService, IUserPrincipal userPrincipal) : base(uploadedFactorPaymentService,
            mapper, logger, permissionService)
        {
            _mapper = mapper;
            _uploadedFactorPaymentService = uploadedFactorPaymentService;
            _logger = logger;
            _permissionService = permissionService;
            _appDocumentService = appDocumentService;
            _userPrincipal = userPrincipal;
        }

        /// <summary>
        /// برای نمایش آیتمهای فایل آپود شده مورد استفاده قرار میگیرد
        /// در فرم بارگذاری فاکتور هنگامی که فایا بارگذاری شود این متد صدا زده میشوود و آیتم های فاکتور را نمایش میدهد
        /// توجه داشته باشید که آیتهای نمایش داده شده دارای فیلتری است که تنها آیتمهایی را نمایش میدهد که فقط کاربر لاگین شده انها را بارگذاری نموده ایت
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="appDocumentId"></param>
        /// <returns></returns>
        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? appDocumentId)
        {
            var uploadedFactorPayments = _uploadedFactorPaymentService.GetByCustomFilter(appDocumentId);
            return await DataSourceLoader.LoadAsync(
                uploadedFactorPayments.ProjectTo<UploadedFactorPaymentListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? appDocumentId)
        {
            var uploadedFactorPayments = _uploadedFactorPaymentService.GetByCustomFilter(appDocumentId);
            return await DataSourceLoader.LoadAsync(
                uploadedFactorPayments.ProjectTo<UploadedFactorPaymentSimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }
        /// <summary>
        /// بارگذاری پرداختی فاکتور
        /// </summary>
        /// <param name="factorPaymentFile"></param>
        /// <returns></returns>
        [HttpPost("uploadFactorPaymentFile")]
        [YashilAuthorization(ActionName = "PostEntity")]
        public async Task<ResultStatus<int>> UploadFactorPaymentFileAsync(IFormFile factorPaymentFile)
        {
            try
            {
                // ابتدا رکورد های مربوط به کاربر لاگین کرده را پاک میکند 
                _uploadedFactorPaymentService.DeleteByUserId(_userPrincipal.Id);
                var res = await _uploadedFactorPaymentService.BulkInsertFactorPaymentsDataAsync(factorPaymentFile);
                if (res)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = true,
                        Message = "کاربر گرامی فایل مورد نظر با موفقیت بارگذاری شد، توجه داشته باشید که پس از ثبت نهایی محتویات فایل بارگذاری میشوند"
                    };
                }

                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل"
                };
            }
            catch(Exception e)
            {
                _logger.InsertLog(LogLevel.Error,
                    new EventId((int)Events.InsertException, Events.InsertException.ToString()),
                    nameof(UploadedFactorPayment), "Uplaod ProductItem Bulk Insert Error", e);
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل"
                };
            }
        }
        /// <summary>
        /// بررسی اینکه پرداختی های فاکتور بارگذاری شده مجاز هستند یا خیر درواقع چک میکنیم که ستون های پرداخت فاکتور معتبر باشند
        /// یعنی مقادیر عددی عددی باشند و مثلا تاریخ مجاز باشد
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckIsValidUploadedFactorPaymentFile")]
        public async Task<ResultStatus<int>> CheckIsValidUploadedFactorPaymentFile()
        {
            var invalidUploadedProductItem = await _uploadedFactorPaymentService
                .ChecksInvalidUploadedFactorPaymentFilesForLoggedInUserAsync();
            if (invalidUploadedProductItem > 0)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی فایل بارگذاری شده دارای خطا میباشد لطفا پس از بررسی مشکلات مجددا اقدام به بارگذاری نمایید"
                };
            }
            return new ResultStatus<int>
            {
                IsSuccess = true,
            };
        }

        /// <summary>
        /// دریافت لیست فایل های بارگذاری شده
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetUploadedFactorPaymentFiles")]
        [YashilAuthorization(ActionName = "UploadedFactorPaymentFiles")]
        public async Task<LoadResult> GetUploadedFactorPaymentFiles(CustomDataSourceLoadOptions loadOptions)
        {
            var uploadedFactorPaymentFiles = _appDocumentService.GetObjectDocuments(
                Enum.GetName(AppEntityEnum.UploadedFactorPayment), (int)DocumentTypeEnum.UploadFactorPayments);

            return await DataSourceLoader.LoadAsync(
                uploadedFactorPaymentFiles.ProjectTo<UploadedFileListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// برای انتقال اطلاعات پرداخت فاکتور بارگذاری شده به جدول factorpayment بر اساس کاربر لاگین شده
        /// کاربری که لاگین کرده و فایل های پرداختی را بارگذاری کرده است
        /// پس از ولیدیشن و اینکه مشکلی نداشته باشد آنها را به جدول factorPayment منتقل میکند
        /// </summary>
        /// <returns></returns>
        [HttpGet("TransferDataFromUploadedFactorPaymentsToFactorPayment")]
        [YashilAuthorization(ActionName = "TransferDataFromUploadedFactorPaymentsToFactorPayment")]
        public async Task<ResultStatus<int>> TransferDataFromUploadedFactorPaymentsToFactorPayment()
        {
            try
            {
                await _uploadedFactorPaymentService.TransferDataFromUploadedFactorPaymentsToFactorPaymentForLoggedInUserAsync();
                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = " ثبت نهایی پرداختی های فاکتور با موفقیت انجام شد"
                };
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error,
                    new EventId((int)Events.InsertException, Events.InsertException.ToString()),
                    nameof(UploadedFactorPayment), "TransferDataFromUploadedFactorPaymentsToFactorPayment Error", e);
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در ثبت نهایی پرداختی های فاکتور"
                };
            }
            
        }
    }
}