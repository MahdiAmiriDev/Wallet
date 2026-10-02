using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.Implementations;
using YashilCommerce.Web.Areas.ECommerce.ViewModels.SaminPaymentModels;
using YashilUserManagement.Core.Services;


namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [Authorize]
    public class SaminPaymentController : YashilMvcBaseController<object>
    {
        private readonly ISaminPaymentConfiguration _saminPaymentConfiguration;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IApplicationService _applicationService;
        private readonly PaymentTransactionUtil _paymentTransactionUtil;
        private readonly IWebHelper _webHelper;
        public SaminPaymentController
        (ApplicationConfiguration applicationConfiguration,
            IMapper mapper, IYashilLogger<object> logger,
            IPermissionService permissionService,
            ISaminPaymentConfiguration saminPaymentConfiguration,
            IOrderService orderService, IPaymentService paymentService, IApplicationService applicationService,
            PaymentTransactionUtil paymentTransactionUtil, IWebHelper webHelper) :
            base(applicationConfiguration, mapper, logger, permissionService)
        {
            _saminPaymentConfiguration = saminPaymentConfiguration;
            _orderService = orderService;
            _paymentService = paymentService;
            _applicationService = applicationService;
            _paymentTransactionUtil = paymentTransactionUtil;
            _webHelper = webHelper;
        }

        /// <summary>
        /// the first step of Samin Payment
        /// </summary>
        /// <param name="oId">orderId</param>
        /// <param name="purchaseType">نوع خرید</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? oId, int? purchaseType)
        {
            if (!purchaseType.HasValue)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"purchaseType is null In Samin Payment",
                    null);

                return null;
            }
            // دریافت پارامتر های مربوط به توکن
            var generateTokenParams =
                await _orderService.GetGenerateTokenParams(
                    oId,
                    PaymentMethodTypesEnum.Samin,
                    _saminPaymentConfiguration.MerchantCode,
                    _saminPaymentConfiguration.TerminalCode
                );
            if (!generateTokenParams.HasValue)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"خروحی متد  GetGenerateTokenParams برابر با Null می باشد.",
                    null);

                return RedirectToAction("Index", "Error");
            }
            var token = await GenerateTokenAsync(
                generateTokenParams.Value.amount,
                generateTokenParams.Value.resNumber,
                generateTokenParams.Value.installmentCount,
                purchaseType.Value
                );
            if (!string.IsNullOrEmpty(token))
            {
                // هدایت کاربر به صفحه درگاه پرداخت سامان به همراه توکن دریافتی از درگاه
                return RedirectToIpg(token);
            }
            else
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"خطا در ساخت توکن برای سفارش با کد: {oId}",
                    null);
            }

            // اگر عدد برگشت مانند -18 آی پی بسته است و.. به مستند مراجعه شود
            return RedirectToAction("Index", "Error");

        }

        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaymentTransaction()
        {
            var saminVerifyViewModel = new VerifyPaymentViewModel
            {
                PaymentMethodType = PaymentMethodTypesEnum.Samin
            };
            var transactionState = Request.Form[SaminResultParam.State].ToString();
            var responseInformation = Request.Form[SaminResultParam.ResponseInformation];
            // رسید دیجیتالی خرید
            var referenceNum = Request.Form[SaminResultParam.RefNum].ToString();
            var resNum = Request.Form[SaminResultParam.ResNum].ToString();
            // در صورتی که مقدار transactionState 
            // مخالف صفر باشد یعنی عملیات با موفقیت انجام نشده است
            var checkRequest = _orderService.CheckRequestField(
             Request.HasFormContentType,
             Request.ContentType,
             transactionState,
             referenceNum,
             resNum);
            if (!checkRequest.IsSuccess)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment", $"Request ارسال شده از سمت بانک خطا دارد:" +
                               $" {checkRequest.Message}", null);

                return RedirectToAction("Index", "Error");
            }

            // CheckRequestField IsSuccess 
            else
            {
                //شماره خرید 
                Payment payment = await _paymentService
                    .GetByReservationNumberIncludeOrderAsync(resNum);
                if (payment == null)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                        "Payment", $"تراکنش پرداخت با این کد یافت نشد:" +
                                   $" {resNum}", null);
                    return RedirectToAction("Index", "Error");
                }

                var baseUrl = _webHelper.GetBaseUrl(true);
                saminVerifyViewModel.OrderListUrl = baseUrl + "/Order";

                if (_paymentTransactionUtil.PaymentTransactionStatus.ContainsKey(resNum))
                {
                    saminVerifyViewModel.Message = "کاربر گرامی خواهشمند است جهت اطلاع از وضعیت پرداخت های خود از سایت پذرنده مجددا اقدام نمایید";
                    // تراکنش را به حالت پردازش می برد تا در صورتی که دوباره فراخوانی شود، پردازش مجدد انجام نشود 
                    return View(saminVerifyViewModel);
                }
                else
                {
                    var token = Request.Form[SaminResultParam.Token];
                    saminVerifyViewModel.RefNum = referenceNum;
                    // بررسی می شود که رسید دیجیتالی ارسالی
                    // از سمت بانک برای تراکنش پرداخت جدید
                    // قبلا به پرداختی از نوع بانک سامان اختصاص داده نشده باشد
                    var refNumberIsExists = await
                        _paymentService.CheckExitsByRefNumberAsync
                            (PaymentMethodTypesEnum.Samin, referenceNum);
                    // رسید دیجیتالی تکراری می باشد
                    if (refNumberIsExists)
                    {
                        if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                        {
                            saminVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                        }
                        else
                        {
                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                PaymentStatusTypesEnum.Unsuccessful,
                                PaymentMethodTypesEnum.Samin,
                                "بروز خطا در متد بازگشت از درگاه : " +
                                "رسید دیجیتالی ارسالی بانک قبلا به تراکنش پرداختی اختصاص شده است.",
                                referenceNum,
                                true);

                            _logger.InsertLog(LogLevel.Error,
                                (int)Events.PaymentException,
                                "Payment",
                                $"refNum : {referenceNum} Exist In Table Payment",
                                null);

                            return RedirectToAction("Index", "Error");
                        }

                    }
                    else
                    {
                        _paymentTransactionUtil.PaymentTransactionStatus.GetOrAdd(resNum, true);
                        saminVerifyViewModel.ResNum = resNum;
                        //
                        // در صورتی که رکورد پرداخت قبلا تایید شده باشد، دوباره آن را بروز رسانی نمی کنیم
                        // این حالت زمانی اتفاق می افتد که متد VerifyPaymentTransactionAsync
                        //بیش از یک بار با کد ResNum 
                        // فراخوانی شده باشد
                        if (payment.PaidDateUtc.HasValue)
                        {
                            if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                            {
                                saminVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                            }
                            else
                            {
                                saminVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                                saminVerifyViewModel.Message = payment.AuthorizationTransactionResult;
                            }
                        }
                        else
                        {
                            saminVerifyViewModel.OrderNumber = payment.Order.Title;
                            if (transactionState.Equals(SaminTransactionState.OK))
                            {
                                // try to get parameters receive from IPG
                                if (!string.IsNullOrEmpty(saminVerifyViewModel.ResNum))
                                {
                                    try
                                    {
                                        await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                            PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway,
                                            PaymentMethodTypesEnum.Samin,
                                            "منتظر تایید متد Verify",
                                            referenceNum,
                                            true);
                                        saminVerifyViewModel.PaymentStatusType =
                                        PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;

                                        var res = await SendTransactionAsync(
                                               SendTransactionTypeEnum.VerifyTransaction,
                                               token,
                                              Int32.Parse(referenceNum));
                                        // مبلغ ارسال شده به درگاه با مبلغ کسر شده از کاربر متفاوت می باشد
                                        if (res.isSuccess)
                                        {
                                            //  تایید تراکنش موفقیت آمیز بود 
                                           
                                            saminVerifyViewModel.ResNum = resNum;
                                            saminVerifyViewModel.Message = string.Format(
                                                res.Message, payment.TotalInt);

                                            try
                                            {
                                                await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                                                    OrderStatusTypesEnum.Complete);
                                                await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                    PaymentStatusTypesEnum.Paid,
                                                    PaymentMethodTypesEnum.Samin,
                                                    saminVerifyViewModel.Message,
                                                    saminVerifyViewModel.RefNum,
                                                     true);
                                                saminVerifyViewModel.PaymentStatusType =
                                                    PaymentStatusTypesEnum.Paid;
                                            }
                                            catch (Exception ex)
                                            {

                                                var message = "paymentId :" + payment.Id +
                                                              " ,PaymentMethodTypesEnum :" +
                                                              PaymentMethodTypesEnum.Samin +
                                                              " ,PaymentStatusTypesEnum :" +
                                                              PaymentStatusTypesEnum.Paid +
                                                              " ,reference number:" + referenceNum + " " +
                                                              ",reservation number: " + resNum +
                                                              ",message" +
                                                              saminVerifyViewModel.Message;

                                                _logger.InsertLog(LogLevel.Error,
                                                    (int)Events.PaymentException, "Payment", message, ex);

                                                await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                          PaymentStatusTypesEnum.Unsuccessful,
                                                          PaymentMethodTypesEnum.Samin,
                                                          message, referenceNum,
                                                            true);
                                                return RedirectToAction("Index", "Error");
                                            }

                                        }
                                        // VerifyTransaction Is not Success 
                                        else
                                        {
                                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                PaymentStatusTypesEnum.Unsuccessful,
                                                PaymentMethodTypesEnum.Samin,
                                                $"عدم تایید در درگاه ثمین :" +
                                                $" {res.Message}",
                                                referenceNum,
                                                true);

                                            _logger.InsertLog(LogLevel.Error,
                                                (int)Events.PaymentException, "Payment",
                                                $"عدم تایید  در درگاه ثمین : {res.Message}",
                                                null);

                                            return RedirectToAction("Index", "Error");
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.InsertLog(LogLevel.Error,
                                        (int)Events.PaymentException, "Payment",
                                            "", ex);

                                        await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                            PaymentStatusTypesEnum.Unsuccessful,
                                            PaymentMethodTypesEnum.Samin,
                                            $"بروز خطا قبل از تایید در بانک سامان",
                                            referenceNum,
                                            "", null,
                                            null, true);
                                        return RedirectToAction("Index", "Error");
                                    }
                                }
                            }
                            else
                            {
                                saminVerifyViewModel.Message = responseInformation;
                                await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                    PaymentStatusTypesEnum.Unsuccessful,
                                    PaymentMethodTypesEnum.Samin, responseInformation,
                                    saminVerifyViewModel.RefNum,
                                    true);

                                saminVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Cancel;
                            }
                        }

                        return View(saminVerifyViewModel);
                    }
                }

            }
            return RedirectToAction("Index", "Error");
        }


        /// <summary>
        ///  نمایش اطلاعات مربوط به درگاه و 
        /// انتخاب مدل پرداخت درگاه پرداخت
        /// در صورتی که درگاه پرداخت دارای مدل های مختلف باشد
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ShowPurchaseType(int recipientId, int installmentCount)
        {
            var saminPaymentViewModel = new SaminPaymentViewModel
            {
                RecipientId = recipientId,
                InstallmentCount = installmentCount
            };
            return View(saminPaymentViewModel);
        }

        #region Ipg New Implimantation

        /// <summary>
        /// متد ساخت توکن 
        /// </summary>
        /// <param name="amount">مبلغ درخواستی</param>
        /// <param name="resNum">کد مرجع در فروشگاه پذیرنده</param>
        /// <param name="installmentCount">تعداد اقساط</param>
        /// <param name="purchaseType">نوع خرید</param>
        /// <returns></returns>
        private async Task<string> GenerateTokenAsync(
           long amount,
           string resNum,
           int? installmentCount,
           int purchaseType)
        {
            var baseUrl = _webHelper.GetBaseUrl(true);

            var tokenRequestModel = new SaminGenerateTokenRequestModel
            {
                Amount = amount,
                ReturnURL = baseUrl + _saminPaymentConfiguration.CallBackPath,
                NumberOfInstallment = installmentCount is > 1 &&
                                       purchaseType == (int)SaminPurchaseTypeEnum.Installment ?
                    installmentCount.Value : 1,
                PurchaseDate = DateTime.Now,
                MerchantReferenceNumber = resNum,
                Type = purchaseType,
                MerchantCode = _saminPaymentConfiguration.MerchantCode,
                TerminalCode = _saminPaymentConfiguration.TerminalCode,
                MerchantPassword = _saminPaymentConfiguration.MerchantPassword,
            };

            var saminGenerateTokenResult = await
                _orderService.HttpClientSendAsync<SaminGenerateTokenRequestModel,
                    SaminGenerateTokenResultModel>
                  (tokenRequestModel,
                   _saminPaymentConfiguration.GenerateTokenUrl);
            if (saminGenerateTokenResult != null)
            {
                if (!string.IsNullOrEmpty(saminGenerateTokenResult.Token))
                {
                    return saminGenerateTokenResult.Token;
                }
                else
                {
                    _logger
                        .InsertLog(LogLevel.Error,
                            (int)Events.PaymentException,
                            "Payment", $"خطا در ساخت توکن : {saminGenerateTokenResult.ResponseInformation} ", null);
                    return string.Empty;
                }

            }
            return string.Empty;
        }
        /// <summary>
        /// متد ارسال به صفحه درگاه پرداخت
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private RedirectResult RedirectToIpg(string token)
        {
            return Redirect($"{_saminPaymentConfiguration.RedirectToIpgUrl}{token}");
        }
        /// <summary>
        /// متد درخواست تایید تراکنش یا لغو تراکنش
        /// </summary>
        /// <param name="sendTransactionType">نوع درخواست </param>
        /// <param name="token">توکن</param>
        /// <param name="refNum">شماره رهگیری</param>
        /// <returns></returns>
        private async Task<(bool isSuccess, string Message)> SendTransactionAsync(
            SendTransactionTypeEnum sendTransactionType,
            string token,
            int refNum)
        {
            var saminConfirmTransactionRequestModel = new SaminConfirmTransactionRequestModel
            {
                Token = token,
                RefNO = refNum,
                CONFIRM_TRANSACTION_STATUS = (int)sendTransactionType
            };
            var sendTrans = await _orderService
                .HttpClientSendAsync<SaminConfirmTransactionRequestModel,
                    SaminConfirmTransactionResponseModel>
                (saminConfirmTransactionRequestModel,
                    _saminPaymentConfiguration.VerifyTransactionUrl);
            if (sendTrans != null)
            {
                if (sendTrans.ResponseCode == 0)
                {
                    return (true, "با موفقیت انجام شد");
                }
                else
                {
                    return (false, sendTrans.ResponseInformation);
                }


            }
            else
            {
                _logger
                    .InsertLog(LogLevel.Error,
                        (int)Events.PaymentException,
                        "Payment",
                        $"خطا متد ConfirmTransaction مقدار خروجی Null دارد.  ", null);
                return (false, string.Empty);
            }

        }
        #endregion


    }
}
