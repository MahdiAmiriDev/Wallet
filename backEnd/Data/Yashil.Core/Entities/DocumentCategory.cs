using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class DocumentCategory  :IBaseEntity<int>
    {
public DocumentCategory()
{
    DocType = new HashSet<DocType>();
    InverseParent = new HashSet<DocumentCategory>();
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
///         /// کد پدر
///</summary>
public int? ParentId { get; set; }
/// <summary>
///         /// موجودیت
///</summary>
public int AppEntityId { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int? DisplayOrder { get; set; }
/// <summary>
///         /// توضیح کامل
///</summary>
public string Description { get; set; }
/// <summary>
///         /// فعال بودن
///</summary>
public bool IsActive { get; set; }
/// <summary>
///         /// اسناد دسته بندی شود؟
///</summary>
public bool IsCategorized { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده سند
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

    public virtual AppEntity AppEntity { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual DocumentCategory Parent { get; set; }
public virtual ICollection<DocType>
    DocType { get; set; }
public virtual ICollection<DocumentCategory>
    InverseParent { get; set; }
    }
 }
