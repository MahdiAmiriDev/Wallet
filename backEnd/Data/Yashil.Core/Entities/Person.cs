using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Person  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Person()
{
    CardInfo = new HashSet<CardInfo>();
    Loan = new HashSet<Loan>();
    LoanGuarantor = new HashSet<LoanGuarantor>();
    PersonBankAccount = new HashSet<PersonBankAccount>();
    PersonEconomicProfile = new HashSet<PersonEconomicProfile>();
    PersonPost = new HashSet<PersonPost>();
    ProductSalesPlanningTeacher = new HashSet<ProductSalesPlanningTeacher>();
    Representation = new HashSet<Representation>();
    RepresentationPerson = new HashSet<RepresentationPerson>();
    RepresentationTeacher = new HashSet<RepresentationTeacher>();
    User = new HashSet<User>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
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
public int? TbaoId { get; set; }
/// <summary>
///         /// شناسه آدرس
///</summary>
public int? AddressId { get; set; }
/// <summary>
///         /// شماره شناسنامه
///</summary>
public int? IdentityNumber { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Address Address { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData EducationGradeNavigation { get; set; }
    public virtual CommonBaseData GenderNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<CardInfo>
    CardInfo { get; set; }
public virtual ICollection<Loan>
    Loan { get; set; }
public virtual ICollection<LoanGuarantor>
    LoanGuarantor { get; set; }
public virtual ICollection<PersonBankAccount>
    PersonBankAccount { get; set; }
public virtual ICollection<PersonEconomicProfile>
    PersonEconomicProfile { get; set; }
public virtual ICollection<PersonPost>
    PersonPost { get; set; }
public virtual ICollection<ProductSalesPlanningTeacher>
    ProductSalesPlanningTeacher { get; set; }
public virtual ICollection<Representation>
    Representation { get; set; }
public virtual ICollection<RepresentationPerson>
    RepresentationPerson { get; set; }
public virtual ICollection<RepresentationTeacher>
    RepresentationTeacher { get; set; }
public virtual ICollection<User>
    User { get; set; }
    }
 }
