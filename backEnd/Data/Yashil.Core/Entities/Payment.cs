using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Payment  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Payment()
{
    CustomerClubPaymentInstallment = new HashSet<CustomerClubPaymentInstallment>();
    Issue = new HashSet<Issue>();
    OrderItemPayment = new HashSet<OrderItemPayment>();
    RefundDetail = new HashSet<RefundDetail>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// سفارش
///</summary>
public int OrderId { get; set; }
/// <summary>
///         /// تاریخ پرداخت به شمسی
///</summary>
public int? PaidDateInt { get; set; }
/// <summary>
///         /// نوع کارت
///</summary>
public int? CardType { get; set; }
/// <summary>
///         /// نام کارت
///</summary>
public string CardName { get; set; }
/// <summary>
///         /// شماره کارت - CardHashPan
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// شماره کارت اعتباری - CardMaskPan
///</summary>
public string MaskedCreditCardNumber { get; set; }
/// <summary>
///         /// CVV2 رمز شده
///</summary>
public string CardCvv2 { get; set; }
/// <summary>
///         /// ماه انقضا کارت رمز شده
///</summary>
public string CardExpirationMonth { get; set; }
/// <summary>
///         /// سال انقضا کارت رمز شده
///</summary>
public string CardExpirationYear { get; set; }
/// <summary>
///         /// کد سفارش به ازای هر پرداخت
///</summary>
public string ReservationNumber { get; set; }
/// <summary>
///         /// شماره پیگیری تراکنش- RefNum
///</summary>
public string AuthorizationTransactionId { get; set; }
/// <summary>
///         /// کد رهگیری پذیرنده-ردیف گردش ترمینال-hekmatId-کد سمت حکمت
///</summary>
public string InstallmentTrackingCode { get; set; }
/// <summary>
///         /// کد دارنده
///</summary>
public string MerchantId { get; set; }
/// <summary>
///         /// شماره ترمینال درگاه پرداخت
///</summary>
public string TerminalNumber { get; set; }
/// <summary>
///         /// کد معامله - token جهت خرید
///</summary>
public string AuthorizationTransactionCode { get; set; }
/// <summary>
///         /// نتیجه معامله-پیام
///</summary>
public string AuthorizationTransactionResult { get; set; }
/// <summary>
///         /// تاریخ و زمان پرداخت شده
///</summary>
public DateTime? PaidDateUtc { get; set; }
/// <summary>
///         /// نقدی یا اقساطی-نوع پرداخت
///</summary>
public int SaleType { get; set; }
/// <summary>
///         /// کد خطا
///</summary>
public int? ErrorCode { get; set; }
/// <summary>
///         /// شماره پیگیری مشتری
///</summary>
public string TraceNumber { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// شناسه وضعیت پرداخت
///</summary>
public int PaymentStatusType { get; set; }
/// <summary>
///         /// نام سیستم روش پرداخت- حکمت-ثمین و شتاب-
///</summary>
public int? PaymentMethodType { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int? InstallmentCount { get; set; }
/// <summary>
///         /// مجموع سفارش
///</summary>
public decimal Total { get; set; }
/// <summary>
///         /// گروه پرداخت
///</summary>
public int? PaymentGroupId { get; set; }
public int YearMonth { get; set; }
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
///         /// زمان تغییر
///</summary>
public DateTime? ModificationDate { get; set; }
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
public string CustomerRefNum { get; set; }
/// <summary>
///         /// شناسه خرید دارنده کارت
///</summary>
public string GoodReferenceId { get; set; }
/// <summary>
///         /// شناسه خرید فروشنده
///</summary>
public string MerchantData { get; set; }
public long? TotalInt { get; set; }
/// <summary>
///         /// مقدار عددی تاریخ شمسی پرداخت
///</summary>
public int? PaidDateUtcInt { get; set; }
/// <summary>
///         /// توکن
///</summary>
public string Token { get; set; }
public bool IsHybrid { get; set; }
public int? HybridPaymentRequestId { get; set; }
public int? HybridPaymentId { get; set; }
public string RetrievalReferenceNumber { get; set; }
public string SystemTraceAuditNumber { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CommonBaseData CardTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Order Order { get; set; }
    public virtual ItemGroup PaymentGroup { get; set; }
    public virtual CommonBaseData PaymentMethodTypeNavigation { get; set; }
    public virtual CommonBaseData PaymentStatusTypeNavigation { get; set; }
    public virtual CommonBaseData SaleTypeNavigation { get; set; }
    public virtual PersianDateMonth YearMonthNavigation { get; set; }
public virtual ICollection<CustomerClubPaymentInstallment>
    CustomerClubPaymentInstallment { get; set; }
public virtual ICollection<Issue>
    Issue { get; set; }
public virtual ICollection<OrderItemPayment>
    OrderItemPayment { get; set; }
public virtual ICollection<RefundDetail>
    RefundDetail { get; set; }
    }
 }
