using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Representation  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Representation()
{
    AdditionalUserProp = new HashSet<AdditionalUserProp>();
    Factor = new HashSet<Factor>();
    OrderItem = new HashSet<OrderItem>();
    ProductSalesPlanning = new HashSet<ProductSalesPlanning>();
    RepresentationCategory = new HashSet<RepresentationCategory>();
    RepresentationEstablishedLicenseType = new HashSet<RepresentationEstablishedLicenseType>();
    RepresentationPerson = new HashSet<RepresentationPerson>();
    RepresentationTeacher = new HashSet<RepresentationTeacher>();
    SaleConfiguration = new HashSet<SaleConfiguration>();
    ShoppingCartItem = new HashSet<ShoppingCartItem>();
    WalletScope = new HashSet<WalletScope>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// فعال
///</summary>
public bool IsActive { get; set; }
/// <summary>
///         /// آدرس
///</summary>
public int? AddressId { get; set; }
/// <summary>
///         /// تایید شده
///</summary>
public bool Confirmed { get; set; }
/// <summary>
///         /// مرکز
///</summary>
public int CompanyId { get; set; }
/// <summary>
///         /// نمایندگی اصلی مرکز
///</summary>
public bool IsCompanyMainRepresentation { get; set; }
/// <summary>
///         /// مؤسس
///</summary>
public int? FounderId { get; set; }
/// <summary>
///         /// شماره مجوز
///</summary>
public string LicenseNumber { get; set; }
/// <summary>
///         /// تاریخ انقاضای مجوز
///</summary>
public int LicenseExpireTime { get; set; }
/// <summary>
///         /// نوع مدرک
///</summary>
public int? LicenseType { get; set; }
/// <summary>
///         /// نوع مالکیت
///</summary>
public int? OwnershipType { get; set; }
/// <summary>
///         /// متراژ
///</summary>
public int? Area { get; set; }
/// <summary>
///         /// درباره
///</summary>
public string About { get; set; }
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
///         /// کد اقتصادی
///</summary>
public string EconomicCode { get; set; }
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
/// <summary>
///         /// ضریب سوبسید وزارت
///</summary>
public decimal VezaratSubsidyFactor { get; set; }
/// <summary>
///         /// نام اختصاری
///</summary>
public string ShortName { get; set; }
/// <summary>
///         /// ضریب سوبسید ساتا
///</summary>
public decimal SataSubsidyFactor { get; set; }
/// <summary>
///         /// نمایندگی مرکزی استان است؟
///</summary>
public bool? IsProvinceHub { get; set; }
public int DisplayOrder { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Address Address { get; set; }
    public virtual Company Company { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual Person Founder { get; set; }
    public virtual CommonBaseData LicenseTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual CommonBaseData OwnershipTypeNavigation { get; set; }
public virtual ICollection<AdditionalUserProp>
    AdditionalUserProp { get; set; }
public virtual ICollection<Factor>
    Factor { get; set; }
public virtual ICollection<OrderItem>
    OrderItem { get; set; }
public virtual ICollection<ProductSalesPlanning>
    ProductSalesPlanning { get; set; }
public virtual ICollection<RepresentationCategory>
    RepresentationCategory { get; set; }
public virtual ICollection<RepresentationEstablishedLicenseType>
    RepresentationEstablishedLicenseType { get; set; }
public virtual ICollection<RepresentationPerson>
    RepresentationPerson { get; set; }
public virtual ICollection<RepresentationTeacher>
    RepresentationTeacher { get; set; }
public virtual ICollection<SaleConfiguration>
    SaleConfiguration { get; set; }
public virtual ICollection<ShoppingCartItem>
    ShoppingCartItem { get; set; }
public virtual ICollection<WalletScope>
    WalletScope { get; set; }
    }
 }
