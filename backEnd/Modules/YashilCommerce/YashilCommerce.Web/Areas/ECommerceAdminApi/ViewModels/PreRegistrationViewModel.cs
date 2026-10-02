using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class PreRegistrationListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        public string UserName { get; set; }
        public string ProductTitle { get; set; }
        public int ProductSalesPlanningId { get; set; }
        public DateTime CreationDate { get; set; }

    }


    public class PreRegistrationEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [Range(0, int.MaxValue)]
        [Required]
        public int? UserId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ProductSalesPlanningId { get; set; }
     
    }





    public class PreRegistrationSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        public string Title { get; set; }

    }

}
