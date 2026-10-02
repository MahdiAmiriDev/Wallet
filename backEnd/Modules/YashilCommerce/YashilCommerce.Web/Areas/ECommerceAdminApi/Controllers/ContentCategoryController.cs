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
    public class ContentCategoryController : BaseController<ContentCategory, int, ContentCategoryListViewModel, ContentCategoryEditModel, ContentCategorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IContentCategoryService _contentCategoryService;
        private readonly IYashilLogger<ContentCategory> _logger;
        private readonly IPermissionService _permissionService;

        public ContentCategoryController(IContentCategoryService contentCategoryService, IMapper mapper, IYashilLogger<ContentCategory> logger, IPermissionService permissionService) : base(contentCategoryService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _contentCategoryService = contentCategoryService;
            _logger = logger;
            _permissionService = permissionService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId)
        {
            var contentCategorys = _contentCategoryService.GetByCustomFilter(parentId);
            return await DataSourceLoader.LoadAsync(contentCategorys.ProjectTo<ContentCategoryListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId)
        {
            var contentCategorys = _contentCategoryService.GetByCustomFilter(parentId);
            return await DataSourceLoader.LoadAsync(contentCategorys.ProjectTo<ContentCategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}