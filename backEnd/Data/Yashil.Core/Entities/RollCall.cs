using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class RollCall  :IBaseEntity<int>,IApplicationBasedEntity
    {
public RollCall()
{
    InverseParent = new HashSet<RollCall>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// کلاس مجازی
///</summary>
public int? VirtualClassId { get; set; }
/// <summary>
///         /// دانشجو
///</summary>
public int? ProductSalesPlanningStudentId { get; set; }
/// <summary>
///         /// جلسه
///</summary>
public int? ClassSessionId { get; set; }
/// <summary>
///         /// حاضر
///</summary>
public bool IsPresence { get; set; }
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
public int? ProductSalesPlanningTeacherId { get; set; }
public int? Duration { get; set; }
public int? TalkTime { get; set; }
public int? Chats { get; set; }
public int? PollVotes { get; set; }
public bool? FileShared { get; set; }
public int? RaiseHand { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual ClassSession ClassSession { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual RollCall Parent { get; set; }
    public virtual ProductSalesPlanningStudent ProductSalesPlanningStudent { get; set; }
    public virtual ProductSalesPlanningTeacher ProductSalesPlanningTeacher { get; set; }
    public virtual VirtualClass VirtualClass { get; set; }
public virtual ICollection<RollCall>
    InverseParent { get; set; }
    }
 }
