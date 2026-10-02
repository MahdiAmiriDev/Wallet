using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ProductCategoryController : BaseController<ProductCategory, int, ProductCategoryListViewModel, ProductCategoryEditModel, ProductCategorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IYashilLogger<ProductCategory> _logger;  private readonly IPermissionService _permissionService;
        public ProductCategoryController(IProductCategoryService productCategoryService, IMapper mapper, IYashilLogger<ProductCategory> logger,IPermissionService permissionService): base(productCategoryService, mapper,logger,permissionService)
        {
            _mapper = mapper;
            _logger = logger; _permissionService = permissionService;
            _productCategoryService = productCategoryService;
        }
        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? categoryId)
        {
            var productCategorys = _productCategoryService.GetByCustomFilter(categoryId);
            return await DataSourceLoader.LoadAsync(productCategorys.ProjectTo<ProductCategoryListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? categoryId)
        {
            var productCategorys = _productCategoryService.GetByCustomFilter(categoryId);
            return await DataSourceLoader.LoadAsync(productCategorys.ProjectTo<ProductCategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }
}