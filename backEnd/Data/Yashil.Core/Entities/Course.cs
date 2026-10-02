using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Course  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// سرفصل دوره
///</summary>
public string Topic { get; set; }
/// <summary>
///         /// پیش نیاز
///</summary>
public string Prerequisite { get; set; }
/// <summary>
///         /// اهداف دوره
///</summary>
public string Target { get; set; }
/// <summary>
///         /// الزامات دوره
///</summary>
public string Requirements { get; set; }
/// <summary>
///         /// مهارت در انتهای دوره
///</summary>
public string Skill { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// سطح مهارت دوره
///</summary>
public int? SkillType { get; set; }
/// <summary>
///         /// نوع گواهي نامه
///</summary>
public int? CertificateType { get; set; }
/// <summary>
///         /// روش ارزیابی دوره
///</summary>
public int? EvaluationMethod { get; set; }
/// <summary>
///         /// مخاطب دوره
///</summary>
public string Audience { get; set; }
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
///         /// ساعت عملی
///</summary>
public int? PracticalHours { get; set; }
/// <summary>
///         /// ساعت نظری
///</summary>
public int? TheoreticalHours { get; set; }
public string IncomingEducationLevel { get; set; }
/// <summary>
///         /// شناسه
///</summary>
public int? MappingId { get; set; }
public string UniqId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CommonBaseData CertificateTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData EvaluationMethodNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData SkillTypeNavigation { get; set; }
    public virtual Product Product { get; set; }
    }
 }
