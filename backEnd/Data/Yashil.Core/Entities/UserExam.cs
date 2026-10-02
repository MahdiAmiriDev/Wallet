using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UserExam  :IBaseEntity<int>,IApplicationBasedEntity
    {
public UserExam()
{
    ExamQuestionAnswer = new HashSet<ExamQuestionAnswer>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// آزمون
///</summary>
public int ExamId { get; set; }
/// <summary>
///         /// کاربر
///</summary>
public int ProductSalesPlanningStudentId { get; set; }
/// <summary>
///         /// نمره کاربر
///</summary>
public double? UserScore { get; set; }
/// <summary>
///         /// وضعیت آزمون
///</summary>
public int ExamStatusType { get; set; }
/// <summary>
///         /// تاریخ و ساعتی که کاربردر آزمون شرکت کرده است
///</summary>
public DateTime? StudentStartDate { get; set; }
/// <summary>
///         /// تاریخ و ساعتی که کاربر دکمه پایان آزمون را میزند
///</summary>
public DateTime? StudentEndDate { get; set; }
/// <summary>
///         /// زمانی که کاربر دکمه اتمام آزمون را زده است
///</summary>
public DateTime? StudentFinishedExamDate { get; set; }
/// <summary>
///         /// نتیجه آزمون
///</summary>
public int? ExamResultType { get; set; }
/// <summary>
///         /// زمان باقیمانده آزمون کاربر
///</summary>
public int ExamRemainingTime { get; set; }
/// <summary>
///         /// نظر استاد
///</summary>
public string TeacherComment { get; set; }
/// <summary>
///         /// تاریخ تصحیح
///</summary>
public DateTime? CorrectionDate { get; set; }
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
    public virtual Exam Exam { get; set; }
    public virtual CommonBaseData ExamResultTypeNavigation { get; set; }
    public virtual CommonBaseData ExamStatusTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual ProductSalesPlanningStudent ProductSalesPlanningStudent { get; set; }
public virtual ICollection<ExamQuestionAnswer>
    ExamQuestionAnswer { get; set; }
    }
 }
