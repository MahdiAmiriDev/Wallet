using System;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using System.Collections.Generic;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class RefundDetailController : BaseController<RefundDetail, int, RefundDetailListViewModel,
        RefundDetailEditModel, RefundDetailSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRefundDetailService _refundDetailService;
        private readonly IYashilLogger<RefundDetail> _logger;
        private readonly IOrderService _orderService;


        public RefundDetailController(IRefundDetailService refundDetailService,
            IMapper mapper,
            IYashilLogger<RefundDetail> logger,
            IPermissionService permissionService, IOrderService orderService) : base(refundDetailService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _refundDetailService = refundDetailService;
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? refundType,
            int? refundStatusType, int? refundId, int? refundGroupId, int? paymentId, int? personBankAccountId, int orderId)
        {
            var userHasAccessForOrder = await _orderService.CheckUserHasAccessForOrder(orderId);
            if (!userHasAccessForOrder)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "GetByCustomFilterForList"),
                    nameof(Factor), "GetByCustomFilterForList Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Factor)}");
            }

            var refundDetails = _refundDetailService.GetByCustomFilter(refundType, refundStatusType, refundId,
                refundGroupId, paymentId, personBankAccountId, orderId);
            return await DataSourceLoader.LoadAsync(refundDetails.ProjectTo<RefundDetailListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        protected override ModifyPropertiesDescriptor GetPropertiesForApplyOrIgnoreUpdate(RefundDetail entity,
            RefundDetailEditModel editModel)
        {
            var modifyPropertiesDescriptor = new ModifyPropertiesDescriptor();
            modifyPropertiesDescriptor.Properties.Add(nameof(RefundDetail.RefundAmount));
            modifyPropertiesDescriptor.UpdateSelectedFieldsOrIgnoreTheme = true;
            return base.GetPropertiesForApplyOrIgnoreUpdate(entity, editModel);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(RefundDetailEditModel editModel)
        {
            bool allowEdit = _refundDetailService.AllowEdit(editModel.Id);
            if (!allowEdit)
            {
                return "کاربر گرامی رکورد عودت وجه انتخابی قابل ویرایش نمی باشد";
            }

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        /// <summary>
        /// جزییات عودت وجه گروه بندی شده حکمت و آنلاین را بر میگرداند
        /// archiveMode 1 آرشیوشده
        /// archiveMode 2 آرشیو نشده
        /// archiveMode 3 همه موارد
        /// </summary>
        /// <param name="archiveMode"></param>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetGroupedItems")]
        [YashilAuthorization()]
        public async Task<LoadResult> GetGroupedItems(int? archiveMode, CustomDataSourceLoadOptions loadOptions)
        {
            loadOptions.SortByPrimaryKey = false;
            var refundDetails = _refundDetailService.GetGroupedItems(archiveMode, true);
            return await DataSourceLoader.LoadAsync(
                refundDetails.ProjectTo<RefundDetailCustomListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        /// <summary>
        /// دریافت همه عودت وجه های گروه بندی شده
        /// جزییات عودت وجه گروه بندی شده حکمت و آنلاین را بر میگرداند
        /// archiveMode 1 آرشیوشده
        /// archiveMode 2 آرشیو نشده
        /// archiveMode 3 همه موارد
        /// </summary>
        /// <param name="archiveMode"></param>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetAllGroupedItems")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetAllGroupedItems(int? archiveMode, CustomDataSourceLoadOptions loadOptions)
        {
            loadOptions.SortByPrimaryKey = false;
            var refundDetails = _refundDetailService.GetAllGroupedItems(archiveMode, true);
            return await DataSourceLoader.LoadAsync(
                refundDetails.ProjectTo<RefundDetailCustomListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpPost("SetRefundDetailPersonBankAccount")]
        [YashilAuthorization()]
        public async Task<ResultStatus<int>> SetRefundDetailPersonBankAccount(
            RefundDetailPersonBankAccountEditModel refundDetailPersonBankAccountEditModel)
        {
            var refundDetail =
                await _refundDetailService.GetAsync(refundDetailPersonBankAccountEditModel.RefundDetailId, false);
            refundDetail.PersonBankAccountId = refundDetailPersonBankAccountEditModel.PersonBankAccountId;
            await _refundDetailService.UpdateAsync(refundDetail, refundDetail.Id, true);
            return new ResultStatus<int>(true, "اطلاعات حساب انتخاب شده با موفقیت برای سفارش عودت داده شده ثبت شد");
        }
        /// <summary>
        /// اطلاعات حساب را از رکورد عودت وجه  پاک می کند
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <returns></returns>
        [HttpPost("RemovePersonBankAccountFromRefundDetail")]
        [YashilAuthorization]
        public async Task<ResultStatus<int>> RemovePersonBankAccountFromRefundDetail(int refundDetailId)
        {
            (bool isSuccess, string message) = await _refundDetailService
                 .RemovePersonBankAccountFromRefundDetailAsync(refundDetailId);

            return new ResultStatus<int>(isSuccess, message);

        }
        /// <summary>
        /// جهت دریافت اطلاعات مربوط به آیتمی که میخواهد تایید و یا رد شود.
        /// این متد هنگامی صدا زده میشود که دکمه تایید و بررسی یک آیتم عودت وجه حکمت زده شود سپس یک پاپ اپ باز میشود که اطلاعاتی نمایش می دهد اطلاعات از ایسن متد گرفته مکیشود
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <returns></returns>

        [HttpGet("GetRefundDetailStatistic")]
        [YashilAuthorization(ActionName = "GetEntity")]
        public async Task<RefundDetailStatisticViewModel> GetRefundDetailStatistic(int refundDetailId)
        {

            var refundDetailStatistic = _refundDetailService.GetRefundDetailStatistic(refundDetailId);
            return await refundDetailStatistic.ProjectTo<RefundDetailStatisticViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// جهت تایید آیتم گروه بندی شده
        /// </summary>
        /// <param name="refundDetailConfirmViewModel"></param>
        /// <returns></returns>
        [HttpPost("ConfirmRefundDetail")]
        [YashilAuthorization()]
        public async Task<ResultStatus<int>> ConfirmRefundDetailAsync(RefundDetailConfirmViewModel refundDetailConfirmViewModel)
        {
            var resultMessage = await _refundDetailService.ConfirmRefundDetailAsync(refundDetailConfirmViewModel.Id,
                refundDetailConfirmViewModel.AnnouncedRefundAmount);
            if (string.IsNullOrEmpty(resultMessage))
            {
                return new ResultStatus<int>(true, "");
            }
            return new ResultStatus<int>(false, resultMessage);
        }

        /// <summary>
        /// جهت رد آیتم گروه بندی شده
        /// </summary>
        /// <param name="refundDetailSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("RejectRefundDetail")]
        [YashilAuthorization()]
        public async Task<ResultStatus<int>> RejectRefundDetail(RefundDetailSimpleViewModel refundDetailSimpleViewModel)
        {
            var resultMessage = await _refundDetailService.RejectRefundDetailAsync(refundDetailSimpleViewModel.Id);
            if (string.IsNullOrEmpty(resultMessage))
            {
                return new ResultStatus<int>(true, "");
            }
            return new ResultStatus<int>(false, resultMessage);
        }

        /// <summary>
        /// در فرم گروه بندی عودت وجه کاربرد دارد
        ///  بر اساس مبلغ و شماره کارت
        /// لیست تراکنش های غودت وجه حکمت که با این شماره کارت و مبلغ همخوانی دارند را نشان میدهد
        /// عودت وجه ها از جدول های
        /// HekmatCanceledTransaction
        /// HekmatEffectlessCanceledTransaction
        /// HekmatEffectlessModifiedTransaction
        /// HekmatModifiedTransaction
        /// میخواند و برمیگرداند
        /// </summary>
        /// <param name="cardNumber">شماره کارت حکمت</param>
        /// <param name="price">قیمت یا مبلغ</param>
        /// <param name="loadOptions">پارامتر های فیلتر</param>
        /// <returns></returns>
        [HttpGet("GetHekmatCanceledOrModifiedTransactions")]
        [YashilAuthorization(ActionName = "GetHekmatCanceledOrModifiedTransactionsAsync")]
        public async Task<LoadResult> GetHekmatCanceledOrModifiedTransactionsAsync(string cardNumber, long price,
            CustomDataSourceLoadOptions loadOptions)
        {
            var hekmatCanceledOrModifiedTransaction = _refundDetailService.GetHekmatCanceledOrModifiedTransactionsAsync(cardNumber, price);
            return await DataSourceLoader.LoadAsync(
                hekmatCanceledOrModifiedTransaction.ProjectTo<HekmatCanceledOrModifiedTransactionViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// رکورد عودت وجه در صورتی که تعیین تکلیف نشده باشد یعنی 
        /// بعد از بارگذاری سی دی تراکنش های عودتی، مبلغ عودت به کاربر آن مقدار نگیرد
        ///  .و کارشناس مربوطه نیز مقدار عودتی به کاربر را از سی دی پیدا نکند تا از طریق فرم وارد کند،
        ///  مجددا این رکورد را در عودتی های ماه جدید به حکمت ارسال میکند
        /// </summary>
        /// <param name="refundDetailSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("SendRefundDetailToNewGroup")]
        [YashilAuthorization()]
        public async Task<ResultStatus<int>> SendRefundDetailToNewGroup(RefundDetailSimpleViewModel refundDetailSimpleViewModel)
        {
            var refundDetail = _refundDetailService.Get(refundDetailSimpleViewModel.Id, true);
            if (refundDetail.RefundAmount.HasValue)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی رکورد عودت وجه انتخابی مقدار عودتی به کاربر دارد، شما نمی توانید این رکورد را به ماه جدید ارسال کنید"
                };
            }
            var resultMessage = await _refundDetailService.SendRefundDetailToNewGroupAsync(refundDetailSimpleViewModel.Id);
            if (string.IsNullOrEmpty(resultMessage))
            {
                return new ResultStatus<int>(false, "خطا در ارسال رکورد عودتی به ماه جدید");
            }
            return new ResultStatus<int>(true, resultMessage);
        }


        /// <summary>
        /// جهت باگانی عودت وجه هنگامی بایگانی صورت می گیرد
        /// این متد در فرم دریافت اطلاعات حساب شخص جهت عودت مورد استفاده قرار می گیرد
        /// </summary>
        /// <param name="refundDetailSimpleViewModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("ArchiveRefundDetail")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<ResultStatus<int>> ArchiveRefundDetailAsync(
            RefundDetailSimpleViewModel refundDetailSimpleViewModel)
        {
            // بایگانی کردن
            var resultMessage = await _refundDetailService.ArchiveRefundDetailAsync(refundDetailSimpleViewModel.Id,
                refundDetailSimpleViewModel.ArchiveDescription);
            if (string.IsNullOrEmpty(resultMessage))
            {
                return new ResultStatus<int>(true, "کاربر گرامی رکورد عودت وجه با موفقیت بایگانی شد");
            }
            return new ResultStatus<int>(false, resultMessage);
        }
        /// <summary>
        /// خارج کردن  رکورد عودت وجه از بایگانی
        /// این متد در منو ثبت اطلاعات حساب جهت عودت استفاده میشود.
        /// </summary>
        /// <param name="refundDetailSimpleViewModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("UnArchiveRefundDetail")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<ResultStatus<int>> UnArchiveRefundDetailAsync(
            RefundDetailSimpleViewModel refundDetailSimpleViewModel)
        {
            // بایگانی کردن
            var resultMessage = await _refundDetailService.UnArchiveRefundDetailAsync(refundDetailSimpleViewModel.Id);
            if (string.IsNullOrEmpty(resultMessage))
            {
                return new ResultStatus<int>(true, "کاربر گرامی اطلاعات با موفقیت از بایگانی حذف شد");
            }
            return new ResultStatus<int>(false, resultMessage);

        }
        /// <summary>
        /// جهت انتصاب یک عودت وجه به یک کاربر دارای نقش crm
        /// این متد در فرم انتصاب عودت وجه به کاربر مورد استفاده قرار میگیرد.
        /// </summary>
        /// <param name="refundDetailEditModel"></param>
        /// <returns></returns>
        [HttpPost("AssignRefundDetail")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<ResultStatus<int>> AssignRefundDetailAsync(
            RefundDetailEditModel refundDetailEditModel)
        {
            if (refundDetailEditModel.Assignee == null)
            {
                return new ResultStatus<int>(false, "کاربر گرامی برای انتصاب لطفا یک نفر را انتخاب کنید");
            }
            var resultMsg = await _refundDetailService.AssignRefundDetailAsync(refundDetailEditModel.Id, refundDetailEditModel.Assignee);
            if (string.IsNullOrEmpty(resultMsg))
            {
                return new ResultStatus<int>(true, "کاربر گرامی بروزرسانی با موفقیت انجام شد");
            }
            return new ResultStatus<int>(false, resultMsg);

        }

        /// <summary>
        /// اطلاعات کامل عودت وجه ها را بر اساس فیلتر ارسالی پارامتر ها نمایش می دهد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="showGrouped">نمایش گروه بندی شده</param>
        /// <param name="showPaymentGrouped">نمایش گروه بندی برای پرداخت شده</param>
        /// <param name="showHasBankAccount">نمایش دارای اطلاعات حساب</param>
        /// <param name="refundCreationDateFromDate">تاریخ شروع ثبت عودت</param>
        /// <param name="refundCreationDateToDate">تاریخ پایان ثبت عودت</param>
        /// <returns></returns>
        [HttpGet("GetRefundDetailFullInfo")]
        [YashilAuthorization(ActionName = "RefundDetailFullInfo")]
        public async Task<LoadResult> GetRefundDetailFullInfo(CustomDataSourceLoadOptions loadOptions,
            int refundCreationDateFromDate, int refundCreationDateToDate,
            bool? showGrouped,
            bool? showPaymentGrouped, bool? showHasBankAccount)
        {
            var refundDetails = _refundDetailService.GetRefundDetailFullInfo(refundCreationDateFromDate, refundCreationDateToDate,
                showGrouped, showPaymentGrouped, showHasBankAccount);
            return await DataSourceLoader.LoadAsync(refundDetails.ProjectTo<RefundDetailFullInfoListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// نام کاربرانی که عودت وجه به انها تخصیص داده و تعداد عودت اختصاص داده شده  را بر می گرداند
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAssignedUsers")]
        [YashilAuthorization()]
        public List<RefundAssignedUserListViewModel> GetAssignedUsers()
        {
            var assignedUsers = _refundDetailService.GetAssignedUsers();
            return _mapper.Map<List<RefundAssignedUserListViewModel>>(assignedUsers);
        }

    }
}