using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Shared
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public MenuViewComponent(ApplicationConfiguration applicationConfiguration, IMapper mapper, ICategoryService categoryService)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var applicationCategories = await _categoryService
                .GetUsedCategoriesAsync(null, null, null, null,
                    null, null,_applicationConfiguration.ApplicationId, false);
            var categoryViewModelList = _mapper.Map<List<CategorySimpleViewModel>>(applicationCategories);
            var menuViewModel = new MenuViewModel { CategoryViewModels = categoryViewModelList };

            var rootCategory = categoryViewModelList.FirstOrDefault(x => !x.ParentId.HasValue);
            if (rootCategory != null)
            {
                menuViewModel.RootCategoryId = rootCategory.Id;
                menuViewModel.FirstLevelCategories = categoryViewModelList.Where(x => x.ParentId == rootCategory.Id).ToList();
            }
            return View(menuViewModel);
        }


    }
}
