using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class YashilConnectionString  :IBaseEntity<int>,IApplicationBasedEntity
    {
public YashilConnectionString()
{
    DashboardConnectionString = new HashSet<DashboardConnectionString>();
    ReportConnectionString = new HashSet<ReportConnectionString>();
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
///         /// تامین کننده داده
///</summary>
public int DataProviderId { get; set; }
/// <summary>
///         /// رشته اتصال
///</summary>
public string ConnectionString { get; set; }
/// <summary>
///         /// توضیح
///</summary>
public string Description { get; set; }
/// <summary>
///         /// فعال بودن
///</summary>
public bool IsActive { get; set; }
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
/// <summary>
///         /// برنامه
///</summary>
public int ApplicationId { get; set; }
/// <summary>
///         /// سطح دسترسی
///</summary>
public int AccessLevelId { get; set; }
public int CreatorOrganizationId { get; set; }
public string ConnectionStringJson { get; set; }

    public virtual Application Application { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual YashilDataProvider DataProvider { get; set; }
public virtual ICollection<DashboardConnectionString>
    DashboardConnectionString { get; set; }
public virtual ICollection<ReportConnectionString>
    ReportConnectionString { get; set; }
    }
 }
