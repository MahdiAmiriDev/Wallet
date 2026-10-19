using System;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Yashil.Common.SharedKernel.Helpers;

namespace YashilPayment.Core.DTO
{
    /// <summary>
    /// مدل مورد استفاده برای ارسال درخواست دریافت توکن
    /// برای درگاه ایران کیش
    /// </summary>
    public class IranKisPaymentTokenRequestDto
    {
        [JsonPropertyName("authenticationEnvelope")]
        public AuthenticationEnvelopeDto AuthenticationEnvelopeDto { get; set; }

        [JsonPropertyName("request")]
        public RequestDto RequestDto { get; set; }
    }

    /// <summary>
    /// اطلاعات پاکت دیجیتال
    /// </summary>

    public class AuthenticationEnvelopeDto
    {
        /// <summary>
        /// بردار تولیدی که در یک الگوریتم رمزنگاری متقارن مورد استفاده قرار میگیرد
        /// </summary>
        public string Iv { get; set; }
        /// <summary>
        /// اطلاعات نتیجه عملیات به صورت رمزنگاری بر اساس مکانیزم پاکت دیجیتال
        /// </summary>
        public string Data { get; set; }
    }


    /// <summary>
    /// اطلاعات درخواست جهت ارسال به درگاه ایران کیش در این مدل قرار می گیرد
    /// </summary>
    public class RequestDto
    {
        /// <summary>
        /// نوع تراکنش
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// شناسه ترمینال
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// شناسه پذیرنده
        /// </summary>
        public string AcceptorId { get; set; }

        /// <summary>
        /// آدرس بازگشتی به سامانه پذیرنده
        /// </summary>
        public string RevertUri { get; set; }

        /// <summary>
        /// مبلغ پرداختی
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// شناسه درخواست به ازای هر تراکنش یکتا باید باشد
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// تاریخ و ساعت تولید درخواست
        /// </summary>
        public long RequestTimestamp { get; set; }

    }

    /// <summary>
    /// مدل تنظیمات مربوط به درگاه پرداخت ایران کیش
    /// </summary>
    public class IranKishPaymentConfiguration
    {
        public string TerminalId { get; set; }
        public string DecryptedTerminalId { get; set; }
        
        public string MerchantId { get; set; }
        public string DecryptedMerchantId { get; set; }
        
        public string Password { get; set; }
        public string DecryptedPassword { get; set; }
        
        public string PublicKey { get; set; }
        public string DecryptedPublicKey { get; set; }
    }

    /// <summary>
    /// مدل مربوط به پاسخ ایجاد توکن
    /// </summary>
    public class GenerateIranKishTokenResponseDto
    {
        /// <summary>
        /// کد نتیجه عملیات
        /// </summary>
        [JsonPropertyName("responseCode")]
        public string ResponseCode { get; set; }
        /// <summary>
        /// توضیحات مرتبط با عملیات
        /// </summary>
        [CanBeNull]
        [JsonPropertyName("description")]
        public string Description { get; set; }
        /// <summary>
        /// نتیجه کلی عملیات
        /// </summary>
        [JsonPropertyName("status")]
        public bool Status { get; set; }
        /// <summary>
        /// اطلاعات مربوط به توکن ایجاد شده
        /// </summary>
        [JsonPropertyName("result")]
        public IranKishTokenDetail Result { get; set; }

    }

    /// <summary>
    /// مدل مربوط به اطلاعات توکن
    /// </summary>
    public class IranKishTokenDetail
    {
        /// <summary>
        /// توکن
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }
        /// <summary>
        /// زمان ایجاد توکن
        /// </summary>
        [JsonPropertyName("initiateTimeStamp")]
        public long InitiateTimestamp { get; set; }
        /// <summary>
        /// زمان انقضای توکن
        /// </summary>
        [JsonPropertyName("expiryTimeStamp")]
        public long ExpiryTimestamp { get; set; }
    }


    /// <summary>
    /// مدل پاسخ درخواست تایید تراکنش درگاه پرداخت ایران کیش
    /// </summary>
    public class VerifyIranKishMerchantTransactionResponse
    {
        /// <summary>
        /// کد نتیجه عملیات
        /// </summary>
        [JsonPropertyName("responseCode")]
        public string ResponseConde { get; set; }

        /// <summary>
        /// توضیحات مرتبط  با عملیات
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// نتیجه کلی عملیات
        /// </summary>
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        /// <summary>
        /// در صورتی که نتیجه کلی موفق باشد شامل اطلاعات مرتبط با توکن خواهد بود
        /// </summary>
        [JsonPropertyName("result")]
        public IranKiskVerifyTransactionResponseDetail Result { get; set; }
    }

    /// <summary>
    /// جزئیات پاسخ تاییدیه تراکنش درگاه پرداخت ایران کیش
    /// </summary>
    public class IranKiskVerifyTransactionResponseDetail
    {
        /// <summary>
        /// نتیجه عملیات تاییدیه
        /// </summary>
        [JsonPropertyName("responseCode")]
        public string ResponseCode { get; set; }

        /// <summary>
        /// شماره سند / پیگیری
        /// </summary>
        [JsonPropertyName("systemTraceAuditNumber")]
        public string SystemTraceAuditNumber { get; set; }

        /// <summary>
        /// شماره ارجاع
        /// </summary>

        [JsonPropertyName("retrievalReferenceNumber")]
        public string RetrievalReferenceNumber { get; set; }


        /// <summary>
        /// تاریخ انجام تراکنش
        /// </summary>
        [JsonPropertyName("transactionDate")]
        public string TransactionDate { get; set; }

        /// <summary>
        /// ساعت انجام تراکنش
        /// </summary>
        [JsonPropertyName("transactionTime")]
        public string TransactionTime { get; set; }


        /// <summary>
        /// کد نوع عملیات
        /// </summary>
        [JsonPropertyName("processCode")]
        public string ProcessCode { get; set; }

        /// <summary>
        /// شناسه پرداخت
        /// </summary>
        [JsonPropertyName("paymentId")]
        public string PaymentId { get; set; }

        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

    }
}
