using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{
    public class HekmatPaymentGatewayTransactionBulkRequestDetailListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }

        public int Id { get; set; }

        public string AppDocumentTitle { get; set; }

        public string ParentTitle { get; set; }

        public string Code { get; set; }

        public string HekmatPaymentGatewayTransactionBulkRequestTitle { get; set; }

        public string CardNumber { get; set; }

        public bool? CardNumberIsValid { get; set; }

        public string NationalCode { get; set; }

        public bool? NationalCodeIsValid { get; set; }

        public string Price { get; set; }

        public bool? PriceIsValid { get; set; }

        public string InstallmentCount { get; set; }

        public bool? InstallmentCountIsValid { get; set; }

        public int StatusCode { get; set; }

        public int ErrorCode { get; set; }

        public string HekmatTransactionResultMessage { get; set; }

        public string CodePath { get; set; }

        public string AccessLevelTitle { get; set; }
        /// <summary>
        /// وضعیت تایید شده یا رد شده
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// تاریخ پرداخت حکمت
        /// </summary>
        public int PaidDateInt { get; set;}

    }


    public class HekmatPaymentGatewayTransactionBulkRequestDetailEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int AppDocumentId { get; set; }

        public int? ParentId { get; set; }

        [StringLength(300)]
        public string Code { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int HekmatPaymentGatewayTransactionBulkRequestId { get; set; }

        [StringLength(32)]
        [Required]

        public string CardNumber { get; set; }

        public bool? CardNumberIsValid { get; set; }

        [StringLength(20)]
        [Required]

        public string NationalCode { get; set; }

        public bool? NationalCodeIsValid { get; set; }

        [Required]

        public Int64 Price { get; set; }

        public bool? PriceIsValid { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public string InstallmentCount { get; set; }

        public bool? InstallmentCountIsValid { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int StatusCode { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int ErrorCode { get; set; }

        [StringLength(2000)]
        public string HekmatTransactionResultMessage { get; set; }

        [StringLength(800)]
        public string CodePath { get; set; }

        [Range(0, int.MaxValue)]

        [Required]

        public int AccessLevelId { get; set; }

    }

    public class HekmatPaymentGatewayTransactionBulkRequestDetailSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        public int ParentId { get; set; }


        public string Title { get; set; }

    }

    public class HekmatPaymentGatewayTransactionBulkRequestDetailListSimpleViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// آیا شماره کارت معتب نمی باشد
        /// </summary>
        public bool CardNumberIsInValid { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        public string NationalCode { get; set; }
        /// <summary>
        /// آیا ک نامعتبر می باشد
        /// </summary>
        public bool NationalCodeIsInValid { get; set; }
        /// <summary>
        /// قیمت
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// قیت نامعتبر می باشد
        /// </summary>
        public bool PriceIsInValid { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public string InstallmentCount { get; set; }
        /// <summary>
        /// تعداد اقساط نامعتبر می باشد
        /// </summary>
        public bool InstallmentCountIsInValid { get; set; }
        /// <summary>
        /// پیام اعتبار سنجی
        /// </summary>
        public string ValidateMessage { get; set; }
    }
}
