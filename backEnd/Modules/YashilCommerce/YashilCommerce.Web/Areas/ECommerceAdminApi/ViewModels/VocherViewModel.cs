using System;
using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class VocherListViewModel : IBaseViewModel
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

        [StringValidation] public string VocherTypeTitle { get; set; }

        [StringValidation] public string OrderItemTitle { get; set; }

        public DateTime? DateOfUse { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
    }


    public class VocherEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }

        public int Id { get; set; }
        [StringLength(300)] [Required] [StringValidation] public string Code { get; set; }
        [StringLength(600)] [StringValidation] public string Title { get; set; }
        [Range(0, int.MaxValue)] [Required] public int VocherTypeId { get; set; }
        public int? OrderItemId { get; set; }
        public DateTime? DateOfUse { get; set; }
        [StringValidation] public string Description { get; set; }
        [Range(0, int.MaxValue)] [Required] public int AccessLevelId { get; set; }
    }


    public class VocherSimpleViewModel : IBaseViewModel
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