using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class YashilDayOfWeek  :IBaseEntity<int>
    {
public YashilDayOfWeek()
{
    ClassRoomReservationSchedule = new HashSet<ClassRoomReservationSchedule>();
    InverseParent = new HashSet<YashilDayOfWeek>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
public string Number { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }

    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual YashilDayOfWeek Parent { get; set; }
public virtual ICollection<ClassRoomReservationSchedule>
    ClassRoomReservationSchedule { get; set; }
public virtual ICollection<YashilDayOfWeek>
    InverseParent { get; set; }
    }
 }
