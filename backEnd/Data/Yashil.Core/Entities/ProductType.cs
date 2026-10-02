using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ProductType  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ProductType()
{
    Category = new HashSet<Category>();
    InverseParent = new HashSet<ProductType>();
    ProductTypeSpecification = new HashSet<ProductTypeSpecification>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// معاف از مالیات
///</summary>
public bool IsTaxExempt { get; set; }
/// <summary>
///         /// دسته مالیات
///</summary>
public int TaxCategoryId { get; set; }
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
    public virtual User ModifyByNavigation { get; set; }
    public virtual ProductType Parent { get; set; }
    public virtual TaxCategory TaxCategory { get; set; }
public virtual ICollection<Category>
    Category { get; set; }
public virtual ICollection<ProductType>
    InverseParent { get; set; }
public virtual ICollection<ProductTypeSpecification>
    ProductTypeSpecification { get; set; }
    }
 }
