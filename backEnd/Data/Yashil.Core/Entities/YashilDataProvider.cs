using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class YashilDataProvider  :IBaseEntity<int>
    {
public YashilDataProvider()
{
    YashilConnectionString = new HashSet<YashilConnectionString>();
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
///         /// نوع پایه
///</summary>
public string BaseType { get; set; }
/// <summary>
///         /// توضیح
///</summary>
public string Description { get; set; }
/// <summary>
///         /// فعال بودن
///</summary>
public bool IsActive { get; set; }
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
public virtual ICollection<YashilConnectionString>
    YashilConnectionString { get; set; }
    }
 }
