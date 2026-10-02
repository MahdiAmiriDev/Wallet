using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Corporation  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Corporation()
{
    CorporationJobCo = new HashSet<CorporationJobCo>();
    InverseParent = new HashSet<Corporation>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// زمینه فعالیت
///</summary>
public string FieldOfActivity { get; set; }
/// <summary>
///         /// سال تاسیس
///</summary>
public string EstablishedYear { get; set; }
/// <summary>
///         /// تعداد کاربران
///</summary>
public int EmployeeCount { get; set; }
/// <summary>
///         /// آیا فعال است
///</summary>
public bool IsActive { get; set; }
/// <summary>
///         /// ایمیل
///</summary>
public string Email { get; set; }
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
    public virtual Corporation Parent { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCo { get; set; }
public virtual ICollection<Corporation>
    InverseParent { get; set; }
    }
 }
