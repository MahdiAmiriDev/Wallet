using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ReportGroup  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ReportGroup()
{
    ReportGroupReport = new HashSet<ReportGroupReport>();
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
///         /// عنوان انگلیسی
///</summary>
public string EnglishTitle { get; set; }
/// <summary>
///         /// توضیح
///</summary>
public string Description { get; set; }
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

    public virtual Application Application { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
public virtual ICollection<ReportGroupReport>
    ReportGroupReport { get; set; }
    }
 }
