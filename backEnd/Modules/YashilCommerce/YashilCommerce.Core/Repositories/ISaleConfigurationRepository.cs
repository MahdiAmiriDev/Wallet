using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Repositories
{
    public interface ISaleConfigurationRepository : IGenericRepository<SaleConfiguration, int>
    {
        IQueryable<SaleConfiguration> GetByCustomFilter(int? representationId, int? companyId);
        /// <summary>
        /// عیر فعال کردن تنظیم فروش های قبلی به جز تنظیم فروش فعلی که در پارامتر ارسال شده است
        /// </summary>
        /// <param name="companyId">the companyId of sale configuration</param>
        /// <param name="representationId">the representationId of sale configuration</param>
        Task DeactivateOldSaleConfigurationsAsync(int companyId, int? representationId, int currentSaleConfigurationId);

        List<SaleConfiguration> GetRepresentationSaleConfigurationByShoppingCartCode(Guid shoppingCartCode);
        List<SaleConfiguration> GetCompanySaleConfigurationByShoppingCartCode(Guid shoppingCartCode);
        Task<SaleConfiguration> GetSaleConfigurationAsync(int productSalesPlanningId, bool readOnly = false);
        Task<List<SaleConfigurationAttributeDto>> GetSaleConfigurationAttributeByOrderIdAsync(int orderId,
            int customerClubId);
        SaleConfiguration GetUserActiveSaleConfiguration(int userId);
        SaleConfiguration GetLastSaleConfigurationByDate(int userId);
        /// <summary>
        /// تنظیم فروش فعال را برمیگرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <returns></returns>
        Task<SaleConfiguration> GetActiveSaleConfigurationAsync(int companyId, int? representationId);

        Task<List<SaleConfigurationAttributeDto>> GetSaleConfigurationAttributeByShoppingCartCodeAsync(int customerClubId,
            Guid shoppingCartCode, int applicationId);

        Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeAsync(int customerClubId,
            int productSalesPlanningId);

        Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeByFilterAsync(int customerClubId, int companyId, int? representationId);

        Task<SaleConfigurationAttributeDto> GetSaleConfigurationAttributeBySaleConfigurationIdAsync(int customerClubId, int saleConfigurationId);
        IQueryable<SaleConfiguration> FindById(int saleConfigurationId, bool readOnly = true);
    }
}