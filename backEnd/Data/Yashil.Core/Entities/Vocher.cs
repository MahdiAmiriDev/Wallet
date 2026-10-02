using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Vocher  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Vocher()
{
    OrderItem = new HashSet<OrderItem>();
 }

public int Id { get; set; }
/// <summary>
///         /// کوپن رمز شده
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان
///</summary>
public string Title { get; set; }
/// <summary>
///         /// شناسه نوع کوپن
///</summary>
public int VocherTypeId { get; set; }
/// <summary>
///         /// شناسه آیتم سفارش - استفاده کننده
///</summary>
public int? OrderItemId { get; set; }
/// <summary>
///         /// تاریخ استفاده
///</summary>
public DateTime? DateOfUse { get; set; }
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
    public virtual OrderItem OrderItemNavigation { get; set; }
    public virtual VocherType VocherType { get; set; }
public virtual ICollection<OrderItem>
    OrderItem { get; set; }
    }
 }
