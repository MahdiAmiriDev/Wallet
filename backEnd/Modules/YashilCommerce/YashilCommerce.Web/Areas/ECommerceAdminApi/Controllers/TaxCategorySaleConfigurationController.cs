using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
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
    public class TaxCategorySaleConfigurationController : BaseController<TaxCategorySaleConfiguration, int, TaxCategorySaleConfigurationListViewModel, TaxCategorySaleConfigurationEditModel, TaxCategorySaleConfigurationSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ITaxCategorySaleConfigurationService _taxCategorySaleConfigurationService;
        private readonly IYashilLogger<TaxCategorySaleConfiguration> _logger;
        private readonly IPermissionService _permissionService;
        public TaxCategorySaleConfigurationController(ITaxCategorySaleConfigurationService taxCategorySaleConfigurationService,
        IMapper mapper,
        IYashilLogger<TaxCategorySaleConfiguration> logger,
        IPermissionService permissionService) : base(taxCategorySaleConfigurationService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _taxCategorySaleConfigurationService = taxCategorySaleConfigurationService;
            _logger = logger;
            _permissionService = permissionService;
        }

        [HttpGet("GetByCustomFilterForList")]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId, int? taxCategoryId, int? saleConfigurationId, int? factorReportType)
        {
            var taxCategorySaleConfigurations = _taxCategorySaleConfigurationService.GetByCustomFilter(parentId, taxCategoryId, saleConfigurationId, factorReportType);
            return await DataSourceLoader.LoadAsync(taxCategorySaleConfigurations.ProjectTo<TaxCategorySaleConfigurationListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? parentId, int? taxCategoryId, int? saleConfigurationId, int? factorReportType)
        {
            var taxCategorySaleConfigurations = _taxCategorySaleConfigurationService.GetByCustomFilter(parentId, taxCategoryId, saleConfigurationId, factorReportType);
            return await DataSourceLoader.LoadAsync(taxCategorySaleConfigurations.ProjectTo<TaxCategorySaleConfigurationSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }


        protected override async Task<string> CustomValidateBeforeInsertAsync(TaxCategorySaleConfigurationEditModel editModel)
        {
            var isInsertedBefore = await _taxCategorySaleConfigurationService.CheckAnyTaskCategorySaleConfigurationInsertedBefore(editModel.TaxCategoryId, editModel.SaleConfigurationId);
            if (isInsertedBefore)
                return "کاربر گرامی دسته بندی مالیاتی که انتخاب کردید تکراری می باشد";

            return await base.CustomValidateBeforeInsertAsync(editModel);
        }
    }
}