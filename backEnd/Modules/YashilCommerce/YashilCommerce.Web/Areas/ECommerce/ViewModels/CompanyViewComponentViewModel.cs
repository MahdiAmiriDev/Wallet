using System.Collections.Generic;
using Yashil.Core.CustomViewModels.ECommerce;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class CompanyViewComponentViewModel
    {
        public List<CompanyViewModel> Companies { get; set; }
        public ProductFilter ProductFilter { get; set; }
    }
}
