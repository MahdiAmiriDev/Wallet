

namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// dto
    /// مورد استفاده در دریافت  تعداد برنامه ریزی دوره های فعالی که
    /// بر اساس شناسه مرکز و شناسه
    /// categoryGroup
    /// گروه بندی شده اند
    /// </summary>
    public class GroupedActiveProductSalesPlanningDto
    {
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// تعداد برنامه ریزی دوره های فعال
        /// </summary>
        public int ProductSalesPlanningCount { get; set; }
    }
}
