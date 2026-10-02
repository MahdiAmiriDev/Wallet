
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class CategoryCustomerClubController : BaseController<CategoryCustomerClub, int, CategoryCustomerClubListViewModel, CategoryCustomerClubEditModel, CategoryCustomerClubSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryCustomerClubService _categoryCustomerClubService;
        private readonly IYashilLogger<CategoryCustomerClub> _logger;
        private readonly IPermissionService _permissionService;
        public CategoryCustomerClubController(ICategoryCustomerClubService categoryCustomerClubService,
        IMapper mapper,
        IYashilLogger<CategoryCustomerClub> logger,
        IPermissionService permissionService) : base(categoryCustomerClubService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _categoryCustomerClubService = categoryCustomerClubService;
            _logger = logger;
            _permissionService = permissionService;
        }
        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? customerClubId, int? categoryId)
        {
            var categoryCustomerClubs = _categoryCustomerClubService.GetByCustomFilter(customerClubId, categoryId,null);
            return await DataSourceLoader.LoadAsync(categoryCustomerClubs.ProjectTo<CategoryCustomerClubListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? customerClubId, int? categoryId)
        {
            var categoryCustomerClubs = _categoryCustomerClubService.GetByCustomFilter(customerClubId, categoryId, null);
            return await DataSourceLoader.LoadAsync(categoryCustomerClubs.ProjectTo<CategoryCustomerClubSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpPost("CheckExistsCategoryCustomerClubAndAddIfNeededByCategoryId")]
        public async Task<ResultStatus<int>> CheckExistsCategoryCustomerClubAndAddIfNeededByCategoryId(CategoryCustomerClubSimpleViewModel editModel)
        {
            var message = await _categoryCustomerClubService.CheckExistsCategoryCustomerClubAndAddIfNeededByCategoryIdAsync(editModel.CategoryId);
            if (string.IsNullOrEmpty(message))
            {
                return new ResultStatus<int>(true, string.Empty);
            }
            return new ResultStatus<int>(false, message);

        }
    }
}