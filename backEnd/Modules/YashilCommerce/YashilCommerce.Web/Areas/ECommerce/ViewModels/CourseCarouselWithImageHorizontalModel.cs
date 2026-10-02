using System.Collections.Generic;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    /// <summary>
    ///  مدل مربوط به کروسل میانی صفحه ی
    ///  پر فرش ترین ها و جدید ترین محصولات
    /// </summary>
    public class CourseCarouselWithImageHorizontalViewModel
    {
        /// <summary>
        /// عنوان کروسل
        /// </summary>
        public string CarouselTitle { get; set; }
        /// <summary>
        /// نام کروسل
        /// </summary>
        public string CarouselName { get; set; }
        /// <summary>
        /// ویو مدل محصولات 
        /// </summary>
        public IEnumerable<CompanyProductSalesPlanningViewModel> CompanyProductSalesPlanningViewModel { get; set; }

    }
}
