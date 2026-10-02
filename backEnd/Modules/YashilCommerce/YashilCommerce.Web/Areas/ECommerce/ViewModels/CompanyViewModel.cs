using Yashil.Common.Core.Attributes;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [StringValidation] public string Title { get; set; }
        [StringValidation] public string ShortName { get; set; }

        public string ModificationDate { get; set; }
    }

    /// <summary>
    /// مدل مربوط به دریافت اطلاعات یک مرکز
    /// </summary>
    public class CompanyInformationViewModel
    {
        /// <summary>
        /// آیا برنامه ریزی دوره فعال دارد یا نه
        /// </summary>
        public bool HasActiveProductSalesPlanning { get; set; }
        /// <summary>
        /// آیا قرارداد فعال دارد یا نه
        /// </summary>
        public bool HasActiveContract { get; set; }
        /// <summary>
        /// آیا تصویر حالت موبایل و دسکتاب دارد یا نه
        /// </summary>
        public bool HasDesktopAndMobileModeImage { get; set; }        
        /// <summary>
        /// آیا نمایش در صفحه ی اصلی برای مرکز فعال است یا نه
        /// </summary>
        public bool? ShowInMainPage { get; set; }

    }
}
