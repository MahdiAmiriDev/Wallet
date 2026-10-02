using System;
using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels
{
    public class UnFactoredOrderItemViewModel
    {
        public int Id { get; set; }
        [StringValidation] public string OrderTitle { get; set; }
        public int SaleConfigurationId { get; set; }
        public int? RepresentationId { get; set; }
        [StringValidation] public string? RepresentationTitle { get; set; }
        public int CompanyId { get; set; }
        public int ApplicationId { get; set; }
        public int ProductId { get; set; }
        public int ProductSalePlanningId { get; set; }
        [StringValidation] public string ProductTitle { get; set; }
        [StringValidation] public string SaleConfigurationTitle { get; set; }
        public DateTime CreationDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal? DiscountAmountInclTax { get; set; }
        public decimal? DiscountAmountExclTax { get; set; }
        public decimal PriceIncDiscount { get; set; }
        public decimal? AdditionalShippingCharge { get; set; }
        public decimal? PriceIncDiscountWithAdditionalShippingCharge { get; set; }
        public decimal? TaxPercentage { get; set; }
        [StringValidation] public string? NationalCode { get; set; }
        [StringValidation] public string? CardNumber { get; set; }

        public bool GenerateFactorIsAllowedBasedOnProductGroup { get; set; }

        /// <summary>
        /// استفاده از گروه محصول برای صدور فاکتور
        /// </summary>
        public bool UseProductGroupForFactor { get; set; }

        /// <summary>
        /// عنوان گروه محصول
        /// </summary>
        public string ProductGroupTitle { get; set; }

        /// <summary>
        /// استفاده از گروه محصول برای صدور کانورت
        /// </summary>
        public bool UseProductGroupForConvert { get; set; }

        /// <summary>
        /// اگر گروه محصول
        /// null
        /// باشد پیام دارای مقدار است
        /// </summary>
        public string ProductGroupIdIsNullMessage { get; set; }
    }
}
