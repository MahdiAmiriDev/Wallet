using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.CustomDtos.Payment;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Extensions;
using Yashil.Infrastructure.Util;
using YashilPayment.Core.Classes;
using YashilPayment.Core.CustomDomainModels;
using YashilPayment.Core.Services;
using YashilPayment.Infrastructure.Helper;
using YashilUserManagement.Core.Services;


namespace YashilPayment.Infrastructure.ServiceImpl;

public class PaymentGatewayService : IPaymentGatewayService
{
    private readonly IMerchantInfoService _merchantInfoService;
    private readonly IYashilHekmatTerminalService _yashilHekmatTerminalService;
    private readonly IHybridPaymentRequestService _hybridPaymentRequestService;
    private readonly IHekmatPaymentGatewayTransactionService _hekmatPaymentGatewayTransactionService;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly IHybridPaymentService _hybridPaymentService;
    private readonly IWebHelper _webHelper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppConfigService _appConfigService;
    private readonly IYashilLogger<object> _logger;

    public PaymentGatewayService(IMerchantInfoService merchantInfoService, IWebHelper webHelper,
        IYashilHekmatTerminalService yashilHekmatTerminalService, IYashilLogger<object> logger,
        IHybridPaymentRequestService hybridPaymentRequestService,
        IHekmatPaymentGatewayTransactionService hekmatPaymentGatewayTransactionService,
        ApplicationConfiguration applicationConfiguration, IUnitOfWork unitOfWork,
        IHybridPaymentService hybridPaymentService, IAppConfigService appConfigService)
    {
        _merchantInfoService = merchantInfoService;
        _webHelper = webHelper;
        _yashilHekmatTerminalService = yashilHekmatTerminalService;
        _logger = logger;
        _hybridPaymentRequestService = hybridPaymentRequestService;
        _hekmatPaymentGatewayTransactionService = hekmatPaymentGatewayTransactionService;
        _applicationConfiguration = applicationConfiguration;
        _unitOfWork = unitOfWork;
        _hybridPaymentService = hybridPaymentService;
        _appConfigService = appConfigService;
    }

    public async Task<PaymentRequestResultDto> GenerateTokenForPaymentRequestAsync(int merchantId, long amount,
        string returnUrl, string additionalData,
        string reservationNumber, int terminalId,
        int installmentCount, string signData,
        string localDateTime,
        bool isHybrid,
        bool isTaxExempt = false,
        string cardNumber = null,
        int? cashPaymentPercentage = null,
        int? shippingAmount = null,
        bool useFormula = true)
    {
        var paymentRequestResult = new PaymentRequestResultDto { Succeeded = false };
        try
        {
            var merchantInfo = await _merchantInfoService.GetAsync(merchantId, true);
            if (merchantInfo == null)
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.MerchantNotFound;
                paymentRequestResult.Message = "پذیرنده با شناسه پذیرنده ارسالی یافت نشد";
                return paymentRequestResult;
            }
            var merchantTerminal = await _yashilHekmatTerminalService.GetAsync(terminalId, true);
            if (merchantTerminal == null || merchantTerminal.MerchantId != merchantId)
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.InvalidTerminal;
                paymentRequestResult.Message = "ترمینال کاربر صحیح نمی باشد.";
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert,
                    null,
                    $"terminal not valid in GenerateTokenForPaymentRequestAsync PaymentRequestAsync with terminal id={terminalId} and merchantId={merchantId}",
                    null);

