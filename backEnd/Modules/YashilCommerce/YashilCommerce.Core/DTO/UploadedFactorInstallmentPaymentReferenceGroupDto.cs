using System;
using Yashil.Core.Entities;


namespace YashilCommerce.Core.DTO
{
    public class UploadedFactorInstallmentPaymentReferenceGroupDto
    {

        /// <summary>
        /// شناسه 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// سال و ماه
        /// </summary>
        public int YearMonth { get; set; }
        /// <summary>
        /// زمان ثبت جدول
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        ///  زمان ثبت جدول به فارسی
        /// </summary>
        public string CreationPersianDateTime { get; set; }
        /// <summary>
        /// شناسه مرجع پرداخت
        /// </summary>
        public string PaymentReferenceId { get; set; }
        /// <summary>
        /// زمان ثبت شناسه مرجع پرداخت
        /// </summary>
        public string PaymentReferencePersianDateTime { get; set; }
    }

    public class UploadedFactorInstallmentPaymentReferenceDto
    {
        public int Id { get; set; }
        /// <summary>
        /// نام دسته بندی
        /// </summary>
        public string CategoryGroupTitle { get; set; }
        /// <summary>
        /// نام مرکز
        /// </summary>
        public string CompanyOrRepresentationTitle { get; set; }
        /// <summary>
        /// کد نمایندگی مرکز
        /// </summary>
        public string CompanyOrRepresentationCode { get; set; }
        /// <summary>
        /// مبلغ قسط
        /// </summary>
        public string InstallmentAmount { get; set; }
        /// <summary>
        /// شماره قسط
        /// </summary>
        public string InstallmentNumber { get; set; }
    }
}
