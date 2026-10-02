using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Menu  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Menu()
{
    InverseParent = new HashSet<Menu>();
 }

/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// مسیر
///</summary>
public string Path { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// آیکون
///</summary>
public string Icon { get; set; }
/// <summary>
///         /// کلاس
///</summary>
public string Class { get; set; }
/// <summary>
///         /// نشان
///</summary>
public string Badge { get; set; }
/// <summary>
///         /// کلاس مشان
///</summary>
public string BadgeClass { get; set; }
/// <summary>
///         /// لینک خارجی
///</summary>
public bool? IsExternalLink { get; set; }
/// <summary>
///         /// پدر
///</summary>
public int? ParentId { get; set; }
/// <summary>
///         /// منبع
///</summary>
public int? ResourceId { get; set; }
/// <summary>
///         /// مجازی
///</summary>
public bool IsVirtual { get; set; }
/// <summary>
///         /// ترتیب
///</summary>
public int? OrderNo { get; set; }
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
public int ApplicationId { get; set; }
public int CreatorOrganizationId { get; set; }
public int? AssistantId { get; set; }
public string Modulekey { get; set; }
public int? Code { get; set; }
/// <summary>
///         /// نیاز به احراز هویت مجدد
///</summary>
public bool RequireReLogin { get; set; }

    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Menu Parent { get; set; }
    public virtual Resource Resource { get; set; }
public virtual ICollection<Menu>
    InverseParent { get; set; }
    }
 }
