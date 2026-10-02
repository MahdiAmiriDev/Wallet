using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.CachedData;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Company
{
    public class CompanyViewComponent : ViewComponent
    {
        private readonly CacheDataService _cacheDataService;

        public CompanyViewComponent(
            CacheDataService cacheDataService)
        {
            _cacheDataService = cacheDataService;
        }
        public async Task<IViewComponentResult> InvokeAsync(
            ProductFilter productFilter, 
            CompanyViewMode companyViewMode = CompanyViewMode.Horizontal)
        {
            List<CompanyViewModel> companies =
                await _cacheDataService.
                    GetActiveCompaniesByFilterAsync(
                        productFilter.SearchTitle,
                        productFilter?.CityId,
                        productFilter?.CategoryId,
                        productFilter?.CategoryGroupId);

            var companyViewComponentViewModel = new CompanyViewComponentViewModel()
            {
                Companies = companies,
                ProductFilter = productFilter
            };

            if (companyViewMode == CompanyViewMode.Horizontal)
            {
                return View("_Horizontal", companyViewComponentViewModel);
            }
            else if (companyViewMode == CompanyViewMode.VerticalMultipleSelection)
            {
                return View("VerticalMultipleSelection", companyViewComponentViewModel);
            }
            else
            {
                return View("_VerticalSingleSelection", companyViewComponentViewModel);
            }
        }
    }
}
