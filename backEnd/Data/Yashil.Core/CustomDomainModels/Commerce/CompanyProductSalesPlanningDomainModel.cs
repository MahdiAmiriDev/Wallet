using System;

namespace Yashil.Core.CustomDomainModels.Commerce
{
    public class CompanyProductSalesPlanningDomainModel
    {
        public int Id { get; set; }
        /// <summary>
        /// مرکز
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// شناسه محصول
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// عنوان محصول
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// قیمت پایه
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// قیمت با تخفیف
        /// </summary>
        public decimal? CalcPrice { get; set; }
        /// <summary>
        /// درصد تخفیف
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        /// <summary>
        ///زمان تغییر
        ///</summary>
        public DateTime? ModificationDate { get; set; }
        /// <summary>
        /// نوع محصول
        /// </summary>
        public int ProductType { get; set; }
    }
}
