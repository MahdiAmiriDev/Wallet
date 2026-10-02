using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class VocherType  :IBaseEntity<int>,IApplicationBasedEntity
    {
public VocherType()
{
    Product = new HashSet<Product>();
    UploadedProductItem = new HashSet<UploadedProductItem>();
    Vocher = new HashSet<Vocher>();
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
///         /// شناسه مرکز
///</summary>
public int CompanyId { get; set; }
/// <summary>
///         /// شناسه نمایندگی
///</summary>
public int? RepresentationId { get; set; }
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
/// <summary>
///         /// شناسه نوع کوپن دیتا بیس sabtenam
///</summary>
public int? TbaoId { get; set; }
public int VoucherTemplateId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual Company Company { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<Product>
    Product { get; set; }
public virtual ICollection<UploadedProductItem>
    UploadedProductItem { get; set; }
public virtual ICollection<Vocher>
    Vocher { get; set; }
    }
 }
