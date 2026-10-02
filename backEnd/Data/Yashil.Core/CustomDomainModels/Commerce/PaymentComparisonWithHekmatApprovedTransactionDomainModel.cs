namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class PaymentComparisonWithHekmatApprovedTransactionDomainModel
    {
        public int Id { get; set; }
        /// <summary>
        /// کد رهگیری پذیرنده
        /// </summary>
        public string? InstallmentTrackingCode { get; set; }
        /// <summary>
        /// کد رهگیری پذیرنده در سی دی تراکنش های تایید شده 
        /// </summary>
        public string? HekmatInstallmentTrackingCode { get; set; }
        /// <summary>
        /// شماره کارت خریدار
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// شماره کارت ثبت شده در   سی دی حکمت
        /// </summary>
        public string? HekmatCardNumber { get; set; }
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
        public string ApplicationTitle { get; set; }
        /// <summary>
        /// مقدار  عددی شماره کارت ثبت شده در حکمت برای مقایسه
        /// </summary>
        public long CardNumberInt { get; set; }
        /// <summary>
        /// مقدار  عددی شماره کارت ثبت شده در سی دی حکمت
        /// </summary>
        public long? HekmatCardNumberInt { get; set; }

        /// <summary>
        ///  شناسه روش پرداخت
        /// </summary>
        public int PaymentMethodType { get; set; }

        /// <summary>
        ///  نام روش پرداخت
        /// </summary>
        public string PaymentMethodTitle { get; set; }
        /// <summary>
        /// شماره مرجع مربوط به پرداخت شتاب
        /// </summary>
        public string? CustomerRefNum { get; set; }

        public int PaymentId { get; set; }
        /// <summary>
        /// مبلغ جدید پرداختی در عودت وجه
        /// </summary>
        public decimal? NewPaymentAmount { get; set; }
    }
}
