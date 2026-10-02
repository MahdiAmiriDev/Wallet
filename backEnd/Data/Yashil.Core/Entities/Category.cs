using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Category  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Category()
{
    CategoryCustomerClub = new HashSet<CategoryCustomerClub>();
    CategorySpecification = new HashSet<CategorySpecification>();
    CompanyMainCategory = new HashSet<CompanyMainCategory>();
    FactorItem = new HashSet<FactorItem>();
    InverseParent = new HashSet<Category>();
    Product = new HashSet<Product>();
    ProductCategory = new HashSet<ProductCategory>();
    RepresentationCategory = new HashSet<RepresentationCategory>();
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
///         /// مرکز
///</summary>
public int? CompanyId { get; set; }
/// <summary>
///         /// ویژگی نوع محصول
///</summary>
public int? ProductTypeId { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int DisplayOrder { get; set; }
/// <summary>
///         /// انتشار داده شده
///</summary>
public bool Published { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
public string CodePath { get; set; }
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
public int? TbaoId { get; set; }
public string CssClass { get; set; }
public int? FanLevelId { get; set; }
public int? FanFieldId { get; set; }
/// <summary>
///         /// ضریب سوبسید
///</summary>
public decimal VezaratSubsidyFactor { get; set; }
/// <summary>
///         /// ضریب سوبسید
///</summary>
public decimal SataSubsidyFactor { get; set; }
/// <summary>
///         /// شامل بیمه است
///</summary>
public bool IsIncludeInsurance { get; set; }
/// <summary>
///         /// واحد سنجش
///</summary>
public string UnitOfMeasurement { get; set; }
/// <summary>
///         /// گروه دسته بندی
///</summary>
public int CategoryGroupId { get; set; }
/// <summary>
///         /// جزییات تبلیغات سریع محصول های ابن دسته بندی
///</summary>
public string ProductQuickPromoDetail { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CategoryGroup CategoryGroup { get; set; }
    public virtual Company Company { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Category Parent { get; set; }
    public virtual ProductType ProductType { get; set; }
public virtual ICollection<CategoryCustomerClub>
    CategoryCustomerClub { get; set; }
public virtual ICollection<CategorySpecification>
    CategorySpecification { get; set; }
public virtual ICollection<CompanyMainCategory>
    CompanyMainCategory { get; set; }
public virtual ICollection<FactorItem>
    FactorItem { get; set; }
public virtual ICollection<Category>
    InverseParent { get; set; }
public virtual ICollection<Product>
    Product { get; set; }
public virtual ICollection<ProductCategory>
    ProductCategory { get; set; }
public virtual ICollection<RepresentationCategory>
    RepresentationCategory { get; set; }
    }
 }
