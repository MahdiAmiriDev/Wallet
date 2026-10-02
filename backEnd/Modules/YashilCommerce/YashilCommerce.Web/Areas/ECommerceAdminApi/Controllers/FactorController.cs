using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stimulsoft.Report;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Report;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilDms.Core.Services;
using YashilReport.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class FactorController : BaseController<Factor, int, FactorListViewModel, FactorEditModel, FactorSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFactorService _factorService;
        private readonly IYashilLogger<Factor> _logger;
        private readonly ICompanyService _companyService;
        private readonly IRepresentationService _representationService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IFactorVerificationConfigurationService _factorVerificationConfigurationService;
        private readonly IAppDocumentService _appDocumentService;
        private readonly IReportStoreService _reportStoreService;
        private readonly IAppConfigService _appConfigService;
        private readonly IFactorVerificationService _factorVerificationService;
        private readonly IFactorItemService _factorItemService;
        private readonly IFactorPaymentService _factorPaymentService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IActivityLogService _activityLogService;

        public FactorController(IFactorService factorService,
            IMapper mapper, IYashilLogger<Factor> logger,
            IPermissionService permissionService,
            ICompanyService companyService, IUserPrincipal userPrincipal,
            IFactorVerificationConfigurationService factorVerificationConfigurationService,
            IRepresentationService representationService,
            IFactorVerificationService factorVerificationService,
            IAppDocumentService appDocumentService,
            IReportStoreService reportStoreService,
            IAdditionalUserPropService additionalUserPropService,
            IAppConfigService appConfigService,
            IFactorItemService factorItemService,
            IFactorPaymentService factorPaymentService, IActivityLogService activityLogService) : base(factorService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorService = factorService;
            _logger = logger;
            _companyService = companyService;
            _userPrincipal = userPrincipal;
            _factorVerificationConfigurationService = factorVerificationConfigurationService;
            _representationService = representationService;
            _factorVerificationService = factorVerificationService;
            _appDocumentService = appDocumentService;
            _reportStoreService = reportStoreService;
            _appConfigService = appConfigService;
            _factorItemService = factorItemService;
            _factorPaymentService = factorPaymentService;
            _activityLogService = activityLogService;
            _additionalUserPropService = additionalUserPropService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? taxCategoryId, int? companyId, int? representationId, int? factorReviewStatusType)
        {
            await CheckUserCompanyAndRepresentation(companyId, representationId);

            var factors =
                _factorService.GetByCustomFilter(taxCategoryId, companyId, representationId, factorReviewStatusType);
            return await DataSourceLoader.LoadAsync(
                factors.ProjectTo<FactorListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }



        /// <summary>
        /// جهت دریافت لیست فاکتورهای یک گروه فاکتور
        /// این متد در فرم گروه بندی فاگتور مورد استفاده قرار میگیرد
        /// و در این فرم لیست آیتم های گروه که در سمت راست بنمایش گذاشته میشوند نشان داده میشود
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorGroupId"></param>
        /// <param name="taxCategoryId"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="factorReviewStatusType"></param>
        /// <returns></returns>
        [HttpGet("GetByCustomFilterForFactorGroupList")]
        [YashilAuthorization(ActionName = "FactorByCustomFilterForFactorGroup")]
        public async Task<LoadResult> GetByCustomFilterForFactorGroupList(CustomDataSourceLoadOptions loadOptions,
            int factorGroupId, int? taxCategoryId, int? companyId, int? representationId, int? factorReviewStatusType)
        {


            await CheckUserCompanyAndRepresentation(companyId, representationId);

            var factors =
                _factorService.GetByCustomFilterForFactorGroupList(factorGroupId, taxCategoryId, companyId, representationId, factorReviewStatusType);
            return await DataSourceLoader.LoadAsync(
                factors.ProjectTo<FactorListForFactorGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// متد برای مشاهده همه فاکتور ها بر اساس فیلتر companyIdو representationId
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="taxCategoryId"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="factorReviewStatusType"></param>
        /// <returns></returns>
        [HttpGet("GetSupervisorFactorList")]
        [YashilAuthorization(ActionName = "GetSupervisorFactorList")]
        public async Task<LoadResult> GetSupervisorFactorList(CustomDataSourceLoadOptions loadOptions,
            int? companyId, int? representationId, int? taxCategoryId, int? factorReviewStatusType)
        {

            await CheckUserCompanyAndRepresentation(companyId, representationId);
            var factors = _factorService.GetSupervisorFactorList(companyId, representationId, taxCategoryId, factorReviewStatusType);
            var result = await DataSourceLoader.LoadAsync(factors.ProjectTo<SuperVisorFactorListViewModel>(_mapper.ConfigurationProvider), loadOptions);
            return result;
        }

        private async Task<string> CustomValidateBeforeCreateFactorAsync(FactorEditModel editModel)
        {
            // چک کردن اینکه کاربر لاگین شده دارای مرکز هست یا خیر
            var companyId = _companyService.GetUserCompany(_userPrincipal.Id)?.Id;
            if (!companyId.HasValue)
            {
                return "کاربر گرامی، با توجه به اینکه نام کاربری شما متعلق به مرکزی نمی باشد امکان ثبت فاکتور ندارید";
            }
            //  بررسی وجود تنظیم بررسی فاکتور فعال
            if (!_factorVerificationConfigurationService.CheckExistenceActiveFactorVerificationConfiguration())
            {
                return "کاربرگرامی، تنظیم بررسی تایید فاکتور فعالی برای سیستم ثبت نشده است لطفا با مدیر سیستم تماس بگیرید.";
            }
            //دریافت کد نمایندگی کاربر لاگین شده


            // بررسی وجود فاکتورهای قبلی تایید نشده
            //if (_factorService.CheckExistenceUnconfirmedFactor(companyId.Value, representationId))
            //{
            //    return "کاربر گرامی شما فاکتورهای تعیین تکلیف نشده دارید، لطفا تا تایید شدن آنها صبر کنید.";
            //}
            // بررسی زمانبندی 
            var validateFactorScheduler = ValidateFactorYashilScheduler();
            if (validateFactorScheduler != null)
            {
                return validateFactorScheduler;
            }

            var validateUnFactoredOrderItemsSaleConfigurationInfo =
                await _factorItemService.ValidateCurrentUserUnFactoredOrderItemsMainCategoryInfo();
            if (!validateUnFactoredOrderItemsSaleConfigurationInfo)
            {
                return "کاربرگرامی، به دلیل اینکه اطلاعات کالا در دسته بندی محصول ثبت نشده است شما قادر به صدور فاکتور نمی باشید، لطفا با مدیر سیستم تماس بگیرید.";
            }


            var userRepresentationId = _additionalUserPropService.GetByUserId(_userPrincipal.Id).RepresentationId;

            if (await _factorItemService.GetUnFactoredOrderItems(companyId.Value, userRepresentationId).AnyAsync() &&

                !(await _factorItemService.GetUnFactoredOrderItems(companyId.Value, userRepresentationId)
                      .AllAsync(item => item.GenerateFactorIsAllowedBasedOnProductGroup)

                  ||

                  await _factorItemService.GetUnFactoredOrderItems(companyId.Value, userRepresentationId)
                      .AllAsync(item => !item.GenerateFactorIsAllowedBasedOnProductGroup)))
            {
                return "کاربر گرامی امکان صدور فاکتور به علت مشخص نبودن گروه محصولات وجود ندارد لطفا با مدیر سیستم تماس بگیرید.";
            }




            var currentUserUnFactoredOrderItems = _factorItemService
                .GetCurrentUserUnFactoredOrderItems(null);

            if (currentUserUnFactoredOrderItems != null && currentUserUnFactoredOrderItems.Any())
            {
                return await base.CustomValidateBeforeInsertAsync(editModel);
            }

            return "کاربر گرامی آیتمی برای فاکتور کردن وجود ندارد";


        }

        /// <summary>
        /// بررسی زمانبندی صدور فاکتور
        /// </summary>
        /// <returns></returns>
        private string ValidateFactorYashilScheduler()
        {
            return _factorService.ValidateFactorYashilScheduler();

        }

        /// <summary>
        /// زمانی یک فاکتور اجازه اپدیت و بروزرسانی را دارد که
        /// در حالت ثبت اولیه و یا رد شده باشد
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeUpdateAsync(FactorEditModel editModel)
        {
            // check the id of factor
            if (editModel.Id != 0)
            {
                var factor = _factorService.Get(editModel.Id, true);

                //check existence of factor
                if (factor != null)
                {
                    //بررسی وجود بررسی فاکتور فعال
                    if (!_factorVerificationConfigurationService.CheckExistenceActiveFactorVerificationConfiguration())
                    {
                        return "کاربرگرامی، تنظیم بررسی تایید فاکتور فعالی برای سیستم ثبت نشده است لطفا با مدیر سیستم تماس بگیرید.";
                    }
                    // دریافت اطلاعات مرکز کاربر لاگین شده
                    var userCompanyId = _companyService.GetUserCompany(_userPrincipal.Id)?.Id;
                    if (!userCompanyId.HasValue)
                    {
                        return "کاربر گرامی مرکز مربوط به اطلاعات کاربری شما تنظیم نشده است. لطفا با ادمین تماس بگیرید";
                    }
                    // check FactorReviewStatusType of factor
                    //تنها در حالت ثبت اولیه و رد شده امکان ویرایش فاکتور وجود دارد
                    if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload
                        || factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Rejected)
                    {
                        return await base.CustomValidateBeforeUpdateAsync(editModel);
                    }

                    else
                    {
                        if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed)
                        {
                            return "کاربر گرامی این فاکتور تایید شده است و امکان ویرایش ندارد.";
                        }
                        else if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Processing)
                        {
                            return "کاربر گرامی این فاکتور در حال بررسی است و امکان ویرایش ندارد.";
                        }
                        return "کاربر گرامی این فاکتور امکان ویرایش ندارد.";
                    }
                }
                else
                {
                    return "چنین فاکتوری وجود ندارد.";
                }
            }
            else
            {
                return "چنین فاکتوری وجود ندارد.";
            }
        }

        /// <summary>
        /// یک فاکتور زمانی اجازه حذف دارد که در حالت ثبت اولیه باشد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            // بررسی وجود فاکتور
            var factor = _factorService.GetFactorIfAccess(id, true);
            if (factor != null)
            {
                // بررسی وضعیت فاکتور 
                if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload)
                {
                    // در حالت ثبت اولیه است پس ادامه کار صورت میگیرد
                    return await base.CustomValidateBeforeDeleteAsync(id);
                }
                else
                {
                    if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Confirmed)
                    {
                        return "کاربر گرامی این فاکتور تایید شده است و امکان حذف ندارد.";
                    }
                    else if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Processing)
                    {
                        return "کاربر گرامی این فاکتور در حال بررسی میباشد و امکان حذف ندارد.";
                    }
                    else if (factor.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Rejected)
                    {
                        return "کاربر گرامی این فاکتور رد شده است و امکان حذف شدن توسط شما را ندارد. لطفا مشکلات فاکتور را حل کنید و مجددا دکمه ثبت نهایی را بزنید";
                    }
                    return "کاربر گرامی این فاکتور امکان حذف شدن ندارد.";
                }
            }
            else
            {
                return "کاربر گرامی چنین فاکتوری وجود ندارد.";
            }
        }

        /// <summary>
        /// سند پی دی اف مربوط به فاکتور را برمیگرداند.
        /// این متد در فرم تایید کنندگان فاکتور استفاده میشود
        /// و فایلی به تایید کننده را نشان میدهد که
        /// توسط تامین کننده اپلود شده است
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorPdf")]
        [YashilAuthorization(ActionName = "ExportPdf")]
        public async Task<FileContentResult> GetFactorPdf(int factorId)
        {

            await CheckUserAccessForFactorId(factorId);

            // دریافت فایل
            var appDoc = _appDocumentService.GetFile(factorId, (int)DocumentTypeEnum.FactorDocument);
            if (appDoc?.DocumentFile != null)
            {
                _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToPdf, ECommerceEvents.ExportToPdf.ToString()), nameof(Factor), "GetFactorPdf", null);

                return File(appDoc.DocumentFile, appDoc.ContentType, appDoc.OrginalName);
            }

            return null;
        }

        /// <summary>
        /// برای نمایش پی دی اف گزارش فاکتور استفاده میشود
        /// و در فرم تامین کننده نشان داده میشود
        /// </summary>
        /// <param name="factorId"></param>
        /// <returns></returns>
        [HttpGet("GenerateFactorFile")]
        [YashilAuthorization(ActionName = "ExportPdf")]
        public async Task<FileResult> GenerateFactorFile(int factorId)
        {

            await CheckUserAccessForFactorId(factorId);

            var factor = _factorService.GetFactorIfAccess(factorId, true);
            if (factor != null)
            {
                var factorReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorId);
                var reportStoreWithParam = new ReportStoreWithParam
                {
                    Params = new Dictionary<string, string> { { "factorId", factorId.ToString() } }
                };

                var reportFile = await _reportStoreService.GetReportFileAsync(factorReportId, StiExportFormat.Pdf, reportStoreWithParam);
                if (reportFile != null)
                {
                    _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToPdf, ECommerceEvents.ExportToPdf.ToString()), nameof(Factor), "GenerateFactorFile", null);

                    return File(reportFile, "application/pdf");
                }
            }
            return null;
        }

        /// <summary>
        /// برای نمایش پی دی اف گزارش فاکتور استفاده میشود
        /// و در فرم تایید کننده نشان داده میشود
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenerateVerifierFactorFile")]
        [YashilAuthorization(ActionName = "ExportPdf")]
        public async Task<FileResult> GenerateVerifierFactorFile(int factorVerificationId)
        {
            // در اینجا نباید از متد getFactorIfAccess استفاده شود
            // زیرا فایل اپلود شده برای کاربر نمیباشد
            var factorVerification = _factorVerificationService.Get(factorVerificationId, true);
            if (factorVerification != null)
            {
                var factor = await _factorService.GetAsync(factorVerification.FactorId, true);
                if (factor != null)
                {
                    await CheckUserAccessForFactorId(factor.Id);


                    var factorReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorId);
                    var reportStoreWithParam = new ReportStoreWithParam()
                    {
                        Params = new Dictionary<string, string> { { "factorId", factor.Id.ToString() } }
                    };

                    var reportFile = await _reportStoreService.GetReportFileAsync(factorReportId, StiExportFormat.Pdf, reportStoreWithParam);
                    if (reportFile != null)
                    {
                        _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToPdf, ECommerceEvents.ExportToPdf.ToString()), nameof(Factor), "GenerateVerifierFactorFile Record", null);

                        return File(reportFile, "application/pdf");
                    }
                }

            }
            return null;
        }
        /// <summary>
        /// ثبت نهایی فاکتور
        /// هنگامی کاربر اجازه زدن دکمه ثبت نهایی را دارد که
        /// فاکتور در حالت رد شده یا ثبت اولیه باشد
        /// فایل آپلود شده باشد
        /// تنظیم بررسی فاکتور وجود داشته باشد
        /// </summary>
        /// <param name="factorModel"></param>
        /// <returns></returns>
        [HttpPost("FinalFactorRegistration")]
        [YashilAuthorization(ActionName = "PostEntity")]
        public async Task<IActionResult> FinalFactorRegistrationAsync(FactorSimpleViewModel factorModel)
        {
            await CheckUserAccessForFactorId(factorModel.Id);

            var factorResultMsg = await _factorService.FinalFactorRegistration(factorModel.Id, true);
            if (factorResultMsg != null)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = factorResultMsg
                });
            }
            return Accepted(new ResultStatus<int>
            {
                IsSuccess = true,
                Message = "کاربر گرامی ثبت نهایی با موفقیت انجام شد و فاکتور شما جهت بررسی ارسال گردید."
            });
        }

        /// <summary>
        /// برای فرم مشاهده همه فاکتور ها
        /// شروط حذف این فرم با شروط حذف فرم فاکتور متفاوت است
        /// در اینجا هنگامی یک فاکتور میتواند حذف شود که در حالت رد شده باشد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteFactor/{id}")]
        [YashilAuthorization(ActionName = "CustomDelete")]
        public async Task<IActionResult> DeleteFactor(int id)
        {
            var factor = _factorService.GetFactorIfAccess(id, true);
            if (factor == null)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = @"کاربر گرامی، فاکتور مورد نظر وجود ندارد یا شما به آن دسترسی ندارید"
                });
            }
            await CheckUserAccessForFactorId(factor.Id);

            if (factor.FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.Rejected)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = @"کاربر گرامی، فاکتور مورد نظر باید در وضعیت رد شده باشد"
                });
            }
            try
            {
                await _factorService.DeleteAsync(id, true);
                var serializedFactor = JsonSerializer.Serialize(factor, new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                await _activityLogService.AddActivityLogAsync(
                    _userPrincipal.Id,
                    ControllerContext.ActionDescriptor.ControllerName
                        .Replace("Controller", ""),
                    ControllerContext.ActionDescriptor.ActionName,
                    id,
                    serializedFactor,
                    "حذف فاکتور"
                );
                _logger.InsertLog(LogLevel.Information, new EventId(3, Events.Delete.ToString()), nameof(Factor), $"Delete Record FactorId = {id}", null);
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = true,
                    Message = "اطلاعات با موفقیت حذف شد",
                    EntityId = id
                });
            }
            catch (Exception ex)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(6, Events.DeleteException.ToString()), nameof(Factor), "Delete Record Record Id Is " + id.ToString(), ex);
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "بروز خطا در حذف رکورد",
                    EntityId = id
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Factor Id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("RemoveFactorFromFactorGroup/{id}")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<IActionResult> RemoveFactorFromFactorGroup(int id)
        {
            var factor = await _factorService.GetAsync(id, false);
            if (factor == null)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = @"کاربر گرامی، فاکتور مورد نظر وجود ندارد یا شما به آن دسترسی ندارید"
                });
            }
            if (factor.FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.Confirmed)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = @"کاربر گرامی، فاکتور مورد نظر باید در وضعیت تایید شده باشد"
                });
            }
            try
            {
                factor.FactorGroupId = null;
                await _factorService.UpdateAsync(factor, id, true);
                _logger.InsertLog(LogLevel.Information, new EventId((int)Events.Update, Events.Update.ToString()), nameof(Factor), " Delete from FactorGroup Record", null);
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = true,
                    Message = "فاکتور از گروه کانورت حذف شد",
                    EntityId = id
                });
            }
            catch (Exception ex)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(6, Events.UpdateException.ToString()), nameof(Factor), "error Delete from FactorGroup Record Id Is " + id.ToString(), ex);
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "بروز خطا در حذف فاکتور از گرو کانورت",
                    EntityId = id
                });
            }
        }

        /// <summary>
        /// ایجاد فاکتور
        /// </summary>
        /// <param name="factorEditModel"></param>
        /// <returns></returns>
        [HttpPost("CreateFactor")]
        [YashilAuthorization(ActionName = "PostEntity")]
        public async Task<IActionResult> CreateFactorAsync(FactorEditModel factorEditModel)
        {
            string msg = await CustomValidateBeforeCreateFactorAsync(factorEditModel);
            if (!string.IsNullOrEmpty(msg))
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = msg
                });
            }
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.InsertLog(LogLevel.Information, new EventId(8, Events.InvalidParamsInsert.ToString()), nameof(Factor), message, null);
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "مقادیر وارد شده صحیح نیست"
                });
            }
            Factor entity = _mapper.Map<FactorEditModel, Factor>(factorEditModel);
            await CustomMapBeforeInsertAsync(factorEditModel, entity);
            try
            {
                var result = await _factorService
                    .CreateFactorAsync(entity, factorEditModel.ProductSalesPlanningIds, true);
                _logger.InsertLog(LogLevel.Information, new EventId(1, Events.Insert.ToString()), nameof(Factor), "Insert Record", null);
                await AfterInsertAsync(factorEditModel, entity);
                if (result)
                {
                    return Accepted(new ResultStatus<int>
                    {
                        IsSuccess = true,
                        Message = "اطلاعات با موفقیت ذخیره شد",
                        EntityId = entity.Id
                    });
                }
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در عملیات دخیره سازی"
                });


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _logger.InsertLog(LogLevel.Error, new EventId(5, Events.InsertException.ToString()), nameof(Factor), "error create factor", ex);
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا در عملیات دخیره سازی"
                });
            }

        }

        /// <summary>
        /// دریافت لیست فاکتورهایی که به یک گروه اساین شده اند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetFactorsAssignedToFactorGroupAsync")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetFactorsAssignedToFactorGroupAsync(
            CustomDataSourceLoadOptions loadOptions,
            int id)
        {
            IQueryable<Factor> entities = _factorService.GetFactorsAssignedToFactorGroupAsync(id);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<FactorAssignListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// دریافت لیست فاکتورهایی که تاکنون گروه بندی نشده اند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی محصول</param>
        /// <returns></returns>
        [HttpGet("GetFactorsNotAssignedToFactorGroupAsync")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetFactorsNotAssignedToFactorGroupAsync(
            CustomDataSourceLoadOptions loadOptions, int? categoryGroupId)
        {
            IQueryable<Factor> entities = _factorService.GetFactorsNotAssignedToFactorGroupAsync(categoryGroupId);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<FactorAssignListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// اساین کردن فاکتورهای انتخاب شده به یک گروه
        /// و یا حذف فاکتورهای انتخاب شده از یک گروه
        /// </summary>
        /// <param name="assignableListEditModel"></param>
        /// <returns></returns>
        [HttpPost("AssignSelectedItemsToFactorGroup")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<ResultStatus<int>> AssignSelectedItemsToFactorGroup(AssignableListEditModel assignableListEditModel)
        {
            var factorIds = new List<int>(assignableListEditModel.SelectedItems);
            var factorGroupId = assignableListEditModel.GroupId;

            //بنا به درخواست مالی کامنت شد
            //if (assignableListEditModel.Assign)
            //{
            //var res = await
            //    _factorService.ValidateAssignSelectedItemsToFactorGroup(factorIds, factorGroupId);
            //if (!res.isSuccess)
            //{
            //    return new ResultStatus<int>
            //    {
            //        IsSuccess = false,
            //        Message = res.message
            //    };
            //}

            //}
            var isSuccess = await _factorService.AssignSelectedItemsToFactorGroup(
                factorIds, factorGroupId,
                 assignableListEditModel.Assign);

            return new ResultStatus<int>
            {
                IsSuccess = isSuccess,
                Message = "گروه بندی فاکتور های انتخاب شده با موفقیت انجام شد."
            };
        }

        /// <summary>
        /// برای دریافت لیست گزارش آیتمهای گروه بندی فاکتور
        /// آیتمهای گروه بندی فاکتور در واقع همان فاکتور ها هستند پس ما گزارشی از فاکتور ها میخواهیم.
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorGroupItemList")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetFactorGroupItemList(CustomDataSourceLoadOptions loadOptions, int factorGroupId)
        {
            var factorGroupItems = await _factorService.GetFactorGroupItemList(factorGroupId).ToListAsync();

            return DataSourceLoader.Load(factorGroupItems, loadOptions);
        }
        /// <summary>
        /// این متد هنگامی استفاده میشود که کاربر دکمه اضافه کردن فاکتور را میزند
        /// و در هنگامی که فرم اضافه کردن فاکتور باز میشود
        /// اگر مشکلی وجود داشته باشد
        /// پیغامی که این متد برمیگرداند را باید به کاربر نشان دهیم
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("ValidateShowSaveBtn")]
        public async Task<IActionResult> ValidateShowSaveBtnAsync()
        {
            var msg = await CustomValidateBeforeCreateFactorAsync(null);
            if (!string.IsNullOrEmpty(msg))
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = msg
                });
            }
            return Accepted(new ResultStatus<int>
            {
                IsSuccess = true,
                Message = null
            });

        }

        /// <summary>
        /// دریافت لیست فاکتورهای گروه بندی شده تایید نهایی شده
        /// این لیست برای نمایش در فرم پرداختی های فاکتور کاربرد دارد
        /// فاکتورهایی که گروه بندی شده اند و گروه آن ثبت نهایی شده است
        /// میتوانند به کاربر نشان داده شوند تا پرداختی برای آن ثبت شود.
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        [HttpGet("GetGroupedFactorList")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetGroupedFactorList(CustomDataSourceLoadOptions loadOptions, int? companyId, int? representationId)
        {
            var entities = _factorService.GetGroupedFactors(companyId, representationId);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<GroupedFactorListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// تمامی تراکنش های پرداخت مربوط به فاکتور را نمایش میدهد 
        ///  و برای تراکنش های از نوع پرداخت حکمت  با دیتای  تراکنش های تایید شده حکمت که از طریق سی دی از شرکت اتکا گرفته میشود
        ///  مقایسه میشود و در صورت وجود نداشتن تراکنش حکمت در سی دی و مغایرت قیمت ، شماره حکمت ، و کد رهگیری پدیرنده
        ///  آن را به عنوان خروجی برمی گرداند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorId"></param>
        /// <param name="showContrast">نمایش مغایرت ها</param>
        /// <returns></returns>
        [HttpGet("PaymentComparisonWithHekmatApprovedTransactionByFactorCode")]
        [YashilAuthorization]
        public async Task<LoadResult> PaymentComparisonWithHekmatApprovedTransactionByFactorCode(
            CustomDataSourceLoadOptions loadOptions, int factorId, bool showContrast)
        {
            await CheckUserAccessForFactorId(factorId);

            var paymentComparisonWithHekmatApprovedTransaction =
                _factorService.GetPaymentComparisonWithHekmatApprovedTransactionByFactorCode(factorId, showContrast);
            return await DataSourceLoader.LoadAsync(
                paymentComparisonWithHekmatApprovedTransaction
                    .ProjectTo<PaymentComparisonWithHekmatApprovedTransactionViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        /// <summary>
        /// دریافت اطلاعات پرداخت فاکتور نظیر اینکه تا کنون چه مقداری پرداخت شده است
        /// و چه مقداری باقیمانده و چه تعدادی پرداخت شده است
        /// این اطلاعات در فرم ویرایش پرداخت فاکتور به نمایش گذاشته میشود.
        /// </summary>
        /// <param name="factorId"></param>
        /// <param name="factorPaymentId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorStatistic")]
        [YashilAuthorization(ActionName = "GetEntity")]
        public async Task<FactorStatisticViewModel> GetFactorStatistic(int factorId, int? factorPaymentId)
        {

            await CheckUserAccessForFactorId(factorId);
            // در صورتی که مقدار factorPaymentId
            // داشته باشد باید مبلغ این پرداختی از اطلاعات فاکتور کسر گردد
            // این قابلیت برای ان گذاشته شده است تا در فرم پرداختی فاکتور بتوان امکان ویرایش داشت و هنگام ویرایش مبلغ پرداختی فاکتور نباید لحاظ شود
            var factorStatistic = _factorService.GetFactorStatistic(factorId);
            if (factorPaymentId != null)
            {
                var factorPayment = await _factorPaymentService.GetAsync(factorPaymentId, true);
                if (factorPayment == null)
                {
                    return null;
                }

                factorStatistic.SumPaid -= factorPayment.Price;
                factorStatistic.PaidFactorInstallmentCount -= factorPayment.InstallmentCount;
            }

            return _mapper.Map<FactorStatisticDomainModel, FactorStatisticViewModel>(factorStatistic);
        }


        /// <summary>
        /// جهت دریافت لیست پرداختی های آیتمهای فاکتور
        /// فاکتورهای گروه بندی شده
        /// این متد در گزارش کانورت مورد استفاده قرار میگیرد و در تب ریز پرداخت های آیتم های گروه قرار میگیرد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorItemPaymentFullInfoViewList")]
        [YashilAuthorization(ActionName = "GetFactorItemPaymentFullInfoViewList")]
        public async Task<LoadResult> GetFactorItemPaymentFullInfoViewList(CustomDataSourceLoadOptions loadOptions,
            int factorGroupId)
        {

            var factorItemPaymentFullInfoViews = _factorService.GetFactorItemPaymentFullInfoViewList(factorGroupId);
            return await DataSourceLoader.LoadAsync(
                factorItemPaymentFullInfoViews.ProjectTo<FactorItemPaymentFullInfoViewViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// به روز رسانی اطلاعات خریدار مربوط به فاکتور
        /// و تغییر وضعیت فاکتور به وضعیت منتظر بارگذاری فایل 
        /// </summary>
        /// <param name="factorInformationEditModel"></param>
        /// <returns></returns>
        [HttpPost("ModifyFactorInformation")]
        [YashilAuthorization(ActionName = "ModifyFactorInformation")]
        public async Task<IActionResult> ModifyFactorInformationAsync(FactorInformationEditModel factorInformationEditModel)
        {

            var additionalUser = _additionalUserPropService.GetByUserId(_userPrincipal.Id);
            if (additionalUser != null && (additionalUser.CompanyId.HasValue || additionalUser.RepresentationId.HasValue))
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی شما مجاز به تغییر اطلاعات فروشنده فاکتور نمی باشید."
                });
            }
            var factor = await _factorService.GetAsync(factorInformationEditModel.Id, false);
            if (factor == null)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "فاکتور مورد نظر یافت نشد."
                });
            }
            if (factor.FactorReviewStatusType != (int)FactorReviewStatusTypeEnum.WaitingForConfirmAdmin)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی فاکتور در وضعیت ثبت اولیه نمی باشد، شما نمی توانید اطلاعات فروشنده را تغییر دهید."
                });
            }
            // اگر فاکتور برای شخص حقوقی باشد باشد و کد اقتصادی را وارد نکرده باشد 
            if (factor.IsLegalPerson && string.IsNullOrEmpty(factorInformationEditModel.EconomicCode))
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، اطلاعات فاکتور برای شخص حقوقی می باشد. لطفا کد اقتصادی را مقدار دهی نمایید.."
                });
            }
            // اگر فاکتور برای شخص حقیقی باشد باشد و کد ملی را وارد نکرده باشد 
            if (!factor.IsLegalPerson && string.IsNullOrEmpty(factorInformationEditModel.NationalCode))
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، اطلاعات فاکتور برای شخص حقیقی می باشد. لطفا کد ملی را مقدار دهی نمایید.."
                });
            }
            factor.CompanyTitle = factorInformationEditModel.CompanyTitle;
            factor.FullAddress = factorInformationEditModel.FullAddress;
            factor.EconomicCode = factorInformationEditModel.EconomicCode;
            factor.Telephone = factorInformationEditModel.Telephone;
            factor.FaxNumber = factorInformationEditModel.FaxNumber;
            factor.FactorReviewStatusType = (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload;
            factor.PersonBankAccountId = factorInformationEditModel.PersonBankAccountId;

            var result = await _factorService.UpdateAsync(factor, factor.Id, true);
            if (result != null)
            {
                return Accepted(new ResultStatus<int>()
                {
                    IsSuccess = true,
                    Message = "اطلاعات با موفقیت به روز رسانی شد.",
                    EntityId = factor.Id
                });
            }
            return Accepted(new ResultStatus<int>()
            {
                IsSuccess = false,
                Message = "بروز خطا در عملیات  به روز رسانی."
            });

        }

        /// <summary>
        /// یک فاکتور زمانی اجازه ایجاد دارد که کاربر متعلق یه یک کمپانی باشد
        /// تنظیم بررسی فاکتور موجود باشد
        /// در صورت مبتنی برقرارداد بودن قرارداد فعال داشته باشدو قرارداد آن اتمام نشده باشد
        /// و اگر مبتنی برقرارداد نبود تنظیم فروش فعال وجود داشته باشد
        /// و در نهایت آیتمی برای فاکتور وجود داشته باشد
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeInsertAsync(FactorEditModel editModel)
        {
            return await CustomValidateBeforeCreateFactorAsync(editModel);
        }
        private async Task CheckUserCompanyAndRepresentation(int? companyId, int? representationId)
        {
            var userHasAccessForCompanyAndRepresentation = await _representationService.CheckUserAccessForCompanyAndRepresentationAsync(companyId, representationId);
            if (!userHasAccessForCompanyAndRepresentation)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(12, "GetByCustomFilterForList"), nameof(Factor),
                    "AccessDenied GetByCustomFilterForList Error", null);

                throw new Exception();
            }
        }
        private async Task CheckUserAccessForFactorId(int factorId)
        {

            bool userHasAccessForFactor = await _factorService.CheckUserAccessForFactorId(factorId);
            if (!userHasAccessForFactor)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "GetByCustomFilterForList"),
                    nameof(Factor), "GetByCustomFilterForList Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Factor)}");
            }
        }
        /// <summary>
        /// تغییر وضعیت فاکتور تایید شده به وضعیت در حال بررسی
        /// این متد برای حالت زیر کاربرد دارد :
        /// برای حذف فاکتور ها تایید شده، فاکتور باید به وضعیت در حال بررسی تغییر کند
        /// تا آخرین تایید کننده این فاکتور را رد کند و
        /// در نهایت تامین کننده فاکتور را حذف کند
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("ChangeFactorToProcessingStatus")]
        [YashilAuthorization(ActionName = "ChangeFactorToProcessingStatus")]
        public async Task<IActionResult> ChangeFactorToProcessingStatus(int id)
        {
            var changedFactorStatus = await _factorService
                .ChangeFactorStatusToProcessingStatusAsync(id);
            if (!changedFactorStatus.IsSucces)
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = changedFactorStatus.Message
                });
            }
            return Accepted(new ResultStatus<int>
            {
                IsSuccess = true,
                Message = "کاربر گرامی, تغییر وضعیت فاکتور به وضعیت در حال بررسی با موفقیت انجام شد."
            });
        }

        /// <summary>
        /// دانلود فاکتور های کانورت شده
        /// با امکان فیلتر بر اساس شناسه گروهبندی
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GetConvertedFactors")]
        //[YashilAuthorization("GetConvertedFactors")]
        public async Task<LoadResult> GetConvertedFactorsAsync(CustomDataSourceLoadOptions loadOptions, int? factorGroupId)
        {

            return await DataSourceLoader.LoadAsync(
                _factorService.GetConvertedFactor(factorGroupId)
                    .ProjectTo<FactorListViewModel>(_mapper.ConfigurationProvider), loadOptions);

        }

        [HttpGet("UpdateFactorInstallmentCountFromSaleConfiguration")]
        [YashilAuthorization("UpdateFactorInstallmentCountFromSaleConfiguration")]
        public async Task<ResultStatus<int>> UpdateFactorInstallmentCountFromSaleConfiguration(int? factorId)
        {
            if (factorId is null or <= 0)
                throw new ArgumentOutOfRangeException(nameof(factorId));

            try
            {
                await _factorService.UpdateFactorInstallmentCountFromSaleConfiguration(factorId.Value);

                return new ResultStatus<int>(true, "بروزرسانی با موفقیت انحام شد.");
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(UpdateFactorInstallmentCountFromSaleConfiguration),
                    $"بروز خطا به هنگام بروز رسانی مقدار تعداد اقساط فاکتور با شناسه فاکتور {factorId} از طریق تنظیم فروش", e);

                return new ResultStatus<int>(false, "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.");
            }
        }


    }
}