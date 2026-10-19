using AutoMapper;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Extensions;
using YashilPayment.Core.Services;
using YashilPayment.Infrastructure.Helper;
using YashilPayment.Web.Areas.Payment.ViewModels;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;
using Language = DNTCaptcha.Core.Language;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers;

public class HekmatIpgController : BaseController<HekmatPaymentGatewayTransaction, int,
    HekmatPaymentGatewayTransactionListViewModel, HekmatPaymentGatewayTransactionEditModel,
    HekmatPaymentGatewayTransactionSimpleViewModel>
{
    private readonly IDNTCaptchaValidatorService _validatorService;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly IYashilHekmatTerminalService _yashilHekmatTerminalService;
    private readonly IMerchantInfoService _merchantInfoService;
    private readonly IHekmatPaymentGatewayTransactionService _hekmatPaymentGatewayTransactionService;
    private readonly IPaymentGatewayService _paymentGatewayService;
    private readonly IHybridPaymentService _hybridPaymentService;
    private readonly IYashilLogger<HekmatPaymentGatewayTransaction> _logger;

    public HekmatIpgController(IHekmatPaymentGatewayTransactionService hekmatPaymentGatewayTransactionService,
        IMapper mapper,
        IYashilLogger<HekmatPaymentGatewayTransaction> logger,
        IPermissionService permissionService, IDNTCaptchaValidatorService validatorService,
        IMerchantInfoService merchantInfoService, IYashilHekmatTerminalService yashilHekmatTerminalService,
        ApplicationConfiguration applicationConfiguration, IPaymentGatewayService paymentGatewayService, IHybridPaymentService hybridPaymentService)
        : base(hekmatPaymentGatewayTransactionService, mapper,
        logger, permissionService)
    {
        _hekmatPaymentGatewayTransactionService = hekmatPaymentGatewayTransactionService;
        _logger = logger;
        _validatorService = validatorService;
        _merchantInfoService = merchantInfoService;
        _yashilHekmatTerminalService = yashilHekmatTerminalService;
        _applicationConfiguration = applicationConfiguration;
        _paymentGatewayService = paymentGatewayService;
        _hybridPaymentService = hybridPaymentService;
    }
    /// <summary>
    /// وقتی کاربر در صفحه پرداخت حکمت دکمه پرداخت را کلیک می کند این متد فراخوانی می شود
    /// .در صورتی که پارامتر های ارسالی کاربر معتیر باشند 
    /// یک تراکنش با وضعیت منتظر بررسی حکمت در دیتا بیس درگاه ثبت می شود و به کاربر پیام مناسب داده می شود
    /// </summary>
    /// <param name="hekmatTransactionParam">پارمتر های ارسالی از صفحه درگاه حکمت</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("Purchase")]
    public async Task<PurchaseResult> Purchase([FromForm] HekmatTransactionParam hekmatTransactionParam)
    {
        var purchaseResult = new PurchaseResult { IsSuccess = false };
        if (!_validatorService.HasRequestValidCaptchaEntry(Language.Persian, DisplayMode.NumberToWord))
        {
            purchaseResult.IsSuccess = false;
            purchaseResult.Message = "عدد وارد شده با متن همخوانی ندارد";
        }
        else
        {
            string token;
            string encryptedToken = hekmatTransactionParam.PaymentRequestToken;

            var (succeeded, message, _, ipgToken, terminal) =
                await _paymentGatewayService.ValidateAndExtractEncryptedTokenData(encryptedToken);

            if (!succeeded || ipgToken == null || terminal == null)
            {
                purchaseResult.Message = "توکن معتبر نمی باشد";
                purchaseResult.IsSuccess = false;
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction),
                    message, null);
                
            }

            purchaseResult.UseFormula = ipgToken.UseFormula;
            purchaseResult.InstallmentCount = ipgToken.InstallmentCount;

            //try
            //{
            //    token = CryptographyHelper.AesDecrypt(encryptedToken);
            //}
            //catch (Exception)
            //{
            //    purchaseResult.IsSuccess = false;
            //    purchaseResult.Message = "توکن معتبر نمی باشد";
            //    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"token is not valid in HekmatIpgController purchase with encryptedToken ={encryptedToken}", null);
            //    return purchaseResult;
            //}

           // var ipgToken = PaymentHelper.GetPrepareTokenData(token);
            //if (ipgToken == null)
            //{
            //    purchaseResult.IsSuccess = false;
            //    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"token is not valid in HekmatIpgController purchase with token ={token}", null);

            //    purchaseResult.Message = "توکن معتبر نمی باشد";
            //}
            //else
            //{
                bool tokenIsExpired = PaymentHelper.IsTokenExpired(ipgToken.TokenExpireDateTime);

                if (tokenIsExpired)
                {
                    purchaseResult.IsSuccess = false;
                    purchaseResult.Message = "زمان اعتبار توکن منقضی شده است";
                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"token is not valid in HekmatIpgController purchase", null);

                }
                else
                {
                    var cardNumber = !string.IsNullOrEmpty(ipgToken.CardNumber) ? ipgToken.CardNumber : hekmatTransactionParam.CardNumber;
                    if (!cardNumber.IsValidValidateHekmatCard() || !hekmatTransactionParam.NationalCode.IsValidNationalCode())
                    {
                        purchaseResult.IsSuccess = false;
                        purchaseResult.Message = "کاربر گرامی ،لطفا ورودی های خود را چک نمایید";
                        _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"invalid parameter cardNumber or nationalCode in HekmatIpgController purchase with nationalCode={cardNumber} or cardNumber={cardNumber}", null);
                    }
                    else
                    {
                        var merchantInfo = await _merchantInfoService.GetAsync(ipgToken.MerchantId, true);
                        var yashilHekmatTerminal = await _yashilHekmatTerminalService.GetAsync(ipgToken.TerminalId, true);
                        if (merchantInfo != null)
                        {
                            var hekmatPaymentGatewayTransaction = new HekmatPaymentGatewayTransaction
                            {
                                HekmatCardNumber = cardNumber,
                                HekmatTerminalId = yashilHekmatTerminal.HekmatTerminalId,
                                YashilHekmatTerminalId = yashilHekmatTerminal.Id,
                                Title = "",
                                Price = ipgToken.Amount,
                                PriceToPay = ipgToken.Amount,
                                ErrorCode = 0,
                                StatusCode = 0,
                                CallBackUrl = ipgToken.ReturnUrl,
                                AdditionalDate = ipgToken.AdditionalData,
                                LocalReservationNumber = ipgToken.ReservationNumber,
                                InstallmentCount = ipgToken.InstallmentCount,
                                HekmatTransactionResultMessage = "منتظر بررسی پرداخت حکمت",
                                Tell = hekmatTransactionParam.Tell,
                                Mobile = hekmatTransactionParam.Mobile,
                                NationalCode = hekmatTransactionParam.NationalCode,
                                CreateBy = _applicationConfiguration.SysUserId,
                                CreationDate = DateTime.Now,
                                CreatorOrganizationId = _applicationConfiguration.PublicUsersOrganizationId,
                                Deleted = false,
                                ApplicationId = _applicationConfiguration.ApplicationId
                            };
                            try
                            {
                                var oldTransactions = await _hekmatPaymentGatewayTransactionService.GetByCustomFilter(null, yashilHekmatTerminal.Id, null, null, null, ipgToken.ReservationNumber).ToListAsync();
                                if (oldTransactions.Any())
                                {
                                    purchaseResult.IsSuccess = false;
                                    purchaseResult.Message = "کد سفارش وارد شده تکراری می باشد";
                                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"failed to insert in HekmatPaymentGatewayTransaction HekmatIpgController purchase", null);
                                }
                                else
                                {
                                    // بررسی ولیدیشن های دیگر سمت پایگاه داده مثل لیست سیاه شماره تلفن و ..
                                    var validateMessageBeforeHekmatPaymentPurchase = _hekmatPaymentGatewayTransactionService
                                        .ValidateBeforeHekmatPaymentPurchase(hekmatPaymentGatewayTransaction, ipgToken.MerchantId);
                                    if (!string.IsNullOrEmpty(validateMessageBeforeHekmatPaymentPurchase))
                                    {
                                        purchaseResult.IsSuccess = false;
                                        purchaseResult.Message = validateMessageBeforeHekmatPaymentPurchase;
                                        _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"ValidateBeforeHekmatPaymentPurchase failed", null);
                                        return purchaseResult;
                                    }

                                    var hekmatTransaction = await _hekmatPaymentGatewayTransactionService.AddAsync(hekmatPaymentGatewayTransaction,
                                        true);
                                    purchaseResult.IsSuccess = true;
                                    purchaseResult.Message =
                                        " کاربر گرامی،درخواست پرداخت شما با موفقیت ثبت شد.طی 24 ساعات آینده نتیجه اعتبار سنجی به شما اعلام خواهد شد.";
                                    purchaseResult.Url = "HybridIpg/VerifyHekmatPaymentTransaction";
                                    purchaseResult.RefrenceNumber = hekmatTransaction.Id.ToString();
                                    purchaseResult.ReservationNumber = hekmatTransaction.LocalReservationNumber;
                                    purchaseResult.Status = (int)HekmatPurchaseResultStatusCodeEnum.Successful;
                                    purchaseResult.CallBackUrl = hekmatTransaction.CallBackUrl;
                                    purchaseResult.AdditionalDate = hekmatTransaction.AdditionalDate;
                                    purchaseResult.Amount = hekmatTransaction.Price;
                                    purchaseResult.BasePrice = await _hybridPaymentService.GetBasePriceAsync
                                        (int.Parse(ipgToken.ReservationNumber));

                                }
                            }
                            catch (Exception e)
                            {
                                purchaseResult.IsSuccess = false;
                                purchaseResult.Message = "خطا در ثبت تراکنش جدید پرداخت حکمت";
                                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"failed to insert in HekmatPaymentGatewayTransaction HekmatIpgController purchase", e);

                                return purchaseResult;
                            }
                        }
                        else
                        {
                            purchaseResult.IsSuccess = false;
                            purchaseResult.Message = "اطلاعات ترمینال صحیح نمی باشد";
                            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"terminal info is not valid in  HekmatPaymentGatewayTransaction HekmatIpgController purchase with merchantId={ipgToken.MerchantId} and terminalId={ipgToken.TerminalId}", null);
                        }
                    }
                }
            //}
        }
        return purchaseResult;
    }



}