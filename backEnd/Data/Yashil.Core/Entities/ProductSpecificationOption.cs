using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ProductSpecificationOption  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// محصول
///</summary>
public int ProductId { get; set; }
/// <summary>
///         /// ويژگي مشخصات
///</summary>
public int SpecificationOptionId { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int DisplayOrder { get; set; }
/// <summary>
///         /// نوع ویژگی
///</summary>
public int AttributeType { get; set; }
/// <summary>
///         /// مقدار متنی
///</summary>
public string CustomValue { get; set; }
/// <summary>
///         /// اجازه فیلتر
///</summary>
public bool AllowFiltering { get; set; }
/// <summary>
///         /// نمایش در جزییات محصول
///</summary>
public bool ShowOnProductPage { get; set; }
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
    public virtual CommonBaseData AttributeTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Product Product { get; set; }
    public virtual SpecificationOption SpecificationOption { get; set; }
    }
 }
