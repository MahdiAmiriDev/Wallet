using Grpc.Core;
using Yashil.Core.Enums;
using YashilPayment.Core.Services;
using YashilPayment.Infrastructure.Helper;
using YashilPayment.Web.Helper;
using Yashil.Core.Protos;


namespace YashilPayment.Web.GrpcServices
{
    public class HekmatIpgService : HekmatIPG.HekmatIPGBase
    {
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly IHekmatPaymentGatewayTransactionService _hekmatPaymentGatewayTransactionService;

        public HekmatIpgService(IPaymentGatewayService paymentGatewayService, IHekmatPaymentGatewayTransactionService hekmatPaymentGatewayTransactionService)
        {
            _paymentGatewayService = paymentGatewayService;
            _hekmatPaymentGatewayTransactionService = hekmatPaymentGatewayTransactionService;
        }

        // پرداخت اقساطی

        #region simple hekmat payment

        /// <summary>
        /// وقتی درخواست پرداخت از سمت پذیرنده ارسال می شود این متد فراخوانی می شود
        /// و بعد از بررسی پارامتر های ارسالی توکن ساخته می شود در پارامتر های بازگشتی متد قرار می گیرد
        /// و پذیرنده با توکن دریافتی  کاربر را به صفحه پرداخت حکمت هدایت می کند  
        /// </summary>
        /// <param name="paymentRequest.merchantId">شناسه پذیرنده</param>
        /// <param name="paymentRequest.amount">مبلغ پرداختی</param>
        /// <param name="paymentRequest.returnUrl">آدرس بازگشتی</param>
        /// <param name="paymentRequest.additionalData">اطلاعات کالایی که پرداخت می شود</param>
        /// <param name="paymentRequest.reservationNumber">شناسه پرداخت پذیرنده</param>
        /// <param name="paymentRequest.terminalId">شناسه ترمینال</param>
        /// <param name="paymentRequest.installmentCount">تعداد اقساط</param>
        /// <param name="paymentRequest.signData">امضاء</param>
        /// <param name="paymentRequest.localDateTime">زمان درخواست</param>
        /// <param name="paymentRequest.cardNumber">شماره کارت</param>
        /// <param name="paymentRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PaymentRequestResultModel> PaymentRequest(PaymentRequestModel paymentRequest,
            ServerCallContext context)
        {
            return GeneratePaymentRequestResult(paymentRequest, false);
        }

