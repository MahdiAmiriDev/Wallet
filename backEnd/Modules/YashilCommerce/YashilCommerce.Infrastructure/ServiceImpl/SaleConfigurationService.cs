using System;
using System.Collections.Generic;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Enums;
using YashilCommerce.Core.DTO;
using Order = Yashil.Core.Entities.Order;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class SaleConfigurationService : GenericService<SaleConfiguration, int>, ISaleConfigurationService
    {
        private readonly ISaleConfigurationRepository _saleConfigurationRepository;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly IYashilLogger<Order> _logger;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;

        public SaleConfigurationService(IUnitOfWork unitOfWork, IProductSalesPlanningService productSalesPlanningService,
            ISaleConfigurationRepository saleConfigurationRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
            IYashilLogger<Order>
            logger, IAdditionalUserPropService additionalUserPropService)
            : base(unitOfWork, saleConfigurationRepository, userPrincipal, eventPublisher)
        {
            _saleConfigurationRepository = saleConfigurationRepository;
            _userPrincipal = userPrincipal;
            _logger = logger;
            _additionalUserPropService = additionalUserPropService;
            _productSalesPlanningService = productSalesPlanningService;
        }



        public IQueryable<SaleConfiguration> GetByCustomFilter(int? representationId, int? companyId)
        {
            return _saleConfigurationRepository.GetByCustomFilter(representationId, companyId);
        }

        public async Task DeactivateOldSaleConfigurationsAsync(int companyId, int? representationId, int currentSaleConfigurationId)
        {
            await _saleConfigurationRepository.DeactivateOldSaleConfigurationsAsync(companyId, representationId, currentSaleConfigurationId);
        }

        public List<SaleConfiguration> GetRepresentationSaleConfigurationByShoppingCartCode(Guid shoppingCartCode)
        {
            return _saleConfigurationRepository.GetRepresentationSaleConfigurationByShoppingCartCode(shoppingCartCode);
        }

        public List<SaleConfiguration> GetCompanySaleConfigurationByShoppingCartCode(Guid shoppingCartCode)
        {
            return _saleConfigurationRepository.GetCompanySaleConfigurationByShoppingCartCode(shoppingCartCode);

        }

        #region SaleConfigurationAttributeDto
        public async Task<int?> GetSaleConfigurationsInstallmentCountAsync(
            List<ProductSalesPlanningPriceDto> productSalesPlanningPrices,
            List<SaleConfigurationAttributeDto> saleConfigurationAttributeDtoList)
        {
            int? saleConfigurationInstallmentCount = null;
            if (saleConfigurationAttributeDtoList.Count > 0)
            {
                foreach (var saleConfiguration in saleConfigurationAttributeDtoList)
                {

                    var totalCalcPrice = productSalesPlanningPrices
                        .Where(x => x.SaleConfigurationId == saleConfiguration.SaleConfigurationId)
                        .Sum(x => x.BasePrice * (1 - saleConfiguration.DiscountPercentage / 100));

                    if (totalCalcPrice != null)
                    {
                        var installmentCount = await GetSaleConfigurationInstallmentCountAsync(totalCalcPrice.Value, saleConfiguration);

                        if (saleConfigurationInstallmentCount.HasValue)
                        {
                            if (installmentCount != saleConfigurationInstallmentCount)
                            {
                                return null;
                            }
                        }
                        saleConfigurationInstallmentCount = installmentCount;
                    }
                }
            }
            return saleConfigurationInstallmentCount;
        }

        public async Task<List<SaleConfigurationAttributeDto>> GetSaleConfigurationAttributeByShoppingCartCodeAsync(
            Guid shoppingCartCode,
            int applicationId)
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            return await _saleConfigurationRepository.GetSaleConfigurationAttributeByShoppingCartCodeAsync(userAdditionalUserProp.CustomerClubId, shoppingCartCode, applicationId);
        }
        public async Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeByCurrentUserCustomerClubAsync(
            int productSalesPlanningId)
        {
            var customerClubId = (await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id)).CustomerClubId;
            return await _saleConfigurationRepository.GetSaleConfigurationAttributeAsync(customerClubId, productSalesPlanningId);
        }

        public async Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeAsync(int productSalesPlanningId, int customerClubId)
        {
            return await _saleConfigurationRepository.GetSaleConfigurationAttributeAsync(customerClubId, productSalesPlanningId);
        }

        public async Task<List<SaleConfigurationAttributeDto>> GetSaleConfigurationAttributeByOrderIdAsync(int orderId, int orderUserId)
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(orderUserId);

            return await _saleConfigurationRepository.GetSaleConfigurationAttributeByOrderIdAsync(orderId, userAdditionalUserProp.CustomerClubId);
        }



        public async Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeByFilterAsync(int customerClubId, int companyId, int? representationId)
        {
            return await _saleConfigurationRepository.GetSaleConfigurationAttributeByFilterAsync(customerClubId, companyId, representationId);
        }

        public async Task<int?> GetSaleConfigurationsInstallmentCountAsync(decimal finalPrice,
           List<SaleConfigurationAttributeDto> saleConfigurations)
        {
            int? saleConfigurationInstallmentCount = null;

            foreach (var saleConfiguration in saleConfigurations)
            {
                var installmentCount = await GetSaleConfigurationInstallmentCountAsync(finalPrice, saleConfiguration);

                if (saleConfigurationInstallmentCount.HasValue)
                {
                    if (installmentCount < saleConfigurationInstallmentCount)
                    {
                        saleConfigurationInstallmentCount = installmentCount;
                    }
                }
                else
                {
                    saleConfigurationInstallmentCount = installmentCount;
                }
            }

            return saleConfigurationInstallmentCount;
        }

        public bool CheckSaleConfigurationAttributeSaleTypes(List<SaleConfigurationAttributeDto> saleConfigurationAttributes)
        {
            return saleConfigurationAttributes.GroupBy(x => x.SaleType).Count() == 1 ||
                   saleConfigurationAttributes.All(x =>
                       x.SaleType == (int)SaleTypesEnum.CashPayment ||
                       x.SaleType == (int)SaleTypesEnum.CashPaymentAndInstallmentPayment) ||
                   saleConfigurationAttributes.All(x =>
                       x.SaleType == (int)SaleTypesEnum.InstallmentPayment ||
                       x.SaleType == (int)SaleTypesEnum.CashPaymentAndInstallmentPayment);

        }

        public IQueryable<SaleConfiguration> FindById(int saleConfigurationId, bool readOnly = true)
        {
            return _saleConfigurationRepository.FindById(saleConfigurationId, readOnly);
        }

        public async Task<bool> CheckAllSaleConfigurationsHasSameInstallmentCountAsync(
            List<ProductSalesPlanningPriceDto> productSalePlanningPrice,
            List<SaleConfigurationAttributeDto> saleConfigurations)
        {
            var installmentCount = await GetSaleConfigurationsInstallmentCountAsync(productSalePlanningPrice, saleConfigurations);
            return installmentCount.HasValue;
        }


        public async Task<int?> GetSaleConfigurationInstallmentCountAsync(decimal finalPrice,
            SaleConfigurationAttributeDto saleConfiguration)
        {
            if (saleConfiguration.ExpressionBasedInstallmentCount)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(saleConfiguration.InstallmentExpression))
                    {
                        // throw new Exception("Fix This");
                        var res = await CSharpScript.EvaluateAsync<int>(string.Format(saleConfiguration.InstallmentExpression, finalPrice));
                        {
                            return res;
                        }
                    }
                    _logger.InsertLog(LogLevel.Error, new EventId(1, Events.GetEntityException.ToString()), nameof(Order),
                        "متد GetSaleConfigurationInstallmentCountAsync -> saleConfiguration.InstallmentExpression بدرستی وارد نشده است",
                        null);
                    {
                        return null;

                    }
                }
                catch (Exception e)
                {
                    _logger.InsertLog(LogLevel.Error, new EventId(1, Events.GetEntityException.ToString()), nameof(Order),
                        "متد GetOrderInstallmentCount -> saleConfiguration.InstallmentExpression بدرستی وارد نشده است", e);
                    {
                        return null;
                    }
                }
            }

            return saleConfiguration.InstallmentCount;
        }
        #endregion
        public override async Task AfterSaveChangesAsync(int commitAsync)
        {
            await _productSalesPlanningService.UpdateProductSalesPlanningCalculatedPriceAsync();
        }

        /// <summary>
        /// تنظیمات فروش را برای نمایندگی در صورتی که مقدار داشته باشد برمی گرداند 
        /// اگر برنامه ریزی فروش نمایندگی نداشت ، تنظیم فروش کمپانی را بر می گرداند
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public Task<SaleConfiguration> GetSaleConfigurationAsync(int productSalesPlanningId, bool readOnly = false)
        {
            return _saleConfigurationRepository.GetSaleConfigurationAsync(productSalesPlanningId, readOnly);
        }


        public SaleConfiguration GetUserActiveSaleConfiguration(int userId)
        {
            return _saleConfigurationRepository.GetUserActiveSaleConfiguration(userId);
        }

        public SaleConfiguration GetLastSaleConfigurationByDate(int userId)
        {
            return _saleConfigurationRepository.GetLastSaleConfigurationByDate(userId);
        }
        /// <summary>
        /// تنظیم فروش فعال را برمیگرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        public Task<SaleConfiguration> GetActiveSaleConfigurationAsync(int companyId, int? representationId)
        {
            return _saleConfigurationRepository.GetActiveSaleConfigurationAsync(companyId, representationId);
        }

        /// <summary>
        /// این متد بررسی می کند که تمامی تنظیم فروش های مربوط به یک مرکز
        /// یا باید دارای گزینه فعال برای
        /// hasTaxCertificate
        /// باشند همانند مقداری که الان در حال درج یا ویرایش است
        /// یا هیچ کدام نباید دارای مقدار فعال برای
        /// hasTaxCertificate
        /// باشند همانند مقداری که درحال ویرایش یا درج است
        /// برای یک تنظیم فروش های یک مرکز این مورد را بررسی می کند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="hasTaxCertificate">دارای گواهی مالیات است</param>
        /// <param name="saleConfigurationId">شناسه تنظم فروش</param>
        /// <returns></returns>
        public async Task<string> CheckAllSalesConfigurationHasTheSameTaxCertificateConfigAsync(int companyId, bool hasTaxCertificate, int? saleConfigurationId = null)
        {
            var hasTaxCertificateQuery = GetAll(true)
                .Where(sc => sc.CompanyId == companyId && sc.IsActive && (!saleConfigurationId.HasValue || sc.Id != saleConfigurationId) 
                )
                .Select(x => x.HasTaxCertificate);

            if (await hasTaxCertificateQuery.AllAsync(x => x == hasTaxCertificate))
                return string.Empty;

            return "کاربر گرامی باید تمامی تنظیم فروش های مربوط به مرکز دارای یک حالت برای (دارای گواهی ارزش افزوده) باشند";
        }

    }
}