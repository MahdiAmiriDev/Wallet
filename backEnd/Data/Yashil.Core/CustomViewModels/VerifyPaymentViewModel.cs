using System.Text.Json.Serialization;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Enums;

namespace Yashil.Core.CustomViewModels
{
    public class VerifyPaymentViewModel : IBaseViewModel
    {

        public int ViewModelId { get; set; }

        /// <summary>
        /// وضعیت انجام تراکنش با دو وضعیت " "Canceled By User" و یا ok" است
        /// </summary>
        [StringValidation]
        public string State { get; set; }

        /// <summary>
        ///  این پارامتر کدی وضعیت پرداخت را مشخص می کند 
        /// </summary>
        [StringValidation]
        public string StateCode { get; set; }

        /// <summary>
        /// شماره پیگیری تراکنش انجامشده است. (این مقدار یکتا است و به ازای هر تراکنش از سوی فنآوا کارت به پذیرنده ارسال میگردد.) 
        /// </summary>
        [StringValidation]
        public string RefNum { get; set; }

        /// <summary>
        /// کدی است که پذیرنده به کالای خود اختصاص داده و آن برای درگاه پرداخت اینترنتی فنآوا کارت ارسال میکند. 
        /// </summary>
        [StringValidation]
        public string ResNum { get; set; }

        /// <summary>
        /// شماره پیگیری خریدار -(شماره مرجع تراکنش -RNN
        /// </summary>
        [StringValidation]
        public string CustomerRefNum { get; set; }

        /// <summary>
        /// این پارامتر ش اره پیگیری تولید شده توسط سپ می باشد.
        /// </summary>
        [StringValidation]
        public string TraceNo { get; set; }

        /// <summary>
        ///   شماره کارت خریدار MasK
        /// </summary>
        [StringValidation]
        public string CardMaskPan { get; set; }

        /// <summary>
        /// این پارامتر ش اره کارت خریدار با الزامات شاپرک می باشد.
        /// </summary>
        [StringValidation]
        public string SecurePan { get; set; }

        /// <summary>
        /// آدرس پست الکترونیک کاربر است که در صفحه پرداخت وارد نموده است
        /// </summary>
        [StringValidation]
        public string Email { get; set; }

        /// <summary>
        /// شماره موبایل کاربر است که در صفحه پرداخت وارد نموده است
        /// </summary>
        [StringValidation]
        public string Mobile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringValidation]
        public string Message { get; set; }

        /// <summary>
        /// مبلغ تراکنش 
        /// </summary>
        [StringValidation]
        public string TransactionAmount { get; set; }

        /// <summary>
        ///   شماره کارت خریدارHas
        /// </summary>
        [StringValidation]
        public string CardHashPan { get; set; }

        /// <summary>
        /// شماره سفارش
        /// </summary>
        [StringValidation]
        public string OrderNumber { get; set; }

        public PaymentMethodTypesEnum PaymentMethodType { get; set; }

        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public PaymentStatusTypesEnum PaymentStatusType { get; set; }

        /// <summary>
        /// به دلیل این که به ازای تمامی پروژه ها یک
        /// callBackUrl
        /// وجود دارد، بنابراین برای مشاهده لیست سفارشات کاربران در هر پروژه باید
        /// url
        /// مربوط به همان پروژه ایجاد گردد
        /// </summary>
        [StringValidation]
        public string OrderListUrl { get; set; }

        /// <summary>
        /// مبلغ پرداخت شده
        /// مورد استفاده در پرداخت نقدی اقساطی طرح باقرالعلوم
        /// به دلیل این که payment
        /// را در آخر درج می کنیم به مبلغ پرداختی نیاز داریم
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public string InstallmentCount { get; set; }

        /// <summary>
        /// شناسه HybridPaymentRequest
        /// </summary>
        public string HybridPaymentRequestId { get; set; }

        /// <summary>
        /// شناسه HekmatGatewayTransaction
        /// </summary>
        public string HekmatGatewayTransactionId { get; set; }

        /// <summary>
        /// شناسه HybridPayment
        /// </summary>
        public string HybridPaymentId { get; set; }

        /// <summary>
        /// RRN
        /// (شماره مرجع بازیابی تراکنش / کد مرجع تراکنش)
        /// کد مرجع تراکنش که در صورت موفق بودن تراکنش ارسال می گردد
        /// </summary>
        public string RetrievalReferenceNumber { get; set; }

        /// <summary>
        /// (شماره پیگیری/ردیابی سیستمی تراکنش)
        /// کد رهگیری تراکنش
        /// در صورت موفق بودن تراکنش ارسال می گردد
        /// </summary>
        public string SystemTraceAuditNumber { get; set; }
        /// <summary>
        /// توکن ساخته شده برای درخواست پرداخت
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// ReservationNumber
        /// </summary>
        public string ReservationNumber { get; set; }


    }
}
 
