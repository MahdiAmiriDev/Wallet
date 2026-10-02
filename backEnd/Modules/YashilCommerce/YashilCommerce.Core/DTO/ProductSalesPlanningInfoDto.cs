namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// اطلاعات مربوط به یک دوره خاص
    /// </summary>
    public class ProductSalesPlanningInfoDto
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان محصول
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// عنوان نمایندگی
        /// </summary>
        public string RepresentationTitle { get; set; }
        /// <summary>
        /// عنوان مرکز
        /// </summary>
        public string CompanyTitle { get; set; }
        /// <summary>
        /// مدت دوره
        /// </summary>
        public int? Duration { get; set; }
        /// <summary>
        /// زمانبندی تشکیل کلاس
        /// </summary>
        public string CoursePlanningSchedule { get; set; }
        /// <summary>
        /// تاریخ شروع دوره
        /// </summary>
        public int? StartDate { get; set; }
        /// <summary>
        /// تاریخ پایان دوره
        /// </summary>
        public int? EndDate { get; set; }
        /// <summary>
        /// نوع برگزاری دوره
        /// </summary>
        public string ImplementationTypeTitle { get; set; }
        /// <summary>
        /// نوع دوره
        /// </summary>
        public string CourseTypeTitle { get; set; }
        /// <summary>
        /// روش اجرای دوره
        /// </summary>
        public string RunTypeTitle { get; set; }
        /// <summary>
        /// جنسیت
        /// </summary>
        public string CustomGenderTitle { get; set; }
    }
}
