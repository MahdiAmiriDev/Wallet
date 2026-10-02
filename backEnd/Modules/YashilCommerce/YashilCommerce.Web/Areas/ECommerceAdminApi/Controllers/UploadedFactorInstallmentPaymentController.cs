using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using System;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class UploadedFactorInstallmentPaymentController : BaseController<UploadedFactorInstallmentPayment, int, UploadedFactorInstallmentPaymentListViewModel, UploadedFactorInstallmentPaymentEditModel, UploadedFactorInstallmentPaymentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUploadedFactorInstallmentPaymentService _uploadedFactorInstallmentPaymentService;
        private readonly IYashilLogger<UploadedFactorInstallmentPayment> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        public UploadedFactorInstallmentPaymentController(IUploadedFactorInstallmentPaymentService uploadedFactorInstallmentPaymentService,
        IMapper mapper,
        IYashilLogger<UploadedFactorInstallmentPayment> logger,
        IPermissionService permissionService,
        IUserPrincipal userPrincipal,
        ApplicationConfiguration applicationConfiguration) : base(uploadedFactorInstallmentPaymentService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _uploadedFactorInstallmentPaymentService = uploadedFactorInstallmentPaymentService;
            _logger = logger;
            _permissionService = permissionService;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }
        /// <summary>
        /// دریافت لیست آپلود شده در جدول
        /// UploadedFactorInstallmentPayments
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="YearMonth"></param>
        /// <returns></returns>
        [HttpGet("GetUploadedFactorInstallmentPayment")]
        public async Task<LoadResult> GetUploadedFactorInstallmentPaymentAsync(CustomDataSourceLoadOptions loadOptions, int YearMonth)
        {
            var uploadedFactorInstallmentPayment = _uploadedFactorInstallmentPaymentService
                        .GetByCustomFilter(YearMonth, _userPrincipal.Id);
            return await DataSourceLoader.LoadAsync(
                        uploadedFactorInstallmentPayment
                        .ProjectTo<UploadedFactorInstallmentPaymentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// ابتدا رکوردهای قبلی مربوط به کاربر لاگین کرده که ایراد دارند را از جدول
        /// UploadedFactorInstallmentPayment
        /// پاک می کند
        /// سپس فایل ارسالی از سمت کاربر را در جدول
        /// UploadedFactorInstallmentPayment
        /// درج می کند
        /// </summary>
        /// <param name="tempFileUploader"></param>
        /// <returns></returns>
        [HttpPost("UploadFactorInstallmentPaymentFile")]
        public async Task<ResultStatus<int>> UploadFactorInstallmentPaymentFileAsync([FromForm] IFormFile tempFileUploader, [FromForm] int yearMonth)
        {
            try
            {
                var result = await _uploadedFactorInstallmentPaymentService.BulkInsertFromExcelAsync(tempFileUploader, yearMonth);

                return new ResultStatus<int>
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message
                };
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error,
                   new EventId((int)Events.InsertException, Events.InsertException.ToString()),
                   nameof(UploadedFactorInstallmentPayment), "Upload Factor Installment Payment Error", e);

                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل"
                };
            }
        }
        /// <summary>
        /// بروزرسانی جدول 
        /// FactorInstallmentPayment
        /// به کمک فایل آپلود شده در جدول
        /// UploadedFactorInstallmentPayment
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateFactorInstallmentPaymentFromUploadedFile")]
        public async Task<ResultStatus<int>> UpdateFactorInstallmentPaymentFromUploadedFileAsync()
        {
            try
            {
                int updatedCount = await _uploadedFactorInstallmentPaymentService
                             .UpdateFactorInstallmentPaymentFromUploadedFileAsync(_userPrincipal.Id);

                if (updatedCount > 0)
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = true,
                        Message = $"{updatedCount} رکورد با موفقیت بروزرسانی شد."
                    };
                }

                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "هیچ رکوردی بروزرسانی نشد."
                };

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error,
                  new EventId((int)Events.UpdateException, Events.InsertException.ToString()),
                  nameof(UploadedFactorInstallmentPayment), "Update Factor Installment Payment from uploaded excel Error", e);

                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "مشکلی در بروزرسانی جدول پیش آمده، دوباره تلاش کنید."
                };
            }
        }

        /// <summary>
        /// دریافت لیست شناسه مرجع پرداخت
        /// که براساس تاریخ ثبت اکسل در جدول
        /// UploadedFactorInstallmentPayment
        /// گروه بندی می شود
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        [HttpGet("GetFactorPaymentInstallmentReferenceGroup")]
        public async Task<LoadResult> GetFactorPaymentInstallmentReferenceGroupAsync(CustomDataSourceLoadOptions loadOptions, int yearMonth)
        {
            try
            {
                var factorPaymentInstallmentGroup =
                    await _uploadedFactorInstallmentPaymentService
                        .GetFactorPaymentInstallmentReferenceGroupAsync(yearMonth);

                return DataSourceLoader.Load(
                    factorPaymentInstallmentGroup
                        .AsQueryable()
                        .ProjectTo<UploadedFactorInstallmentPaymentReferenceGroupViewModel>(
                            _mapper.ConfigurationProvider),
                    loadOptions);

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error,
                                 new EventId((int)Events.UpdateException, Events.InsertException.ToString()),
                                 nameof(UploadedFactorInstallmentPayment), "Get Factor Payment Installment Reference Error", e);

                return null;
            }
        }

        [HttpGet("GetFactorPaymentInstallmentReference")]
        public async Task<LoadResult> GetFactorPaymentInstallmentReferenceAsync(CustomDataSourceLoadOptions loadOptions, DateTime creationDate)
        {
            var factorPaymentInstallment =  _uploadedFactorInstallmentPaymentService.GetFactorPaymentInstallmentReferenceAsync(creationDate);
            return await DataSourceLoader.LoadAsync(
                   factorPaymentInstallment                       
                       .ProjectTo<UploadedFactorInstallmentPaymentReferenceViewModel>(
                           _mapper.ConfigurationProvider),
                   loadOptions);
        }
    }
}