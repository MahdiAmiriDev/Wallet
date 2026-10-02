using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Attributes;
using Yashil.Common.Core.Interfaces;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class ShoppingCartItemListViewModel : IBaseViewModel
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

        [StringValidation] public string ShoppingCartTypeTitle { get; set; }

        [StringValidation] public string ProductSalesPlanningTitle { get; set; }

        public decimal CustomerEnteredPrice { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int RepresentationId { get; set; }
        public int CompanyId { get; set; }
        public int ProductSalesPlanningId { get; set; }


    }


    public class ShoppingCartItemEditModel : IBaseViewModel
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
        [Required] public int ShoppingCartType { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ProductSalesPlanningId { get; set; }
        [StringValidation] public string AttributesXml { get; set; }
        [Required] public decimal CustomerEnteredPrice { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int Quantity { get; set; }
        [StringValidation] public string Description { get; set; }
    }


    public class ShoppingCartItemSimpleViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        public int Quantity { get; set; }
        public int RepresentationId { get; set; }

    }

}
