using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class ContractListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string ContractNumber { get; set; }
        public int ArrangementDate { get; set; }
        [StringValidation] public string ContractStatusTitle { get; set; }
        public bool Confirmed { get; set; }
        public int? TurnoverExpect { get; set; }
        [StringValidation] public string LicenseNumber { get; set; }
        [StringValidation] public string EconomicCode { get; set; }
        [StringValidation] public string FounderMobileNumber { get; set; }
        [StringValidation] public string FounderPhoneNumber { get; set; }
        public bool IsActive { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }
        public int LicenseExpireDate { get; set; }
        [StringValidation] public string RepresentationTitle { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }

        // TODO Fix This Remove From ProductSalePlanning ******
        public decimal ProfitPercentage { get; set; }
        // TODO Fix This Remove From ProductSalePlanning ******
        public decimal DiscountPercentage { get; set; }
    }


    public class ContractEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringLength(300)][StringValidation] public string Code { get; set; }

        [StringLength(600)] [StringValidation]
        [Required]
        public string Title { get; set; }

        [Range(0, int.MaxValue)]
        [Required]
        public int SaleConfigurationId { get; set; }

        [StringLength(100)]
        [Required]
        [StringValidation] public string ContractNumber { get; set; }



        [Range(0, int.MaxValue)] [Required] public int ArrangementDate { get; set; }
        [Range(0, int.MaxValue)] [Required] public int ContractType { get; set; }
        public bool IsActive { get; set; }

        [Range(0, int.MaxValue)] [Required] public int ContractStatus { get; set; }
        public bool Confirmed { get; set; }
        public int? TurnoverExpect { get; set; }

        [StringLength(400)][StringValidation] public string LicenseNumber { get; set; }


        [StringLength(100)] [StringValidation] public string EconomicCode { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int AccessLevelId { get; set; }
        public SaleConfigurationEditModel SaleConfiguration { get; set; }
    }





    public class ContractSimpleViewModel : IBaseViewModel
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