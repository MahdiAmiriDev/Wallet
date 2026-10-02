using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class PaymentListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        public int OrderId { get; set; }

        public int? PaidDateInt { get; set; }

        [StringValidation] public string CardTypeTitle { get; set; }

        [StringValidation] public string ReservationNumber { get; set; }

        [StringValidation] public string AuthorizationTransactionId { get; set; }
        [StringValidation] public string AuthorizationTransactionResult { get; set; }

        [StringValidation] public string AuthorizationTransactionCode { get; set; }
        /// <summary>
        ///  تاریخ تایید پرداخت
        /// </summary>
        public DateTime? PaidDateUtc { get; set; }

        [StringValidation] public string SaleTypeTitle { get; set; }

        public int? ErrorCode { get; set; }

        [StringValidation] public string TraceNumber { get; set; }

        [StringValidation] public string PaymentStatusTypeTitle { get; set; }
        public int PaymentStatusType { get; set; }

        [StringValidation] public string PaymentMethodTypeTitle { get; set; }

        public decimal? PricePerInstallmentExclSubsidy { get; set; }

        public decimal? PricePerInstallmentInclSubsidy { get; set; }
        /// <summary>
        /// تعداد قسط در صورتی که پرداخت اقساطی باشد
        /// </summary>
        public int? InstallmentCount { get; set; }
        /// <summary>
        /// مبلغ نهایی برای پرداخت
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// مبلغ نهایی برای نمایش در فرم پرداخت
        /// </summary>
        public long? PayTotal
        {
            get
            {
                if (Total != null)
                {
                    return (long)Total;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// مبلغ جدید اصلاح شده پرداختی
        /// </summary>
        public decimal? NewPaymentAmount { get; set; }

        public decimal? SumRefundAmount { get; set; }
    }

    public class CustomerClubPaymentGroupItemListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        public int OrderId { get; set; }
        /// <summary>
        ///  کد پرداخت
        /// </summary>
        [StringValidation] public string ReservationNumber { get; set; }
        /// <summary>
        /// کد سفارش
        /// </summary>
        [StringValidation] public string OrderTitle { get; set; }
        /// <summary>
        /// نام دوره
        /// </summary>
        [StringValidation] public string ProductTitle { get; set; }
        /// <summary>
        /// نام طرح
        /// </summary>
        [StringValidation] public string ApplicationName { get; set; }
        /// <summary>
        /// کد ملی
        /// </summary>
        [StringValidation] public string NationalCode { get; set; }
        /// <summary>
        /// کدملی صاحب حکمت-کد ناجا
        /// </summary>
        [StringValidation] public string CustomerClubCardCode { get; set; }
        /// <summary>
        /// نام استان
        /// </summary>
        [StringValidation] public string ProvinceTitle { get; set; }
        /// <summary>
        /// نام شهر
        /// </summary>
        [StringValidation] public string CityTitle { get; set; }
        /// <summary>
        /// شماره کارت
        /// </summary>
        [StringValidation] public string CardNumber { get; set; }
        /// <summary>
        /// تعداد اقساط
        /// </summary>
        public int InstallmentCount { get; set; }
        public decimal? Total { get; set; }
        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime CreationDate { get; set; }

    }

    public class PaymentEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int OrderId { get; set; }
        public int? PaidDateInt { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CardType { get; set; }
        [StringValidation] public string CardName { get; set; }
        [StringValidation] public string CardNumber { get; set; }
        [StringValidation] public string MaskedCreditCardNumber { get; set; }
        [StringValidation] public string CardCvv2 { get; set; }
        [StringValidation] public string CardExpirationMonth { get; set; }
        [StringValidation] public string CardExpirationYear { get; set; }
        [StringLength(400)][StringValidation]
        [Required]
        public string ReservationNumber { get; set; }
        [StringLength(600)] [StringValidation] public string AuthorizationTransactionId { get; set; }
        [StringLength(600)] [StringValidation] public string AuthorizationTransactionCode { get; set; }
        [StringValidation] public string AuthorizationTransactionResult { get; set; }
        public DateTime? PaidDateUtc { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int SaleType { get; set; }
        public int? ErrorCode { get; set; }
        [StringLength(200)][StringValidation] public string TraceNumber { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int PaymentStatusType { get; set; }
        public int? PaymentMethodType { get; set; }
        public decimal? PricePerInstallmentExclSubsidy { get; set; }
        public decimal? PricePerInstallmentInclSubsidy { get; set; }
        public int? InstallmentCountInclSubsidy { get; set; }
        public int? InstallmentCountExclSubsidy { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class PaymentSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

    public class SuccessFulPaymentListViewModel
    {
        public int PaymentStatusType { get; set; }
        /// <summary>
        ///  تاریخ تایید پرداخت
        /// </summary>
        public DateTime? PaidDateUtc { get; set; }
        /// <summary>
        /// نام درگاه بانکی
        /// </summary>
        [StringValidation] public string PaymentMethodTypeTitle { get; set; }
        /// <summary>
        /// مبلغ نهایی برای پرداخت
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// مبلغ نهایی برای نمایش در فرم پرداخت
        /// </summary>
        public long? PayTotal
        {
            get
            {
                if (Total != null)
                {
                    return (long)Total;
                }
                else
                {
                    return null;
                }
            }
        }
    }
    public class PaymentAssignListViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string ReservationNumber { get; set; }
        [StringValidation] public string OrderTitle { get; set; }
        /// <summary>
        /// مبلغ نهایی برای پرداخت
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// مبلغ نهایی برای نمایش در فرم پرداخت
        /// </summary>
        public long? PayTotal
        {
            get
            {
                if (Total != null)
                {
                    return (long)Total;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
