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
	public class DependentProductSaleController : BaseController<DependentProductSale ,int,DependentProductSaleListViewModel, DependentProductSaleEditModel,DependentProductSaleSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IDependentProductSaleService _dependentProductSaleService;
        private readonly IYashilLogger<DependentProductSale> _logger;  private readonly IPermissionService _permissionService;
        public DependentProductSaleController(IDependentProductSaleService dependentProductSaleService, IMapper mapper, IYashilLogger<DependentProductSale> logger,IPermissionService permissionService): base(dependentProductSaleService, mapper,logger,permissionService)
        {
            _mapper=mapper;
            _logger = logger; _permissionService = permissionService;
            _dependentProductSaleService=dependentProductSaleService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? mainProduct, int? relatedProduct)
        {
            var dependentProductSales = _dependentProductSaleService.GetByCustomFilter(mainProduct,relatedProduct);
            return await DataSourceLoader.LoadAsync(dependentProductSales.ProjectTo<DependentProductSaleListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? mainProduct, int? relatedProduct)
        {
            var dependentProductSales = _dependentProductSaleService.GetByCustomFilter(mainProduct,relatedProduct);
            return await DataSourceLoader.LoadAsync(dependentProductSales.ProjectTo<DependentProductSaleSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      