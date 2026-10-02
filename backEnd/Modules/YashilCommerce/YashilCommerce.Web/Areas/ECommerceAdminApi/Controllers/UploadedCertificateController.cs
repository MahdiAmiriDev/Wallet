
using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Filters;
using MimeKit;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Data;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.Enums;
using YashilDms.Core.Services;
using YashilLight.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class UploadedCertificateController : BaseController<UploadedCertificate, int, UploadedCertificateListViewModel, UploadedCertificateEditModel, UploadedCertificateSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IUploadedCertificateService _uploadedCertificateService;
        private readonly IYashilLogger<UploadedCertificate> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAppDocumentService _appDocumentService;
        public UploadedCertificateController(IUploadedCertificateService uploadedCertificateService,
        IMapper mapper,
        IYashilLogger<UploadedCertificate> logger,
        IPermissionService permissionService, IUserPrincipal userPrincipal, IAppDocumentService appDocumentService) : base(uploadedCertificateService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _uploadedCertificateService = uploadedCertificateService;
            _logger = logger;
            _permissionService = permissionService;
            _userPrincipal = userPrincipal;
            _appDocumentService = appDocumentService;
        }

        /// <summary>
        /// بارگذاری فایل اکسل گواهینامه
        /// </summary>
        /// <param name="tempCertificateFile"></param>
        /// <returns></returns>
        [HttpPost("UploadUserCertificateFile")]
        [YashilAuthorization(ActionName = "PostEntity")]
        public async Task<ResultStatus<int>> UploadCertificateFile(IFormFile tempCertificateFile)
        {
            try
            {

                if (tempCertificateFile == null)
                    return new ResultStatus<int>(false, "کاربر گرامی فایل بارگذاری شده فاقد مقدار صحیح می بشاد");


                // ابتدا رکورد های مربوط به کاربر لاگین کرده را پاک میکند 
                await _uploadedCertificateService.DeleteByUserIdAsync(_userPrincipal.Id);

                var insertResult = await _uploadedCertificateService.BulkInsertFromExcelAsync(tempCertificateFile);

                if (!insertResult.IsSuccess)
                    return new ResultStatus<int>(false, "بزور خطا به هنگام بارگذاری فایل اکسل");

                var invalidRowCount =
                   await _uploadedCertificateService.GetInvalidUploadedCertificateCountAsync(_userPrincipal.Id);

                if (invalidRowCount > 0)
                    return new ResultStatus<int>(false, "خطا در ثبت نهایی، آیتم های بارگذاری شده دارای خطا می باشد");


                return new ResultStatus<int>(true, "عملیات با موفقیت انجام شد.");

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(UploadedCertificate),
                    "بروز خطا به هنگام درج اطلاعات گواهینامه از طریق اکسل", e);

                return new ResultStatus<int>(false, "بزور خطا به هنگام بارگذاری فایل اکسل");
            }



        }


        [HttpGet("AddCertificateFromUploadedCertificate")]
        [YashilAuthorization(ActionName = "GetEntity")]
        public async Task<ResultStatus<int>> AddCertificateFromUploadedCertificateAsync()
        {
            try
            {
                if (!await _uploadedCertificateService.HasUserUploadAnyCertificateAsync(_userPrincipal.Id))
                    return new ResultStatus<int>(false,
                        "کاربر گرامی شما هیج گواهینامه ای در سامانه بارگذاری نکرده اید.");

                var invalidRowCount =
                    await _uploadedCertificateService.GetInvalidUploadedCertificateCountAsync(_userPrincipal.Id);

                if (invalidRowCount > 0)
                    return new ResultStatus<int>(false, "خطا در ثبت نهایی، آیتم های بارگذاری شده دارای خطا می باشد");

                var isHashingDoneSuccessful =
                    await _uploadedCertificateService.HashRequiredColumnsAsync(_userPrincipal.Id);

                if (!isHashingDoneSuccessful)
                    return new ResultStatus<int>(false, "بروز خطا به هنگام انجام عملیات رمزنگاری");

                var isSuccessfulInsertUploadedCertificateToCertificate = await _uploadedCertificateService
                    .AddCertificateFromUploadedCertificateAsync(_userPrincipal.Id);

                if (isSuccessfulInsertUploadedCertificateToCertificate <= 0)
                    return new ResultStatus<int>(false,
                        "کاربر گرامی، در ثبت نهایی درج کاربر از طریق اکسل خطا رخ داده است");
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(UploadedCertificate),
                    "بروز خطا به هنگام درج اطلاعات گواهینامه از طریق جدول آپلود شده به جدول اصلی", e);

                return new ResultStatus<int>(false, "بزور خطا به هنگام درج اطلاعات");
            }

            return new ResultStatus<int>(true, "عملیات با موفقیت انجام شد.");



        }


        /// <summary>
        /// دانلود نمونه اکسل برای بارگذاری کاربران
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExportSampleCertificateFileToExcel")]
        public FileResult ExportSampleUsersFileToExcel()
        {
            var sampleUsersFileWorkBook = _uploadedCertificateService.CopySampleCertificateFileToWoorkbook();

            return File(sampleUsersFileWorkBook, MimeTypes.GetMimeType("SampleCertificateFile.xlsx"),
                "SampleCertificateFile.xlsx");
        }

        /// <summary>
        /// دریافت اطلاعات فایل اکسل ذخیره شده - برای نمایش دادن در لیست
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="showInvalidItem"></param>
        /// <returns></returns>
        [HttpGet("GetUploadedCertificateItems")]
        [YashilAuthorization(ActionName = "GetEntity")]
        public async Task<LoadResult> GetUploadedUserItemsAsync(CustomDataSourceLoadOptions loadOptions, bool showInvalidItem)
        {
            return await DataSourceLoader.LoadAsync(
                _uploadedCertificateService.GetByUserId(_userPrincipal.Id, showInvalidItem)
                    .ProjectTo<UploadedCertificateListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// اطلاعات فایل بارگذاری کاربران را بر می گرداند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GeUploadedInfo")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GeUploadedInfo(CustomDataSourceLoadOptions loadOptions)
        {
            var transactionFiles = _appDocumentService.GetObjectDocumentsByUserId(
                Enum.GetName(AppEntityEnum.UploadedCertificate), _userPrincipal.Id, (int)DocumentTypeEnum.UploadedCertificate);


            return await DataSourceLoader.LoadAsync(
                transactionFiles.ProjectTo<UploadedCertificateInfoViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}