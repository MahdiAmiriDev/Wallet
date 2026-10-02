using System;
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
using YashilUserManagement.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Yashil.Core.DefaultContractResolvers;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ContractController : BaseController<Contract, int, ContractListViewModel, ContractEditModel,
        ContractSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IContractService _contractService;
        private readonly ISaleConfigurationService _saleConfigurationService;
        private readonly IAddressService _addressService;
        private readonly ICategoryService _categoryService;
        private readonly IFactorService _factorService;
        private readonly IYashilLogger<Contract> _logger;

        public ContractController(
            IContractService contractService,
            IMapper mapper,
            ISaleConfigurationService saleConfigurationService,
            IAddressService addressService,
            IYashilLogger<Contract> logger,
            IPermissionService permissionService,
            IFactorService factorService,
            ICategoryService categoryService) : base(contractService, mapper,
            logger, permissionService)
        {
            _mapper = mapper;
            _saleConfigurationService = saleConfigurationService;
            _addressService = addressService;
            _factorService = factorService;
            _categoryService = categoryService;
            _contractService = contractService;
            _logger = logger;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? companyId,
            int? representationId, int? taxCategoryId, int? contractType, int? contractStatus, int? addressId)
        {
            var contracts = _contractService.GetByCustomFilter(companyId, representationId, contractType,
                contractStatus, addressId);
            return await DataSourceLoader.LoadAsync(
                contracts.ProjectTo<ContractListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? companyId,
            int? representationId, int? taxCategoryId, int? contractType, int? contractStatus, int? addressId)
        {
            var contracts = _contractService.GetByCustomFilter(companyId, representationId, contractType,
                contractStatus, addressId);
            return await DataSourceLoader.LoadAsync(
                contracts.ProjectTo<ContractSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task CustomMapBeforeUpdateAsync(ContractEditModel editModel, Contract entity)
        {

            await DeActiveOldContractsAsync(editModel);
            if (entity.SaleConfiguration != null)
            {
                await _saleConfigurationService.UpdateAsync(entity.SaleConfiguration, entity.SaleConfiguration.Id);
            }
            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }

        protected override async Task CustomMapBeforeInsertAsync(ContractEditModel editModel, Contract entity)
        {
            //deactivate all old Active contract
            await DeActiveOldContractsAsync(editModel);
            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        /// <summary>
        /// deactivate all old Active contract
        /// </summary>
        /// <param name="editModel"></param>
        private async Task DeActiveOldContractsAsync(ContractEditModel editModel)
        {
            if (editModel.SaleConfiguration.IsActive)
            {
                await _contractService.DeactivateOldContractsAsync(editModel.SaleConfiguration.CompanyId, editModel.SaleConfiguration.RepresentationId,editModel.SaleConfigurationId);
            }
        }
        protected override async Task<string> CustomValidateBeforeUpdateAsync(ContractEditModel editModel)
        {
            // ایتدا قرارداد با تنظیم فروش آن دریافت میشود
            var contract = _contractService.GetWithSaleConfiguration(editModel.Id, true);
            // چک میکنیم که ایا تا کنون برای این قرارداد و تنظیم فروش فاکتور ایجاد شده است یا خیر
            // اگر ایجاد نشده بود ادامه کار انجام میشود.
            // اگر ایجاد شده بدو به کاربر خطای مناسب نشان داده میشود.
            var hasAnyFactor = _factorService.CheckExistenceFactorForSaleConfiguration(contract.SaleConfigurationId);
            // اگر HasTaxCertificate آن تغییر کرده بود
            if (contract.SaleConfiguration.HasTaxCertificate != editModel.SaleConfiguration.HasTaxCertificate)
            {
                if (hasAnyFactor)
                {
                    return
                        "کاربر گرامی، برای این تنظیم فروش فاکتور ثبت شده است و شما نمیتوانید گواهی ارزش افزوده آنرا تغییر دهید.";
                }
            }
            if (!editModel.SaleConfiguration.IsGroupedByTaxCategory)
            {
                var isValidAllMainCategories = _categoryService.CheckAllMainCategoriesHasUnitOfMeasurementAndCode();
                if (!isValidAllMainCategories)
                {
                    return "کاربر گرامی، همه دسته بندی های اصلی سایت باید دارای مقدار واحد سنجش و کد کالا باشند لطفا ابتدا آنها را بررسی کنید.";
                }
            }
            else
            {
                var isValid = CheckHasValueUnitOfMeasurementAndCode(editModel.SaleConfiguration);
                if (!isValid)
                {
                    return "کاربر گرامی، مقادیر نام کالا و واحد سنجش و کد کالا را برای مالیات و معاف از مالیات کانورت وارد نمایید..";
                }
            }
            //در صورتی که تمامی ولیدیشن ها به هنگام بروزرسانی قرار داد درست بود برای بروزرسانی قرار داد 
            //ابتدا قرار داد قبلی را یک بار به عنوان لاگ در سیستم ذخیره می کنیم
            var exception = new Exception("Contract Updated");
            //تبدیل کردن آبجکت contract به جی سون
            //به همراه این تنظیم که پروپرتی salesConfiguration را در فرآیند تبدیل ایگنور کند
            var serializedContract = JsonConvert.SerializeObject(contract, new JsonSerializerSettings
            {
                ContractResolver = new ContractDefaultResolver("SaleConfiguration")
            });
            //گرفتن لاگ مربوط به بروزرسانی قرارداد (قرارداد قبلی را در لاگ ذخیره می کنیم
            _logger.InsertLog(LogLevel.Information, (int)Events.Update, nameof(Contract), serializedContract, exception);


            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(ContractEditModel editModel)
        {
            if (!editModel.SaleConfiguration.IsGroupedByTaxCategory)
            {
                var isValidAllMainCategories = _categoryService.CheckAllMainCategoriesHasUnitOfMeasurementAndCode();
                if (!isValidAllMainCategories)
                {
                    return "کاربر گرامی، همه دسته بندی های اصلی سایت باید دارای مقدار واحد سنجش و کد کالا باشند لطفا ابتدا آنها را بررسی کنید.";
                }
            }
            else
            {
                var isValid = CheckHasValueUnitOfMeasurementAndCode(editModel.SaleConfiguration);
                if (!isValid)
                {
                    return "کاربر گرامی، مقادیر نام کالا و واحد سنجش و کد کالا را برای مالیات و معاف از مالیات کانورت وارد نمایید..";
                }
            }
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
    }
}