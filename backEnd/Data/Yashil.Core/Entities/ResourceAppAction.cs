using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ResourceAppAction  :IBaseEntity<int>
    {
public ResourceAppAction()
{
    ActivityLog = new HashSet<ActivityLog>();
    RoleResourceAction = new HashSet<RoleResourceAction>();
 }

public int Id { get; set; }
/// <summary>
///         /// عملیات
///</summary>
public int AppActionId { get; set; }
/// <summary>
///         /// منبع
///</summary>
public int ResourceId { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }

    public virtual AppAction AppAction { get; set; }
    public virtual Resource Resource { get; set; }
public virtual ICollection<ActivityLog>
    ActivityLog { get; set; }
public virtual ICollection<RoleResourceAction>
    RoleResourceAction { get; set; }
    }
 }
