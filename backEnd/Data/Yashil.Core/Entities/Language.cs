using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Language  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Language()
{
    LocaleStringResource = new HashSet<LocaleStringResource>();
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
///         /// فرهنگ زبان
///</summary>
public string Culture { get; set; }
/// <summary>
///         /// کد سئو منحصر به فرد
///</summary>
public string UniqueSeoCode { get; set; }
/// <summary>
///         /// نام تصویر پرچم
///</summary>
public string FlagImageFileName { get; set; }
/// <summary>
///         /// آیا زاست چین می باشد؟
///</summary>
public bool Rtl { get; set; }
/// <summary>
///         /// محدود به فروشگاه
///</summary>
public bool LimitedToStores { get; set; }
/// <summary>
///         /// شناسه ارز پیش فرض
///</summary>
public int DefaultCurrencyId { get; set; }
/// <summary>
///         /// منتشر شود
///</summary>
public bool Published { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int DisplayOrder { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual Currency DefaultCurrency { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<LocaleStringResource>
    LocaleStringResource { get; set; }
    }
 }