                return paymentRequestResult;
            }

            if (!merchantTerminal.IsActive)
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.InactiveTerminal;
                paymentRequestResult.Message = "ترمینال کاربر فعال نمی باشد.";
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert,
                    nameof(HekmatPaymentGatewayTransaction),
                    $"terminal is not active GenerateTokenForPaymentRequestAsync PaymentRequestAsync with terminal id={terminalId} and merchantId={merchantId}",
                    null);

                return paymentRequestResult;
            }
            string ipAddress = _webHelper.GetCurrentIpAddress();
            if (string.IsNullOrEmpty(merchantTerminal.IpAddress))
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.InvalidIpAddress;
                paymentRequestResult.Message = "آی پی آدرس ترمینال دارای مقدار نمی باشد.";
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert,
                    null,
                    $"terminal Ip not valid in GenerateTokenForPaymentRequestAsync PaymentRequestAsync with terminal id={terminalId} and merchantId={merchantId}, Terminal IpAddress Is Null and requestedIp ={ipAddress} ",
                    null);

                return paymentRequestResult;
            }
            if (!string.IsNullOrEmpty(merchantTerminal.IpAddress) && merchantTerminal.IpAddress != ipAddress)
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.InvalidIpAddress;
                paymentRequestResult.Message = "آی پی آدرس ترمینال صحیح نمی باشد.";
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert,
                    null,
                    $"terminal Ip not valid in GenerateTokenForPaymentRequestAsync PaymentRequestAsync with terminal id={terminalId} and merchantId={merchantId} and requestedIp ={ipAddress} and terminal Ip = {merchantTerminal.IpAddress}",
                    null);

                return paymentRequestResult;
            }

            if (installmentCount <= 0 || installmentCount > merchantTerminal.MaxInstallmentCount)
            {
                paymentRequestResult.StatusCode =
                    (int)PaymentGatewayRequestStatusCodeEnum.InvalidInstallmentCount;
                paymentRequestResult.Message =
                    "تعداد اقساط ارسالی معتبر نمی باشد - تعداد اقساط بیشتر از سقف تعیین شده می باشد.";

                return paymentRequestResult;
            }

            if (amount < merchantTerminal.MinAmount || amount > merchantTerminal.MaxAmount)
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.InvalidAmount;
                paymentRequestResult.Message =
                    "مبلغ ارسالی معتبر نمی باشد - مبلغ ارسالی خارج از محدوده حداقل و حداکثر مبلغ می باشد.";

                return paymentRequestResult;
            }

            if (!string.IsNullOrEmpty(cardNumber) && !cardNumber.IsValidValidateHekmatCard())
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.InvalidCardNumber;
                paymentRequestResult.Message = "شماره کارت معتبر نمی باشد.";

                return paymentRequestResult;
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.ReturnUrlIsEmpty;
                paymentRequestResult.Message = "آدرس صفحه بازگشتی دارای مقدار نمی باشد.";

                return paymentRequestResult;
            }

            if (string.IsNullOrEmpty(reservationNumber))
            {
                paymentRequestResult.StatusCode =
                    (int)PaymentGatewayRequestStatusCodeEnum.ReservationNumberIsEmpty;
                paymentRequestResult.Message = "شناسه سفارش پذیرنده دارای مقدار نمی باشد.";

                return paymentRequestResult;
            }

            if (string.IsNullOrEmpty(localDateTime))
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.localDateTimeIsEmpty;
                paymentRequestResult.Message = "تاریخ درخواست پرداخت دارای مقدار نمی باشد.";

                return paymentRequestResult;
            }

            // اگر پرداخت اقساطی باشد و پرداخت ترکیبی نباشد ،
            // باید cashPaymentPercentage مقدار نداشته باشد
            if (!isHybrid && cashPaymentPercentage.HasValue)
            {
                paymentRequestResult.StatusCode =
                    (int)PaymentGatewayRequestStatusCodeEnum.InvalidCashPaymentPercentage;
                paymentRequestResult.Message =
                    "مقدار درصد پرداخت نقدی باید در درخواست اقساطی بدون مقدار باشد";

                return paymentRequestResult;
            }
            // در صورت پرداخت ترکیبی و درصورت مشخس بودن
            //cashPaymentPercentage
            // باید بررسی کنیم که مقداری بین 0 تا 100 داشته باشد
            else if (isHybrid && cashPaymentPercentage.HasValue &&
                     (cashPaymentPercentage > 100 || cashPaymentPercentage < 0))
            {
                paymentRequestResult.StatusCode =
                    (int)PaymentGatewayRequestStatusCodeEnum.InvalidCashPaymentPercentage;
                paymentRequestResult.Message =
                    "مقدار درصد پرداخت نقدی دارای مقدار معتبری نمی باشد، مقداری بین 0 تا 100 انتخاب کنید";

                return paymentRequestResult;
            }


            string data = $"{merchantId};{terminalId};{reservationNumber};{amount};{installmentCount}";
            if (isHybrid && cashPaymentPercentage.HasValue)
            {
                data += $";{cashPaymentPercentage}";
            }
            if (isHybrid && shippingAmount.HasValue)
            {
                data += $";{shippingAmount.Value}";
            }
            if (isHybrid && isTaxExempt)
            {
                data += $";{true}";
            }

            bool signatureIsOk = CryptoHelper.VerifySignature(data, signData, merchantTerminal.PublicKey);
            if (!signatureIsOk)
            {
                paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.InvalidSignature;
                paymentRequestResult.Message = "امضا معتبر نمی باشد";
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert,
                    null,
                    $"signature data is not valid in HekmatIpgService PaymentRequestAsync with terminal id={terminalId} and merchantId={merchantId} and requestedIp ={ipAddress} and terminal Ip = {merchantTerminal.IpAddress}",
                    null);

                return paymentRequestResult;
            }

            // اگر پرداخت ترکیبی باشد بررسی میکنیم که قبلا این تراکنش ثبت شده است یا خیر
            if (isHybrid)
            {
                var oldHybridPaymentRequest = await _hybridPaymentRequestService
                    .GetByCustomFilter(merchantId, terminalId, null, reservationNumber,
                        _applicationConfiguration.ApplicationId).FirstOrDefaultAsync();
                if (oldHybridPaymentRequest != null)
                {
                    // در اینجا چون از قبل درخواست ذخیره شده پس باید اطلاعات ارسالی با اطلاعات قبلی همخوانی داشته باشد
                    // در غیر این صورت این درخواست یک درخواست جدید با شماره تراکنش تکراری می باشد
                    if (oldHybridPaymentRequest.AdditionalDate != additionalData ||
                        oldHybridPaymentRequest.InstallmentCount != installmentCount ||
                        oldHybridPaymentRequest.BasePrice != amount
                       )
                    {
                        paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum
                            .DuplicateLocalReservationNumber;
                        paymentRequestResult.Message =
                            "کد سفارش وارد شده تکراری می باشد یا اطلاعات ارسالی با اطلاعات قبلی یکسان نمی باشد";
                        return paymentRequestResult;
                    }

                    // چون پذیرنده ممکن است درصد پرداخت نقدی را مشخص نکرده باشد
                    // و در صورتی که درخواست قبلی در سامانه وجود داشته باشد پس درصد را از درخواست قبلی میخوانیم
                    cashPaymentPercentage = oldHybridPaymentRequest.CashPaymentPercentage;
                }
            }
            // اگر پرداخت اقساطی بود بررسی میکنیم که این شماره تراکنش قبلا پرداختی نداشته باشد
            else
            {
                //لیست تراکنش هایی که قبلا بابت این کد رهگیری ثبت شده اند
                var oldTransactions =
                    await _hekmatPaymentGatewayTransactionService
                        .GetByCustomFilter(null, terminalId, null, null, null, reservationNumber)
                        .ToListAsync();
                if (oldTransactions.Any())
                {
                    var oldTransaction = oldTransactions.FirstOrDefault();

                    //  در این حالت قبلا در خواستی با این کد سفارش ثبت شده
                    if (oldTransaction != null)
                    {
                        // یا تراکنش سمت حکمت رفته است و پاسخ آمده که در این صورت حتما توکن حکمت مقدار دارد
                        // یا اینکه همچنان به سمت حکمت نرفته و در وضعیت منتظر بررسی حکمت می باشد
                        if (!string.IsNullOrWhiteSpace(oldTransaction.HekmatToken)
                            || oldTransaction.StatusCode ==
                            (int)PaymentGatewayResultStatusCodeEnum.WaitingForReview)
                        {
                            paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum
                                .DuplicateLocalReservationNumber;
                            paymentRequestResult.Message = "کد سفارش وارد شده تکراری می باشد";
                            return paymentRequestResult;
                        }
                    }
                }
            }

            var tokenData = PaymentHelper.CreateToken(ipAddress,
                DateTime.Now.AddMinutes(20), merchantId,
                terminalId, reservationNumber, localDateTime, returnUrl,
                amount, installmentCount, cardNumber,
                additionalData, cashPaymentPercentage,
                shippingAmount, isTaxExempt,useFormula);
            var token = CryptographyHelper.AesEncrypt(tokenData);
            paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.Successful;
            paymentRequestResult.Token = token;
            paymentRequestResult.Message = "با موفقیت انجام شد";
            paymentRequestResult.Succeeded = true;
        }

        catch (Exception ex)
        {
            _logger.InsertLog(LogLevel.Error,
                (int)Events.InsertException, "HekmatIpg",
                $"خطا در درخواست پرداخت", ex);

            paymentRequestResult.StatusCode = (int)PaymentGatewayRequestStatusCodeEnum.Error;
            paymentRequestResult.Message = "خطا در برقراری ارتباط";
            return paymentRequestResult;
        }

        return paymentRequestResult;
    }

    public async Task<HybridPaymentStatusResultDto> ValidateAndGetHybridPaymentStatusAsync(int merchantId, int terminalId,
        string reservationNumber, string signData,bool useFormula)
    {
        // پارامتر های ارسال شده به سرویس بررسی وضعیت پرداخت را اعتبار سنجی می کند 
        var result = new HybridPaymentStatusResultDto
        {
            ReservationNumber = reservationNumber
        };
        try
        {

            (var succeed, result.StatusCode, result.ErrorCode, result.Message) =
               await ValidatePaymentStatusAsync(merchantId, terminalId, reservationNumber, signData);
            if (!succeed)
            {
                return result;
            }

            var (_, _, hybridPaymentRequest) =
                await _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(merchantId, terminalId,
                    reservationNumber, useFormula,_applicationConfiguration.ApplicationId);
            if (hybridPaymentRequest == null)
            {
                result.StatusCode = (int)PaymentGatewayResultStatusCodeEnum.Unknown;
                result.ErrorCode = (int)PaymentGatewayErrorCodeEnum.TransactionNotFound;
                result.Message = "چنین تراکنشی یافت نشد";
                return result;
            }



            result.ErrorCode = (int)PaymentGatewayErrorCodeEnum.NotAnyError;
            result.StatusCode =
                PaymentHelper.GetHybridPaymentRequestStatusCode(hybridPaymentRequest.HybridPaymentRequestStatusType);
            if (result.StatusCode == (int)PaymentGatewayResultStatusCodeEnum.Unknown)
            {
                result.Message = "خطا سیستمی در دریافت وضعیت پرداخت";
            }



            result.ReferenceNumber = hybridPaymentRequest.Id.ToString();
            result.InstallmentCount = hybridPaymentRequest.InstallmentCount;
            result.CashPaymentPercentage = hybridPaymentRequest.CashPaymentPercentage;
            result.HybridPayments = hybridPaymentRequest.HybridPayments;
            return result;
        }
        catch (Exception ex)
        {
            // خطا رخ داده است
            result.StatusCode = (int)PaymentGatewayResultStatusCodeEnum.Unknown;
            result.ErrorCode = (int)PaymentGatewayErrorCodeEnum.SystemError;
            result.Message = "خطا سیستمی در دریافت وضعیت پرداخت";
            _logger.InsertLog(LogLevel.Error,
                (int)Events.GetEntityException, "IPG",
                $"خطا در بررسی نتیجه تراکنش پرداخت", ex);
            return result;
        }

    }
    /// <summary>
    /// وضعیت پرداخت حکمت را بر می گرداند، نتیجه پرداخت حکمت به همراه پیام نتیجه پرداخت را بر می گرداند
    /// </summary>
    /// <param name="merchantId">شناسه پذیرنده</param>
    /// <param name="terminalId">شناسه ترمینال</param>
    /// <param name="reservationNumber">شناسه پرداخت پذیرنده</param>
    /// <param name="signData">امضاء</param>
    /// <returns></returns>
    public async Task<HekmatPaymentStatusResultDto> GetHekmatPaymentStatusAsync(int merchantId, int terminalId,
        string reservationNumber, string signData)
    {
        var result = new HekmatPaymentStatusResultDto
        {
            ReservationNumber = reservationNumber
        };
        try
        {
            (var succeed, result.StatusCode, result.ErrorCode, result.Message) =
                await ValidatePaymentStatusAsync(merchantId, terminalId, reservationNumber, signData);
            if (!succeed)
            {
                return result;
            }
            var transaction =
                await _hekmatPaymentGatewayTransactionService.GetByReservationNumberAndTerminalIdAsync(
                    reservationNumber, terminalId);
            if (transaction == null)
            {
                result.StatusCode = (int)PaymentGatewayResultStatusCodeEnum.Unknown;
                result.ErrorCode = (int)PaymentGatewayErrorCodeEnum.TransactionNotFound;
                result.Message = "چنین تراکنشی یافت نشد";
                return result;
            }
            result.ErrorCode = (int)PaymentGatewayErrorCodeEnum.NotAnyError;
            result.StatusCode = transaction.StatusCode == 0
                ? (int)PaymentGatewayResultStatusCodeEnum.WaitingForReview
                : transaction.StatusCode == 1 && transaction.ErrorCode == 1
                    ? (int)PaymentGatewayResultStatusCodeEnum.Successful
                    : transaction.StatusCode == 2 && transaction.ErrorCode == 30
                        ? (int)PaymentGatewayResultStatusCodeEnum.Unsuccessful
                        : transaction.StatusCode == 50 && transaction.ErrorCode == 50
                            ? (int)PaymentGatewayResultStatusCodeEnum.Review
                            : (int)PaymentGatewayResultStatusCodeEnum.Unknown;

            result.Message = transaction.HekmatTransactionResultMessage;
            result.RefrenceNumber = transaction.Id.ToString();
            result.PaidDate = transaction.ApproveDateInt.ToString();
            return result;
        }
        catch (Exception ex)
        {
            // خطا رخ داده است
            result.StatusCode = (int)PaymentGatewayResultStatusCodeEnum.Unknown;
            result.ErrorCode = (int)PaymentGatewayErrorCodeEnum.SystemError;
            result.Message = "خطا سیستمی در دریافت وضعیت پرداخت حکمت";
            _logger.InsertLog(LogLevel.Error,
                (int)Events.GetEntityException, "HekmatIpg",
                $"خطا در بررسی نتیجه تراکنش پرداخت حکمت", ex);
            return result;
        }
    }

    private async Task<(bool succeed, int statusCode, int errorCode, string message)> ValidatePaymentStatusAsync(int merchantId, int terminalId, string reservationNumber, string signData)
    {
        string inputValidation = string.Empty;
        if (merchantId == 0)
        {
            inputValidation += "شناسه پذیرنده باید دارای مقدار باشد.";
        }

        if (terminalId == 0)
        {
            inputValidation += "شناسه ترمینال باید دارای مقدار باشد.";
        }

        if (string.IsNullOrWhiteSpace(reservationNumber))
        {
            inputValidation += "شناسه پرداخت پذیرنده باید دارای مقدار باشد.";
        }

        if (string.IsNullOrWhiteSpace(signData))
        {
            inputValidation += "امضاء باید دارای مقدار باشد.";
        }

        if (!string.IsNullOrEmpty(inputValidation))
        {
            return (false, (int)PaymentGatewayResultStatusCodeEnum.Unknown, (int)PaymentGatewayErrorCodeEnum.InvalidParameters, inputValidation);
        }
        var merchantInfo = await _merchantInfoService.GetAsync(merchantId, true);
        if (merchantInfo == null)
        {
            return (false, (int)PaymentGatewayResultStatusCodeEnum.Unknown, (int)PaymentGatewayErrorCodeEnum.MerchantNotFound, "پذیرنده با شناسه پذیرنده ارسالی یافت نشد");
        }

        string ipAddress = _webHelper.GetCurrentIpAddress();
        var merchantTerminal = await _yashilHekmatTerminalService.GetAsync(terminalId, true);
        if (merchantTerminal == null || merchantTerminal.MerchantId != merchantId ||
            merchantTerminal.IpAddress != ipAddress)
        {
            return (false, (int)PaymentGatewayResultStatusCodeEnum.Unknown, (int)PaymentGatewayErrorCodeEnum.InvalidTerminal, "ترمینال کاربر صحیح نمی باشد");
        }

        if (!merchantTerminal.IsActive)
        {
            return (false, (int)PaymentGatewayResultStatusCodeEnum.Unknown, (int)PaymentGatewayErrorCodeEnum.InactiveTerminal, "ترمینال کاربر فعال نمی باشد");
        }

        var data = $"{merchantId};{terminalId};{reservationNumber}";
        bool signatureIsOk = CryptoHelper.VerifySignature(data, signData, merchantTerminal.PublicKey);
        if (!signatureIsOk)
        {
            return (false, (int)PaymentGatewayResultStatusCodeEnum.Unknown, (int)PaymentGatewayErrorCodeEnum.InvalidSignature, "امضای دیجیتال معتبر نمی باشد");
        }

        return (true, 0, 0, string.Empty);
    }


    public async Task<(bool succeed, string message, int status, IpgToken token, YashilHekmatTerminalInfoDomainModel
        terminalInfo)> ValidateAndExtractEncryptedTokenData(string encryptedToken)
    {
        if (string.IsNullOrEmpty(encryptedToken))
        {
            return (false, "مقدار توکن ارسال نشده است", 1, null, null);
        }

        string token;
        try
        {
            token = CryptographyHelper.AesDecrypt(encryptedToken);
        }
        catch (Exception)
        {
            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                $"token is not valid in HybridIpgController index with token ={encryptedToken}", null);
            return (false, "توکن معتبر نمی باشد", 1, null, null);
        }

        var ipgToken = PaymentHelper.GetPrepareTokenData(token);
        // بررسی معتبر بودن شماره کارت حکمت
        if (!string.IsNullOrEmpty(ipgToken.CardNumber) && !ipgToken.CardNumber.IsValidValidateHekmatCard())
        {
            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                $"card is not valid in HybridIpgController index with cardNumber={ipgToken.CardNumber} and merchant={ipgToken.MerchantId} and terminalId={ipgToken.TerminalId}",
                null);
            return (false, "شماره کارت ارسالی معتبر نمی باشد", 5, ipgToken, null);
        }

        if (ipgToken.CashPaymentPercentage.HasValue &&
            (ipgToken.CashPaymentPercentage > 100 || ipgToken.CashPaymentPercentage < 0))

        {
            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                $"CashPaymentPercentage is not valid in HybridIpgController index with CashPaymentPercentage={ipgToken.CashPaymentPercentage} and merchant={ipgToken.MerchantId} and terminalId={ipgToken.TerminalId}",
                null);
            return (false, "درصد پرداخت نقدی باید بین 0 تا 100 باشد", 5, ipgToken, null);
        }

        bool tokenIsExpired = PaymentHelper.IsTokenExpired(ipgToken.TokenExpireDateTime);
        if (tokenIsExpired)
        {
            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                $"token is expired in HybridIpgController index ", null);
            return (false, "زمان اعتبار توکن منقضی شده است", 4, ipgToken, null);
        }

        
        var terminalInfo = await _yashilHekmatTerminalService.GetYashilHekmatTerminalByIdAndIp(ipgToken.MerchantId,
            ipgToken.TerminalId,
            ipgToken.IpAddress, _applicationConfiguration.ApplicationId).Select(x =>
            new YashilHekmatTerminalInfoDomainModel
            {
                Id = x.Id,
                IsActive = x.IsActive,
                MerchantName = x.Merchant.MerchantName,
                Url = x.Url,
                SupplierWage = x.SupplierWage,
                InstituteWage = x.InstituteWage,
                CashInstituteWage = x.CashInstituteWage,
                CashSupplierWage = x.CashSupplierWage
            }).FirstOrDefaultAsync();
        if (terminalInfo == null)
        {
            _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                $"terminal info is not valid in HybridIpgController index with ip={ipgToken.IpAddress} and merchant={ipgToken.MerchantId} and terminalId={ipgToken.TerminalId}",
                null);
            return (false, "اطلاعات پذیرنده معتبر نمی باشد", 3, ipgToken, null);
        }

        return (true, string.Empty, 0, ipgToken, terminalInfo);
    }

    public async Task<(bool succeeded, string message, int status, HybridPayment hybridPayment, IpgToken ipgToken)>
        ValidateAndCreateHekmatHybridPaymentAsync(string paymentRequestToken, int? cashPaymentPercentage)
    {
        // استخراج اطلاعات توکن و پذیرنده و بررسی معتبر بودن توکن
        var (succeeded, message, status, ipgToken, terminalInfo) =
            await ValidateAndExtractEncryptedTokenData(paymentRequestToken);

        if (!succeeded || ipgToken == null || terminalInfo == null)
        {
            return (false, message, status, null, null);
        }

        // بررسی میکنیم که یا باید توکن مقدار درصد نقدی را داشته باشد یا در کنار توکن درصد نیز ارسال شده باشد
        // نمی تواند هر دو مقدار داشته باشد
        if ((ipgToken.CashPaymentPercentage.HasValue && cashPaymentPercentage.HasValue)
            || (!ipgToken.CashPaymentPercentage.HasValue && !cashPaymentPercentage.HasValue))
        {
            return (false, "اطلاعات ارسالی معتبر نمی باشد", 1, null, null);
        }

        // بررسی میکنم که قبلا درخواست تراکنشی ثبت شده است یا خیر
        var (remainingHekmatPrice, _, hybridPaymentRequest) = await
            _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(ipgToken.MerchantId, ipgToken.TerminalId,
                ipgToken.ReservationNumber, ipgToken.UseFormula, _applicationConfiguration.ApplicationId);

        // اگر قبلا این اطلاعات وجود داشته باشد باید اطلاعات را بررسی کنیم که با قبلی یکی باشد در این صورت باید پیام شماره تراکنش تکراری بدهیم
        if (hybridPaymentRequest != null)
        {
            if (hybridPaymentRequest.AdditionalData != ipgToken.AdditionalData ||
                // نباید درصد را مقایسه کنیم زیرا ممکن است درصد توسط پذیرنده ارسال نشده باشد و کاربر درصد را مشخص کرده باشد 
                // آنگاه پذیرنده از درصد با خبر نیست
                //hybridPaymentRequest.CashPaymentPercentage != ipgToken.CashPaymentPercentage ||
                hybridPaymentRequest.BasePrice != ipgToken.Amount ||
                hybridPaymentRequest.InstallmentCount != ipgToken.InstallmentCount)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                    $"result of GetHybridPaymentRequestDtoWithRemainingPricesAsync:requested token is not valid with old hybridPaymentRequest data with token ={paymentRequestToken}",
                    null);
                return (false, "توکن معتبر نمی باشد", 1, null, null);
            }

            if (hybridPaymentRequest.HybridPaymentRequestStatusType !=
                (int)HybridPaymentRequestStatusTypeEnum.InitialRegistration
                && hybridPaymentRequest.HybridPaymentRequestStatusType !=
                (int)HybridPaymentRequestStatusTypeEnum.IncompletePayment)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                    $"result of GetHybridPaymentRequestDtoWithRemainingPricesAsync:requested hybridPaymentRequest is not valid for hybridPaymentRequestStatus  hybridPaymentRequestId= {hybridPaymentRequest.Id}",
                    null);
                return (false, "کاربر گرامی شما امکان پرداخت برای این درخواست را ندارید", 1, null, null);
            }

            // حال که اطلاعات ارسالی معتبر است و قبلا درخواست وجود داشته
            // بررسی میکنیم که این درخواست تراکنش تعیین تکلیف نشده نداشته باشد
            // فرقی نمیکند که چه تراکنشی باشد از هر نوع تراکنشی نباید تعیین تکلیف نشده داشته باشد
            var hasAnyNotDeterminedPayment = hybridPaymentRequest.HybridPayments
                .Any(x => x.HybridPaymentStatusType == (int)HybridPaymentStatusTypeEnum.SendToPaymentGateway
                          || x.HybridPaymentStatusType ==
                          (int)HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway);
            if (hasAnyNotDeterminedPayment)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                    $"result of GetHybridPaymentRequestDtoWithRemainingPricesAsync:old hybridPaymentRequest Have HybridPayments Status : SendToPaymentGateway or WaitingResponseFromPaymentGateway. hybridPaymentRequestId= {hybridPaymentRequest.Id}",
                    null);
                return (false,
                    "کاربر گرامی شما تراکنش تعیین تکلیف نشده دارید و تا زمان مشخص شدن وضعیت آن ها امکان پرداخت برای شما وجود ندارد",
                    1, null, null);
            }

            if (remainingHekmatPrice <= 0)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                    $"result of GetHybridPaymentRequestDtoWithRemainingPricesAsync:requested hybridPaymentRequest exists old hybridPaymentRequest but remainingHekmatPrice <= 0 . hybridPaymentRequestId= {hybridPaymentRequest.Id}",
                    null);
                return (false, "مبلغی برای پرداخت اقساطی وجود ندارد", 1, null, null);
            }

            // برای مبلغ باقیمانده یک پرداخت حکمت ثبت می کنیم
            // این حالت زمانی رخ میدهد که اصلاح قیمت افزایشی حکمت رخ داده باشد و پرداخت 
            // جدید برای حکمت درج می کنیم
            try
            {
                var hybridPayment = await CreateAndInsertHybridPaymentAsync(remainingHekmatPrice,
                    hybridPaymentRequest.InstallmentCount, hybridPaymentRequest.Id,
                    SaleTypesEnum.InstallmentPayment, PaymentMethodTypesEnum.Hekmat);
                return (true, string.Empty, 0, hybridPayment, ipgToken);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(HybridPaymentRequest),
                    $"error while inserting hybridPayment for HekmatPayment", e);
                return (false, "خطا در انجام عملیات", 1, null, null);
            }
        }
        else
        {
            // از این پس با cashPaymentPercentage کار میکنیم 
            if (ipgToken.CashPaymentPercentage.HasValue)
            {
                cashPaymentPercentage = ipgToken.CashPaymentPercentage;
            }

            try
            {
                var newHybridPaymentRequest =
                    await CreateAndInsertHybridPaymentRequestAsync(cashPaymentPercentage, ipgToken,
                        terminalInfo);
                // چون تاکنون درخواستی وجود نداشته پس مبلغ حکمت را مستقیما درج میکنیم 
                var hekmatPrice = _hybridPaymentRequestService.CalculateHekmatPrice(
                    ipgToken.Amount,
                    cashPaymentPercentage.Value,
                    ipgToken.InstallmentCount,
                    newHybridPaymentRequest.InstituteWage,
                    newHybridPaymentRequest.SupplierWage,
                    newHybridPaymentRequest.Tax,
                    newHybridPaymentRequest.ShippingAmount,
                    ipgToken.UseFormula
                    );
                // مبلغ کل اقساطی باید از مبلغ تنظیم شده در appConfig
                // بیشتر باشد
                (var isValidHekmatPrice, var hekmatPriceValidateMessage) = await ValidateHekmatPriceAsync(hekmatPrice);
                if (!isValidHekmatPrice)
                {
                    return (false, hekmatPriceValidateMessage, 1, null, null);
                }
                var hybridPayment = await CreateAndInsertHybridPaymentAsync(hekmatPrice,
                    ipgToken.InstallmentCount,
                    newHybridPaymentRequest.Id,
                    SaleTypesEnum.InstallmentPayment,
                    PaymentMethodTypesEnum.Hekmat);
                return (true, string.Empty, 0, hybridPayment, ipgToken);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(HybridPaymentRequest),
                    "error while inserting hybridPaymtRequest and hybridPayment for hekmatPayment", e);
                return (false, "خطا در انجام عملیات", 1, null, null);
            }
        }
    }
    /// <summary>
    /// مبلغ کل اقساطی باید از مبلغ تنظیم شده در appConfig
    /// بیشتر باشد
    /// </summary>
    /// <param name="price">مبلغ کل اقساطی</param>
    /// <returns></returns>
    public async Task<(bool isValid, string message)> ValidateHekmatPriceAsync(long price)
    {
        var hekmatMinimumPriceToPay = await _appConfigService.GetValueByApplicationIdAsync(
            ApplicationConfigKey.YashilHekmatMinimumPriceToPay,
            _applicationConfiguration.ApplicationId);


        if (hekmatMinimumPriceToPay != 0 && price > 0
            && price <= hekmatMinimumPriceToPay)
        {
            return (false, $"کاربر گرامی مبلغ اقساطی کمتر از {CurrencyUtility.FormatPrice(hekmatMinimumPriceToPay)} تومان است و قابل پرداخت نمی باشد.");
        }

        return (true, string.Empty);
    }

    public async Task<(bool succeeded, string message, int status, HybridPayment hybridPayment, IpgToken ipgToken)>
        ValidateAndCreateCashHybridPaymentAsync(string paymentRequestToken, int? cashPaymentPercentage)
    {
        // استخراج اطلاعات توکن و پذیرنده و بررسی معتبر بودن توکن
        var (succeeded, message, _, ipgToken, terminalInfo) =
            await ValidateAndExtractEncryptedTokenData(paymentRequestToken);
        if (!succeeded || ipgToken == null || terminalInfo == null)
        {
            return (false, message, 1, null, null);
        }

        // بررسی میکنیم که یا باید توکن مقدار درصد نقدی را داشته باشد یا در کنار توکن درصد نیز ارسال شده باشد
        // نمی تواند هر دو مقدار داشته باشد
        if ((ipgToken.CashPaymentPercentage.HasValue && cashPaymentPercentage.HasValue)
            || (!ipgToken.CashPaymentPercentage.HasValue && !cashPaymentPercentage.HasValue))
        {
            return (false, "اطلاعات ارسالی معتبر نمی باشد", 1, null, null);
        }

        // بررسی میکنم که قبلا درخواست تراکنشی ثبت شده است یا خیر
        var (_, remainingCashPrice, hybridPaymentRequest) =
            await _hybridPaymentRequestService.GetHybridPaymentRequestDtoWithRemainingPricesAsync(ipgToken.MerchantId,
                ipgToken.TerminalId, ipgToken.ReservationNumber, ipgToken.UseFormula, _applicationConfiguration.ApplicationId);
        // اگر قبلا این اطلاعات وجود داشته باشد باید اطلاعات را بررسی کنیم که با قبلی یکی باشد در این صورت باید پیام شماره تراکنش تکراری بدهیم
        if (hybridPaymentRequest != null)
        {
            if (hybridPaymentRequest.AdditionalData != ipgToken.AdditionalData ||
                // نباید درصد را مقایسه کنیم زیرا ممکن است درصد توسط پذیرنده ارسال نشده باشد و کاربر درصد را مشخص کرده باشد 
                // آنگاه پذیرنده از درصد با خبر نیست
                //hybridPaymentRequest.CashPaymentPercentage != ipgToken.CashPaymentPercentage ||
                hybridPaymentRequest.BasePrice != ipgToken.Amount ||
                hybridPaymentRequest.InstallmentCount != ipgToken.InstallmentCount)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                    $"requested token is not valid with old hybridPaymentRequest data with token ={paymentRequestToken}",
                    null);
                return (false, "توکن معتبر نمی باشد", 1, null, null);
            }

            if (hybridPaymentRequest.HybridPaymentRequestStatusType !=
                (int)HybridPaymentRequestStatusTypeEnum.InitialRegistration
                && hybridPaymentRequest.HybridPaymentRequestStatusType !=
                (int)HybridPaymentRequestStatusTypeEnum.IncompletePayment)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HybridPaymentRequest),
                    $"requested hybridPaymentRequest is not valid for hybridPaymentRequestStatus  hybridPaymentRequestId= {hybridPaymentRequest.Id}",
                    null);
                return (false, "کاربر گرامی شما امکان پرداخت برای این درخواست را ندارید", 1, null, null);
            }

            // حال که اطلاعات ارسالی معتبر است و قبلا درخواست وجود داشته
            // بررسی میکنیم که این درخوات تراکنش تعیین تکلیف شده نداشته باشد
            // فرقی نمیکند که چه تراکنشی باشد از هر نوع تراکنشی نباید تعیین تکلیف نشده داشته باشد
            var hasAnyNotDeterminedPayment = hybridPaymentRequest.HybridPayments
                .Any(x => x.HybridPaymentStatusType == (int)HybridPaymentStatusTypeEnum.SendToPaymentGateway
                          || x.HybridPaymentStatusType ==
                          (int)HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway);
            if (hasAnyNotDeterminedPayment)
            {
                return (false,
                    "کاربر گرامی شما تراکنش تعیین تکلیف نشده دارید و تا زمان مشخص شدن تراکنش قبلی امکان پرداخت ندارید",
                    1, null, null);
            }

            // حال مبلغ باقیمانده اقساطی را محاسبه میکنیم
            // ممکن است قبلا پرداخت اقساطی داشته باشد و بعدا مبلغ اصلاح شود
            // که البته الان این موضوع کاربردی ندارد ولی بعدا برای اصلاح تراکنشات این مورد درست است
            if (remainingCashPrice <= 0)
            {
                return (false, "مبلغی برای پرداخت نقدی وجود ندارد", 1, null, null);
            }

            // برای مبلغ باقیمانده یک درخواست ثبت میکنیم
            try
            {
                var hybridPayment = await CreateAndInsertHybridPaymentAsync(remainingCashPrice,
                    null, hybridPaymentRequest.Id, SaleTypesEnum.CashPayment, PaymentMethodTypesEnum.Ansar);
                return (true, string.Empty, 0, hybridPayment, ipgToken);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(HybridPaymentRequest),
                    $"error while inserting hybridPayment for cashPayment", e);
                return (false, "خطا در انجام عملیات", 1, null, null);
            }
        }
        else
        {
            // از این پس با cashPaymentPercentage کار میکنیم 
            if (ipgToken.CashPaymentPercentage.HasValue)
            {
                cashPaymentPercentage = ipgToken.CashPaymentPercentage;
            }

            // اگر قبلا درخواستی وجود نداشته باشد پس قطعا درخواست اول است و باید درصد پرداخت 100 درصد باشد در غیر این صورت خطایی وجود دارد
            // زیرا بیزینس به این گونه است که ابتدا پرداخت اقساطی صورت گیرد
            // تنها در حالتی ممکن است در این قسمت از کد باشیم که درصد پردخت نقدی 100 باشد 
            if (cashPaymentPercentage != 100)
            {
                return (false, "اطلاعات ارسالی معتبر نمی باشد", 1, null, null);
            }

            try
            {
                var newHybridPaymentRequest =
                    await CreateAndInsertHybridPaymentRequestAsync(cashPaymentPercentage, ipgToken, terminalInfo);
                // چون تاکنون درخواستی وجود نداشته پس مبلغ حکمت را مستقیما درج میکنیم 
                var cashPrice = _hybridPaymentRequestService.CalculateCashPrice
                   (ipgToken.Amount,
                    cashPaymentPercentage.Value,
                    newHybridPaymentRequest.CashInstituteWage,
                    newHybridPaymentRequest.Tax,
                    newHybridPaymentRequest.ShippingAmount, ipgToken.UseFormula);

                var hybridPayment = await CreateAndInsertHybridPaymentAsync(cashPrice, null,
                    newHybridPaymentRequest.Id, SaleTypesEnum.CashPayment, PaymentMethodTypesEnum.Ansar);
                return (true, string.Empty, 0, hybridPayment, ipgToken);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InsertException, nameof(HybridPaymentRequest),
                    $"error while inserting hybridPaymtRequest and hybridPayment for cashPayment", e);
                return (false, "خطا در انجام عملیات", 1, null, null);
            }
        }
    }


    /// <summary>
    /// متد ایجاد یک رکورد hybridPayment در پایگاه داده
    /// </summary>
    /// <param name="remainingHekmatPaymentPrice"></param>
    /// <param name="installmentCount"></param>
    /// <param name="hybridPaymentRequestId"></param>
    /// <param name="saleTypesEnum"></param>
    /// <param name="paymentMethodTypesEnum"></param>
    /// <returns></returns>
    private async Task<HybridPayment> CreateAndInsertHybridPaymentAsync(long remainingHekmatPaymentPrice,
        int? installmentCount, int hybridPaymentRequestId, SaleTypesEnum saleTypesEnum,
        PaymentMethodTypesEnum paymentMethodTypesEnum
    )
    {
        var hybridPayment = new HybridPayment
        {
            Price = remainingHekmatPaymentPrice,
            HybridPaymentRequestId = hybridPaymentRequestId,
            SaleType = (int)saleTypesEnum,
            InstallmentCount = installmentCount,
            HybridPaymentStatusType = (int)HybridPaymentStatusTypeEnum.SendToPaymentGateway,
            PaymentMethodType = (int)paymentMethodTypesEnum,
            CreateBy = _applicationConfiguration.SysUserId,
            CreationDate = DateTime.Now,
            ApplicationId = _applicationConfiguration.ApplicationId,
            PaidDateUtcInt = DateFunction.ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            PaidDateInt = DateFunction.ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            AccessLevelId = 0,
            Deleted = false,
            ReservationNumber = string.Empty,
            Title = string.Empty,
            CreatorOrganizationId = 1
        };
        var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            hybridPayment = await _hybridPaymentService.AddAsync(hybridPayment,true);
            hybridPayment.ReservationNumber = $"HP-{hybridPayment.Id}";
            await _hybridPaymentService.UpdateAsync(hybridPayment, hybridPayment.Id, true);
            await transaction.CommitAsync();
            return hybridPayment;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// متد ایجاد و درج در پایگاه داده 
    /// </summary>
    /// <param name="cashPaymentPercentage"></param>
    /// <param name="ipgToken"></param>
    /// <param name="terminalInfo"></param>
    /// <returns></returns>
    private async Task<HybridPaymentRequest> CreateAndInsertHybridPaymentRequestAsync(
        int? cashPaymentPercentage, IpgToken ipgToken,
        YashilHekmatTerminalInfoDomainModel terminalInfo)
    {
        var taxPercentage = await _hybridPaymentService.GetTaxPercentageAsync();
        var newHybridPaymentRequest = new HybridPaymentRequest
        {
            BasePrice = ipgToken.Amount,
            MerchantId = ipgToken.MerchantId,
            CallBackUrl = ipgToken.ReturnUrl,
            AdditionalDate = ipgToken.AdditionalData,
            Tax = !ipgToken.IsTaxExempt && taxPercentage.HasValue ? Convert.ToInt32(taxPercentage.Value) : 0,
            Title = string.Empty,
            LocalReservationNumber = ipgToken.ReservationNumber,
            HekmatCardNumber = ipgToken.CardNumber,
            InstallmentCount = ipgToken.InstallmentCount,
            SupplierWage = terminalInfo.SupplierWage,
            InstituteWage = terminalInfo.InstituteWage,
            CashInstituteWage = terminalInfo.CashInstituteWage,
            CashSupplierWage = terminalInfo.CashSupplierWage,
            CashPaymentPercentage = ipgToken.CashPaymentPercentage ??
                                    cashPaymentPercentage,
            HybridPaymentRequestStatusType = (int)HybridPaymentRequestStatusTypeEnum.InitialRegistration,
            CreateBy = _applicationConfiguration.SysUserId,
            CreationDate = DateTime.Now,
            ApplicationId = _applicationConfiguration.ApplicationId,
            AccessLevelId = 0,
            Deleted = false,
            CreatorOrganizationId = 1,
            YashilHekmatTerminalId = ipgToken.TerminalId,
            ShippingAmount = ipgToken.ShippingAmount,
            UseFormula = ipgToken.UseFormula

        };
        var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            newHybridPaymentRequest = await _hybridPaymentRequestService.AddAsync(newHybridPaymentRequest, true);
            newHybridPaymentRequest.Title = $"HYP-{newHybridPaymentRequest.Id}";
            await _hybridPaymentRequestService.UpdateAsync(newHybridPaymentRequest, newHybridPaymentRequest.Id, true);
            await transaction.CommitAsync();
            return newHybridPaymentRequest;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}