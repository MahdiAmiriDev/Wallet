using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class WalletProgram  :IBaseEntity<int>,IApplicationBasedEntity
    {
public WalletProgram()
{
    InverseParent = new HashSet<WalletProgram>();
    WalletAssignment = new HashSet<WalletAssignment>();
    WalletCardGroup = new HashSet<WalletCardGroup>();
    WalletCouponBatch = new HashSet<WalletCouponBatch>();
    WalletScope = new HashSet<WalletScope>();
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
public bool? IsActive { get; set; }
public int WalletProgramType { get; set; }
public decimal TotalAmount { get; set; }
public decimal AllocatedAmount { get; set; }
public decimal ConsumedAmount { get; set; }
public decimal ReservedAmount { get; set; }
public decimal ExpiredAmount { get; set; }
public string CurrencyCode { get; set; }
public int WalletProgramAllocationMethodType { get; set; }
public DateTime StartDate { get; set; }
public DateTime? EndDate { get; set; }
public int WalletProgramStatusType { get; set; }
public byte[] RowVersion { get; set; }
public int? TargetUserCount { get; set; }
public decimal? AmountPerUser { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual WalletProgram Parent { get; set; }
    public virtual CommonBaseData WalletProgramAllocationMethodTypeNavigation { get; set; }
    public virtual CommonBaseData WalletProgramStatusTypeNavigation { get; set; }
    public virtual CommonBaseData WalletProgramTypeNavigation { get; set; }
public virtual ICollection<WalletProgram>
    InverseParent { get; set; }
public virtual ICollection<WalletAssignment>
    WalletAssignment { get; set; }
public virtual ICollection<WalletCardGroup>
    WalletCardGroup { get; set; }
public virtual ICollection<WalletCouponBatch>
    WalletCouponBatch { get; set; }
public virtual ICollection<WalletScope>
    WalletScope { get; set; }
    }
 }
