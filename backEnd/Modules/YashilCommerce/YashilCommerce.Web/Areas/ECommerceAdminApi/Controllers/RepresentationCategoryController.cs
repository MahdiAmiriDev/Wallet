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
    public class RepresentationCategoryController : BaseController<RepresentationCategory, int,
        RepresentationCategoryListViewModel, RepresentationCategoryEditModel, RepresentationCategorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepresentationCategoryService _representationCategoryService;
        private readonly IAdditionalUserPropService _additionalUserPropService;

        public RepresentationCategoryController(IRepresentationCategoryService representationCategoryService,
            IMapper mapper, IYashilLogger<RepresentationCategory> logger,IPermissionService permissionService, IAdditionalUserPropService additionalUserPropService): base(representationCategoryService, mapper,logger,permissionService)
        {
            _mapper = mapper;
            _additionalUserPropService = additionalUserPropService;
            _representationCategoryService = representationCategoryService;
        }

        [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,
            int? representationId, int? categoryId)
        {
            var representationCategories =
                _representationCategoryService.GetByCustomFilter(representationId, categoryId);
            return await DataSourceLoader.LoadAsync(
                representationCategories.ProjectTo<RepresentationCategoryListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,
            int? representationId, int? categoryId)
        {
            var representationCategorys =
                _representationCategoryService.GetByCustomFilter(representationId, categoryId);
            return await DataSourceLoader.LoadAsync(
                representationCategorys.ProjectTo<RepresentationCategorySimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        [HttpGet("GetRepresentationCategorysNotAssignedToRepresentationAsync")]
        public async Task<LoadResult> GetRepresentationCategoryNotAssignedToCompanyAsync(
            CustomDataSourceLoadOptions loadOptions, int id)
        {
            var entities = _representationCategoryService.GetCompanyCategory(id, false, false);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<CategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetRepresentationCategorysAssignedToRepresentationAsync")]
        public async Task<LoadResult> GetRepresentationCategoryAssignedToCompanyAsync(
            CustomDataSourceLoadOptions loadOptions, int id)
        {
            var entities = _representationCategoryService.GetCompanyCategory(id, false, true);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<CategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// دریافت لیست دسته بندی های اساین شده به یک نمایندگی
        /// در فیلتر مرکز و نمایندگی و دسته بندی کاربرد دارد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        [HttpGet("GetRepresentationCategories")]
        public async Task<LoadResult> GetRepresentationCategoriesAsync(
            CustomDataSourceLoadOptions loadOptions, int representationId)
        {
            var entities = _representationCategoryService.GetCompanyCategory(representationId, false, true);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<CategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// دریافت دسته بندی نمایندگی بر اساس شناسه نمایندگی
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="representationId">شناسه نمایندگی</param>
        /// <returns></returns>
        [HttpGet("GetByRepresentationId")]
        public async Task<LoadResult> GetByRepresentationId(
            CustomDataSourceLoadOptions loadOptions, int representationId)
        {
            var entities = _representationCategoryService
                .GetByRepresentationId(representationId,true);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<CategoryRepresentationListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }
    }
}