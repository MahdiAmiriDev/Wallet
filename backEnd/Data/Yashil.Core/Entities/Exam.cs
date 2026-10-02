using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class Exam  :IBaseEntity<int>,IApplicationBasedEntity
    {
public Exam()
{
    ExamQuestion = new HashSet<ExamQuestion>();
    InverseExamSurvey = new HashSet<Exam>();
    SurveyOption = new HashSet<SurveyOption>();
    UserExam = new HashSet<UserExam>();
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
///         /// دسته بندی آزمون های دوره
///</summary>
public int ExamCategoryId { get; set; }
/// <summary>
///         /// توضیحات
///</summary>
public string Description { get; set; }
/// <summary>
///         /// تاریخ شروع
///</summary>
public int StartDateInt { get; set; }
/// <summary>
///         /// تاریخ شروع میلادی
///</summary>
public DateTime StartDate { get; set; }
/// <summary>
///         /// تاریخ پایان میلادی
///</summary>
public DateTime EndDate { get; set; }
/// <summary>
///         /// تاریخ پایان
///</summary>
public int EndDateInt { get; set; }
/// <summary>
///         /// مدت زمان به دقیقه
///</summary>
public int Duration { get; set; }
/// <summary>
///         /// زمان شروع
///</summary>
public TimeOnly StartTime { get; set; }
/// <summary>
///         /// زمان پایان
///</summary>
public TimeOnly EndTime { get; set; }
/// <summary>
///         /// ضریب
///</summary>
public int Coefficient { get; set; }
/// <summary>
///         /// نوع آزمون
///</summary>
public int ExamType { get; set; }
/// <summary>
///         /// آیا داری سوالات ارفاقی است؟
///</summary>
public bool HasBonusQuestions { get; set; }
/// <summary>
///         /// تعداد سوالات ارفاقی
///</summary>
public int BonusQuestionsCount { get; set; }
/// <summary>
///         /// هر سوال نمره متفاوت دارد؟
///</summary>
public bool QuestionsHaveDifferentScores { get; set; }
/// <summary>
///         /// سوالات دارای نمره منفی می باشند؟
///</summary>
public bool QuestionsHaveNegativeScore { get; set; }
/// <summary>
///         /// نمایش گزینه ها با ترتیب تصادفی
///</summary>
public bool DisplayOptionsInRandomOrder { get; set; }
/// <summary>
///         /// نمایش سوالات با ترتیب تصادفی
///</summary>
public bool DisplayQuestionsInRandomOrder { get; set; }
/// <summary>
///         /// نمایش سوال بعدی بعد از پاسخ
///</summary>
public bool? DisplayNextQuestionAfterAnswer { get; set; }
/// <summary>
///         /// نمایش سوال بعدی بعد از کلیک رو نمایش سوال بعدی
///</summary>
public bool? DisplayNextQuestionAfterClickNext { get; set; }
/// <summary>
///         /// نمایش سوال بعدی بعد از اتمام زمان
///</summary>
public bool? DisplayNextQuestionAfterTimeOut { get; set; }
/// <summary>
///         /// اتمام آزمون بعد از زمان مشخص شده
///</summary>
public bool? FinishExamAfterSpecifiedTime { get; set; }
/// <summary>
///         /// نمره آزمون
///</summary>
public double? Score { get; set; }
/// <summary>
///         /// نمره قبولی
///</summary>
public double AcceptScore { get; set; }
/// <summary>
///         /// نوع نمایش نتایج
///</summary>
public int ExamDisplayResultType { get; set; }
/// <summary>
///         /// نیازمند نظرسنجی
///</summary>
public bool NeedSurvey { get; set; }
public bool SaveEveryQuestionAfterAnswer { get; set; }
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
///         /// شناسه آزمون نظر سنجی - وقتی از بانک نظر سنجی انتخاب می شود شناسه آن در این فیلد ذخیره می شود
///</summary>
public int? SurveyId { get; set; }
/// <summary>
///         /// وقتی در تعریف آزمون تیک نیاز مند نظر سنجی خورده باشد، شناسه نظر سنجی در این فیلد قرار می گیرد
///</summary>
public int? ExamSurveyId { get; set; }
/// <summary>
///         /// اختصاص آزمون به همه دانشجویان
///</summary>
public bool AssignAllStudentToExam { get; set; }
public bool? ShowExamAnswerSheet { get; set; }
/// <summary>
///         /// Indicates whether viewing the content is mandatory (1 = Yes, 0 = No)
///</summary>
public bool? ViewingTheContentIsMandatory { get; set; }

    public virtual AccessLevel AccessLevel { get; set; }
    public virtual Application Application { get; set; }
    public virtual User CreateByNavigation { get; set; }
    public virtual Organization CreatorOrganization { get; set; }
    public virtual ExamCategory ExamCategory { get; set; }
    public virtual CommonBaseData ExamDisplayResultTypeNavigation { get; set; }
    public virtual Exam ExamSurvey { get; set; }
    public virtual CommonBaseData ExamTypeNavigation { get; set; }
    public virtual User ModifyByNavigation { get; set; }
public virtual ICollection<ExamQuestion>
    ExamQuestion { get; set; }
public virtual ICollection<Exam>
    InverseExamSurvey { get; set; }
public virtual ICollection<SurveyOption>
    SurveyOption { get; set; }
public virtual ICollection<UserExam>
    UserExam { get; set; }
    }
 }
