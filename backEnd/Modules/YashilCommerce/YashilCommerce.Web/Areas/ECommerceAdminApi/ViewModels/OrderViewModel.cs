using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class OrderListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        [StringValidation] public string Title { get; set; }

        [StringValidation] public string CardNumber { get; set; }

        [StringValidation] public string NationalCode { get; set; }

        [StringValidation] public string UserTitle { get; set; }

        [StringValidation] public string? RecipientMobileNumber { get; set; }
        public int OrderStatusType { get; set; }
        [StringValidation] public string OrderStatusTypeTitle { get; set; }
        [StringValidation] public string SubsidyTitle { get; set; }

        [StringValidation] public string SubsidyStatusTypeTitle { get; set; }

        [StringValidation] public string SubsidyTypeTitle { get; set; }
        public int CreationDateInt { get; set; }

        [StringValidation] public string PaymentMethods { get; set; }

        /// <summary>
        /// عنوان نسبت خریدار با صاحب حکمت کارت
        /// </summary>
        [StringValidation]  public string RelationshipWithBuyerTitle { get; set; }
        /// <summary>
        /// عنوان شهر سفارش دهنده
        /// </summary>
        [StringValidation]  public string CityTitle { get; set; }
        /// <summary>
        /// وضعیت وزارت و ساتا
        /// </summary>
        [StringValidation] public string MembershipTypeTitle { get; set; }

        /// <summary>
        /// عنوان وضعیت جنسیت فرد
        /// </summary>
        public string GenderTitle { get; set; }

    }
    public class OrderEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringValidation] public string CardNumber { get; set; }
        [Required] public Guid Code { get; set; }
        public int? LastOrderId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int UserId { get; set; }
        [StringLength(10)] [StringValidation] public string NationalCode { get; set; }
        public int? AddressId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int MembershipType { get; set; }
        public int? CustomerClubId { get; set; }
        public int? CustomerClubCardId { get; set; }
        public int? BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public int? PickupAddressId { get; set; }
        public bool PickupInStore { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int RecipientId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int OrderStatusType { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ShippingStatusType { get; set; }
        public int? PaymentMethodType { get; set; }
        [StringValidation] public string CustomerCurrencyCode { get; set; }

        [Required] public decimal CurrencyRate { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CustomerTaxDisplayTypeId { get; set; }

        [Required] public decimal SubtotalInclTax { get; set; }

        [Required] public decimal SubtotalExclTax { get; set; }

        [Required] public decimal SubTotalDiscountInclTax { get; set; }

        [Required] public decimal SubTotalDiscountExclTax { get; set; }

        [Required] public decimal ShippingInclTax { get; set; }

        [Required] public decimal ShippingExclTax { get; set; }

        [Required] public decimal PaymentMethodAdditionalFeeInclTax { get; set; }

        [Required] public decimal PaymentMethodAdditionalFeeExclTax { get; set; }
        [StringValidation] public string TaxRates { get; set; }

        [Required] public decimal WagePercentage { get; set; }

        [Required] public decimal Discount { get; set; }
        public decimal? TaxInclSubsidy { get; set; }
        public decimal? TotalInclSubsidy { get; set; }
        public decimal? TaxExclSubsidy { get; set; }
        public decimal? TotalExclSubsidy { get; set; }

        [Required] public decimal RefundedAmount { get; set; }
        public int? RewardPointsHistoryEntryId { get; set; }
        [StringValidation] public string CheckoutAttributeDescription { get; set; }
        [StringValidation] public string CheckoutAttributesXml { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CustomerLanguageId { get; set; }
        public int? AffiliateId { get; set; }
        [StringValidation] public string CustomerIp { get; set; }
        public bool AllowStoringCreditCardNumber { get; set; }
        [StringValidation] public string CaptureTransactionId { get; set; }
        [StringValidation] public string CaptureTransactionResult { get; set; }
        [StringValidation] public string SubscriptionTransactionId { get; set; }
        [StringLength(20)][StringValidation] public string ShippingMethod { get; set; }
        [StringValidation] public string ShippingRateComputationMethodSystemName { get; set; }
        [StringValidation] public string CustomValuesXml { get; set; }
        [StringLength(100)]
        [StringValidation]
        [Required]
        public string CustomOrderNumber { get; set; }
        [StringLength(600)] [Required] [StringValidation] public string Title { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CreationDateInt { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        public int? SubsidyId { get; set; }
        public int? SubsidyStatusType { get; set; }
        public int? SubsidyType { get; set; }
        /// <summary>
        /// اجازه مرجوع کردن کالا
        ///</summary>
        public bool? AllowRefund { get; set; }
        /// <summary>
        /// اجازه ویرایش
        ///</summary>
        public bool? AllowEdit { get; set; }

        public string RecipientMobileNumber { get; set; }
    }
    public class OrderSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

    }


    public class OrderStatisticViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        [StringValidation] public string CardNumber { get; set; }


        [StringValidation] public string UserTitle { get; set; }

        [StringValidation] public string NationalCode { get; set; }


        public int OrderStatusType { get; set; }
        [StringValidation] public string OrderStatusTypeTitle { get; set; }



        [StringValidation] public string Title { get; set; }

        public int CreationDateInt { get; set; }

        public DateTime? ModificationDate { get; set; }

        [StringValidation] public string SubsidyTitle { get; set; }
        public int SubsidyStatusType { get; set; }

        [StringValidation] public string SubsidyStatusTypeTitle { get; set; }

        [StringValidation] public string SubsidyTypeTitle { get; set; }
        [StringValidation] public string? RecipientMobileNumber { get; set; }
        [StringValidation] public string? MobileNumber { get; set; }
        /// <summary>
        /// مبلغ باقیمانده که کاربر باید آنرا پرداخت کند
        /// </summary>
        public decimal? RemainingAmountToPay { get; set; }



    }

    /// <summary>
    /// مدل مربوط به سفارش های که در حالت منتظر تایید تامین کننده برای پرداخت هستند
    /// </summary>
    public class OrdersWithWaitingForRequiredSupplierApprovalBeforePaymentViewModel
    {
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شناسه ایتم سفارش
        /// </summary>
        public int OrderItemId { get; set; }
        /// <summary>
        /// عنوان محصول
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }
        /// <summary>
        /// شناسه کاربر سفارش دهنده
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// کد ملی
        /// </summary>
        public string NationalCode { get; set; }
    }
}
