namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// مدل جستجو در برنامه ریزی دوره
    /// </summary>
    public class ProductSalesPlanningSearchDto
    {
        /// <summary>
        /// شناسه برنامه ریزی دوره
        /// </summary>
        public int ProductSalesPlanningId { get; set; }
        /// <summary>
        /// عنوان برنامه ریزی دوره
        /// </summary>
        public string ProductSalesPlanningTitle { get; set; }

        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }

        /// <summary>
        /// شناسه نمایندگی
        /// </summary>
        public int? RepresentationId { get; set; }

        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        public string? RepresentationTitle { get; set; }

    }

    /// <summary>
    /// مدل جستجو در مراکز
    /// </summary>
    public class CompanySearchDto
    {
        /// <summary>
        /// شناسه مرکز
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string Title { get; set; }
    }
}
