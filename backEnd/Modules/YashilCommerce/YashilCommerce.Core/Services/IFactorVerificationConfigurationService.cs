using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IFactorVerificationConfigurationService : IGenericService<FactorVerificationConfiguration, int>
    {
        bool CheckExistenceActiveFactorVerificationConfiguration();

        /// <summary>
        /// get the active factorVerification based on applicationId
        /// </summary>
        /// <returns></returns>
        FactorVerificationConfiguration GetActiveFactorVerificationConfiguration();

        bool CheckExistenceActiveFactorVerification();

        /// <summary>
        /// تمامی تنظیمات بررسی فاکتور قبلی را غیر فعال میکند.
        /// </summary>
        void DeactivateOldFactorVerificationConfigurations();
    }
}