namespace Yashil.Core.CustomDomainModels.BankServiceModels
{
    public class BankAccountInformation
    {
        /// <summary>
        /// شماره حساب
        /// </summary>
        public string Deposit { get; set; }
        /// <summary>
        /// نام بانک
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        public string Card { get; set; }
        /// <summary>
        /// صاحب حساب
        /// </summary>
        public string DepositOwners { get; set; }
        /// <summary>
        /// شماره شبا
        /// </summary>
        public string IBAN { get; set; }
        /*
            * ‌02   حساب فعال است
            ‌* 03   حساب مسدود با قابلیت واریز
            ‌* 04   حساب مسدود بدون قابلیت واریز
            ‌* 05   حساب راکد است
            ‌* 06   بروز خطادر پاسخ دهی, شرح خطا در فیلد توضیحات است
            ‌* 07   سایر موارد
         */
        /// <summary>
        /// وضعیت سپرده
        /// </summary>
        public string DepositStatus { get; set; }
    }

    public class BankAccountInquiry
    {
        /// <summary>
        /// کد پیگیری، اگر ارسال شده باشد همان مقدار و در غیر اینصورت یک رشته تصادفی تولید و برگردانده
        /// </summary>
        public string TrackId { get; set; }
        public BankAccountInformation Result { get; set; }
        /// <summary>
        /// DONE: فراخوانی موفق سرویس
        /// FAILED: فراخوانی ناموفق سرویس
        /// </summary>
        public string Status { get; set; }
    }
}
