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

    public class ProductSpecificationOptionController : BaseController<ProductSpecificationOption, int, ProductSpecificationOptionListViewModel, ProductSpecificationOptionEditModel, ProductSpecificationOptionSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductSpecificationOptionService _productSpecificationService;
        private readonly IYashilLogger<ProductSpecificationOption> _logger;  private readonly IPermissionService _permissionService;
        public ProductSpecificationOptionController(IProductSpecificationOptionService productSpecificationService, IMapper mapper, IYashilLogger<ProductSpecificationOption> logger,IPermissionService permissionService): base(productSpecificationService, mapper,logger,permissionService)
        {
            _mapper = mapper;
            _logger = logger; _permissionService = permissionService;
            _productSpecificationService = productSpecificationService;
        }
        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? productId, int? specificationOption)
        {
            var productSpecifications = _productSpecificationService.GetByCustomFilter(productId, specificationOption);
            return await DataSourceLoader.LoadAsync(productSpecifications.ProjectTo<ProductSpecificationOptionListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? productId, int? specificationOption)
        {
            var productSpecifications = _productSpecificationService.GetByCustomFilter(productId, specificationOption);
            return await DataSourceLoader.LoadAsync(productSpecifications.ProjectTo<ProductSpecificationOptionSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}