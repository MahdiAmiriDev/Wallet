using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class DashboardStore  :IBaseEntity<int>,IApplicationBasedEntity
    {
public DashboardStore()
{
    DashboardConnectionString = new HashSet<DashboardConnectionString>();
    DashboardGroupDashboard = new HashSet<DashboardGroupDashboard>();
    RoleDashboard = new HashSet<RoleDashboard>();
    UserDashboard = new HashSet<UserDashboard>();
 }

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
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// داشبورد
///</summary>
public byte[] DashboardFile { get; set; }
/// <summary>
///         /// کلاس
///</summary>
public string CssClass { get; set; }
/// <summary>
///         /// تصویر
///</summary>
public byte[] Picture { get; set; }
/// <summary>
///         /// رنگ
///</summary>
public string Color { get; set; }
/// <summary>
///         /// انیمیشن
///</summary>
public bool? Animation { get; set; }
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
public int CreatorOrganizationId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<DashboardConnectionString>
    DashboardConnectionString { get; set; }
public virtual ICollection<DashboardGroupDashboard>
    DashboardGroupDashboard { get; set; }
public virtual ICollection<RoleDashboard>
    RoleDashboard { get; set; }
public virtual ICollection<UserDashboard>
    UserDashboard { get; set; }
    }
 }
