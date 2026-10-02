using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Yashil.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Core.Web;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilCommerce.Web.DataFactory;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.DTO;
using Yashil.Common.SharedKernel.Helpers;
using YashilCommerce.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(SessionFixationFilterAsync))]
    public class OrderController : YashilMvcBaseController<Order>
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IOrderDataFactory _orderDataFactory;
        private readonly IHekmatService _hekmatService;
        private readonly IOrderItemService _orderItemService;
        private readonly IMaktabKhoneApiService _maktabKhoneApiService;
        private readonly IHumaxService _humaxService;
        private readonly IYashilLogger<Order> _logger;


        private readonly IAdditionalUserPropService _additionalUserPropService;
        // GET: /<controller>/

        public OrderController(
                ApplicationConfiguration applicationConfiguration,
                IMapper mapper,
                IYashilLogger<Order> logger,
                IPermissionService permissionService,
                IOrderService orderService,
                IUserPrincipal userPrincipal,
                IOrderDataFactory orderDataFactory,
                IHekmatService hekmatService,
                IOrderItemService orderItemService,
                IAdditionalUserPropService additionalUserPropService, IMaktabKhoneApiService maktabKhoneApiService, IHumaxService humaxService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _orderService = orderService;
            _applicationConfiguration = applicationConfiguration;
            _userPrincipal = userPrincipal;
            _orderService = orderService;
            _orderDataFactory = orderDataFactory;
            _hekmatService = hekmatService;
            _orderItemService = orderItemService;
            _additionalUserPropService = additionalUserPropService;
            _maktabKhoneApiService = maktabKhoneApiService;
            _humaxService = humaxService;
            _logger = logger;
        }
        public IActionResult ShowMessage(string message)
        {
            return ViewComponent("MessageScreen", new { message });
        }

        /// <summary>
        /// 1.	آخرین سفارش با وضعیت پرداخت "کامل" ثبت شده کاربر با هر وضعیتی.
        /// 2.	 سفارش هایی با وضعیت های زیر
        ///     o ثبت اولیه
        ///     o	"منتظر تایید تامین کننده" 
        ///     o	” در حال بررسی”  (قبل از پرداخت قیمت بروز می شود)
        ///     o*"پرداخت ناموفق"  (30 دقیقه مهلت پرداخت دارد)
        ///     o	"پرداخت ناقص"  (قبل از پرداخت قیمت بروز می شود)
        ///     o*"انصراف از پرداخت"  (30 دقیقه مهلت پرداخت دارد)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> CurrentOrderListAsync()
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }
            await _hekmatService.UpdateUserHekmatTypeOrderAndPaymentStatusAsync(_userPrincipal.Id);
            var currentOrders = _orderService.GetCurrentOrders(_applicationConfiguration.ApplicationId);
            var currentOrderItems = _orderService.GetCurrentOrderItems(_applicationConfiguration.ApplicationId);

            _orderItemService.AssignVoucherForOrderItems(_applicationConfiguration.ApplicationId);

            return await PrepareOrderViewModelAsync(currentOrders, currentOrderItems);
        }
        /// <summary>
        /// وضعیت پرداخت SendToPaymentGateway یا WaitingResponseFromPaymentGateway باشد
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> WaitingForResultOrderListAsync()
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }
            var waitingForResultOrder = _orderService.GetWaitingForResultOrders(_applicationConfiguration.ApplicationId);
            var waitingForResultOrderItems = _orderService.GetWaitingForResultOrderItems(_applicationConfiguration.ApplicationId);

            return await PrepareOrderViewModelAsync(waitingForResultOrder, waitingForResultOrderItems);
        }
        /// <summary>
        /// سفارش هایی که وضعیت کامل می باشد 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> CompletePayOrderListAsync()
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }
            var completePayOrder = _orderService.GetCompletePayOrders(_applicationConfiguration.ApplicationId);
            var completePayOrderItems = _orderItemService.GetCompletePayOrderItems(_applicationConfiguration.ApplicationId, true);
            return await PrepareOrderViewModelAsync(completePayOrder, completePayOrderItems);
        }
        /// <summary>
        /// سفارش هایی که وضعیت سفارش تکمیل شده و وضعیت پرداخت موفق دارند، توسط کاربر مرجوع می شود
        /// (کل مبلغ پرداختی عودت داده میشود)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> RefundOrderListAsync()
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }
            var refundOrPartialRefundOrders = _orderService.GetRefundOrders(_applicationConfiguration.ApplicationId);
            var refundOrPartialRefundOrderItems = _orderService.GetRefundOrderItems(_applicationConfiguration.ApplicationId);
            return await PrepareOrderViewModelAsync(refundOrPartialRefundOrders, refundOrPartialRefundOrderItems);
        }
        /// <summary>
        /// سفارش هایی که کاربر بعد از اتمام مدت منتظر پرداخت، پرداختی انجام نداده,
        /// توسط سیستم انصراف زده میشود  
        /// و یا سفارش هایی که در تب سفارشات جاری میباشند و خود کاربر دکمه انصراف سفارش را میزند
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> CanceledOrderListAsync()
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }
            var canceledOrder = _orderService.GetCanceledOrders(_applicationConfiguration.ApplicationId);
            var canceledOrderItems = _orderService.GetCanceledOrderItems(_applicationConfiguration.ApplicationId);
            return await PrepareOrderViewModelAsync(canceledOrder, canceledOrderItems);
        }
        /// <summary>
        /// سفارش هایی که وضعیت یارانه  آنها "ثبت اولیه" هستند
        /// ( یعنی “منتظر تایید” یا “عدم تایید” یارانه میباشند)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<ActionResult> WaitingForSubsidyOrderListAsync()
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }
            var waitingForSubsidyOrders = _orderService.GetWaitingForSubsidyOrders(_applicationConfiguration.ApplicationId);
            var waitingForSubsidyOrderItems = _orderService.GetWaitingForSubsidyOrderItems(_applicationConfiguration.ApplicationId);
            return await PrepareOrderViewModelAsync(waitingForSubsidyOrders, waitingForSubsidyOrderItems);
        }
        private async Task<ActionResult> PrepareOrderViewModelAsync(IQueryable<Order> orders, IQueryable<OrderItem> orderItems)
        {
            var editedOrderId = 0;
            var editedOrderIdStr = HttpContext.Request.Cookies[CookiKey.OrderId];
            if (!string.IsNullOrWhiteSpace(editedOrderIdStr))
            {
                int.TryParse(editedOrderIdStr, out editedOrderId);
            }

            var orderListPresentationViewModelList = new List<OrderListPresentationViewModel>();
            var orderSimplePresentationViewModelList = await orders.ProjectTo<OrderSimplePresentationViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            var orderItemSimplePresentationViewModelList = await orderItems.ProjectTo<OrderItemSimplePresentationViewModel>(_mapper.ConfigurationProvider).ToListAsync();

            foreach (var orderSimplePresentationViewModel in orderSimplePresentationViewModelList)
            {
                orderListPresentationViewModelList.Add(new OrderListPresentationViewModel
                {
                    OrderSimpleViewModel = orderSimplePresentationViewModel,
                    OrderItemSimplePresentationViewModels = orderItemSimplePresentationViewModelList.Where(x => x.OrderId == orderSimplePresentationViewModel.Id).ToList()

                });
                var allowOrderEdit = _orderService.CheckOrderAllowEdit(orderSimplePresentationViewModel.Id, orderSimplePresentationViewModel.LastOrderId, orderSimplePresentationViewModel.AllowEdit, (OrderStatusTypesEnum)orderSimplePresentationViewModel.OrderStatusType);
                if (allowOrderEdit && editedOrderId == orderSimplePresentationViewModel.Id)
                {
                    orderSimplePresentationViewModel.AllowCancelEdit = true;
                    allowOrderEdit = false;
                }
                orderSimplePresentationViewModel.AllowPay = _orderService.CheckOrderAllowPay((OrderStatusTypesEnum)orderSimplePresentationViewModel.OrderStatusType, (SubsidyStatusTypesEnum?)orderSimplePresentationViewModel.SubsidyStatusType);
                orderSimplePresentationViewModel.Editable = allowOrderEdit;
                orderSimplePresentationViewModel.Refundable = _orderService.CheckOrderAllowRefund((OrderStatusTypesEnum)orderSimplePresentationViewModel.OrderStatusType, orderSimplePresentationViewModel.AllowRefund);
                orderSimplePresentationViewModel.Cancelable = _orderService.CheckOrderAllowCancel(orderSimplePresentationViewModel.Id, (OrderStatusTypesEnum)orderSimplePresentationViewModel.OrderStatusType);
            }
            return View("OrderList", orderListPresentationViewModelList);
        }

        /// <summary>
        /// Get Order List 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var applicationConfigurationViewModel = new ApplicationConfigurationViewModel()
            {
                SubsidyBased = userAdditionalUserProp.SupportSubsidy,
                AddressBased = userAdditionalUserProp.AddressBased,
            };
            return View(applicationConfigurationViewModel);
        }

        #region Order Detail
        public async Task<ActionResult> Detail(int? orderId)
        {
            OrderSummaryViewModel orderSummary = await _orderDataFactory.FillOrderSummaryByOrderId(orderId);
            return View("Detail", orderSummary);
        }

        #endregion
        #region CheckOut

        /// <summary>
        /// فرم وضعیت سفارش قبل از پرداخت 
        /// </summary>
        /// <param name="recipientId"></param>
        /// <returns></returns>
        public async Task<ActionResult> CheckOut(int? recipientId)
        {
            return View(await _orderDataFactory.PrepareCheckoutViewModelByRecipientIdAsync(recipientId));
        }
        /// <summary>
        /// فرم وضعیت سفارش قبل از پرداخت 
        /// </summary>
        /// <param name="recipientId"></param>
        /// <returns></returns>

        public async Task<ActionResult> OrderSummery(string recipientId)
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }
            OrderSummaryViewModel orderSummaryViewModel = new OrderSummaryViewModel();
            if (!string.IsNullOrEmpty(recipientId))
            {
                var decryptedRecipientId = CryptographyHelper.Base64Decode(CryptographyHelper.AesDecrypt(recipientId));
                if (string.IsNullOrEmpty(decryptedRecipientId))
                {
                    return Accepted(string.Empty, "InvalidRecipientId");
                }
                if (int.TryParse(decryptedRecipientId, out var recipientIdInt))
                {
                    orderSummaryViewModel = await _orderDataFactory.FillOrderSummaryByRecipientId(recipientIdInt);
                    if (orderSummaryViewModel.OrderItemDetailPresentationViewModels == null || orderSummaryViewModel.OrderItemDetailPresentationViewModels.Count == 0)
                    {
                        return Accepted(string.Empty, "ShoppingCartItem");
                    }
                    // در صورتی که یارانه در وضعیت ثبت اولیه باشد کاربر نمی تواند ادامه خرید انجام بدهد 
                    if (orderSummaryViewModel.OrderSummaryForPayViewModel.SubsidyCanBeUsed
                        && orderSummaryViewModel.SubsidyStatusType == (int)SubsidyStatusTypesEnum.InitialRegistration)
                    {
                        return Accepted(string.Empty, "SubsidyStatusIsInitialRegistration");
                    }
                }


            }
            return View("_OrderSummary", orderSummaryViewModel);
        }
        public async Task<ActionResult> LastOrderStatus()
        {
            var order = _orderService.GetUserLastOrder(_userPrincipal.Id);
            var orderSimplePresentationViewModel = await order.ProjectTo<OrderSimplePresentationViewModel>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
            return View(orderSimplePresentationViewModel);
        }
        #endregion

        /// <summary>
        /// این متد برای زمانی که کاربر محصولی را خریداری کرده است
        /// که نیاز است برای دریافت خدمات به سایت خارج از سامانه هدایت شود
        /// کاربرد دارد.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        [Route("RedirectToCompanySiteForReceiveProduct")]
        public async Task<IActionResult> RedirectToCompanySiteForReceiveProduct(string key, int orderItemId)
        {
            if (string.IsNullOrEmpty(key) || orderItemId <= 0)
                return View("Error");

            switch (key)
            {
                case RedirectToCompanySiteKey.MaktabKhoneRedirectKey:

                    return await RedirectUserToMaktabKhoneSiteForReceiveProduct(orderItemId, key);

                default:
                    return View("Error");
            }
        }


        /// <summary>
        /// این متد برای هدایت کاربر به وبسایت مکتب خونه برای دریافت خدمات
        /// محصول کاربرد دارد
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <param name="companyApiKey"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<IActionResult> RedirectUserToMaktabKhoneSiteForReceiveProduct(int orderItemId, string companyApiKey)
        {
            var userHasAccessToOrderItem = await _orderService.CheckCurrentUserHasAccessToOrderItemAndPayCompletely(orderItemId, companyApiKey).AnyAsync();

            if (!userHasAccessToOrderItem)
                InsertUserHasNotAccessToOrderItemLogAndReturnErrorPage(orderItemId, companyApiKey);

            var orderItem = await _orderItemService.GetCurrentUserOrderItemAsync(orderItemId);

            if (!orderItem.IsSubscriptionWithApi.HasValue || !orderItem.IsSubscriptionWithApi.Value)
            {
                var addUserToSubscriptionResult = await _maktabKhoneApiService.AddUserToSubscriptionPlanAsync(_userPrincipal.UserName);

                if (!addUserToSubscriptionResult.isSuccess)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(Order),
                        $"بروز خطا به هنگام ثبت نام کاربر در سایت مکتب خونه {addUserToSubscriptionResult.message}", null);
                    return View("Error");
                }
                try
                {
                    orderItem.IsSubscriptionWithApi = true;
                    await _orderItemService.UpdateAsync(orderItem, orderItem.Id, true);
                }
                catch (Exception e)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.AccessDeniedGetEntity, nameof(Order),
                        $"بروز خطا به هنگام بروزرسانی مقدار IsSubscriptionWithApi برای شناسه سفارش {orderItemId}", e);

                    return View("Error");
                }

            }

            var getTokenResponse = await _maktabKhoneApiService.GetAuthenticationToken(_userPrincipal.UserName);

            if (!getTokenResponse.isSuccess)
            {

                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(Order),
                    $"بروز خطا به هنگام ورود به سایت مکتب خونه {getTokenResponse.message}", null);
                return View("Error");
            }
            return Redirect(getTokenResponse.redirectUrl);
        }

        /// <summary>
        /// این متد جهت ثبت نام کاربر و هدایت او به
        /// سرویس
        /// Humax
        /// کاربرد دارد
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <param name="companyApiKey"></param>
        /// <returns></returns>
        [HttpGet("DoRegisterAndRedirectToHumaxServiceForCurrentUser")]
        public async Task<IActionResult> DoRegisterAndRedirectToHumaxServiceForCurrentUserAsync(int orderItemId, string companyApiKey)
        {

            var userHasAccessToOrderItem = await _orderService.CheckCurrentUserHasAccessToOrderItemAndPayCompletely(orderItemId, companyApiKey).AnyAsync();

            if (!userHasAccessToOrderItem)
                InsertUserHasNotAccessToOrderItemLogAndReturnErrorPage(orderItemId, companyApiKey);

            HumaxRegisterUserResponseDto registerInHumaxServiceResult = new();

            string errorMessage = "بروز خطا به هنگام انجام عملیات ثبت نام و دریافت لینک ورود به سرویس آزمون استعداد یابی شغلی,";

            try
            {
                registerInHumaxServiceResult = await _humaxService.DoRegisterAndGetRedirectUrlToHumaxServiceForCurrentUserAsync();

                if (registerInHumaxServiceResult == null || !registerInHumaxServiceResult.Success)
                {
                    if (!string.IsNullOrEmpty(registerInHumaxServiceResult?.Message))
                        errorMessage = registerInHumaxServiceResult.Message;

                    InsertLog(null, errorMessage);

                    return View("Error");
                }

                return Redirect(registerInHumaxServiceResult.Url);
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(registerInHumaxServiceResult?.Message))
                    errorMessage = registerInHumaxServiceResult.Message;

                InsertLog(e, errorMessage);

                return View("Error");
            }
        }

        /// <summary>
        /// این متد جهت درج لاگ کاربرد دارد
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        [NonAction]
        private void InsertLog(Exception e, string message)
        {
            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(Order), message, e);
        }

        /// <summary>
        /// این متد برای مواقعی که کاربر به یک
        /// orderItemId
        /// دسترسی ندارد یعنی کاربر جاری خریدار این شماره سفارش نیست
        /// کاربرد دارد
        /// این متد لاگ مربوطه در دیتابیس ثبت می کند و کاربر را به صفحه ی
        /// Error
        /// هدایت می کند.
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <param name="companyApiKey"></param>
        /// <returns></returns>
        [NonAction]
        private ViewResult InsertUserHasNotAccessToOrderItemLogAndReturnErrorPage(int orderItemId, string companyApiKey)
        {
            _logger.InsertLog(LogLevel.Error, (int)Events.AccessDeniedGetEntity, nameof(Order),
                $"بروز خطا کابر جاری به شناسه {_userPrincipal.Id} ایتم سفارشی به شناسه " +
                $"{orderItemId} ثبت نکرده است همچنین کلید ارسالی برابر است با{companyApiKey}", null);

            return View("Error");
        }


        /// <summary>
        /// دریافت اطلاعات سفارش های که در ضعیت منتظر تایید
        /// تامین کننده قبل از پرداخت هستند.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> WaitingForSupplierApproval()
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }

            return await PrepareOrderViewModelAsync(_orderService.GetCurrentUserWaitingForSupplierApprovalForPaymentOrder(),
                _orderService.GetCurrentUserWaitingForSupplierApprovalForPaymentOrderItem());
        }
    }
}