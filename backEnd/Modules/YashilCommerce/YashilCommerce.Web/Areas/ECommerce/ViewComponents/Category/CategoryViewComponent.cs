using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.Category
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;

        public CategoryViewComponent(ApplicationConfiguration applicationConfiguration, IMapper mapper, ICategoryService categoryService)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _categoryService = categoryService;
        }
        public IViewComponentResult Invoke()
        {
            var applicationCategories = _categoryService.GetApplicationCategories(_applicationConfiguration.ApplicationId);
            var categoryViewModels = applicationCategories.ProjectTo<CategorySimpleViewModel>(_mapper.ConfigurationProvider).ToList();
            return View("_Category", categoryViewModels);
        }
    }
}
