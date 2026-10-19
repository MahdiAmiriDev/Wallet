using System;
using Yashil.Core.Enums;
using YashilPayment.Core.CustomDomainModels;
using YashilPayment.Core.Enums;

namespace YashilPayment.Infrastructure.Helper
{
    public static class PaymentHelper
    {
        public static T GetTokenItem<T>(string[] tokenData, TokenKeys tokenKey) where T : IConvertible
        {
            return (T)Convert.ChangeType(tokenData[(int)tokenKey], typeof(T));

        }

        public static T? GetTokenItGetTokenItemNullable<T>(string[] tokenData, TokenKeys tokenKey) where T : struct
        {
            if (string.IsNullOrEmpty(tokenData[(int)tokenKey]))
            {
                return null;
            }
            return (T)Convert.ChangeType(tokenData[(int)tokenKey], typeof(T));

        }

        /// <summary>
        /// ساخت توکن به وسیله اطلاعات مربوط به درخواست پرداخت
        /// </summary>
        /// <param name="ipAddress">آی پی آدرس</param>
        /// <param name="expireDate">زمان انقضا توکن</param>
        /// <param name="merchantId">شناسه پذیرنده</param>
        /// <param name="terminalId">شناسه ترمینال</param>
        /// <param name="reservationNumber">شناسه پرداخت پذیرنده</param>
        /// <param name="localDateTime">زمان درخواست کاربر</param>
        /// <param name="returnUrl">آدرس برگشتی</param>
        /// <param name="amount">مبلغ</param>
        /// <param name="installmentCount">تعداد اقساط</param>
        /// <param name="cardNumber">شماره کارت</param>
        /// <param name="additionalData">اطلاعات خرید</param>
        /// <param name="cashPaymentPercentage">میزان خرید نقدی</param>
        /// <param name="shippingAmount">هزینه ارسال</param>
        /// <param name="isTaxExempt">معاف از مالیات است</param>
        /// <param name="useFormula">آیا از فرمول برای محاسبه قیمت استفاده کند
        /// در حالتی که از درگاه نقدی اقساطی طرح باقرالعلوم اقدام به پرداخت می کنیم
        /// نیازی به استفاده از فرمول برای محاسبه قیمت نیست</param>
        /// <returns></returns>
        public static string CreateToken(string ipAddress, DateTime expireDate, int merchantId,
            int terminalId, string reservationNumber,
            string localDateTime, string returnUrl,
            long amount, int installmentCount,
            string cardNumber = null,
            string additionalData = null,
            int? cashPaymentPercentage = null,
            int? shippingAmount = null,
            bool isTaxExempt = false,
            bool useFormula = true)
        {
            return $"{ipAddress};{DateTime.Now.AddMinutes(20)};" +
                   $"{merchantId};{terminalId};{reservationNumber};" +
                   $"{localDateTime};{returnUrl};" +
                   $"{amount};{installmentCount};{cardNumber};" +
                   $"{additionalData};{cashPaymentPercentage};" +
                   $"{shippingAmount};{isTaxExempt};"+
                   $"{useFormula}";
        }

