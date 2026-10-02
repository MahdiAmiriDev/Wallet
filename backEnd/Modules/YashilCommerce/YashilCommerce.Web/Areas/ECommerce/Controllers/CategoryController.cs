using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilDms.Core.Services;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using YashilCommerce.Web.Areas.ECommerce.CachedData;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class CategoryController : YashilMvcBaseController<Category>
    {
        private readonly IMapper _mapper;
        private readonly CacheDataService _cacheDataService;

        public CategoryController(
            ApplicationConfiguration applicationConfiguration,
            IMapper mapper,
            IYashilLogger<Category> logger,
            IPermissionService permissionService,
            ICategoryService categoryService,
            IAppDocumentService appDocumentService,
            IStaticCacheManager staticCacheManager,
            CacheDataService cacheDataService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _cacheDataService = cacheDataService;
        }


        [AllowAnonymous]
        public async Task<IActionResult> CategoryMobileMenuList(List<int> company, int? cityId)
        {
            var categoryHierarchicalViewModelList = await _cacheDataService
                .GetCategoryHierarchicallyModel(company, null, null, cityId);

            return View("_MobileCategoryMenuList", categoryHierarchicalViewModelList);

        }

    }
}
