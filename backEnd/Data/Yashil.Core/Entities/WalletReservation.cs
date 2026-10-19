using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class WalletReservation  :IBaseEntity<int>,IApplicationBasedEntity
    {
public WalletReservation()
{
    InverseParent = new HashSet<WalletReservation>();
    WalletTransaction = new HashSet<WalletTransaction>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
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
public int WalletAssignmentId { get; set; }
public int UserId { get; set; }
public decimal Amount { get; set; }
public int WalletReservationStatusType { get; set; }
public string IdempotencyKey { get; set; }
public DateTime ExpiresAt { get; set; }
public DateTime? CapturedAt { get; set; }
public DateTime? ReleasedAt { get; set; }
public byte[] RowVersion { get; set; }
public int? PaymentId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual WalletReservation Parent { get; set; }
    public virtual Payment Payment { get; set; }
    public virtual User User { get; set; }
    public virtual WalletAssignment WalletAssignment { get; set; }
public virtual ICollection<WalletReservation>
    InverseParent { get; set; }
public virtual ICollection<WalletTransaction>
    WalletTransaction { get; set; }
    }
 }
