using System.Collections.Generic;
using Yashil.Core.CustomViewModels.ECommerce;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class CategoriesCarouselProductSalePlanningViewModel
    {
        public List<ProductSalePlanningViewModel> ProductSalePlannings { get; set; }
        /// <summary>
        /// بر اساس سوبسید
        /// </summary>
        public bool SubsidyBased { get; set; }
        /// <summary>
        /// نمایش صفحه ی بدون نتیجه در صورت پیدا نشدن محصولی بر اساس فیلتر
        /// </summary>
        public bool? ShowNoResultOnEmpty { get; set; }
        /// <summary>
        /// نمایش محصولات به صورت لیست
        /// </summary>
        public bool? ShowProductsByList { get; set; }
        /// <summary>
        /// آیا کاربر جاری رزومه را کامل پر کرده است
        /// </summary>
        public bool IsResumeFillOutEnough { get; set; }
        public string CategoryTitle { get; set; }
        public int CategoryId { get; set; }


    }
}
