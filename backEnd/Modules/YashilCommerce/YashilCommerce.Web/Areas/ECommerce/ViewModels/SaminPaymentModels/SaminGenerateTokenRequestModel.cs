using System;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels.SaminPaymentModels
{
    /// <summary>
    /// پارمتر های مربوط به درخواست تولید توکن
    /// </summary>
    public class SaminGenerateTokenRequestModel
    {
        /// <summary>
        /// مبلغ تراکنش
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// آدرسی که پس از انجام عملیات پرداخت، نتیجه به آن برگردانده می شود.
        /// </summary>
        public string ReturnURL { get; set; }
        /// <summary>
        ///تعداد اقساط خرید
        /// 1 = خرید با بن ریالی
        /// 1 = خرید اعتباری
        /// زرگتر از 1 = خرید اقساطی با تعداد قسط تعیین شده از سوی پذیرنده
        /// </summary>
        public int NumberOfInstallment { get; set; }
        /// <summary>
        /// زمان تراکنش در سمت پذیرنده
        /// </summary>
        public DateTime PurchaseDate { get; set; }
        /// <summary>
        /// کد مرجع در فروشگاه پذیرنده، که این متغیر در پاسخ تراکنش برای پذیرنده ارسال می گردد
        /// تا بتواند بین درخواست تراکنش و نتیجه انجام آن تطابق ایجاد نماید.
        /// </summary>
        public string MerchantReferenceNumber { get; set; }
        /// <summary>
        /// نوع تراکنش --- نشان دهنده نوع حسابی است که بهنگام خرید توسط مشتری انتخاب میگردد
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// کد پذیرنده --- این کد بعد از عقد قرارداد با پذیرنده در اختیار قرار می گیرد.
        /// </summary>
        public string MerchantCode { get; set; }
        /// <summary>
        /// شماره ترمینال --- این کد بعد از عقد قرارداد با پذیرنده در اختیار قرار می گیرد
        /// </summary>
        public string TerminalCode { get; set; }
        /// <summary>
        /// رمز پذیرنده --- این رمز منحصر به هر پذیرنده است وباید در حفظ آن کوشا باشد
        /// </summary>
        public string MerchantPassword { get; set; }
    }

}