        public static bool IsTokenExpired(string[] tokenData)
        {
            var tokenExpireDate = GetTokenItem<DateTime>(tokenData, TokenKeys.TokenExpireDateTime);
            return DateTime.Compare(tokenExpireDate, DateTime.Now) < 0;
        }
        public static bool IsTokenExpired(DateTime tokenExpireDate)
        {
            return DateTime.Compare(tokenExpireDate, DateTime.Now) < 0;
        }
        /// <summary>
        /// مقدار توکن را بر اساس جدا کننده
        /// به یک مدل
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IpgToken GetPrepareTokenData(string token)
        {
            try
            {
                var ipgToken = new IpgToken();
                var tokenData = token.Split(';');

                ipgToken.IpAddress = GetTokenItem<string>(tokenData, TokenKeys.IpAddress);
                ipgToken.MerchantId = GetTokenItem<int>(tokenData, TokenKeys.MerchantId);
                ipgToken.TerminalId = GetTokenItem<int>(tokenData, TokenKeys.TerminalId);
                ipgToken.ReservationNumber = GetTokenItem<string>(tokenData, TokenKeys.ReservationNumber);
                ipgToken.LocalDateTime = GetTokenItem<DateTime>(tokenData, TokenKeys.LocalDateTime);
                ipgToken.Amount = GetTokenItem<long>(tokenData, TokenKeys.Amount);
                ipgToken.ReturnUrl = GetTokenItem<string>(tokenData, TokenKeys.ReturnUrl);
                ipgToken.CardNumber = GetTokenItem<string>(tokenData, TokenKeys.CardNumber);
                ipgToken.TokenExpireDateTime = GetTokenItem<DateTime>(tokenData, TokenKeys.TokenExpireDateTime);
                ipgToken.InstallmentCount = GetTokenItem<int>(tokenData, TokenKeys.InstallmentCount);
                ipgToken.AdditionalData = GetTokenItem<string>(tokenData, TokenKeys.AdditionalData);
                ipgToken.CashPaymentPercentage = GetTokenItGetTokenItemNullable<int>(tokenData, TokenKeys.CashPaymentPercentage);
                ipgToken.ShippingAmount = GetTokenItGetTokenItemNullable<int>(tokenData, TokenKeys.ShippingAmount);
                ipgToken.IsTaxExempt = GetTokenItem<bool>(tokenData, TokenKeys.IsTaxExempt);
                ipgToken.UseFormula = GetTokenItem<bool>(tokenData, TokenKeys.UseFormula);

                return ipgToken;
            }
            catch (Exception)
            {
                return null;
            }

        }



        /// <summary>
        /// برای دریافت کد وضعیت hybridPayment
        /// که به پذیرنده ارسال میشود مورد استفاده قرار میگیرد
        /// </summary>
        /// <param name="paymentStatusType"></param>
        /// <returns></returns>
        public static int GetHybridPaymentStatusCode(int? paymentStatusType)
        {
            return (paymentStatusType == (int)HybridPaymentStatusTypeEnum.WaitingResponseFromPaymentGateway
                    || paymentStatusType == (int)HybridPaymentStatusTypeEnum.SendToPaymentGateway)
                ? (int)PaymentGatewayResultStatusCodeEnum.WaitingForReview
                : (paymentStatusType == (int)HybridPaymentStatusTypeEnum.Paid)
                    ? (int)PaymentGatewayResultStatusCodeEnum.Successful
                    : (paymentStatusType == (int)HybridPaymentStatusTypeEnum.Unsuccessful
                       || paymentStatusType == (int)HybridPaymentStatusTypeEnum.Cancel)
                        ? (int)PaymentGatewayResultStatusCodeEnum.Unsuccessful
                        : (paymentStatusType == (int)HybridPaymentStatusTypeEnum.Refund)
                            ? (int)PaymentGatewayResultStatusCodeEnum.Refund
                            : (int)PaymentGatewayResultStatusCodeEnum.Unknown;
        }


        /// <summary>
        /// جهت دریافت کد وضعیت hybridPaymentRequest
        /// که به پذیرنده ارسال میشود مورد استفاده قرار میگیرد
        /// </summary>
        /// <param name="hybridPaymentRequestStatus"></param>
        /// <returns></returns>
        public static int GetHybridPaymentRequestStatusCode(int hybridPaymentRequestStatus)
        {
            var result = (int)PaymentGatewayResultStatusCodeEnum.Unknown;
            if (hybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.Complete)
            {
                result = (int)PaymentGatewayResultStatusCodeEnum.Successful;
            }

            else if (hybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.Refund)
            {
                result = (int)PaymentGatewayResultStatusCodeEnum.Refund;
            }

            else if (hybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.IncompletePayment)
            {
                result = (int)PaymentGatewayResultStatusCodeEnum.Incomplete;
            }

            else if (hybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.InitialRegistration)
            {
                result = (int)PaymentGatewayResultStatusCodeEnum.Initial;
            }

            else if (hybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.CancelBySystem)
            {
                result = (int)PaymentGatewayResultStatusCodeEnum.Unsuccessful;
            }
            else if (hybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.Processing)
            {
                result = (int)PaymentGatewayResultStatusCodeEnum.WaitingForReview;
            }
            else if (hybridPaymentRequestStatus == (int)HybridPaymentRequestStatusTypeEnum.Canceled)
            {
                result = (int)PaymentGatewayResultStatusCodeEnum.Canceled;
            }

            return result;
        }
    }


}
