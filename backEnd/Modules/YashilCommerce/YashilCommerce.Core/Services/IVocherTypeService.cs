using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IVocherTypeService : IGenericService<VocherType, int>
    {
        public IQueryable<VocherType> GetByCustomFilter(int? companyId, bool readOnly = false);


        /// <summary>
        /// انواع ووچر های مربوط به یک مرکز را بر می گرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        IQueryable<VocherType> GetVocherTypesByCompanyId(int companyId);
        /// <summary>
        /// نوع ووچر برای یک مرکز را بر می گرداند
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <returns></returns>
        Task<bool> IsExistVoucherTypeForCompanyAsync(int companyId);
        /// <summary>
        ///  لیست تمامی کوپن های مربوط به نوع مشخص شده را برمیگرداند 
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        IQueryable<Vocher> GetVochersList(int vocherTypeId);
        /// <summary>
        /// حذف کوپن های تخصیص نیافته یک نوع مشخص 
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        Task<int> DeleteNotAssignedVochersAsync(int vocherTypeId);
    }
}      
 