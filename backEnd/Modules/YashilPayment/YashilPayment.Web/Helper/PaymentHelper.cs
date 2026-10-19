using Yashil.Core.CustomViewModels;
using Yashil.Core.Protos;

namespace YashilPayment.Web.Helper
{
    public static class PaymentValidator
    {
        /// <summary>
        /// بررسی پارامتر های ورودی از نظر مقادیر
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <param name="isHybrid"></param>
        /// <returns></returns>
        public static string CheckPaymentRequest(PaymentRequestModel paymentRequest, bool isHybrid = false)
        {
            string result = string.Empty;
            if (paymentRequest.MerchantId == 0)
            {
                result += "شناسه پذیرنده باید دارای مقدار باشد.";
            }

            if (paymentRequest.TerminalId == 0)
            {
                result += "شناسه ترمینال باید دارای مقدار باشد.";
            }

            if (string.IsNullOrWhiteSpace(paymentRequest.ReservationNumber))
            {
                result += "شناسه پرداخت پذیرنده باید دارای مقدار باشد.";
            }

            if (string.IsNullOrWhiteSpace(paymentRequest.ReturnUrl))
            {
                result += "آدرس بازگشتی باید دارای مقدار باشد.";
            }

            if (string.IsNullOrWhiteSpace(paymentRequest.LocalDateTime))
            {
                result += "زمان درخواست پرداخت باید دارای مقدار باشد";
            }

            if (string.IsNullOrWhiteSpace(paymentRequest.SignData))
            {
                result += "امضاء باید دارای مقدار باشد.";
            }

            if (paymentRequest.Amount <= 0)
            {
                result += "مبلغ پرداخت باید بزرگتر از صفر باشد.";
            }
            if (paymentRequest.ShippingAmount < 0)
            {
                result += "هزینه ارسال باید بزرگتر مساوی صفر باشد.";
            }

            if (!isHybrid && paymentRequest.CashPaymentPercentage.HasValue)
            {
                result += "مقدارپارامترهای ورودی صحیح نمی باشد، درصد پرداخت نقدی نمیتواند مقدار داشته باشد";
            }
            else if (isHybrid && paymentRequest.CashPaymentPercentage.HasValue 
                              && (paymentRequest.CashPaymentPercentage < 0 || paymentRequest.CashPaymentPercentage > 100))
            {
                result += "مقدار درصد پرداخت نقدی صحیح نمی باشد و باید دارای مقداری بین 0 تا 100 باشد";
            }

            return result;
        }

        public static string CheckVerifyData(VerifyData verifyData)
        {
            string result = string.Empty;
            if (string.IsNullOrWhiteSpace(verifyData.Token))
            {
                result += "Token must not be null. ";
            }

            if (string.IsNullOrWhiteSpace(verifyData.SignData.ToString()))
            {
                result += "SignData must not be null. ";
            }

            return result;
        }
        
        
       
    }
}