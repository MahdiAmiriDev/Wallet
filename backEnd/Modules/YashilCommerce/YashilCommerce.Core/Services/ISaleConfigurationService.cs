using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface ISaleConfigurationService : IGenericService<SaleConfiguration, int>
    {
        IQueryable<SaleConfiguration> GetByCustomFilter(int? representationId, int? companyId);

        /// <summary>
        /// deactivating old configurations 
        /// </summary>
        /// <param name="companyId">the companyId of sale configuration</param>
        /// <param name="representationId">the representationId of sale configuration</param>
        Task DeactivateOldSaleConfigurationsAsync(int companyId, int? representationId, int currentSaleConfigurationId);


        List<SaleConfiguration> GetRepresentationSaleConfigurationByShoppingCartCode(Guid shoppingCartCode);
        List<SaleConfiguration> GetCompanySaleConfigurationByShoppingCartCode(Guid shoppingCartCode);

        Task<List<SaleConfigurationAttributeDto>> GetSaleConfigurationAttributeByShoppingCartCodeAsync(Guid shoppingCartCode,
            int applicationId);

        /// <summary>
        /// تنظیمات فروش را برای نمایندگی در صورتی که مقدار داشته باشد برمی گرداند 
        /// اگر برنامه ریزی فروش نمایندگی نداشت ، تنظیم فروش کمپانی را بر می گرداند
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Task<SaleConfiguration> GetSaleConfigurationAsync(int productSalesPlanningId, bool readOnly = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">شفارش</param>
        /// <param name="orderUserId">شناسه کاربر سفارش</param>
        /// <returns></returns>
        Task<List<SaleConfigurationAttributeDto>> GetSaleConfigurationAttributeByOrderIdAsync(int orderId, int orderUserId);
        SaleConfiguration GetUserActiveSaleConfiguration(int userId);
        SaleConfiguration GetLastSaleConfigurationByDate(int userId);
        /// <summary>
        /// تنظیم فروش فعال را برمیگرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        Task<SaleConfiguration> GetActiveSaleConfigurationAsync(int companyId, int? representationId);


        Task<int?> GetSaleConfigurationInstallmentCountAsync(decimal finalPrice,
            SaleConfigurationAttributeDto saleConfiguration);

        Task<int?> GetSaleConfigurationsInstallmentCountAsync(List<ProductSalesPlanningPriceDto> finalPrice,
            List<SaleConfigurationAttributeDto> saleConfigurations);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productSalePlanningPrice"></param>
        /// <param name="saleConfigurations"></param>
        /// <returns>در صورتی که مقدار این متد نادرست باشد یعنی تعداد اقساط آیتم ها متفاوت می باشد</returns>
        Task<bool> CheckAllSaleConfigurationsHasSameInstallmentCountAsync(List<ProductSalesPlanningPriceDto> productSalePlanningPrice,
            List<SaleConfigurationAttributeDto> saleConfigurations);

        Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeByCurrentUserCustomerClubAsync(int productSalesPlanningId);
        Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeAsync(int productSalesPlanningId, int customerClubId);

        Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeByFilterAsync(int customerClubId, int companyId, int? representationId);

        /// <summary>
        /// قیمت نهایی سفارش را می گیرد و اقساط را محاسبه می کند
        /// </summary>
        /// <param name="finalPrice">قیمت نهایی محاسبه شده</param>
        /// <param name="saleConfigurations">تنظیمات فروش آیتم های سفارش</param>
        /// <returns></returns>
        Task<int?> GetSaleConfigurationsInstallmentCountAsync(decimal finalPrice,
            List<SaleConfigurationAttributeDto> saleConfigurations);

        /// <summary>
        /// بررسی می کند که آیا این تنظیمات فروش متناسبی دارند یا نه مثلا همگی نقدی یا اقساطی را پشتیبانی کنند 
        /// </summary>
        /// <param name="saleConfigurationAttributes"></param>
        /// <returns></returns>
        bool CheckSaleConfigurationAttributeSaleTypes(List<SaleConfigurationAttributeDto> saleConfigurationAttributes);

        IQueryable<SaleConfiguration> FindById(int saleConfigurationId, bool readOnly = true);

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
        Task<string> CheckAllSalesConfigurationHasTheSameTaxCertificateConfigAsync(int companyId, bool hasTaxCertificate,int? saleConfigurationId = null);
    }


}