using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class SiteVisit  :IBaseEntity<int>,IApplicationBasedEntity
    {
public SiteVisit()
{
    InverseParent = new HashSet<SiteVisit>();
 }

public int Id { get; set; }
/// <summary>
///         /// آدرس صفحه ی که کاربر از آن بازدید کرده است
///</summary>
public string Url { get; set; }
/// <summary>
///         /// تاریخ
///</summary>
public int DateInt { get; set; }
/// <summary>
///         /// تعداد بازدید
///</summary>
public int VisitCount { get; set; }
/// <summary>
///         /// دستگاه بازدید کننده
///</summary>
public string Device { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual SiteVisit Parent { get; set; }
public virtual ICollection<SiteVisit>
    InverseParent { get; set; }
    }
 }
