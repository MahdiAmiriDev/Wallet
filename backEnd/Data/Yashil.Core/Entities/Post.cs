using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Post  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Post()
{
    InverseParent = new HashSet<Post>();
    PersonPost = new HashSet<PersonPost>();
    RepresentationPerson = new HashSet<RepresentationPerson>();
 }

public int Id { get; set; }
/// <summary>
///         /// کلید پست
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// شغل
///</summary>
public int? JobId { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// پست مجازی
///</summary>
public bool IsVirtual { get; set; }
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
    public virtual Job Job { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Post Parent { get; set; }
public virtual ICollection<Post>
    InverseParent { get; set; }
public virtual ICollection<PersonPost>
    PersonPost { get; set; }
public virtual ICollection<RepresentationPerson>
    RepresentationPerson { get; set; }
    }
 }
