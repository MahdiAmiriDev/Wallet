using System.Collections.Generic;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            FirstLevelCategories = new List<CategorySimpleViewModel>();
            CategoryViewModels = new List<CategorySimpleViewModel>();
        }
        public List<CategorySimpleViewModel> CategoryViewModels { get; set; }
        public List<CategorySimpleViewModel> FirstLevelCategories { get; set; }
        public int RootCategoryId { get; set; }
        public bool AllowFilterByCity { get; internal set; }
    }
}
