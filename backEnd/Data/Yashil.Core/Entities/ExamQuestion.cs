using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ExamQuestion  :IBaseEntity<int>,IApplicationBasedEntity
    {
public ExamQuestion()
{
    ExamQuestionAnswer = new HashSet<ExamQuestionAnswer>();
    InverseNextExamQuestion = new HashSet<ExamQuestion>();
 }

public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// سوال
///</summary>
public int QuestionId { get; set; }
/// <summary>
///         /// سوال بعدی
///</summary>
public int? NextExamQuestionId { get; set; }
/// <summary>
///         /// آزمون
///</summary>
public int ExamId { get; set; }
/// <summary>
///         /// مدت زمان به دقیقه
///</summary>
public int? Duration { get; set; }
/// <summary>
///         /// ترتیب نمایش
///</summary>
public int DisplayOrder { get; set; }
/// <summary>
///         /// نمره منفی
///</summary>
public double? NegativeScore { get; set; }
/// <summary>
///         /// امتیاز
///</summary>
public double? Score { get; set; }
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
    public virtual User ModifyByNavigation { get; set; }
    public virtual ExamQuestion NextExamQuestion { get; set; }
    public virtual Question Question { get; set; }
public virtual ICollection<ExamQuestionAnswer>
    ExamQuestionAnswer { get; set; }
public virtual ICollection<ExamQuestion>
    InverseNextExamQuestion { get; set; }
    }
 }
