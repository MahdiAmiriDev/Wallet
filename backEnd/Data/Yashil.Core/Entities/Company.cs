using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Company  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Company()
{
    AdditionalUserProp = new HashSet<AdditionalUserProp>();
    Category = new HashSet<Category>();
    CompanyMainCategory = new HashSet<CompanyMainCategory>();
    CompanyVirtualClassRoomConfig = new HashSet<CompanyVirtualClassRoomConfig>();
    Factor = new HashSet<Factor>();
    InverseParent = new HashSet<Company>();
    NewsStore = new HashSet<NewsStore>();
    Product = new HashSet<Product>();
    Representation = new HashSet<Representation>();
    SaleConfiguration = new HashSet<SaleConfiguration>();
    Service = new HashSet<Service>();
    VocherType = new HashSet<VocherType>();
    WalletScope = new HashSet<WalletScope>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// فعال
///</summary>
public bool IsActive { get; set; }
/// <summary>
///         /// تایید شده
///</summary>
public bool Confirmed { get; set; }
/// <summary>
///         /// آدرس
///</summary>
public int? AddressId { get; set; }
/// <summary>
///         /// درباره
///</summary>
public string About { get; set; }
/// <summary>
///         /// نوع مجوز تاسیس
///</summary>
public int? EstablishedLicenseType { get; set; }
/// <summary>
///         /// اهداف و ماموریت
///</summary>
public string Goal { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// توانمندی ها و سوابق
///</summary>
public string Ability { get; set; }
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
public int? TbaoId { get; set; }
public int DisplayOrder { get; set; }
public bool? ShowInMainPage { get; set; }
public string ShortName { get; set; }
/// <summary>
///         /// آدرس مربوط به ارسال کد کوپن
///</summary>
public string VoucherUrlPrefix { get; set; }
/// <summary>
///         /// فرمت زیرنویس
///</summary>
public string SubTitleFormat { get; set; }
/// <summary>
///         /// شناسه گروه بندی مراکز
///</summary>
public int? CompanyGroupId { get; set; }
public string CodePath { get; set; }
/// <summary>
///         /// میتواند کاربر مراکز دیگر را ببیند؟
///</summary>
public bool CanUserSeeOtherCompany { get; set; }
public bool? ApiSupported { get; set; }
public string ApiLocalKey { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Address Address { get; set; }
    public virtual Application Application { get; set; }
    public virtual CompanyGroup CompanyGroup { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CommonBaseData EstablishedLicenseTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Company Parent { get; set; }
public virtual ICollection<AdditionalUserProp>
    AdditionalUserProp { get; set; }
public virtual ICollection<Category>
    Category { get; set; }
public virtual ICollection<CompanyMainCategory>
    CompanyMainCategory { get; set; }
public virtual ICollection<CompanyVirtualClassRoomConfig>
    CompanyVirtualClassRoomConfig { get; set; }
public virtual ICollection<Factor>
    Factor { get; set; }
public virtual ICollection<Company>
    InverseParent { get; set; }
public virtual ICollection<NewsStore>
    NewsStore { get; set; }
public virtual ICollection<Product>
    Product { get; set; }
public virtual ICollection<Representation>
    Representation { get; set; }
public virtual ICollection<SaleConfiguration>
    SaleConfiguration { get; set; }
public virtual ICollection<Service>
    Service { get; set; }
public virtual ICollection<VocherType>
    VocherType { get; set; }
public virtual ICollection<WalletScope>
    WalletScope { get; set; }
    }
 }
