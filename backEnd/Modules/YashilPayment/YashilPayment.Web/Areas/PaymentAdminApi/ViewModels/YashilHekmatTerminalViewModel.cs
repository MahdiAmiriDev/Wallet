using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
    public class YashilHekmatTerminalListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [StringValidation] public string Title { get; set; }
        /// <summary>
        /// فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// ترمینال حکمت
        /// </summary>
        [StringValidation] public string HekmatTerminalTitle { get; set; }
        /// <summary>
        ///شناسه پذیرنده
        /// </summary>
        public int MerchantId { get; set; }
        /// <summary>
        /// عنوان پذیرنده
        /// </summary>
        [StringValidation] public string MerchantTitle { get; set; }
        /// <summary>
        /// آی پی آدرس
        /// </summary>
        [StringValidation] public string? IpAddress { get; set; }
        /// <summary>
        /// آدرس سایت
        /// </summary>
        [StringValidation] public string? Url { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [StringValidation] public string? Port { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [StringValidation] public string? Description { get; set; }
        /// <summary>
        /// سقف اقساط
        /// </summary>
        public int MaxInstallmentCount { get; set; }
        /// <summary>
        /// حداقل مبلغ تراکنش
        /// </summary>
        public long MinAmount { get; set; }
        /// <summary>
        /// حداکثر مبلغ تراکتش
        /// </summary>
        public long MaxAmount { get; set; }
        /// <summary>
        /// درصد کارمزد اقساطی پذیرنده
        /// </summary>
        public decimal SupplierWage { get; set; }
        /// <summary>
        /// درصد کارمزد اقساطی انستتیو 
        /// </summary>
        public decimal InstituteWage { get; set; }
        /// <summary>
        /// درصد کارمزد نقدی پذینرده
        /// </summary>
        public decimal CashSupplierWage { get; set; }
        /// <summary>
        /// درصد کارمزد نقدی انستیتو
        /// </summary>
        public decimal CashInstituteWage { get; set; }

    }


    public class YashilHekmatTerminalEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// فعال بودن
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// شناسه ترمینال
        /// </summary>
        [Range(0, int.MaxValue)]
        [Required]
        public int HekmatTerminalId { get; set; }
        /// <summary>
        /// شناسه پذیرنده
        /// </summary>
        [Range(0, int.MaxValue)]
        [Required]
        public int MerchantId { get; set; }
        /// <summary>
        /// آدرس سایت 
        /// </summary>
        [StringValidation] public string? Url { get; set; }
        /// <summary>
        /// کلید عمومی
        /// </summary>
        [Required]
        [StringValidation] public string PublicKey { get; set; }
        /// <summary>
        /// آی پی
        /// </summary>
       
        [StringValidation] public string? IpAddress { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        [StringValidation] public string? Description { get; set; }
        /// <summary>
        /// سقف اقساط
        /// </summary>
        [Range(0, int.MaxValue)]
        [Required] public int MaxInstallmentCount { get; set; }
        /// <summary>
        /// حداقل مبلغ تراکنش
        /// </summary>
        [Required] public long MinAmount { get; set; }
        /// <summary>
        /// حداکثر مبلغ تراکتش
        /// </summary>
        [Required] public long MaxAmount { get; set; }


        /// <summary>
        /// درصد کارمزد اقساطی پذیرنده
        /// </summary>
        [Range(0, 100)]
        public decimal SupplierWage { get; set; }
        /// <summary>
        /// درصد کارمزد اقساطی انستتیو 
        /// </summary>
        [Range(0, 100)]
        public decimal InstituteWage { get; set; }
        /// <summary>
        /// درصد کارمزد نقدی پذینرده
        /// </summary>
        [Range(0, 100)]
        public decimal CashSupplierWage { get; set; }
        /// <summary>
        /// درصد کارمزد نقدی انستیتو
        /// </summary>
        [Range(0, 100)]
        public decimal CashInstituteWage { get; set; }
    }


    public class YashilHekmatTerminalSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

    }

    public class YashilHekmatTerminalWithMerchantViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان ترمینال حکمت
        /// </summary>
        [StringValidation] public string HekmatTerminalTitle { get; set; }
        /// <summary>
        ///  نام پذیرنده
        /// </summary>
        [StringValidation] public string MerchantName { get; set; }
        /// <summary>
        /// آدرس سایت پذیرنده
        /// </summary>
        [StringValidation] public string MerchantUrl { get; set; }
        /// <summary>
        /// فعال بودن
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// درصد کارمزد اقساطی پذیرنده
        /// </summary>
        public decimal SupplierWage { get; set; }
        /// <summary>
        /// درصد کارمزد اقساطی انستتیو 
        /// </summary>
        public decimal InstituteWage { get; set; }
        /// <summary>
        /// درصد کارمزد نقدی پذیرنده
        /// </summary>
        public decimal CashSupplierWage { get; set; }
        /// <summary>
        /// درصد کارمزد نقدی انستیتو
        /// </summary>
        public decimal CashInstituteWage { get; set; }
    }

}
