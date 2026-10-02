using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ActivityLog  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ActivityLog()
{
    InverseParent = new HashSet<ActivityLog>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// مجوز
///</summary>
public int ResourceAppActionId { get; set; }
/// <summary>
///         /// شناسه رکورد
///</summary>
public int ObjectId { get; set; }
/// <summary>
///         /// محتوای رکورد قبلی
///</summary>
public string PreviousRecordContent { get; set; }
/// <summary>
///         /// کاربر
///</summary>
public int UserId { get; set; }
/// <summary>
///         /// آدرس آی پی
///</summary>
public string IpAddress { get; set; }
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
    public virtual ActivityLog Parent { get; set; }
    public virtual ResourceAppAction ResourceAppAction { get; set; }
    public virtual User User { get; set; }
public virtual ICollection<ActivityLog>
    InverseParent { get; set; }
    }
 }
