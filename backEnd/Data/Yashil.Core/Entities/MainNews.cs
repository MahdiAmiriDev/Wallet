using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class MainNews  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// خبر
///</summary>
public int NewsStoreId { get; set; }
/// <summary>
///         /// خبر عادی
///</summary>
public bool Simplenews { get; set; }
/// <summary>
///         /// نمایش در اسلایدر صفحه اصلی
///</summary>
public bool ShowInMainPageSlider { get; set; }
/// <summary>
///         /// خبر داغ
///</summary>
public bool IsHotNews { get; set; }
/// <summary>
///         /// خبر منتخب
///</summary>
public bool SelectedNews { get; set; }
/// <summary>
///         /// خبر اصلی سرویس خبری
///</summary>
public bool ServiceMainNews { get; set; }
/// <summary>
///         /// نمایش در قسمت چند رسانه ای
///</summary>
public bool ShowAsMultimedia { get; set; }
/// <summary>
///         /// خبر مهم
///</summary>
public bool ShowAsImportantNews { get; set; }
/// <summary>
///         /// انتخاب سردبیر
///</summary>
public bool EditorSelected { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int DisplayOrder { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }

    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual NewsStore NewsStore { get; set; }
    }
 }
