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
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ProductTypeController : BaseController<ProductType, int, ProductTypeListViewModel, ProductTypeEditModel, ProductTypeSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductTypeService _productTypeService;
        private readonly IAppConfigService _appConfigService;
        private readonly IYashilLogger<ProductType> _logger; private readonly IPermissionService _permissionService;
        public ProductTypeController(IProductTypeService productTypeService, IMapper mapper, IAppConfigService appConfigService, IYashilLogger<ProductType> logger, IPermissionService permissionService) : base(productTypeService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _appConfigService = appConfigService;
            _logger = logger; _permissionService = permissionService;
            _productTypeService = productTypeService;
        }
        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId)
        {
            var productTypes = _productTypeService.GetByCustomFilter(parentId);
            return await DataSourceLoader.LoadAsync(productTypes.ProjectTo<ProductTypeListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId)
        {
            var productTypes = _productTypeService.GetByCustomFilter(parentId);
            return await DataSourceLoader.LoadAsync(productTypes.ProjectTo<ProductTypeSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task CustomMapBeforeInsertAsync(ProductTypeEditModel editModel, ProductType entity)
        {
            await SetEntityDefaultPropsAsync(editModel, entity);
            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }
        
        protected override async Task CustomMapBeforeUpdateAsync(ProductTypeEditModel editModel, ProductType entity)
        {
            await SetEntityDefaultPropsAsync(editModel, entity);
            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }
        private async Task SetEntityDefaultPropsAsync(ProductTypeEditModel editModel, ProductType entity)
        {
            if (editModel.TaxCategoryId == 0)
            {
                entity.TaxCategoryId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilDefaultTaxExemptTaxCategoryId);
            }
        }
    }
}