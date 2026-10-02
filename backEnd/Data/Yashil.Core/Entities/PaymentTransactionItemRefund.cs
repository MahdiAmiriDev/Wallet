using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class PaymentTransactionItemRefund  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// شناسه درخواست عودت وجه
///</summary>
public int RefundPaymentTransactionItemRequestId { get; set; }
public int YashilHekmatTerminalId { get; set; }
/// <summary>
///         /// شناسه تراکنش پدیرنده
///</summary>
public int? ReservationNumber { get; set; }
/// <summary>
///         /// مبلغ تراکنش نهایی (ریالی)
///</summary>
public decimal NewPaymentAmount { get; set; }
/// <summary>
///         /// مبلغ تراکنش ارسالی(یال)
///</summary>
public decimal OldPaymentAmount { get; set; }
/// <summary>
///         /// مبلغی که باید عودت داده شده-مبلغ پرداختی به دارنده
///</summary>
public decimal? RefundAmount { get; set; }
/// <summary>
///         /// نوع عودت وجه - با توجه به نوع پرداخت-آنلاین-ناجا-حکمت
///</summary>
public int? RefundType { get; set; }
/// <summary>
///         /// وضعیت عودت وجه
///</summary>
public int? RefundStatusType { get; set; }
/// <summary>
///         /// شماره حساب شخص
///</summary>
public int? PersonBankAccountId { get; set; }
/// <summary>
///         /// تاریخ بازپرداخت
///</summary>
public DateTime? RefundTime { get; set; }
/// <summary>
///         /// تایید شده
///</summary>
public bool? Confirmed { get; set; }
public decimal? AnnouncedRefundAmount { get; set; }
public int? RefundAmountSetBy { get; set; }
public DateTime? RefundAmountSetDate { get; set; }
public int? CreationDateInt { get; set; }
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
    public virtual User ModifyByNavigation { get; set; }
    public virtual RefundPaymentTransactionItemRequest RefundPaymentTransactionItemRequest { get; set; }
    public virtual YashilHekmatTerminal YashilHekmatTerminal { get; set; }
    }
 }
