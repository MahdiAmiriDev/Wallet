using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class UploadedQuestion  :IBaseEntity<int>
    {
public int Id { get; set; }
/// <summary>
///         /// شناسه سند
///</summary>
public int AppDocumentId { get; set; }
/// <summary>
///         /// کد
///</summary>
public string Code { get; set; }
/// <summary>
///         /// عنوان سوال
///</summary>
public string Title { get; set; }
/// <summary>
///         /// عنوان سوال نامعتبر است؟
///</summary>
public bool? TitleIsInValid { get; set; }
/// <summary>
///         /// دسته بندی سوال
///</summary>
public string QuestionCategoryTitle { get; set; }
/// <summary>
///         /// شناسه دسته بندی سوال
///</summary>
public int? QuestionCategoryId { get; set; }
/// <summary>
///         /// دسته بندی سوال نامعتبر است
///</summary>
public bool? QuestionCategoryTitleIsInValid { get; set; }
/// <summary>
///         /// درجه سختی سوال
///</summary>
public string QuestionDifficultyDegreeTypeTitle { get; set; }
/// <summary>
///         /// شناسه درجه سختی سوال
///</summary>
public int? QuestionDifficultyDegreeType { get; set; }
/// <summary>
///         /// درجه سختی سوال نامعتبر است
///</summary>
public bool? QuestionDifficultyDegreeTypeTitleIsInValid { get; set; }
/// <summary>
///         /// نوع سوال
///</summary>
public string QuestionTypeTitle { get; set; }
/// <summary>
///         /// شناسه نوع سوال
///</summary>
public int? QuestionType { get; set; }
/// <summary>
///         /// نوع سوال نامعتبر است؟
///</summary>
public bool? QuestionTypeIsInValid { get; set; }
/// <summary>
///         /// پاسخ تشریحی سوال
///</summary>
public string DescriptiveAnswer { get; set; }
/// <summary>
///         /// پاسخ تشریحی سوال نامعتبر است؟
///</summary>
public bool? DescriptiveAnswerIsInValid { get; set; }
/// <summary>
///         /// گزینه اول
///</summary>
public string QuestionOptionTitle1 { get; set; }
/// <summary>
///         /// گزینه اول نامعتبر است؟
///</summary>
public bool? QuestionOptionTitle1IsInValid { get; set; }
/// <summary>
///         /// گزینه دوم
///</summary>
public string QuestionOptionTitle2 { get; set; }
/// <summary>
///         /// گزینه دوم نامعتبر است؟
///</summary>
public bool? QuestionOptionTitle2IsInValid { get; set; }
/// <summary>
///         /// گزینه سوم
///</summary>
public string QuestionOptionTitle3 { get; set; }
/// <summary>
///         /// گزینه سوم نامعتبر است؟
///</summary>
public bool? QuestionOptionTitle3IsInValid { get; set; }
/// <summary>
///         /// گزینه چهارم
///</summary>
public string QuestionOptionTitle4 { get; set; }
/// <summary>
///         /// گزینه چهارم نامعتبر است؟
///</summary>
public bool? QuestionOptionTitle4IsInValid { get; set; }
/// <summary>
///         /// گزینه صحیح
///</summary>
public string CorrectOption { get; set; }
/// <summary>
///         /// گزینه صحیح نامعتبر است
///</summary>
public bool? CorrectOptionIsInValid { get; set; }
/// <summary>
///         /// علت نامعتبر بودن
///</summary>
public string ErrorMessage { get; set; }
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
///         /// حذف شده
///</summary>
public bool Deleted { get; set; }

    public virtual AppDocument AppDocument { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
    }
 }
