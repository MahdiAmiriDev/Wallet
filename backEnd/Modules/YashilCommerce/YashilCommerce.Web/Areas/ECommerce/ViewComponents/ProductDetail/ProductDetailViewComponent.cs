using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Core.CustomViewModels.ECommerce;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Services;
using Yashil.Core.Entities;
using Yashil.Common.Core.Caching;
using Yashil.Infrastructure.Caching;
using Yashil.Core.Enums;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.ProductDetail
{
    public class ProductDetailViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IStaticCacheManager _staticCacheManager;

        public ProductDetailViewComponent(IMapper mapper, IProductSalesPlanningService productSalesPlanningService,
            IAdditionalUserPropService additionalUserPropService, IUserPrincipal userPrincipal,
            ISaleConfigurationService saleConfigurationService, IStaticCacheManager staticCacheManager)
        {
            _mapper = mapper;
            _productSalesPlanningService = productSalesPlanningService;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
            _saleConfigurationService = saleConfigurationService;
            _staticCacheManager = staticCacheManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pspId)
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<ProductSalesPlanning>.GetProductDetailCacheKey,
                pspId,
                userAdditionalUserProp.CustomerClubId);

            cacheKey.CacheTime = (int)CacheTime.Default;
            return await _staticCacheManager.GetAsync(cacheKey,
                  async () =>
                  {
                      var customerClubData = new Dictionary<string, object>
                          { { "customerClubId", userAdditionalUserProp.CustomerClubId } };
                      IQueryable<ProductSalesPlanning>
                          productSalesPlanning = _productSalesPlanningService.FindById(pspId);
                      ProductSalesPlanningCustomViewModel productSalesPlanningCustomViewModel = await
                          productSalesPlanning
                              .ProjectTo<ProductSalesPlanningCustomViewModel>(_mapper.ConfigurationProvider,
                                  customerClubData).FirstOrDefaultAsync();

                      #region CalcInstallmentCount
                      if (productSalesPlanningCustomViewModel != null)
                      {

                          var saleConfigurationAttributeInfo = await _saleConfigurationService
                              .GetSaleConfigurationAttributeByCurrentUserCustomerClubAsync(pspId);

                          List<SaleConfigurationAttributeDto> saleConfigurationAttributeDtoList =
                              new List<SaleConfigurationAttributeDto> { saleConfigurationAttributeInfo };
                          if (productSalesPlanning != null)
                          {
                              var productSalesPlanningPriceDto = new ProductSalesPlanningPriceDto
                              {
                                  BasePrice = productSalesPlanning.FirstOrDefault()?.Price,
                                  SaleConfigurationId = productSalesPlanning.FirstOrDefault().SaleConfigurationId
                              };
                              List<ProductSalesPlanningPriceDto> productSalesPlanningPriceDtoList =
                                  new List<ProductSalesPlanningPriceDto> { productSalesPlanningPriceDto };


                              productSalesPlanningCustomViewModel.InstallmentCount = await _saleConfigurationService
                                  .GetSaleConfigurationsInstallmentCountAsync
                                      (productSalesPlanningPriceDtoList, saleConfigurationAttributeDtoList);
                          }


                      }
                      #endregion
                      return View("_ProductDetail", productSalesPlanningCustomViewModel);

                  });
        }
    }
}
