using System.Collections.Generic;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    public class CategoryHierarchicalViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Tag { get; set; }
        public string Href { get; set; }
        public List<CategoryHierarchicalViewModel> SubCategories { get; set; }
    }
}
