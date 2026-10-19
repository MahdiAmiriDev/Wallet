using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Enums;


namespace YashilPayment.Core.DTO
{
    /// <summary>
    /// پروپرتی های ارسالی برای درخواست توکن
    /// </summary>
    public class HekmatPaymentOnlineTokenRequestDto
    {
        /// <summary>
        /// کد فروشنده : حداکثر 19 رقمی می باشد
        /// </summary>
        [JsonPropertyName("mid")]
        public string MerchantId { get; set; }
        /// <summary>
        /// کلمه عبور که از سمت سایت ارسال میشود
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// تاریخ محلی سایت
        /// </summary>
        public string LocalDateTime { get; set; }
        /// <summary>
        /// نوع تراکنش
        /// </summary>
        [JsonPropertyName("txnType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// شماره ترمینال
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// مبلغ تراکنش به ریا ل
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// کد تراکنش سمت سایت
        /// یکتا باید باشد
        /// </summary>
        [JsonPropertyName("clientTxnId")]
        public string ReservationNumber { get; set; }
        /// <summary>
        /// آدرس برگشتی به سایت
        /// همان
        /// CallBackUrl
        /// </summary>
        public string RedirectUrl { get; set; }
        /// <summary>
        /// امضای الکترونیک اطلاعات
        /// مقدار آن باید
        /// base64 string
        /// باشد
        /// </summary>
        public string Signature { get; set; }

        [JsonPropertyName("saleInfo")]
        public HekmatOnlinePaymentGatewaySaleInfo SaleInfo { get; set; }
    }
    /// <summary>
    /// پروپرتی های برگشتی از درخواست توکن
    /// </summary>
    public class HekmatPaymentOnlineTokenResponseDto
    {
        /// <summary>
        /// امضای الکترونیک اطلاعات
        /// مقدار آن باید
        /// base64 string
        /// باشد
        /// این امضا از سمت درگاه بر می گردد
        /// با 
        /// PublicKey
        /// که در گاه در اختیار ما قرار می هد
        /// Validate
        /// می شود
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// وکن دریافتی در سرویس دریافت توکن 
        /// </summary>
        public string Token { get; set; }


    }

    public class HekmatOnlinePaymentGatewaySaleInfo
    {
        /// <summary>
        /// شناسه خرید؛ عموماً در خریدهای دولتی استفاده می‌شود.
        /// </summary>
        [JsonPropertyName("referenceId")]
        public string? ReferenceId { get; set; }

        /// <summary>
        /// شماره فاکتور مشتری که در رسید نهایی پرداخت نمایش داده می‌شود.
        /// در دیتابیس درگاه ذخیره نمی‌شود.
        /// </summary>
        [JsonPropertyName("invoiceNo")]
        public string? InvoiceNo { get; set; }

        /// <summary>
        /// تعداد اقساط. اگر بزرگتر از صفر باشد، تراکنش به‌صورت اقساطی در نظر گرفته می‌شود.
        /// </summary>
        [JsonPropertyName("installmentsCount")]
        public int? InstallmentsCount { get; set; }

        /// <summary>
        /// اطلاعات تسهیم تراکنش.
        /// مجموع amount ها باید برابر مبلغ خرید باشد.
        /// </summary>
        [JsonPropertyName("apportionment")]
        public List<HekmatOnlinePaymentGatewayApportionment>? Apportionment { get; set; }

        /// <summary>
        /// اطلاعات سبد خرید / کالاهای مرتبط با خرید.
        /// </summary>
        [JsonPropertyName("goods")]
        public List<HekmatOnlinePaymentGatewayMerchantGood>? Goods { get; set; }
    }
    public class HekmatOnlinePaymentGatewayApportionment
    {
        [JsonPropertyName("accountId")]
        public string AccountId { get; set; } = null!;

        [JsonPropertyName("amount")]
        public long Amount { get; set; }
    }
    public class HekmatOnlinePaymentGatewayMerchantGood
    {
        /// <summary>
        /// کد کالا
        /// </summary>
        [JsonPropertyName("goodId")]
        public string? GoodId { get; set; }

        /// <summary>
        /// تعداد / وزن / مقدار کالا
        /// </summary>
        [JsonPropertyName("goodCount")]
        public decimal? GoodCount { get; set; }

        /// <summary>
        /// مبلغ کل کالا
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// نام کالا
        /// </summary>
        [JsonPropertyName("goodName")]
        public string? GoodName { get; set; }

        /// <summary>
        /// مبلغ تخفیف
        /// </summary>
        [JsonPropertyName("discount")]
        public decimal? Discount { get; set; }
    }

    /// <summary>
    /// پارامتر هایی که از سمت درگاه بعد از پرداخت کاربر به
    /// CallBackUrl
    /// ارسال می شود
    /// </summary>
    public class HekmatOnlinePaymentGatewayCallbackDto
    {
        /// <summary>
        /// Ok  در صورت پرداخت موفق
        /// Cancel  در صورتی که پرداخت ناموفق باشد
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
        /// <summary>
        /// توکن ارسالی به سایت پرداخت
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }
        /// <summary>
        /// کد فروشنده که بصورت یک رشته حداکثر 19 رقمی
        /// </summary>
        [JsonPropertyName("mid")]
        public string MerchantId { get; set; }
        /// <summary>
        /// نوع تراکنش
        /// 000000 = خرید
        /// 500000 = پرداخت ) موسسات سوم
        /// </summary>
        [JsonPropertyName("txnType")]
        public string TransactionType { get; set; }
        /// <summary>
        /// شماره ترمینال
        /// </summary>
        [JsonPropertyName("terminalId")]
        public string TerminalId { get; set; }
        /// <summary>
        /// مبلغ تراکنش به ریال
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        /// <summary>
        ///  کد تراکنش سمت سایت
        ///   یکتا باید باشد
        /// </summary>
        [JsonPropertyName("clientTxnId")]
        public string ReservationNumber { get; set; }
        /// <summary>
        /// اطالعات اضافی سایت
        /// درصورت ارسال در درخواست توکن ارسال می گردد
        /// به صورت
        /// string Base64 (2000)
        /// در صورتی که پذیرنده در ساخت توکن ارسال کرده باشد
        /// بعد از پرداخت همان را به
        /// CallBackUrl Method
        /// بر می گرداند و سمت پرداخت درگاه ذخیره نمی شود
        /// </summary>
        [JsonPropertyName("merchantPrivateData")]
        public string MerchantPrivateData { get; set; }
        /// <summary>
        /// (شماره مرجع بازیابی تراکنش / کد مرجع تراکنش)
        /// کد مرجع تراکنش که در صورت موفق بودن تراکنش ارسال می گردد
        /// </summary>
        [JsonPropertyName("rrn")]
        public string RetrievalReferenceNumber { get; set; }
        /// <summary>
        /// (شماره پیگیری/ردیابی سیستمی تراکنش)
        /// کد رهگیری تراکنش
        /// در صورت موفق بودن تراکنش ارسال می گردد
        /// </summary>
        [JsonPropertyName("stan")]
        public int SystemTraceAuditNumber { get; set; }
        /// <summary>
        /// تاریخ پرداخت به فرمت yyyyMMddHHmmss
        /// و میلادی که در صورت موفق بودن تراکنش ارسال میگردد
        /// </summary>
        [JsonPropertyName("txnDate")]
        public string TransactionDate { get; set; }
        /// <summary>
        /// امضای الکترونیکی اطلاعات
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }


