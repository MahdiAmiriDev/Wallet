using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class RepresentationPersonListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string PersonTitle { get; set; }

        [StringValidation] public string PostTitle { get; set; }

        public int? FromDate { get; set; }

        public int? ToDate { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        [StringValidation] public string NationalCode { get; set; }
    }


    public class RepresentationPersonEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [Range(0, int.MaxValue)] [Required] public int RepresentationId { get; set; }
        [Range(0, int.MaxValue)] [Required] public int PersonId { get; set; }
        [Range(0, int.MaxValue)] [Required] public int PostId { get; set; }
        public int? CooperationType { get; set; }
        public int? FromDate { get; set; }
        public int? ToDate { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }
    }


    public class RepresentationPersonSimpleViewModel : IBaseViewModel
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