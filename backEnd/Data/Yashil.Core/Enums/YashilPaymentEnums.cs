namespace Yashil.Core.Enums
{
    /// <summary>
    /// statusCode های ممکن
    /// در جواب درخواست درگاه حکمت
    /// </summary>
    public enum HekmatPaymentStatusResultStatusCodeEnum
    {
        /// <summary>
        /// وضعیت نامشخص
        /// </summary>
        UnKnown = 6,

        /// <summary>
        ///  ثبت اولیه و منتظر بررسی پرداخت حکمت
        /// </summary>
        InQueueToProcess = 0,

        /// <summary>
        /// پرداخت حکمت با موفقیت انجام شد.
        /// </summary>
        Successful = 1,

        /// <summary>
        /// پرداخت حکمت نا موفق می باشد.
        /// </summary>
        Unsuccessful = 2,

        /// <summary>
        /// بررسی مجدد وضعیت پرداخت حکمت (وضعیت نامشخص)
        /// </summary>
        TryAgain = 3,
    }

    /// <summary>
    /// statusCode های ممکن
    /// در جواب درخواست درگاه حکمت
    /// </summary>
    public enum HekmatPaymentStatusResultErrorCodeEnum
    {
        /// <summary>
        /// بدون خطا
        /// </summary>
        Successful = 0,

        /// <summary>
        /// پذیرنده با شناسه پذیرنده ارسالی یافت نشد
        /// </summary>
        MerchantNotFound = 1,

        /// <summary>
        /// ترمینال کاربر صحیح نمی باشد
        /// </summary>
        InvalidTerminal = 2,

        /// <summary>
        /// ترمینال کاربر فعال نمی باشد
        /// </summary>
        InactiveTerminal = 3,

        /// <summary>
        /// امضای دیجیتال معتبر نمی باشد
        /// </summary>
        InvalidSignature = 4,

        /// <summary>
        /// چنین تراکنشی یافت نشد
        /// </summary>
        ReservationNotFound = 5,

        /// <summary>
        /// خطا سیستمی در دریافت وضعیت پرداخت حکمت
        /// </summary>
        SystemError = 6,

        /// <summary>
        /// پارامتر های ورودی معتبر نمی باشد
        /// </summary>
        InvalidParameters = 7
    }

    /// <summary>
    /// وقتی کاربر استفاده کننده از درگاه پرداخت حکمت یا ترکیبی، درخواست پرداخت می کند
    /// درخواست کاربر بررسی می شود و کد وضعیت برای درخواست کاربر تولید می شود.
    /// کد وضعیت درخواست پرداخت  کاربر به صورت زیر می باشد 
    /// </summary>
    public enum PaymentGatewayRequestStatusCodeEnum
    {
        /// <summary>
        /// عملیات درخواست پرداخت  با موفقیت انجام شد
        /// </summary>
        Successful = 0,

        /// <summary>
        /// ترمینال کاربر فعال نمی باشد
        /// </summary>
        InactiveTerminal = 1,

        /// <summary>
        /// ترمینال کاربر صحیح نمی باشد
        /// </summary>
        InvalidTerminal = 2,

        /// <summary>
        /// کد رهگیری مشتری به ازای هر پرداخت تکراری می باشد
        /// </summary>
        DuplicateLocalReservationNumber = 3,

        /// <summary>
        /// امضا معتبر نمی باشد
        /// </summary>
        InvalidSignature = 4,

        /// <summary>
        /// شماره کارت معتبر نمی باشد
        /// </summary>
        InvalidCardNumber = 5,

        /// <summary>
        /// آدرس صفحه بازگشتی دارای مقدار نمی باشد
        /// </summary>
        ReturnUrlIsEmpty = 6,

        /// <summary>
        /// شناسه سفارش پذیرنده دارای مقدار نمی باشد
        /// </summary>
        ReservationNumberIsEmpty = 7,

        /// <summary>
        /// تعداد اقساط ارسالی از سمت پذیرنده معتبر نمی باشد
        /// </summary>
        InvalidInstallmentCount = 8,

        /// <summary>
        /// مبلغ ارسالی از سمت پذیرنده معتبر نمی باشد
        /// </summary>
        InvalidAmount = 9,

        /// <summary>
        /// تاریخ درخواست پرداخت پذیرنده دارای مقدار نمی باشد
        /// </summary>
        localDateTimeIsEmpty = 10,

        /// <summary>
        /// پذیرنده با شناسه پذیرنده ارسالی یافت نشد
        /// </summary>
        MerchantNotFound = 11,

        /// <summary>
        /// مقدار درصد پرداخت نقدی نامعتبر است
        /// </summary>
        InvalidCashPaymentPercentage = 12,
        /// <summary>
        /// آدرس آی پی نامعتبر است
        /// </summary>
        InvalidIpAddress = 13,

        /// <summary>
        /// خطا در ثبت تراکنش
        /// </summary>
        Error = 99,
    }

    public enum HekmatPurchaseResultStatusCodeEnum
    {
        /// <summary>
        /// پرداخت شما با موفقیت ثبت شد و منتظر بررسی می باشد
        /// </summary>
        Successful = 1,

        /// <summary>
        /// کاربر دکمه انصراف را زده است
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// خطا
        /// </summary>
        Error = 99,
    }

    /// <summary>
    /// خطاهای رخ داده شده در دریافت وضعیت پرداخت حکمت.
    /// وقتی متد دریافت وضعیت (نتیجه) پرداخت حکمت صدا زده می شود ممکن است خطای اتفاق بیافتد
    /// کد های زیر برای خطای رخ داده شده می باشد
    /// </summary>
    public enum PaymentGatewayErrorCodeEnum
    {
        /// <summary>
        /// بدون خطا
        /// </summary>
        NotAnyError = 0,

        /// <summary>
        /// پذیرنده با شناسه پذیرنده ارسالی یافت نشد
        /// </summary>
        MerchantNotFound = 1,

        /// <summary>
        /// ترمینال کاربر صحیح نمی باشد
        /// </summary>
        InvalidTerminal = 2,

        /// <summary>
        /// ترمینال کاربر فعال نمی باشد
        /// </summary>
        InactiveTerminal = 3,

        /// <summary>
        /// امضای دیجیتال معتبر نمی باشد
        /// </summary>
        InvalidSignature = 4,

        /// <summary>
        /// چنین تراکنشی یافت نشد
        /// </summary>
        TransactionNotFound = 5,

        /// <summary>
        /// خطا سیستمی در دریافت وضعیت پرداخت حکمت
        /// </summary>
        SystemError = 6,

        /// <summary>
        /// پارامتر های ورودی معتبر نمی باشد
        /// </summary>
        InvalidParameters = 7
    }

    /// <summary>
    /// کد های وضعیت نتیجه پرداخت حکمت
    /// وقتی متد دریافت وضعیت (نتیجه) پرداخت حکمت صدا زده می شود، نتیجه پرداخت حکمت در قالب کد های زیر می باشد
    /// </summary>
    public enum PaymentGatewayResultStatusCodeEnum
    {
        /// <summary>
        /// پرداخت در حال بررسی می باشد
        /// </summary>
        WaitingForReview = 0,

        /// <summary>
        /// پرداخت با موفقیت انجام شد
        /// </summary>
        Successful = 1,

        /// <summary>
        /// پرداخت ناموفق می باشد
        /// </summary>
        Unsuccessful = 2,

        #region hybridPaymentStatus

        /// <summary>
        /// ثبت اولیه
        /// </summary>
        Initial = 4,

        /// <summary>
        /// پرداخت ناقص
        /// </summary>
        Incomplete = 3,
        /// <summary>
        /// عودت وجه
        /// </summary>
        Refund = 5,
        #endregion

        /// <summary>
        /// بررسی مجدد
        /// </summary>
        Review = 50,
        /// <summary>
        /// کاربر دکمه انصراف در صفحه درگاه زده است
        /// </summary>
        Canceled = 6,

        /// <summary>
        /// وضعیت نامشخص
        /// </summary>
        Unknown = -1,
    }
    
    /// <summary>
    /// نوع فروش نفدی یا اقساطی
    /// </summary>
    public enum SaleTypesEnum
    {
        /// <summary>
        /// پرداخت نقدی
        /// </summary>
        CashPayment = 1104,
        /// <summary>
        /// پرداخت اقساطی
        /// </summary>
        InstallmentPayment = 1105,
        /// <summary>
        /// نقدی و اقساطی
        /// </summary>
        CashPaymentAndInstallmentPayment = 2305

    }

    /// <summary>
    /// خطاهایی که در هنگام ورود کاربر به صفحه درگاه و بررسی
    /// توکن ممکن است خطایی رخ دهد
    /// کد وضعیت خطای رخ داده در زیر آمده است
    /// </summary>
    public enum HekmatIpgPageErrorEnum
    {
        /// <summary>
        /// مقدار توکن نامعتبر است
        /// </summary>
        InvalidToken = 1,
        /// <summary>
        /// ترمینالی بر اساس شناسه پذیرنده و با شناسه ترمینال ارسالی یافت نشد
        /// </summary>
        TerminalNotFound  = 3,
        /// <summary>
        /// زمان توکن منقضی شده است
        /// </summary>
        TokenExpired = 4,
        /// <summary>
        /// شماره کارت نامعتبر است
        /// </summary>
        CardNumberInvalid = 5

    }


}