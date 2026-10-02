using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class JobCo  :IBaseEntity<int>,IApplicationBasedEntity
    {
public JobCo()
{
    CorporationJobCo = new HashSet<CorporationJobCo>();
    InverseParent = new HashSet<JobCo>();
    JobCoSkill = new HashSet<JobCoSkill>();
    JobCoTags = new HashSet<JobCoTags>();
    JobCoUser = new HashSet<JobCoUser>();
 }

public int Id { get; set; }
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
///         /// دسته بندی مشاغل
///</summary>
public int JobCoCategoryId { get; set; }
/// <summary>
///         /// شرح موقعیت شغلی
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
    public virtual JobCoCategory JobCoCategory { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual JobCo Parent { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCo { get; set; }
public virtual ICollection<JobCo>
    InverseParent { get; set; }
public virtual ICollection<JobCoSkill>
    JobCoSkill { get; set; }
public virtual ICollection<JobCoTags>
    JobCoTags { get; set; }
public virtual ICollection<JobCoUser>
    JobCoUser { get; set; }
    }
 }
