using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class DocType  :IBaseEntity<int>
    {
public DocType()
{
    AppDocument = new HashSet<AppDocument>();
 }

/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int? DisplayOrder { get; set; }
/// <summary>
///         /// دسته بندی
///</summary>
public int DocumentCategoryId { get; set; }
/// <summary>
///         /// آیا فایل روی دیسک ذخیره شود؟
///</summary>
public bool SaveToDisk { get; set; }
/// <summary>
///         /// حداکثر اندازه
///</summary>
public int MaxSize { get; set; }
/// <summary>
///         /// حداکثر تعداد
///</summary>
public int MaxCount { get; set; }
/// <summary>
///         /// فرمت سند
///</summary>
public int DocFormatId { get; set; }
/// <summary>
///         /// آیا فایل تصویر است؟
///</summary>
public bool IsImage { get; set; }
/// <summary>
///         /// آیا تصویر کراپ شود؟
///</summary>
public bool CropImage { get; set; }
/// <summary>
///         /// نسبت تصویر
///</summary>
public double AspectRatio { get; set; }
/// <summary>
///         /// آیا عنوان اجبرای است؟
///</summary>
public bool IsTitleRequired { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده
///</summary>
public int CreatorOrganizationId { get; set; }
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
///         /// حداکثر عرض
///</summary>
public int? MaxWidth { get; set; }
/// <summary>
///         /// حداکثر ارتفاع
///</summary>
public int? MaxHeight { get; set; }
/// <summary>
///         /// کانورت فرمت عکس به Webp
///</summary>
public bool ConvertToWebp { get; set; }
public int? MinHeight { get; set; }
public int? MinWidth { get; set; }
public bool? IsMobileMode { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual DocFormat DocFormat { get; set; }
    public virtual DocumentCategory DocumentCategory { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<AppDocument>
    AppDocument { get; set; }
    }
 }
