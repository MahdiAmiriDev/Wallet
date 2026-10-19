namespace YashilPayment.Core.Const
{

    /// <summary>
    /// زبان مورد استفاده با استاندارد 2 حرفی برای ارسال به صفحه درگاه حکمت آنلاین
    /// </summary>
    public class HekmatOnlinePaymentGatewayLanguageConst
    {
        public const string Persian = "fa";
    }
    /// <summary>
    /// پروپرتی هایی که بعد از پرداخت کاربر
    /// از سمت درگاه به متد
    /// call Back
    /// ارسال می شود
    /// </summary>
    public class HekmatOnlinePaymentGatewayCallbackParamsConst
    {
        /// <summary>
        /// Ok  در صورت پرداخت موفق
        /// Cancel  در صورتی که پرداخت ناموفق باشد
        /// </summary>
        public const string Status = "status";
        /// <summary>
        /// توکن ارسالی به سایت پرداخت
        /// </summary>
        public const string Token = "token";
        /// <summary>
        /// کد فروشنده که بصورت یک رشته حداکثر 19 رقمی
        /// </summary>
        public const string MerchantId = "mid";
        /// <summary>
        /// نوع تراکنش
        /// 000000 = خرید
        /// 500000 = پرداخت ) موسسات سوم
        /// </summary>
        public const string TransactionType = "txnType";
        /// <summary>
        /// شماره ترمینال
        /// </summary>
        public const string TerminalId = "terminalId";
        /// <summary>
        /// مبلغ تراکنش به ریال
        /// </summary>
        public const string Amount = "amount";
        /// <summary>
        ///  کد تراکنش سمت سایت
        ///   یکتا باید باشد
        /// </summary>
        public const string ReservationNumber = "clientTxnId";
        /// <summary>
        /// درصورت ارسال در درخواست توکن ارسال می گردد
        /// به صورت
        /// string Base64 (2000)
        /// در صورتی که پذیرنده در ساخت توکن ارسال کرده باشد
        /// بعد از پرداخت همان را به
        /// CallBackUrl Method
        /// بر می گرداند و سمت پرداخت درگاه ذخیره نمی شود
        /// </summary>
        public const string MerchantPrivateData = "merchantPrivateData";
        /// <summary>
        /// (شماره مرجع بازیابی تراکنش / کد مرجع تراکنش)
        /// کد مرجع تراکنش که در صورت موفق بودن تراکنش ارسال می گردد
        /// </summary>
        public const string RetrievalReferenceNumber = "rrn";
        /// <summary>
        /// (شماره پیگیری/ردیابی سیستمی تراکنش)
        /// کد رهگیری تراکنش
        /// در صورت موفق بودن تراکنش ارسال می گردد
        /// </summary>
        public const string SystemTraceAuditNumber = "stan";
        /// <summary>
        /// تاریخ پرداخت به فرمت yyyyMMddHHmmss
        /// و میلادی که در صورت موفق بودن تراکنش ارسال میگردد
        /// </summary>
        public const string TransactionDate = "txnDate";
        /// <summary>
        /// مبلغ کل تخفیف
        /// </summary>
        public const string Discount = "discount";
        /// <summary>
        /// بر اساس ساختار MerchantGood که در ساختار
        /// SaleInfo
        /// آورده شده است
        /// </summary>
        public const string MerchantGood = "goods";
        /// <summary>
        /// امضای الکترونیکی اطلاعات
        /// </summary>
        public const string Signature = "signature";
    }
    /// <summary>
    /// پیام بررسی تایید تراکنش
    /// </summary>
    public class HekmatOnlinePaymentGatewayVerificationMessageConst
    {
        /// <summary>
        /// پارامتر های ارسالی از
        /// درگاه یا مقدار ندارند
        /// یا دارای مقدار اشتباه هستنند
        /// </summary>
        public static string InvalidCallBackRequestData = "کاربر گرامی اطلاعات ارسالی از درگاه پرداخت به سایت نامعتبر می باشد.";
        /// <summary>
        /// unsuccessful payment transaction
        /// </summary>
        public const string Failed = "متاسفانه بانک خريد شما را تاييد نکرده است";
        /// <summary>
        /// Call Back Param of Ipg Is Null
        /// </summary>
        public const string CallBackParamIsNull = "پارامتر های ارسالی از سمت درگاه نال می باشند ";
        /// <summary>
        /// Param Invalid Message
        /// </summary>
        public const string IsInvalidCallBackParam = "  مقدار ارسالی {0} نامعتبر می باشد";

        /// <summary>
        /// Param Invalid Message
        /// </summary>
        public const string VerifyDataWithSignatureInvalid = "دیتاهای ارسالی با امضای اکترونیکی مطابقت ندارد.";

    }
    /// <summary>
    /// نوع تراکنش 
    /// </summary>
    public class HekmatOnlinePaymentTransactionTypeConst
    {
        /// <summary>
        /// خرید
        /// برای ثبت تراکنش های حکمت از این اسیتفاده می کنیم
        /// </summary>
        public static string Buy = "000000";

        /// <summary>
        /// پرداخت موسسات سوم
        /// </summary>
        public static string Payment = "500000";
    }
    /// <summary>
    /// وضعیت بررسی نتیجه استعلام
    /// </summary>
    public class HekmatOnlinePaymentInquiryPayStatusTypeConst
    {
        /// <summary>
        /// در صورتی که فقط تراکنش موفقی ثبت شده
        ///ولی هنوز از طرف پذیرنده تعیین تکلیف
        /// نشده است
        /// </summary>
        public const string PayedOnly = "payed_only";
        /// <summary>
        /// رصورتی که تراکنش ابطال یا ریورس شده باشد
        /// </summary>
        public const string PayedReverse = "payed_reverse";
        /// <summary>
        /// تراکنش از سمت پذیرنده تایید شده باشد
        /// </summary>
        public const string PayedConfirm = "payed_confirm";
    }
}
