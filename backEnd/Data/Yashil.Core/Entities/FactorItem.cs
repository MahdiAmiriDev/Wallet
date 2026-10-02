using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class FactorItem  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// فاکتور
///</summary>
public int FactorId { get; set; }
/// <summary>
///         /// سفارش
///</summary>
public int OrderItemId { get; set; }
/// <summary>
///         /// هش جزییات سفارش
///</summary>
public string OrderItemHash { get; set; }
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
///         /// شناسه دسته بندی اصلی
///</summary>
public int? MainCategoryId { get; set; }
/// <summary>
///         /// عنوان دسته بندی اصلی
///</summary>
public string MainCategoryTitle { get; set; }
/// <summary>
///         /// کد دسته بندی اصلی
///</summary>
public string MainCategoryCode { get; set; }
/// <summary>
///         /// گروه آیتم های سفارش حذف شده  که گروه بندی فاکتور شده است
///</summary>
public int? DeletedOrderItemGroupId { get; set; }
/// <summary>
///         /// آیا شامل بیمه است؟
///</summary>
public bool IsIncludeInsurace { get; set; }
public string MainCategoryUnitOfMeasurement { get; set; }
/// <summary>
///         /// شناسه گروه بندی محصول برای فاکتور
///</summary>
public int? ProductGroupId { get; set; }
/// <summary>
///         /// کد گروه بندی محصول برای فاکتور
///</summary>
public string ProductGroupCode { get; set; }
/// <summary>
///         /// کد اصلی گروه بندی محصول برای فاکتور
///</summary>
public string ProductGroupMainCode { get; set; }
/// <summary>
///         /// عنوان گروه بندی محصول برای فاکتور
///</summary>
public string ProductGroupTitle { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual FactorGroup DeletedOrderItemGroup { get; set; }
    public virtual Factor Factor { get; set; }
    public virtual Category MainCategory { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual OrderItem OrderItem { get; set; }
    }
 }
