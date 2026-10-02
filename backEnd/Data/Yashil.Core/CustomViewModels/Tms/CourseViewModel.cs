using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.CustomViewModels.Tms
{

    public class CourseListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringValidation] public string Code { get; set; }
        [StringValidation] public string ProductTitle { get; set; }
        [StringValidation] public string SkillTypeTitle { get; set; }
        [StringValidation] public string CertificateTypeTitle { get; set; }

        [StringValidation] public string EvaluationMethodTitle { get; set; }

        public int Duration { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }
        [StringValidation] public string CategoryTitle { get; set; }
    }


    public class CourseEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ProductId { get; set; }
        public int? SkillType { get; set; }
        public int? CertificateType { get; set; }
        public int? EvaluationMethod { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int Duration { get; set; }
    }

    public class CourseSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
    }

}