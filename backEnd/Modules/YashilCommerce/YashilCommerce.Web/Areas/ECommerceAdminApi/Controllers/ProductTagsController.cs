using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using YashilCommerce.Core.Services;
using Yashil.Common.Web.Infrastructure.Filters;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.Utils.About;
using static DevExpress.Utils.Filtering.ExcelFilterOptions;
using Microsoft.EntityFrameworkCore.DynamicLinq;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class ProductTagsController : BaseController<ProductTags ,int,ProductTagsListViewModel, ProductTagsEditModel,ProductTagsSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductTagsService _productTagsService;
		private readonly IYashilLogger<ProductTags> _logger;
        private readonly IPermissionService _permissionService;
        public ProductTagsController(IProductTagsService productTagsService, 
        IMapper mapper, 
        IYashilLogger<ProductTags> logger,
        IPermissionService permissionService) : base(productTagsService, mapper,logger, permissionService)
        {
            _mapper = mapper;
            _productTagsService = productTagsService;
			_logger = logger;
			_permissionService = permissionService;
        }

        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? parentId, int? productId, int? tagType)
        {
            var productTagss = _productTagsService.GetByCustomFilter(parentId,productId,tagType);
            return await DataSourceLoader.LoadAsync(productTagss.ProjectTo<ProductTagsListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? parentId, int? productId, int? tagType)
        {
            var productTagss = _productTagsService.GetByCustomFilter(parentId,productId,tagType);
            return await DataSourceLoader.LoadAsync(productTagss.ProjectTo<ProductTagsSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// دریافت ProductTagsEditModel
        /// بر اساس شناسه محصول
        /// ProductId
        /// مورد استفاده در کامپوننت 
        /// tabase
        /// مربوط به تعریف محصول که پس از درج محصول بتوان تگ 
        /// های مربوط به آن را درج کرد
        /// </summary>
        /// <param name="productId">ProductId</param>
        /// <returns></returns>
        [HttpGet("GetByProductId")]
        [YashilAuthorization]
        public async Task<ProductTagsEditModel> GetByProductIdAsync(int productId)
        {
            return await _productTagsService.GetByCustomFilter(null, productId, null)
                   .ProjectTo<ProductTagsEditModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }
      }
}      