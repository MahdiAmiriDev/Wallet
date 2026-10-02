using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class OrderItemFullInfoViewViewModel
    {
        public int Id { get; set; }
        [StringValidation] public string? ApplicationTitle { get; set; }
        [StringValidation] public string? CardNumber { get; set; }
        [StringValidation] public string? NationalCode { get; set; }
        public int CreationDateInt { get; set; }
        [StringValidation] public string ProductTitle { get; set; }
        [StringValidation] public string OrganTitle { get; set; }
        [StringValidation] public string CompanyTitle { get; set; }
        [StringValidation] public string RepresentationTitle { get; set; }
        [StringValidation] public string CityTitle { get; set; }
        [StringValidation] public string ProvinceTitle { get; set; }
        [StringValidation] public string UserName { get; set; }
        public decimal DiscountAmountExclTax { get; set; }
        public decimal SataSubsidePrice { get; set; }
        public decimal VezaratSubsidyPrice { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
        public int OrderStatusType { get; set; }
        /// <summary>
        /// گروه دسته بندی
        /// </summary>
        public string CategoryGroupTitle { get; set; }
        /// <summary>
        /// وضعیت تایید یارانه
        /// </summary>
        public string SubsidyStatusTypeTitle { get; set; }

    }
}
