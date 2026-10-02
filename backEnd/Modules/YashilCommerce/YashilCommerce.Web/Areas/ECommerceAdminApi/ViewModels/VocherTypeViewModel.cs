using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class VocherTypeListViewModel : IBaseViewModel
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

        public int CompanyId { get; set; }

        public int? RepresentationId { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }

        public int? TbaoId { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }

        public int VocherTypeId { get; set; }
    }


    public class VocherTypeEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)][StringValidation] public string Code { get; set; }
        [StringLength(600)] [Required] [StringValidation] public string Title { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CompanyId { get; set; }
        public int? RepresentationId { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        public int? TbaoId { get; set; }
    }





    public class VocherTypeSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

    }

    public class VocherTypeSimpleEditModel
    {
        public int Id { get; set; }
    }

    public class VocherTypeVochersListViewModel
    {
        /// <summary>
        /// شناسه ووچر
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شناسه  سفارش
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// وضعیت اختصاص ووچر
        /// </summary>
        public bool State { get; set; }
    }

}
