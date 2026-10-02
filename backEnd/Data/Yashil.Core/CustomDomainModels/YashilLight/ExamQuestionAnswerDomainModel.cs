
namespace Yashil.Core.CustomDomainModels.YashilLight
{
    public class ExamQuestionAnswerDomainModel
    {
        public int Id => ExamQuestionId;
        public int ExamQuestionId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionType { get; set; }
        public string ExamQuestionTitle { get; set; }
        public int? ExamQuestionAnswerId { get; set; }
        public string DescriptiveAnswer { get; set; }
        public double? UserScore { get; set; }
        public double? ExamQuestionScore { get; set; }
        /// <summary>
        /// پاسخ صحیح
        /// </summary>
        public string CorrectAnswer { get; set; }
        /// <summary>
        /// نمره کابر به درصد
        /// </summary>
        public double? ScoreInPercentage { get; set; }
        /// <summary>
        /// آیا فایل آپلود شده است یا نه
        /// </summary>
        public bool? FileIsUploaded { get; set; }
    }
}
