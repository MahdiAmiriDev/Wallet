using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class RefundPaymentTransactionItemRequest  :IBaseEntity<int>,IApplicationBasedEntity
    {
public RefundPaymentTransactionItemRequest()
{
    InverseParent = new HashSet<RefundPaymentTransactionItemRequest>();
    PaymentTransactionItemRefund = new HashSet<PaymentTransactionItemRefund>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// شناسه ترمینال
///</summary>
public int YashilHekmatTerminalId { get; set; }
/// <summary>
///         /// شناسه تراکنش پدیرنده
///</summary>
public string ReservationNumber { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// نوع درخواست
///</summary>
public int RefundPaymentTransactionItemRequestType { get; set; }
/// <summary>
///         /// وضعیت درخواست
///</summary>
public int RefundPaymentTransactionItemRequestStatus { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// دلیل رد
///</summary>
public string RejectRreason { get; set; }
public string CodePath { get; set; }
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
    public virtual RefundPaymentTransactionItemRequest Parent { get; set; }
    public virtual CommonBaseData RefundPaymentTransactionItemRequestStatusNavigation { get; set; }
    public virtual CommonBaseData RefundPaymentTransactionItemRequestTypeNavigation { get; set; }
    public virtual YashilHekmatTerminal YashilHekmatTerminal { get; set; }
public virtual ICollection<RefundPaymentTransactionItemRequest>
    InverseParent { get; set; }
public virtual ICollection<PaymentTransactionItemRefund>
    PaymentTransactionItemRefund { get; set; }
    }
 }
