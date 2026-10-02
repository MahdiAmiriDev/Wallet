using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class AppEntity  :IBaseEntity<int>
    {
public AppEntity()
{
    AppEntityAttributeMapping = new HashSet<AppEntityAttributeMapping>();
    AppEntityCustomerClub = new HashSet<AppEntityCustomerClub>();
    DocumentCategory = new HashSet<DocumentCategory>();
    ItemGroup = new HashSet<ItemGroup>();
    LocaleStringResource = new HashSet<LocaleStringResource>();
    Product = new HashSet<Product>();
    Service = new HashSet<Service>();
 }

public int Id { get; set; }
/// <summary>
///         /// عنوان انگلیسی جدول
///</summary>
public string Title { get; set; }
/// <summary>
///         /// ایجاد تب برای فیلدهای توضیح
///</summary>
public bool GenerateTabForDescColumn { get; set; }
/// <summary>
///         /// دارای فایل
///</summary>
public bool HasAttachmenet { get; set; }
/// <summary>
///         /// کد سیستمی جدول
///</summary>
public int? SystemId { get; set; }
/// <summary>
///         /// عنوان ستون نمایشی
///</summary>
public string TitlePropertyName { get; set; }
/// <summary>
///         /// با تعداد رکوردهای بزرگتر از 1000
///</summary>
public bool? IsLarge { get; set; }
/// <summary>
///         /// جدول مجازی-نتیجه شکست یک جدول  واقعی
///</summary>
public bool IsVirtualEntity { get; set; }
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
/// <summary>
///         /// فیلد json ویژگی ها
///</summary>
public string Props { get; set; }
/// <summary>
///         /// عنوان فارسی جدول
///</summary>
public string PersianTitle { get; set; }
/// <summary>
///         /// عنوان انگلیسی جدول
///</summary>
public string EnglishTitle { get; set; }
/// <summary>
///         /// رکورهای جدول به تفکیک برنامه می باشد
///</summary>
public string ApplicationBased { get; set; }

public virtual ICollection<AppEntityAttributeMapping>
    AppEntityAttributeMapping { get; set; }
public virtual ICollection<AppEntityCustomerClub>
    AppEntityCustomerClub { get; set; }
public virtual ICollection<DocumentCategory>
    DocumentCategory { get; set; }
public virtual ICollection<ItemGroup>
    ItemGroup { get; set; }
public virtual ICollection<LocaleStringResource>
    LocaleStringResource { get; set; }
public virtual ICollection<Product>
    Product { get; set; }
public virtual ICollection<Service>
    Service { get; set; }
    }
 }
