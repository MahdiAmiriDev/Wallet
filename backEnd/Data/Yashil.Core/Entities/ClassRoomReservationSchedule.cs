using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ClassRoomReservationSchedule  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ClassRoomReservationSchedule()
{
    InverseParent = new HashSet<ClassRoomReservationSchedule>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// زمان شروع
///</summary>
public TimeOnly StartTime { get; set; }
public TimeOnly EndTime { get; set; }
/// <summary>
///         /// نوع زمان بندی کلاس
///</summary>
public int ClassRoomReservationScheduleStatusType { get; set; }
/// <summary>
///         /// روز برگذاری کلاس
///</summary>
public int DayOfWeekId { get; set; }
/// <summary>
///         /// تاریخ برگذاری کلاس
///</summary>
public DateTime? DateTime { get; set; }
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
public int ClassRoomReservationId { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual ClassRoomReservation ClassRoomReservation { get; set; }
    public virtual CommonBaseData ClassRoomReservationScheduleStatusTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual YashilDayOfWeek DayOfWeek { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ClassRoomReservationSchedule Parent { get; set; }
public virtual ICollection<ClassRoomReservationSchedule>
    InverseParent { get; set; }
    }
 }
