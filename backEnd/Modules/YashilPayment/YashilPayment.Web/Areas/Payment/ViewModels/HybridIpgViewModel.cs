using Yashil.Common.Core.Attributes;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.Payment.ViewModels
{
    /// <summary>
    /// وقتی کاربر از سایت فروشگاهی به صفحه
    /// Ipg
    /// فرستاده می شود اطلاعاتی در قالب پارمتر های زیر به صفحه
    /// Ipg
    /// ارسال می شود
    /// برای پرداخت ترکیبی
    /// </summary>
    public class HybridIpgViewModel
    {
        public HybridIpgViewModel()
        {
            HybridPayments = new List<HybridPaymentIpgListViewModel>();
        }

        /// <summary>
        /// مبلغ پایه ارسالی به درگاه
        /// </summary>
        public long BasePrice { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        /// نام پذیرنده
        /// </summary>
        [StringValidation] public string MerchantName { get; set; }
        /// <summary>
        /// آدرس پذیرنده
        /// </summary>
        [StringValidation] public string MerchantUrl { get; set; }
        /// <summary>
        /// شماره پذیرنده
        /// </summary>
        public int MerchantId { get; set; }
        /// <summary>
        /// توکن 
        /// </summary>
        [StringValidation] public string PaymentRequestToken { get; set; }
        /// <summary>
        /// شماره ترمینال
        /// </summary>
        public int TerminalId { get; set; }
        /// <summary>
        /// اطلاعات سفارشی که پرداخت حکمت انجام می شود  
        /// </summary>
        [StringValidation] public string? AdditionalDate { get; set; }
        /// <summary>
        /// آدرس بازگشتی به سایت پذیرنده
        /// </summary>
        [StringValidation] public string CallBackUrl { get; set; }
        /// <summary>
        /// شناسه پرداخت پذیرنده
        /// </summary>
        [StringValidation] public string ReservationNumber { get; set; }
        /// <summary>
        /// درصد پرداخت نقدی
        /// </summary>
        public int? CashPaymentPercentage { get; set; }
        /// <summary>
        /// شناسه درخوایت پرداخت ترکیبی
        /// </summary>
        public int? HybridPaymentRequestId { get; set; }
        /// <summary>
        /// درصد سود پرداخت اقساطی تامیین کننده
        /// </summary>
        public decimal SupplierWage { get; set; }
        /// <summary>
        /// درصد سود پرداخت اقساطی سایت یا شرکت
        /// </summary>
        public decimal InstituteWage { get; set; }
        /// <summary>
        /// درصد سود پرداخت نقدی سایت یا شرکت
        /// </summary>
        public decimal CashInstituteWage { get; set; }
        /// <summary>
        /// درصد سود پرداخت نقدی تامیین کننده
        /// </summary>
        public decimal CashSupplierWage { get; set; }
        /// <summary>
        /// وضعیت درخواست پرداخت ترکیبی
        /// </summary>
        public int? HybridPaymentRequestStatus { get; set; }
        [StringValidation] public string HybridPaymentRequestStatusTitle { get; set; }
        /// <summary>
        /// مبلغ محاسبه شده برای پرداخت نقدی با توجه به درصدرداخت نقدی انتخاب شده
        /// </summary>
        public long? CashPaymentPrice {get;set;}
        /// <summary>
        ///  مبلغ محاسبه شده برای پرداخت اقساطی با توجه به درصدرداخت نقدی انتخاب شده
        /// </summary>
        public long? HekmatPaymentPrice { get; set; }
        /// <summary>
        /// مبلغ باقیمانده نقدی
        /// ممکن است قبلا پرداخت نقدی داشته باشد و بعدا مبلغ اصلاح شود
        /// که البته الان این موضوع کاربردی ندارد ولی بعدا برای اصلاح تراکنشات این مورد درست است
        /// </summary>
        public long? RemainingCashPaymentPrice { get; set; }
        /// <summary>
        /// مبلغ باقیمانده اقساطی
        /// ممکن است قبلا پرداخت اقساطی داشته باشد و بعدا مبلغ اصلاح شود
        /// که البته الان این موضوع کاربردی ندارد ولی بعدا برای اصلاح تراکنشات این مورد درست است
        /// </summary>
        public long? RemainingHekmatPaymentPrice { get; set; }
        /// <summary>
        /// حداکثر مبلغ پرداختی نقدی : وقتی درصد نقدی برابر با 100 باشد
        /// </summary>
        public long  MaxCashPaymentPrice {get;set;}
        /// <summary>
        /// حداکثر مبلغ پرداختی اقساطی : وقتی درصد نقدی برابر با 0 باشد
        /// </summary>
        public long  MaxHekmatPaymentPrice { get;set;}
        /// <summary>
        /// هزینه ارسال
        /// </summary>
        public int? ShippingAmount { get; set; }
        /// <summary>
        ///  درصد مالیات
        /// </summary>
        public int Tax { get; set; }

        public List<HybridPaymentIpgListViewModel>? HybridPayments { get; set; }
        /// <summary>
        /// پرداخت تعیین تکلیف نشده دارد
        /// HybridPaymentStatusType یعنی
        /// در وضعیت ارسال کاربر به درگاه پرداخت می باشد
        /// یا در وضعیت منتظر پاسخ درگاه می باشد : کاربر مشخصات را زده و متد
        /// verfiy
        /// سمت پذیرنده قراخوانی شده است 
        /// </summary>
        public bool HasAnyPaymentNotDetermined { get; set; }
        /// <summary>
        /// اجازه پرداخت اقساطی با حکمت
        /// </summary>
        public bool AllowPayHekmat {get; set; }
        /// <summary>
        /// اجازه پرداخت نقدی
        /// </summary>
        public bool AllowPayCash { get; set; }
        /// <summary>
        /// مبلغ هر قسط
        /// </summary>
        public long? AmountOfEachInstallment { get; set;}
    }
}
