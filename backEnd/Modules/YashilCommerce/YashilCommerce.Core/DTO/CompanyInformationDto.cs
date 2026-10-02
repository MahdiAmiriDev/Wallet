
namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// DTO
    /// مربوط به دریافت اطلاعات یک مرکز
    /// </summary>
    public class CompanyInformationDto
    {
        /// <summary>
        /// آیا برنامه ریزی دوره فعال دارد یا نه
        /// </summary>
        public bool HasActiveProductSalesPlanning { get; set; }
        /// <summary>
        /// آیا قرار داد فعال دارد یا نه
        /// </summary>
        public bool HasActiveContract { get; set; }
        /// <summary>
        /// آیا تصویر موبایل و دسکتاب دارد یا نه
        /// </summary>
        public bool HasDesktopAndMobileModeImage { get; set; }
        /// <summary>
        /// آیا گزینه نمایش در صفحه ی اصلی آن فعال است یا نه
        /// </summary>
        public bool? ShowInMainPage { get; set; }
    }
}
