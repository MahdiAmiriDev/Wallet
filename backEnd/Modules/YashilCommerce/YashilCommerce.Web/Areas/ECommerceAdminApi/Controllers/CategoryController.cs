using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.ServiceImpl;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class CategoryController : BaseController<Category, int, CategoryListViewModel, CategoryEditModel,
        CategorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;

        public CategoryController(ICategoryService categoryService, IMapper mapper,
            IYashilLogger<Category> logger,
            IPermissionService permissionService, IAdditionalUserPropService additionalUserPropService,
            IUserPrincipal userPrincipal) 
            : base(categoryService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _additionalUserPropService = additionalUserPropService;
            _userPrincipal = userPrincipal;
            _categoryService = categoryService;
        }

        [HttpGet("GetByCompanyForList")]
        public async Task<LoadResult> GetByCompanyForList(CustomDataSourceLoadOptions loadOptions,
            int? companyId)
        {
            // TODO:fixed Filter
            var representations = //companyId.HasValue
                                  //  ? _categoryService.GetByCompanyId(companyId.Value):
                _categoryService.GetAll(true);

            return await DataSourceLoader.LoadAsync(
                representations.ProjectTo<CategoryListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCompanyMainCategoryId/{companyMainCategoryId}")]
        public Task<List<CategorySimpleViewModel>> GetByCompanyMainCategoryId(
            int companyMainCategoryId)
        {
            var courseCategories = _categoryService.GetByCompanyMainCategoryId(companyMainCategoryId);
            return courseCategories.ProjectTo<CategorySimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("GetMainCategoriesByRepresentationId")]
        public async Task<LoadResult> GetMainCategoriesByRepresentationId(CustomDataSourceLoadOptions loadOptions,
            int repId)
        {
            var categorys = _categoryService.GetByCustomFilter(null, null, repId);
            return await DataSourceLoader.LoadAsync(
                categorys.ProjectTo<CategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetRepresentationCategories")]
        public async Task<LoadResult> GetRepresentationCourseCategories(CustomDataSourceLoadOptions loadOptions,
            int representationId)
        {
            var representationCourseCategories = _categoryService.GetRepresentationCourseCategories(representationId);
            var res = representationCourseCategories.Select(x => new CategoryListViewModel
            {
                Title = x.Title,
                Id = x.Id,
                ParentId = x.ParentId,
                DisplayOrder = x.DisplayOrder
            });
            return await DataSourceLoader.LoadAsync(res, loadOptions);
        }

        [HttpGet("GetRepresentationMainCategories")]
        public async Task<LoadResult> GetRepresentationMainCategories(CustomDataSourceLoadOptions loadOptions,
            int representationId)
        {
            var representationCourseCategories = _categoryService.GetRepresentationMainCategories(representationId);
            return await DataSourceLoader.LoadAsync(
                representationCourseCategories.ProjectTo<MainCategorySimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]

        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId,
            int? companyId, bool loadLevelOneNodes = false)
        {
            // مرکز کاربر جاری را دریافت می کنیم
            var currentUserCompanyId = await
                _additionalUserPropService.FindByUserId(_userPrincipal.Id)
                    .Select(x => x.CompanyId)
                    .FirstOrDefaultAsync();

            companyId = currentUserCompanyId ?? companyId;
            // در صورتی که کاربر جاری مرکز داشته باشددسته بندی آن مرکز به همراه دسته بندی های بدون مرکز دریافت می شود
            // در غیر اینصورت دسته بندی های بدون مرکز دریافت می شود
            var categories = _categoryService.GetByCustomFilter(parentId, companyId, null, loadLevelOneNodes);
            return await DataSourceLoader.LoadAsync(
                categories.ProjectTo<CategoryListViewModel>(_mapper.ConfigurationProvider,
                    new Dictionary<string, object>
                    {
                        { "currentUserCompanyId", currentUserCompanyId }
                    }),
                loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId,
            int? companyId)
        {
            var categorys = _categoryService.GetByCustomFilter(parentId, companyId, null);
            return await DataSourceLoader.LoadAsync(
                categorys.ProjectTo<CategoryCustomViewModelForSelect>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(CategoryEditModel editModel)
        {
            var isValidParent = await CheckParentIsValidAsync(editModel);
            if (!isValidParent)
            {
                return "کاربر گرامی دسته پدر انتخاب شده صحیح نیست لطفا دسته پدر دیگری را انتخاب نمایید.";
            }
            // در صورتی که کاربر جاری دارای مرکز باشد فقط دسته بندی خود را می تواند تغییر بدهد
            var currentUserCompany = await _additionalUserPropService.FindByUserId(_userPrincipal.Id)
                .Select(x => x.CompanyId)
                .FirstOrDefaultAsync();
            if (currentUserCompany.HasValue && !editModel.CompanyId.HasValue
                || (currentUserCompany.HasValue && currentUserCompany.Value != editModel.CompanyId.Value)
                )
            {
                return "کاربر گرامی امکان تغییر دسته بندی برای شما وجود ندارد.";

            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var checkAllowDelete = await _categoryService.AllowDeleteCategoryAsync(id);
            if (!checkAllowDelete.allowDelete)
            {
                return checkAllowDelete.message;
            }
            return await base.CustomValidateBeforeDeleteAsync(id);
        }
        protected override async Task<(bool isSuccess, string message)> AfterInsertAsync(CategoryEditModel editModel, Category entity)
        {
            if (entity.Id > 0)
                await _categoryService.AssignToCompanyAndRepresentationAsync(entity.Id);
            return (true, string.Empty);
        }

        protected override async Task CustomMapBeforeInsertAsync(CategoryEditModel editModel, Category entity)
        {
            var currentUserCompanyId = await _additionalUserPropService.FindByUserId(_userPrincipal.Id)
                    .Select(x => x.CompanyId)
                    .FirstOrDefaultAsync();
            if (currentUserCompanyId.HasValue)
            {
                entity.CompanyId = currentUserCompanyId;
            }

        }

        private async Task<bool> CheckParentIsValidAsync(CategoryEditModel editModel)
        {
            if (!editModel.ParentId.HasValue)
            {
                return true;
            }

            var parentCategory = await _categoryService.GetAsync(editModel.ParentId.Value, true);
            if (parentCategory.CodePath.Split(',').Any(x => x == editModel.Id.ToString()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// دریافت برترین دسته بندی ها 
        /// براساس تعداد دوره و ترتیب نمایش
        /// </summary>
        /// <param name="topCount">تعداد آیتم های قابل نمایش</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetTopCategories")]
        public async Task<List<TopCategoryDto>> GetTopCategoriesAsync(int topCount)
        {
            return await _categoryService.GetTopCategoriesAsync(topCount);
        }

    }
}