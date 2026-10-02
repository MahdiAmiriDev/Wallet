using System;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Enums;
using YashilDms.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class UploadedVoucherController : BaseController<UploadedVoucher, int, UploadedVoucherListViewModel,
        UploadedVoucherEditModel, UploadedVoucherSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUploadedVoucherService _uploadedVoucherService;
        private readonly IYashilLogger<UploadedVoucher> _logger;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IVocherService _voucherService;
        private readonly IAppDocumentService _appDocumentService;
        private readonly ICompanyService _companyService;
        private readonly IPermissionService _permissionService;
        public UploadedVoucherController(IUploadedVoucherService uploadVoucherService,
            IMapper mapper, ApplicationConfiguration applicationConfiguration,
            IVocherTypeService voucherTypeService, IVocherService voucherService,
            IYashilLogger<UploadedVoucher> logger,
            IPermissionService permissionService, IUserPrincipal userPrincipal, IAppDocumentService appDocumentService,
            ICompanyService companyService) : base(uploadVoucherService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _uploadedVoucherService = uploadVoucherService;
            _logger = logger;
            _permissionService = permissionService;
            _userPrincipal = userPrincipal;
            _appDocumentService = appDocumentService;
            _companyService = companyService;
            _applicationConfiguration = applicationConfiguration;
            _voucherService = voucherService;
            _appDocumentService = appDocumentService;
        }

        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? appDocumentId)
        {
            var uploadVoucherTypes = _uploadedVoucherService.GetByCustomFilter(appDocumentId);
            return await DataSourceLoader.LoadAsync(
                uploadVoucherTypes.ProjectTo<UploadedVoucherListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? appDocumentId)
        {
            var uploadVoucherTypes = _uploadedVoucherService.GetByCustomFilter(appDocumentId);
            return await DataSourceLoader.LoadAsync(
                uploadVoucherTypes.ProjectTo<UploadedVoucherSimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// ابتدا رکورد های قبلی مربوط به کاربر لاگین کرده را از جدول 
        /// Temp UploadVoucher
        /// پاک میکند. 
        /// سپس فایل ارسالی از سمت کاربر را به سرویس درج فایل از طریق اکسل می فرستد
        /// درصورتی که بارگذاری فایل اکسل با موفقیت انجام شود و خطایی در آیتم های مربوط به قیمت نباشد
        /// جدول Voucher
        /// را به روز رسانی میکند
        /// </summary>
        /// <param name="tempVoucherFile">فایل ارسالی ار سمت کاربر</param>
        /// <param name="companyId">سناسه مرکز</param>
        /// <param name="voucherTypeId">نوع کوپن</param>
        /// <returns></returns>
        [HttpPost("uploadedVoucherFile/{companyId},{voucherTypeId}")]
        public async Task<ResultStatus<int>> UploadedVoucherFileAsync(IFormFile tempVoucherFile, int companyId,
            int voucherTypeId)
        {
            try
            {
                // ابتدا رکورد های مربوط به کاربر لاگین کرده را پاک میکند 
                _uploadedVoucherService.DeleteByUserId(_userPrincipal.Id);
                var res = await _uploadedVoucherService.BulkInsertFromExcelAsync(tempVoucherFile, companyId,
                    voucherTypeId);
                if (res.IsSuccess)
                {
                    // بعد از درج کد کوپن مقادیر کد کوپن ها را باید رمز کنیم
                    var encryptionResult = await EncryptVoucherCode(_userPrincipal.Id);
                    if (encryptionResult)
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = true,
                            Message = "کاربر گرامی فایل اکسل کد کوپن با موفقیت بارگذاری شد"
                        };
                    }

                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "خطا در بارگذاری کد کوپن"
                    };
                }

                return res;
            }
            catch
            {
                _logger.InsertLog(LogLevel.Error,
                    new EventId((int) Events.InsertException, Events.InsertException.ToString()),
                    nameof(RefundDetail), "Upload VoucherCode Bulk Insert Error", null);
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در بارگذاری فایل اکسل"
                };
            }
        }

        /// <summary>
        /// شناسه مرکز و شناسه نوه ووچر انتخابی را برای دانلود نمونه به متد  
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="voucherTypeId">شناسه مرکز</param>
        /// <returns></returns>
        [HttpGet("ExportCompanyVoucherCodeToExcel")]
        public async Task<FileResult> ExportCompanyVoucherCodeToExcelAsync(int? companyId, int? voucherTypeId)
        {
            if (companyId == null || voucherTypeId == null)
            {
                return null;
            }

            var ms = await _uploadedVoucherService.CopySampleVoucherCodeFileToWorkBook(companyId.Value,
                voucherTypeId.Value);
            return File(ms, MimeTypes.GetMimeType("VoucherCodeExcel.xlsx"), "VoucherCodeExcel.xlsx");
        }

        [HttpPost("AddVoucherFromUploadedVoucher")]
        public async Task<ResultStatus<int>> AddVoucherFromUploadedVoucherAsync(int? companyId, int? voucherTypeId)
        {
            if (companyId == null)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، مرکز انتخابی وجود ندارد"
                };
            }

            if (voucherTypeId == null)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، نوع کوپن انتخابی وجود ندارد"
                };
            }

            int? res = await _voucherService.AddVoucherFromUploadedVoucher(_applicationConfiguration.ApplicationId,
                _userPrincipal.Id, companyId.Value, voucherTypeId.Value);
            if (res > 0)
            {
                // ابتدا رکورد های مربوط به کاربر لاگین کرده را پاک میکند 
                _uploadedVoucherService.DeleteByUserId(_userPrincipal.Id);
                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "کاربر گرامی فایل مورد نظر با موفقیت بارگذاری شد"
                };
            }

            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "خطا در ثبت نهایی"
            };
        }

        /// <summary>
        /// دریافت اطلاعات فایل اکسل ذخیره شده - برای نمایش دادن در لیست
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="showInvalidItem"></param>
        /// <returns></returns>
        [HttpGet("GetUploadedVoucher")]
        public async Task<LoadResult> GetUploadedVoucherAsync(CustomDataSourceLoadOptions loadOptions)
        {
            var uploadedVoucherCodes = _uploadedVoucherService.GetByUserId(_userPrincipal.Id);
            return await DataSourceLoader.LoadAsync(
                uploadedVoucherCodes.ProjectTo<UploadedVoucherListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// فایل بارگذاری شده کد کوپن را بر می گرداند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetTransactionFiles")]
        [YashilAuthorization(ActionName = "UploadedVoucherFiles")]
        public async Task<LoadResult> GetTransactionFilesAsync(CustomDataSourceLoadOptions loadOptions)
        {
            var transactionFiles = _appDocumentService.GetObjectDocumentsByUserId(
                Enum.GetName(AppEntityEnum.UploadedVoucher), _userPrincipal.Id,
                (int) DocumentTypeEnum.UploadVoucher);

            return await DataSourceLoader.LoadAsync(
                transactionFiles.ProjectTo<UploadedFileListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// کد کوپن بارگذاری شده توسط کاربر را بعد از بارگذاری موفق رمز می کند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="voucherUrlPrefix">آدرس ارسال کد کوپن</param>
        /// <returns></returns>
        private async Task<bool> EncryptVoucherCode(int userId)
        {
            var result = await _uploadedVoucherService.EncryptVoucherCode(userId);
            return result;
        }
    }
}