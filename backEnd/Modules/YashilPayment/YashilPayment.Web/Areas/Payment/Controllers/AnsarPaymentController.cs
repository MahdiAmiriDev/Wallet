using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core.Configurations;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Enums;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.Implementations;
using YashilUserManagement.Core.Services;

namespace YashilPayment.Web.Areas.Payment.Controllers
{
    [Authorize]
    public class AnsarPaymentController : BaseAnsarPaymentController<object>
    {
        private readonly IAnsarPaymentConfiguration _ansarPaymentConfiguration;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly PaymentTransactionUtil _paymentTransactionUtil;
        private readonly IApplicationService _applicationService;
        private readonly IHostEnvironment _environment;


        public AnsarPaymentController(ApplicationConfiguration applicationConfiguration, IMapper mapper, IYashilLogger<object> logger, IPermissionService permissionService, IAnsarPaymentConfiguration ansarPaymentConfiguration, IOrderService orderService, IPaymentService paymentService, PaymentTransactionUtil paymentTransactionUtil, IApplicationService applicationService, IHostEnvironment environment) : base(applicationConfiguration, mapper, logger, permissionService, ansarPaymentConfiguration, environment)
        {
            _ansarPaymentConfiguration = ansarPaymentConfiguration;
            _orderService = orderService;
            _paymentService = paymentService;
            _paymentTransactionUtil = paymentTransactionUtil;
            _applicationService = applicationService;
            _environment = environment;
        }

        public async Task<IActionResult> Index(int? oId)
        {
            if (!oId.HasValue)
            {
                return RedirectToAction("Index", "Error");
            }

            var order = await _orderService.GetAsync(oId.Value, true);
            if (order == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var paymentInformation = await _orderService.CreateNewPaymentAndReturnAsync(
                oId.Value,
                PaymentMethodTypesEnum.Ansar,
                _ansarPaymentConfiguration.MID,
                _ansarPaymentConfiguration.TerminalId);

            if (paymentInformation != null && paymentInformation.Total.HasValue && paymentInformation.Total.Value != 0)
            {
                var amount = decimal.ToInt32(paymentInformation.Total.Value).ToString();
                var resNumber = _orderService.GetReservationNumber(oId.Value, paymentInformation.PaymentId);

                var token = await GenerateTokenAsync(amount, resNumber, order.RecipientMobileNumber);
                if (!string.IsNullOrEmpty(token))
                {
                    return RedirectToIpg(token);
                }
                else
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                        "Payment", $"خطا در ساخت توکن برای سفارش با کد: {resNumber}", null);
                }
            }
            return RedirectToAction("Index", "Error");
        }

        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaymentTransactionAsync()
        {
            var ansarPaymentVerifyViewModel = new VerifyPaymentViewModel
            {
                PaymentMethodType = PaymentMethodTypesEnum.Ansar
            };
            //check parameter come back from IPG page
            if (RequestUnpack(out var transactionState))
            {
                var resNumber = Request.Form[AnsarResultParam.ResNum];
                var payment = await _paymentService.GetByReservationNumberIncludeOrderAsync(resNumber);

                if (payment == null)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                        "Payment", $"سفارشی با این کد یافت نشد: {resNumber}", null);
                    return RedirectToAction("Index", "Error");
                }

                var application = await _applicationService.GetAsync(payment.ApplicationId, true);

                ansarPaymentVerifyViewModel.OrderListUrl = application.Url + "/Order";

