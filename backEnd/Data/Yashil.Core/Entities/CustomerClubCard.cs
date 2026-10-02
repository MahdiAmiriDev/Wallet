using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CustomerClubCard  :IBaseEntity<int>
    {
public CustomerClubCard()
{
    Order = new HashSet<Order>();
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
///         /// شخص
///</summary>
public int? CustomerClubCardPersonId { get; set; }
/// <summary>
///         /// باشگاه
///</summary>
public int CustomerClubId { get; set; }
/// <summary>
///         /// شماره کارت
///</summary>
public string CardNumber { get; set; }
/// <summary>
///         /// سقف مبلغ خرید به ریال
///</summary>
public decimal? MaximumPurchaseAmount { get; set; }
/// <summary>
///         /// سقف تعداد خرید
///</summary>
public int? MaximumNumberPurchases { get; set; }
/// <summary>
///         /// جمع مبلغ استفاده شده به ریال
///</summary>
public decimal? AmountUsed { get; set; }
/// <summary>
///         /// تاریخ پایان
///</summary>
public int ToDate { get; set; }
/// <summary>
///         /// تاریخ شروع
///</summary>
public int FromDate { get; set; }
/// <summary>
///         /// فعال
///</summary>
public bool IsActive { get; set; }
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
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual CustomerClub CustomerClub { get; set; }
    public virtual CustomerClubCardPerson CustomerClubCardPerson { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<Order>
    Order { get; set; }
    }
 }