    /// <summary>
    /// نتیجه بررسی اعتبار سنجی  پارمتر های ارسالی از درگاه به
    /// CallBackUrl
    /// </summary>
    public class HekmatOnlinePaymentGatewayCallbackValidationResult
    {
        /// <summary>
        /// آیا معتبر است؟
        /// </summary>
        public bool IsValid { get; init; }
        /// <summary>
        /// پیام خطا
        /// </summary>
        public string ErrorMessage { get; init; }
        /// <summary>
        /// این متد درصورت موفق بودن اعتبار سنجی پارامتر های برگشتی از درگاه صدا زده می شود
        /// </summary>
        /// <returns></returns>
        public static HekmatOnlinePaymentGatewayCallbackValidationResult Success() =>
            new() { IsValid = true };
        /// <summary>
        /// این متد درصورت نا موفق بودن اعتبار سنجی پارامتر های برگشتی از درگاه صدا زده می شود
        /// در صورت خطا پیام خطا به این متد ارسال می شود
        /// </summary>
        /// <returns></returns>
        public static HekmatOnlinePaymentGatewayCallbackValidationResult Failure(string message) =>
            new()
            {
                IsValid = false,
                ErrorMessage = message
            };
    }
    /// <summary>
    /// مدل نتیجه بررسی و تایید تراکنش آنلاین حکمت
    /// </summary>
    public class HekmatOnlinePaymentGatewayTransactionVerificationResponseDto
    {
        public HekmatOnlinePaymentGatewayTransactionVerificationResponseDto(
            PaymentMethodTypesEnum paymentMethodTypesEnum = PaymentMethodTypesEnum.Hekmat,
            bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            VerifyPaymentViewModel.PaymentMethodType = paymentMethodTypesEnum;
        }
        /// <summary>
        /// مدل تاییده پرداخت که به ویوی مشاهده
        /// نتیجه پرداخت برای کاربر ارسال می شود
        /// </summary>
        public VerifyPaymentViewModel VerifyPaymentViewModel { get; set; } = new();
        /// <summary>
        /// تایید تراکنش
        /// پرداخت کاربر با موفقت انجام شد
        /// </summary>
        public bool IsSuccess { get; set; }
    }
    /// <summary>
    /// مدل درخواست تایید تراکنش آنلاین حکمت
    /// </summary>
    public class HekmatOnlinePaymentGatewayTransactionConfirmPayRequestDto
    {
        /// <summary>
        /// کد فروشنده : حداکثر 19 رقمی می باشد
        /// </summary>
        [JsonPropertyName("mid")]
        public string MerchantId { get; set; }
        /// <summary>
        /// کلمه عبور که از سمت سایت ارسال میشود
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }
        /// <summary>
        /// وکن دریافتی در سرویس دریافت توکن
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }
        /// <summary>
        /// امضای الکترونیک اطلاعات
        /// مقدار آن باید
        /// base64 string
        /// باشد
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
    /// <summary>
    /// مدل نتیجه تایید تراکنش آنلاین حکمت
    /// </summary>
    public class HekmatOnlinePaymentGatewayTransactionConfirmPayResponseDto
    {
        /// <summary>
        /// درصورتی که عملیات با موفقیت انجام گردد کد تراکنش مرکز 
        /// ارسال می گردد
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }
        /// <summary>
        /// امضای الکترونیک اطلاعات
        /// مقدار آن باید
        /// base64 string
        /// باشد
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
    /// <summary>
    /// مدل خطای برگشتی از درگاه
    /// </summary>
    public class HekmatOnlinePaymentGatewayErrorResponseDto
    {
        public string ErrorId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string HttpStatus { get; set; }
        public List<string> ErrorArgs { get; set; }
    }
    /// <summary>
    /// مدل درخواست استعلام تراکنش
    /// </summary>
    public class HekmatOnlinePaymentGatewayInquiryPayRequestDto
    {
        /// <summary>
        /// کد فروشنده
        /// Sign Field 1
        /// </summary>
        [JsonPropertyName("mid")]
        public string MerchantId { get; set; }

