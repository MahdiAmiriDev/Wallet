using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Service  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Service()
{
    InverseParent = new HashSet<Service>();
    NewsStore = new HashSet<NewsStore>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public int? ParentId { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// کد موجودیت
///</summary>
public int? AppEntityId { get; set; }
/// <summary>
///         /// کد رکورد
///</summary>
public long? ObjectId { get; set; }
/// <summary>
///         /// سرویس های اصلی
///</summary>
public bool IsMain { get; set; }
/// <summary>
///         /// عنوان انگلیسی
///</summary>
public string EnglishTitle { get; set; }
/// <summary>
///         /// نمایش سرویس در صفحه اصلی
///</summary>
public bool ShowInMainPage { get; set; }
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
public int? CompanyId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual AppEntity AppEntity { get; set; }
    public virtual Application Application { get; set; }
    public virtual Company Company { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Service Parent { get; set; }
public virtual ICollection<Service>
    InverseParent { get; set; }
public virtual ICollection<NewsStore>
    NewsStore { get; set; }
    }
 }
