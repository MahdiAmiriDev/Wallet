using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Certificate  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// تاریخ صدور
///</summary>
public string IssueDateInt { get; set; }
/// <summary>
///         /// شماره گواهینامه
///</summary>
public string Number { get; set; }
/// <summary>
///         /// شهر
///</summary>
public string CityTitle { get; set; }
/// <summary>
///         /// نام نمایندگی
///</summary>
public string RepresentationTitle { get; set; }
/// <summary>
///         /// نام استاد
///</summary>
public string TeacherName { get; set; }
/// <summary>
///         /// کد دوره
///</summary>
public string CourseCode { get; set; }
/// <summary>
///         /// نام دوره
///</summary>
public string CourseTitle { get; set; }
/// <summary>
///         /// نوع دوره
///</summary>
public string CourseType { get; set; }
/// <summary>
///         /// مدت دوره
///</summary>
public string CourseDuration { get; set; }
/// <summary>
///         /// جنسیت
///</summary>
public string SexTitle { get; set; }
/// <summary>
///         /// نام
///</summary>
public string Name { get; set; }
/// <summary>
///         /// نام خانوادگی
///</summary>
public string LastName { get; set; }
/// <summary>
///         /// نام پدر
///</summary>
public string FatherName { get; set; }
/// <summary>
///         /// نام لاتین
///</summary>
public string LatinName { get; set; }
/// <summary>
///         /// نام خانوادگی لاتین
///</summary>
public string LatinLastName { get; set; }
/// <summary>
///         /// کد ملی
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// شماره شناسنامه
///</summary>
public string IdentityNumber { get; set; }
/// <summary>
///         /// محل صدور
///</summary>
public string PlaceOfIssue { get; set; }
/// <summary>
///         /// نمره
///</summary>
public string Score { get; set; }
/// <summary>
///         /// موبایل
///</summary>
public string MobileNumber { get; set; }
/// <summary>
///         /// تلفن
///</summary>
public string Tell { get; set; }
/// <summary>
///         /// ایمیل
///</summary>
public string Email { get; set; }
/// <summary>
///         /// تاریخ تولد
///</summary>
public string DateOfBirthInt { get; set; }
/// <summary>
///         /// آخرین مدرک
///</summary>
public string LastEvidence { get; set; }
/// <summary>
///         /// رشته تحصیلی
///</summary>
public string FieldOfStudy { get; set; }
/// <summary>
///         /// هزینه دوره
///</summary>
public string CoursePrice { get; set; }
/// <summary>
///         /// موبایل استاد
///</summary>
public string TeacherMobile { get; set; }
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

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
