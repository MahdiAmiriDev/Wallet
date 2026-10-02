using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class CitySubsidyFactorListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string CityTitle { get; set; }

        public decimal VezaratSubsidyFactor { get; set; }

        public decimal SataSubsidyFactor { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


    public class CitySubsidyFactorEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CityId { get; set; }

        [Required] public decimal VezaratSubsidyFactor { get; set; }

        [Required] public decimal SataSubsidyFactor { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class CitySubsidyFactorSimpleViewModel : IBaseViewModel
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
