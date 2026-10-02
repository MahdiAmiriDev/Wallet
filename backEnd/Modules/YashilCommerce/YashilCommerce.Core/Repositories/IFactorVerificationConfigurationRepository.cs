using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
	public interface IFactorVerificationConfigurationRepository : IGenericRepository<FactorVerificationConfiguration,int>
    {
        /// <summary>
        /// check is there any active factorVerificationConfiguration for a appropriate user based on applicationId
        /// if exist return true 
        /// </summary>
        /// <returns></returns>
        bool CheckExistenceActiveFactorVerificationConfiguration();
        /// <summary>
        /// get the active factorVerification based on applicationId Include Roles
        /// </summary>
        /// <returns></returns>
        FactorVerificationConfiguration GetActiveFactorVerificationConfigurationIncludeRoles();
        /// <summary>
        /// get the active factorVerification based on applicationId
        /// </summary>
        /// <returns></returns>
        FactorVerificationConfiguration GetActiveFactorVerificationConfiguration();

        bool CheckExistenceActiveFactorVerification();

        /// <summary>
        /// تمامی تنظیمات بررسی فاکتور فعال قبلی را غیرفعال میکند
        /// </summary>
        void DeactivateOldFactorVerificationConfigurations();
    }
}      
 