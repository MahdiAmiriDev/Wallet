using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class MediaCategory  :IBaseEntity<int>,IApplicationBasedEntity
    {
public MediaCategory()
{
    InverseParent = new HashSet<MediaCategory>();
    MediaSession = new HashSet<MediaSession>();
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
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// برنامه ریزی فروش
///</summary>
public int? ProductSalesPlanningId { get; set; }
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
///         /// پدر
///</summary>
public int? ParentId { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int DisplayOrder { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual MediaCategory Parent { get; set; }
    public virtual ProductSalesPlanning ProductSalesPlanning { get; set; }
public virtual ICollection<MediaCategory>
    InverseParent { get; set; }
public virtual ICollection<MediaSession>
    MediaSession { get; set; }
    }
 }
