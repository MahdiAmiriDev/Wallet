using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Yashil.Core.CustomViewModels;




namespace YashilPayment.Core.DTO
{
    /// <summary>
    /// مدل درخواست ایجاد سفارش برای کاربر در درگاه اینترنتی وایب و ایجاد لینک پرداخت
    /// </summary>
    public class CreateVibeOrderRequestDto
    {
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// ارزش کل سبد خرید
        /// </summary>
        [JsonPropertyName("cart_amount")]
        public int CartAmount { get; set; }

        /// <summary>
        /// ارزش کل سبد خرید به قیمت اقساطی (با مارکاپ) همراه با مالیات
        /// </summary>
        [JsonPropertyName("cart_amount_installment")]
        public int CartAmountInstallment { get; set; }

        /// <summary>
        /// تعداد اقساط سفارش جهت تسویه با پذیرنده(فقط در قراردادهای اقساط پویا)
        /// </summary>
        [JsonPropertyName("installments")]
        public int Installments { get; set; }

        /// <summary>
        /// 	آدرس کال‌بک پس از پایان فرآیند پرداخت
        /// </summary>
        [JsonPropertyName("callback_url")]
        public string CallbackUrl { get; set; }

        /// <summary>
        ///  جزئیات سبد خرید
        /// </summary>
        [JsonPropertyName("data")]
        public VibeOrderData Data { get; set; } = null;
    }

    /// <summary>
    /// داده های مربوط به سبد خرید
    /// </summary>
    public class VibeOrderData
    {
        /// <summary>
        /// 	ارزش کل سبد خرید بدون مالیات
        /// 	(پس از تخفیف)
        /// </summary>
        [JsonPropertyName("goods_amount")]
        public double GoodsAmount { get; set; }

        /// <summary>
        /// 	مبلغ مالیات بر ارزش افزوده به ریال
        /// </summary>
        [JsonPropertyName("tax")]
        public decimal? Tax { get; set; }

        /// <summary>
        /// 	لیست اقلام سبد خرید
        /// </summary>
        [JsonPropertyName("items")]
        public List<VibeOrderItem> OrderItems { get; set; } = new();
    }

    /// <summary>
    /// داده های مربوط به ایتم های سفارش
    /// </summary>
    public class VibeOrderItem
    {
        /// <summary>
        /// 	شناسه محصول
        /// 	(توسط کسب‌وکار ساخته می‌شود)
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 	نام محصول
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 	قیمت محصول به ریال بدون تخفیف
        /// </summary>
        [JsonPropertyName("price")]
        public double Price { get; set; }

        /// <summary>
        /// تعداد محصول سفارش داده شده
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// مبغ تخفیف به ریال
        /// در صورت عدم وجود صفر ارسال شود
        /// </summary>
        [JsonPropertyName("discount")]
        public double? Discount { get; set; } = 0;
    }

    /// <summary>
    /// مدل مربوط به دریافت پاسخ ایجاد یک سفارش در درگاه اینترنتی وایب 
    /// که در پاسخ لینک پرداخت سفارش را به عنوان پاسخ دریافت می کنیم
    /// </summary>
    public class CreateVibeOrderResponseDto
    {
        public CreateVibeOrderResponseDto(string paymentUrl = null, string errorMessage = null)
        {
            PaymentUrl = paymentUrl;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// لینک پرداخت سفارش
        /// </summary>
        [JsonPropertyName("payment_url")]
        public string PaymentUrl { get; set; }

        /// <summary>
        /// پیام خطا
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// پاسخ مربوط به درخواست تایید تراکنش درگاه وایب
    /// </summary>
    public class VerifyVibeOrderResponseDto
    {
        public VerifyVibeOrderResponseDto(bool isSuccess = false)
        {
            IsSuccess = isSuccess;           
        }

        /// <summary>
        /// وضعیت تایید یا عدم تایید سفارش
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// در صورتی که 
        /// false
        /// باشد باید کاربر را به صفحه ی خطا ارجاع داد و در غیر این صورت به صفحه نتیجه
        /// تراکنش ارجاع می دهیم.
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// زمان اولین قسط
        /// </summary>
        public DateTime FirstInstallmentDate { get; set; }

        public VerifyPaymentViewModel verifyPaymentViewModel { get; set; }
    }

    /// <summary>
    /// زمانی که تایید تراکنش ناموفق بوده و در پاسخ 
    /// statusCode
    /// دریافتی 200 نمی باشد و با خطا مواجه شده ایم
    /// </summary>
    public class VerifyVibeOrderFailedDto
    {
        /// <summary>
        /// کد خطای تایید تراکنش
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }
        /// <summary>
        /// جزئیات علت عدم تایید
        /// </summary>
        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        /// <summary>
        /// داده های مربوط به خطا
        /// </summary>
        [JsonPropertyName("data")]
        public List<VibeOrderValidationError> Data { get; set; } = new();
    }

    /// <summary>
    /// داده های مربوط به خطا
    /// </summary>
    public class VibeOrderValidationError
    {
        /// <summary>
        /// مثلا 
        /// body یا ref_id
        /// </summary>
        [JsonPropertyName("loc")]
        public List<string> Loc { get; set; } = new();

        /// <summary>
        /// پیام طخا
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; }

        /// <summary>
        /// نوع خطا
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// تنظیمات مربوط به درگاه وایب
    /// </summary>
    public class VibeConfiguration
    {
        /// <summary>
        /// آدرس کال بک مربوط به درگاه وایب
        /// </summary>
        public string CallBackUrl { get; set; }
        /// <summary>
        ///کلید اتصال به درگاه وایب
        ///که بصورت رمز شده قرار گرفته است.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// آدرس درگاه وایب
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// آدرس مربوط به تایید تراکنش درگاه وایب
        /// </summary>
        public string VerifyUrl { get; set; }

        /// <summary>
        /// حداکثر تعداد روی که میتوان یک سفارش از نوع وایب را کنسل کرد
        /// </summary>
        public byte CancelOrderDeadLine { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int InstallmentCount { get; set; }
    }
}
