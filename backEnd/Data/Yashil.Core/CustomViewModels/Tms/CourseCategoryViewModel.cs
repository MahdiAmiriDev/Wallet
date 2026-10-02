using Yashil.Common.Core.Attributes;

namespace Yashil.Core.CustomViewModels.Tms
{
    public class CourseCategoryViewModel
    {
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string ProductTitle { get; set; }

        [StringValidation] public string SkillTypeTitle { get; set; }

        [StringValidation] public string CertificateTypeTitle { get; set; }

        [StringValidation] public string EvaluationMethodTitle { get; set; }

        public int Duration { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }
        [StringValidation] public string CategoryTitle { get; set; }
    }
}
