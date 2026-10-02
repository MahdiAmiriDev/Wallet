using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.DataFactory;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.FilterItemsContainer
{
    public class FilterItemsContainerViewComponent : ViewComponent
    {
        private readonly ICompanyService _companyService;
        private readonly ICategoryService _categoryService;
        private readonly IPublicDataFactory _publicDataFactory;
        private readonly IRepresentationService _representationService;
        private readonly ICategoryGroupService _categoryGroupService;

        public FilterItemsContainerViewComponent(
            ICompanyService companyService,
            ICategoryService categoryService,
            IPublicDataFactory publicDataFactory,
            IRepresentationService representationService,
            ICategoryGroupService categoryGroupService)
        {

            _companyService = companyService;

            _categoryService = categoryService;
            _publicDataFactory = publicDataFactory;
            _representationService = representationService;
            _categoryGroupService = categoryGroupService;
        }
        public async Task<IViewComponentResult> InvokeAsync(ProductFilter productFilter, FilterContainerViewMode filterContainerViewMode = FilterContainerViewMode.DesktopMode)
        {
            if (productFilter != null)
            {
                if (productFilter.CityId.HasValue)
                {
                    var city = await _publicDataFactory.GetCity(productFilter.CityId.Value);
                    productFilter.CityName = city?.FullTitle;
                }
                if (productFilter.CategoryId.HasValue)
                {
                    productFilter.CategoryTitle = _categoryService.GetTitle(productFilter.CategoryId.Value);
                }
                if (productFilter.Company != null && productFilter.Company.Count > 0)
                {
                    productFilter.CompanyTitle = _companyService.GetTitle(productFilter.Company[0]);
                }
                if (productFilter.RepresentationId.HasValue)
                {
                    productFilter.RepresentationTitle = _representationService.GetTitle(productFilter.RepresentationId.Value);
                }
                if (productFilter.CategoryGroupId.HasValue)
                {
                    productFilter.CategoryGroupTitle = await _categoryGroupService.GetTitleAsync(productFilter.CategoryGroupId.Value);
                }
            }
            if (filterContainerViewMode == FilterContainerViewMode.MobileMode)
            {
                return View("_Mobile", productFilter);
            }
            else
            {
                return View("_Desktop", productFilter);
            }

        }
    }
}
