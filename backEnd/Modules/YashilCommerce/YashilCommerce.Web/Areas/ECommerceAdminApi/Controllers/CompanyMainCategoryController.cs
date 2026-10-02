using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class CompanyMainCategoryController : BaseController<CompanyMainCategory, int
        , CompanyMainCategoryListViewModel, CompanyMainCategoryEditModel,
        CompanyMainCategorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyMainCategoryService _companyMainCategoryService;
        private readonly IRepresentationCategoryService _representationCategoryService;

        public CompanyMainCategoryController(
            ICompanyMainCategoryService companyMainCategoryService, IMapper mapper,
            IYashilLogger<CompanyMainCategory> logger, IPermissionService permissionService,
            IRepresentationCategoryService representationCategoryService) : base(companyMainCategoryService,
            mapper, logger, permissionService)
        {
            _mapper = mapper;
            _representationCategoryService = representationCategoryService;
            _companyMainCategoryService = companyMainCategoryService;
        }

        [HttpGet("GetCompanyMainCategorysNotAssignedToCompanyAsync")]
        public async Task<LoadResult> GetCompanyMainCategorysNotAssignedToCompanyAsync(
            CustomDataSourceLoadOptions loadOptions, int id, int companyId)
        {
            var entities = _companyMainCategoryService.GetCompanyCategory
                (companyId, true, false);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<CategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetCompanyMainCategorysAssignedToCompanyAsync")]
        public async Task<LoadResult> GetCompanyMainCategorysAssignedToCompanyAsync(
            CustomDataSourceLoadOptions loadOptions, int id, int companyId)
        {
            var entities = _companyMainCategoryService.GetCompanyCategory
                (companyId, true, true);
            return await DataSourceLoader.LoadAsync(
                entities.ProjectTo<CategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpPost("AssignSelectedItemsToCompany")]
        public async Task<bool> AssignSelectedItemsToCompany(AssignableListEditModel assignableListEditModel)
        {
            return await _companyMainCategoryService.AssignSelectedItemsToCompany(
                new List<int>(assignableListEditModel.SelectedItems), assignableListEditModel.GroupId,
                assignableListEditModel.Assign);
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var isExistsProductCategoryOfMainCategory = await _companyMainCategoryService.
                CheckExistsProductCategoryOfMainCategoryAsync(id);
            if (isExistsProductCategoryOfMainCategory)
            {
                return "کاربر گرامی دسته بندی محصولات از این دسته بندی اصلی تعریف شده است.امکان حذف این دسته بندی اصلی وجود ندارد";
            }
            var companyMainCategoryId =await _companyMainCategoryService
                .FindById(id)
                .Select(x=>x.CategoryId)
                .FirstOrDefaultAsync();
            var isExistCategoryInRepresentationCategory =
                await _representationCategoryService.CheckExistRepresentationCategoryByCategoryIdAsync(
                    companyMainCategoryId);
            if (isExistCategoryInRepresentationCategory)
            {
                return "کاربر گرامی این رکورد در فرم های دیگر استفاده شده است و امکان حذف آن وجود ندارد.";
            }

            return await base.CustomValidateBeforeDeleteAsync(id);
        }

        [HttpPut("PutEntityCustom")]
        public async Task<HttpResponseMessage> PutEntityCustom(CompanyMainCategoryCustomEditModel editModel)
        {
            var entity = new CompanyMainCategory
            {
                DisplayOrder = editModel.EditModel.DisplayOrder,
                Id = editModel.EditModel.Id
            };
            await _companyMainCategoryService.UpdateAsync(entity, entity.Id, new List<string> { "DisplayOrder" },
                  true, true);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        //[HttpGet("GetByCompanyMainCategory/{id}")]
        //public async Task<List<CompanyMainCategorySimpleViewModel>> GetByCompanyMainCategory(int id)
        //{
        //    var mainCategories = _companyMainCategoryService.GetMainCourseCategoriesByCompanyMainCategory(id);
        //    return await mainCategories.ProjectTo<CompanyMainCategorySimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        //}

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? companyId,
            int? mainCategoryId)
        {
            var companyMainCategories = _companyMainCategoryService.GetByCustomFilter(companyId, mainCategoryId);
            return await DataSourceLoader.LoadAsync(
                companyMainCategories.ProjectTo<CompanyMainCategoryListViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? companyId,
            int? mainCategoryId)
        {
            var companyMainCategories = _companyMainCategoryService.GetByCustomFilter(companyId, mainCategoryId);
            return await DataSourceLoader.LoadAsync(
                companyMainCategories.ProjectTo<CompanyMainCategorySimpleViewModel>(_mapper.ConfigurationProvider),
                loadOptions);
        }
    }
}