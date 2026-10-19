using System;
using System.Net.Http;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Configurations;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Util;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.Implementations;
using YashilPayment.Core.Const;
using YashilPayment.Core.DTO;
using YashilPayment.Core.Enums;
using YashilPayment.Core.Services;
using YashilUserManagement.Core.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class HekmatOnlinePaymentGatewayService : IHekmatOnlinePaymentGatewayService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IYashilLogger<Payment> _logger;
        private readonly IHekmatOnlinePaymentConfiguration _hekmatOnlinePaymentConfiguration;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IApplicationService _applicationService;
        private readonly PaymentTransactionUtil _paymentTransactionUtil;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly YashilAppDbContext _walletContext;
        private readonly IUserPrincipal _principal;
        private readonly IWalletSettlementService _walletSettlement;

        public HekmatOnlinePaymentGatewayService(IHttpClientFactory httpClientFactory, IYashilLogger<Payment> logger,
            IHekmatOnlinePaymentConfiguration hekmatOnlinePaymentConfiguration, IOrderService orderService, IPaymentService paymentService,
            IApplicationService applicationService, PaymentTransactionUtil paymentTransactionUtil, ApplicationConfiguration applicationConfiguration,
            YashilAppDbContext walletContext, IUserPrincipal principal, IWalletSettlementService walletSettlement)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _hekmatOnlinePaymentConfiguration = hekmatOnlinePaymentConfiguration;
            _orderService = orderService;
            _paymentService = paymentService;
            _applicationService = applicationService;
            _paymentTransactionUtil = paymentTransactionUtil;
            _applicationConfiguration = applicationConfiguration;
            _walletContext = walletContext;
            _principal = principal;
            _walletSettlement = walletSettlement;
        }

        #region Private Method

        /// <summary>
        /// ساخت توکن
        /// </summary>
        /// <param name="amount">مبلغ</param>
        /// <param name="reservationNumber">شناسه پیگیری سمت پذیرنده</param>
        /// <param name="installmentsCount">تعداد اقساط</param>
        /// <returns></returns>
        private async Task<(string token, string signature, string errorMessae, bool isSuccess)>
          GetToken(
          long amount,
          string reservationNumber,
          int? installmentsCount)
        {
            if (amount == 0)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"شناسه سفارش برای درج پرداخت و ارسال به درگاه مقدار ندارد",
                    null);
                return (string.Empty, string.Empty, "مبلغ پرداخت مقدار ندارد", false);
            }
            var hekmatPaymentOnlineGetTokenDto = new HekmatPaymentOnlineTokenRequestDto
            {
                Amount = amount,
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(_hekmatOnlinePaymentConfiguration.Password)),
                MerchantId = _hekmatOnlinePaymentConfiguration.MerchantId,
                TerminalId = _hekmatOnlinePaymentConfiguration.TerminalId,
                RedirectUrl = _hekmatOnlinePaymentConfiguration.CallbackUrl,
                TransactionType = HekmatOnlinePaymentTransactionTypeConst.Buy,
                ReservationNumber = reservationNumber,
                LocalDateTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                SaleInfo = new HekmatOnlinePaymentGatewaySaleInfo
                {
                    InstallmentsCount = installmentsCount ?? 0
                }

            };
            var signData = $"{hekmatPaymentOnlineGetTokenDto.MerchantId}&{hekmatPaymentOnlineGetTokenDto.LocalDateTime}&{hekmatPaymentOnlineGetTokenDto.TransactionType}&" +
                           $"{hekmatPaymentOnlineGetTokenDto.TerminalId}&{hekmatPaymentOnlineGetTokenDto.Amount}&{hekmatPaymentOnlineGetTokenDto.ReservationNumber}";
            var rsa = new RSACryptoServiceProvider();
            var privateKey = _hekmatOnlinePaymentConfiguration.SitePrivateKey;
            rsa.ImportFromPem(privateKey.ToCharArray());
            byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(signData), SHA512.Create());
            hekmatPaymentOnlineGetTokenDto.Signature = Convert.ToBase64String(signedData);
            var client = _httpClientFactory.CreateClient("Payment");
            var request = new HttpRequestMessage(HttpMethod.Post,
                _hekmatOnlinePaymentConfiguration.GenerateTokenUrl);

            var data = JsonSerializer.Serialize(hekmatPaymentOnlineGetTokenDto,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

            request.Content = new StringContent(data,
                Encoding.UTF8, "application/json");
            try
            {
                var response = await client.SendAsync(request);


                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse =
                        JsonConvert.DeserializeObject<HekmatOnlinePaymentGatewayErrorResponseDto>(responseContent);
                    _logger
                        .InsertLog(LogLevel.Error,
                            (int)Events.PaymentException,
                            "Payment",
                            $"HekmatOnlinePaymentGateway  : GetTokenMethod Error | " +
                            $"clientTxnId / ReservationNumber : {reservationNumber} | " +
                            $"ErrorId: {errorResponse?.ErrorId} | " +
                            $"ErrorCode: {errorResponse?.ErrorCode} | " +
                            $"Message: {errorResponse?.ErrorMessage} | " +
                            $"HttpStatus: {errorResponse?.HttpStatus} | " +
                            $"Response: {responseContent}",
                            null);
                    return (string.Empty, string.Empty, "خطا در ساخت توکن ", false);

                }
                var generateTokenResponseModel = JsonSerializer
                    .Deserialize<HekmatPaymentOnlineTokenResponseDto>(responseContent
                        , new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                // دریافت توکن موفق بود
                if (generateTokenResponseModel is
                    {
                        Signature: not null and not "",
                        Token: not null and not ""
                    }
                    && VerifySignature(
                        generateTokenResponseModel.Token,
                        generateTokenResponseModel.Signature,
                        _hekmatOnlinePaymentConfiguration.HekmatIpgPublicKey
                        )
                    )
                {

                    return (generateTokenResponseModel.Token, generateTokenResponseModel.Signature, string.Empty, true);
                }

                _logger
                    .InsertLog(LogLevel.Error,
                        (int)Events.PaymentException,
                        "Payment", $"خطا در ساخت توکن درگاه پرداخت آنلاین حکمت برای سفارش با کد:" +
                                   $" {reservationNumber} --"
                        ,
                        null);
                return (string.Empty, string.Empty, "توکن مقدار ندارد یا ", false);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"خطا در ساخت توکن برای سفارش با کد: {reservationNumber}", e);
                return (string.Empty, string.Empty, "خطا در ساخت توکن برای سفارش", false);
            }
        }

        /// <summary>
        /// متد تایید تراکنش آنلاین حکمت
        /// این متد در
        /// CallBack Url
        /// برای تایید تراکنش فراخوانی می شود
        /// </summary>
        /// <param name="token">توکن ارسالی از سمت درگاه حکمت انلاین</param>
        /// <param name="reservationNumber">شناسه ارسالی از سمت سایت </param>
        /// <returns></returns>
        private async Task<(string transactionId, string errorMessae, bool isSuccess)> ConfirmPayAsync
            (string token, string reservationNumber)
        {
            var confirmPayRequestDto = new HekmatOnlinePaymentGatewayTransactionConfirmPayRequestDto
            {

                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(_hekmatOnlinePaymentConfiguration.Password)),
                MerchantId = _hekmatOnlinePaymentConfiguration.MerchantId,
                Token = token

            };
            var signData = $"{confirmPayRequestDto.MerchantId}&{confirmPayRequestDto.Token}";
            var rsa = new RSACryptoServiceProvider();
            var privateKey = _hekmatOnlinePaymentConfiguration.SitePrivateKey;
            rsa.ImportFromPem(privateKey.ToCharArray());
            byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(signData), SHA512.Create());
            confirmPayRequestDto.Signature = Convert.ToBase64String(signedData);
            var client = _httpClientFactory.CreateClient("Payment");
            var request = new HttpRequestMessage(HttpMethod.Post,
                _hekmatOnlinePaymentConfiguration.VerifyTransactionUrl);

            var data = JsonSerializer.Serialize(confirmPayRequestDto,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

            request.Content = new StringContent(data,
                Encoding.UTF8, "application/json");

            try
            {
                var response = await client.SendAsync(request);

                var res = await response.Content.ReadAsStringAsync();

                var confirmPayResponse = JsonSerializer
                    .Deserialize<HekmatOnlinePaymentGatewayTransactionConfirmPayResponseDto>
                        (res);
                // دریافت توکن موفق بود
                if (confirmPayResponse is
                    {
                        Signature: not null and not "",
                        TransactionId: not null and not ""
                    } && CryptoHelper.VerifySignature(confirmPayResponse.TransactionId, confirmPayResponse.Signature,
                        _hekmatOnlinePaymentConfiguration.HekmatIpgPublicKey))


                {
                    return (confirmPayResponse.TransactionId, string.Empty, true);
                }

                _logger
                    .InsertLog(LogLevel.Error,
                        (int)Events.PaymentException,
                        "Payment", " کد تراکنش یا امضا برابر مقدار نامعتبر دارد - متد تایید تایید تراکنش با کد:" +
                                   $" {reservationNumber} -- Signature : {confirmPayResponse?.Signature} -- TransactionId : {confirmPayResponse?.TransactionId}", null);
                return (string.Empty, string.Empty, false);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"خطا در تایید تراکنش برای سفارش با کد: {reservationNumber}", e);
                return (string.Empty, "خطا در تایید تراکنش", false);
            }
        }

        private bool VerifySignature(
            string token,
            string signature,
            string publicKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(token);
            byte[] signatureBytes = Convert.FromBase64String(signature);

            using RSA rsa = RSA.Create();

            rsa.ImportFromPem(publicKey);

            var res = rsa.VerifyData(
                data,
                signatureBytes,
                HashAlgorithmName.SHA512,
                RSASignaturePadding.Pkcs1
            );
            return res;
        }

        /// <summary>
        /// بررسی پارامتر های مهمی که از سمت درگاه به
        /// CallBackUrl
        /// ارسال می شود
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        private HekmatOnlinePaymentGatewayCallbackValidationResult ValidateCallbackParameters(
            HekmatOnlinePaymentGatewayCallbackDto callback)
        {
            var data = $"{callback.Token}&{callback.MerchantId}&" +
                                      $"{callback.TransactionType}&{callback.TerminalId}&{callback.Amount}&{callback.ReservationNumber}";

            //if (!CryptoHelper.VerifySignature(data, callback.Signature,
            //        _hekmatOnlinePaymentConfiguration.HekmatIpgPublicKey))
            //{
            //    return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(HekmatOnlinePaymentGatewayVerificationMessageConst.VerifyDataWithSignatureInvalid);
            //}

            if (string.IsNullOrEmpty(callback.Status))
                return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(
                    string.Format(HekmatOnlinePaymentGatewayVerificationMessageConst.IsInvalidCallBackParam, "وضعیت"));

            if (string.IsNullOrEmpty(callback.ReservationNumber))
                return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(
                    string.Format(HekmatOnlinePaymentGatewayVerificationMessageConst.IsInvalidCallBackParam, "کد تراکنش سایت"));

            if (string.IsNullOrEmpty(callback.RetrievalReferenceNumber))
                return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(
                                string.Format(HekmatOnlinePaymentGatewayVerificationMessageConst.IsInvalidCallBackParam, "کد مرجع تراکنش"));

            if (callback.SystemTraceAuditNumber is < 100000 or > 999999)
                return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(
                    string.Format(HekmatOnlinePaymentGatewayVerificationMessageConst.IsInvalidCallBackParam, "شماره پیگیری سیستمی تراکنش"));

            if (string.IsNullOrEmpty(callback.Token))
                return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(
                    string.Format(HekmatOnlinePaymentGatewayVerificationMessageConst.IsInvalidCallBackParam, "توکن"));

            if (string.IsNullOrEmpty(callback.Signature))
                return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(
                        string.Format(HekmatOnlinePaymentGatewayVerificationMessageConst.IsInvalidCallBackParam, "امضای الکترونیکی"));

            if (callback.Amount <= 0)
                return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(
                    string.Format(HekmatOnlinePaymentGatewayVerificationMessageConst.IsInvalidCallBackParam, "مبلغ"));

            if (string.IsNullOrEmpty(callback.TransactionDate))
                return HekmatOnlinePaymentGatewayCallbackValidationResult.Failure(
                    string.Format(HekmatOnlinePaymentGatewayVerificationMessageConst.IsInvalidCallBackParam, "تاریخ تراکنش"));

            // در صورت نیاز:
            // بررسی فرمت
            // بررسی طول
            // بررسی مقادیر مجاز
            // بررسی ارتباط پارامترها با یکدیگر

            return HekmatOnlinePaymentGatewayCallbackValidationResult.Success();
        }

        /// <summary>
        /// به روز رسانی تراکنش پرداخت و سفارش
        /// </summary>
        /// <param name="verifyPaymentViewModel">پرارمتر های بررسی پرداخت</param>
        /// <param name="paymentId">شناسه پرداخت</param>
        /// <param name="paymentStatusType">وضعیت چدید پرداخت</param>
        /// <param name="oldPaymentStatusType">وضعیت قبلی پرداخت</param>
        /// <param name="orderId">شناسه سفارش</param>
        /// <param name="orderStatusType">وضعیت جدید سفارش</param>
        /// <param name="paymentResultMessage">پیام جهت بروزرسانی پرداخت</param>
        /// <param name="updateFailedMessage">پیام خطا در بروز رسانی تراکنش پرداخت</param>
        /// <returns></returns>
        private async Task UpdatePaymentAndOrderByInquiryParamsAsync(
            VerifyPaymentViewModel verifyPaymentViewModel,
            int paymentId,
            PaymentStatusTypesEnum paymentStatusType,
            PaymentStatusTypesEnum oldPaymentStatusType,
            int orderId,
            OrderStatusTypesEnum orderStatusType,
            string paymentResultMessage,
            string updateFailedMessage
            )
        {
            try
            {
                verifyPaymentViewModel.Message = paymentResultMessage;
                verifyPaymentViewModel.PaymentStatusType = paymentStatusType;

                await _orderService.UpdateOrderStatusAsync(orderId, orderStatusType);

                await _paymentService.UpdatePaymentStatusAsync(paymentId,
                    paymentStatusType,
                    PaymentMethodTypesEnum.HekmatOnline,
                    verifyPaymentViewModel.Message,
                    null,
                    null,
                    null,
                    null,
                true,
                    verifyPaymentViewModel.Token,
                    verifyPaymentViewModel.RetrievalReferenceNumber,
                    verifyPaymentViewModel.SystemTraceAuditNumber);


            }
            catch (Exception ex)
            {

                var message = "HekmatOnlinePaymentGatewayService = >" +
                              " VerifyPaymentTransactionByTokenAsync Method => paymentId :" + paymentId +
                              " ,RetrievalReferenceNumber / Rrn :" + verifyPaymentViewModel.RetrievalReferenceNumber +
                              " ,ReservationNumber : " + verifyPaymentViewModel.ReservationNumber +
                              " ,SystemTraceAuditNumber / Stan : " + verifyPaymentViewModel.SystemTraceAuditNumber;

                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", message, ex);

                await _paymentService.UpdatePaymentStatusAsync(paymentId,
                    oldPaymentStatusType,
                    PaymentMethodTypesEnum.HekmatOnline,
                    updateFailedMessage, null,
                    null, null,
                    null, true,
                    verifyPaymentViewModel.Token,
                    verifyPaymentViewModel.RetrievalReferenceNumber,
                    verifyPaymentViewModel.SystemTraceAuditNumber);

                verifyPaymentViewModel.Message = updateFailedMessage;
                verifyPaymentViewModel.PaymentStatusType = oldPaymentStatusType;

            }
        }
        #endregion

        /// <summary>
        /// ساخت توکن
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns></returns>
        public async Task<(string token, string signature, string errorMessae, bool isSucces)> GenerateTokenAsync(
            int orderId)
        {
            // دریافت اطلاعات پرداختی های سفارش
            var paymentInformation = await _orderService.CreateNewPaymentAndReturnAsync(orderId,
                PaymentMethodTypesEnum.HekmatOnline,
                _hekmatOnlinePaymentConfiguration.MerchantId,
                _hekmatOnlinePaymentConfiguration.TerminalId);
            if (paymentInformation is
                {
                    Total: not 0 and not null
                })
            {
                var generatedToken =
                    await GetToken(
                        Convert.ToInt64(
                        paymentInformation.Total.Value),
                        paymentInformation.ReservationNumber,
                        paymentInformation?.InstallmentCount);

                if (generatedToken.isSuccess)
                {
                    var checkPaymentExistByToken = await _paymentService.CheckExistByTokenAsync(
                        generatedToken.token,
                        (int)PaymentMethodTypesEnum.HekmatOnline);
                    if (!checkPaymentExistByToken)
                    {
                         
                       await _paymentService.UpdateTokenAsync(generatedToken.token, paymentInformation.PaymentId);

                        return (generatedToken.token, generatedToken.signature, string.Empty, true);
                    }
                    _logger
                        .InsertLog(LogLevel.Error,
                            (int)Events.PaymentException,
                            "Payment", $"توکن ساخته شده تکراری می باشد. :" +
                                       $" token :  {generatedToken.token} - Generated Token for PaymentId = {paymentInformation.PaymentId} "
                            ,
                            null);
                    return (string.Empty, string.Empty, "توکن ساخته شده تکراری می باشد", false);
                }
                return (string.Empty, string.Empty, generatedToken.errorMessae, false);
            }

            return (string.Empty, string.Empty, "مبلغ پرداختی مقدار اشتباه دارد.", false);
        }

        /// <summary>Payment آماده‌شدهٔ Mixed را بدون ساخت Payment دوم به درخواست توکن متصل می‌کند.</summary>
        public async Task<(string token, string signature, string errorMessae, bool isSucces)>
            GenerateMixedTokenAsync(int orderId, int paymentId)
        {
            await using var lockConnection = new SqlConnection(_walletContext.Database.GetConnectionString());
            await lockConnection.OpenAsync();
            await using var lockTransaction = await lockConnection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            if (!await LockMixedPaymentAsync(lockConnection, lockTransaction, paymentId))
                return (string.Empty, string.Empty, "تلاش پرداخت در حال پردازش است.", false);
            var payment = await _walletContext.Payment.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == paymentId && x.OrderId == orderId
                && x.ApplicationId == _applicationConfiguration.ApplicationId && !x.Deleted
                && x.PaymentMethodType == (int)PaymentMethodTypesEnum.HekmatOnline
                && x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway
                && x.Total > 0m && (x.Token == null || x.Token == ""));
            var reservation = payment == null ? null : await _walletContext.WalletReservation
                .AsNoTracking().FirstOrDefaultAsync(x => x.PaymentId == paymentId
                    && x.ApplicationId == _applicationConfiguration.ApplicationId
                    && x.UserId == _principal.Id && !x.Deleted
                    && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                    && x.ExpiresAt > DateTime.UtcNow);
            var orderValid = payment != null && await _walletContext.Order.AsNoTracking().AnyAsync(x =>
                x.Id == orderId && x.UserId == _principal.Id
                && x.ApplicationId == _applicationConfiguration.ApplicationId && !x.Deleted);
            if (!orderValid || reservation == null || reservation.Amount <= 0m
                || !string.IsNullOrWhiteSpace(payment.Token)
                || decimal.Truncate(payment.Total) != payment.Total)
                return (string.Empty, string.Empty, "تلاش Mixed معتبر یا قابل ارسال نیست.", false);

            // پیش از تماس خارجی تلاش را Claim می‌کنیم. Timeout ساخت Token نباید درخواست دوم بسازد.
            var claimed = await _walletContext.Payment.Where(x => x.Id == payment.Id
                    && x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway
                    && (x.Token == null || x.Token == ""))
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.PaymentStatusType,
                    (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway));
            if (claimed != 1)
                return (string.Empty, string.Empty, "تلاش پرداخت قبلاً آغاز شده است.", false);

            var generated = await GetToken(Convert.ToInt64(payment.Total), payment.ReservationNumber,
                payment.InstallmentCount);
            if (!generated.isSuccess)
                return (string.Empty, string.Empty, generated.errorMessae, false);
            if (await _paymentService.CheckExistByTokenAsync(generated.token,
                    (int)PaymentMethodTypesEnum.HekmatOnline)
                || await _paymentService.UpdateTokenAsync(generated.token, payment.Id) != 1)
                return (string.Empty, string.Empty, "توکن تکراری یا تلاش پرداخت تغییر کرده است.", false);
            return (generated.token, generated.signature, string.Empty, true);
        }

        /// <summary>قفل سراسری تلاش Mixed را میان Callback، Inquiry و ساخت Token برقرار می‌کند.</summary>
        private static async Task<bool> LockMixedPaymentAsync(SqlConnection connection,
            System.Data.Common.DbTransaction transaction, int paymentId)
        {
            await using var command = new SqlCommand(
                "DECLARE @result int; EXEC @result = sys.sp_getapplock "
                + "@Resource = @resource, @LockMode = 'Exclusive', @LockOwner = 'Transaction', "
                + "@LockTimeout = 15000; SELECT @result;", connection, (SqlTransaction)transaction);
            command.Parameters.AddWithValue("@resource", $"WalletMixed:HekmatOnline:Payment:{paymentId}");
            return Convert.ToInt32(await command.ExecuteScalarAsync()) >= 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="callbackDto"></param>
        /// <returns></returns>
        public async Task<HekmatOnlinePaymentGatewayTransactionVerificationResponseDto> VerifyPaymentTransactionAsync
            (HekmatOnlinePaymentGatewayCallbackDto callbackDto)
        {
            HekmatOnlinePaymentGatewayTransactionVerificationResponseDto transactionVerificationResponse = new();

            var verifyPaymentViewModel = transactionVerificationResponse.VerifyPaymentViewModel;

            var jsonCallbackDto = JsonSerializer.Serialize(callbackDto, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            if (string.IsNullOrEmpty(callbackDto.ReservationNumber))
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    $"HekmatOnlinePaymentGatewayService => VerifyPaymentTransactionAsync . callbackDto = {jsonCallbackDto}",
                    $"callbackDto.ReservationNumber is null Or Empty", null);

                verifyPaymentViewModel.Message = "کاربر گرامی اطلاعات ارسالی از درگاه پرداخت مشکل دارد.";
                verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                return transactionVerificationResponse;
            }

            var payment = await _paymentService
               .GetByReservationNumberIncludeOrderAsync(callbackDto.ReservationNumber);

            if (payment == null)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException, nameof(VerifyPaymentTransactionAsync),
                    $"HekmatOnlinePaymentGatewayService => VerifyPaymentTransactionAsync : Payment Find By ReservationId is null." +
                               $" callbackDto = {jsonCallbackDto}" +
                    $"callbackDto.ReservationNumber = {callbackDto.ReservationNumber}", null);

                verifyPaymentViewModel.Message = "کاربر گرامی اطلاعات ارسالی از درگاه پرداخت مشکل دارد.";
                verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                return transactionVerificationResponse;
            }

            var mixedReservation = await _walletContext.WalletReservation.AsNoTracking()
                .FirstOrDefaultAsync(x => x.PaymentId == payment.Id && !x.Deleted);
            if (mixedReservation != null)
                return await VerifyMixedAsync(payment, mixedReservation, callbackDto);

            verifyPaymentViewModel.RetrievalReferenceNumber = callbackDto.RetrievalReferenceNumber;
            verifyPaymentViewModel.SystemTraceAuditNumber = callbackDto.SystemTraceAuditNumber.ToString();

            // کاربر دکمه انصراف را زده
            if (callbackDto.Status == HekmatOnlinePaymentGatewayPaymentStatusEnum.Cancel.ToString())
            {
                await UpdatePaymentAndOrderByInquiryParamsAsync(
                    verifyPaymentViewModel,
                    payment.Id,
                    PaymentStatusTypesEnum.Cancel,
                    (PaymentStatusTypesEnum)payment.PaymentStatusType,
                    payment.OrderId,
                    OrderStatusTypesEnum.UnsuccessfulPayment,
                    "انصراف از پرداخت - کاربر دکمه انصراف از پرداخت را زده است.",
                    "خطا در بروزرسانی پرداخت به وضعیت انصراف از پرداخت  توسط کاربر."
                    );

                return transactionVerificationResponse;
            }

            var validationResult = ValidateCallbackParameters(callbackDto);
            if (!validationResult.IsValid)
            {
                _logger.InsertLog(LogLevel.Error,
                     (int)Events.PaymentException, nameof(VerifyPaymentTransactionAsync),
                     $"HekmatOnlinePaymentGatewayService => VerifyPaymentTransactionAsync ." +
                     $" callbackDto = {jsonCallbackDto}" +
                     $"callbackDto is Invalid - {validationResult.ErrorMessage}", null);

                verifyPaymentViewModel.Message = "کاربر گرامی اطلاعات ارسالی از درگاه پرداخت مشکل دارد.";
                verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
                return transactionVerificationResponse;

            }

            var application = await _applicationService.GetAsync(payment.ApplicationId, true);
            verifyPaymentViewModel.OrderListUrl = application.Url + "/Order";

            // بررسی جهت اینکه این بررسی تایید تراکنش دوبار فراخوانی نشده باشد
            if (_paymentTransactionUtil.PaymentTransactionStatus.ContainsKey(callbackDto.ReservationNumber))
            {
                verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                verifyPaymentViewModel.Message = "کاربر گرامی خواهشمند است  فرآیند بروز رسانی وضعیت پرداخت شما در حال بررسی می باشد. ";
                return transactionVerificationResponse;
            }

            var retrievalReferenceNumberIsExists = await
                _paymentService.CheckExitsByRetrievalReferenceNumberAsync(PaymentMethodTypesEnum.HekmatOnline, callbackDto.RetrievalReferenceNumber);
            if (retrievalReferenceNumberIsExists)
            {
                if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                {
                    verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                }
                else
                {
                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                        (PaymentStatusTypesEnum)payment.PaymentStatusType,
                        PaymentMethodTypesEnum.HekmatOnline,
                        "بروز خطا در متد بازگشت از درگاه : " +
                        "رسید دیجیتالی ارسالی بانک قبلا به تراکنش پرداختی اختصاص شده است.",
                        null,
                        callbackDto.RetrievalReferenceNumber,
                        null,
                        null,
                        true,
                        callbackDto.Token,
                        callbackDto.RetrievalReferenceNumber,
                        callbackDto.SystemTraceAuditNumber.ToString()
                        );

                    _logger.InsertLog(LogLevel.Error,
                        (int)Events.PaymentException, nameof(VerifyPaymentTransactionAsync),
                        $"Payment id is {payment.Id} - " +
                        $"retrievalReferenceNumber : {callbackDto.RetrievalReferenceNumber} Exist In Table Payment",
                        null);

                    verifyPaymentViewModel.Message = "کاربر گرامی رسید دیجیتالی ارسالی بانک قبلا به تراکنش پرداختی اختصاص شده است و امکان بروز رسانی تراکنش پرداخت وجود ندارد.";
                    verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                    return transactionVerificationResponse;
                }
            }
            else
            {
                _paymentTransactionUtil.PaymentTransactionStatus.GetOrAdd(callbackDto.ReservationNumber, true);

                // در صورتی که رکورد پرداخت قبلا تایید شده باشد، دوباره آن را بروز رسانی نمی کنیم
                // این حالت زمانی اتفاق می افتد که متد VerifyPaymentTransactionAsync
                //بیش از یک بار با کد ResNum 
                // فراخوانی شده باشد

                if (payment.PaidDateUtc.HasValue)
                {
                    if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                    {
                        verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                    }
                    else
                    {
                        verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                        verifyPaymentViewModel.Message = payment.AuthorizationTransactionResult;
                    }
                }
                else
                {
                    verifyPaymentViewModel.OrderNumber = payment.Order.Title;
                    if (callbackDto.Status == HekmatOnlinePaymentGatewayPaymentStatusEnum.Ok.ToString())
                    {
                        verifyPaymentViewModel.State = callbackDto.Status;
                        try
                        {
                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway,
                                PaymentMethodTypesEnum.HekmatOnline,
                                "منتظر تایید متد Verify",
                                "",
                                "",
                                null,
                                null,
                                true);

                            verifyPaymentViewModel.PaymentStatusType =
                                PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;

                            var confirmPayResult = await ConfirmPayAsync(callbackDto.Token, callbackDto.ReservationNumber);
                            if (confirmPayResult.isSuccess)
                            {
                                verifyPaymentViewModel.RefNum = callbackDto.RetrievalReferenceNumber;
                                verifyPaymentViewModel.ResNum = callbackDto.ReservationNumber;
                                verifyPaymentViewModel.Message = "پرداخت تراکنش با موفقیت انجام شد.";
                                try
                                {
                                    await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                                        OrderStatusTypesEnum.Complete);

                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                        PaymentStatusTypesEnum.Paid,
                                        PaymentMethodTypesEnum.HekmatOnline,
                                        verifyPaymentViewModel.Message,
                                        verifyPaymentViewModel.RefNum,
                                        callbackDto.RetrievalReferenceNumber,
                                        null,
                                        null, true);
                                    verifyPaymentViewModel.PaymentStatusType =
                                        PaymentStatusTypesEnum.Paid;


                                }
                                catch (Exception ex)
                                {

                                    var message = " paymentId :" + payment.Id +
                                                  " ,PaymentMethodTypesEnum :" + PaymentMethodTypesEnum.HekmatOnline +
                                                  " ,PaymentStatusTypesEnum :" + PaymentStatusTypesEnum.Paid +
                                                  " ,Reservation number: " + callbackDto.ReservationNumber +
                                                  ", RRN: " + callbackDto.RetrievalReferenceNumber +
                                                  ", Message" + verifyPaymentViewModel.Message;

                                    _logger.InsertLog(LogLevel.Error,
                                        (int)Events.PaymentException, nameof(VerifyPaymentTransactionAsync), message, ex);

                                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                        PaymentStatusTypesEnum.Unsuccessful, PaymentMethodTypesEnum.HekmatOnline,
                                        message, "",
                                        verifyPaymentViewModel.CustomerRefNum,
                                        null,
                                        null, true);
                                    verifyPaymentViewModel.Message = "کاربر گرامی در بروزرسانی تراکنش پرداخت خطایی رخ داده است لطفا با واحد پشتیبانی تماس بگیرید";
                                    verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                                    return transactionVerificationResponse;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.InsertLog(LogLevel.Error,
                                (int)Events.PaymentException, nameof(VerifyPaymentTransactionAsync),
                                "", ex);

                            await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                                PaymentStatusTypesEnum.Unsuccessful,
                                PaymentMethodTypesEnum.HekmatOnline,
                                $"بروز خطا قبل از تایید در بانک آنلاین حکمت",
                                callbackDto.RetrievalReferenceNumber,
                                "", null,
                                null, true);
                            verifyPaymentViewModel.Message = "بروز خطا قبل از تایید در بانک آنلاین حکمت";
                            verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                            return transactionVerificationResponse;
                        }

                    }
                    else
                    {
                        verifyPaymentViewModel.Message = HekmatOnlinePaymentGatewayVerificationMessageConst.Failed;

                        await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                            PaymentStatusTypesEnum.Unsuccessful,
                            PaymentMethodTypesEnum.HekmatOnline,
                            HekmatOnlinePaymentGatewayVerificationMessageConst.Failed,
                            callbackDto.RetrievalReferenceNumber, true);

                        verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Cancel;
                    }
                }
            }
            return transactionVerificationResponse;
        }

        /// <summary>نتیجه درگاه Mixed را با قفل مشترک تعیین تکلیف و سپس سهم Wallet را تسویه می‌کند.</summary>
        private async Task<HekmatOnlinePaymentGatewayTransactionVerificationResponseDto> VerifyMixedAsync(
            Payment originalPayment, WalletReservation originalReservation,
            HekmatOnlinePaymentGatewayCallbackDto callback)
        {
            var response = new HekmatOnlinePaymentGatewayTransactionVerificationResponseDto(
                PaymentMethodTypesEnum.HekmatOnline);
            var view = response.VerifyPaymentViewModel;
            view.Token = originalPayment.Token;
            view.ResNum = originalPayment.ReservationNumber;
            var application = await _applicationService.GetAsync(originalPayment.ApplicationId, true);
            view.OrderListUrl = application.Url + "/Order";

            await using var lockConnection = new SqlConnection(_walletContext.Database.GetConnectionString());
            await lockConnection.OpenAsync();
            await using var lockTransaction = await lockConnection.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            if (!await LockMixedPaymentAsync(lockConnection, lockTransaction, originalPayment.Id))
                return MixedPending(response, "تلاش پرداخت در حال بررسی است.");

            var payment = await _walletContext.Payment.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == originalPayment.Id);
            var reservation = await _walletContext.WalletReservation.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == originalReservation.Id);
            var validOrder = payment != null && reservation != null
                && await _walletContext.Order.AsNoTracking().AnyAsync(x =>
                    x.Id == payment.OrderId && x.ApplicationId == payment.ApplicationId
                    && x.UserId == reservation.UserId && !x.Deleted);
            if (!validOrder || payment.Deleted || reservation.Deleted
                || payment.ApplicationId != _applicationConfiguration.ApplicationId
                || payment.PaymentMethodType != (int)PaymentMethodTypesEnum.HekmatOnline
                || reservation.ApplicationId != payment.ApplicationId
                || reservation.PaymentId != payment.Id || reservation.Amount <= 0m
                || payment.Total <= 0m || string.IsNullOrWhiteSpace(payment.Token))
                return MixedPending(response, "اطلاعات تلاش پرداخت Mixed ناسازگار است.");

            if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                return await FinishMixedCaptureAsync(payment, reservation, response);
            if (payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Unsuccessful
                || payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Cancel
                || payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Void)
            {
                var finalInquiry = reservation.WalletReservationStatusType
                    == (int)WalletReservationStatusType.Reserved
                    ? await InquiryPayAsync(payment.Token) : null;
                if (finalInquiry?.Status == HekmatOnlinePaymentInquiryPayStatusTypeConst.PayedReverse
                    && finalInquiry.Amount == payment.Total)
                    await _walletSettlement.ReleaseAsync(
                        new WalletReleaseRequestDto { ReservationId = reservation.Id });
                view.PaymentStatusType = (PaymentStatusTypesEnum)payment.PaymentStatusType;
                view.Message = "پرداخت درگاه ناموفق ثبت شده است؛ وضعیت Reservation باید بررسی شود.";
                return response;
            }

            if (callback != null && (callback.Token != payment.Token
                || callback.ReservationNumber != payment.ReservationNumber
                || callback.Amount != payment.Total
                || callback.MerchantId != _hekmatOnlinePaymentConfiguration.MerchantId
                || callback.TerminalId != _hekmatOnlinePaymentConfiguration.TerminalId))
                return MixedPending(response, "اطلاعات Callback با Payment تطابق ندارد.");

            // پاسخ Confirm دارای امضای معتبر، پرداخت را قطعی می‌کند. خطا یا Timeout نتیجهٔ نامعلوم است.
            var confirmed = false;
            if (callback?.Status == HekmatOnlinePaymentGatewayPaymentStatusEnum.Ok.ToString())
            {
                if (!ValidateCallbackParameters(callback).IsValid)
                    return MixedPending(response, "اطلاعات Callback معتبر نیست.");
                try
                {
                    await _walletContext.Payment.Where(x => x.Id == payment.Id
                            && x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway)
                        .ExecuteUpdateAsync(s => s.SetProperty(x => x.PaymentStatusType,
                            (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway));
                    var confirm = await ConfirmPayAsync(callback.Token, callback.ReservationNumber);
                    confirmed = confirm.isSuccess;
                }
                catch (Exception error)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                        nameof(VerifyMixedAsync), $"PaymentId:{payment.Id} Confirm outcome unknown", error);
                }
            }

            HekmatOnlinePaymentGatewayInquiryPayResponseDto inquiry = null;
            if (!confirmed)
                inquiry = await InquiryPayAsync(payment.Token);
            if (inquiry != null && inquiry.Amount != payment.Total)
                return MixedPending(response, "مبلغ استعلام درگاه با Payment تطابق ندارد.");
            if (!confirmed && inquiry?.Status == HekmatOnlinePaymentInquiryPayStatusTypeConst.PayedReverse)
            {
                try
                {
                    await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                        PaymentStatusTypesEnum.Unsuccessful, PaymentMethodTypesEnum.HekmatOnline,
                        "ابطال قطعی پرداخت در استعلام درگاه", true);
                    var release = await _walletSettlement.ReleaseAsync(
                        new WalletReleaseRequestDto { ReservationId = reservation.Id });
                    if (!release.IsSuccess)
                        return MixedPending(response, "پرداخت ابطال شد؛ Release نیازمند بازیابی است.");
                    await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                        OrderStatusTypesEnum.UnsuccessfulPayment, true);
                    view.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
                    view.Message = "پرداخت درگاه ابطال شد.";
                    return response;
                }
                catch (Exception error)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                        nameof(VerifyMixedAsync), $"PaymentId:{payment.Id} reverse recovery failed", error);
                    return MixedPending(response, "ثبت ابطال در حال بازیابی است.");
                }
            }
            if (!confirmed && inquiry?.Status != HekmatOnlinePaymentInquiryPayStatusTypeConst.PayedConfirm)
                return MixedPending(response, "نتیجه درگاه هنوز قطعی نیست؛ Reservation محفوظ است.");

            view.RefNum = callback?.RetrievalReferenceNumber ?? inquiry?.RetrievalReferenceNumber;
            view.RetrievalReferenceNumber = view.RefNum;
            view.SystemTraceAuditNumber = (callback?.SystemTraceAuditNumber
                ?? inquiry?.SystemTraceAuditNumber)?.ToString();
            try
            {
                await _paymentService.UpdatePaymentStatusAsync(payment.Id,
                    PaymentStatusTypesEnum.Paid, PaymentMethodTypesEnum.HekmatOnline,
                    "پرداخت درگاه تایید شد", view.RefNum, view.RefNum,
                    null, null, true, payment.Token, view.RefNum, view.SystemTraceAuditNumber);
            }
            catch (Exception error)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    nameof(VerifyMixedAsync), $"PaymentId:{payment.Id} Paid persistence failed", error);
                if (!await _walletContext.Payment.AsNoTracking().AnyAsync(x => x.Id == payment.Id
                    && x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid))
                    return MixedPending(response, "درگاه تایید شد؛ ثبت Payment نیازمند بازیابی است.");
            }
            return await FinishMixedCaptureAsync(payment, reservation, response);
        }

        /// <summary>Capture تکرارپذیر را اجرا می‌کند و Order را فقط با بدهی صفر کامل می‌سازد.</summary>
        private async Task<HekmatOnlinePaymentGatewayTransactionVerificationResponseDto> FinishMixedCaptureAsync(
            Payment payment, WalletReservation reservation,
            HekmatOnlinePaymentGatewayTransactionVerificationResponseDto response)
        {
            var view = response.VerifyPaymentViewModel;
            var capture = await _walletSettlement.CaptureAsync(new WalletCaptureRequestDto
            {
                ReservationId = reservation.Id, PaymentId = payment.Id,
                IdempotencyKey = $"WalletCapture:Payment:{payment.Id}"
            });
            if (!capture.IsSuccess)
            {
                await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                    OrderStatusTypesEnum.IncompletePayment, true);
                view.PaymentStatusType = PaymentStatusTypesEnum.Paid;
                view.Message = "درگاه پرداخت شد؛ ثبت سهم کیف پول در حال بازیابی است.";
                return response;
            }
            var debt = await _orderService.CalcAmountForPayAsync(
                (int)PaymentMethodTypesEnum.HekmatOnline, payment.OrderId);
            await _orderService.UpdateOrderStatusAsync(payment.OrderId,
                debt.Total == 0m ? OrderStatusTypesEnum.Complete
                    : OrderStatusTypesEnum.IncompletePayment, true);
            view.PaymentStatusType = PaymentStatusTypesEnum.Paid;
            view.Message = debt.Total == 0m ? "پرداخت درگاه و کیف پول تکمیل شد."
                : "پرداخت ثبت شد؛ مانده سفارش باقی است.";
            return response;
        }

        /// <summary>نتیجه نامعلوم را بدون Release یا تغییر قطعی Order گزارش می‌کند.</summary>
        private static HekmatOnlinePaymentGatewayTransactionVerificationResponseDto MixedPending(
            HekmatOnlinePaymentGatewayTransactionVerificationResponseDto response, string message)
        {
            response.VerifyPaymentViewModel.PaymentStatusType =
                PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway;
            response.VerifyPaymentViewModel.Message = message;
            return response;
        }

        /// <summary>
        /// ساخت آدرس پرداخت
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string CreatePaymentUrl(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token is required.", nameof(token));

            return _hekmatOnlinePaymentConfiguration.RedirectToIpgUrl +
                $"?token={Uri.EscapeDataString(token)}" +
                $"&lang={HekmatOnlinePaymentGatewayLanguageConst.Persian}";
        }

        /// <summary>
        /// بررسی تراکنش پرداخت بر اساس توکن ارسالی از سمت درگاه پرداخت
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        public async Task<HekmatOnlinePaymentGatewayTransactionVerificationResponseDto> VerifyPaymentTransactionByTokenAsync(string token)
        {
            HekmatOnlinePaymentGatewayTransactionVerificationResponseDto transactionVerificationResponse = new();
            var verifyPaymentViewModel = transactionVerificationResponse.VerifyPaymentViewModel;

            var payment = await _paymentService
                .GetByTokenAsync(token, (int)PaymentMethodTypesEnum.HekmatOnline, true, _applicationConfiguration.ApplicationId);
            if (payment == null)
                return transactionVerificationResponse;
            var mixedReservation = await _walletContext.WalletReservation.AsNoTracking()
                .FirstOrDefaultAsync(x => x.PaymentId == payment.Id && !x.Deleted);
            if (mixedReservation != null)
                return await VerifyMixedAsync(payment, mixedReservation, null);
            verifyPaymentViewModel.Token = payment.Token;

            var inquiryPayResponse = await InquiryPayAsync(token);
            if (inquiryPayResponse != null)
            {
                var application = await _applicationService.GetAsync(_applicationConfiguration.ApplicationId, true);

                verifyPaymentViewModel.OrderListUrl = application.Url + "/Order";

                verifyPaymentViewModel.RetrievalReferenceNumber = inquiryPayResponse.RetrievalReferenceNumber;
                verifyPaymentViewModel.ResNum = payment.ReservationNumber;
                verifyPaymentViewModel.SystemTraceAuditNumber = inquiryPayResponse.SystemTraceAuditNumber.ToString();

                if (inquiryPayResponse.Status == HekmatOnlinePaymentInquiryPayStatusTypeConst.PayedConfirm &&
                    payment.PaymentStatusType != (int)PaymentStatusTypesEnum.Paid &&
                    payment.Total == inquiryPayResponse.Amount)
                {
                    await UpdatePaymentAndOrderByInquiryParamsAsync(
                        verifyPaymentViewModel,
                        payment.Id,
                        PaymentStatusTypesEnum.Paid,
                        (PaymentStatusTypesEnum)payment.PaymentStatusType,
                        payment.OrderId,
                        OrderStatusTypesEnum.Complete,
                        "پرداخت تراکنش با موفقیت انجام شد.",
                        "خطا در بروز رسانی تراکنش و تایید تراکنش پرداخت بعد از استعلام در متد بررسی بر اساس توکن"
                        );
                    return transactionVerificationResponse;

                }

                if (inquiryPayResponse.Status == HekmatOnlinePaymentInquiryPayStatusTypeConst.PayedOnly)
                {
                    var voidPayResponse = await VoidPayAsync(token);
                    if (voidPayResponse != null)
                    {

                        await UpdatePaymentAndOrderByInquiryParamsAsync(
                            verifyPaymentViewModel,
                            payment.Id,
                            PaymentStatusTypesEnum.Void,
                            (PaymentStatusTypesEnum)payment.PaymentStatusType,
                            payment.OrderId,
                            OrderStatusTypesEnum.UnsuccessfulPayment,
                            "پرداخت تراکنش به دلیل تایید نشدن در سمت پذیرنده / سایت ، ابطال می گردد و به حساب مشتری بر می گردد.",
                            "خطا در ابطال تراکنش در متد بررسی بر اساس توکن"
                        );
                        return transactionVerificationResponse;
                    }
                    else
                    {
                        await UpdatePaymentAndOrderByInquiryParamsAsync(
                            verifyPaymentViewModel,
                            payment.Id,
                            PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway,
                            (PaymentStatusTypesEnum)payment.PaymentStatusType,
                            payment.OrderId,
                            OrderStatusTypesEnum.Processing,
                            "پرداخت تراکنش به دلیل تایید نشدن در سمت پذیرنده / سایت ، ابطال می گردد و به حساب مشتری بر می گردد.",
                            "خطا در ابطال تراکنش در متد بررسی بر اساس توکن"
                        );
                        return transactionVerificationResponse;
                    }


                }

                if (inquiryPayResponse.Status == HekmatOnlinePaymentInquiryPayStatusTypeConst.PayedReverse)
                {
                    await UpdatePaymentAndOrderByInquiryParamsAsync(
                        verifyPaymentViewModel,
                        payment.Id,
                        PaymentStatusTypesEnum.Unsuccessful,
                        (PaymentStatusTypesEnum)payment.PaymentStatusType,
                        payment.OrderId,
                        OrderStatusTypesEnum.UnsuccessfulPayment,
                        "تراکنش شما ابطال شده است و پرداخت ناموق می باشد",
                        "خطا در به روز رسانی پرداخت ناموفق برای تراکنش های ابطال شده"
                    );
                    return transactionVerificationResponse;
                }
            }

            await UpdatePaymentAndOrderByInquiryParamsAsync(
                verifyPaymentViewModel,
                payment.Id,
                PaymentStatusTypesEnum.Unsuccessful,
                (PaymentStatusTypesEnum)payment.PaymentStatusType,
                payment.OrderId,
                OrderStatusTypesEnum.UnsuccessfulPayment,
                "تراکنش شما ابطال شده است و پرداخت ناموفق می باشد",
                "خطا در به روز رسانی پرداخت ناموفق برای تراکنش های ابطال شده"
            );
            verifyPaymentViewModel.PaymentStatusType = PaymentStatusTypesEnum.Unsuccessful;
            verifyPaymentViewModel.Message = "کاربر گرامی خواهشمند است جهت اطلاع از وضعیت پرداخت های خود از سایت پذرنده مجددا اقدام نمایید";
            return transactionVerificationResponse;
        }


        /// <summary>
        /// استعلام تراکنش حکمت آنلاین 
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        public async Task<HekmatOnlinePaymentGatewayInquiryPayResponseDto> InquiryPayAsync(string token)
        {
            var inquiryPayRequest = new HekmatOnlinePaymentGatewayInquiryPayRequestDto
            {
                Token = token,
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(_hekmatOnlinePaymentConfiguration.Password)),
                MerchantId = _hekmatOnlinePaymentConfiguration.MerchantId
            };
            var signData = $"{inquiryPayRequest.MerchantId}&{inquiryPayRequest.Token}";
            var rsa = new RSACryptoServiceProvider();
            var privateKey = _hekmatOnlinePaymentConfiguration.SitePrivateKey;
            rsa.ImportFromPem(privateKey.ToCharArray());
            byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(signData), SHA512.Create());
            inquiryPayRequest.Signature = Convert.ToBase64String(signedData);
            var client = _httpClientFactory.CreateClient("Payment");
            var request = new HttpRequestMessage(HttpMethod.Post,
                _hekmatOnlinePaymentConfiguration.InquireTransactionUrl);

            var data = JsonSerializer.Serialize(inquiryPayRequest,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            request.Content = new StringContent(data,
                Encoding.UTF8, "application/json");
            try
            {
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse =
                        JsonConvert.DeserializeObject<HekmatOnlinePaymentGatewayErrorResponseDto>(responseContent);
                    _logger
                        .InsertLog(LogLevel.Error,
                            (int)Events.PaymentException,
                            nameof(VerifyPaymentTransactionAsync),
                            $"HekmatOnlinePaymentGateway  : InquiryPayMethod Error | " +
                            $"Token: {token} | " +
                            $"ErrorId: {errorResponse?.ErrorId} | " +
                            $"ErrorCode: {errorResponse?.ErrorCode} | " +
                            $"Message: {errorResponse?.ErrorMessage} | " +
                            $"HttpStatus: {errorResponse?.HttpStatus} | " +
                            $"Response: {responseContent}",
                            null);
                    return null;

                }
                var inquiryPaymentResponse = JsonSerializer
                    .Deserialize<HekmatOnlinePaymentGatewayInquiryPayResponseDto>(
                        responseContent, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                // دریافت توکن موفق بود
                if (inquiryPaymentResponse is
                    {
                        Signature: not null and not "",
                        Status: not null and not ""
                    }
                    && VerifySignature(
                        inquiryPaymentResponse.RetrievalReferenceNumber,
                        inquiryPaymentResponse.Signature,
                        _hekmatOnlinePaymentConfiguration.HekmatIpgPublicKey
                    )
                    )
                {
                    return inquiryPaymentResponse;
                }
                _logger
                    .InsertLog(LogLevel.Error,
                        (int)Events.PaymentException,
                        nameof(VerifyPaymentTransactionAsync), $"خطا در استعلام تراکنش :" +
                                   $" token :  {token} "
                        ,
                        null);
                return null;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    nameof(VerifyPaymentTransactionAsync), "خطا در استعلام تراکنش" +
                               $" token : {token}", e);
                return null;
            }

        }

        /// <summary>
        /// ابطال تراکنش بر اساس توکن
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HekmatOnlinePaymentGatewayVoidPayResponseDto> VoidPayAsync(string token)
        {
            var voidPayRequest = new HekmatOnlinePaymentGatewayVoidPayRequestDto
            {
                Token = token,
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(_hekmatOnlinePaymentConfiguration.Password)),
                MerchantId = _hekmatOnlinePaymentConfiguration.MerchantId
            };

            var signData = $"{voidPayRequest.MerchantId}&{voidPayRequest.Token}";
            var rsa = new RSACryptoServiceProvider();
            var privateKey = _hekmatOnlinePaymentConfiguration.SitePrivateKey;
            rsa.ImportFromPem(privateKey.ToCharArray());
            byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(signData), SHA512.Create());
            voidPayRequest.Signature = Convert.ToBase64String(signedData);
            var client = _httpClientFactory.CreateClient("Payment");
            var request = new HttpRequestMessage(HttpMethod.Post,
                _hekmatOnlinePaymentConfiguration.InquireTransactionUrl);

            var data = JsonSerializer.Serialize(voidPayRequest,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            request.Content = new StringContent(data,
                Encoding.UTF8, "application/json");
            try
            {
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse =
                        JsonConvert.DeserializeObject<HekmatOnlinePaymentGatewayErrorResponseDto>(responseContent);
                    _logger
                        .InsertLog(LogLevel.Error,
                            (int)Events.PaymentException,
                            nameof(VerifyPaymentTransactionAsync),
                            $"HekmatOnlinePaymentGatewayService  : VoidPay Error | " +
                            $"Token: {token} | " +
                            $"ErrorId: {errorResponse?.ErrorId} | " +
                            $"ErrorCode: {errorResponse?.ErrorCode} | " +
                            $"Message: {errorResponse?.ErrorMessage} | " +
                            $"HttpStatus: {errorResponse?.HttpStatus} | " +
                            $"Response: {responseContent}",
                            null);
                    return null;

                }
                var voidPayResponse = JsonSerializer
                    .Deserialize<HekmatOnlinePaymentGatewayVoidPayResponseDto>(
                        responseContent, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                // دریافت توکن موفق بود
                if (voidPayResponse is
                    {
                        Signature: not null and not "",
                        TransactionId: not null and not ""
                    }
                    && VerifySignature(
                        voidPayResponse.TransactionId,
                        voidPayResponse.Signature,
                        _hekmatOnlinePaymentConfiguration.HekmatIpgPublicKey)
                    )
                {
                    return voidPayResponse;
                }
                _logger
                    .InsertLog(LogLevel.Error,
                        (int)Events.PaymentException, nameof(VerifyPaymentTransactionAsync),
                        "HekmatOnlinePaymentGatewayService => VoidPay  -  خطا در ابطال تراکنش بر اساس توکن  :" +
                        $" token :  {token} ",
                        null);
                return null;
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, nameof(VerifyPaymentTransactionAsync),
                    "HekmatOnlinePaymentGatewayService => VoidPay " +
                    $"خطا در ابطال تراکنش بر اساس توکن" +
                               $" token : {token}", e);
                return null;
            }
        }

    }


}
