using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class ProductListPartialViewModel
    {
        public CategorySimpleViewModel CategoryViewModel { get; set; }
        public ProductFilter ProductFilter { get; set; }
        public int CategoryId { get; set; }
        /// <summary>
        /// اگر ترو باشد باشد اسلایدر را بر اساس ای دی مدل صفحه پیدا می کند و کانفیگ را انجام می دهد
        /// </summary>
        public bool? SetSliderCustomConfig {get; set;}
        /// <summary>
        /// نمایش محصولات به صورت لیست
        /// </summary>
        public bool? ShowProductsByList { get; set; }
        /// <summary>
        /// نمایش صفحه بدون نتیجه
        /// </summary>
        public bool? ShowNoResultOnEmpty { get; set; }
    }
}
