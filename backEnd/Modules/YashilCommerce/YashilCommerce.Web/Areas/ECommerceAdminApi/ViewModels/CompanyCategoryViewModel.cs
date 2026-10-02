using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class CompanyCategoryListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        [StringValidation] public string CategoryTitle { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
    }


    public class CompanyCategoryEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [Range(0, int.MaxValue)] [Required] public int CompanyMainCategory { get; set; }
        [Range(0, int.MaxValue)] [Required] public int Category { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }
    }


    public class CompanyCategorySimpleViewModel : IBaseViewModel
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