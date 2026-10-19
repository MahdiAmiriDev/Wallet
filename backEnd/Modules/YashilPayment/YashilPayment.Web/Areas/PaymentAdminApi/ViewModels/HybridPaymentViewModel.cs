using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
    public class HybridPaymentListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        public Int64 Price { get; set; }

        [StringValidation] public string HybridPaymentRequestTitle { get; set; }

        public int? PaidDateInt { get; set; }

        [StringValidation] public string AuthorizationTransactionId { get; set; }

        [StringValidation] public string InstallmentTrackingCode { get; set; }

        [StringValidation] public string ReservationNumber { get; set; }

        [StringValidation] public string TraceNumber { get; set; }

        [StringValidation] public string AuthorizationTransactionCode { get; set; }

        public DateTime? PaidDateUtc { get; set; }

        public int? PaidDateUtcInt { get; set; }

        public int SaleType { get; set; }

        public int? ErrorCode { get; set; }

        public int? InstallmentCount { get; set; }

        [StringValidation] public string HybridPaymentStatusTypeTitle { get; set; }

        [StringValidation] public string PaymentMethodTypeTitle { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
    }


    public class HybridPaymentEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)]  [Required] [StringValidation] public string Title { get; set; }

        [Required] public Int64 Price { get; set; }
        [Range(0, int.MaxValue)] [Required] public int HybridPaymentRequestId { get; set; }
        public int? PaidDateInt { get; set; }
        [StringLength(600)] [StringValidation] public string AuthorizationTransactionId { get; set; }
        [StringLength(600)] [StringValidation] public string InstallmentTrackingCode { get; set; }
        [StringLength(400)] [Required] [StringValidation] public string ReservationNumber { get; set; }
        [StringLength(200)][StringValidation] public string TraceNumber { get; set; }
        [StringLength(600)] [StringValidation] public string AuthorizationTransactionCode { get; set; }
        [StringValidation] public string AuthorizationTransactionResult { get; set; }
        public DateTime? PaidDateUtc { get; set; }
        public int? PaidDateUtcInt { get; set; }
        [Range(0, int.MaxValue)] [Required] public int SaleType { get; set; }
        public int? ErrorCode { get; set; }
        public int? InstallmentCount { get; set; }
        [Range(0, int.MaxValue)] [Required] public int HybridPaymentStatusType { get; set; }
        public int? PaymentMethodType { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }
    }


    public class HybridPaymentSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

    public class HybridPaymentIpgListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        public int PaidDateInt { get; set; }
        public int SaleType { get; set; }
        [StringValidation] public string SaleTypeTitle { get; set; }
        public int PaymentMethodtype { get; set; }

        [StringValidation] public string PaymentMethodTypeTitle { get; set; }
        public int HybridPaymentStatusType { get; set; }
        [StringValidation] public string HybridPaymentStatusTypeTitle { get; set; }
        public long Price { get; set; }
        
    }

    public class HybridPaymentIpgVerifyViewModel
    {
        /// <summary>
        /// شناسه پرداخت  : نقدی یا اقساطی
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شناسه درخواست پرداخت ترکیبی
        /// </summary>
        public int HybridPaymentRequestId { get; set; }
        /// <summary>
        /// کد پیگیری درگاه پرداخت ترکیبی
        /// </summary>
        [StringValidation] public string HybridPaymentRequestTitle { get; set; }
        /// <summary>
        /// وضعیت پرداخت : اقساطی یا نقدی
        /// </summary>
        public int HybridPaymentStatusType { get; set; }
        /// <summary>
        /// تاریخ تایید
        /// </summary>
        public DateTime? PaidDateUtc { get; set; }
        [StringValidation] public string AuthorizationTransactionResult { get; set; }
        /// <summary>
        /// آدرس بازگشتی به سایت پذیرنده
        /// </summary>
        [StringValidation] public string HybridPaymentRequestCallBackUrl { get; set; }
        /// <summary>
        /// مبلغ پرداختی
        /// </summary>
        public long Price { get; set; }
        /// <summary>
        /// از فرمول برای محاسبه قیمت استفاده شود ؟
        /// </summary>
        public bool? UseFormula { get; set; }


    }
    /// <summary>
    /// مدلی که از سمت کلاینت برای حذف پرداخت ترکیبی به سمت سرور ارسال می شود
    /// تا پرداخت ترکیبی کنسل شود
    /// </summary>
    public class HybridPaymentCancelModel
    {
        /// <summary>
        /// شناسه ای که از سمت پذیرنده به درگاه ارسال می شود
        /// </summary>
        public int ReservationNumber { get; set; }
        /// <summary>
        /// وضعیت درخواست پرداخت که به سمت پذیرنده ارسال می شود
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// توکن 
        /// </summary>
        [StringValidation] public string? PaymentRequestToken { get; set; }
        /// <summary>
        /// آدرس بازگشتی به سایت پذیرنده
        /// </summary>
        [StringValidation] public string? CallBackUrl { get; set; }
    }
    
}