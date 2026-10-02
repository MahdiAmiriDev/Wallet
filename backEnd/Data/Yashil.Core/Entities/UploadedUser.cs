using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UploadedUser  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// شناسه سند
///</summary>
public int AppDocumentId { get; set; }
/// <summary>
///         /// شماره کاربری
///</summary>
public string UserNumber { get; set; }
/// <summary>
///         /// نام کاربری
///</summary>
public string UserName { get; set; }
/// <summary>
///         /// نام کاربری نامعتبر می باشد
///</summary>
public bool? UserNameIsInvalid { get; set; }
/// <summary>
///         /// کلمه عبور
///</summary>
public string Password { get; set; }
/// <summary>
///         /// کلمه عبور هش شده
///</summary>
public byte[] PasswordHash { get; set; }
/// <summary>
///         /// کلمه عبور هش شده
///</summary>
public byte[] PasswordSalt { get; set; }
/// <summary>
///         /// کلمه عبور نامعتبر می باشد
///</summary>
public bool? PasswordIsInvalid { get; set; }
/// <summary>
///         /// نام
///</summary>
public string Name { get; set; }
/// <summary>
///         /// نام خانوادگی
///</summary>
public string LastName { get; set; }
/// <summary>
///         /// جنسیت
///</summary>
public string Gender { get; set; }
/// <summary>
///         /// مقدار نوع جنسیت
///</summary>
public int? GenderType { get; set; }
/// <summary>
///         /// نام پدر
///</summary>
public string FatherName { get; set; }
/// <summary>
///         /// شماره شناسنامه
///</summary>
public string IdentityNumber { get; set; }
/// <summary>
///         /// کد ملی
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// کد ملی نا معتبر می باشد
///</summary>
public bool? NationalCodeIsInvalid { get; set; }
/// <summary>
///         /// تاریخ تولد
///</summary>
public string BirthDate { get; set; }
/// <summary>
///         /// رایانامه (Email)
///</summary>
public string Email { get; set; }
/// <summary>
///         /// نام شهر
///</summary>
public string CityTitle { get; set; }
/// <summary>
///         /// آدرس
///</summary>
public string Address { get; set; }
/// <summary>
///         /// کد پستی
///</summary>
public string PostalCode { get; set; }
/// <summary>
///         /// شماره تلفن
///</summary>
public string PhoneNumber { get; set; }
/// <summary>
///         /// شماره موبایل
///</summary>
public string MobileNumber { get; set; }
/// <summary>
///         /// پیام خطا
///</summary>
public string ErrorMessage { get; set; }
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
public int? UserGroupId { get; set; }
/// <summary>
///         /// نام مرکز
///</summary>
public string CompanyTitle { get; set; }
/// <summary>
///         /// شناسه مرکز
///</summary>
public int? CompanyId { get; set; }
/// <summary>
///         /// عنوان مرکز معتبر است؟
///</summary>
public bool? CompanyTitleIsInvalid { get; set; }
/// <summary>
///         /// اگر مرکز کاربر تغییر کرده باشد برابر 1 می شود
///</summary>
public bool? UserCompanyChanged { get; set; }
public int? RepresentationId { get; set; }
public int? CategoryId { get; set; }
public string MainCompanyTitle { get; set; }
public int? MaximumUploadCapacity { get; set; }

    public virtual AppDocument AppDocument { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
