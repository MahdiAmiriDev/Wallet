using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.ObjectFactory;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Home.Product
{
    public class ProductsByFilterViewComponent : ViewComponent
    {
        private readonly IProductSalePlanningFactory _productSalePlanningFactory;
        private readonly IAppConfigService _appConfigService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        public ProductsByFilterViewComponent(IProductSalePlanningFactory productSalePlanningFactory,
            IAppConfigService appConfigService, ApplicationConfiguration applicationConfiguration)
        {
            _productSalePlanningFactory = productSalePlanningFactory;
            _appConfigService = appConfigService;
            _applicationConfiguration = applicationConfiguration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productFilter"></param>
        /// <param name="categoryGroupId">گروه دسته بندی</param>
        /// <param name="groupedCategoryId">دسته بندی پدر</param>
        /// <param name="showNoResultOnEmpty"></param>
        /// <param name="showProductsByList"></param>
        /// <param name="showHomePageProductsView"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(ProductFilter productFilter, int? categoryGroupId,
            int? groupedCategoryId, bool showNoResultOnEmpty, bool? showProductsByList, bool showHomePageProductsView)
        {
            var pageSize = await _appConfigService.GetValueByApplicationIdAsync(
                ApplicationConfigKey.YashilCommerceProductPageSize, _applicationConfiguration.ApplicationId);
            var pageIndex = (productFilter != null && productFilter.PageIndex.HasValue) ? productFilter.PageIndex : 1;

            int? categoryId = groupedCategoryId ?? productFilter?.CategoryId;
            categoryGroupId ??= productFilter?.CategoryGroupId;

            return await _productSalePlanningFactory.InvokeAsync(productFilter?.SearchTitle, categoryId,
                productFilter?.RepresentationId, productFilter?.CityId, categoryGroupId, productFilter?.Company,
                showNoResultOnEmpty, pageSize, pageIndex, showProductsByList, showHomePageProductsView);
        }
    }
}