        /// <summary>
        /// کلمه عبور Base64 شده
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// توکن دریافتی از سرویس getToken
        /// Sign Field 2
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// امضای الکترونیکی
        /// </summary>
        public string Signature { get; set; }
    }
    /// <summary>
    /// مدل نتیجه استعلام تراکنش
    /// </summary>
    public class HekmatOnlinePaymentGatewayInquiryPayResponseDto
    {
        /// <summary>
        /// امضای الکترونیکی مرکز پرداخت
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// نوع تراکنش
        /// </summary>
        [JsonPropertyName("txnType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// وضعیت تراکنش
        /// payed_only
        /// payed_rev
        /// payed_confirm
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// شماره مرجع تراکنش
        /// Sign Field 1
        /// </summary>
        [JsonPropertyName("rrn")]
        public string RetrievalReferenceNumber { get; set; }

        /// <summary>
        /// کد رهگیری تراکنش
        /// </summary>
        [JsonPropertyName("stan")]
        public long SystemTraceAuditNumber { get; set; }

        /// <summary>
        /// شماره کارت Mask شده
        /// </summary>
        [JsonPropertyName("maskPan")]
        public string MaskedCardNumber { get; set; }

        /// <summary>
        /// تاریخ انجام تراکنش
        /// yyyyMMddHHmmss
        /// </summary>
        [JsonPropertyName("txnDate")]
        public string TransactionDate { get; set; }
    }

    public class HekmatOnlinePaymentGatewayVoidPayRequestDto
    {
        /// <summary>
        /// کد فروشنده
        /// Sign Field 1
        /// </summary>
        [JsonPropertyName("mid")]
        public string MerchantId { get; set; } = string.Empty;

        /// <summary>
        /// کلمه عبور Base64
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// توکن دریافتی از سرویس getToken
        /// Sign Field 2
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// امضای الکترونیکی
        /// </summary>
        [JsonPropertyName("signature")]
        public string Signature { get; set; } = string.Empty;
    }

    public class HekmatOnlinePaymentGatewayVoidPayResponseDto
    {
        /// <summary>
        /// امضای الکترونیکی مرکز پرداخت
        /// </summary>
        public string Signature { get; set; } = string.Empty;

        /// <summary>
        /// کد تراکنش مرکز
        /// Sign Field 1
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;
    }
}