        /// <summary>
        /// وضعیت پرداخت حکمت را بر می گرداند، نتیجه پرداخت حکمت به همراه پیام نتیجه پرداخت را بر می گرداند
        /// </summary>
        /// <param name="paymentStatus.merchantId">شناسه پذیرنده</param>
        /// <param name="paymentStatus.terminalId">شناسه ترمینال</param>
        /// <param name="paymentStatus.reservationNumber">شناسه پرداخت پذیرنده</param>
        /// <param name="paymentStatus.signData">امضاء</param>
        /// <param name="paymentStatus"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<PaymentStatusResultModel> GetPaymentStatus(PaymentStatusModel paymentStatus,
            ServerCallContext context)
        {
            var resultModel = new PaymentStatusResultModel();
            var res = await _paymentGatewayService.GetHekmatPaymentStatusAsync(paymentStatus.MerchantId,
                paymentStatus.TerminalId, paymentStatus.ReservationNumber, paymentStatus.SignData);
            resultModel.ErrorCode = res.ErrorCode;
            resultModel.Message = res.Message;
            resultModel.StatusCode = res.StatusCode;
            if (!string.IsNullOrEmpty(res.ReservationNumber))
            {
                resultModel.ReservationNumber = res.ReservationNumber;
            }

            if (!string.IsNullOrEmpty(res.RefrenceNumber))
            {
                resultModel.RefrenceNumber = res.RefrenceNumber;
            }

            if (!string.IsNullOrEmpty(res.PaidDate))
            {
                resultModel.PaidDate = res.PaidDate;
            }

            return resultModel;
        }

        #endregion

        //پرداخت ترکیبی

        #region Hybrid Payment
        /// <summary>
        /// وقتی درخواست پرداخت از سمت پذیرنده ارسال می شود این متد فراخوانی می شود
        /// و بعد از بررسی پارامتر های ارسالی توکن ساخته می شود و در پارامتر های بازگشتی متد قرار می گیرد
        /// و پذیرنده با توکن دریافتی  کاربر را به صفحه پرداخت ترکیبی هدایت می کند  
        /// </summary>
        /// <param name="paymentRequest.merchantId">شناسه پذیرنده</param>
        /// <param name="paymentRequest.amount">مبلغ پرداختی</param>
        /// <param name="paymentRequest.returnUrl">آدرس بازگشتی</param>
        /// <param name="paymentRequest.additionalData">اطلاعات کالایی که پرداخت می شود</param>
        /// <param name="paymentRequest.reservationNumber">شناسه پرداخت پذیرنده</param>
        /// <param name="paymentRequest.terminalId">شناسه ترمینال</param>
        /// <param name="paymentRequest.installmentCount">تعداد اقساط</param>
        /// <param name="paymentRequest.signData">امضاء</param>
        /// <param name="paymentRequest.localDateTime">زمان درخواست</param>
        /// <param name="paymentRequest.cardNumber">شماره کارت</param>
        /// <param name="paymentRequest.cashPaymentPercentage">درصد مبلغ پرداختی</param>
        /// <param name="paymentRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PaymentRequestResultModel> HybridPaymentRequest(PaymentRequestModel paymentRequest,
            ServerCallContext context)
        {
            return GeneratePaymentRequestResult(paymentRequest, true);
        }

        public override async Task<HybridPaymentRequestStatusResultModel> GetHybridPaymentRequestStatus(PaymentStatusModel paymentStatus,
            ServerCallContext context)
        {
            var resultModel = new HybridPaymentRequestStatusResultModel();

            var res = await _paymentGatewayService.ValidateAndGetHybridPaymentStatusAsync(
                paymentStatus.MerchantId,
                paymentStatus.TerminalId,
                paymentStatus.ReservationNumber,
                paymentStatus.SignData,
                paymentStatus.UseFormula
            );
            resultModel.ErrorCode = res.ErrorCode;
            resultModel.Message = res.Message;
            resultModel.StatusCode = res.StatusCode;
            resultModel.InstallmentCount = res.InstallmentCount;
            resultModel.CahPaymentPercentage = res.CashPaymentPercentage;
            resultModel.HekmatPayments.AddRange(res.HybridPayments
                .Where(x => x.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat)
                .Select(x => new PaymentStatusResultModel
                {
                    //:todo check this
                    ReservationNumber = x.Id.ToString(),
                    StatusCode = PaymentHelper.GetHybridPaymentStatusCode(x.HybridPaymentStatusType),
                    ErrorCode = (int)PaymentGatewayErrorCodeEnum.NotAnyError,
                    Message = x.AuthorizationTransactionResult,
                    PaidDate = x.PaidDateInt.ToString(),
                }).ToList());
            resultModel.OnlinePayments.AddRange(res.HybridPayments
                .Where(x => x.PaymentMethodType == (int)PaymentMethodTypesEnum.Ansar)
                .Select(x => new PaymentStatusResultModel
                {
                    //:todo check this
                    ReservationNumber = x.ReservationNumber,
                    StatusCode = PaymentHelper.GetHybridPaymentStatusCode(x.HybridPaymentStatusType),
                    ErrorCode = (int)PaymentGatewayErrorCodeEnum.NotAnyError,
                    Message = x.AuthorizationTransactionResult,
                    PaidDate = x.PaidDateInt.ToString(),
                }).ToList());
            if (!string.IsNullOrEmpty(res.ReservationNumber))
            {
                resultModel.ReservationNumber = res.ReservationNumber;
            }

            if (!string.IsNullOrEmpty(res.ReferenceNumber))
            {
                resultModel.ReferenceNumber = res.ReferenceNumber;
            }

            return resultModel;
        }

        #endregion
        // متدهای مشترک پرداخت اقساطی و پرداخت ترکیبی

        #region shared method
        /// <summary>
        /// متد بررسی و ایجاد توکن پرداخت
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <param name="isHybrid">آیا پرداخت ترکیبی است</param>
        /// <returns></returns>
        private async Task<PaymentRequestResultModel> GeneratePaymentRequestResult(
            PaymentRequestModel paymentRequest,
            bool isHybrid)
        {
            var resultModel = new PaymentRequestResultModel
            {
                Succeeded = false,
            };
            string paymentRequestValidationResultMessage =
                PaymentValidator.CheckPaymentRequest(paymentRequest, isHybrid);
            if (string.IsNullOrEmpty(paymentRequestValidationResultMessage))
            {
                var res = await _paymentGatewayService.GenerateTokenForPaymentRequestAsync(
                    paymentRequest.MerchantId,
                    paymentRequest.Amount,
                    paymentRequest.ReturnUrl,
                    paymentRequest.AdditionalData,
                    paymentRequest.ReservationNumber,
                    paymentRequest.TerminalId,
                    paymentRequest.InstallmentCount,
                    paymentRequest.SignData,
                    paymentRequest.LocalDateTime,
                    isHybrid,
                    paymentRequest.IsTaxExempt,
                    paymentRequest.CardNumber,
                    paymentRequest.CashPaymentPercentage,
                    paymentRequest.ShippingAmount, paymentRequest.UseFormula);

                resultModel.Succeeded = res.Succeeded;
                resultModel.StatusCode = res.StatusCode;
                if (!string.IsNullOrEmpty(res.Token))
                {
                    resultModel.Token = res.Token;
                }

                if (!string.IsNullOrEmpty(res.Message))
                {
                    resultModel.Message = res.Message;
                }
            }
            else
            {
                resultModel.Message = paymentRequestValidationResultMessage;
            }

            return resultModel;
        }
        #endregion


        /// <summary>
        /// این متد جهت لغو کردن یک سفارش از نوع اقساطی حکمت
        /// که هنوز برای دریافت نتیجه سمت حکمت ارسال نشده است کاربرد دارد
        /// و وضعیت آن سفارش را به لغو شده تغییر می دهد
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CanelHekmatPaymentRequestResultModel> CancelHekmatPaymentRequest(CanelHekmatPaymentRequestModel request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.SignData))
                return new CanelHekmatPaymentRequestResultModel()
                {
                    IsSuccess = false,
                    Message = "مقدار توکن ارسالی نمیتواند خالی باشد."
                };

            var validateTokenResponse = await _paymentGatewayService.ValidateAndExtractEncryptedTokenData(request.SignData);

            if (!validateTokenResponse.succeed)
                return new CanelHekmatPaymentRequestResultModel()
                {
                    IsSuccess = false,
                    Message = "مقدار توکن ارسالی نامعتبر نمی باشد."
                };
            
            var cancelTransactionResult = await _hekmatPaymentGatewayTransactionService
                .CancelHekmatTransactionAsync(validateTokenResponse.token.TerminalId,
                validateTokenResponse.token.ReservationNumber,validateTokenResponse.token.Amount);

            return new CanelHekmatPaymentRequestResultModel
            {
                IsSuccess = cancelTransactionResult.isSuccess,
                Message = cancelTransactionResult.message
            };

        }
    }
}