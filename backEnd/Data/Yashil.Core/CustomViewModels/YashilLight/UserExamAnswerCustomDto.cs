using System.Collections.Generic;

namespace Yashil.Core.CustomViewModels.YashilLight
{
    public class UserExamAnswerDto
    {
        /// <summary>
        /// کد آزمون
        /// </summary>
        public string ExamCode { get; set; }
        /// <summary>
        /// لیست پاسخ های کاربر
        /// </summary>
        public List<UserExamAnswerCustomDto> Answers { get; set; }
    }

    public class UserExamAnswerCustomDto
    {
        /// <summary>
        /// شناسه سوال مربوط به آزمون
        /// </summary>
        public int ExamQuestionId { get; set; }
        /// <summary>
        /// شناسه گزینه سوال انتخابی کاربر
        /// </summary>
        public int? QuestionOptionId { get; set; }
        /// <summary>
        /// پاسخ تشریحی کاربر
        /// </summary>
        public string DescriptiveAnswer { get; set; }
        /// <summary>
        /// آیا پاسخ داده شده است یا خیر؟
        /// اگر پاسخ داده شده باشد true
        /// و اگر از پاسخ ادن منصرف شده باشد false میگیرد
        /// </summary>
        public bool Answered { get; set; }
        /// <summary>
        /// نوع سئوال
        /// </summary>
        public int QuestionType { get; set; }
    }
}
