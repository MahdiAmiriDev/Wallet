using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CategoryGroup  :IBaseEntity<int>,IApplicationBasedEntity
    {
public CategoryGroup()
{
    Category = new HashSet<Category>();
    Factor = new HashSet<Factor>();
    OrderItem = new HashSet<OrderItem>();
    ProductGroup = new HashSet<ProductGroup>();
    Subsidy = new HashSet<Subsidy>();
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
public int DisplayOrder { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<Category>
    Category { get; set; }
public virtual ICollection<Factor>
    Factor { get; set; }
public virtual ICollection<OrderItem>
    OrderItem { get; set; }
public virtual ICollection<ProductGroup>
    ProductGroup { get; set; }
public virtual ICollection<Subsidy>
    Subsidy { get; set; }
    }
 }
