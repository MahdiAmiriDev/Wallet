using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ICertificateRepository : IGenericRepository<Certificate, int>
    {
        /// <summary>
        /// اطلاعات گواهینامه را بر اساس شماره گواهینامه دریافت می کند
        /// </summary>
        /// <param name="certificateNumber">شماره گواهینامه</param>
        /// <returns></returns>
        Task<Certificate> GetByNumberAsync(string certificateNumber);
        /// <summary>
        /// دریافت لیست گواهینامه ها بر اساس فیلتر های ارسالی در پارامتر
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        IQueryable<Certificate> GetByCustomFilter(string nationalCode);
    }
}
