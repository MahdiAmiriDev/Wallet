using System.Collections.Generic;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class IndexPageViewModel
    {
        public IndexPageViewModel()
        {
            FirstLevelCategories = new List<CategorySimpleViewModel>();
            CategoryViewModels = new List<CategoryViewModel>();
            ProductFilter = new ProductFilter();
            CompanyViewModels = new List<CompanyViewModel>();
        }
        public List<CategoryViewModel> CategoryViewModels { get; set; }
        public List<CategorySimpleViewModel> FirstLevelCategories { get; set; }
        public ProductFilter ProductFilter { get; set; }
        public List<CompanyViewModel> CompanyViewModels { get; set; }
        public ProductListViewType ProductListViewType { get; set; }
        public bool AllowFilterByCity { get; set; }
        public string BannerUrl { get; set; }


    }
    public enum ProductListViewType
    {
        horizentalCard,
        verticalCard
    }


}
