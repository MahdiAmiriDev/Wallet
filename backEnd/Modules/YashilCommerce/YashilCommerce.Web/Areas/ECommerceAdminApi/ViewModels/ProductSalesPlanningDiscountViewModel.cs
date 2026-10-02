using System.ComponentModel.DataAnnotations;
using Yashil.Common.Core.Interfaces;
namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{

    public class ProductSalesPlanningDiscountListViewModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int? ParentId { get; set; }
        public int Id { get; set; }

        public string Code { get; set; }

        public string ProductSalesPlanningTitle { get; set; }

        public string CustomerClubId { get; set; }

        public string CustomerClubTitle { get; set; }
        /// <summary>
        /// درصد تخفیف برنامه ریزی دوره
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        /// درصد تخفیف تنظیم فروش 
        /// </summary>
        public decimal? SaleConfigurationDiscountPercentage { get; set; }

        public string AccessLevelTitle { get; set; }

    }


    public class ProductSalesPlanningDiscountEditModel : IBaseViewModel
    {
        public int ViewModelId
        {
            get => Id;
            set => Id = value;
        }
        public int Id { get; set; }
        [StringLength(300)] public string Code { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int ProductSalesPlanningId { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int CustomerClubId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Description { get; set; }
        [Range(0, int.MaxValue)]
        [Required] public int AccessLevelId { get; set; }
        
    }





    public class ProductSalesPlanningDiscountSimpleViewModel : IBaseViewModel
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
