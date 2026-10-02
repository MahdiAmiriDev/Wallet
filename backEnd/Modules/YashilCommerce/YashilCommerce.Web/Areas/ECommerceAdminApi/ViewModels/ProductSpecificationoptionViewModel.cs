using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class ProductSpecificationOptionListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        [StringValidation] public string ProductTitle { get; set; }

        [StringValidation] public string SpecificationOptionTitle { get; set; }

        public int DisplayOrder { get; set; }

        public int AttributeType { get; set; }

        public bool AllowFiltering { get; set; }

        public bool ShowOnProductPage { get; set; }

        [StringValidation] public string AccessLevelTitle { get; set; }
        [StringValidation] public string SpecificationTitle { get; set; }
        [StringValidation] public string AttributeTypeTitle { get; set; }
        public int SpecificationId { get; set; }
    }

    public class ProductSpecificationOptionEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ProductId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int SpecificationOptionId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int DisplayOrder { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AttributeType { get; set; }
        [StringValidation] public string CustomValue { get; set; }
        public bool AllowFiltering { get; set; }
        public bool ShowOnProductPage { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }

        public int SpecificationId { get; set; }

    }
    public class ProductSpecificationOptionSimpleViewModel : IBaseViewModel
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
