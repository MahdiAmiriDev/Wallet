using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CustomerClubCardPerson  :IBaseEntity<int>,IApplicationBasedEntity
    {
public CustomerClubCardPerson()
{
    CustomerClubCard = new HashSet<CustomerClubCard>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
public byte[] EncryptedName { get; set; }
public byte[] EncryptedLastName { get; set; }
/// <summary>
///         /// نام پدر
///</summary>
public string FatherName { get; set; }
/// <summary>
///         /// جنسیت
///</summary>
public int Gender { get; set; }
/// <summary>
///         /// تاریخ تولد
///</summary>
public int? BirthDate { get; set; }
/// <summary>
///         /// شماره تلفن
///</summary>
public string PhoneNumber { get; set; }
/// <summary>
///         /// مدرک تحصیلی
///</summary>
public int? EducationGrade { get; set; }
/// <summary>
///         /// رایانامه (Email)
///</summary>
public string Email { get; set; }
/// <summary>
///         /// کد ملی
///</summary>
public string NationalCode { get; set; }
/// <summary>
///         /// توضیحات
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
///         /// شماره موبایل
///</summary>
public string MobileNumber { get; set; }
/// <summary>
///         /// آدرس
///</summary>
public string Address { get; set; }
public int? TbaoId { get; set; }
public bool IsLegalPerson { get; set; }
public byte[] EncryptedNationalCode { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<CustomerClubCard>
    CustomerClubCard { get; set; }
    }
 }
