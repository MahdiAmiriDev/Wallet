using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class QuestionOption  :IBaseEntity<int>,IApplicationBasedEntity
    {
public QuestionOption()
{
    ExamQuestionAnswer = new HashSet<ExamQuestionAnswer>();
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
///         /// گزینه نظر سنجی
///</summary>
public int? SurveyOptionId { get; set; }
/// <summary>
///         /// متن گزینه
///</summary>
public string Description { get; set; }
/// <summary>
///         /// آیا پاسخ صحیح است
///</summary>
public bool IsCorrectAnswer { get; set; }
/// <summary>
///         /// ترتیب
///</summary>
public int DisplayOrder { get; set; }
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
///         /// امتیاز
///</summary>
public double? Score { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual Question Question { get; set; }
    public virtual SurveyOption SurveyOption { get; set; }
public virtual ICollection<ExamQuestionAnswer>
    ExamQuestionAnswer { get; set; }
    }
 }
