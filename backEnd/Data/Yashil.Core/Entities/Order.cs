using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Order  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Order()
{
    InverseLastOrder = new HashSet<Order>();
    Issue = new HashSet<Issue>();
    OrderItem = new HashSet<OrderItem>();
    OrderNote = new HashSet<OrderNote>();
    OrderTrakking = new HashSet<OrderTrakking>();
    Payment = new HashSet<Payment>();
    Refund = new HashSet<Refund>();
    WalletReservation = new HashSet<WalletReservation>();
    WalletTransaction = new HashSet<WalletTransaction>();
 }

public int Id { get; set; }
/// <summary>
///         /// شناسه سفارش
///</summary>
public Guid Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// شماره کارت - CardHashPan
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// سفارش تغییر داده شده
///</summary>
public int? LastOrderId { get; set; }
/// <summary>
///         /// شناسه مشتری
///</summary>
public int UserId { get; set; }
/// <summary>
///         /// کد ملی
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// آدزس
///</summary>
public int? AddressId { get; set; }
/// <summary>
///         /// نوع عضویت
///</summary>
public int MembershipType { get; set; }
/// <summary>
///         /// کد کارت باشگاه مشتریان
///</summary>
public int? CustomerClubCardId { get; set; }
/// <summary>
///         /// شناسه آدرس صورتحساب
///</summary>
public int? BillingAddressId { get; set; }
/// <summary>
///         /// شناسه آدرس حمل و نقل
///</summary>
public int? ShippingAddressId { get; set; }
/// <summary>
///         /// شناسه آدرس وانت
///</summary>
public int? PickupAddressId { get; set; }
/// <summary>
///         /// گزینه حمل و نقل وانت
///</summary>
public bool PickupInStore { get; set; }
/// <summary>
///         /// شناسه وضعیت سفارش
///</summary>
public int OrderStatusType { get; set; }
/// <summary>
///         /// شناسه وضعیت حمل و نقل
///</summary>
public int ShippingStatusType { get; set; }
/// <summary>
///         /// کد ارزی مشتری
///</summary>
public string CustomerCurrencyCode { get; set; }
/// <summary>
///         /// نرخ ارز
///</summary>
public decimal CurrencyRate { get; set; }
/// <summary>
///         /// شناسه نوع نمایش مالیات مشتری
///</summary>
public int CustomerTaxDisplayTypeId { get; set; }
/// <summary>
///         /// مجموع زیر سفارش (شامل مالیات)
///</summary>
public decimal SubtotalInclTax { get; set; }
/// <summary>
///         /// مجموع زیر سفارش (بدون مالیات)
///</summary>
public decimal SubtotalExclTax { get; set; }
/// <summary>
///         /// تخفیف زیرمجموعه سفارش (شامل مالیات)
///</summary>
public decimal SubTotalDiscountInclTax { get; set; }
/// <summary>
///         /// تخفیف زیرمجموعه سفارش (بدون مالیات)
///</summary>
public decimal SubTotalDiscountExclTax { get; set; }
/// <summary>
///         /// ارسال سفارش (شامل مالیات)
///</summary>
public decimal ShippingInclTax { get; set; }
/// <summary>
///         /// ارسال سفارش (بدون مالیات)
///</summary>
public decimal ShippingExclTax { get; set; }
/// <summary>
///         /// هزینه پرداخت اضافی (شامل مالیات)
///</summary>
public decimal PaymentMethodAdditionalFeeInclTax { get; set; }
/// <summary>
///         /// هزینه پرداخت اضافی (بدون مالیات)
///</summary>
public decimal PaymentMethodAdditionalFeeExclTax { get; set; }
/// <summary>
///         /// نرخ مالیات
///</summary>
public string TaxRates { get; set; }
/// <summary>
///         /// درصد کارمزد سایت از مشتری
///</summary>
public decimal WagePercentage { get; set; }
/// <summary>
///         /// تخفیف سفارش را تعیین می کند (برای سفارش کل اعمال می شود)
///</summary>
public decimal Discount { get; set; }
/// <summary>
///         /// مالیات سفارش شامل یارانه
///</summary>
public decimal? TaxInclSubsidy { get; set; }
/// <summary>
///         /// مجموع سفارش شامل یارانه
///</summary>
public decimal? TotalInclSubsidy { get; set; }
/// <summary>
///         /// مالیات سفارش بدون یارانه
///</summary>
public decimal? TaxExclSubsidy { get; set; }
/// <summary>
///         /// مجموع سفارش بدون یارانه
///</summary>
public decimal? TotalExclSubsidy { get; set; }
/// <summary>
///         /// مبلغ بازپرداخت
///</summary>
public decimal RefundedAmount { get; set; }
/// <summary>
///         /// شناسه تاریخچه امتیاز پاداش
///</summary>
public int? RewardPointsHistoryEntryId { get; set; }
/// <summary>
///         /// توضیحات ویژگی پرداخت
///</summary>
public string CheckoutAttributeDescription { get; set; }
/// <summary>
///         /// ویژگی های پرداخت در قالب XML
///</summary>
public string CheckoutAttributesXml { get; set; }
/// <summary>
///         /// شناسه زبان مشتری
///</summary>
public int CustomerLanguageId { get; set; }
/// <summary>
///         /// شناسه وابسته
///</summary>
public int? AffiliateId { get; set; }
/// <summary>
///         /// آدرس IP مشتری
///</summary>
public string CustomerIp { get; set; }
/// <summary>
///         /// ذخیره شماره کارت اعتباری مجاز است یا خیر
///</summary>
public bool AllowStoringCreditCardNumber { get; set; }
/// <summary>
///         /// شناسه معامله ضبط
///</summary>
public string CaptureTransactionId { get; set; }
/// <summary>
///         /// نتیجه معامله ضبط
///</summary>
public string CaptureTransactionResult { get; set; }
/// <summary>
///         /// شناسه معاملات مشترک
///</summary>
public string SubscriptionTransactionId { get; set; }
/// <summary>
///         /// روش حمل و نقل
///</summary>
public string ShippingMethod { get; set; }
/// <summary>
///         /// روش محاسبه نرخ حمل و نقل
///</summary>
public string ShippingRateComputationMethodSystemName { get; set; }
/// <summary>
///         /// مقادیر مربوط درخواست پردازش پرداخت در قالب xml
///</summary>
public string CustomValuesXml { get; set; }
/// <summary>
///         /// وضعیت یارانه (تایید یا عدم تایید)
///</summary>
public int? SubsidyStatusType { get; set; }
/// <summary>
///         /// نوع یارانه
///</summary>
public int? SubsidyType { get; set; }
/// <summary>
///         /// شناسه یارانه
///</summary>
public int? SubsidyId { get; set; }
/// <summary>
///         /// سفارش ویرایش شده-در واقع حذف شده است
///</summary>
public bool IsModified { get; set; }
/// <summary>
///         /// ایجاد کننده
///</summary>
public int CreateBy { get; set; }
/// <summary>
///         /// ویرایش کننده
///</summary>
public int? ModifyBy { get; set; }
/// <summary>
///         /// زمان ایجاد
///</summary>
public DateTime CreationDate { get; set; }
/// <summary>
///         /// زمان ایجاد به شمسی
///</summary>
public int CreationDateInt { get; set; }
/// <summary>
///         /// زمان تغییر
///</summary>
public DateTime? ModificationDate { get; set; }
public int CreationDateYearInt { get; set; }
public int CreationDateMonthInt { get; set; }
public int CreationDateDayInt { get; set; }
/// <summary>
///         /// برنامه
///</summary>
public int ApplicationId { get; set; }
/// <summary>
///         /// سطح دسترسی
///</summary>
public int AccessLevelId { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }
/// <summary>
///         /// اجازه مرجوع کردن کالا
///</summary>
public bool? AllowRefund { get; set; }
/// <summary>
///         /// اجازه ویرایش
///</summary>
public bool? AllowEdit { get; set; }
public bool? IsMessageSent { get; set; }
public string OldNationalCode { get; set; }
/// <summary>
///         /// شماره موبایل
///</summary>
public string RecipientMobileNumber { get; set; }
public long? CardNumberInt { get; set; }
public int? ModificationDateInt { get; set; }
public string RecipientName { get; set; }
public string RecipientLastName { get; set; }
/// <summary>
///         /// شناسه باشگاه مشتریان
///</summary>
public int CustomerClubId { get; set; }
/// <summary>
///         /// تاریخ تولد تحویل گیرنده
///</summary>
public int? RecipientBirthDate { get; set; }
/// <summary>
///         /// جنسیت تحویل گیرنده
///</summary>
public int? RecipientGenderType { get; set; }
public int? RecipientRelationshipWithBuyerId { get; set; }
public int? CityId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Affiliate Affiliate { get; set; }
    public virtual Application Application { get; set; }
    public virtual Address BillingAddress { get; set; }
    public virtual City City { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual DayNumber CreationDateDayIntNavigation { get; set; }
    public virtual MonthNumber CreationDateMonthIntNavigation { get; set; }
    public virtual YearNumber CreationDateYearIntNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CustomerClub CustomerClub { get; set; }
    public virtual CustomerClubCard CustomerClubCard { get; set; }
    public virtual Order LastOrder { get; set; }
    public virtual CommonBaseData MembershipTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData OrderStatusTypeNavigation { get; set; }
    public virtual CommonBaseData RecipientGenderTypeNavigation { get; set; }
    public virtual CommonBaseData RecipientRelationshipWithBuyer { get; set; }
    public virtual RewardPointsHistory RewardPointsHistoryEntry { get; set; }
    public virtual Address ShippingAddress { get; set; }
    public virtual CommonBaseData ShippingStatusTypeNavigation { get; set; }
    public virtual Subsidy Subsidy { get; set; }
    public virtual CommonBaseData SubsidyStatusTypeNavigation { get; set; }
    public virtual CommonBaseData SubsidyTypeNavigation { get; set; }
    public virtual User User { get; set; }
public virtual ICollection<Order>
    InverseLastOrder { get; set; }
public virtual ICollection<Issue>
    Issue { get; set; }
public virtual ICollection<OrderItem>
    OrderItem { get; set; }
public virtual ICollection<OrderNote>
    OrderNote { get; set; }
public virtual ICollection<OrderTrakking>
    OrderTrakking { get; set; }
public virtual ICollection<Payment>
    Payment { get; set; }
public virtual ICollection<Refund>
    Refund { get; set; }
public virtual ICollection<WalletReservation>
    WalletReservation { get; set; }
public virtual ICollection<WalletTransaction>
    WalletTransaction { get; set; }
    }
 }
