using JetBrains.Annotations;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class CompanyProductSalesPlanningViewModel
    {
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
        /// شناسه برنامه ریزی محصول
        /// </summary>
        public int Id { get; set; }
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
        /// زمان تغییر
        ///</summary>
        [CanBeNull]
        public string ModificationDate { get; set; }
        /// <summary>
        /// آیا اشتغال است
        /// </summary>
        public bool IsEmployment { get; set; }
    }
}
