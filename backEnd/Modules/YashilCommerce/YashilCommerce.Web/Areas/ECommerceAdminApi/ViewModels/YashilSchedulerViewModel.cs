using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class YashilSchedulerListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string OccuranceTypeTitle { get; set; }

        public int StartDate { get; set; }

        public int? EndDate { get; set; }

        public int FromEra { get; set; }

        public int ToEra { get; set; }

        public int RepeatNumber { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
    }


    public class YashilSchedulerEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)] [StringValidation] public string Code { get; set; }
        [StringLength(600)] [StringValidation] 
        [Required]
        public string Title { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)] 
        [Required] 
        public int OccuranceType { get; set; }

        [Required]
        public int StartDate { get; set; }
        public int? EndDate { get; set; }
        [Range(0, int.MaxValue)] 
        [Required] 
        public int FromEra { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int ToEra { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int RepeatNumber { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int AccessLevelId { get; set; }
    }


    public class YashilSchedulerSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }
        [StringValidation] public string OccuranceTypeTitle { get; set; }

        public int StartDate { get; set; }

        public int? EndDate { get; set; }

        public int FromEra { get; set; }

        public int ToEra { get; set; }

        public int RepeatNumber { get; set; }
    }
}