using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Core;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Category
{
    public class MenuCategoryViewComponent : YashilViewComponent
    {
        private readonly ICategoryService _categoryService;
      
        public IViewComponentResult Invoke()
        {
            var applicationCategories = _categoryService.GetApplicationCategories(ApplicationConfiguration.ApplicationId);
            var  categoryViewModels = applicationCategories.ProjectTo<CategorySimpleViewModel>(Mapper.ConfigurationProvider).ToList();
            return View("_MenuCategoryViewComponent", model: categoryViewModels);
        }

        public MenuCategoryViewComponent(ApplicationConfiguration applicationConfiguration, IMapper mapper, ICategoryService categoryService) : base(applicationConfiguration, mapper)
        {
            _categoryService = categoryService;
        }
    }
}
