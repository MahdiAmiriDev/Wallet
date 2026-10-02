using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class City  :IBaseEntity<int>
    {
public City()
{
    Address = new HashSet<Address>();
    CitySubsidyFactor = new HashSet<CitySubsidyFactor>();
    CorporationJobCo = new HashSet<CorporationJobCo>();
    InverseParent = new HashSet<City>();
    MerchantInfo = new HashSet<MerchantInfo>();
    Order = new HashSet<Order>();
    WalletScope = new HashSet<WalletScope>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// عرض جغرافیایی
///</summary>
public double? Latitude { get; set; }
/// <summary>
///         /// طول جغرافیایی
///</summary>
public double? Longitude { get; set; }
/// <summary>
///         /// تقسیم بندی سفارشی
///</summary>
public int? CustomCategory { get; set; }
/// <summary>
///         /// تقسیمات کشوری
///</summary>
public int CountryDivisionType { get; set; }
/// <summary>
///         /// مرکز استان
///</summary>
public bool? ProvinceCenter { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
public int? FanCityId { get; set; }
public string ShortTitle { get; set; }

    public virtual CommonBaseData CountryDivisionTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual CommonBaseData CustomCategoryNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual City Parent { get; set; }
public virtual ICollection<Address>
    Address { get; set; }
public virtual ICollection<CitySubsidyFactor>
    CitySubsidyFactor { get; set; }
public virtual ICollection<CorporationJobCo>
    CorporationJobCo { get; set; }
public virtual ICollection<City>
    InverseParent { get; set; }
public virtual ICollection<MerchantInfo>
    MerchantInfo { get; set; }
public virtual ICollection<Order>
    Order { get; set; }
public virtual ICollection<WalletScope>
    WalletScope { get; set; }
    }
 }
