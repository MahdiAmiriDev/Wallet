using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class TaxCategorySaleConfiguration  :IBaseEntity<int>,IApplicationBasedEntity
    {
public TaxCategorySaleConfiguration()
{
    InverseParent = new HashSet<TaxCategorySaleConfiguration>();
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
///         /// توضیحات
///</summary>
public string Description { get; set; }
public string CodePath { get; set; }
/// <summary>
///         /// دسته بندی مالیاتی
///</summary>
public int TaxCategoryId { get; set; }
/// <summary>
///         /// تنظیم فروش
///</summary>
public int SaleConfigurationId { get; set; }
/// <summary>
///         /// نوع گزارش فاکتور : با جزییات  - بدون جزییات
///</summary>
public int FactorReportType { get; set; }
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
    public virtual CommonBaseData FactorReportTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual TaxCategorySaleConfiguration Parent { get; set; }
    public virtual SaleConfiguration SaleConfiguration { get; set; }
    public virtual TaxCategory TaxCategory { get; set; }
public virtual ICollection<TaxCategorySaleConfiguration>
    InverseParent { get; set; }
    }
 }
