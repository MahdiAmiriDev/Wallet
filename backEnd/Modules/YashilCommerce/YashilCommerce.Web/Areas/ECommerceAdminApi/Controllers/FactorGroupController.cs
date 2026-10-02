using System;
using System.Collections.Generic;
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
using Yashil.Core.CustomDomainModels.Report;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilReport.Core.Services;
using YashilUserManagement.Core.Services;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class FactorGroupController : BaseController<FactorGroup, int, FactorGroupListViewModel, FactorGroupEditModel
        , FactorGroupSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFactorGroupService _factorGroupService;
        private readonly IYashilLogger<FactorGroup> _logger;
        private readonly IAppConfigService _appConfigService;
        private readonly IReportStoreService _reportStoreService;
        private readonly IActivityLogService _activityLogService;
        private readonly IUserPrincipal _userPrincipal;


        public FactorGroupController(IFactorGroupService factorGroupService, IMapper mapper,
            IYashilLogger<FactorGroup> logger, IPermissionService permissionService,
            IAppConfigService appConfigService, IReportStoreService reportStoreService,
            IActivityLogService activityLogService, IUserPrincipal userPrincipal) : base(factorGroupService, mapper,
            logger, permissionService)
        {
            _mapper = mapper;
            _factorGroupService = factorGroupService;
            _logger = logger;
            _appConfigService = appConfigService;
            _reportStoreService = reportStoreService;
            _activityLogService = activityLogService;
            _userPrincipal = userPrincipal;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId)
        {
            var factorGroups = _factorGroupService.GetByCustomFilter(parentId);
            return await DataSourceLoader.LoadAsync(
                factorGroups.ProjectTo<FactorGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId)
        {
            var factorGroups = _factorGroupService.GetByCustomFilter(parentId);
            return await DataSourceLoader.LoadAsync(
                factorGroups.ProjectTo<FactorGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// ثبت نهایی گروه فاکتور
        /// پس از ثبت نهایی امکان دانلود گزارش های گروه و گزارش کلی گروه وجود دارد
        /// پس از ثبت نهایی امکان ویرایش و یا حذف گروه به هیچ عنوان وجود ندارد
        /// </summary>
        /// <param name="factorGroupSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("ConfirmFactorGroup")]
        [YashilAuthorization(ActionName = "PostEntity")]
        public async Task<IActionResult> ConfirmFactorGroup(FactorGroupSimpleViewModel factorGroupSimpleViewModel)
        {
            // ابتدا معتبر بودن گروه بندی فاکتور را بررسی می کنیم
            var checkIsValidFactorGroup = await CheckIsValidFactorGroup(factorGroupSimpleViewModel.Id);
            //  بررسی فاکتور دارای خطا باشد از ثبت نهایی گروه بندی فاکتور جلوگیری می کنیم
            if (!checkIsValidFactorGroup.IsValid)
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = checkIsValidFactorGroup.Message
                });
            }

            var result = await _factorGroupService.ConfirmFactorGroup(factorGroupSimpleViewModel.Id, true);
            if (result.isSuccessful)
            {
                await _factorGroupService.AddFactorGroupItemAsync(factorGroupSimpleViewModel.Id);
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "گروه بندی فاکتور با موفقیت ثبت نهایی شد"
                });
            }

            return Accepted(new ResultStatus<int>
            {
                IsSuccess = false,
                Message = result.message
            });
        }
        /// <summary>
        /// دریافت گروه های فاکتوری که میتوانند بعنوان گروه پدر انتخاب شوند
        /// گروه هایی میتوانند بعنوان گروه پدر انتخاب شودند که اولا فاکتوری در خود نداشته باشند و ثانیا در حالت اولیه باشند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetForSelectParent")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetForSelectParent(CustomDataSourceLoadOptions loadOptions)
        {
            var factorGroups = _factorGroupService.GetForSelectParent();
            return await DataSourceLoader.LoadAsync(
                factorGroups.ProjectTo<FactorGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// ارزیابی قبل از درج گروه فاکتور
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeInsertAsync(FactorGroupEditModel editModel)
        {
            if (editModel.ParentId == null)
            {
                return await base.CustomValidateBeforeInsertAsync(editModel);
            }
            var parentGroup = await _factorGroupService.GetAsync(editModel.ParentId, true);
            if (parentGroup != null)
            {
                if (_factorGroupService.CheckFactorGroupIsParent(editModel.ParentId.Value))
                {
                    return await base.CustomValidateBeforeInsertAsync(editModel);
                }
                return "کاربر گرامی گروه انتخاب شده بعنوان پدر دارای فاکتور میباشد و نمیتواند بعنوان گروه پدر انتخاب شود";
            }
            return "کاربر گرامی گروه پدر انتخاب شده صحیح نیست";
        }
        /// <summary>
        /// ارزیابی قبل از بروزرسانی گروه فاکتور
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeUpdateAsync(FactorGroupEditModel editModel)
        {
            var factorGroup = _factorGroupService.Get(editModel.Id, true);
            if (factorGroup.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Confirmed)
            {
                return "کاربر گرامی این گروه نهایی شده است و امکان ویرایش ندارد";
            }
            if (editModel.ParentId.HasValue && editModel.Id == editModel.ParentId.Value)
            {
                return "کاربر گرامی یک گروه نمیتواند پدر خود باشد";
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
        /// <summary>
        /// ارزیابی قبل از حذف فاکتور
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var factorGroup = await _factorGroupService.GetAsync(id, false);
            if (factorGroup.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Confirmed)
            {
                return "کاربر گرامی این گروه ثبت نهایی شده است و امکان ویرایش یا بروزرسانی ندارد";
            }

            if (_factorGroupService.CheckFactorGroupIsParent(factorGroup))
            {
                if (!_factorGroupService.CheckParentHasAnyFactorGroup(factorGroup))
                {
                    return await base.CustomValidateBeforeDeleteAsync(id);
                }
                return "کاربر گرامی این گروه نمیتواند حذف شود.";
            }
            return await base.CustomValidateBeforeDeleteAsync(id);
        }
        /// <summary>
        /// برای دریافت گزارش کانورت
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GetFactorGroupConvert")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<LoadResult> GetFactorGroupConvert(CustomDataSourceLoadOptions loadOptions, int factorGroupId)
        {
            var factorGroupConvert = await _factorGroupService.GetFactorGroupConvert(factorGroupId).ToListAsync();
            return DataSourceLoader.Load(factorGroupConvert, loadOptions);
        }
        /// <summary>
        /// ایجاد گزارش کانورت گروه انتخاب شده
        /// این متد برای گروه فاکتور تایید نهایی شده صدا زده میشود
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GenerateFactorGroupConvertFile")]
        [YashilAuthorization(ActionName = "ExportExcel")]
        public async Task<FileResult> GenerateFactorGroupConvertFile(int factorGroupId)
        {
            var factorGroup = _factorGroupService.GetFactorGroupIfConfirm(factorGroupId, true);
            if (factorGroup != null)
            {
                var factorGroupReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorGroupConvertReportId);

                ReportStoreWithParam reportStoreWithParam = new ReportStoreWithParam
                {
                    Params = new Dictionary<string, string> { { "factorGroupId", factorGroupId.ToString() } }
                };

                var reportFile = await _reportStoreService.GetReportFileAsync(factorGroupReportId, StiExportFormat.Excel2007, reportStoreWithParam);
                if (reportFile != null)
                {
                    _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(FactorGroup), "GenerateFactorGroupConvertFile Record", null);

                    return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{factorGroup.Title}.xlsx");
                }
            }
            return null;
        }
        /// <summary>
        /// ایجاد گزارش کانورت گروه انتخاب شده
        /// این متد برای گروه فاکتور تایید نهایی شده صدا زده میشود
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GenerateFactorGroupConvertFullReportFile")]
        [YashilAuthorization(ActionName = "ExportExcel")]
        public async Task<FileResult> GenerateFactorGroupConvertFullReportFile(int factorGroupId)
        {
            var factorGroup = _factorGroupService.GetFactorGroupIfConfirm(factorGroupId, true);
            if (factorGroup != null)
            {
                var reportStoreWithParam = new ReportStoreWithParam
                {
                    Params = new Dictionary<string, string> { { "factorGroupId", factorGroupId.ToString() } }
                };
                var factorGroupReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorGroupConvertReportId);

                var factorReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorGroupItemReportId);

                var deletedFactoredOrderItemReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.DeletedFactoredOrderItemReportId);

                var factorGroupFactorItemReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorGroupFactorItemReportId);

                var factorGroupFactorPaymentReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorGroupFactorPaymentId);

                var reportFile = await _reportStoreService.GetReportFileAsync(new[] { factorGroupReportId, factorReportId, factorGroupFactorItemReportId, factorGroupFactorPaymentReportId, deletedFactoredOrderItemReportId }, StiExportFormat.Excel2007, reportStoreWithParam);
                if (reportFile != null)
                {
                    _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(FactorGroup), "GenerateFactorGroupConvertFile Record", null);

                    return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"گزارش کانورت جامع مالی {factorGroup.Title}.xlsx");
                }
            }
            return null;
        }
        /// <summary>
        /// دریافت گزارش آیتمهای گروه، گروه بندی فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GenerateFactorGroupItemReportFile")]
        [YashilAuthorization(ActionName = "ExportExcel")]
        public async Task<FileResult> GenerateFactorGroupItemReportFile(int factorGroupId)
        {
            var factorGroup = _factorGroupService.GetFactorGroupIfConfirm(factorGroupId, true);
            if (factorGroup != null)
            {
                var factorReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorGroupItemReportId);
                var reportStoreWithParam = new ReportStoreWithParam
                {
                    Params = new Dictionary<string, string> { { "factorGroupId", factorGroupId.ToString() } }
                };

                var reportFile = await _reportStoreService.GetReportFileAsync(factorReportId, StiExportFormat.Excel2007, reportStoreWithParam);
                if (reportFile != null)
                {
                    _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(FactorGroup), "GenerateFactorGroupItemReportFile Record", null);

                    return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }
            }
            return null;
        }

        /// <summary>
        /// جهت دریافت گزارش ریز آیتم های فاکتورهای گروه بندی شده یک گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GenerateFactorGroupFactorItemReportFile")]
        [YashilAuthorization(ActionName = "ExportExcel")]
        public async Task<FileResult> GenerateFactorGroupFactorItemReportFile(int factorGroupId)
        {
            var factorGroup = _factorGroupService.GetFactorGroupIfConfirm(factorGroupId, true);
            if (factorGroup != null)
            {
                var factorItemReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorGroupFactorItemReportId);
                var reportStoreWithParam = new ReportStoreWithParam
                {
                    Params = new Dictionary<string, string> { { "factorGroupId", factorGroupId.ToString() } }
                };

                var reportFile = await _reportStoreService.GetReportFileAsync(factorItemReportId, StiExportFormat.Excel2007, reportStoreWithParam);
                if (reportFile != null)
                {
                    _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(FactorGroup), "GenerateFactorGroupItemReportFile Record", null);

                    return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{factorGroup.Title}.xlsx");
                }
            }
            return null;
        }

        /// <summary>
        /// جهت دریافت گزارش ریز پرداختی های آیتم های فاکتور فاکتورهای گروه بندی شده یک گروه فاکتور
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("GenerateFactorGroupFactorItemPaymentReportFile")]
        [YashilAuthorization(ActionName = "ExportExcel")]
        public async Task<FileResult> GenerateFactorGroupFactorItemPaymentReportFile(int factorGroupId)
        {
            var factorGroup = _factorGroupService.GetFactorGroupIfConfirm(factorGroupId, true);
            if (factorGroup != null)
            {
                var factorItemPaymentReportId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceFactorGroupFactorPaymentId);
                var reportStoreWithParam = new ReportStoreWithParam
                {
                    Params = new Dictionary<string, string> { { "factorGroupId", factorGroupId.ToString() } }
                };

                var reportFile = await _reportStoreService.GetReportFileAsync(factorItemPaymentReportId, StiExportFormat.Excel2007, reportStoreWithParam);
                if (reportFile != null)
                {
                    _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(FactorGroup), "GenerateFactorGroupConvertFile Record", null);

                    return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{factorGroup.Title}.xlsx");
                }
            }
            return null;
        }


        /// <summary>
        ///  بررسی میکنیم که برای گروه داده شده داده های مجاز وارد شده است یا خیر
        /// یعنی باید نام کالا، کد کالا و واحد سنجش مقدار داشته باشند.
        /// CheckIsValidTaxCategoryOrTaxDataForConvertFactorGroup
        /// </summary>
        /// <param name="factorGroupId"></param>
        /// <returns></returns>
        [HttpGet("CheckIsValidFactorGroup")]
        [YashilAuthorization(ActionName = "GetForSelect")]
        public async Task<FactorGroupValidateResultParam> CheckIsValidFactorGroup(int factorGroupId)
        {

            var factorGroup = await _factorGroupService.GetAsync(factorGroupId, true);
            if (factorGroup == null)
            {
                _logger.InsertLog(LogLevel.Error, new EventId((int)Events.GetEntityException, Events.GetEntityException.ToString()), nameof(FactorGroup), $"No FactorGroup Found for id  {factorGroupId} Error", null);
                throw new Exception($"{Events.GetEntityException} : {nameof(FactorGroup)}");
            }

            var checkIsValidTaxCategoryOrTaxDataForConvertFactorGroup =
                _factorGroupService.CheckIsValidTaxCategoryOrTaxDataForConvertFactorGroup(factorGroup.Id);
            if (!checkIsValidTaxCategoryOrTaxDataForConvertFactorGroup)
            {
                return new FactorGroupValidateResultParam
                {
                    IsValid = false,
                    Message = "کاربر گرامی ممکن است یک یا بیش از یکی از مقادیر نام کالا، کد کالا و یا واحد سنجش مقدار نداشته باشند، لطفا مقادیر مشخص شده را اصلاح کنید "
                };

            }
            // پرداخت هایی که تایید شده اند اما در سهم پرداخت آیتم سفارش وجود ندارند را جدول سهم پرداخت آیتم سفارش درج می کند 
            await _factorGroupService.FixMissedOrderItemPriceRightShareAsync();
            // بررسی می کند که آیا تسهیم پرداختی برای آیتم سفارش به درستی انجام شد است یا خیر ؟
            var isValidOrderItemPriceRightShare = await _factorGroupService.checkIsValidOrderItemPriceRightShareAsync();
            if (!isValidOrderItemPriceRightShare)
            {
                return new FactorGroupValidateResultParam
                {
                    IsValid = false,
                    Message = "کاربر گرامی، تسهیم پرداخت های آیتم سفارش به درستی انجام نشده است. امکان ثبت نهایی گروه بندی فاکتور وجود ندارد ."
                };
            }
            return new FactorGroupValidateResultParam
            {
                IsValid = true
            };
        }

        /// <summary>
        /// تغییر وضعیت گروه فاکتور به وضعیت ثبت اولیه :
        /// در صورتی که کاربر بخواهد فاکتوری را از گروه فاکتور هایی که ثبت نهایی شده اند
        /// حذف یا اضافه کند باید با این متد گروه فاکتور را به وضعیت ثبت اولیه تغییر دهد
        /// و با استفاده از فرم، فاکتور های گروه را کم و زیاد کند
        /// </summary>
        /// <param name="factorGroupSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("ChangeFactorGroupStateToInitial")]
        [YashilAuthorization(ActionName = "ChangeFactorGroupStateToInitial")]
        public async Task<IActionResult> ChangeFactorGroupStateToInitial(FactorGroupSimpleViewModel factorGroupSimpleViewModel)
        {
            var changedFactorGroup = await _factorGroupService
                .ChangeFactorGroupStateToInitialAsync(factorGroupSimpleViewModel.Id);
            if (!changedFactorGroup.IsSucces)
            {

                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = changedFactorGroup.Message
                });
            }
            await _activityLogService.AddActivityLogAsync(
                _userPrincipal.Id,
                ControllerContext.ActionDescriptor.ControllerName
                    .Replace("Controller", ""),
                ControllerContext.ActionDescriptor.ActionName,
                factorGroupSimpleViewModel.Id,
                string.Empty,
                "تغییر وضعیت گروه فاکتور از ثبت نهایی به ثبت اولیه"
            );
            return Accepted(new ResultStatus<int>
            {
                IsSuccess = true,
                Message = "کاربر گرامی, تغییر وضعیت گروه فاکتور به ثبت اولیه با موفقیت انجام شد."
            });
        }


    }
}