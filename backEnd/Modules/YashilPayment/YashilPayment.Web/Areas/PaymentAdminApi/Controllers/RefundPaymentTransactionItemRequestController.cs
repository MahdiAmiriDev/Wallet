using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minio.Exceptions;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilPayment.Core.DTO;
using YashilUserManagement.Core.Services;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class RefundPaymentTransactionItemRequestController : BaseController<RefundPaymentTransactionItemRequest, int, RefundPaymentTransactionItemRequestListViewModel, RefundPaymentTransactionItemRequestEditModel, RefundPaymentTransactionItemRequestSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRefundPaymentTransactionItemRequestService _refundPaymentTransactionItemRequestService;
        private readonly IYashilLogger<RefundPaymentTransactionItemRequest> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IUserRoleService _UserRoleService;
        private readonly IYashilHekmatTerminalSupplierService _yashilHekmatTerminalSupplierService;
        private readonly IUserPrincipal _userPrincipal;

        public RefundPaymentTransactionItemRequestController(IRefundPaymentTransactionItemRequestService refundPaymentTransactionItemRequestService,
        IMapper mapper,
        IYashilLogger<RefundPaymentTransactionItemRequest> logger,
        IPermissionService permissionService, IUserRoleService userRoleService, IYashilHekmatTerminalSupplierService yashilHekmatTerminalSupplierService, IUserPrincipal userPrincipal) : base(refundPaymentTransactionItemRequestService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _refundPaymentTransactionItemRequestService = refundPaymentTransactionItemRequestService;
            _logger = logger;
            _permissionService = permissionService;
            _UserRoleService = userRoleService;
            _yashilHekmatTerminalSupplierService = yashilHekmatTerminalSupplierService;
            _userPrincipal = userPrincipal;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId, int? yashilHekmatTerminalId, int? refundPaymentTransactionItemRequestType, int? refundPaymentTransactionItemRequestStatus)
        {
            var refundPaymentTransactionItemRequests = _refundPaymentTransactionItemRequestService.GetByCustomFilter(parentId, yashilHekmatTerminalId, refundPaymentTransactionItemRequestType, refundPaymentTransactionItemRequestStatus);
            return await DataSourceLoader.LoadAsync(refundPaymentTransactionItemRequests.ProjectTo<RefundPaymentTransactionItemRequestListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId, int? yashilHekmatTerminalId, int? refundPaymentTransactionItemRequestType, int? refundPaymentTransactionItemRequestStatus)
        {
            var refundPaymentTransactionItemRequests = _refundPaymentTransactionItemRequestService.GetByCustomFilter(parentId, yashilHekmatTerminalId, refundPaymentTransactionItemRequestType, refundPaymentTransactionItemRequestStatus);
            return await DataSourceLoader.LoadAsync(refundPaymentTransactionItemRequests.ProjectTo<RefundPaymentTransactionItemRequestSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// ردیافت لیست تراکنش های موفق سامانه ی ایزی بوک
        /// </summary>
        /// <param name="supplierId">شناسه فروشنده در سامانه ایزی بوک</param>
        /// <param name="startDateInt"></param>
        /// <param name="endDateInt"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="AccessDeniedException"></exception>
        [HttpGet("GetIsiBookCompleteOrders")]
        [YashilAuthorization("GetIsiBookCompleteOrders")]
        public async Task<LoadResult> GetIsiBookCompleteOrdersAsync(int supplierId, int? startDateInt, int? endDateInt)
        {
            if (supplierId == 0)
                throw new ArgumentException(nameof(supplierId));

            await CheckCurrentUserHasAccessToHekmatTerminalSupplierAsync(supplierId);

            if (!startDateInt.HasValue)
            {
                var dateTimeNow = DateTime.Now;

                startDateInt = DateFunction.ToShamsiInt(dateTimeNow.Year, dateTimeNow.Month - 1, dateTimeNow.Day);
            }

            endDateInt ??= DateFunction.TodayShamsiInt();

            return new LoadResult()
            {
                data = await _refundPaymentTransactionItemRequestService
                    .GetIsiBookCompleteOrder(supplierId, startDateInt.Value, endDateInt.Value)
                    .ToListAsync()
            };

        }

        /// <summary>
        /// این متد جهت درج درخواست جدید برای لغو یک سفارش کاربرد دارد
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [HttpPost("AddRefundPaymentTransactionItemRequest")]
        [YashilAuthorization("AddRefundPaymentTransactionItemRequest")]
        public async Task<ResultStatus<int>> AddRefundPaymentTransactionItemRequestAsync(AddRefundPaymentTransactionItemRequestDto dto)
        {
            if (dto.SupplierId == 0 || dto.TerminalId == 0 || string.IsNullOrEmpty(dto.ReservationNumber))
                throw new ArgumentException(nameof(dto.SupplierId) + " - " + nameof(dto.TerminalId) +
                                            " - " + nameof(dto.ReservationNumber));
            try
            {
                await CheckCurrentUserHasAccessToHekmatTerminalSupplierAsync(dto.SupplierId, dto.TerminalId);

                if (await _refundPaymentTransactionItemRequestService
                        .HasUserInsertRecordBeforeFroPaymentTransactionAsync(dto.TerminalId, dto.ReservationNumber))
                    return new ResultStatus<int>(false, "کاربر گرامی درخواست لغو برای این سفارش ثبت شده است.");

                await _refundPaymentTransactionItemRequestService.AddRefundPaymentTransactionItemRequestAsync(dto);

                await _refundPaymentTransactionItemRequestService.SaveChangeAsync();

                return new ResultStatus<int>(true, "عملیات با موفقیت انجام شد.");

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, 
                    nameof(AddRefundPaymentTransactionItemRequestAsync),
                    "بروز خطا به هنگام درج : RefundPaymentTransactionItemRequest", e);

                return new ResultStatus<int>(false, "بروز خطا به هنگام انجام عملیات.");
            }

        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر به
        /// HekmatTerminalSupplier
        /// درخواستی دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="yashilHekmatTerminalId"></param>
        /// <returns></returns>
        /// <exception cref="AccessDeniedException"></exception>
        [NonAction]
        public async Task CheckCurrentUserHasAccessToHekmatTerminalSupplierAsync(int supplierId, int? yashilHekmatTerminalId = null)
        {
            var userHasAccessToSupplier =
                await _yashilHekmatTerminalSupplierService.CheckUserHasAccessToHekmatTerminalSupplierAsync(_userPrincipal.Id,
                    supplierId, yashilHekmatTerminalId);

            if (!userHasAccessToSupplier)
            {
                var isAdmin = await _UserRoleService.IsAdminInApplicationAsync(_userPrincipal.Id, _userPrincipal.ApplicationId);

                if (!isAdmin)
                    throw new AccessDeniedException("access denied for supplierId");
            }
        }
       
        
        [HttpGet("GetByCustomFilterForActions")]
        [YashilAuthorization(ActionName = "GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForActions(CustomDataSourceLoadOptions loadOptions)
        {
            var paymentTransactionItemRefundRequests = _refundPaymentTransactionItemRequestService
                .GetByCustomFilter(null,null,null,null);
                
            return await DataSourceLoader.LoadAsync(paymentTransactionItemRefundRequests
                .ProjectTo<PaymentTransactionItemRefundRequestListForModifyActionViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        [HttpPost("RefundPaymentTransactionItemRequestAction")]
        [YashilAuthorization("PostEntity")]
        public async Task<IActionResult> PaymentTransactionItemRefundRequestActionAsync(PaymentTransactionItemRefundRequestSimpleEditModel editModel)
        {
            var result = await _refundPaymentTransactionItemRequestService
                .ApplyRefundPaymentTransactionItemRequestActionAsync(
                    editModel.PaymentTransactionItemRefundRequestId,
                    editModel.RejectRequest);

            if (result.isSucces)
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "عملیات با موفقیت انجام شد"
                   
                });
            }
            else
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = result.message
                });
            }
        }
    }
}

   