using System.Collections.Generic;

namespace Yashil.Core.CustomDomainModels.YashilLight
{
    public class AssignableQuestionsRandomlyByQuestionCountDomainModel
    {
        public int ExamId { get; set; }
        public List<QuestionCountByTypeAndDifficultyDomainModel> SelectedData { get; set; }
    }
}
