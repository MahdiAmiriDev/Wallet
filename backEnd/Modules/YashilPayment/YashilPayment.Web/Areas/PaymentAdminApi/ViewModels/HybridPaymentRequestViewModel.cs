using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
    public class HybridPaymentRequestListViewModel : IBaseViewModel
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
        /// <summary>
        /// مبلغ پایه
        /// </summary>

        public Int64 BasePrice { get; set; }
        /// <summary>
        /// نام پذیرنده
        /// </summary>

        [StringValidation] public string MerchantTitle { get; set; }
        /// <summary>
        /// نام ترمینال
        /// </summary>

        [StringValidation] public string YashilHekmatTerminalTitle { get; set; }
        /// <summary>
        /// شناسه سفارش تراکنش که به صورت یکتا از سمت پذیرنده ارسال می شود
        /// </summary>

        [StringValidation] public string LocalReservationNumber { get; set; }
        /// <summary>
        /// وضعیت درخواست پرداخت نقدی اقساطی
        /// </summary>

        public int StatusCode { get; set; }
        /// <summary>
        /// شماره کارت حکمت
        /// </summary>

        [StringValidation] public string HekmatCardNumber { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>

        public int? InstallmentCount { get; set; }
        /// <summary>
        /// درصد اقساطی ترمینال 
        /// </summary>

        public decimal SupplierWage { get; set; }
        /// <summary>
        /// درصد اقساطی درگاه
        /// </summary>

        public decimal InstituteWage { get; set; }
        /// <summary>
        /// درصد نقدی درگاه
        /// </summary>

        public int CashInstituteWage { get; set; }
        /// <summary>
        /// درصد نقدی ترمینال
        /// </summary>

        public int CashSupplierWage { get; set; }
        /// <summary>
        /// مالیات
        /// </summary>

        public int Tax { get; set; }
        /// <summary>
        /// درصد نقدی که از سمت پذیرنده ارسال می شود
        /// در غیر اینصورت کاربر انتخاب می کند
        /// </summary>

        public int? CashPaymentPercentage { get; set; }

        [StringValidation] public string HybridPaymentRequestStatusTypeTitle { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
    }


    public class HybridPaymentRequestEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)]  [Required] [StringValidation] public string Title { get; set; }

        [Required] public Int64 BasePrice { get; set; }
        [Range(0, int.MaxValue)] [Required] public int MerchantId { get; set; }
        [Range(0, int.MaxValue)] [Required] public int YashilHekmatTerminalId { get; set; }

        [Required] [StringValidation] public string CallBackUrl { get; set; }
        [StringValidation] public string AdditionalDate { get; set; }
        [StringLength(400)] [Required] [StringValidation] public string LocalReservationNumber { get; set; }
        [Range(0, int.MaxValue)] [Required] public int StatusCode { get; set; }
        [StringLength(100)] [StringValidation] public string HekmatCardNumber { get; set; }
        public int? InstallmentCount { get; set; }

        [Range(0, double.MaxValue)]
        [Required]
        public decimal SupplierWage { get; set; }

        [Range(0, double.MaxValue)]
        [Required]
        public decimal InstituteWage { get; set; }

        [Range(0, int.MaxValue)] [Required] public int CashInstituteWage { get; set; }
        [Range(0, int.MaxValue)] [Required] public int CashSupplierWage { get; set; }
        [Range(0, int.MaxValue)] [Required] public int Tax { get; set; }
        public int? CashPaymentPercentage { get; set; }
        [Range(0, int.MaxValue)] [Required] public int HybridPaymentRequestStatus { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }
    }


    public class HybridPaymentRequestSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

    public class HybridPaymentRequestIpgViewModel : IBaseViewModel
    {
        public HybridPaymentRequestIpgViewModel()
        {
            HybridPayments = new List<HybridPaymentIpgListViewModel>();
        }

        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        public long BasePrice { get; set; }
        public decimal SupplierWage { get; set; }
        public decimal InstituteWage { get; set; }
        public int InstallmentCount { get; set; }
        public decimal CashInstituteWage { get; set; }
        public decimal CashSupplierWage { get; set; }
        [StringValidation] public string CardNumber { get; set; }
        public int HybridPaymentRequestStatus { get; set; }
        [StringValidation] public string HybridPaymentRequestStatusTypeTitle { get; set; }
        public int CashPaymentPercentage { get; set; }
        [StringValidation] public string AdditionalData { get; set; }
        public List<HybridPaymentIpgListViewModel> HybridPayments { get; set; }
    }
    /// <summary>
    /// برای درخواست محاسبه قیمت مورد استفاده قرار میگیرد
    /// این ویومدل در هنگامی مورد استفاده قرار میگیرد که در صفحه پرداخت ترکیبی کاربر درصد پرداخت نقدی را وارد میکن و دکمه محاسبه قیمت را میزند
    /// </summary>
    public class CalculatePriceForPayRequestViewModel
    {
        [Required]
        [StringValidation] public string PaymentRequestToken { get; set; }
        [Required]
        [Range(0,100)]
        public int CashPaymentPercentage { get; set; }

    }
    public class CalculatePriceForPayResultViewModel
    {
        public bool Succeed { get; set; }
        [StringValidation] public string Message { get; set; }
        public long BasePrice { get; set; }
        public long CashPaymentPrice { get; set; }
        public long HekmatPaymentPrice { get; set; }
        public int CashPaymentPercentage { get; set; }
        public int InstallmentCount { get; set; }
        public decimal SupplierWage { get; set; }
        public decimal InstituteWage { get; set; }
        public decimal CashInstituteWage { get; set; }
        public decimal CashSupplierWage { get; set; }

        public bool IsValidHekmatPrice { get; set; }
        public int? ShippingAmount { get; set; }
        public int Tax { get; set; }
    }
    
    /// <summary>
    /// این ویومدل برای زدن یکی از دکمه های پرداخت نقدی یا اقساطی برای ارسال اطلاعات به سرور مورد استفاده قرار میگیرد
    /// </summary>
    public class HybridPurchaseRequestViewModel
    {
        /// <summary>
        /// توکن ارسال شده برای پرداخت
        /// </summary>
        [Required]
        [StringValidation] public string PaymentRequestToken { get; set; }
        /// <summary>
        /// درصد پرداخت نقدی
        /// ممکن است از سمت سایت پذیرنده این مقدار آمده باشد
        /// در غیر اینصورت توسط کاربر درصد پرداخت نقدی انتخاب می شود
        /// </summary>
        [Range(0,100)]
        public int? CashPaymentPercentage { get; set; }

    }
}