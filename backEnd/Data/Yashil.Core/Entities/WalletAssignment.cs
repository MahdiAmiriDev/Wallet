using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class WalletAssignment  :IBaseEntity<int>,IApplicationBasedEntity
    {
public WalletAssignment()
{
    InverseParent = new HashSet<WalletAssignment>();
    WalletReservation = new HashSet<WalletReservation>();
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
public int WalletProgramId { get; set; }
public int UserId { get; set; }
public decimal AssignedAmount { get; set; }
public decimal ConsumedAmount { get; set; }
public decimal ReservedAmount { get; set; }
public decimal ExpiredAmount { get; set; }
public decimal? AvailableAmount { get; set; }
public DateTime StartDate { get; set; }
public DateTime? EndDate { get; set; }
public int WalletAssignmentStatusType { get; set; }
public int WalletAssignmentSourceType { get; set; }
public byte[] RowVersion { get; set; }

    public virtual WalletAssignment Parent { get; set; }
    public virtual User User { get; set; }
    public virtual WalletProgram WalletProgram { get; set; }
public virtual ICollection<WalletAssignment>
    InverseParent { get; set; }
public virtual ICollection<WalletReservation>
    WalletReservation { get; set; }
public virtual ICollection<WalletTransaction>
    WalletTransaction { get; set; }
    }
 }
