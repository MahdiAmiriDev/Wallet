using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerce.ViewComponents.ProductCategory
{
    public class ProductCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public ProductCategoryViewComponent(ApplicationConfiguration applicationConfiguration, IMapper mapper, ICategoryService categoryService)
        {
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _categoryService = categoryService;

        }

        public async Task<IViewComponentResult> InvokeAsync(int? parentId, List<int> company)
        {
            int? companyId = null;
            if (company != null && company.Count > 0)
            {
                companyId = company.FirstOrDefault();
            }

            var applicationCategories = await _categoryService
                .GetUsedCategoriesAsync(parentId,null, companyId, null, null,
                    null,_applicationConfiguration.ApplicationId, false);
            List<CategorySimpleViewModel> applicationCategoryList = _mapper.Map<List<CategorySimpleViewModel>>(applicationCategories);

            int rootCategoryId = -1;
            var rootCategory = applicationCategoryList.FirstOrDefault(x => (!parentId.HasValue && !x.ParentId.HasValue) || x.Id == parentId);
            if (rootCategory != null)
            {
                rootCategoryId = rootCategory.Id;
            }

            var levelOneCategories = applicationCategoryList.Where(x => (!parentId.HasValue && x.ParentId == rootCategoryId)
                                                                      || (parentId.HasValue && x.Id == rootCategoryId));

            var model = levelOneCategories.Select(x => new CategoryHierarchicalViewModel
            {
                Id = x.Id.ToString(),
                Title = x.Title,
                SubCategories = applicationCategoryList.Where(cn => cn.ParentId == x.Id).Select(c => new CategoryHierarchicalViewModel
                {
                    Id = c.Id.ToString(),
                    Title = c.Title,
                }).ToList()
            });
            List<CategoryHierarchicalViewModel> cc = new List<CategoryHierarchicalViewModel>();
            return View("_ProductCategory", cc);
        }
    }
}
