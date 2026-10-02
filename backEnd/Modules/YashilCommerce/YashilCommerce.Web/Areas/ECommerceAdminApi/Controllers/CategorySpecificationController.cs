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
	public class CategorySpecificationController : BaseController<CategorySpecification ,int,CategorySpecificationListViewModel, CategorySpecificationEditModel,CategorySpecificationSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICategorySpecificationService _categorySpecificationService;
        private readonly IYashilLogger<CategorySpecification> _logger;  private readonly IPermissionService _permissionService;
        public CategorySpecificationController(ICategorySpecificationService categorySpecificationService, IMapper mapper, IYashilLogger<CategorySpecification> logger,IPermissionService permissionService): base(categorySpecificationService, mapper,logger,permissionService)
        {
            _mapper=mapper;
            _logger = logger; _permissionService = permissionService;
            _categorySpecificationService=categorySpecificationService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? categoryId, int? specificationId)
        {
            var categorySpecifications = _categorySpecificationService.GetByCustomFilter(categoryId,specificationId);
            return await DataSourceLoader.LoadAsync(categorySpecifications.ProjectTo<CategorySpecificationListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? categoryId, int? specificationId)
        {
            var categorySpecifications = _categorySpecificationService.GetByCustomFilter(categoryId,specificationId);
            return await DataSourceLoader.LoadAsync(categorySpecifications.ProjectTo<CategorySpecificationSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      