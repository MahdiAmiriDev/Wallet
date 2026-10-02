using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ProductSalesPlanningItemGroup  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ProductSalesPlanningItemGroup()
{
    InverseParent = new HashSet<ProductSalesPlanningItemGroup>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
public int ItemGroupId { get; set; }
public int ProductSalesPlanningId { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual ItemGroup ItemGroup { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ProductSalesPlanningItemGroup Parent { get; set; }
    public virtual ProductSalesPlanning ProductSalesPlanning { get; set; }
public virtual ICollection<ProductSalesPlanningItemGroup>
    InverseParent { get; set; }
    }
 }
