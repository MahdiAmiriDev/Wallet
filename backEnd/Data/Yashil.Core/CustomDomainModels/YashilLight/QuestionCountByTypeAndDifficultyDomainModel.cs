
namespace Yashil.Core.CustomDomainModels.YashilLight
{
    public class QuestionCountByTypeAndDifficultyDomainModel
    {
        /// <summary>
        /// تعداد سوالات
        /// </summary>
        public int QuestionCount { get; set; }
        /// <summary>
        //  نوع سوال
        /// </summary>
        public int QuestionType { get; set; }
        /// <summary>
        /// عنوان نوع سوال
        /// </summary>
        public string QuestionTypeTitle { get; set; }
        /// <summary>
        /// درجه سختی سوال
        /// </summary>
        public int QuestionDifficultyDegreeType { get; set; }
        /// <summary>
        /// عنوان درجه سختی سوال
        /// </summary>
        public string QuestionDifficultyDegreeTypeTitle { get; set; }
        /// <summary>
        /// تعداد جواب ها
        /// </summary>
        public int QuestionOptionCount { get; set; }
        /// <summary>
        /// تعداد سوالات انتخاب شده
        /// </summary>
        public int SelectedQuestionCount { get; set; }
    }
}
