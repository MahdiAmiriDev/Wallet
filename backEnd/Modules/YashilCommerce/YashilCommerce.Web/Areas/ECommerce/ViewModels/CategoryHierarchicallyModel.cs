using System.Collections.Generic;

namespace YashilCommerce.Web.Areas.ECommerce.ViewModels
{
    /// <summary>
    /// مدل مربوط به دسته بندی ها به صورت سلسله مراتبی
    /// </summary>
    public class CategoryHierarchicallyModel
    {
        public List<CategoryHierarchicalViewModel> CategoryHierarchicalList { get; set; }
        public CategoryHierarchicalViewModel CategoryHierarchical { get; set; }
        /// <summary>
        /// شناسه دسته بندی
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
