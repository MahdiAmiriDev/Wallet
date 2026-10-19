namespace YashilPayment.Core.Enums
{
    public enum TokenKeys
    {
        IpAddress = 0,
        TokenExpireDateTime = 1,
        MerchantId = 2,
        TerminalId = 3,
        ReservationNumber = 4,
        LocalDateTime = 5,
        ReturnUrl = 6,
        Amount = 7,
        InstallmentCount = 8,
        CardNumber = 9,
        AdditionalData = 10,
        CashPaymentPercentage = 11,
        ShippingAmount = 12,
        IsTaxExempt = 13,
        UseFormula = 14
    }
    public enum HekmatStatusCodeEnum
    {
        /// <summary>
        /// if HekmatErrorCode = 0 => در انتظار ارسال به حکمت 
        /// if HekmatErrorCode = 99 => به حکمت ارسال شده و منتظر نتیجه پرداخت  
        /// </summary>
        Waiting = 0,
        /// <summary>
        /// if HekmatErrorCode = 1 => پرداخت با موفقیت انجام شد
        /// </summary>
        Success = 1,
        /// <summary>
        /// if HekmatErrorCode = 30 => پرداخت موفق نبوده و دلیل پرداخت در فیلد HekmatTransactionResultMessage
        /// if  HekmatErrorCode = 7 => تراکنش عودت زده است و دیگر جز تراکنش های موفق نیست
        /// </summary>
        Unsuccessful = 2,
        /// <summary>
        /// if HekmatErrorCode = 3 => انصراف از ارسال تراکنش پرداخت به سمت حکمت
        /// </summary>
        Cancel = 3,
    }
    public enum HekmatErrorCodeEnum
    {
        /// <summary>
        /// if HekmatStatusCode = 0 => در انتظار ارسال به حکمت 
        /// </summary>
        Waiting = 0,
        /// <summary>
        /// if HekmatStatusCode = 0 => به حکمت ارسال شده و منتظر نتیجه پرداخت  
        /// </summary>
        SnedToHekmat = 99,
        /// <summary>
        /// if HekmatStatusCode = 1 => پرداخت با موفقیت انجام شد
        /// </summary>
        Success = 1,
        /// <summary>
        /// if HekmatStatusCode = 2 => پرداخت موفق نبوده و دلیل پرداخت در فیلد HekmatTransactionResultMessage 
        /// </summary>
        Unsuccessful = 30,
        /// <summary>
        /// if HekmatStatusCode = 3 => انصراف از ارسال تراکنش پرداخت به سمت حکمت
        /// </summary>
        Cancel = 6,
        /// <summary>
        /// عودت داده شده
        /// یعنی این تراکنش قبلا تایید شده بود و برای عودت به حکمت ارسال شده است
        /// و مبلغ تراکنش دیگر از حکمت کارت کسر نمی شود و مبلغ کسر شده به صاحب کارت عودت داده می شود
        /// </summary>
        Refunded= 7
    }

    /// <summary>
    /// نوع درخواست عودت تراکنش
    /// </summary>
    public enum RefundPaymentTransactionItemRequestTypeEnum
    {
        /// <summary>
        /// لغو
        /// </summary>
        Cancel = 3132
    }

    /// <summary>
    /// وضعیت درخواست عودت تراکنش
    /// </summary>
    public enum RefundPaymentTransactionItemRequestStatusEnum
    {
        /// <summary>
        /// تکمیل
        /// </summary>
        Complete = 3136,
        /// <summary>
        /// رد توسط کارشناس 
        /// </summary>
        RejectByCrm = 3135,
        /// <summary>
        /// تایید توسط کارشناس
        /// </summary>
        AcceptByCrm = 3134,
        /// <summary>
        /// ثبت اولیه درخواست
        /// توسط تامین کننده
        /// </summary>
        InitialRequestBySupplier = 3133
    }

    /// <summary>
    /// نتیجه تراکنش کاربر در درگاه وایب
    /// </summary>
    public enum VerifyVibePaymentStatusEnum
    {
        /// <summary>
        /// موفق
        /// </summary>
        Ok,
        /// <summary>
        /// ناموفق
        /// </summary>
        NOK
    }
}
