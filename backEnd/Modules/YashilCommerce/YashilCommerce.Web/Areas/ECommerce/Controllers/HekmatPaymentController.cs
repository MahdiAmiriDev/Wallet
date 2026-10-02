using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [Authorize]
    public class HekmatPaymentController : YashilMvcBaseController<object>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHekmatPaymentConfiguration _hekmatPaymentConfiguration;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IHekmatService _hekmatService;
        private readonly IApplicationService _applicationService;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly IConfiguration _configuration;
        private readonly IUserPrincipal _userPrincipal;

        public HekmatPaymentController(ApplicationConfiguration applicationConfiguration, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, IYashilLogger<object> logger, IHekmatService hekmatService,
            IPermissionService permissionService, IHekmatPaymentConfiguration hekmatPaymentConfiguration,
            IApplicationService applicationService,
            IOrderService orderService, IPaymentService paymentService, StoreConfiguration storeConfiguration, IConfiguration configuration, IUserPrincipal userPrincipal)
            : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _hekmatPaymentConfiguration = hekmatPaymentConfiguration;
            _orderService = orderService;
            _paymentService = paymentService;
            _storeConfiguration = storeConfiguration;
            _configuration = configuration;
            _userPrincipal = userPrincipal;
            _hekmatService = hekmatService;
            _applicationService = applicationService;
        }

        /// <summary>
        /// این متد هنگامی فراخوانی میشود که کاربر بخواهد بوسیله درگاه انستیتو پرداخت خود را انجام دهد
        /// در این متد سرویس حکمت فراخوانی میشود و سپس کاربر به صفحه درگاه انستیتو انتقال داده میشود.
        /// </summary>
        /// <param name="oId"></param>
        /// <param name="isHybrid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(int? oId, bool isHybrid = false)
        {
            // باید مقدار oId پاس داده شود
            if (!oId.HasValue)
            {
                return RedirectToAction("Index", "Error");
            }

            // دریافت اطلاعات سفارش
            var order = await _orderService.GetAsync(oId.Value, true);
            if (order == null)
            {
                return RedirectToAction("Index", "Error");
            }

            PaymentInformation paymentInformation = new();
            AmountForPayment amountForPayment = new();

            if (isHybrid)
            {
                amountForPayment = await _orderService.CalcAmountForPayAsync((int)PaymentMethodTypesEnum.Hybrid, oId.Value);
            }
            else
            {
                // دریافت اطلاعات پرداختی های سفارش
                paymentInformation = await _orderService.CreateNewPaymentAndReturnAsync(oId.Value,
                    PaymentMethodTypesEnum.Hekmat,
                    _hekmatPaymentConfiguration.MerchantId,
                    _hekmatPaymentConfiguration.TerminalId);
            }

            if (amountForPayment != null && amountForPayment.Total != 0 && isHybrid)
            {
                await SendRequestPaymentAsync(amountForPayment.Total, amountForPayment.InstallmentCount, order.Title,
                     order.CardNumber, null, oId, true);

            }
            else if (paymentInformation != null && paymentInformation.Total.HasValue &&
                     paymentInformation.Total.Value != 0 && !isHybrid)
            {
                // اگر باقیمانده پرداختی وجود داشته باشد و یا تا کنون پرداختی برای سفارش انجام نشده باشد مجاز به ادامه عملیات هستیم

                await SendRequestPaymentAsync(paymentInformation.Total.Value, paymentInformation.InstallmentCount, order.Title,
                    order.CardNumber, paymentInformation.PaymentId, oId);

            }

            // اگر اطلاعات پرداخت وجود نداشته باشد یا مقدار پرداختی وجود نداشته باشد باگی در سامانه وجود دارد
            else if (paymentInformation != null)
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                    $"Total Value has not value or is Zero with paymentId ={paymentInformation.PaymentId}", null);
            else
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                    $"paymentInformation is null For OrderId ={oId}", null);
            }

            return RedirectToAction("Index", "Error");
        }

        private async Task SetPaymentStatusTypeUnsuccessfulAsync(int paymentId)
        {
            await _paymentService.UpdatePaymentStatusAsync(paymentId, PaymentStatusTypesEnum.Unsuccessful,
                PaymentMethodTypesEnum.Hekmat, HekmatResultStatusMessage.Cancel, "", true);
        }

        /// <summary>
        /// صفحه بازگشت از درگاه
        /// این متد هنگامی فراخوانی میشود که کاربر در صفحه درگاه اطلاعات خود را وارد کرده
        /// و دکمه پرداخت را زده و یا دکمه انصراف را زده و به آدرس بازگشتی هدایت میشود
        /// در آدرس بازگشت حکمت این متد فراخوانی میشود.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaymentTransaction()
        {
            // در هنگام فراخوانی این متد باید اطلاعاتی به صورت پست ارسال شده باشد
            // چک میکنیم که حتما reservationNumber و Status ارسال شده باشند
            if (Request.HasFormContentType && !string.IsNullOrEmpty(
                    Request.Form[HekmatVerifyParams.ReservationNumber]) &&
                !string.IsNullOrEmpty(Request.Form[HekmatVerifyParams.StatusCode]))
            {
                var hekmatResult = new VerifyPaymentViewModel()
                {
                    // اگر سفارش موفقیت امیز باشد refrenceNumber  حتما مقدار دارد
                    RefNum = Request.Form[HekmatVerifyParams.ReferenceNumber],
                    // شناسه پرداخت سایت است
                    ResNum = Request.Form[HekmatVerifyParams.ReservationNumber],
                    // کد وضعیت بازگشتی از درگاه است حالت 0 1 99 دارد
                    StateCode = Request.Form[HekmatVerifyParams.StatusCode],
                    PaymentMethodType = PaymentMethodTypesEnum.Hekmat,
                    Message = Request.Form[HekmatVerifyParams.Message]
                };


                // دریافت اطلاعات پرداخت
                var payment = await _paymentService.GetAsync(int.Parse(hekmatResult.ResNum), true);


                if (payment != null)
                {
                    var order = await _orderService.GetAsync(payment.OrderId, true);


                    if (order != null)
                    {
                        // شماره سفارش
                        hekmatResult.OrderNumber = order.Title;
                        var application = await _applicationService.GetAsync(payment.ApplicationId, true);
                        hekmatResult.OrderListUrl = application.Url + "/Order";
                        // اگر وضعیت پرداخت در حال بررسی باشد
                        //یعنی کاربر در صفحه درگاه اطلاعات را بصورت کامل پرکرده است و 
                        // دکمه پرداخت را زده
                        //و اطلاعات جهت بررسی توسط درگاه به سمت حکمت ارسال میشود و تا زمان بررسی پرداخت در این وضعیت باقی میماند
                        // نکته مهم این است که پرداخت در حال بررسی در حکمت به معنای پرداخت  شده نمیباشد و درواقع تراکنش در صف بررسی میباشد
                        // برای اطلاع از وضعیت پرداخت پس از طی مدت زمانی
                        // متد checkTransaction  را فراخوانی میکنیم.
                        if (!string.IsNullOrEmpty(hekmatResult.RefNum) && hekmatResult.StateCode ==
                            ((int)HekmatPurchaseResultStatusCodeEnum.Successful).ToString())
                        {
                            //hekmatResult.Message = HekmatResultStatusMessage.Pending;
                            await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                                OrderStatusTypesEnum.Processing);


                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway, PaymentMethodTypesEnum.Hekmat,
                                HekmatResultStatusMessage.Pending, hekmatResult.RefNum, true);


                            hekmatResult.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;

                            _logger.InsertLog(LogLevel.Information, (int)Events.Insert, nameof(Payment),
                                $"sucessful payment in hekmatIpg for processing for payment id ={payment.Id}", null);
                        }
                        // اگر وضعیت پرداخت درحالت انصراف باشد یعنی کاربر در صفحه درگاه دکمه انصراف را زده است.
                        else if (hekmatResult.StateCode == ((int)HekmatPurchaseResultStatusCodeEnum.Cancel).ToString())
                        {
                            hekmatResult.Message = HekmatResultStatusMessage.Cancel;
                            await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Cancel,
                                PaymentMethodTypesEnum.Hekmat, HekmatResultStatusMessage.Cancel, hekmatResult.RefNum,
                                true);
                            hekmatResult.PaymentStatusType = PaymentStatusTypesEnum.Cancel;
                        }

                        return View(hekmatResult);
                    }
                }

            }

            return RedirectToAction("Index", "Error");
        }


        /// <summary>
        /// این متد زمانی که کاربر یک پرداخت
        /// نقدی اقساطی را انجام داده است و در انتهای فرآیند
        /// پرداخت برای
        /// verify
        /// عملیات پرداخت انجام شده این متد فراخوانی می شود
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> VerifyHybridPaymentTransaction()
        {
            // در هنگام فراخوانی این متد باید اطلاعاتی به صورت پست ارسال شده باشد
            // چک میکنیم که حتما reservationNumber و Status ارسال شده باشند
            if (Request.HasFormContentType && !string.IsNullOrEmpty(
                    Request.Form[HekmatVerifyParams.ReservationNumber]) &&
                !string.IsNullOrEmpty(Request.Form[HekmatVerifyParams.StatusCode]))
            {
                var hekmatResult = new VerifyPaymentViewModel()
                {
                    // اگر سفارش موفقیت امیز باشد refrenceNumber  حتما مقدار دارد
                    RefNum = Request.Form[HekmatVerifyParams.ReferenceNumber],
                    // شناسه پرداخت سایت است
                    ResNum = Request.Form[HekmatVerifyParams.ReservationNumber],
                    // کد وضعیت بازگشتی از درگاه است حالت 0 1 99 دارد
                    StateCode = Request.Form[HekmatHybridPaymentVerifyParams.Status],
                    PaymentMethodType = PaymentMethodTypesEnum.Hekmat,
                    Message = Request.Form[HekmatVerifyParams.Message],
                    Amount = Request.Form[HekmatVerifyParams.Amount],
                    InstallmentCount = Request.Form[HekmatVerifyParams.InstallmentCount],
                    HybridPaymentRequestId = Request.Form[HekmatVerifyParams.HybridPaymentRequestId],
                    HybridPaymentId = Request.Form[HekmatVerifyParams.HybridPaymentId],
                    HekmatGatewayTransactionId = Request.Form[HekmatVerifyParams.HekmatGatewayTransactionId]
                };
                try
                {
                    var orderId = int.Parse(hekmatResult.ResNum);

                    if (orderId == default(int))
                        return RedirectToAction("Index", "Error");

                    var order = await _orderService.GetAsync(orderId, true);

                    if (order == null)
                        return RedirectToAction("Index", "Error");

                    hekmatResult.OrderNumber = order.Title;
                    var application = await _applicationService.GetAsync(order.ApplicationId, true);
                    hekmatResult.OrderListUrl = application.Url + "/Order";

                    await _paymentService.AddAsync(new Payment()
                    {
                        Code = "1",
                        OrderId = orderId,
                        PaidDateInt = DateFunction.TodayShamsiInt(),
                        ReservationNumber = _storeConfiguration.OrderReferenceNumberPrefix + "_" + hekmatResult.ResNum,

                        MerchantId = CryptographyHelper.AesDecrypt(_configuration
                            .GetSection("HybridPaymentConfiguration:MerchantId").Get<string>()),

                        TerminalNumber = CryptographyHelper.AesDecrypt(_configuration
                            .GetSection("HybridPaymentConfiguration:TerminalId").Get<string>()),

                        AuthorizationTransactionResult =
                            hekmatResult.StateCode == ((int)PaymentGatewayResultStatusCodeEnum.WaitingForReview).ToString()
                                ? HekmatResultStatusMessage.Pending
                                : hekmatResult.StateCode == ((int)PaymentGatewayResultStatusCodeEnum.Successful).ToString() ?
                                    HekmatResultStatusMessage.Successful: HekmatResultStatusMessage.Cancel,

                        PaidDateUtc = DateTime.UtcNow,
                        SaleType = (int)SaleTypesEnum.CashPaymentAndInstallmentPayment,

                        PaymentStatusType =
                            hekmatResult.StateCode == ((int)PaymentGatewayResultStatusCodeEnum.WaitingForReview).ToString()
                                ?
                                (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway
                                : hekmatResult.StateCode == ((int)PaymentGatewayResultStatusCodeEnum.Successful).ToString()
                                    ? (int)PaymentStatusTypesEnum.Paid
                                    : (int)PaymentStatusTypesEnum.Unsuccessful,

                        PaymentMethodType = (int)PaymentMethodTypesEnum.Hybrid,
                        InstallmentCount = int.Parse(hekmatResult.InstallmentCount),
                        Total = decimal.Parse(hekmatResult.Amount),
                        YearMonth = DateFunction.TodayShamsiYearAndMonthInt(),
                        CreateBy = _userPrincipal.Id,
                        CreationDate = DateTime.Now,
                        ApplicationId = _userPrincipal.ApplicationId,
                        AccessLevelId = 0,
                        Deleted = false,
                        CreatorOrganizationId = 1,
                        TotalInt = int.Parse(hekmatResult.Amount),
                        IsHybrid = true,
                        AuthorizationTransactionId = hekmatResult.HekmatGatewayTransactionId,
                        InstallmentTrackingCode = hekmatResult.HekmatGatewayTransactionId,
                        HybridPaymentRequestId = int.Parse(hekmatResult.HybridPaymentRequestId), 
                        HybridPaymentId = int.Parse(hekmatResult.HybridPaymentId)
                    }, true);


                    if (hekmatResult.StateCode == ((int)PaymentGatewayResultStatusCodeEnum.WaitingForReview).ToString())
                    {


                        await _orderService.UpdateOrderStatusAsync(orderId, OrderStatusTypesEnum.Processing,true);

                        hekmatResult.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;

                        _logger.InsertLog(LogLevel.Information, (int)Events.Insert, nameof(Payment),
                            $"sucessful payment in hekmatIpg for processing for order id ={orderId}", null);

                    }else if (hekmatResult.StateCode == ((int)PaymentGatewayResultStatusCodeEnum.Successful).ToString())
                    {
                        await _orderService.UpdateOrderStatusAsync(orderId, OrderStatusTypesEnum.Complete, true);
                        hekmatResult.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                    }
                    else if (hekmatResult.StateCode == ((int)PaymentGatewayResultStatusCodeEnum.Canceled).ToString())
                    {
                        hekmatResult.Message = HekmatResultStatusMessage.Cancel;
                        hekmatResult.PaymentStatusType = PaymentStatusTypesEnum.Cancel;
                    }

                    return View("VerifyPaymentTransaction", hekmatResult);
                }
                catch (Exception e)
                {

                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(HekmatPaymentController),
                        $"an error occurred in verifying hekmat hybrid payment", e);

                    return RedirectToAction("Index", "Error");
                }

            }

            return RedirectToAction("Index", "Error");


        }


        public async Task<IActionResult> SendRequestPaymentAsync(decimal total, int? installmentCount,
            string orderTitle, string cardNumber, int? paymentId, int? orderId, bool isHybrid = false)
        {
            // شماره کارت وارد شده در مرحله تحویل گیرنده توسط کاربر پر شده باشد 
            // برای ارسال به درگاه پرداخت باید این شماره کارت به همراه سایر پارامتر ها به درگاه ارسال شود
            // و کاربر در صفحه درگاه با همین شماره کارت میتواند پرداخت حکمت انجام بدهد
            // زیرا شماره کارت وارد شده در مرحله تحویل گیرنده ممکن است یارانه دریافت کرده باشد
            // اگر شماره کارتی وجود نداشته باشد در صفحه درگاه از کاربر شماره کارت دریافت میشود.
            var amount = decimal.ToInt32(total);
            installmentCount = installmentCount ?? 1;
            var reservationNumber = paymentId ?? orderId.Value;
            // ارسال درخواست به درگاه
            var paymentRequestResult = await _hekmatService.RequestPaymentInHekmatIpgAsync(reservationNumber.ToString(),
                amount, installmentCount.Value,
                cardNumber, orderTitle, isHybrid);


            // پاسخ درگاه را بررسی میکنیم
            if (paymentRequestResult != null)
            {
                // اگر توکن داشته باشیم و پاسخ درست باشد کاربر را به صفحه درگاه به همراه توکن دریافتی ارسال میکنیم
                if (paymentRequestResult.HasToken &&
                    paymentRequestResult.StatusCode == (int)PaymentGatewayRequestStatusCodeEnum.Successful)
                {
                    // هدایت کاربر به صفحه درگاه انستیتو به همراه توکن دریافتی از درگاه
                    var remotePost = new RemotePost(_httpContextAccessor)
                    {
                        Url = !isHybrid ? _hekmatPaymentConfiguration.IpgUrl : "/HybridIpg" //TODO
                    };
                    // اضافه کردن اطلاعات توکن و پست کردن آن به صفحه درگاه
                    remotePost.Add("encryptedToken", paymentRequestResult.Token);
                    await remotePost.PostAsync();

                }
                // در غیر این صورت حالت های عدم تولید توکن را بررسی میکنیم
                else
                {
                    // ممکن است کد سفارش تکراری باشد پس در اینجا وضعیت سفارش را بررسی میکنیم
                    if (paymentRequestResult.StatusCode ==
                        (int)PaymentGatewayRequestStatusCodeEnum.DuplicateLocalReservationNumber && !isHybrid)
                    {
                        await _hekmatService.UpdateOrderAndPaymentStatusByPaymentIdAsync(paymentId.Value,
                            orderId.Value);
                    }
                    // در غیر اینصورت خطای دیگری وجود دارد که هم لاک میزنی هم وضعیت پرداخت را غیر موفق میکنیم
                    else if (!isHybrid)
                    {
                        await SetPaymentStatusTypeUnsuccessfulAsync(paymentId.Value);

                        string orderIdOrPaymentId = (paymentId == null || paymentId == 0) ? "paymentId" : "orderId";

                        _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                            $"fail to create request Hekmat Ipg with statusCode ={paymentRequestResult.StatusCode}" +
                            $" and message = {paymentRequestResult.Message} for {orderIdOrPaymentId} : {reservationNumber}",
                            null);
                        return RedirectToAction("Index", "Error");
                    }
                }
            }
            // اگر اطلاعات درواستی درگاه خالی باشد خطایی رخ داده است
            else if (!isHybrid)
            {
                await SetPaymentStatusTypeUnsuccessfulAsync(paymentId.Value);
            }

            _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Order",
                "بروز خطا در پرداخت درگاه حکمت", null);

            return RedirectToAction("Index", "Error");
        }
    }
}