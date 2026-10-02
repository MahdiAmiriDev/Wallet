using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.ObjectFactory;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.ProductVertical
{
    public class ProductVerticalListViewComponent : ViewComponent
    {
        private readonly IProductSalePlanningFactory _productSalePlanningFactory;
        private readonly IAppConfigService _appConfigService;
        public ProductVerticalListViewComponent(IProductSalePlanningFactory productSalePlanningFactory, IAppConfigService appConfigService)
        {
            _productSalePlanningFactory = productSalePlanningFactory;
            _appConfigService = appConfigService;
        }
        public async  Task<IViewComponentResult> InvokeAsync(string title,int categoryId, ProductFilter productFilter)
        {
            var pageSize = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceProductPageSize);
            return await _productSalePlanningFactory.InvokeAsync(title,categoryId,  productFilter.RepresentationId, productFilter.CityId, null,productFilter.Company,false, pageSize);
        }
    }
}
