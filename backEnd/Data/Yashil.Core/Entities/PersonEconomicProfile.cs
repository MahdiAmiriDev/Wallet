using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class PersonEconomicProfile  :IBaseEntity<int>,IApplicationBasedEntity
    {
public PersonEconomicProfile()
{
    InverseParent = new HashSet<PersonEconomicProfile>();
    SaleConfiguration = new HashSet<SaleConfiguration>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// شخص
///</summary>
public int PersonId { get; set; }
/// <summary>
///         /// نام
///</summary>
public string Name { get; set; }
/// <summary>
///         /// کد اقتصادی
///</summary>
public string EconomicCode { get; set; }
/// <summary>
///         /// شناسه آدرس
///</summary>
public int? AddressId { get; set; }
public bool IsLegalPerson { get; set; }
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
    public virtual Address Address { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual PersonEconomicProfile Parent { get; set; }
    public virtual Person Person { get; set; }
public virtual ICollection<PersonEconomicProfile>
    InverseParent { get; set; }
public virtual ICollection<SaleConfiguration>
    SaleConfiguration { get; set; }
    }
 }
