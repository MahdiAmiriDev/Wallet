using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UserHomeWork  :IBaseEntity<int>,IApplicationBasedEntity
    {
public UserHomeWork()
{
    InverseParent = new HashSet<UserHomeWork>();
 }

public int Id { get; set; }
public int? ParentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// شناسه تکلیف
///</summary>
public int HomeWorkId { get; set; }
/// <summary>
///         /// کاربر فراگیره تکلیف
///</summary>
public int ProductSalesPlanningStudentId { get; set; }
/// <summary>
///         /// وضعیت تکلیف
///</summary>
public int HomeWorkStatusType { get; set; }
/// <summary>
///         /// پاسخ کاربر
///</summary>
public string Answer { get; set; }
/// <summary>
///         /// زمان ثبت پاسخ
///</summary>
public DateTime? AnswerDate { get; set; }
/// <summary>
///         /// نمره کاربر
///</summary>
public double? UserScore { get; set; }
/// <summary>
///         /// تاریخ تصحیح
///</summary>
public DateTime? CorrectionDate { get; set; }
/// <summary>
///         /// نظر استاد
///</summary>
public string TeacherComment { get; set; }
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
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual HomeWork HomeWork { get; set; }
    public virtual CommonBaseData HomeWorkStatusTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual UserHomeWork Parent { get; set; }
    public virtual ProductSalesPlanningStudent ProductSalesPlanningStudent { get; set; }
public virtual ICollection<UserHomeWork>
    InverseParent { get; set; }
    }
 }
