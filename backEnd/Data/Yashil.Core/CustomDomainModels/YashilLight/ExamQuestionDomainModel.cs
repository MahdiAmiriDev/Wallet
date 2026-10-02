using System.Collections.Generic;

namespace Yashil.Core.CustomDomainModels.YashilLight
{
    public class ExamQuestionDomainModel
    {
        /// <summary>
        /// شناسه سوال
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ذخیره سوال در سمت دیتا بیس بعد از پاسخ کاربر
        /// </summary>
        public bool SaveEveryQuestionAfterAnswer { get; set; }
        /// <summary>
        /// شناسه سوال آزمون
        /// </summary>
        public int ExamQuestionId { get; set; }
        /// <summary>
        /// شناسه پاسخ داده شده
        /// </summary>
        public int? ExamQuestionAnswerId { get; set; }
        /// <summary>
        /// امتیاز
        ///</summary>
        public double? Score { get; set; }
        /// <summary>
        /// عنوان سوال
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// شناسه نوع سوال
        /// </summary>
        public int QuestionType { get; set; }
        /// <summary>
        /// شناسه درجه سختی سوال
        /// </summary>
        public int QuestionDifficultyDegreeType { get; set; }
        /// <summary>
        /// عکس سوال
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// ویدیو سوال
        /// </summary>
        public string Video { get; set; }
        /// <summary>
        /// فایل صوتی سوال
        /// </summary>
        public string Sound { get; set; }
        /// <summary>
        /// آیا پاسخ ها سوال جند انتخابی است؟
        /// </summary>
        public bool IsMultiSelectAnswers { get; set; }
        /// <summary>
        /// پاسخ تشریحی کاربر هنگامی که در آزمون شرکت کرده باشد و آزمون هنوز به تمام نرسیده
        /// </summary>
        public string TextAnswer { get; set; }
        /// <summary>
        /// نوع محتوای سوال
        /// </summary>
        public int QuestionContentType { get; set; }
        /// <summary>
        /// گزینه های سوال
        /// </summary>
        public IEnumerable<QuestionOptionDomainModel> QuestionOptions { get; set; }
    }
    
}
