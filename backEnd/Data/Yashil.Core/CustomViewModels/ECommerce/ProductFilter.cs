using System.Collections.Generic;
using Yashil.Core.Enums;

namespace Yashil.Core.CustomViewModels.ECommerce
{
    public class ProductFilter
    {
        public ProductFilter()
        {
            ListViewMode = ListViewMode.Card;
        }
        public List<CategoryViewModel> SelectedCategoryWithParents { get; set; }

        public ListViewMode ListViewMode { get; set; }
        //public int? RootCategoryId { get; set; }
        public int? CategoryId { get; set; }
        public List<int> Company { get; set; }
        //public int? CompanyId { get; set; }
        public int? ItemCountInPage { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? RepresentationId { get; set; }
        public string RepresentationTitle { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryCssClass { get; set; }
        public string CompanyTitle { get; set; }
        public string SearchTitle { get; set; }
        public int? PageIndex { get; set; }
        /// <summary>
        /// گروه دسته بندی
        /// </summary>
        public string CategoryGroupTitle { get; set; }
        /// <summary>
        /// شناسه گروه دسته بندی
        /// </summary>
        public int? CategoryGroupId { get; set; }
    }
    public class CategoryViewModel
    {
        public int? ParentId { get; set; }
        public int Id { get; set; }

        public string ParentTitle { get; set; }

        public string CssClass { get; set; }

        public string Title { get; set; }

        public string CompanyTitle { get; set; }

        public int DisplayOrder { get; set; }
    }
}
