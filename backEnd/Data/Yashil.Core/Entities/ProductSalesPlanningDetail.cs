using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ProductSalesPlanningDetail  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ProductSalesPlanningDetail()
{
    InverseParent = new HashSet<ProductSalesPlanningDetail>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// برنامه ریزی دوره
///</summary>
public int ProductSalesPlanningId { get; set; }
/// <summary>
///         /// قیمت
///</summary>
public decimal? Price { get; set; }
/// <summary>
///         /// شارژ اضافه شده به هزینه ارسال
///</summary>
public decimal AdditionalShippingCharge { get; set; }
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
    public virtual User ModifyByNavigation { get; set; }
    public virtual ProductSalesPlanningDetail Parent { get; set; }
    public virtual ProductSalesPlanning ProductSalesPlanning { get; set; }
public virtual ICollection<ProductSalesPlanningDetail>
    InverseParent { get; set; }
    }
 }
