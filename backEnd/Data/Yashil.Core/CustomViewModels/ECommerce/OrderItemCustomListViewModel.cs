using Yashil.Common.Core.Attributes;

namespace Yashil.Core.CustomViewModels.ECommerce
{
    public class OrderItemCustomListViewModel  
    {
       

        public int Id { get; set; }

        [StringValidation] public string Title { get; set; }

        public int OrderStatusType { get; set; }

        [StringValidation] public string MobileNumber { get; set; }

        //[StringValidation] public string NationalCode { get; set; }

        [StringValidation] public string RepresentationCode { get; set; }

        [StringValidation] public string ProductCode { get; set; }

        [StringValidation] public string OrderCode { get; set; }

        public int OrderPaidDateInt { get; set; }

        public int CreationDateInt { get; set; }

        [StringValidation] public string OrderTitle { get; set; }

        [StringValidation] public string ProductSalesPlanningTitle { get; set; }

        [StringValidation] public string RepresentationTitle { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal PriceIncDiscount { get; set; }

        public decimal WageAmountIncTax { get; set; }

        public int? SubsidyStatus { get; set; }

        [StringValidation] public string SubsidyTypeTitle { get; set; }

        public decimal SubsidyPrice { get; set; }

        public decimal TaxPrice { get; set; }

        public decimal PriceTotal { get; set; }

        public int InstallmentCount { get; set; }

        [StringValidation] public string SaleTypeTitle { get; set; }

        public decimal PricePerInstallment { get; set; }

        public decimal? RefundedAmount { get; set; }


        [StringValidation] public string Code { get; set; }

       


    }
}
