using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class VirtualClass  :IBaseEntity<int>,IApplicationBasedEntity
    {
public VirtualClass()
{
    RollCall = new HashSet<RollCall>();
    VirtualClassRecordingDetail = new HashSet<VirtualClassRecordingDetail>();
    VirtualClassStudent = new HashSet<VirtualClassStudent>();
    VirtualClassTeacher = new HashSet<VirtualClassTeacher>();
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
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// سازمان ایجاد کننده رکورد
///</summary>
public int CreatorOrganizationId { get; set; }
/// <summary>
///         /// زمان شروع
///</summary>
public TimeOnly StartTime { get; set; }
/// <summary>
///         /// زمان پایان
///</summary>
public TimeOnly EndTime { get; set; }
/// <summary>
///         /// برنامه ریزی دوره
///</summary>
public int ProductSalesPlanningId { get; set; }
/// <summary>
///         /// زمان شروع کلاس
///</summary>
public int OpenningDate { get; set; }
/// <summary>
///         /// زمان اضافی
///</summary>
public int? ExtraTime { get; set; }
public string AttendeePassword { get; set; }
public string ModeratorPassword { get; set; }
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
public string WelcomeMessage { get; set; }
/// <summary>
///         /// شناسه تنظیمات کلاس مجازی
///</summary>
public int? VirtualClassRoomConfigId { get; set; }
public DateTime StartDateTime { get; set; }
public DateTime EndDateTime { get; set; }
public int CompanyVirtualClassRoomConfigId { get; set; }
public string RoomId { get; set; }
public bool? AssignAllStudentsToVirtualClass { get; set; }
public bool Record { get; set; }
public int VirtualClassType { get; set; }
public string YearMonth { get; set; }
public int? Duration { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual CompanyVirtualClassRoomConfig CompanyVirtualClassRoomConfig { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ProductSalesPlanning ProductSalesPlanning { get; set; }
    public virtual VirtualClassRoomConfig VirtualClassRoomConfig { get; set; }
    public virtual CommonBaseData VirtualClassTypeNavigation { get; set; }
public virtual ICollection<RollCall>
    RollCall { get; set; }
public virtual ICollection<VirtualClassRecordingDetail>
    VirtualClassRecordingDetail { get; set; }
public virtual ICollection<VirtualClassStudent>
    VirtualClassStudent { get; set; }
public virtual ICollection<VirtualClassTeacher>
    VirtualClassTeacher { get; set; }
    }
 }
