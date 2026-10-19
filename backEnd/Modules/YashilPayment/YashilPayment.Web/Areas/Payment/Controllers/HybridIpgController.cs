using System.Globalization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.CustomDtos.Payment;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Enums;
using YashilCommerce.Infrastructure.Implementations;
using YashilPayment.Core.Services;
using YashilPayment.Infrastructure.Helper;
using YashilPayment.Web.Areas.Payment.ViewModels;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using Yashil.Core.Protos;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Services;
using YashilUserManagement.Core.Services;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace YashilPayment.Web.Areas.Payment.Controllers;

public class HybridIpgController : BaseAnsarPaymentController<HybridPaymentRequest>
{
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly IMapper _mapper;
    private readonly IHybridPaymentRequestService _hybridPaymentRequestService;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IHybridPaymentService _hybridPaymentService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PaymentTransactionUtil _paymentTransactionUtil;
    private readonly IHekmatPaymentConfiguration _hekmatPaymentConfiguration;
    private readonly ICommonBaseDataService _commonBaseDataService;
    private readonly IHostEnvironment _environment;
    private readonly IIranKishPaymentService _iranKishPaymentService;

    public HybridIpgController(ApplicationConfiguration applicationConfiguration, IMapper mapper,
        IYashilLogger<HybridPaymentRequest> logger, IPermissionService permissionService,
        IAnsarPaymentConfiguration ansarPaymentConfiguration,
        IHybridPaymentRequestService hybridPaymentRequestService, IPaymentGatewayService paymentGatewayService,
        IHybridPaymentService hybridPaymentService, IHttpContextAccessor httpContextAccessor,
        PaymentTransactionUtil paymentTransactionUtil, IHekmatPaymentConfiguration hekmatPaymentConfiguration,
        ICommonBaseDataService commonBaseDataService,
        IAppConfigService appConfigService,
        ITaxCategoryService taxCategoryService, IHostEnvironment environment, IIranKishPaymentService iranKishPaymentService)
        : base(applicationConfiguration, mapper, logger, permissionService, ansarPaymentConfiguration, environment)
    {
        _applicationConfiguration = applicationConfiguration;
        _mapper = mapper;
        _hybridPaymentRequestService = hybridPaymentRequestService;
        _paymentGatewayService = paymentGatewayService;
        _hybridPaymentService = hybridPaymentService;
        _httpContextAccessor = httpContextAccessor;
        _paymentTransactionUtil = paymentTransactionUtil;
        _hekmatPaymentConfiguration = hekmatPaymentConfiguration;
        _commonBaseDataService = commonBaseDataService;
        _environment = environment;
        _iranKishPaymentService = iranKishPaymentService;
    }

    /// <summary>
    /// پذیرنده توکن را به صفحه اصلی درگاه پرداخت ترکیبی ارسال و کاربر را به این صفحه هدایت می نماید
    /// 
    /// </summary>
    /// <param name="encryptedToken">توکن ارتباطی بین پذیرنده و دارنده</param>
    /// <returns></returns>
    public async Task<IActionResult> Index([FromForm] string encryptedToken)
    {
        var resultModel = new ResultModel
        {
            Succeeded = false,
            Status = 0
        };
        // استخراج اطلاعات توکن و پذیرنده و بررسی معتبر بودن توکن
        (resultModel.Succeeded, resultModel.Message, resultModel.Status, var ipgToken, var terminalInfo) =
            await _paymentGatewayService.ValidateAndExtractEncryptedTokenData(encryptedToken);
        if (!resultModel.Succeeded || ipgToken == null || terminalInfo == null)
        {
            return View("IpgError", resultModel);
        }

        var (remainingHekmatPrice, remainingCashPrice, hybridPaymentRequest) =
            await _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(
                ipgToken.MerchantId,
                ipgToken.TerminalId,
                ipgToken.ReservationNumber,
                ipgToken.UseFormula,
                _applicationConfiguration.ApplicationId
            );
        //  اگر قبلا این اطلاعات وجود داشته باشد باید اطلاعات را بررسی کنیم که با قبلی یکی باشد در این صورت باید پیام توکن معتبر نمی باشد را بدهیم 
        if (hybridPaymentRequest != null &&
            (hybridPaymentRequest.AdditionalData != ipgToken.AdditionalData ||
             // نباید درصد را مقایسه کنیم زیرا ممکن است درصد توسط پذیرنده ارسال نشده باشد و کاربر درصد را مشخص کرده باشد 
             // آنگاه پذیرنده از درصد با خبر نیست
             //hybridPaymentRequest.CashPaymentPercentage != ipgToken.CashPaymentPercentage ||
             hybridPaymentRequest.BasePrice != ipgToken.Amount ||
             hybridPaymentRequest.InstallmentCount != ipgToken.InstallmentCount))
        {
            resultModel.Message = "توکن معتبر نمی باشد";
            resultModel.Status = 1;
            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                $"requested token is not valid with old hybridPaymentRequest data with token ={encryptedToken}", null);
            return View("IpgError", resultModel);
        }

