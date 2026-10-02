using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class PersianCalendar  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string ShamsiDate { get; set; }
/// <summary>
///         /// تاریخ شمسی
///</summary>
public string DayOfWeek { get; set; }
/// <summary>
///         /// روز هفته
///</summary>
public string ShamsiDateInt { get; set; }
/// <summary>
///         /// تاریخ شمسی به Int
///</summary>
public string GregorianDate { get; set; }
/// <summary>
///         /// تاریخ میلادی
///</summary>
public string Code { get; set; }
/// <summary>
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }
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
public int? DayNumber { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
