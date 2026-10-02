using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class FactorVerificationConfigurationRoleListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string FactorVerificationConfigurationTitle { get; set; }

        [StringValidation] public string RoleTitle { get; set; }

        public int VerificationOrder { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


    public class FactorVerificationConfigurationRoleEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int FactorVerificationConfigurationId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int RoleId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int VerificationOrder { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class FactorVerificationConfigurationRoleSimpleViewModel : IBaseViewModel
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
