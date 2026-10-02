namespace YashilCommerce.Core.Enums
{
    /// <summary>
    /// request params are parameters that receieve from ipg
    /// status show the payment status 1 is sucessful 99 is pending otherwise error
    /// orderId is the identifier in site
    /// transaction code is identifier in ipg
    /// </summary>
    public class HekmatRequestParams
    {
        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public const string Status = "status";
        /// <summary>
        /// پیغام درگاه
        /// </summary>
        public const string Message = "message";
        /// <summary>
        /// شناسه پرداخت سایت یا پذیرنده
        /// </summary>
        public const string ReservationNumber = "reservationNumber";

        /// <summary>
        /// شناسه پرداخت درگاه
        /// </summary>
        public const string RefrenceNumber = "refrenceNumber";
        /// <summary>
        /// آدرس درگاه بازگشتی
        /// </summary>
        public const string CallBackUrl = "callBackUrl";
        /// <summary>
        /// آدرس درگاه بازگشتی
        /// </summary>
        public const string Amount = "amount";
        /// <summary>
        /// مبلغ پایه
        /// </summary>
        public const string BasePrice = "basePrice";
        /// <summary>
        /// آیا پرداخت نقدی اقساطی است
        /// در مواقعی که خرید از سامانه ی باقرالعلوم به صورت
        /// نقدی اقساطی است کاربرد دارد
        /// </summary>
        public const string UseFormula = "useFormula";
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public const string  InstallmentCount = "installmentCount";
    }

    public class HekmatResultStatusMessage
    {
        /// <summary>
        /// result of 99 means pending 
        /// </summary>
        public const string Pending = "در انتظار تایید";

        /// <summary>
        /// result of 1 means Successful 
        /// </summary>
        public const string Successful = "موفقیت آمیز";

        /// <summary>
        /// result of 0 means CANCEL BY USER IN IPG 
        /// </summary>
        public const string Cancel = "انصراف از پرداخت";
    }

    /// <summary>
    /// the parameters of ansar request for ipg
    /// </summary>
    public class AnsarRequestParam
    {
        /// <summary>
        /// the amount of request to ipg
        /// </summary>
        public const string Amount = "Amount";
        /// <summary>
        ///  the reservation number parameter
        /// </summary>
        public const string ResNum = "ResNum";

        /// <summary>
        /// the Merchant ID
        /// </summary>
        public const string MID = "MID";
        /// <summary>
        /// the redirect url
        /// </summary>
        public const string RedirectURL = "redirectURL";

        public const string Token = "Token";


    }

    /// <summary>
    /// the input parameters name for Saman payment
    /// </summary>
    public class SamanRequestParam
    {
        /// <summary>
        /// the Merchant ID
        /// کد فروشنده است که برای شناسایی فروشنده در هر تراکنش استفاده می شود
        /// </summary>
        public const string MID = "MID";
        /// <summary>
        ///  the reservation number parameter
        /// شناسه خریدی است که توسط پذیرنده بابت خرید تولید می شود 
        /// </summary>
        public const string ResNum = "ResNum";
        /// <summary>
        /// the amount of request to ipg
        /// مبلغ تراکنش خرید است که فروشنده می خواهد از خریدار دریافت ن اید. این اطلان باید بصورت یک عدد صحیح و بدون اعشار باشد و
        /// از هرگونه کاراکتر غیرعددی مانند نقطه (.) پرهیز شود.
        /// فرمت ارسالی باید بصورت   باشد.Double 
        /// </summary>
        public const string Amount = "Amount";
        /// <summary>
        /// the redirect url
        /// آدرس  بازگشتی پذیرنده میباشد
        /// </summary>
        public const string RedirectURL = "RedirectURL";
        /// <summary>
        ///  مبلغ سگمنت1. درحال حاضر استفاده نمیشود باید مقدار 0 داشته باشد
        /// </summary>
        public const string SegAmount1 = "SegAmount1";
        /// <summary>
        ///   مبلغ سگمنت2. درحال حاضر استفاده نمیشود باید مقدار 0 داشته باشد
        /// </summary>
        public const string SegAmount2 = "SegAmount2";
        /// <summary>
        ///   مبلغ سگمنت3. درحال حاضر استفاده نمیشود باید مقدار 0 داشته باشد 
        /// </summary>
        public const string SegAmount3 = "SegAmount3";
        /// <summary>
        ///   مبلغ سگمنت4. درحال حاضر استفاده نمیشود باید مقدار 0 داشته باشد 
        /// </summary>
        public const string SegAmount4 = "SegAmount4";
        /// <summary>
        ///   مبلغ سگمنت5. درحال حاضر استفاده نمیشود باید مقدار 0 داشته باشد 
        /// </summary>
        public const string SegAmount5 = "SegAmount5";
        /// <summary>
        ///   مبلغ سگمنت6. درحال حاضر استفاده نمیشود باید مقدار 0 داشته باشد 
        /// </summary>
        public const string SegAmount6 = "SegAmount6";
        /// <summary>
        /// داده ای اضافی است که برای گزارشگیری می توان از آن استفاده ن ود 
        /// </summary>
        public const string AdditionalData1 = "AdditionalData1";
        /// <summary>
        /// داده ای اضافی است که برای گزارشگیری می توان از آن استفاده ن ود
        /// </summary>
        public const string AdditionalData2 = "AdditionalData2";
        /// <summary>
        /// مبلغ کارمزد می باشد که به مبلغ اصلی اضافه می شود و از دارنده کارت کسر می شود.
        /// </summary>
        public const string Wage = "Wage";
        /// <summary>
        /// توکن دریافتی از درگاه سامان
        /// </summary>
        public const string Token = "Token";


    }
    /// <summary>
    /// the result of Saman payment sep IPG
    /// </summary>
    public class SamanResultParam
    {
        /// <summary>
        /// ین پارامتر مشخص کننده وضعیت تراکنش می باشد
        /// </summary>
        public const string State = "State";
        /// <summary>
        /// این پارامتر کدی وضعیت پرداخت را مشخص می کند
        /// </summary>
        public const string StateCode = "Status";
        /// <summary>
        /// این پارامتر همان شناسه خرید می باشد که توسط پذیرنده ارسال شده بود.
        /// </summary>
        public const string ResNum = "ResNum";
        /// <summary>
        /// کد پذیرنده یا ترمینال اختصاصی پذیرنده می باشد.
        /// </summary>
        public const string MID = "MID";
        /// <summary>
        /// این پارامتر کدی است تا 50 حرف یا عدد که برای هرتراکنش ایجاد میشود
        /// </summary>
        public const string RefNum = "RefNum";
        /// <summary>
        ///  این پارامتر ش اره کارت خریدار بصورت کد شده می باشد. 
        /// </summary>
        public const string CID = "CID";
        /// <summary>
        /// این پارامتر ش اره پیگیری تولید شده توسط سپ می باشد.
        /// </summary>
        public const string TraceNo = "TraceNo";
        /// <summary>
        /// این پارامتر شماره کارت خریدار با الزامات شاپرک می باشد.
        /// </summary>
        public const string SecurePan = "SecurePan";
        /// <summary>
        /// شماره مرجع خرید
        /// </summary>
        public const string Rrn = "RRN";
        /// <summary>
        /// شماره مرجع خرید
        /// </summary>
        public const string TerminalId = "TerminalId";
        /// <summary>
        /// شماره کارت هش شده
        /// </summary>
        public const string HashedCardNumber = "HashedCardNumber";



    }
    /// <summary>
    /// the result of post back 
    /// </summary>
    public class AnsarResultParam
    {
        /// <summary>
        /// the reference number
        /// </summary>
        public const string RefNum = "RefNum";
        public const string Token = "token";
        /// <summary>
        ///  the reservation number parameter
        /// </summary>
        public const string ResNum = "ResNum";
        /// <summary>
        /// customer reference number 
        /// </summary>
        public const string CustomerRefNum = "CustomerRefNum";
        /// <summary>
        /// cardMaskPan
        /// </summary>
        public const string CardMaskPan = "CardMaskPan";
        /// <summary>
        /// email address
        /// </summary>
        public const string EmailAddress = "EmailAddress";
        /// <summary>
        /// MobileNo
        /// </summary>
        public const string MobileNo = "MobileNo";
        /// <summary>
        /// transaction State
        /// </summary>
        public const string State = "State";
        /// <summary>
        /// شناسه خرید فروشنده
        /// </summary>
        public const string MerchantData = "merchantData";
        /// <summary>
        /// شناسه خرید دارنده کارت
        /// </summary>
        public const string GoodReferenceId = "goodReferenceId";

        /// <summary>
        /// مبلغ پرداخت شده در درگاه نقدی
        /// </summary>
        public const string TransactionAmount = "transactionAmount";

    }


    public class SaminResultParam
    {
        /// <summary>
        /// ResponseCode = 0 : عملیات با موفقیت انجام شد
        /// </summary>
        public const string State = "ResponseCode";
        /// <summary>
        /// نتیجه بازگشت از صفحه درگاه پرداخت
        /// </summary>
        public const string ResponseInformation = "ResponseInformation";
        /// <summary>
        /// توکن ارسالی از سمت صفحه درگاه پرداخت
        /// </summary>
        public const string Token = "Token";
        /// <summary>
        /// شماره رهگیری : شناسه منحصر به فرد که از سمت درگاه بر می گردد
        /// </summary>
        public const string RefNum = "RefNO";
        /// <summary>
        /// کد مرجع در فروشگاه پذیرنده که از سمت پذیرنده به درگاه ارسال می شود
        /// </summary>
        public const string ResNum = "MerchantReferenceNumber";


    }
    /// <summary>
    /// some message for ansar result
    /// </summary>
    public class AnsarMessage
    {
        public const string ErrorMessage = "خطا در تأیید تراکنش: ";
        public const string SuccessfulVerify = @"تأیید تراکنش به مبلغ: {0} ریال، با موفقیت انجام شد";
    }
    /// <summary>
    /// the state of Ansar transaction State
    /// </summary>
    public class AnsarTransactionState
    {
        public const string OK = "OK";
    }
    /// <summary>
    /// the state of Ansar transaction State
    /// </summary>
    public class SamanTransactionState
    {
        public const string OK = "OK";
        public const string CanceledByUser = "CanceledByUser";
        public const string InvalidAmount = "InvalidAmount";
        public const string InvalidTransaction = "InvalidTransaction";
        public const string InvalidCardNumber = "InvalidCardNumber";
        public const string NoSuchIssuer = "NoSuchIssuer";
        public const string ExpiredCardPickUp = "ExpiredCardPickUp";
        public const string AllowablePINTriesExceededPickUp = "AllowablePINTriesExceededPickUp";
        public const string IncorrectPIN = "IncorrectPIN";
        public const string ExceedsWithdrawalAmountLimit = "ExceedsWithdrawalAmountLimit";
        public const string TransactionCannotBeCompleted = "TransactionCannotBeCompleted";
        public const string ResponseReceivedTooLate = "ResponseReceivedTooLate";
        public const string SuspectedFraudPickUp = "SuspectedFraudPickUp";
        public const string NoSufficientFunds = "NoSufficientFunds";
        public const string IssuerDownSlm = "IssuerDownSlm";
        public const string TMEError = "TMEError";
    }


    public class SaminTransactionState
    {
        public const string OK = "0";


    }

    public class SaminTransactionStateMessage
    {
        public const string OK = "بانک صحت رسيد ديجيتالي شما را تصديق نمود. فرايند خريد تکميل گشت";

        /// <summary>
        /// Transaction was canceled by user
        /// </summary>
        public const string CanceledByUser = "تراكنش توسط خريدار كنسل شد";
        /// <summary>
        /// unsuccessful payment transaction
        /// </summary>
        public const string Failed = "متاسفانه بانک خريد شما را تاييد نکرده است";
    }

    /// <summary>
    /// messages of saman transaction state sep IPG
    /// </summary>
    public class SamanTransactionStateMessage
    {
        public const string OK = "بانک صحت رسيد ديجيتالي شما را تصديق نمود. فرايند خريد تکميل گشت";
        /// <summary>
        /// Transaction was canceled by user
        /// </summary>
        public const string CanceledByUser = "تراكنش توسط خريدار كنسل شد";
        /// <summary>
        /// Amount of revers teransaction is more than teransaction
        /// </summary>
        public const string InvalidAmount = "مبلغ تراکنش همخوانی ندارد";
        /// <summary>
        /// Can not find teransaction
        /// </summary>
        public const string InvalidTransaction = "تراکنش یافت نشد";
        /// <summary>
        /// Card number is wrong
        /// </summary>
        public const string InvalidCardNumber = "کارت نامعتبر است";
        /// <summary>
        ///  Issuer can not find
        /// </summary>
        public const string NoSuchIssuer = "پذیرنده یافت نشد";
        /// <summary>
        /// The card is expired
        /// </summary>
        public const string ExpiredCardPickUp = "کارت منقضی است";
        /// <summary>
        /// For third time user enter a wrong PIN so card become invalid
        /// </summary>
        public const string AllowablePINTriesExceededPickUp = "تعداد تلاش ناموفق رمز عبور بیش از حد است";
        /// <summary>
        /// Pin card is wrong
        /// </summary>
        public const string IncorrectPIN = "رمز اشتباه است";
        /// <summary>
        /// Exceeds withdrawal from amount limit
        /// </summary>
        public const string ExceedsWithdrawalAmountLimit = "مبلغ بیش از سقف محاز است";
        /// <summary>
        /// PIN and PAD are currect but Transaction Cannot Be Completed
        /// </summary>
        public const string TransactionCannotBeCompleted = "امکان تایید تراکنش وجود ندارد";
        /// <summary>
        /// Timeout occur
        /// </summary>
        public const string ResponseReceivedTooLate = "پاسخ دریافت شده منقضی شده است";
        /// <summary>
        ///  User did not insert cvv2 & expiredate or they are wrong.
        /// </summary>
        public const string SuspectedFraudPickUp = "cvv2 یا تاریخ کارت نامعتبر است";
        /// <summary>
        /// there are not suficient funds in the account
        /// </summary>
        public const string NoSufficientFunds = "مشتری نامعتبر است";
        /// <summary>
        /// The bank server is down
        /// </summary>
        public const string IssuerDownSlm = "سرورهای بانک دچار خطا شده اند";
        /// <summary>
        /// unknown error occur
        /// </summary>
        public const string TMEError = "خطای ناشناخته";
        /// <summary>
        /// reverce transaction message
        /// </summary>
        public const string ReverseTransaction = "مبلغ تراکنش با مبلغ درگاه همخوانی ندارد، بزودی مبلغ کسر شده به حساب شما بازگردانده میشود.";
        /// <summary>
        /// unsuccessful payment transaction
        /// </summary>
        public const string Failed = "متاسفانه بانک خريد شما را تاييد نکرده است";
    }


    public class PaymentTransactionStateMessage
    {
        public const string OK = "بانک صحت رسيد ديجيتالي شما را تصديق نمود. فرايند خريد تکميل گشت";
        /// <summary>
        /// Transaction was canceled by user
        /// </summary>
        public const string CanceledByUser = "تراكنش توسط خريدار كنسل شد";
        /// <summary>
        /// Amount of revers teransaction is more than teransaction
        /// </summary>
        public const string InvalidAmount = "مبلغ تراکنش همخوانی ندارد";
        /// <summary>
        /// Can not find teransaction
        /// </summary>
        public const string InvalidTransaction = "تراکنش یافت نشد";
        /// <summary>
        /// Card number is wrong
        /// </summary>
        public const string InvalidCardNumber = "کارت نامعتبر است";
        /// <summary>
        ///  Issuer can not find
        /// </summary>
        public const string NoSuchIssuer = "پذیرنده یافت نشد";
        /// <summary>
        /// The card is expired
        /// </summary>
        public const string ExpiredCardPickUp = "کارت منقضی است";
        /// <summary>
        /// For third time user enter a wrong PIN so card become invalid
        /// </summary>
        public const string AllowablePINTriesExceededPickUp = "تعداد تلاش ناموفق رمز عبور بیش از حد است";
        /// <summary>
        /// Pin card is wrong
        /// </summary>
        public const string IncorrectPIN = "رمز اشتباه است";
        /// <summary>
        /// Exceeds withdrawal from amount limit
        /// </summary>
        public const string ExceedsWithdrawalAmountLimit = "مبلغ بیش از سقف محاز است";
        /// <summary>
        /// PIN and PAD are currect but Transaction Cannot Be Completed
        /// </summary>
        public const string TransactionCannotBeCompleted = "امکان تایید تراکنش وجود ندارد";
        /// <summary>
        /// Timeout occur
        /// </summary>
        public const string ResponseReceivedTooLate = "پاسخ دریافت شده منقضی شده است";
        /// <summary>
        ///  User did not insert cvv2 & expiredate or they are wrong.
        /// </summary>
        public const string SuspectedFraudPickUp = "cvv2 یا تاریخ کارت نامعتبر است";
        /// <summary>
        /// there are not suficient funds in the account
        /// </summary>
        public const string NoSufficientFunds = "مشتری نامعتبر است";
        /// <summary>
        /// The bank server is down
        /// </summary>
        public const string IssuerDownSlm = "سرورهای بانک دچار خطا شده اند";
        /// <summary>
        /// unknown error occur
        /// </summary>
        public const string TMEError = "خطای ناشناخته";
        /// <summary>
        /// reverce transaction message
        /// </summary>
        public const string ReverseTransaction = "مبلغ تراکنش با مبلغ درگاه همخوانی ندارد، بزودی مبلغ کسر شده به حساب شما بازگردانده میشود.";
        /// <summary>
        /// unsuccessful payment transaction
        /// </summary>
        public const string Failed = "متاسفانه بانک خريد شما را تاييد نکرده است";
    }

    /// <summary>
    /// some message for Walle tStatus Message 
    /// </summary>
    public class WalletStatusMessage
    {
        public const string ErrorMessage = "خطا در تأیید تراکنش";
        public const string SuccessfulVerify = "تراکنش با موفقیت انجام شد";
        public const string Invalidity = "اعتبار شما به پایان رسید";
        public const string CancelByUser = "کاربر گرامی شما از پرداخت انصراف داده اید.";
    }
    /// <summary>
    /// در بارگذاری محصولات از طریق فایل اکسل سر ستون های فایل اکسل باید به صورت زیر باشد
    /// </summary>
    public class TmpUploadedProductExcelHeaderColumns
    {

        public static object[] HeaderColumnsValues = new object[] { "نام موسسه","کد بسته", "عنوان بسته", "شرح مختصر", "نوع بسته", "پایه تحصیلی",
            "رشته تحصیلی","قیمت پایه - ریال","مبلغ تخفیف - ریال",
            "هزینه ارسال - ریال","قیمت آزمون با تخفیف - ریال","قیمت کتاب با تخفیف - ریال","درصد تخفیف","وضعیت انتشار"};
    }

    public class UploadedProductType
    {
        /// <summary>
        /// نوع بسته کتاب می باشد
        /// </summary>
        public const string Book = "کتاب";

        /// <summary>
        /// نوع بسته آزمون می باشد
        /// </summary>
        public const string Test = "آزمون";

        /// <summary>
        /// نوع بسته کامل می باشد
        /// </summary>
        public const string Group = "کامل";
    }

    public class UploadedProductPublisheType
    {
        /// <summary>
        /// منشر شده
        /// </summary>
        public const string Published = "منتشر یافته";

        /// <summary>
        /// منتشر نشده
        /// </summary>
        public const string Unpublished = "منتشر نیافته";


    }
    /// <summary>
    /// وضعیت نمایش زیر نویش
    /// </summary>
    public class DisplaySubTitleType
    {
        /// <summary>
        /// نمایش
        /// </summary>
        public const string Display = "نمایش";

        /// <summary>
        /// عدم نمایش
        /// </summary>
        public const string NoDisplay = "عدم نمایش";


    }
    /// <summary>
    /// Result Of Post Back For Saderat GateWay
    /// </summary>
    public class SaderatResultParam
    {
        /// <summary>
        /// کد نتیجه انجام تراکنش
        /// </summary>
        public const int Respcode = 0 ;
        /// <summary>
        /// متن نتیجه انجام تراکنش
        /// </summary>
        public const string Respmsg = "Respmsg";
        /// <summary>
        /// مبلغ کسر شده از حساب مشتری به ریا ل 
        /// </summary>
        public const long Amount = 0;
        /// <summary>
        /// شماره فاکتور یا سربال خرید پذیرنده
        /// </summary>
        public const string InvoiceId = "InvoiceId";
        /// <summary>
        /// اطلاعات تکمیلی که توسط پذیرنده ارسال می  شود
        /// </summary>
        public const string Payload = "Payload";
        /// <summary>
        /// شماره ترمینالی که از طرف پرداخت الکترونیک سپهر در اختیار پذیرنده قرار می گیرد
        /// </summary>
        public const string TerminalId = "terminalid";
        /// <summary>
        /// شماره پیگیری که بعد از تراکنش موفق به پذیرنده ارسال می گردد
        /// </summary>
        public const string TracenNmber = "tracenumber";
        /// <summary>
        /// شماره سند بانکی
        /// </summary>
        public const string Rrn = "Rrn";
        /// <summary>
        /// زمان و تاریخ تراکنش
        /// </summary>
        public const string DatePaid = "DatePaid";
        /// <summary>
        /// رسید دیجیتال
        /// </summary>
        public const string DigitalReceipt = "DigitalReceipt";
        /// <summary>
        /// نام بانک صادر کننده ی دارنده ی کارت
        /// </summary>
        public const string IssuerBank = "IssuerBank";
        /// <summary>
        /// شماره ی کارت پرداخت کننده به صورت 
        /// Mask شده
        /// </summary>
        public const string CardNumber = "CardNumber";

    }

    /// <summary>
    /// the state of Saderat transaction State
    /// </summary>
    public class SaderatTransactionState
    {
        public const int Success = 0 ;
    }

    /// <summary>
    /// some message for saderat result
    /// </summary>
    public class SaderatMessage
    {
        public const string ErrorMessage = "خطا در تأیید تراکنش: ";
        public const string SuccessfulVerify = @"تأیید تراکنش به مبلغ: {0} ریال، با موفقیت انجام شد";
    }

    /// <summary>
    /// وضعیت وزارت و ساتا
    /// </summary>
    public class MembershipType
    {
        /// <summary>
        /// وزارت
        /// </summary>
        public const string Vezarat = "وزارت";
        /// <summary>
        /// ساتا
        /// </summary>
        public const string Sata = "ساتا" ;
    }

    /// <summary>
    /// زمانی که کاربر پرداخت حکمت را تکمیل می کند
    /// و درخواست
    /// verify
    /// تراکنش ارسال می شود برای دریافت پارامتر های فرم
    /// از مقادیر موجود در کلاس استفاده می کنیم
    /// که برابر هستند با کلید های مربوط به
    /// پروپرتی های فرم ارسال شده
    /// </summary>
    public class HekmatVerifyParams
    {
        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public const string StatusCode = "statusCode";
        /// <summary>
        /// پیغام درگاه
        /// </summary>
        public const string Message = "message";
        /// <summary>
        /// شناسه پرداخت سایت یا پذیرنده
        /// </summary>
        public const string ReservationNumber = "reservationNumber";

        /// <summary>
        /// شناسه پرداخت درگاه
        /// </summary>
        public const string ReferenceNumber = "refrenceNumber";
        /// <summary>
        /// آدرس درگاه بازگشتی
        /// </summary>
        public const string CallBackUrl = "callBackUrl";
        /// <summary>
        /// آدرس درگاه بازگشتی
        /// </summary>
        public const string Amount = "amount";
        /// <summary>
        /// مبلغ پایه
        /// </summary>
        public const string BasePrice = "basePrice";
        /// <summary>
        /// آیا پرداخت نقدی اقساطی است
        /// در مواقعی که خرید از سامانه ی باقرالعلوم به صورت
        /// نقدی اقساطی است کاربرد دارد
        /// </summary>
        public const string UseFormula = "useFormula";
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public const string InstallmentCount = "installmentCount";
        /// <summary>
        /// شناسه
        /// HybrdiPaymentRequest
        /// </summary>
        public const string HybridPaymentRequestId = "hybridPaymentRequestId";
        /// <summary>
        /// شناسه
        /// HybridPayment
        /// </summary>
        public const string HybridPaymentId = "hybridPaymentId";
        /// <summary>
        /// شناسه
        /// HekmatGatewayTransaction
        /// </summary>
        public const string HekmatGatewayTransactionId = "hekmatGatewayTransactionId";
    }


    public class HekmatHybridPaymentVerifyParams
    {
        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public const string Status = "statusCode";
    }

    /// <summary>
    ///زمانی که کاربر پرداخت را در درگاه ایران کیش انجام می دهد
    /// پاسخی که از درگاه به سامانه ی ما ارسال می شود شامل پارامتر های است
    /// که باید دریافت کنیم و بررسی آن ها انجام شود
    /// که آین پارامتر ها در مدل زیر آورده شده اند
    /// </summary>
    public class IranKishVerifyParams
    {
        /// <summary>
        /// کد نتیجه عملیات
        /// </summary>
        public const string ResponseCode = "responseCode";
        /// <summary>
        /// شناسه پرداخت
        /// </summary>
        public const string PaymentId = "paymentId";
        /// <summary>
        /// شناسه درخواست
        /// </summary>
        public const string RequestId = "RequestId";
        /// <summary>
        /// شماره ارجاع درگاه
        /// </summary>
        public const string RetrievalReferenceNumber = "retrievalReferenceNumber";
        /// <summary>
        /// شماره پیگیری یا سند
        /// </summary>
        public const string SystemTraceAuditNumber = "systemTraceAuditNumber";

        /// <summary>
        /// مبلغ پرداختی
        /// </summary>
        public const string Amount = "amount";

        /// <summary>
        /// توکن
        /// </summary>
        public const string Token = "token";

        /// <summary>
        /// شماره کارت به صورت ماسک شده
        /// </summary>
        public const string MaskedPan = "maskedPan";

    }

    /// <summary>
    /// کد تراکنش موفق برای پاسخ های درگاه ایران کیش
    /// </summary>
    public class IranKishTransactionResult
    {
        /// <summary>
        /// تراکنش موفق
        /// </summary>
        public const string Success = "00";
    }

    /// <summary>
    /// پیام پرداخت درگاه ایران کیش
    /// </summary>
    public class IranKishMessage
    {
        public const string SuccessfulVerify = @"تأیید تراکنش به مبلغ: {0} ریال، با موفقیت انجام شد";
    }

}
