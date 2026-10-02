namespace Yashil.Core.CustomDomainModels
{
    /// <summary>
    /// این مدل به منظظور اینکه تراکنش های عودت وجه حکمت را نمایش دهد استفاده میشود
    /// در واقع برای نمایش اطلاعات از جدول های
    ///[HekmatCanceledTransaction] 
    ///[HekmatEffectlessCanceledTransaction]
    ///[HekmatEffectlessModifiedTransaction]
    ///[HekmatModifiedTransaction]
    /// استفاده میشود
    /// این جدول ها با هم union میشوند
    /// و در یک لیست به نمایش گذاشته میشود
    /// در فرم گروه بندی عودت وجه کاربرد دارد
    /// در فسمت بررسی عودت وجه درون کامپوننت لیست تراکنشات عودت وجه حکمت
    /// </summary>
    public class HekmatCanceledOrModifiedTransactionDomainModel
    {
        /// <summary>
        ///  مبلغ تراکنش ارسالی به حکمت
        /// در واقع این مبلغ، مبلغ اولیه ای است که به حکمت ارسال شده است و بعدا اصلاح یا عودت داده شده است
        /// </summary>
        public long? Price { get; set; }
        /// <summary>
        /// مبلغ عودت
        /// </summary>
        public long? RefundPrice { get; set; }
        /// <summary>
        /// شماره کارت حکمت
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// تاربخ تراکنش 
        /// </summary>
        public string TransactionDate { get; set; }
    }
}
