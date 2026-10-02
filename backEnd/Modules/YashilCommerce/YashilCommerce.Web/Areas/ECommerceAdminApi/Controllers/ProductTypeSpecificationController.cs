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
	public class ProductTypeSpecificationController : BaseController<ProductTypeSpecification ,int,ProductTypeSpecificationListViewModel, ProductTypeSpecificationEditModel,ProductTypeSpecificationSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductTypeSpecificationService _productTypeSpecificationService;
        private readonly IYashilLogger<ProductTypeSpecification> _logger;  private readonly IPermissionService _permissionService;
        public ProductTypeSpecificationController(IProductTypeSpecificationService productTypeSpecificationService, IMapper mapper, IYashilLogger<ProductTypeSpecification> logger,IPermissionService permissionService): base(productTypeSpecificationService, mapper,logger,permissionService)
        {
            _mapper=mapper;
            _logger = logger; _permissionService = permissionService;
            _productTypeSpecificationService=productTypeSpecificationService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? productTypeId, int? specificationId)
        {
            var productTypeSpecifications = _productTypeSpecificationService.GetByCustomFilter(productTypeId,specificationId);
            return await DataSourceLoader.LoadAsync(productTypeSpecifications.ProjectTo<ProductTypeSpecificationListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? productTypeId, int? specificationId)
        {
            var productTypeSpecifications = _productTypeSpecificationService.GetByCustomFilter(productTypeId,specificationId);
            return await DataSourceLoader.LoadAsync(productTypeSpecifications.ProjectTo<ProductTypeSpecificationSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      