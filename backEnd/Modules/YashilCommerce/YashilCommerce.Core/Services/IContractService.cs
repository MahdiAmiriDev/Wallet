using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IContractService : IGenericService<Contract, int>
    {

        IQueryable<Contract> GetByCustomFilter(int? companyId, int? representationId, int? contractType, int? contractStatus, int? addressId);
        
        /// <summary>
        /// deactivating all old active contract 
        /// </summary>
        /// <param name="companyId">the companyId of contract</param>
        /// <param name="representationId">the representationId of Contract. representationId can be null</param>
        Task DeactivateOldContractsAsync(int companyId, int? representationId,int currentSaleConfigurationId);
        
        /// <summary>
        /// دریافت قرارداد با saleConfiguration
        /// </summary>
        /// <param name="editModelId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Contract GetWithSaleConfiguration(int editModelId, bool readOnly);
       
    }
}