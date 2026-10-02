using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HybridPayment  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// قیمت
///</summary>
public long Price { get; set; }
/// <summary>
///         /// کد لیست درخواست  پرداخت های ترکیبی
///</summary>
public int HybridPaymentRequestId { get; set; }
/// <summary>
///         /// تاریخ پرداخت به شمسی
///</summary>
public int? PaidDateInt { get; set; }
/// <summary>
///         /// شماره پیگیری تراکنش- RefNum
///</summary>
public string AuthorizationTransactionId { get; set; }
/// <summary>
///         /// کد رهگیری پذیرنده-ردیف گردش ترمینال-hekmatId-کد سمت حکمت
///</summary>
public string InstallmentTrackingCode { get; set; }
/// <summary>
///         /// کد سفارش به ازای هر پرداخت
///</summary>
public string ReservationNumber { get; set; }
/// <summary>
///         /// شماره پیگیری مشتری
///</summary>
public string TraceNumber { get; set; }
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
///         /// مقدار عددی تاریخ شمسی پرداخت
///</summary>
public int? PaidDateUtcInt { get; set; }
/// <summary>
///         /// نقدی یا اقساطی-نوع پرداخت
///</summary>
public int SaleType { get; set; }
/// <summary>
///         /// کد خطا
///</summary>
public int? ErrorCode { get; set; }
/// <summary>
///         /// تعداد اقساط
///</summary>
public int? InstallmentCount { get; set; }
/// <summary>
///         /// شناسه وضعیت پرداخت
///</summary>
public int HybridPaymentStatusType { get; set; }
/// <summary>
///         /// نام سیستم روش پرداخت- حکمت-ثمین و شتاب-
///</summary>
public int? PaymentMethodType { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual HybridPaymentRequest HybridPaymentRequest { get; set; }
    public virtual CommonBaseData HybridPaymentStatusTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData PaymentMethodTypeNavigation { get; set; }
    public virtual CommonBaseData SaleTypeNavigation { get; set; }
    }
 }