        var taxPercentage = await _hybridPaymentService.GetTaxPercentageAsync();
        if (!taxPercentage.HasValue)
        {
            resultModel.Message = "شناسه دسته بندی مالیات در تنظیمات به درستی درج نشده است.";
            resultModel.Status = 1;
            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                $"YashilPayment.Tax.DefaultTaxCategoryId key not int Value Or Not Exists In AppConfig", null);
            return View("IpgError", resultModel);
        }

        // مقدار دهی ویومدل
        var hybridIpgViewModel = new HybridIpgViewModel()
        {
            PaymentRequestToken = encryptedToken,
            BasePrice = ipgToken.Amount,
            InstallmentCount = ipgToken.InstallmentCount,
            MerchantName = terminalInfo.MerchantName,
            MerchantUrl = terminalInfo.Url,
            TerminalId = terminalInfo.Id,
            AdditionalDate = ipgToken.AdditionalData,
            CallBackUrl = ipgToken.ReturnUrl,
            ReservationNumber = ipgToken.ReservationNumber,
            MerchantId = ipgToken.MerchantId,
            // چون پذیرنده ممکن است درصد پرداخت نقدی را مشخص نکرده باشد
            // و در صورتی که درخواست قبلی در سامانه وجود داشته باشد پس درصد را از درخواست قبلی میخوانیم

            CashPaymentPercentage = hybridPaymentRequest?.CashPaymentPercentage ?? ipgToken.CashPaymentPercentage,
            // چون ممکن است برای پرداخت های بعدی امده باشد ممکن است در این مدت درصد پذیرنده تغییر کرده باشد و برای جلوگیری از بهم خوردگی محاسبات باید درصدهای قبلی درخواست را در نظر بگیریم
            SupplierWage = hybridPaymentRequest?.SupplierWage ?? terminalInfo.SupplierWage,
            InstituteWage = hybridPaymentRequest?.InstituteWage ?? terminalInfo.InstituteWage,
            CashSupplierWage = hybridPaymentRequest?.CashSupplierWage ?? terminalInfo.CashSupplierWage,
            CashInstituteWage = hybridPaymentRequest?.CashInstituteWage ?? terminalInfo.CashInstituteWage,
            Tax = hybridPaymentRequest?.Tax ?? (!ipgToken.IsTaxExempt ? Convert.ToInt32(taxPercentage.Value) : 0),
            ShippingAmount = hybridPaymentRequest?.ShippingAmount ?? ipgToken.ShippingAmount

        };
        // اگر درصد پرداخت مشخص باشد که مبلغ پرداخت نقدی و حداقل حداکثر را بر اساس آن محاسبه میکنیم
        if (hybridIpgViewModel.CashPaymentPercentage.HasValue)
        {

            hybridIpgViewModel.CashPaymentPrice = _hybridPaymentRequestService.CalculateCashPrice(ipgToken.Amount,
                hybridIpgViewModel.CashPaymentPercentage.Value,
                hybridIpgViewModel.CashInstituteWage,
                hybridIpgViewModel.Tax,
                hybridIpgViewModel.ShippingAmount, ipgToken.UseFormula
                );
            // hybridIpgViewModel.MaxCashPaymentPrice = hybridIpgViewModel.CashPaymentPrice.Value;

            hybridIpgViewModel.HekmatPaymentPrice = _hybridPaymentRequestService.CalculateHekmatPrice(ipgToken.Amount,
                hybridIpgViewModel.CashPaymentPercentage.Value,
                hybridIpgViewModel.InstallmentCount,
                hybridIpgViewModel.InstituteWage,
                hybridIpgViewModel.SupplierWage,
                hybridIpgViewModel.Tax,
                hybridIpgViewModel.ShippingAmount,
                ipgToken.UseFormula
                );
            hybridIpgViewModel.MaxHekmatPaymentPrice = hybridIpgViewModel.HekmatPaymentPrice.Value;

            if (hybridPaymentRequest != null)
            {
                hybridIpgViewModel.HybridPaymentRequestId = hybridPaymentRequest.Id;
                hybridIpgViewModel.HybridPaymentRequestStatus = hybridPaymentRequest.HybridPaymentRequestStatusType;
                var hybridPayments =
                    await _hybridPaymentService.GetByCustomFilter(hybridPaymentRequest.Id, null, null,
                            _applicationConfiguration.ApplicationId)
                        .ProjectTo<HybridPaymentIpgListViewModel>(_mapper.ConfigurationProvider).ToListAsync();
                hybridIpgViewModel.HybridPayments = hybridPayments;

                hybridIpgViewModel.RemainingCashPaymentPrice = remainingCashPrice;
                hybridIpgViewModel.RemainingHekmatPaymentPrice = remainingHekmatPrice;
            }
            else
            {
                hybridIpgViewModel.RemainingCashPaymentPrice = hybridIpgViewModel.CashPaymentPrice;
                hybridIpgViewModel.RemainingHekmatPaymentPrice = hybridIpgViewModel.HekmatPaymentPrice;
            }
        }
        // در غیر این صورت بر اساس 100 درصد پرداخت نقدی و 0 درصد پرداخت اقساطی حداکثر قیمت ها را محاسبه میکنیم
        else
        {
            hybridIpgViewModel.MaxCashPaymentPrice = _hybridPaymentRequestService
                .CalculateCashPrice(ipgToken.Amount,
                100,
                hybridIpgViewModel.CashInstituteWage,
                hybridIpgViewModel.Tax,
                hybridIpgViewModel.ShippingAmount, ipgToken.UseFormula);
            hybridIpgViewModel.MaxHekmatPaymentPrice = _hybridPaymentRequestService
                .CalculateHekmatPrice(
                ipgToken.Amount,
                0,
                hybridIpgViewModel.InstallmentCount,
                hybridIpgViewModel.InstituteWage,
                hybridIpgViewModel.SupplierWage,
                hybridIpgViewModel.Tax,
                hybridIpgViewModel.ShippingAmount, ipgToken.UseFormula);
        }

        if (hybridIpgViewModel.HybridPayments != null)
        {
            hybridIpgViewModel.HasAnyPaymentNotDetermined = hybridIpgViewModel.HybridPayments.Any(x =>
                x.HybridPaymentStatusType == (int)HybridPaymentStatusTypeEnum.SendToPaymentGateway
                || x.HybridPaymentStatusType == (int)HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway);
        }


        hybridIpgViewModel.AllowPayHekmat = hybridIpgViewModel.CashPaymentPercentage.HasValue &&
                                            !hybridIpgViewModel.HasAnyPaymentNotDetermined &&
                                             hybridIpgViewModel.RemainingHekmatPaymentPrice > 0 &&
                                             (!hybridIpgViewModel.HybridPaymentRequestId.HasValue ||
                                                (hybridIpgViewModel.HybridPaymentRequestId.HasValue &&
                                                 (hybridIpgViewModel.HybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.InitialRegistration ||
                                                  hybridIpgViewModel.HybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.IncompletePayment)));

        hybridIpgViewModel.AllowPayCash = hybridIpgViewModel.CashPaymentPercentage.HasValue &&
                                           !hybridIpgViewModel.AllowPayHekmat &&
                                           !hybridIpgViewModel.HasAnyPaymentNotDetermined &&
                                            hybridIpgViewModel.RemainingCashPaymentPrice > 0 &&
                                            (!hybridIpgViewModel.HybridPaymentRequestId.HasValue ||
                                              (hybridIpgViewModel.HybridPaymentRequestId.HasValue &&
                                               (hybridIpgViewModel.HybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.InitialRegistration ||
                                                hybridIpgViewModel.HybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.IncompletePayment)));

        hybridIpgViewModel.AmountOfEachInstallment = hybridIpgViewModel.RemainingHekmatPaymentPrice / hybridIpgViewModel.InstallmentCount;
        return View(hybridIpgViewModel);
    }


    /// <summary>
    /// در صفحه پرداخت ترکیبی وقتی کاربر دکمه پرداخت اقساطی را میزند این متد صدا زده میشود
    /// </summary>
    /// <param name="hybridPurchaseRequestViewModel"></param>
    /// <returns></returns>
    [HttpPost("HekmatPurchase")]
    public async Task<IActionResult> HekmatPurchaseAsync(HybridPurchaseRequestViewModel hybridPurchaseRequestViewModel)
    {
        var resultModel = new ResultModel
        {
            Succeeded = false,
            Status = 0
        };
        (resultModel.Succeeded, resultModel.Message, resultModel.Status, var hybridPayment, var ipgToken) =
            await _paymentGatewayService.ValidateAndCreateHekmatHybridPaymentAsync(
                hybridPurchaseRequestViewModel.PaymentRequestToken,
                hybridPurchaseRequestViewModel.CashPaymentPercentage);

        if (!resultModel.Succeeded)
        {
            return View("IpgError", resultModel);
        }
        // await CreateHekmatTokenAndRedirectToHekmatIpgAsync(ipgToken, hybridPayment.Id, hybridPayment.Price);

        var backUrl = $"{_applicationConfiguration.BaseUrl}hybridIpg/VerifyHekmatPaymentTransaction";


        //درصورتی که از درگاه نقدی اقساطی طرح باقر العلوم اقدام به پرداخت
        //کرده باشیم مقدار useFormula = true
        //بوده بنابراین لازم است که این مقدار را پس از این که کاربر دکمه ی پرداخت اقساطی را زد
        //مجددا داشته باشیم به دلیل این که در این حالت 
        //payment درج نشده
        //و باید در مرحله ی آخر که کاربر پرداخت اقساطی موفق آمیز ثبت کرد
        //payment درچ کنیم در shp

        //ipgToken.UseFormula;

        var hekmatToken = PaymentHelper.CreateToken(ipgToken.IpAddress,
            ipgToken.TokenExpireDateTime,
            ipgToken.MerchantId,
            ipgToken.TerminalId,
            hybridPayment.Id.ToString(),
            ipgToken.LocalDateTime.ToString(CultureInfo.InvariantCulture), backUrl,
            hybridPayment.Price,
            ipgToken.InstallmentCount,
            ipgToken.CardNumber,
            ipgToken.AdditionalData, useFormula: ipgToken.UseFormula);
        var encryptedHekmatToken = CryptographyHelper.AesEncrypt(hekmatToken);

        var remotePost = new RemotePost(_httpContextAccessor)
        {
            Url = _hekmatPaymentConfiguration.IpgUrl
        };
        remotePost.Add("encryptedToken", encryptedHekmatToken);
        await remotePost.PostAsync();

        return View("IpgError", resultModel);
    }

    /// <summary>
    /// در صفحه پرداخت ترکیبی وقتی کاربر دکمه پرداخت نقدی را میزند این متد صدا زده میشود
    /// </summary>
    /// <param name="hybridPurchaseRequestViewModel"></param>
    /// <returns></returns>
    [HttpPost("CashPurchase")]
    public async Task<IActionResult> CashPurchaseAsync(HybridPurchaseRequestViewModel hybridPurchaseRequestViewModel)
    {
        var resultModel = new ResultModel
        {
            Succeeded = false,
            Status = 0
        };
        (resultModel.Succeeded, resultModel.Message, resultModel.Status, var hybridPayment, _) =
            await _paymentGatewayService.ValidateAndCreateCashHybridPaymentAsync(
                hybridPurchaseRequestViewModel.PaymentRequestToken,
                hybridPurchaseRequestViewModel.CashPaymentPercentage);

        if (!resultModel.Succeeded)
        {
            return View("IpgError", resultModel);
        }

        var token = await _iranKishPaymentService.GenerateTokenAsync(decimal.ToInt32(hybridPayment.Price),
            hybridPayment.ReservationNumber);

        if (!string.IsNullOrEmpty(token))
        {
            var remotePost = new RemotePost(_httpContextAccessor);
            remotePost.Url = "https://ikc.shaparak.ir/iuiv3/IPG/Index/";
            remotePost.Add("tokenIdentity", token);
            await remotePost.PostAsync();
        }

        resultModel.Message = "خطا در برقراری ارتباط با درگاه پرداخت";
        _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
            nameof(HybridPayment), $"خطا در ساخت توکن برای سفارش با کد: {hybridPayment.ReservationNumber}", null);


        return View("IpgError", resultModel);

    }


    /// <summary>
    /// هنگامی که کاربر پرداخت اقساطی را در پرداخت ترکیبی انجام داد و از صفحه پرداخت اقساطی بازگشت این متد صدا زده میشود
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> VerifyHekmatPaymentTransactionAsync()
    {
        // در هنگام فراخوانی این متد باید اطلاعاتی به صورت پست ارسال شده باشد
        // چک میکنیم که حتما reservationNumber و Status ارسال شده باشند
        if (Request.HasFormContentType && !string.IsNullOrEmpty(
                Request.Form[HekmatRequestParams.ReservationNumber]) &&
            !string.IsNullOrEmpty(Request.Form[HekmatRequestParams.Status]))
        {
            var hekmatResult = new VerifyHybridPaymentViewModel
            {
                // اگر سفارش موفقیت امیز باشد refrenceNumber  حتما مقدار دارد
                RefrenceNumber = Request.Form[HekmatRequestParams.RefrenceNumber],
                // شناسه پرداخت سایت است
                ReservationNumber = Request.Form[HekmatRequestParams.ReservationNumber],
                // کد وضعیت بازگشتی از درگاه است حالت 0 1 99 دارد
                StateCode = Request.Form[HekmatRequestParams.Status],
                PaymentMethodType = PaymentMethodTypesEnum.Hekmat,
                Message = Request.Form[HekmatRequestParams.Message],
                CallBackUrl = Request.Form[HekmatRequestParams.CallBackUrl],
                Amount = Request.Form[HekmatRequestParams.Amount],
                BasePrice = Request.Form[HekmatRequestParams.BasePrice],
                UseFormula = Request.Form[HekmatRequestParams.UseFormula]

            };
            // دریافت اطلاعات پرداخت
            var canConvertToInt = int.TryParse(hekmatResult.ReservationNumber, out int hybridPaymentId);

            if (!canConvertToInt)
            {
                return View("VerifyPayment", hekmatResult);
            }

            var hybridPaymentQuery = _hybridPaymentService.FindById(hybridPaymentId, true);

            var hybridPayment = await hybridPaymentQuery.ProjectTo<HybridPaymentIpgVerifyViewModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (hybridPayment != null)
            {
                // شماره سفارش
                hekmatResult.HybridPaymentRequestTitle = hybridPayment.HybridPaymentRequestTitle;
                hekmatResult.CallBackUrl = hybridPayment.HybridPaymentRequestCallBackUrl;


                // اگر وضعیت پرداخت در حال بررسی باشد
                //یعنی کاربر در صفحه درگاه اطلاعات را بصورت کامل پرکرده است و 
                // دکمه پرداخت را زده
                //و اطلاعات جهت بررسی توسط درگاه به سمت حکمت ارسال میشود و تا زمان بررسی پرداخت در این وضعیت باقی میماند
                // نکته مهم این است که پرداخت در حال بررسی در حکمت به معنای پرداخت  شده نمیباشد و درواقع تراکنش در صف بررسی میباشد
                // برای اطلاع از وضعیت پرداخت پس از طی مدت زمانی
                // متد checkTransaction  را فراخوانی میکنیم.
                if (!string.IsNullOrEmpty(hekmatResult.RefrenceNumber) && hekmatResult.StateCode ==
                    ((int)HekmatPurchaseResultStatusCodeEnum.Successful).ToString())
                {
                    await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                        HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway, PaymentMethodTypesEnum.Hekmat,
                        HekmatResultStatusMessage.Pending, hekmatResult.RefrenceNumber);

                    await _hybridPaymentRequestService.UpdateHybridPaymentRequestStatusAsync(
                        hybridPayment.HybridPaymentRequestId
                        , HybridPaymentRequestStatusTypeEnum.Processing, true);

                    hekmatResult.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway;
                    _logger.InsertLog(LogLevel.Information, (int)Events.Insert, nameof(Payment),
                        $"successful payment in HybridIpg Hekmat for processing for payment id ={hybridPayment.Id}",
                        null);
                }
                // اگر وضعیت پرداخت درحالت انصراف باشد یعنی کاربر در صفحه درگاه دکمه انصراف را زده است.
                else if (hekmatResult.StateCode == ((int)HekmatPurchaseResultStatusCodeEnum.Cancel).ToString())
                {
                    // در صورتی که وضعیت پرداخت منتظر پاسخ درگاه و پرداخت شده نباشد
                    // می توان وضعیت آن را به انصراف کاربر تغییر داد
                    if (hybridPayment.HybridPaymentStatusType !=
                        (int)HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway &&
                        hybridPayment.HybridPaymentStatusType !=
                        (int)HybridPaymentStatusTypeEnum.Paid)
                    {
                        hekmatResult.Message = HekmatResultStatusMessage.Cancel;
                        await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                            HybridPaymentStatusTypeEnum.Cancel,
                            PaymentMethodTypesEnum.Hekmat,
                            HekmatResultStatusMessage.Cancel,
                            hekmatResult.RefrenceNumber,
                            true);

                        await _hybridPaymentRequestService.UpdateHybridPaymentRequestStatusAsync(
                            hybridPayment.HybridPaymentRequestId
                            , HybridPaymentRequestStatusTypeEnum.Canceled,
                            true);
                        hekmatResult.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Cancel;
                    }
                }

                //در پرداخت نقدی اقساطی طرح باقرالعلوم کاربرد دارد برای این که
                //مشخص شود از فرمول بخواند یا نه
                var useFormula = Convert.ToBoolean(hekmatResult.UseFormula ?? "true");

                var (_, _, hybridPaymentRequest) =
                    await _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(hybridPayment.HybridPaymentRequestId, useFormula);
                hekmatResult.HybridPaymentStatusResultModel = MapHybridPaymentRequestDtoToHybridPaymentStatusResultModel(hybridPaymentRequest);
                hekmatResult.HybridPaymentStatusResultModel.HekmatPaymentGatewayTransactionId = int.Parse(hekmatResult.RefrenceNumber);
                hekmatResult.HybridPaymentId = hybridPaymentId;
                return View("VerifyPayment", hekmatResult);
            }
            else
            {
                return View("VerifyPayment", hekmatResult);
            }
        }

        return View("IpgError", new ResultModel { Succeeded = false, Message = "بروز خطا", Status = 1 });
    }


    /// <summary>
    /// هنگامی که کاربر پرداخت اقساطی را در پرداخت ترکیبی انجام داد و از صفحه پرداخت اقساطی بازگشت این متد صدا زده میشود
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> VerifyAnsarPaymentTransactionAsync()
    {
        var ansarPaymentVerifyViewModel = new VerifyHybridPaymentViewModel
        {
            PaymentMethodType = PaymentMethodTypesEnum.Ansar
        };
        //check parameter come back from IPG page
        if (RequestUnpack(out var transactionState))
        {
            string? resNumber = Request.Form[AnsarResultParam.ResNum];

            if (string.IsNullOrEmpty(resNumber))
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"resNumber : {resNumber} is Empty Or Null", null);
                return View("IpgError",
                    new ResultModel { Succeeded = false, Message = "بروز خطا قبل از تایید در بانک انصار", Status = 1 });
            }

            var hybridPaymentQuery = _hybridPaymentService.FindByReservationNumber(resNumber, true);

            var hybridPayment = await hybridPaymentQuery
                .ProjectTo<HybridPaymentIpgVerifyViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();



            if (hybridPayment == null)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"سفارشی با این کد یافت نشد: {resNumber}", null);
                return View("IpgError",
                    new ResultModel { Succeeded = false, Message = "پرداختی یافت نشد", Status = 1 });
            }

            if (hybridPayment.Price.ToString() != Request.Form[AnsarResultParam.TransactionAmount])
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"عدم یکسان بودن مبلع پرداختی ثبت شده در درگاه با مبلغی که در hybridPayment ثبت شده است مبلغ درگاه برابر {Request.Form[AnsarResultParam.TransactionAmount]} و مبلغ hbridPayment برابر {hybridPayment.Price} است", null);

                return View("IpgError",
                    new ResultModel { Succeeded = false, Message = "عدم تطابق مبلغ پرداخت شده.", Status = 1 });
            }


            ansarPaymentVerifyViewModel.CallBackUrl = hybridPayment.HybridPaymentRequestCallBackUrl;

            if (_paymentTransactionUtil.PaymentTransactionStatus.ContainsKey(resNumber))
            {
                ansarPaymentVerifyViewModel.Message =
                    "کاربر گرامی خواهشمند است جهت اطلاع از وضعیت پرداخت از طریق سایت پذیرنده مجددا مراجعه نمایید";
                // تراکنش را به حالت پردازش می برد تا در صورتی که دوباره فراخوانی شود، پردازش مجدد انجام نشود 
                return View("VerifyPayment", ansarPaymentVerifyViewModel);
            }

            //باید همیشه مقدار داشته باشد و نال نباشد.
            /// <summary>
            /// آیا از فرمول استفاده کند برای محاسبه قیمت در
            /// پرداخت نقدی اقساطی طرح باقرالعلوم کاربرد دارد
            /// </summary>
            var useFormula = hybridPayment.UseFormula ?? true;


            // دریافت CustomerRefNum  از اطلاعات ارسالی بانک
            // این پارامتر بسیار مهم است و هنگام تایید پرداخت ها توسط مالی مورد استفاده قرار میگیرد
            var customerRefNum = Request.Form[AnsarResultParam.CustomerRefNum];
            // دریافت شناسه خرید دارنده کارت از اطلاعات ارسال شده توسط بانک
            var goodReferenceId = Request.Form[AnsarResultParam.GoodReferenceId];
            // دریافت شناسه خرید فروشنده از اطلاعات ارسال شده توسط بانک
            var merchantData = Request.Form[AnsarResultParam.MerchantData];
            _paymentTransactionUtil.PaymentTransactionStatus.GetOrAdd(resNumber, true);
            ansarPaymentVerifyViewModel.ReservationNumber = resNumber;

            // در صورتی که رکورد پرداخت قبلا تایید شده باشد، دوباره آن را بروز رسانی نمی کنیم
            // این حالت زمانی اتفاق می افتد که متد VerifyPaymentTransactionAsync
            //بیش از یک باز با کد ResNum 
            // فراخوانی شده باشد
            if (hybridPayment.PaidDateUtc.HasValue)
            {
                if (hybridPayment.HybridPaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                {
                    ansarPaymentVerifyViewModel.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Paid;
                }
                else
                {
                    ansarPaymentVerifyViewModel.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Unsuccessful;
                    ansarPaymentVerifyViewModel.Message = hybridPayment.AuthorizationTransactionResult;
                }
            }
            else
            {

                ansarPaymentVerifyViewModel.HybridPaymentRequestTitle = hybridPayment.HybridPaymentRequestTitle;
                // ansarPaymentVerifyViewModel.OrderNumber = hybridPayment.hybridPaymentRequest.Title;
                // check transaction was success full or not
                if (transactionState.Equals(AnsarTransactionState.OK))
                {
                    // try to get parameters receive from IPG
                    ansarPaymentVerifyViewModel.State = transactionState;
                    if (!string.IsNullOrEmpty(resNumber))
                    {
                        // try to login for verify transaction
                        // if not verify transaction it will be returned to customer account
                        try
                        {
                            await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway,
                                PaymentMethodTypesEnum.Ansar, ansarPaymentVerifyViewModel.Message,
                                ansarPaymentVerifyViewModel.RefrenceNumber, customerRefNum, goodReferenceId,
                                merchantData, true);

                            ansarPaymentVerifyViewModel.HybridPaymentStatusTypeEnum =
                                HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway;

                            // fill the model
                            string? token = Request.Form[AnsarResultParam.Token];
                            string? refNum = Request.Form[AnsarResultParam.RefNum];
                            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refNum))
                            {
                                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                                    "Payment", $"token : {token} is Empty Or Null || refNum : {refNum} is Empty Or Null", null);
                                return View("IpgError",
                                    new ResultModel { Succeeded = false, Message = "بروز خطا قبل از تایید در بانک انصار", Status = 1 });
                            }
                            var host = ApplicationConfiguration.BaseUrl
                            .Replace("https://", "")
                            .Replace("/", "");
                            var res = await VerifyMerchantTransAsync(token, refNum, host);

                            if (!res)
                            {
                                await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                    HybridPaymentStatusTypeEnum.Unsuccessful,
                                    PaymentMethodTypesEnum.Ansar, "بروز خطا تایید در بانک انصار",
                                    ansarPaymentVerifyViewModel.RefrenceNumber, customerRefNum, goodReferenceId, merchantData,
                                    true);
                                ansarPaymentVerifyViewModel.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Unsuccessful;
                            }
                            else
                            {
                                ansarPaymentVerifyViewModel.RefrenceNumber = refNum;
                                ansarPaymentVerifyViewModel.RefrenceNumber = Request.Form[AnsarResultParam.RefNum];
                                ansarPaymentVerifyViewModel.ReservationNumber = Request.Form[AnsarRequestParam.ResNum];
                                ansarPaymentVerifyViewModel.CustomerRefNum = customerRefNum;
                                ansarPaymentVerifyViewModel.CardMaskPan = Request.Form[AnsarResultParam.CardMaskPan];
                                ansarPaymentVerifyViewModel.Email = Request.Form[AnsarResultParam.EmailAddress];
                                ansarPaymentVerifyViewModel.Mobile = Request.Form[AnsarResultParam.MobileNo];
                                ansarPaymentVerifyViewModel.Message = string.Format(
                                    AnsarMessage.SuccessfulVerify, hybridPayment.Price);
                                try
                                {
                                    // _hybridPaymentService.UpdateHybridPaymentRequestStatus(payment.OrderId,
                                    //     OrderStatusTypesEnum.Complete);

                                    await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                        HybridPaymentStatusTypeEnum.Paid,
                                        PaymentMethodTypesEnum.Ansar,
                                        ansarPaymentVerifyViewModel.Message,
                                        ansarPaymentVerifyViewModel.RefrenceNumber, customerRefNum, goodReferenceId,
                                        merchantData, true);

                                    // بررسی میکنیم که  مبلغ باقیمانده اقساطی و نقدی پرداخت شده است و مبلغ دیگری باقی نمانده باشد
                                    var (remainingHekmatPrice, remainingCashPrice, _) =
                                        await _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(
                                            hybridPayment
                                                .HybridPaymentRequestId, useFormula);

                                    if (remainingHekmatPrice == 0 && remainingCashPrice == 0)
                                    {
                                        await _hybridPaymentRequestService
                                            .UpdateHybridPaymentRequestStatusAsync(
                                                hybridPayment.HybridPaymentRequestId,
                                                HybridPaymentRequestStatusTypeEnum.Complete, true);

                                        ansarPaymentVerifyViewModel.HybridPaymentStatusTypeEnum =
                                            HybridPaymentStatusTypeEnum.Paid;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var message = "paymentId :" + hybridPayment.Id +
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
                                    await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                        HybridPaymentStatusTypeEnum.Unsuccessful, PaymentMethodTypesEnum.Ansar,
                                        message, "", customerRefNum, goodReferenceId, merchantData, true);

                                    return View("IpgError",
                                        new ResultModel
                                        {
                                            Succeeded = false,
                                            Message = "بروز خطا در انجام عملیات",
                                            Status = 1
                                        });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.InsertLog(LogLevel.Error,
                                (int)Events.PaymentException, nameof(HybridPayment), "", ex);
                            await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                HybridPaymentStatusTypeEnum.Unsuccessful,
                                PaymentMethodTypesEnum.Ansar, "بروز خطا قبل از تایید در بانک انصار", "",
                                customerRefNum, goodReferenceId, merchantData, true);
                            return View("IpgError",
                                new ResultModel
                                {
                                    Succeeded = false,
                                    Message = "بروز خطا قبل از تایید در بانک انصار",
                                    Status = 1
                                });
                        }
                    }
                }
                else
                {

                    // fill the result of unsuccessful transaction
                    ansarPaymentVerifyViewModel.ReservationNumber = resNumber;
                    ansarPaymentVerifyViewModel.State = transactionState;
                    ansarPaymentVerifyViewModel.Message = "انصراف کاربر از پرداخت";
                    await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                        HybridPaymentStatusTypeEnum.Unsuccessful,
                        PaymentMethodTypesEnum.Ansar, "انصراف کاربر از پرداخت",
                        ansarPaymentVerifyViewModel.RefrenceNumber, customerRefNum, goodReferenceId, merchantData,
                        true);
                    ansarPaymentVerifyViewModel.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Cancel;
                }
            }


            var (_, _, hybridPaymentRequest) =
                await _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(
                    hybridPayment
                        .HybridPaymentRequestId, useFormula);
            ansarPaymentVerifyViewModel.HybridPaymentStatusResultModel = MapHybridPaymentRequestDtoToHybridPaymentStatusResultModel(hybridPaymentRequest);
            ansarPaymentVerifyViewModel.Amount = hybridPayment.Price.ToString();
            ansarPaymentVerifyViewModel.HybridPaymentId = hybridPayment.Id;
            return View("VerifyPayment", ansarPaymentVerifyViewModel);
        }

        return View("IpgError", new ResultModel { Succeeded = false, Message = "پرداختی یافت نشد", Status = 1 });
    }


    /// <summary>
    /// متدی که کاربر بعد از انجام تراکنش در درگاه ایران کیش به آن هدایت می شود
    /// و کار بررسی وضعیت تراکنش اتخاذ تصمیم های لازم را انجام می دهد.
    /// </summary>
    /// <returns></returns>
    [HttpPost("VerifyIranKishPaymentTransaction")]
    public async Task<IActionResult> VerifyIranKishPaymentTransactionAsync()
    {
        var iranKishPaymentVerifyViewModel = new VerifyHybridPaymentViewModel
        {
            PaymentMethodType = PaymentMethodTypesEnum.IranKish
        };


        //check parameter come back from IPG page

        var verifyIranKishPurchaseTransactionConfirmationRequestFieldsResult = VerifyIranKishPurchaseTransactionConfirmationRequestFields();

        if (verifyIranKishPurchaseTransactionConfirmationRequestFieldsResult.IsSuccess)
        {
            //شناسه درخواستی که به سمت بانک ارسال کرده ایم برای ره گیری تراکنش در سمت اپلیکیشن
            string? requestNumber = Request.Form[IranKishVerifyParams.RequestId];

            if (string.IsNullOrEmpty(requestNumber))
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    nameof(VerifyIranKishPaymentTransactionAsync), $"requestNumber : {requestNumber} is Empty Or Null", null);
                return View("IpgError",
                    new ResultModel { Succeeded = false, Message = "بروز خطا قبل از تایید در درگاه پرداخت ", Status = 1 });
            }

            var hybridPaymentQuery = _hybridPaymentService.FindByReservationNumber(requestNumber, true);

            var hybridPayment = await hybridPaymentQuery
                .ProjectTo<HybridPaymentIpgVerifyViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();



            if (hybridPayment == null)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"سفارشی با این کد یافت نشد: {requestNumber}", null);
                return View("IpgError",
                    new ResultModel { Succeeded = false, Message = "پرداختی یافت نشد", Status = 1 });
            }

            //باید مقدار پرداختی کاربر در درگاه و مقدار ثبت شده در بانک اطلاعاتی ما یکسان باشد
            if (hybridPayment.Price.ToString() != Request.Form[IranKishVerifyParams.Amount])
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"عدم یکسان بودن مبلع پرداختی ثبت شده در درگاه با مبلغی که در hybridPayment ثبت شده است مبلغ درگاه برابر {Request.Form[IranKishVerifyParams.Amount]} و مبلغ hbridPayment برابر {hybridPayment.Price} است", null);

                return View("IpgError",
                    new ResultModel { Succeeded = false, Message = "عدم تطابق مبلغ پرداخت شده.", Status = 1 });
            }


            iranKishPaymentVerifyViewModel.CallBackUrl = hybridPayment.HybridPaymentRequestCallBackUrl;

            if (_paymentTransactionUtil.PaymentTransactionStatus.ContainsKey(requestNumber))
            {
                iranKishPaymentVerifyViewModel.Message =
                    "کاربر گرامی خواهشمند است جهت اطلاع از وضعیت پرداخت از طریق سایت پذیرنده مجددا مراجعه نمایید";

                // تراکنش را به حالت پردازش می برد تا در صورتی که دوباره فراخوانی شود، پردازش مجدد انجام نشود 
                return View("VerifyPayment", iranKishPaymentVerifyViewModel);
            }

            //باید همیشه مقدار داشته باشد و نال نباشد.
            /// <summary>
            /// آیا از فرمول استفاده کند برای محاسبه قیمت در
            /// پرداخت نقدی اقساطی طرح باقرالعلوم کاربرد دارد
            /// </summary>
            var useFormula = hybridPayment.UseFormula ?? true;


            // دریافت CustomerRefNum  از اطلاعات ارسالی بانک
            // این پارامتر بسیار مهم است و هنگام تایید پرداخت ها توسط مالی مورد استفاده قرار میگیرد
            var customerRefNum = Request.Form[IranKishVerifyParams.RetrievalReferenceNumber];

            // دریافت شناسه خرید دارنده کارت از اطلاعات ارسال شده توسط بانک
            var goodReferenceId = Request.Form[IranKishVerifyParams.SystemTraceAuditNumber];

            // دریافت شناسه خرید فروشنده از اطلاعات ارسال شده توسط بانک
            var merchantData = Request.Form[IranKishVerifyParams.PaymentId];

            _paymentTransactionUtil.PaymentTransactionStatus.GetOrAdd(requestNumber, true);

            iranKishPaymentVerifyViewModel.ReservationNumber = requestNumber;

            // در صورتی که رکورد پرداخت قبلا تایید شده باشد، دوباره آن را بروز رسانی نمی کنیم
            // این حالت زمانی اتفاق می افتد که متد VerifyPaymentTransactionAsync
            //بیش از یک باز با کد ResNum 
            // فراخوانی شده باشد
            if (hybridPayment.PaidDateUtc.HasValue)
            {
                if (hybridPayment.HybridPaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                {
                    iranKishPaymentVerifyViewModel.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Paid;
                }
                else
                {
                    iranKishPaymentVerifyViewModel.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Unsuccessful;
                    iranKishPaymentVerifyViewModel.Message = hybridPayment.AuthorizationTransactionResult;
                }
            }
            else
            {

                iranKishPaymentVerifyViewModel.HybridPaymentRequestTitle = hybridPayment.HybridPaymentRequestTitle;

                //قبلا بررسی شده اگر مقدار نداشته باشد به این مرحله نمی رسد.
                var transactionState = Request.Form[IranKishVerifyParams.ResponseCode];

                if (transactionState == IranKishTransactionResult.Success)
                {
                    // try to get parameters receive from IPG
                    iranKishPaymentVerifyViewModel.State = transactionState;

                    if (!string.IsNullOrEmpty(requestNumber))
                    {
                        // try to login for verify transaction
                        // if not verify transaction it will be returned to customer account
                        try
                        {
                            await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway,
                                PaymentMethodTypesEnum.IranKish, iranKishPaymentVerifyViewModel.Message,
                                iranKishPaymentVerifyViewModel.RefrenceNumber, customerRefNum, goodReferenceId,
                                merchantData, true);

                            iranKishPaymentVerifyViewModel.HybridPaymentStatusTypeEnum =
                                HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway;

                            // fill the model
                            string? token = Request.Form[IranKishVerifyParams.Token];

                            string? refNum = Request.Form[IranKishVerifyParams.RetrievalReferenceNumber];

                            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refNum))
                            {
                                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                                    "Payment", $"token : {token} is Empty Or Null || refNum : {refNum} is Empty Or Null",
                                    null);

                                return View("IpgError",
                                    new ResultModel { Succeeded = false, Message = "بروز خطا قبل از تایید در بانک انصار", Status = 1 });
                            }


                            var iranKishPaymentConfiguration =
                                _iranKishPaymentService.GetIranKishPaymentConfiguration();

                            var res = await _iranKishPaymentService.VerifyMerchantTransactionAsync(iranKishPaymentConfiguration.TerminalId,
                                customerRefNum, goodReferenceId, token);

                            if (!res)
                            {
                                await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                    HybridPaymentStatusTypeEnum.Unsuccessful,
                                    PaymentMethodTypesEnum.IranKish, "بروز خطا در تایید تراکنش درگاه ایران کیش",
                                    iranKishPaymentVerifyViewModel.RefrenceNumber, customerRefNum, goodReferenceId, merchantData,
                                    true);

                                iranKishPaymentVerifyViewModel.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Unsuccessful;
                                iranKishPaymentVerifyViewModel.Message = "بروز خطا به هنگام انجام عملیات تایید تراکنش";
                            }
                            else
                            {
                                iranKishPaymentVerifyViewModel.RefrenceNumber = refNum;
                                iranKishPaymentVerifyViewModel.ReservationNumber = requestNumber;
                                iranKishPaymentVerifyViewModel.CustomerRefNum = customerRefNum;
                                iranKishPaymentVerifyViewModel.CardMaskPan = Request.Form[IranKishVerifyParams.MaskedPan];
                                iranKishPaymentVerifyViewModel.Email = string.Empty;
                                iranKishPaymentVerifyViewModel.Mobile = string.Empty;
                                iranKishPaymentVerifyViewModel.Message = string.Format(IranKishMessage.SuccessfulVerify, hybridPayment.Price);
                                try
                                {

                                    await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                        HybridPaymentStatusTypeEnum.Paid,
                                        PaymentMethodTypesEnum.IranKish,
                                        iranKishPaymentVerifyViewModel.Message,
                                        iranKishPaymentVerifyViewModel.RefrenceNumber, customerRefNum, goodReferenceId,
                                        merchantData, true);

                                    // بررسی میکنیم که  مبلغ باقیمانده اقساطی و نقدی پرداخت شده است و مبلغ دیگری باقی نمانده باشد
                                    var (remainingHekmatPrice, remainingCashPrice, _) =
                                        await _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(
                                            hybridPayment
                                                .HybridPaymentRequestId, useFormula);

                                    if (remainingHekmatPrice == 0 && remainingCashPrice == 0)
                                    {
                                        await _hybridPaymentRequestService
                                            .UpdateHybridPaymentRequestStatusAsync(
                                                hybridPayment.HybridPaymentRequestId,
                                                HybridPaymentRequestStatusTypeEnum.Complete, true);

                                        iranKishPaymentVerifyViewModel.HybridPaymentStatusTypeEnum =
                                            HybridPaymentStatusTypeEnum.Paid;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var message = "paymentId :" + hybridPayment.Id +
                                                  " ,PaymentMethodTypesEnum :" +
                                                  PaymentMethodTypesEnum.IranKish +
                                                  " ,PaymentStatusTypesEnum :" +
                                                  PaymentStatusTypesEnum.Paid +
                                                  " ,reference number:" + refNum + " " +
                                                  ",reservation number: " +
                                                  requestNumber +
                                                  ", customerRefNum: " +
                                                  customerRefNum +
                                                  ", goodReferenceId: " +
                                                  goodReferenceId +
                                                  ", merchantData" +
                                                  merchantData +
                                                  ",message" + iranKishPaymentVerifyViewModel.Message;


                                    _logger.InsertLog(LogLevel.Error,
                                        (int)Events.PaymentException, "Payment", message, ex);

                                    await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                        HybridPaymentStatusTypeEnum.Unsuccessful, PaymentMethodTypesEnum.IranKish,
                                        message, "", customerRefNum, goodReferenceId, merchantData, true);

                                    return View("IpgError",
                                        new ResultModel
                                        {
                                            Succeeded = false,
                                            Message = "بروز خطا در انجام عملیات",
                                            Status = 1
                                        });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.InsertLog(LogLevel.Error,
                                (int)Events.PaymentException, nameof(HybridPayment), "", ex);


                            await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                                HybridPaymentStatusTypeEnum.Unsuccessful,
                                PaymentMethodTypesEnum.IranKish, "بروز خطا قبل از تایید در بانک انصار", "",
                                customerRefNum, goodReferenceId, merchantData, true);


                            return View("IpgError",
                                new ResultModel
                                {
                                    Succeeded = false,
                                    Message = "بروز خطا قبل از تایید در بانک انصار",
                                    Status = 1
                                });
                        }
                    }
                }
                else
                {

                    // fill the result of unsuccessful transaction
                    iranKishPaymentVerifyViewModel.ReservationNumber = requestNumber;
                    iranKishPaymentVerifyViewModel.State = transactionState;
                    iranKishPaymentVerifyViewModel.Message = "انصراف کاربر از پرداخت";
                    await _hybridPaymentService.UpdatePaymentStatusAsync(hybridPayment.Id,
                        HybridPaymentStatusTypeEnum.Unsuccessful,
                        PaymentMethodTypesEnum.Ansar, "انصراف کاربر از پرداخت",
                        iranKishPaymentVerifyViewModel.RefrenceNumber, customerRefNum, goodReferenceId, merchantData,
                        true);
                    iranKishPaymentVerifyViewModel.HybridPaymentStatusTypeEnum = HybridPaymentStatusTypeEnum.Cancel;
                }
            }


            var (_, _, hybridPaymentRequest) =
                await _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(
                    hybridPayment
                        .HybridPaymentRequestId, useFormula);

            iranKishPaymentVerifyViewModel.HybridPaymentStatusResultModel = MapHybridPaymentRequestDtoToHybridPaymentStatusResultModel(hybridPaymentRequest,false);

            iranKishPaymentVerifyViewModel.Amount = hybridPayment.Price.ToString();

            iranKishPaymentVerifyViewModel.HybridPaymentId = hybridPayment.Id;

            return View("VerifyPayment", iranKishPaymentVerifyViewModel);
        }


        _logger.InsertLog(LogLevel.Error,(int)Events.PaymentException,nameof(VerifyIranKishPaymentTransactionAsync), verifyIranKishPurchaseTransactionConfirmationRequestFieldsResult.Message, null);

        return View("IpgError", new ResultModel { Succeeded = false, Message = "بروز خط در تایید تراکنش لطفا با مدیر سیستم تماس بگیرید", Status = 1 });
    }


    /// <summary>
    /// این متد پارامتر های که رسید پرداختی درگاه ایران کیش
    /// را بررسی می کند و مقادیر بررسی شده باید حتما مقدار داشته باشند به دلیل این که
    /// در فرآیند تایید تراکنش نقش دارند
    /// </summary>
    /// <returns></returns>
    [NonAction]
    private ResultStatus<int> VerifyIranKishPurchaseTransactionConfirmationRequestFields()
    {

        if (!Request.HasFormContentType)
        {
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "بروز خطا"
            };
        }

        if (string.IsNullOrEmpty(Request.Form[IranKishVerifyParams.ResponseCode]))
        {
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message =
                    "خريد شما توسط بانک تاييد شده است اما رسيد ديجيتالي شما تاييد نگشت! مشکلي در فرايند رزرو خريد شما پيش آمده است",
            };
        }

        if (string.IsNullOrEmpty(Request.Form[IranKishVerifyParams.RetrievalReferenceNumber]))
        {
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message =
                        "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت"
            };
        }

        if (string.IsNullOrEmpty(Request.Form[IranKishVerifyParams.SystemTraceAuditNumber]))
        {
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "خطا در دریافت تاییدیه بانک"
            };
        }

        return new ResultStatus<int>(true, string.Empty);
    }



    public async Task<IActionResult> CancelPaymentAsync(int reservationNumber, string paymentRequestToken, string? callBackUrl)
    {
        var resultModel = new ResultModel
        {
            Succeeded = false,
            Status = 0
        };
        try
        {
            // استخراج اطلاعات توکن و پذیرنده و بررسی معتبر بودن توکن
            var (succeeded, message, _, ipgToken, terminalInfo) =
                await _paymentGatewayService.ValidateAndExtractEncryptedTokenData(paymentRequestToken);
            if (!succeeded || ipgToken == null || terminalInfo == null)
            {
                resultModel.Succeeded = false;
                resultModel.Message = "مقدار توکن معتیر نیست.";
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert,
                    nameof(HybridPaymentRequest), $"Token Is Invalid ={paymentRequestToken}", null);

                return View("IpgError", resultModel);
            }
            var remotePost = new RemotePost(_httpContextAccessor)
            {
                Url = callBackUrl
            };
            remotePost.Add("ReservationNumber", reservationNumber.ToString());
            remotePost.Add("statusCode", PaymentGatewayResultStatusCodeEnum.Canceled.ToString());

            await remotePost.PostAsync();

        }
        catch (Exception e)
        {
            resultModel.Succeeded = false;
            resultModel.Message = "توکن معتبر نمی باشد.";
            _logger.InsertLog(LogLevel.Error,
                (int)Events.InvalidParamsInsert,
                nameof(HybridPaymentRequest),
                $"token is not valid in HybridPayment purchase with encryptedToken ={paymentRequestToken}",
                e);

        }
        return View("IpgError", resultModel);
    }


    private HybridPaymentRequestStatusResultModel MapHybridPaymentRequestDtoToHybridPaymentStatusResultModel(HybridPaymentRequestDto hybridPaymentRequest,
        bool ansarPayment = true)
    {
        var result = new HybridPaymentRequestStatusResultModel
        {
            ErrorCode = (int)PaymentGatewayErrorCodeEnum.NotAnyError,
            StatusCode = PaymentHelper.GetHybridPaymentRequestStatusCode(hybridPaymentRequest.HybridPaymentRequestStatusType),
            InstallmentCount = hybridPaymentRequest.InstallmentCount,
            CahPaymentPercentage = hybridPaymentRequest.CashPaymentPercentage,
            ReferenceNumber = hybridPaymentRequest.ReferenceNumber,
            ReservationNumber = hybridPaymentRequest.ReservationNumber,
            Message = _commonBaseDataService.GetTitle
                (hybridPaymentRequest.HybridPaymentRequestStatusType),
            HybridPaymentRequestId = hybridPaymentRequest.Id
        };

        var paymentMethodType = ansarPayment ? (int)PaymentMethodTypesEnum.Ansar : (int)PaymentMethodTypesEnum.IranKish;

        result.HekmatPayments.AddRange(hybridPaymentRequest.HybridPayments
            .Where(x => x.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat)
            .Select(x => new PaymentStatusResultModel
            {
                ReservationNumber = x.Id.ToString(),
                StatusCode = PaymentHelper.GetHybridPaymentStatusCode(x.HybridPaymentStatusType),
                ErrorCode = (int)PaymentGatewayErrorCodeEnum.NotAnyError,
                Message = x.AuthorizationTransactionResult ?? string.Empty,
                PaidDate = x.PaidDateInt.ToString(),
            }).ToList());
        result.OnlinePayments.AddRange(hybridPaymentRequest.HybridPayments
            .Where(x => x.PaymentMethodType == paymentMethodType)
            .Select(x => new PaymentStatusResultModel
            {
                ReservationNumber = x.ReservationNumber,
                StatusCode = PaymentHelper.GetHybridPaymentStatusCode(x.HybridPaymentStatusType),
                ErrorCode = (int)PaymentGatewayErrorCodeEnum.NotAnyError,
                Message = x.AuthorizationTransactionResult ?? string.Empty,
                PaidDate = x.PaidDateInt.ToString(),
            }).ToList());
        return result;
    }


}