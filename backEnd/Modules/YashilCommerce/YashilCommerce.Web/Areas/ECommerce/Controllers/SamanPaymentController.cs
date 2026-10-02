using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels.SamanPaymentModels;
using YashilUserManagement.Core.Services;
using YashilCommerce.Infrastructure.Implementations;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [Authorize]
    public class SamanPaymentController : YashilMvcBaseController<object>
    {
        private readonly ISamanPaymentConfiguration _samanPaymentConfiguration;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly PaymentTransactionUtil _paymentTransactionUtil;
        private readonly IApplicationService _applicationService;

        public SamanPaymentController(ApplicationConfiguration applicationConfiguration,
            IMapper mapper, IYashilLogger<object> logger,
            IPermissionService permissionService,
            IOrderService orderService,
            ISamanPaymentConfiguration samanPaymentConfiguration,
            IPaymentService paymentService, IApplicationService applicationService,
            PaymentTransactionUtil paymentTransactionUtil) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _orderService = orderService;
            _samanPaymentConfiguration = samanPaymentConfiguration;
            _paymentService = paymentService;
            _applicationService = applicationService;
            _paymentTransactionUtil = paymentTransactionUtil;
        }

        /// <summary>
        /// the first step of Saman Payment
        /// </summary>
        /// <param name="oId">orderId</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? oId)
        {
            if (!oId.HasValue)
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"شناسه سفارش برای درج پرداخت و ارسال به درگاه مقدار ندارد",
                    null);

                return RedirectToAction("Index", "Error");
            }
            var order = _orderService.Get(oId, true);
            if (order == null)
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"سفارشی با این شناسه وجود ندارد: {oId}",
                    null);

                return RedirectToAction("Index", "Error");
            }

            if (string.IsNullOrEmpty(_samanPaymentConfiguration.TerminalId))
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"درگاه پرداخت سامان ترمینال آی دی ندارد",
                    null);

                return RedirectToAction("Index", "Error");
            }
            var paymentInformation = await _orderService
                .CreateNewPaymentAndReturnAsync(oId.Value, PaymentMethodTypesEnum.Saman
                    , "", _samanPaymentConfiguration.TerminalId);

            if (paymentInformation != null &&
                paymentInformation.Total.HasValue &&
                paymentInformation.Total.Value != 0)
            {
                try
                {
                    var amount = decimal.ToInt64(paymentInformation.Total.Value);
                    var resNumber = _orderService.GetReservationNumber(
                        oId.Value, paymentInformation.PaymentId);

                    var token = await GenerateTokenAsync(
                        amount,
                        resNumber,
                        order.RecipientMobileNumber);
                    if (!string.IsNullOrEmpty(token))
                    {
                        return RedirectToIpg(token);
                    }
                    else
                    {
                        _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                            (int)Events.PaymentException,
                            "Payment",
                            $"خطا در ساخت توکن برای سفارش با کد: {resNumber}",
                            null);
                    }
                }
                catch (Exception ex)
                {
                    await _paymentService.UpdatePaymentStatusAsync(paymentInformation.PaymentId, PaymentStatusTypesEnum.Unsuccessful, PaymentMethodTypesEnum.Saman, SamanTransactionStateMessage.IssuerDownSlm, "", true);
                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                        (int)Events.PaymentException, "Payment",
                        "", ex);
                    return RedirectToAction("Index", "Error");
                }

                // اگر عدد برگشت مانند -18 آی پی بسته است و.. به مستند مراجعه شود

                return RedirectToAction("Index", "Error");
            }
            return RedirectToAction("Index", "Error");
        }

        /// <summary>
        /// this method invoke when customer comback from ipg
        /// it verify the transaction of sep ipg
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaymentTransaction()
        {
            var samanVerifyViewModel = new VerifyPaymentViewModel
            {
                PaymentMethodType = PaymentMethodTypesEnum.Saman
            };

            if (RequestUnpack(out var transactionState))
            {
                //شماره خرید
                var resNumber = Request.Form[SamanResultParam.ResNum];
                Payment payment = await _paymentService
                    .GetByReservationNumberIncludeOrderAsync(resNumber);
                if (payment == null)
                {
                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error, (int)Events.PaymentException,
                        "Payment", $"تراکنش پرداخت با این کد یافت نشد:" +
                                   $" {resNumber}", null);
                    return RedirectToAction("Index", "Error");
                }

                var application = await _applicationService.GetAsync(payment.ApplicationId, true);
                samanVerifyViewModel.OrderListUrl = application.Url + "/Order";

                if (_paymentTransactionUtil.PaymentTransactionStatus.ContainsKey(resNumber))
                {
                    samanVerifyViewModel.Message = "کاربر گرامی خواهشمند است جهت اطلاع از وضعیت پرداخت های خود از سایت پذرنده مجددا اقدام نمایید";
                    // تراکنش را به حالت پردازش می برد تا در صورتی که دوباره فراخوانی شود، پردازش مجدد انجام نشود 
                    return View(samanVerifyViewModel);
                }
                else
                {

                    // رسید دیجیتالی خرید
                    var refNum = Request.Form[SamanResultParam.RefNum];
                    //شماره مرجع
                    var rrn = Request.Form[SamanResultParam.Rrn];
                    //شماره رهگیری
                    // شماره ترمینال
                    var terminalId = Request.Form[SamanResultParam.TerminalId];

                    // بررسی می شود که رسید دیجیتالی ارسالی
                    // از سمت بانک برای تراکنش پرداخت جدید
                    // قبلا به پرداختی از نوع بانک سامان اختصاص داده نشده باشد
                    var refNumberIsExists = await
                        _paymentService.CheckExitsByRefNumberAsync(PaymentMethodTypesEnum.Saman, refNum.ToString());
                    // رسید دیجیتالی تکراری می باشد
                    if (refNumberIsExists)
                    {
                        if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                        {
                            samanVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                        }
                        else
                        {
                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                PaymentStatusTypesEnum.Unsuccessful,
                                PaymentMethodTypesEnum.Saman,
                                "بروز خطا در متد بازگشت از درگاه : " +
                                "رسید دیجیتالی ارسالی بانک قبلا به تراکنش پرداختی اختصاص شده است.",
                                refNum,
                                rrn,
                                null, null,
                                true);
                            _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                                (int)Events.PaymentException,
                                "Payment",
                                $"refNum : {refNum} Exist In Table Payment",
                                null);

                            return RedirectToAction("Index", "Error");
                        }

                    }
                    else
                    {
                        _paymentTransactionUtil.PaymentTransactionStatus.GetOrAdd(resNumber, true);
                        samanVerifyViewModel.ResNum = resNumber;
                        //
                        // در صورتی که رکورد پرداخت قبلا تایید شده باشد، دوباره آن را بروز رسانی نمی کنیم
                        // این حالت زمانی اتفاق می افتد که متد VerifyPaymentTransactionAsync
                        //بیش از یک بار با کد ResNum 
                        // فراخوانی شده باشد
                        if (payment.PaidDateUtc.HasValue)
                        {
                            if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                            {
                                samanVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                            }
                            else
                            {
                                samanVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                                samanVerifyViewModel.Message = payment.AuthorizationTransactionResult;
                            }
                        }
                        else
                        {
                            samanVerifyViewModel.OrderNumber = payment.Order.Title;
                            if (transactionState.Equals(SamanTransactionState.OK))
                            {
                                // try to get parameters receive from IPG
                                samanVerifyViewModel.State = transactionState;
                                if (!string.IsNullOrEmpty(samanVerifyViewModel.ResNum))
                                {
                                    try
                                    {
                                        await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                            PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway,
                                            PaymentMethodTypesEnum.Saman,
                                            "منتظر تایید متد Verify",
                                            refNum,
                                            rrn,
                                            null,
                                            null,
                                            true);
                                        samanVerifyViewModel.PaymentStatusType =
                                        PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;

                                        var res = await SendTransactionAsync(
                                            Int64.Parse(terminalId),
                                            refNum,
                                            SendTransactionTypeEnum.VerifyTransaction);
                                        // مبلغ ارسال شده به درگاه با مبلغ کسر شده از کاربر متفاوت می باشد
                                        if (res != null &&
                                            res.Success && res.TransactionDetail != null)
                                        {
                                            // مبلغ ارسالی به درگاه با مبلغ کسر شده از کاربر برابر نیست
                                            if (res.TransactionDetail.OrginalAmount !=
                                                decimal.ToInt64(payment.Total) ||
                                                res.TransactionDetail.AffectiveAmount !=
                                                decimal.ToInt64(payment.Total))
                                            {

                                                var reverse = await SendTransactionAsync(
                                                    Int64.Parse(terminalId),
                                                    refNum,
                                                    SendTransactionTypeEnum.Reverse);
                                                // تراکنش به حساب کاربر بازگشت داده شد
                                                if (reverse is { Success: true })
                                                {
                                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                        PaymentStatusTypesEnum.Unsuccessful,
                                                        PaymentMethodTypesEnum.Saman,
                                                        "بروز خطا در متد تایید بانک سامان : مبلغ ارسال شده به درگاه " +
                                                        "با مبلغ کسر شده از کاربر متفاوت می باشد و مبلغ کسر شده به حساب کاربر بازگشت داده شد.",
                                                        refNum,
                                                        rrn,
                                                        null, null,
                                                        true);

                                                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                                                        (int)Events.PaymentException, "Payment",
                                                        $"resNum : {resNumber} -- refNum : {refNum}  reverseTransaction Success ",
                                                        null);

                                                    return RedirectToAction("Index", "Error");

                                                }
                                                else
                                                {
                                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                        PaymentStatusTypesEnum.Unsuccessful,
                                                        PaymentMethodTypesEnum.Saman,
                                                        "بروز خطا در متد بازگشت مبلغ کسر شده به حساب کاربر.",
                                                        refNum,
                                                        rrn,
                                                        null, null,
                                                        true);

                                                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                                                        (int)Events.PaymentException,
                                                        "Payment",
                                                        "res.TransactionDetail.OrginalAmount != res.TransactionDetail.AffectiveAmount reverseTransaction Is Not Success",
                                                        null);

                                                    return RedirectToAction("Index", "Error");
                                                }
                                            }
                                            //  تایید تراکنش موفقیت آمیز بود 
                                            else
                                            {
                                                samanVerifyViewModel.RefNum = refNum;
                                                samanVerifyViewModel.ResNum = resNumber;
                                                samanVerifyViewModel.Message = string.Format(
                                                    res.ResultDescription, payment.TotalInt);

                                                try
                                                {
                                                    await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                                                        OrderStatusTypesEnum.Complete);
                                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                        PaymentStatusTypesEnum.Paid,
                                                        PaymentMethodTypesEnum.Saman,
                                                        samanVerifyViewModel.Message,
                                                        samanVerifyViewModel.RefNum,
                                                        rrn,
                                                        null,
                                                        null, true);
                                                    samanVerifyViewModel.PaymentStatusType =
                                                        PaymentStatusTypesEnum.Paid;

                                                    samanVerifyViewModel.CardMaskPan = res.TransactionDetail.MaskedPan;
                                                }
                                                catch (Exception ex)
                                                {

                                                    var message = "paymentId :" + payment.Id +
                                                                  " ,PaymentMethodTypesEnum :" +
                                                                  PaymentMethodTypesEnum.Saman +
                                                                  " ,PaymentStatusTypesEnum :" +
                                                                  PaymentStatusTypesEnum.Paid +
                                                                  " ,reference number:" + refNum + " " +
                                                                  ",reservation number: " +
                                                                  resNumber +
                                                                  ", RRN: " +
                                                                  rrn +
                                                                  ",message" +
                                                                  samanVerifyViewModel.Message;
                                                    _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                                                        (int)Events.PaymentException, "Payment", message, ex);
                                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                              PaymentStatusTypesEnum.Unsuccessful, PaymentMethodTypesEnum.Saman,
                                                              message, "",
                                                              samanVerifyViewModel.CustomerRefNum,
                                                              null,
                                                              null, true);
                                                    return RedirectToAction("Index", "Error");
                                                }
                                            }
                                        }
                                        // VerifyTransaction Is not Success 
                                        else
                                        {
                                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                PaymentStatusTypesEnum.Unsuccessful,
                                                PaymentMethodTypesEnum.Saman,
                                                $"بروز خطا قبل از تایید در بانک سامان : {res?.ResultDescription}",
                                                refNum,
                                                "", null,
                                                null, true);

                                            _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                                                (int)Events.PaymentException, "Payment",
                                                $"بروز خطا قبل از تایید در بانک سامان : {res?.ResultDescription}",
                                                null);

                                            return RedirectToAction("Index", "Error");
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                                            (int)Events.PaymentException, "Payment",
                                            "", ex);

                                        await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                            PaymentStatusTypesEnum.Unsuccessful,
                                            PaymentMethodTypesEnum.Saman,
                                            $"بروز خطا قبل از تایید در بانک سامان",
                                            refNum,
                                            "", null,
                                            null, true);
                                        return RedirectToAction("Index", "Error");
                                    }
                                }
                            }
                            else
                            {
                                samanVerifyViewModel.Message = SamanTransactionStateMessage.Failed;
                                samanVerifyViewModel.Message = CheckTransactionState(transactionState, out var paymentStatusType);
                                await _paymentService.UpdatePaymentStatusAsync(payment.Id, paymentStatusType, PaymentMethodTypesEnum.Saman, samanVerifyViewModel.Message, samanVerifyViewModel.RefNum, true);
                                samanVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Cancel;
                            }
                        }

                        return View(samanVerifyViewModel);
                    }
                }
            }
            return RedirectToAction("Index", "Error");
        }

        /// <summary>
        /// check the reason of failed transaction in Saman payment
        /// </summary>
        /// <param name="transactionState"></param>
        /// <param name="paymentStatusType"></param>
        /// <returns></returns>
        private string CheckTransactionState(string transactionState, out PaymentStatusTypesEnum paymentStatusType)
        {
            paymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
            if (transactionState.Equals(SamanTransactionState.CanceledByUser) || transactionState.Equals(string.Empty))
            {
                // Transaction was canceled by user
                paymentStatusType = PaymentStatusTypesEnum.Cancel;
                return SamanTransactionStateMessage.CanceledByUser;

            }
            else if (transactionState.Equals(SamanTransactionState.InvalidAmount))
            {
                // Amount of revers teransaction is more than teransaction
                return SamanTransactionStateMessage.InvalidAmount;
            }
            else if (transactionState.Equals(SamanTransactionState.InvalidTransaction))
            {
                // Can not find teransaction
                return SamanTransactionStateMessage.InvalidTransaction;
            }
            else if (transactionState.Equals(SamanTransactionState.InvalidCardNumber))
            {
                // Card number is wrong
                return SamanTransactionStateMessage.InvalidCardNumber;
            }
            else if (transactionState.Equals(SamanTransactionState.NoSuchIssuer))
            {
                // Issuer can not find
                return SamanTransactionStateMessage.NoSuchIssuer;
            }
            else if (transactionState.Equals(SamanTransactionState.ExpiredCardPickUp))
            {
                // The card is expired
                return SamanTransactionStateMessage.ExpiredCardPickUp;
            }
            else if (transactionState.Equals(SamanTransactionState.AllowablePINTriesExceededPickUp))
            {
                // For third time user enter a wrong PIN so card become invalid
                return SamanTransactionStateMessage.AllowablePINTriesExceededPickUp;
            }
            else if (transactionState.Equals(SamanTransactionState.IncorrectPIN))
            {
                // Pin card is wrong
                return SamanTransactionStateMessage.IncorrectPIN;
            }
            else if (transactionState.Equals(SamanTransactionState.ExceedsWithdrawalAmountLimit))
            {
                // Exceeds withdrawal from amount limit
                return SamanTransactionStateMessage.ExceedsWithdrawalAmountLimit;
            }
            else if (transactionState.Equals(SamanTransactionState.TransactionCannotBeCompleted))
            {
                // PIN and PAD are currect but Transaction Cannot Be Completed
                return SamanTransactionStateMessage.TransactionCannotBeCompleted;
            }
            else if (transactionState.Equals(SamanTransactionState.ResponseReceivedTooLate))
            {
                // Timeout occur
                return SamanTransactionStateMessage.ResponseReceivedTooLate;
            }
            else if (transactionState.Equals(SamanTransactionState.SuspectedFraudPickUp))
            {
                // User did not insert cvv2 & expiredate or they are wrong.
                return SamanTransactionStateMessage.SuspectedFraudPickUp;
            }
            else if (transactionState.Equals(SamanTransactionState.NoSufficientFunds))
            {
                // there are not suficient funds in the account
                return SamanTransactionStateMessage.NoSufficientFunds;
            }
            else if (transactionState.Equals(SamanTransactionState.IssuerDownSlm))
            {
                // The bank server is down
                return SamanTransactionStateMessage.IssuerDownSlm;
            }
            else if (transactionState.Equals(SamanTransactionState.TMEError))
            {
                // unknown error occur
                return SamanTransactionStateMessage.TMEError;
            }

            return SamanTransactionStateMessage.TMEError;
        }

        private bool RequestUnpack(out string state)
        {
            state = string.Empty;
            if (!CheckRequestField().IsSuccess)
            {
                return false;
            }
            state = Request.Form[SamanResultParam.State].ToString();

            return true;
        }
        private ResultStatus<int> CheckRequestField()
        {
            if (!Request.HasFormContentType || Request.ContentType == null)
            {
                return new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "بروز خطا"
                };
            }

            if (Request.Form[SamanResultParam.State].ToString().Equals(string.Empty))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خريد شما توسط بانک تاييد شده است اما رسيد ديجيتالي شما تاييد نگشت! مشکلي در فرايند رزرو خريد شما پيش آمده است"
                };

            }

            if (Request.Form[SamanResultParam.RefNum].ToString().Equals(string.Empty) && Request.Form[SamanResultParam.State].ToString().Equals(string.Empty))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت"
                };

            }

            if (Request.Form[SamanResultParam.ResNum].ToString().Equals(string.Empty) && Request.Form[SamanResultParam.State].ToString().Equals(string.Empty))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خطا در برقرار ارتباط با بانک"
                };

            }
            return new ResultStatus<int> { IsSuccess = true };
        }

        #region Ipg New Implimantation
        /// <summary>
        /// ساخت توکن برای ارسال کاربر به همراه توکن به صفحه درگاه پرداخت
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="resNum"></param>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        private async Task<string> GenerateTokenAsync(
            long amount, string resNum,
            string mobileNumber)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var tokenRequestModel = new GenerateTokenRequestModel
            {
                Amount = amount,
                ResNum = resNum,
                RedirectUrl = _samanPaymentConfiguration.CallBackUrl,
                CellNumber = mobileNumber,
                Action = "Token",
                TerminalId = _samanPaymentConfiguration.TerminalId
            };


            var client = new HttpClient(httpHandler);

            var request = new HttpRequestMessage(HttpMethod.Post,
                _samanPaymentConfiguration.GenerateTokenUrl);

            var data = JsonSerializer.Serialize(tokenRequestModel);

            request.Content = new StringContent(data,
                Encoding.UTF8, "application/json");

            try
            {
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var generateTokenResponseModel = JsonSerializer
                    .Deserialize<GenerateTokenResponseModel>
                        (await response.Content.ReadAsStringAsync());
                // دریافت توکن موفق بود
                if (generateTokenResponseModel?.status == 1)
                {
                    return generateTokenResponseModel.token;
                }
                else
                {
                    _logger
                        .InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                            (int)Events.PaymentException,
                          "Payment", $"خطا در ساخت توکن برای سفارش با کد:" +
                                   $" {resNum} --" +
                                   $"ErrorCode : {generateTokenResponseModel?.errorCode} " +
                                   $"-- ErrorDesc : {generateTokenResponseModel?.errorDesc}",
                            null);
                }
            }
            catch (Exception e)
            {
                _logger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"خطا در ساخت توکن برای سفارش با کد: {resNum}", e);
            }

            return string.Empty;
        }

        /// <summary>
        /// ارسال کاربر به صفحه درگاه پرداخت سامان به همراه توکن
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private RedirectResult RedirectToIpg(string token)
        {
            return Redirect($"{_samanPaymentConfiguration.RedirectToIpgUrl}{token}");
        }

        /// <summary>
        /// برای تایید تراکنش این متد را با پارمتر
        /// SendTransactionTypeEnum.VerifyTransaction
        /// صدامی زنیم و
        /// برای بازگشت به حساب خریدار در صورت رخ دادن خطا این متد را با ارسال پامتر
        /// SendTransactionTypeEnum.Reverse
        /// صدا می زنیم
        /// </summary>
        /// <param name="terminalNumber"></param>
        /// <param name="refNumber"></param>
        /// <param name="sendTransactionType">نوع ارسال تراکنش: تایید تراکنش یا بازگشت به حساب</param>
        /// <returns></returns>
        private async Task<TransResponseModel> SendTransactionAsync(
            long terminalNumber,
            string refNumber,
            SendTransactionTypeEnum sendTransactionType
            )
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var verifyMerchantTransModel = new TransRequestModel
            {
                TerminalNumber = terminalNumber,
                RefNum = refNumber
            };

            var client = new HttpClient(httpHandler);
            string requestUrl;
            // درخواست ارسال تایید تراکنش
            if (sendTransactionType == SendTransactionTypeEnum.VerifyTransaction)
            {
                requestUrl = _samanPaymentConfiguration.VerifyTransactionUrl;
            }
            // درخواست بازگشت به حساب کاربر
            else
            {
                requestUrl = _samanPaymentConfiguration.ReverseTransactionUrl;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            var data = JsonSerializer.Serialize(verifyMerchantTransModel);

            request.Content = new StringContent(data, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var verifyMerchantTransResponseModel = JsonSerializer.Deserialize<TransResponseModel>(await response.Content.ReadAsStringAsync());

                if (verifyMerchantTransResponseModel != null)
                {
                    return verifyMerchantTransResponseModel;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return null;
        }


        #endregion
    }
}
