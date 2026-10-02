using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Question  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Question()
{
    ExamQuestion = new HashSet<ExamQuestion>();
    QuestionOption = new HashSet<QuestionOption>();
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
///         /// دسته بندی سوالات دوره
///</summary>
public int QuestionCategoryId { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// نوع سوال
///</summary>
public int QuestionType { get; set; }
/// <summary>
///         /// درجه سختی سوال
///</summary>
public int QuestionDifficultyDegreeType { get; set; }
/// <summary>
///         /// پاسخ تشریحی
///</summary>
public string DescriptiveAnswer { get; set; }
/// <summary>
///         /// مدت زمان به ثانیه
///</summary>
public int? Duration { get; set; }
/// <summary>
///         /// مشاهده توسط سایر اساتید
///</summary>
public bool CanOtherTeachersSee { get; set; }
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
///         /// نوع محتوای سوال
///</summary>
public int QuestionContentType { get; set; }
/// <summary>
///         /// نوع محتوای  گزینه
///</summary>
public int QuestionOptionContentType { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    public virtual QuestionCategory QuestionCategory { get; set; }
    public virtual CommonBaseData QuestionContentTypeNavigation { get; set; }
    public virtual CommonBaseData QuestionDifficultyDegreeTypeNavigation { get; set; }
    public virtual CommonBaseData QuestionOptionContentTypeNavigation { get; set; }
    public virtual CommonBaseData QuestionTypeNavigation { get; set; }
public virtual ICollection<ExamQuestion>
    ExamQuestion { get; set; }
public virtual ICollection<QuestionOption>
    QuestionOption { get; set; }
    }
 }
