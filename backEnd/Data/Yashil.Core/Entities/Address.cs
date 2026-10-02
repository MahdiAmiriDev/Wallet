using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Address  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Address()
{
    Affiliate = new HashSet<Affiliate>();
    Company = new HashSet<Company>();
    OrderBillingAddress = new HashSet<Order>();
    OrderShippingAddress = new HashSet<Order>();
    Person = new HashSet<Person>();
    PersonEconomicProfile = new HashSet<PersonEconomicProfile>();
    Recipient = new HashSet<Recipient>();
    Representation = new HashSet<Representation>();
 }

public int Id { get; set; }
/// <summary>
///         /// آدرس
///</summary>
public string FullAddress { get; set; }
/// <summary>
///         /// شهر
///</summary>
public int? CityId { get; set; }
/// <summary>
///         /// رایانامه (Email)
///</summary>
public string Email { get; set; }
/// <summary>
///         /// شماره تلفن
///</summary>
public string Telephone { get; set; }
/// <summary>
///         /// شماره موبایل
///</summary>
public string MobileNumber { get; set; }
/// <summary>
///         /// دور نگار
///</summary>
public string FaxNumber { get; set; }
/// <summary>
///         /// کد پستی
///</summary>
public string PostalCode { get; set; }
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
public int? FanAgentId { get; set; }
public bool IsEncrypted { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual City City { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<Affiliate>
    Affiliate { get; set; }
public virtual ICollection<Company>
    Company { get; set; }
public virtual ICollection<Order>
    OrderBillingAddress { get; set; }
public virtual ICollection<Order>
    OrderShippingAddress { get; set; }
public virtual ICollection<Person>
    Person { get; set; }
public virtual ICollection<PersonEconomicProfile>
    PersonEconomicProfile { get; set; }
public virtual ICollection<Recipient>
    Recipient { get; set; }
public virtual ICollection<Representation>
    Representation { get; set; }
    }
 }
