using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ReportStore  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ReportStore()
{
    ReportConnectionString = new HashSet<ReportConnectionString>();
    ReportGroupReport = new HashSet<ReportGroupReport>();
    ReportStoreApplication = new HashSet<ReportStoreApplication>();
    RoleReport = new HashSet<RoleReport>();
    UserReport = new HashSet<UserReport>();
 }

/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// گزارش
///</summary>
public byte[] ReportFile { get; set; }
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
///         /// توضیح
///</summary>
public string Description { get; set; }
/// <summary>
///         /// کلید گزارش
///</summary>
public string ReportKey { get; set; }
/// <summary>
///         /// طراحی
///</summary>
public string DesignString { get; set; }
/// <summary>
///         /// ماژول
///</summary>
public string ModuleKey { get; set; }
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
public virtual ICollection<ReportConnectionString>
    ReportConnectionString { get; set; }
public virtual ICollection<ReportGroupReport>
    ReportGroupReport { get; set; }
public virtual ICollection<ReportStoreApplication>
    ReportStoreApplication { get; set; }
public virtual ICollection<RoleReport>
    RoleReport { get; set; }
public virtual ICollection<UserReport>
    UserReport { get; set; }
    }
 }
