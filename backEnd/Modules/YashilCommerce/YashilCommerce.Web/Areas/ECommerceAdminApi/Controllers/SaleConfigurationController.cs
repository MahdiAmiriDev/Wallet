using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class SaleConfigurationController : BaseController<SaleConfiguration, int, SaleConfigurationListViewModel, SaleConfigurationEditModel, SaleConfigurationSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly ICategoryService _categoryService;
        private readonly IFactorService _factorService;
        private readonly IAppConfigService _appConfigService;
        private readonly ISaleConfigurationAttributeService _salesConfigurationAttributeService;
        private readonly IYashilLogger<SaleConfiguration> _logger;
        private readonly IActivityLogService _activityLogService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;

        public SaleConfigurationController(ISaleConfigurationService saleConfigurationService, IMapper mapper, IYashilLogger<SaleConfiguration> logger, IPermissionService permissionService, IFactorService factorService, IAppConfigService appConfigService, ICategoryService categoryService, ISaleConfigurationAttributeService salesConfigurationAttributeService, IActivityLogService activityLogService, IUserPrincipal userPrincipal, IOrderItemService orderItemService, IProductSalesPlanningService productSalesPlanningService) : base(saleConfigurationService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorService = factorService;
            _saleConfigurationService = saleConfigurationService;
            _appConfigService = appConfigService;
            _categoryService = categoryService;
            _salesConfigurationAttributeService = salesConfigurationAttributeService;
            _activityLogService = activityLogService;
            _userPrincipal = userPrincipal;
            _orderItemService = orderItemService;
            _productSalesPlanningService = productSalesPlanningService;
            _logger = logger;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? representationId, int? companyId, int? taxCategoryId)
        {
            var saleConfigurations = _saleConfigurationService.GetByCustomFilter(representationId, companyId);
            return await DataSourceLoader.LoadAsync(saleConfigurations.ProjectTo<SaleConfigurationListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? representationId, int? companyId, int? taxCategoryId)
        {
            var saleConfigurations = _saleConfigurationService.GetByCustomFilter(representationId, companyId);
            return await DataSourceLoader.LoadAsync(saleConfigurations.ProjectTo<SaleConfigurationSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task CustomMapBeforeUpdateAsync(SaleConfigurationEditModel editModel, SaleConfiguration entity)
        {
            entity.LicenseExpireDate = editModel.ToDate;
            // deactivating old sale configuration
            await DeActiveOldSaleConfigurationsAsync(editModel);
            await UpdateHasTaxCertificateForUnFactoredOrderItemBySaleConfigurationIdAsync(entity.Id, editModel.HasTaxCertificate);
        }

        protected override async Task CustomMapBeforeInsertAsync(SaleConfigurationEditModel editModel, SaleConfiguration entity)
        {
            entity.LicenseExpireDate = editModel.ToDate;
            // deactivating old sale configuration
            await DeActiveOldSaleConfigurationsAsync(editModel);
        }

        private async Task DeActiveOldSaleConfigurationsAsync(SaleConfigurationEditModel editModel)
        {
            // check the sale configuration is active or not
            // for deactivating old sale configuration current sale configuration for update or save must active
            if (editModel.IsActive)
            {
                await _saleConfigurationService.DeactivateOldSaleConfigurationsAsync(editModel.CompanyId, editModel.RepresentationId, editModel.Id);
            }
        }
        /// <summary>
        /// بررسی و به روز رسانی فیلد دارای گزارش ارزش افزوده آیتم های سفارش مربوط به تنظیم فروش
        /// </summary>
        /// <param name="saleConfigurationId">شناسه تنظیم فروش</param>
        /// <param name="editModelHasTaxCertificate"></param>
        /// <returns></returns>
        private async Task UpdateHasTaxCertificateForUnFactoredOrderItemBySaleConfigurationIdAsync(
            int saleConfigurationId,
            bool editModelHasTaxCertificate)
        {
            var saleConfigurationHasTaxCertificate = await _saleConfigurationService.FindById(saleConfigurationId)
                .Select(x => x.HasTaxCertificate)
                .FirstOrDefaultAsync();
            // در صورتی که مقدار فیلد دارای گزارش ارزش افزروده تغییر داده شود
            // فیلد دارای گزارش ارزش افزروده در آیتم سفارش های فاکتور نشده مربوط به این تنظیم فروش 
            // را به روز رسانی می کنیم
            if (saleConfigurationHasTaxCertificate != editModelHasTaxCertificate)
            {
                await _orderItemService.UpdateHasTaxCertificateForUnFactoredOrderItemBySaleConfigurationIdAsync(saleConfigurationId, editModelHasTaxCertificate);
            }

        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(SaleConfigurationEditModel editModel)
        {
            // ایتدا قرارداد با تنظیم فروش آن دریافت میشود
            var saleConfiguration = _saleConfigurationService.Get(editModel.Id, true);
            // چک میکنیم که ایا تا کنون برای این قرارداد و تنظیم فروش فاکتور ایجاد شده است یا خیر
            // اگر ایجاد نشده بود ادامه کار انجام میشود.
            // اگر ایجاد شده بدو به کاربر خطای مناسب نشان داده میشود.
            //var hasAnyFactor = _factorService.CheckExistenceFactorForSaleConfiguration(saleConfiguration.Id);
            // اگر HasTaxCertificate آن تغییر کرده بود
            //if (saleConfiguration.HasTaxCertificate != editModel.HasTaxCertificate)
            //{
            //    if (hasAnyFactor)
            //    {
            //        return
            //            "کاربر گرامی، برای این تنظیم فروش، فاکتور ثبت شده است و شما نمی توانید گواهی ارزش افزوده آن را تغییر دهید";
            //    }
            //}


            // TODO Fix This Remove From ProductSalePlanning ******
            //if (saleConfiguration.OtherDeductionsPercentage != editModel.OtherDeductionsPercentage)
            //{
            //    if (hasAnyFactor)
            //    {
            //        return "کاربر گرامی، برای این تنظیم فروش، فاکتور ثبت شده است و شما نمی توانید درصد سایر کسورات آن را تغییر دهید";
            //    }
            //}

            if (!editModel.IsGroupedByTaxCategory)
            {
                var isValidAllMainCategories = _categoryService.CheckAllMainCategoriesHasUnitOfMeasurementAndCode();
                if (!isValidAllMainCategories)
                {
                    return
                        "کاربر گرامی، همه دسته بندی های اصلی سایت باید دارای مقدار واحد سنجش و کد کالا باشند لطفا ابتدا آنها را بررسی کنید.";
                }
            }
            else
            {
                var isValid = CheckHasValueUnitOfMeasurementAndCode(editModel);
                if (!isValid)
                {
                    return
                        "کاربر گرامی، مقادیر نام کالا و واحد سنجش و کد کالا را برای مالیات و معاف از مالیات کانورت وارد نمایید.";
                }
            }

            if (await _productSalesPlanningService.CheckExistProductSalePlanningBySaleConfigurationId(editModel.Id)
                && (saleConfiguration.CompanyId != editModel.CompanyId ||
                    saleConfiguration.RepresentationId != editModel.RepresentationId)
               )
            {
                return "کاربر گرامی، برای این تنظیم فروش برنامه ریزی دوره تعریف شده است امکان تغییر مرکز یا نمایندگی را ندارید.";
            }

            //در صورتی که تمامی ولیدیشن ها چک شده و درست بودند برای بروزرسانی تنظیم فروش
            //ابتدا تنظیم فروش قبلی (قبل از بروزرسانی) را یک بار به عنوان لاگ در سیستم ذخیره می کنیم
            var exception = new Exception("SaleConfiguration Updated");
            //تبدیل کردن آبجکت salseConfiguration به جی سون 
            var serializedSaleConfiguration = JsonSerializer.Serialize(saleConfiguration, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            // TODO بررسی دلیل این وجود قسمت
            //var taxCertificateValidationMessageResult = await _saleConfigurationService
            //    .CheckAllSalesConfigurationHasTheSameTaxCertificateConfigAsync(editModel.CompanyId,
            //        editModel.HasTaxCertificate,editModel.Id);

            //if (!string.IsNullOrEmpty(taxCertificateValidationMessageResult))
            //    return taxCertificateValidationMessageResult;


            //لاگ گرفتن از مقدار تنظیم قرار داد قبلی
            _logger.InsertLog(LogLevel.Information, (int)Events.Update, nameof(SaleConfiguration),
                serializedSaleConfiguration, exception);

            await _activityLogService.AddActivityLogAsync(
                _userPrincipal.Id,
                ControllerContext.ActionDescriptor.ControllerName
                    .Replace("Controller", ""),
                ControllerContext.ActionDescriptor.ActionName,
                saleConfiguration.Id,
                serializedSaleConfiguration,
                "به روز رسانی تنظم فروش"
            );

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(SaleConfigurationEditModel editModel)
        {
            if (!editModel.IsGroupedByTaxCategory)
            {
                var isValidAllMainCategories = _categoryService.CheckAllMainCategoriesHasUnitOfMeasurementAndCode();
                if (!isValidAllMainCategories)
                {
                    return "کاربر گرامی، همه دسته بندی های اصلی سایت باید دارای مقدار واحد سنجش و کد کالا باشند لطفا ابتدا آنها را بررسی کنید.";
                }
            }
            else
            {
                var isValid = CheckHasValueUnitOfMeasurementAndCode(editModel);
                if (!isValid)
                {
                    return "کاربر گرامی، مقادیر نام کالا و واحد سنجش و کد کالا را برای مالیات و معاف از مالیات کانورت وارد نمایید..";
                }
            }
            // TODO بررسی دلیل این وجود قسمت
            //var taxCertificateValidationMessageResult = await _saleConfigurationService
            //    .CheckAllSalesConfigurationHasTheSameTaxCertificateConfigAsync(editModel.CompanyId,
            //     editModel.HasTaxCertificate);

            //if (!string.IsNullOrEmpty(taxCertificateValidationMessageResult))
            //    return taxCertificateValidationMessageResult;

            return await base.CustomValidateBeforeInsertAsync(editModel);
        }
        /// <summary>
        /// در صورتی که کاربر فیلد کانورت بر اساس دسته بندی مالیاتی را انتخاب کرده باشد
        /// باید مقادیر مربوط به نام کالا  -  کدکالا - واحدسنجش را بامالیات  و معاف از مالیات مقدار دارند   
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        private bool CheckHasValueUnitOfMeasurementAndCode(SaleConfigurationEditModel editModel)
        {
            return !string.IsNullOrEmpty(editModel.TaxCodeForFactor) &&
                   !string.IsNullOrEmpty(editModel.TaxExemptCodeForFactor) &&
                   !string.IsNullOrEmpty(editModel.TaxTitleForFactor) &&
                   !string.IsNullOrEmpty(editModel.TaxExemptTitleForFactor) &&
                   !string.IsNullOrEmpty(editModel.TaxUnitOfMeasurementForFactor) &&
                   !string.IsNullOrEmpty(editModel.TaxExemptUnitOfMeasurementForFactor);
        }

        /// <summary>
        /// تنظیم فروش فعال را برمی گرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        [HttpGet("GetActiveOldSaleConfigurations")]
        public async Task<SaleConfigurationCustomViewModel> GetActiveOldSaleConfigurations(int companyId, int? representationId)
        {
            var saleConfigurationService = await _saleConfigurationService.GetActiveSaleConfigurationAsync(companyId, representationId);
            var hekmatCustomerClubId = await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubsHekmatCustomerClubId);
            var result = _mapper.Map<SaleConfiguration, SaleConfigurationCustomViewModel>(saleConfigurationService);
            if (result != null)
            {
                result.DiscountPercentage = await _salesConfigurationAttributeService.GetByCustomFilter(
                        null,
                        null,
                        result.Id,
                        hekmatCustomerClubId)
                    .Where(x => x.CustomerClubId == hekmatCustomerClubId
                                && x.SaleConfiguration.IsActive)
                    .Select(x => x.DiscountPercentage)
                    .FirstOrDefaultAsync();
            }
            return result;
        }

        [HttpGet("GetApplicationDefaultIsGroupedByTaxCategory")]
        public async Task<bool> GetApplicationDefaultIsGroupedByTaxCategory()
        {
            return await _appConfigService.GetBooleanValueAsync("YashilCommerce.Factor.Convert.GroupByTaxCategory");
        }
    }
}