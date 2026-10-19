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
    public class SaderatPaymentController : BaseSaderatPaymentController<object>
    {
        private readonly ISaderatPaymentConfiguration _saderatPaymentConfiguration;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly PaymentTransactionUtil _paymentTransactionUtil;
        private readonly IApplicationService _applicationService;

        public SaderatPaymentController(ApplicationConfiguration applicationConfiguration,
            IMapper mapper, IYashilLogger<object> logger, IPermissionService permissionService,
            IOrderService orderService, IPaymentService paymentService,
            PaymentTransactionUtil paymentTransactionUtil, IApplicationService applicationService,
            ISaderatPaymentConfiguration saderatPaymentConfiguration, IHttpContextAccessor httpContextAccessor)
            : base(applicationConfiguration, mapper, logger, permissionService, saderatPaymentConfiguration,
                httpContextAccessor)
        {
            _saderatPaymentConfiguration = saderatPaymentConfiguration;
            _orderService = orderService;
            _paymentService = paymentService;
            _applicationService = applicationService;
            _paymentTransactionUtil = paymentTransactionUtil;
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
                oId.Value, PaymentMethodTypesEnum.Saderat, _saderatPaymentConfiguration.MID, _saderatPaymentConfiguration.TerminalId.ToString());

            if (paymentInformation != null && paymentInformation.Total.HasValue && paymentInformation.Total.Value != 0)
            {
                var amount = (long)paymentInformation.Total.Value;

                var reservationNumber = _orderService.GetReservationNumber(oId.Value, paymentInformation.PaymentId);

                var token = await GenerateTokenAsync(amount, reservationNumber);

                if (!string.IsNullOrEmpty(token))
                {
                    await RedirectToIpg(token, (long)Convert.ToDouble(_saderatPaymentConfiguration.TerminalId));

                }
                else
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", $"خطا در ساخت توکن برای سفارش با کد: {reservationNumber}", null);
                }

            }
            else
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", $"خطا اطلاعات پرداخت مرتبط با شماره سفارش یافت نشد: {oId.Value}", null);
            }
            return RedirectToAction("Index", "Error");
        }

        /// <summary>
        /// متد تایید تراکنش
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaymentTransactionAsync()
        {
            var saderatPaymentVerifyViewModel = new VerifyPaymentViewModel
            {
                PaymentMethodType = PaymentMethodTypesEnum.Saderat
            };

            // check parameter of comec back from Saderat IPG page
            if (RequestUnpack(out var transactionState))
            {
                var resNumber = Request.Form[SaderatResultParam.InvoiceId];
                var payment = await _paymentService.GetByReservationNumberIncludeOrderAsync(resNumber);

                if (payment == null)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", $"سفارشی با این کد یافت نشد: {resNumber}", null);
                    return RedirectToAction("Index", "Error");
                }

                var application = await _applicationService.GetAsync(payment.ApplicationId, true);

                saderatPaymentVerifyViewModel.OrderListUrl = application.Url + "/Order";

                if (_paymentTransactionUtil.PaymentTransactionStatus.ContainsKey(resNumber))
                {
                    saderatPaymentVerifyViewModel.Message = "کاربر گرامی خواهشمند است جهت اطلاع از وضعیت پرداخت های خود از سایت پذیرنده مجددا اقدام نمایید";
                    // تراکنش را به حالت پردازش می برد تا در صورتی که دوباره فراخوانی شود، پردازش مجدد انجام نشود 
                    return View(saderatPaymentVerifyViewModel);
                }
                else
                {
                    var customerRefNum = Request.Form[SaderatResultParam.Rrn];

                    var goodReferenceId = Request.Form[SaderatResultParam.DigitalReceipt];

                    _paymentTransactionUtil.PaymentTransactionStatus.GetOrAdd(resNumber, true);

                    saderatPaymentVerifyViewModel.ResNum = resNumber;


                    if (payment.PaidDateUtc.HasValue)
                    {
                        if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                        {
                            saderatPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                        }
                        else
                        {
                            saderatPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                            saderatPaymentVerifyViewModel.Message = payment.AuthorizationTransactionResult;
                        }
                    }
                    else
                    {
                        saderatPaymentVerifyViewModel.OrderNumber = payment.Order.Title;

                        if (transactionState.Equals(SaderatTransactionState.Success))
                        {
                            saderatPaymentVerifyViewModel.State = transactionState.ToString();

                            if (!string.IsNullOrEmpty(resNumber))
                            {
                                try
                                {

                                }
                                catch (Exception e)
                                {
                                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", "", e);

                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                                                   PaymentStatusTypesEnum.Unsuccessful,
                                                                                   PaymentMethodTypesEnum.Saderat,
                                           "بروز خطا قبل از تایید در بانک صادرات -- sepehrIpgwsClient.login(req)",
                                           "", customerRefNum, goodReferenceId, " ", true);

                                    return RedirectToAction("Index", "Error");

                                }

                                var verifyRequestList = new string[1];

                                try
                                {
                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                            PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway,
                                            PaymentMethodTypesEnum.Saderat, saderatPaymentVerifyViewModel.Message,
                                            saderatPaymentVerifyViewModel.RefNum, customerRefNum, goodReferenceId, " ", true);
                                    saderatPaymentVerifyViewModel.PaymentStatusType =
                                        PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                                    // fill the model

                                    var digitalReceipt = Request.Form[SaderatResultParam.DigitalReceipt].ToString();

                                    string terminalId = Request.Form[SaderatResultParam.TerminalId].ToString();

                                    long tId = (long)Convert.ToDouble(terminalId);

                                    var host = ApplicationConfiguration.BaseUrl.Replace("https://", "").Replace("/", "");

                                    var res = await VerifyMerchantTransAsync(digitalReceipt, tId, host);

                                    if (!res)
                                    {
                                        await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                                            PaymentMethodTypesEnum.Saderat, "بروز خطا تایید در بانک صادرات",
                                            saderatPaymentVerifyViewModel.RefNum, customerRefNum, goodReferenceId, " ", true);
                                        saderatPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                                    }
                                    else
                                    {
                                        var traceNumber = Request.Form[SaderatResultParam.TracenNmber];
                                        saderatPaymentVerifyViewModel.RefNum = traceNumber;
                                        saderatPaymentVerifyViewModel.ResNum = Request.Form[SaderatResultParam.InvoiceId];
                                        saderatPaymentVerifyViewModel.CustomerRefNum = customerRefNum;
                                        saderatPaymentVerifyViewModel.CardMaskPan = Request.Form[SaderatResultParam.CardNumber];


                                        saderatPaymentVerifyViewModel.Message = string.Format(SaderatMessage.SuccessfulVerify, payment.TotalInt);

                                        try
                                        {
                                            await _orderService.UpdateOrderStatusAsync(payment.OrderId, OrderStatusTypesEnum.Complete);

                                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                                  PaymentStatusTypesEnum.Paid,
                                                  PaymentMethodTypesEnum.Saderat, saderatPaymentVerifyViewModel.Message,
                                                  saderatPaymentVerifyViewModel.RefNum, customerRefNum, goodReferenceId, " ", true);

                                            saderatPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                                        }
                                        catch (Exception e)
                                        {

                                            var message = "paymentId :" + payment.Id +
                                                          " ,PaymentMethodTypesEnum :" +
                                                          PaymentMethodTypesEnum.Saderat +
                                                          " ,PaymentStatusTypesEnum :" +
                                                          PaymentStatusTypesEnum.Paid +
                                                          " ,reference number:" + traceNumber + " " +
                                                          ",reservation number: " +
                                                          resNumber +
                                                          ", customerRefNum: " +
                                                          customerRefNum +
                                                          ", goodReferenceId: " +
                                                          goodReferenceId +
                                                          ", merchantData" +
                                                          " " +
                                                          ",message" + saderatPaymentVerifyViewModel.Message;

                                            _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", message, e);

                                            await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                                                        PaymentMethodTypesEnum.Saderat,
                                                      message, "", customerRefNum, goodReferenceId, " ", true);

                                            return RedirectToAction("Index", "Error");
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", "", e);

                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                                            PaymentMethodTypesEnum.Saderat, "بروز خطا قبل از تایید در بانک صادرات", "", customerRefNum, goodReferenceId, " ", true);

                                    return RedirectToAction("Index", "Error");
                                }
                            }
                        }
                        else
                        {
                            // fill the result of unsuccessful transaction
                            saderatPaymentVerifyViewModel.ResNum = resNumber;
                            saderatPaymentVerifyViewModel.State = transactionState.ToString();
                            saderatPaymentVerifyViewModel.Message = "انصراف کاربر از پرداخت";
                            await _paymentService.UpdatePaymentStatusAsync(payment.Id, PaymentStatusTypesEnum.Unsuccessful,
                                  PaymentMethodTypesEnum.Saderat, "انصراف کاربر از پرداخت",
                                  saderatPaymentVerifyViewModel.RefNum, customerRefNum, goodReferenceId, " ",
                                  true);
                            saderatPaymentVerifyViewModel.PaymentStatusType = PaymentStatusTypesEnum.Cancel;
                        }

                    }
                    return View(saderatPaymentVerifyViewModel);
                }
            }
            return RedirectToAction("Index", "Error");
        }
    }
}
