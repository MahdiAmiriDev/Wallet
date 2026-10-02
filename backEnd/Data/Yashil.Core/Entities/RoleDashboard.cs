using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class RoleDashboard  :IBaseEntity<int>,IApplicationBasedEntity
    {
/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// نقش
///</summary>
public int RoleId { get; set; }
/// <summary>
///         /// داشبورد
///</summary>
public int DashboardId { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
public int CreatorOrganizationId { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual DashboardStore Dashboard { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Role Role { get; set; }
    }
 }
