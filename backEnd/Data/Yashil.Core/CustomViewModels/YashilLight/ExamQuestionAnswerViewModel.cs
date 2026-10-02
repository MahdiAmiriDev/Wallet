using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yashil.Core.CustomViewModels.YashilLight
{
    public class ExamQuestionAnswerBatchEditModel
    {
        public int UserExamId { get; set; }
        public int ExamId { get; set; }

        public List<ExamQuestionAnswerTeacherCorrectionEditModel> ExamQuestionAnswers { get; set; }
    }

    /// <summary>
    /// در فرم تصحیح آزمون توسط استاد مورد استفاده قرار میگیرد
    /// </summary>
    public class ExamQuestionAnswerTeacherCorrectionEditModel
    {
        public int? ExamQuestionAnswerId { get; set; }
        public int ExamQuestionId { get; set; }
        [Range(0, double.MaxValue)]
        public double? UserScore { get; set; }

        /// <summary>
        /// نمره کابر به درصد
        /// </summary>
        public double? ScoreInPercentage { get; set; }

    }
}
