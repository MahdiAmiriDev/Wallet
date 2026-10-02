using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilCommerce.Core.Repositories
{
    public interface IVocherTypeRepository : IGenericRepository<VocherType, int>
    {
        public IQueryable<VocherType> GetByCustomFilter(int? companyId, bool readOnly = false);
        /// <summary>
        /// انواع ووچر های مربوط به یک مرکز را بر می گرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        IQueryable<VocherType> GetVocherTypesByCompanyId(int companyId);
    }
}      
 