using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class YashilHekmatTerminalSupplier  :IBaseEntity<int>,IApplicationBasedEntity
    {
public YashilHekmatTerminalSupplier()
{
    InverseParent = new HashSet<YashilHekmatTerminalSupplier>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// ترمینال
///</summary>
public int YashilHekmatTerminalId { get; set; }
/// <summary>
///         /// کاربر
///</summary>
public int UserId { get; set; }
/// <summary>
///         /// تامین کننده محصولات ترمینال
///</summary>
public int SupplierId { get; set; }
/// <summary>
///         /// تامین کننده محصولات ترمینال
///</summary>
public string SupplierTitle { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual YashilHekmatTerminalSupplier Parent { get; set; }
    public virtual User User { get; set; }
public virtual ICollection<YashilHekmatTerminalSupplier>
    InverseParent { get; set; }
    }
 }
