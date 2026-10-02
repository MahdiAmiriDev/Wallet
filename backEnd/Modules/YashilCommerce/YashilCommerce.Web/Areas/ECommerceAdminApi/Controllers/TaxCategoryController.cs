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
    public class TaxCategoryController : BaseController<TaxCategory, int, TaxCategoryListViewModel, TaxCategoryEditModel, TaxCategorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ITaxCategoryService _taxCategoryService;
        private readonly IYashilLogger<TaxCategory> _logger; private readonly IPermissionService _permissionService;
        public TaxCategoryController(ITaxCategoryService taxCategoryService, IMapper mapper, IYashilLogger<TaxCategory> logger, IPermissionService permissionService) : base(taxCategoryService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _logger = logger; _permissionService = permissionService;
            _taxCategoryService = taxCategoryService;
        }
        /// <summary>
        /// دریافت لیست دسته بندی های مالیاتی یک برنامه
        /// این متد جهت نمایش و ذخیره در تنظیمات فروش بکار میرود
        /// دسته بندی های مالیاتی یک برنامه دریافت میشود و نام کانورت و کد و واحد سنجش آن
        /// در تظیمات فروش نشان داده میشود و ذخیره میشود
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("GetDefaultTaxValuesForFactor")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetTaxDefaultValuesForFactor(CustomDataSourceLoadOptions loadOptions)
        {
            var taxCategories = _taxCategoryService.GetAll(true);
            return await DataSourceLoader.LoadAsync(taxCategories.ProjectTo<TaxCategoryDefaultValuesViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }



        /// <summary>
        /// این متد بررسی می کند که
        /// saleConfigurationId
        /// قبلا استفاده شده است یا نه
        /// TaxCategory
        /// بر می گرداند که مقداری برای آن  در
        /// taxCategorySaleConfiguration
        /// درج نشده است
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="saleConfigurationId"></param>
        /// <param name="taxCategorySaleConfigurationId"></param>
        /// <returns></returns>
        [HttpGet("GetTaxCategoryNotUsedInTaxCategorySaleConfigurationForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetTaxCategoryNotUsedInTaxCategorySaleConfigurationForSelectAsync(CustomDataSourceLoadOptions loadOptions, int saleConfigurationId , int? taxCategorySaleConfigurationId = null)
        {
            var saleConfigurations =
                _taxCategoryService.GetTaxCategoryNotUsedInTaxCategorySaleConfiguration(saleConfigurationId , taxCategorySaleConfigurationId);
            return await DataSourceLoader.LoadAsync(saleConfigurations.ProjectTo<TaxCategorySimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

    }
}