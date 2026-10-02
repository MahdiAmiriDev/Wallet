using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ExamQuestionAnswer  :IBaseEntity<int>,IApplicationBasedEntity
    {
public int Id { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// کاربر
///</summary>
public int UserExamId { get; set; }
/// <summary>
///         /// سوال
///</summary>
public int ExamQuestionId { get; set; }
/// <summary>
///         /// گزینه انتخابی
///</summary>
public int? QuestionOptionId { get; set; }
/// <summary>
///         /// پاسخ سوال تشریحی
///</summary>
public string DescriptiveAnswer { get; set; }
/// <summary>
///         /// پاسخ تشریحی درست است یا نه
///</summary>
public bool? DescriptiveAnswerIsCorrect { get; set; }
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
/// <summary>
///         /// نمره کسب شده توسط دانشجو
///</summary>
public double? Score { get; set; }
public double? ScoreInPercentage { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual ExamQuestion ExamQuestion { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual QuestionOption QuestionOption { get; set; }
    public virtual UserExam UserExam { get; set; }
    }
 }
