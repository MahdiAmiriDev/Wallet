using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ClassRoomReservation  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ClassRoomReservation()
{
    ClassRoomReservationSchedule = new HashSet<ClassRoomReservationSchedule>();
    InverseParent = new HashSet<ClassRoomReservation>();
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
/// <summary>
///         /// شناسه کلاس فیزیکی
///</summary>
public int ClassRoomId { get; set; }
/// <summary>
///         /// شناسه برنامه ریزی دوره
///</summary>
public int? ProductSalesPlanningId { get; set; }
/// <summary>
///         /// وضعیت رزرو کلاس فیزیکی
///</summary>
public int ClassRoomReservationStatusType { get; set; }
/// <summary>
///         /// نوع درخواست برای رزرو کلاس:
        /// فقط رزرو می باشد یعنی بدون  برنامه ریزی دوره
        /// یا
        /// دارای برنامه ریزی دوره یعنی حتما برنامه ریزی دوره ای برای این کلاس وجود دارد
///</summary>
public int ClassRoomReservationRequestType { get; set; }
/// <summary>
///         /// تاریخ شروع به شمسی
///</summary>
public DateOnly StartDate { get; set; }
/// <summary>
///         /// تاریخ شروع به شمسی
///</summary>
public int StartDateInt { get; set; }
/// <summary>
///         /// تاریخ پایان به شمسی
///</summary>
public DateOnly EndDate { get; set; }
/// <summary>
///         /// تاریخ پایان به شمسی
///</summary>
public int EndDateInt { get; set; }
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

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual ClassRoom ClassRoom { get; set; }
    public virtual CommonBaseData ClassRoomReservationRequestTypeNavigation { get; set; }
    public virtual CommonBaseData ClassRoomReservationStatusTypeNavigation { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ClassRoomReservation Parent { get; set; }
    public virtual ProductSalesPlanning ProductSalesPlanning { get; set; }
public virtual ICollection<ClassRoomReservationSchedule>
    ClassRoomReservationSchedule { get; set; }
public virtual ICollection<ClassRoomReservation>
    InverseParent { get; set; }
    }
 }
