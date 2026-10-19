
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

    public class HekmatPaymentGatewayTransactionListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        public int HekmatReservationNumber { get; set; }
        [StringValidation] public string LocalReservationNumber { get; set; }
        [StringValidation] public string MerchantTitle { get; set; }
        [StringValidation] public string HekmatTerminalTitle { get; set; }
        public Int64 Price { get; set; }
        public Int64? PriceToPay { get; set; }
        public int InstallmentCount { get; set; }
        public int? StatusCode { get; set; }
        public int? ErrorCode { get; set; }
        [StringValidation] public string HekmatCardNumber { get; set; }
        [StringValidation] public string NationalCode { get; set; }
        [StringValidation] public string Mobile { get; set; }
        [StringValidation] public string Tell { get; set; }
        public int? ApproveDateInt { get; set; }
        public int? CreationDateInt { get; set; }
        public int? OldHekmatId { get; set; }
        public bool HasBeenInvoiced { get; set; }
        public int? FactorId { get; set; }
        [StringValidation] public string HekmatTransactionResultMessage { get; set; }


    }


    public class HekmatPaymentGatewayTransactionEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }

        [Required] public Int64 Price { get; set; }
        public Int64? BasePrice { get; set; }
        public Int64? PriceToPay { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int MerchantId { get; set; }

        [Required] [StringValidation] public string CallBackUrl { get; set; }
        [StringValidation] public string AdditionalDate { get; set; }
        [StringLength(400)] [Required] [StringValidation] public string LocalReservationNumber { get; set; }
        public int? StatusCode { get; set; }
        public int? ErrorCode { get; set; }
        [StringLength(100)] [StringValidation] public string HekmatCardNumber { get; set; }
        [StringLength(100)] [StringValidation] public string NationalCode { get; set; }
        [StringLength(100)] [StringValidation] public string Mobile { get; set; }
        [StringLength(100)] [StringValidation] public string Tell { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int InstallmentCount { get; set; }
        [StringValidation] public string HekmatToken { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int HekmatTerminalId { get; set; }
        [StringValidation] public string HekmatTransactionResultMessage { get; set; }
        public bool HasBeenInvoiced { get; set; }
        public DateTime? ApproveDate { get; set; }
        public int? ApproveDateInt { get; set; }
        public int? OldHekmatId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int HekmatReservationNumber { get; set; }
        public int? FactorId { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class HekmatPaymentGatewayTransactionSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

    }


    public class HekmatPaymentGatewayTransactionCustomViewModel 
    {
      

        public int Id { get; set; }
        /// <summary>
        /// قیمت
        /// </summary>
        public Int64 Price { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>

        [StringValidation] public string HekmatCardNumber { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>

        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// تعداد قسط
        /// </summary>
        public int InstallmentCount { get; set; }
        /// <summary>
        /// کد رهگیری پذیرنده سمت حکمت
        /// </summary>

        public int HekmatReservationNumber { get; set; }

        /// <summary>
        /// آیا سفارش آموزشی می باشد یا خیر ؟ 
        /// </summary>
        public int IsLearn { get; set; }

    }

}
