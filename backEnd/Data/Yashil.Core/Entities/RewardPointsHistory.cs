using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class RewardPointsHistory  :IBaseEntity<int>,IApplicationBasedEntity
    {
public RewardPointsHistory()
{
    Order = new HashSet<Order>();
 }

public int Id { get; set; }
/// <summary>
///         /// شناسه مشتری
///</summary>
public int CustomerId { get; set; }
/// <summary>
///         /// شناسه فروشگاه
///</summary>
public int StoreId { get; set; }
/// <summary>
///         /// امتیاز اضافه شده است
///</summary>
public int Points { get; set; }
/// <summary>
///         /// تعادل امتیاز
///</summary>
public int? PointsBalance { get; set; }
/// <summary>
///         /// مقدار استفاده شده
///</summary>
public decimal UsedAmount { get; set; }
/// <summary>
///         /// پیام
///</summary>
public string Message { get; set; }
/// <summary>
///         /// تاریخ و زمانی که امتیاز دیگر معتبر نخواهد بود
///</summary>
public DateTime? EndDateUtc { get; set; }
/// <summary>
///         /// تعداد امتیازهای معتبر که هنوز هزینه نکرده اید
///</summary>
public int? ValidPoints { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<Order>
    Order { get; set; }
    }
 }
