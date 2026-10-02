using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class PaymentComparisonWithHekmatApprovedTransactionViewModel
    {
        public int Id { get; set; }

        /// <summary>
        /// کد رهگیری پذیرنده
        /// </summary>
        [StringValidation] public string? InstallmentTrackingCode { get; set; }
        /// <summary>
        /// کد رهگیری پذیرنده در سی دی تراکنش های تایید شده 
        /// </summary>
        [StringValidation] public string? HekmatInstallmentTrackingCode { get; set; }
        /// <summary>
        /// شماره کارت خریدار
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }
        /// <summary>
        /// شماره کارت ثبت شده در   سی دی حکمت
        /// </summary>
        [StringValidation] public string? HekmatCardNumber { get; set; }
        /// <summary>
        /// تاریخ پرداخت با حکمت کارت در سایت 
        /// </summary>
        public int PaymentPaidDateInt { get; set; }
        /// <summary>
        /// تاریخ پرداخت حکمت در سی دی تراکنش های تایید شده 
        /// </summary>
        public int? HekmatTransactionDate { get; set; }
        /// <summary>
        /// مبلغ پرداخت 
        /// </summary>
        public long PaymentTotalInt { get; set; }
        /// <summary>
        /// مبلغ پرداخت حکمت در سی دی تراکنش های تایید شده 
        /// </summary>
        public long? HekmatPrice { get; set; }
        /// <summary>
        ///تعداد قسط 
        /// </summary>
        public long? InstallmentCount { get; set; }
        /// <summary>
        /// عنوان پروژه
        /// </summary>
        [StringValidation] public string ApplicationTitle { get; set; }
        /// <summary>
        /// مقدار  عددی شماره کارت ثبت شده در حکمت برای مقایسه
        /// </summary>
        public long CardNumberInt { get; set; }
        /// <summary>
        /// مقدار  عددی شماره کارت ثبت شده در سی دی حکمت
        /// </summary>
        public long? HekmatCardNumberInt { get; set; }

        public bool IsNotSamePrice { get; set; }
        public bool IsNotSameCardNumber { get; set; }
        public bool IsNotSameInstallmentTrackingCode { get; set; }

        /// <summary>
        ///  شناسه روش پرداخت
        /// </summary>
        public int PaymentMethodType { get; set; }

        /// <summary>
        ///  نام روش پرداخت
        /// </summary>
        [StringValidation] public string PaymentMethodTitle { get; set; }
        /// <summary>
        /// آیا اطلاعات تراکنش های پرداخت حکمت در سی دی حکمت وجود  ندارد
        /// </summary>
        public bool NotExistInHekmat { get; set; }
        /// <summary>
        /// شماره مرجع مربوط به پرداخت شتاب
        /// </summary>
        [StringValidation] public string? CustomerRefNum { get; set; }

        public int PaymentId { get; set; }
        /// <summary>
        /// جهت نمایش دکمه عودت وجه های پرداخت
        /// اگر عودت وجهی وجود داشته باشد آنگاه این دکمه را نشان میدهیم
        /// </summary>
        public bool AllowShowRefundForPaymentBtn { get; set; }
    }
}
