using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class OrderNoteListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string OrderTitle { get; set; }

        public int DownloadId { get; set; }

        public bool DisplayToCustomer { get; set; }

        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

    }


    public class OrderNoteEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int OrderId { get; set; }

        [Required][StringValidation] public string Note { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int DownloadId { get; set; }
        public bool DisplayToCustomer { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)]
        [Required]
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
    }





    public class OrderNoteSimpleViewModel : IBaseViewModel
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
