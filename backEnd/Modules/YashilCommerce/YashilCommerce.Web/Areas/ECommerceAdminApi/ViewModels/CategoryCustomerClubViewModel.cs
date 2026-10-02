using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class CategoryCustomerClubListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        public string CustomerClubTitle { get; set; }

        public string CategoryTitle { get; set; }

    }


    public class CategoryCustomerClubEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)] public string Code { get; set; }
        [StringLength(600)]
        [Required] public string Title { get; set; }
        public int? CustomerClubId { get; set; }
        public int? CategoryId { get; set; }
        public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class CategoryCustomerClubSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }


    }

}
