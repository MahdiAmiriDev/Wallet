using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IContractRepository : IGenericRepository<Contract, int>
    {

        IQueryable<Contract> GetByCustomFilter(int? companyId, int? representationId,  int? contractType, int? contractStatus, int? addressId);

        /// <summary>
        /// get user contract
        /// if not exist return null
        /// </summary>
        /// <returns></returns>
        Contract GetActiveContractUser(int userId);
        /// <summary>
        /// دریافت قرارداد به همراه تنظیم فروش
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        Contract GetWithSaleConfiguration(int contractId, bool readOnly);
        
    }
}