                if (_paymentTransactionUtil.PaymentTransactionStatus.ContainsKey(resNumber))
                {
                    ansarPaymentVerifyViewModel.Message = "کاربر گرامی خواهشمند است جهت اطلاع از وضعیت پرداخت های خود از سایت پذرنده مجددا اقدام نمایید";
                    // تراکنش را به حالت پردازش می برد تا در صورتی که دوباره فراخوانی شود، پردازش مجدد انجام نشود 
                    return View(ansarPaymentVerifyViewModel);
                }
                else
                {
                    var resNum = Request.Form[AnsarResultParam.ResNum];
                    // دریافت CustomerRefNum  از اطلاعات ارسالی بانک
                    // این پارامتر بسیار مهم است و هنگام تایید پرداخت ها توسط مالی مورد استفاده قرار میگیرد
                    var customerRefNum = Request.Form[AnsarResultParam.CustomerRefNum];
                    // دریافت شناسه خرید دارنده کارت از اطلاعات ارسال شده توسط بانک
                    var goodReferenceId = Request.Form[AnsarResultParam.GoodReferenceId];
                    // دریافت شناسه خرید فروشنده از اطلاعات ارسال شده توسط بانک
                    var merchantData = Request.Form[AnsarResultParam.MerchantData];
                    _paymentTransactionUtil.PaymentTransactionStatus.GetOrAdd(resNum, true);
                    ansarPaymentVerifyViewModel.ResNum = resNum;

                    // در صورتی که رکورد پرداخت قبلا تایید شده باشد، دوباره آن را بروز رسانی نمی کنیم
                    // این حالت زمانی اتفاق می افتد که متد VerifyPaymentTransactionAsync
                    //بیش از یک بار با کد ResNum 
                    // فراخوانی شده باشد
                    if (payment.PaidDateUtc.HasValue)
                    {
                        if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                        {
                            ansarPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                        }
                        else
                        {
                            ansarPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                            ansarPaymentVerifyViewModel.Message = payment.AuthorizationTransactionResult;
                        }
                    }
                    else
                    {
                        ansarPaymentVerifyViewModel.OrderNumber = payment.Order.Title;
                        // check transaction was success full or not
                        if (transactionState.Equals(AnsarTransactionState.OK))
                        {
                            // try to get parameters receive from IPG
                            ansarPaymentVerifyViewModel.State = transactionState;
                            if (!string.IsNullOrEmpty(resNumber))
                            {
                                try
                                {

                                }
                                catch (Exception ex)
                                {
                                    _logger.InsertLog(LogLevel.Error,
                                        (int)Events.PaymentException, "Payment", "", ex);
                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                                           PaymentMethodTypesEnum.Ansar,
                                           "بروز خطا قبل از تایید در بانک انصار -- tosanIpgwsClient.login(req)", "", customerRefNum, goodReferenceId, merchantData, true);
                                    return RedirectToAction("Index", "Error");
                                }
                                //login

                                var verifyRequestList = new string[1]; // Array of size 1, filled with nulls.

                                try
                                {
                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                            PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway,
                                            PaymentMethodTypesEnum.Ansar, ansarPaymentVerifyViewModel.Message,
                                            ansarPaymentVerifyViewModel.RefNum, customerRefNum, goodReferenceId, merchantData, true);
                                    ansarPaymentVerifyViewModel.PaymentStatusType =
                                        PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                                    // fill the model

                                    var token = Request.Form[AnsarResultParam.Token];
                                    var refNum = Request.Form[AnsarResultParam.RefNum];
                                    var host = ApplicationConfiguration.BaseUrl
                                        .Replace("https://", "")
                                        .Replace("/", "");
                                    var res = await VerifyMerchantTransAsync(token, refNum,host);

                                    if (!res)
                                    {
                                        await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                                            PaymentMethodTypesEnum.Ansar, "بروز خطا تایید در بانک انصار",
                                            ansarPaymentVerifyViewModel.RefNum, customerRefNum, goodReferenceId, merchantData, true);
                                        ansarPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                                    }
                                    else
                                    {
                                        ansarPaymentVerifyViewModel.RefNum = refNum;
                                        ansarPaymentVerifyViewModel.ResNum = Request.Form[AnsarRequestParam.ResNum];
                                        ansarPaymentVerifyViewModel.CustomerRefNum = customerRefNum;
                                        ansarPaymentVerifyViewModel.CardMaskPan =
                                            Request.Form[AnsarResultParam.CardMaskPan];
                                        ansarPaymentVerifyViewModel.Email = Request.Form[AnsarResultParam.EmailAddress];
                                        ansarPaymentVerifyViewModel.Mobile = Request.Form[AnsarResultParam.MobileNo];


                                        ansarPaymentVerifyViewModel.Message = string.Format(
                                            AnsarMessage.SuccessfulVerify, payment.TotalInt);
                                        try
                                        {
                                            await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                                                OrderStatusTypesEnum.Complete);
                                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                  PaymentStatusTypesEnum.Paid,
                                                  PaymentMethodTypesEnum.Ansar, ansarPaymentVerifyViewModel.Message,
                                                  ansarPaymentVerifyViewModel.RefNum, customerRefNum, goodReferenceId, merchantData, true);
                                            ansarPaymentVerifyViewModel.PaymentStatusType =
                                                PaymentStatusTypesEnum.Paid;
                                        }
                                        catch (Exception ex)
                                        {

                                            var message = "paymentId :" + payment.Id +
                                                          " ,PaymentMethodTypesEnum :" +
                                                          PaymentMethodTypesEnum.Ansar +
                                                          " ,PaymentStatusTypesEnum :" +
                                                          PaymentStatusTypesEnum.Paid +
                                                          " ,reference number:" + refNum + " " +
                                                          ",reservation number: " +
                                                          resNumber +
                                                          ", customerRefNum: " +
                                                          customerRefNum +
                                                          ", goodReferenceId: " +
                                                          goodReferenceId +
                                                          ", merchantData" +
                                                          merchantData +
                                                          ",message" + ansarPaymentVerifyViewModel.Message;
                                            _logger.InsertLog(LogLevel.Error,
                                                (int)Events.PaymentException, "Payment", message, ex);
                                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                      PaymentStatusTypesEnum.Unsuccessful, PaymentMethodTypesEnum.Ansar,
                                                      message, "", customerRefNum, goodReferenceId, merchantData, true);
                                            return RedirectToAction("Index", "Error");
                                        }


                                    }


                                }
                                catch (Exception ex)
                                {
                                    _logger.InsertLog(LogLevel.Error,
                                        (int)Events.PaymentException, "Payment", "", ex);
                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                                            PaymentMethodTypesEnum.Ansar, "بروز خطا قبل از تایید در بانک انصار", "", customerRefNum, goodReferenceId, merchantData, true);
                                    return RedirectToAction("Index", "Error");
                                }
                            }
                        }
                        else
                        {
                            // fill the result of unsuccessful transaction
                            ansarPaymentVerifyViewModel.ResNum = resNumber;
                            ansarPaymentVerifyViewModel.State = transactionState;
                            ansarPaymentVerifyViewModel.Message = "انصراف کاربر از پرداخت";
                            await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                                  PaymentMethodTypesEnum.Ansar, "انصراف کاربر از پرداخت",
                                  ansarPaymentVerifyViewModel.RefNum, customerRefNum, goodReferenceId, merchantData,
                                  true);
                            ansarPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Cancel;
                        }
                    }
                    return View(ansarPaymentVerifyViewModel);
                }
            }

            return RedirectToAction("Index", "Error");
        }
        
    }
}