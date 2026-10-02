using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Resource  :IBaseEntity<int>
    {
public Resource()
{
    Menu = new HashSet<Menu>();
    ResourceAppAction = new HashSet<ResourceAppAction>();
    ResourceAttribute = new HashSet<ResourceAttribute>();
 }

/// <summary>
///         /// کد
///</summary>
public int Id { get; set; }
/// <summary>
///         /// آدرس منبع
///</summary>
public string Title { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// نوع
///</summary>
public int? Type { get; set; }
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

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<Menu>
    Menu { get; set; }
public virtual ICollection<ResourceAppAction>
    ResourceAppAction { get; set; }
public virtual ICollection<ResourceAttribute>
    ResourceAttribute { get; set; }
    }
 }
