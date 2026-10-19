using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilPayment.Web.Areas.PaymentAdminApi.ViewModels
{

    public class HekmatTerminalListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string? Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string TerminalNumber { get; set; }

        public bool? IsActive { get; set; }

        public int? MaxInstallmentCount { get; set; }

        public bool IsLearn { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

        [StringValidation] public string? Description { get; set; }
    }


    public class HekmatTerminalEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringValidation] public string? Code { get; set; }
        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }
        [StringLength(200)][StringValidation]
        [Required]
        public string TerminalNumber { get; set; }
        public bool? IsActive { get; set; }
        public int? MaxInstallmentCount { get; set; }
        public bool IsLearn { get; set; }
        [StringValidation] public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class HekmatTerminalSimpleViewModel : IBaseViewModel
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
