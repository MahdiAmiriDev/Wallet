using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class DocFormat  :IBaseEntity<int>
    {
public DocFormat()
{
    DocType = new HashSet<DocType>();
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
///         /// فرمت های قابل قبول
///</summary>
public string Extensions { get; set; }
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
public bool AllowEdit { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<DocType>
    DocType { get; set; }
    }
 }
