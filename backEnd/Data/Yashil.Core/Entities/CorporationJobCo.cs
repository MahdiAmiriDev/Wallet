using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CorporationJobCo  :IBaseEntity<int>,IApplicationBasedEntity
    {
public CorporationJobCo()
{
    InverseParent = new HashSet<CorporationJobCo>();
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
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// شغل
///</summary>
public int JobCoId { get; set; }
/// <summary>
///         /// شرکت
///</summary>
public int CorporationId { get; set; }
/// <summary>
///         /// تاریخ شروع
///</summary>
public DateTime StartDate { get; set; }
/// <summary>
///         /// تاریخ پایان
///</summary>
public DateTime EndDate { get; set; }
/// <summary>
///         /// آیا فعال است یا نه؟
///</summary>
public bool IsActive { get; set; }
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
/// <summary>
///         /// جنسیت
///</summary>
public int? CustomGenderType { get; set; }
/// <summary>
///         /// نوع همکاری
///</summary>
public int? CooperationType { get; set; }
/// <summary>
///         /// نوع حقوق
///</summary>
public int? SalaryType { get; set; }
/// <summary>
///         /// موقعیت مکانی
///</summary>
public int? CityId { get; set; }
/// <summary>
///         /// حداقل سابقه کار
///</summary>
public int? MinimumWorkExperience { get; set; }
/// <summary>
///         /// حداقل مدرک تحصیلی
///</summary>
public int? EducationGradeType { get; set; }
/// <summary>
///         /// وضعیت نظام وظیفه
///</summary>
public int? MilitaryServiceStatusType { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual City City { get; set; }
    public virtual CommonBaseData CooperationTypeNavigation { get; set; }
    public virtual Corporation Corporation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData CustomGenderTypeNavigation { get; set; }
    public virtual CommonBaseData EducationGradeTypeNavigation { get; set; }
    public virtual JobCo JobCo { get; set; }
    public virtual CommonBaseData MilitaryServiceStatusTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CorporationJobCo Parent { get; set; }
    public virtual CommonBaseData SalaryTypeNavigation { get; set; }
public virtual ICollection<CorporationJobCo>
    InverseParent { get; set; }
public virtual ICollection<JobCoUser>
    JobCoUser { get; set; }
    }
 }
