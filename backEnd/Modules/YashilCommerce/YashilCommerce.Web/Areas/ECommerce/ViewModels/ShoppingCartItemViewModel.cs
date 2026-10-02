using System.Collections.Generic;
using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class ShoppingCartItemViewModel
    {
        public int Id { get; set; }
        [StringValidation] public string Code { get; set; }

        [StringValidation] public string Title { get; set; }

        public decimal CustomerEnteredPrice { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // TODO Fix This Remove From ProductSalePlanning ******
        public decimal CalcPrice { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        [StringValidation] public string CompanyTitle { get; set; }
        /// <summary>
        /// مبلغ با تخفیف
        /// </summary>
        public decimal Discount
        {
            get
            {
                return (UnitPrice - CalcPrice);
            }
        }
        public int ProductSalesPlanningId { get; set; }

        public int? MaxSataSubsidy { get; set; }

        public int? MaxVezaratSubsidy { get; set; }

    }

    public class ShoppingCartItemPageViewModel
    {
        public List<ShoppingCartItemViewModel> ShoppingCartItemViewModels { get; set; }
        public bool SubsidyBased{ get; set; }
    }
}
