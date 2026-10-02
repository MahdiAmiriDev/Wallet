using System.Collections.Generic;

namespace Yashil.Core.CustomDomainModels.YashilLight
{
    /// <summary>
    /// مدل مربوط به پاسخ کاربر به سوال های یک آزمون
    /// </summary>
    public class ExamAnswerSheetDomainModel
    {
        public int QuestionContentType { get; set; }
        
        public bool IsSingleSelect { get; set; }
        public int Id { get; set; }

        public int ExamQuestionId { get; set; }

        public string QuestionTitle { get; set; }

        public string CorrectDescriptiveAnswer { get; set; }

        public bool IsMultipleChoice { get; set; }

        public List<QuestionOptionModel> QuestionOptions { get; set; }

        public QuestionOptionAnswerSheetModel QuestionOptionAnswerSheetModel { get; set; }

        public DescriptiveQuestionAnswerSheetDomainModel DescriptiveQuestionAnswerSheet { get; set; }

    }

    /// <summary>
    /// مدل اطلاهات سوال های چند گزینه ای آزمون
    /// </summary>
    public class QuestionOptionModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public int QuestionOptionContentType { get; set; }
    }


    /// <summary>
    /// مدل پاسخ کاربر در سوال های چندگزینه ای
    /// </summary>
    public class QuestionOptionAnswerSheetModel
    {
        public int ExamQuestionAnswerId { get; set; }

        public int ExamQuestionId { get; set; }

        public int? SelectedUserQuestionOptionId { get; set; }
    }

    /// <summary>
    /// مدل پاسخ کاربر در سوال های تشریحی
    /// </summary>
    public class DescriptiveQuestionAnswerSheetDomainModel
    {
        public int ExamQuestionAnswerId { get; set; }

        public int ExamQuestionId { get; set; }

        public string UserDescriptiveAnswer { get; set; }
    }
}
