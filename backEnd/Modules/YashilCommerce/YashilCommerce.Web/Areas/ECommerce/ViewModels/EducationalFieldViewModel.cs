namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    /// <summary>
    /// رشته های تحصیلی درج شده در جدول
    /// CommonBaseData
    /// در این ویو مدل ست می شوند
    /// بر اساس
    /// CommonBaseType =  EducationalFieldType
    /// </summary>
    public class EducationalFieldViewModel
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
    }
}
