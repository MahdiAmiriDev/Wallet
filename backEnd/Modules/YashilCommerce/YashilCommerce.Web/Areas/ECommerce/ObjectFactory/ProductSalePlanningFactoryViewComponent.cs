using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Caching;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.ObjectFactory;
using YashilCommerce.Core.Services;
using Yashil.Common.Core.Classes;
using Yashil.Core.Enums;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerce.ObjectFactory
{

    public class ProductSalePlanningFactoryViewComponent : ViewComponent, IProductSalePlanningFactory
    {
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;


        public ProductSalePlanningFactoryViewComponent(
            ApplicationConfiguration applicationConfiguration,
            IMapper mapper,
            IProductSalesPlanningService productSalesPlanningService,
            IStaticCacheManager staticCacheManager,
            IUserPrincipal userPrincipal,
            IAdditionalUserPropService additionalUserPropService)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _productSalesPlanningService = productSalesPlanningService;
            _staticCacheManager = staticCacheManager;
            _userPrincipal = userPrincipal;
            _additionalUserPropService = additionalUserPropService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string title, int? categoryId, int? representationId,
            int? cityId, int? categoryGroupId, List<int> company, bool showNoResultOnEmpty, int itemCount, int? pageIndex = 1, bool? showProductsByList = null, bool showHomePageProductsView = false)
        {
            var userAdditionalUserProp =
                await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
                YashilModelCacheDefaults<ProductSalesPlanning>.ProductSalesPlanningByFilterModelKey,
                userAdditionalUserProp.CustomerClubId,
                userAdditionalUserProp.IsResumeFillOutEnough,
                 categoryId,
                representationId, cityId, categoryGroupId, company, showProductsByList);

            cacheKey.CacheTime = (int)CacheTime.Default;

            if (string.IsNullOrEmpty(title) && itemCount == 30 && pageIndex == 1)
            {
                return await _staticCacheManager.GetAsync(cacheKey, async () =>
                {
                    var productsByFilterViewModel = await GetProductPlanningByFilter(
                        userAdditionalUserProp,
                        title,
                        categoryId,
                        categoryGroupId,
                        cityId,
                        company,
                        representationId,
                        showNoResultOnEmpty,
                        itemCount,
                        1,
                        showProductsByList
                        );

                    if (showHomePageProductsView)
                        return View("_CategoriesCarouselProductSalePlanning", productsByFilterViewModel);


                    return View("_ProductsByFilter", productsByFilterViewModel);
                });
            }
            else
            {
                var productsByFilterViewModel = await GetProductPlanningByFilter(
                    userAdditionalUserProp,
                    title,
                    categoryId,
                    categoryGroupId,
                    cityId,
                    company,
                    representationId,
                    showNoResultOnEmpty,
                    itemCount,
                    pageIndex.Value,
                    showProductsByList);

                if (showHomePageProductsView)
                    return View("_CategoriesCarouselProductSalePlanning", productsByFilterViewModel);


                return View("_ProductsByFilter", productsByFilterViewModel);
            }
        }

        public async Task<ProductsByFilterViewModel> GetProductPlanningByFilter(
            AdditionalUserPropsDto userAdditionalUserProp
            , string title, int? categoryId, int? categoryGroupId, int? cityId, List<int> company, int? representationId
            , bool showNoResultOnEmpty, int pageSize, int pageIndex, bool? showProductsByList)
        {

            var productSalesPlannings =
                await _productSalesPlanningService.SearchProductSalesPlanningsAsync(title, null,
                _applicationConfiguration.ApplicationId, categoryId, company, representationId, cityId, categoryGroupId, true, true);


            var additionalAutoMapperParameters = new Dictionary<string, object>
            {
                { "customerClubId", userAdditionalUserProp.CustomerClubId }
            };

            List<ProductSalePlanningViewModel> productSalePlanningList;


            //اگر در حالتی که دوره ها را به صورت لیست نمایش می دهیم نباشیم عملیات گروه بندی دوره ها بر اساس 
            //مرکز دوره را انجام می دهیم و از هر مرکز با توجه به الگوریتم نوشته شده تعداد مشخصی دوره را می آوریم
            if (showProductsByList.HasValue && !showProductsByList.Value && company != null && !company.Any())
            {
                //اطلاعات برنامه ریزی دوره های فعال که گروه بندی شده اند بر اساس مرکز و شناسه دسته بندی گروه
                var groupedActiveProductSalesPlanningInfo = await _productSalesPlanningService
                    .GetGroupedActiveProductSalesPlanningCountAsync(
                        _applicationConfiguration.ApplicationId,
                        categoryId, company, representationId, cityId, categoryGroupId, productSalesPlannings);

                //تعداد مراکز شرکت کرده در گروه بندی
                var countOfAllCompanyThatJoinToGroup = groupedActiveProductSalesPlanningInfo
                    .Select(x => x.CompanyId).Count();

                int countOfPspNeedToTakeFromEachCompany;
                if (countOfAllCompanyThatJoinToGroup != 0)
                {
                    //تعداد برنامه ریزی دوره های مورد نیاز از هر مرکز
                    countOfPspNeedToTakeFromEachCompany = (int)Math.Floor((decimal)(pageSize / countOfAllCompanyThatJoinToGroup));
                }
                else
                {
                    countOfPspNeedToTakeFromEachCompany = pageSize;
                }

                additionalAutoMapperParameters.Add("takeCount", countOfPspNeedToTakeFromEachCompany);

                var groupedProductSalesPlanning = await productSalesPlannings
                    .GroupBy(x => x.Product.CompanyId)
                    .ProjectTo<ProductSalePlanningGroupedByCompanyViewModel>(_mapper.ConfigurationProvider, additionalAutoMapperParameters)
                    .ToListAsync();

                productSalePlanningList = groupedProductSalesPlanning.SelectMany(x => x.GroupedProductSalesPlanning).ToList();

            }
            else
            {
                productSalesPlannings = productSalesPlannings.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                productSalePlanningList = await productSalesPlannings
                    .ProjectTo<ProductSalePlanningViewModel>(_mapper.ConfigurationProvider, additionalAutoMapperParameters)
                    .ToListAsync();
            }


            return new ProductsByFilterViewModel
            {
                IsResumeFillOutEnough = userAdditionalUserProp.IsResumeFillOutEnough,
                ProductSalePlannings = productSalePlanningList,
                SubsidyBased = userAdditionalUserProp.SupportSubsidy,
                ShowNoResultOnEmpty = showNoResultOnEmpty,
                ShowProductsByList = showProductsByList
            };
        }
    }
}
