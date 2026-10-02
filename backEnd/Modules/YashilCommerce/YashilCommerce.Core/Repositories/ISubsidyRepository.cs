using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface ISubsidyRepository : IGenericRepository<Subsidy, int>
    {
        Subsidy GetCurrentSubsidy();
        Task<Subsidy> GetApplicationCurrentSubsidyAsync(int applicationId);
        /// <summary>
        /// تنظیم یارانه قبلی برنامه را غیر فعال می کند 
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        Task DeactivateOldSubsidyByApplicationIdAsync(int applicationId);

        IQueryable<Subsidy> GetByCustomFilter(int? categoryGroupId, int? subsidyGroupId);
        IQueryable<Subsidy> FindById(int id, bool readOnly = true);
        /// <summary>
        ///  بر اساس مصرف یارانه سفارشات تایید شده یا منتظر تایید یا پرداخت ناقص
        ///  اگر یارانه مصرف شده از سقف یارانه در ساتا و وزارت بیشتر مساوی باشد
        ///  یارانه ساتا و وزارت برای هر گروه دسته بندی یارانه
        ///  غیر فعال می گردد
        /// </summary>
        /// <returns></returns>
        Task UpdateSubsidyStatusBySubsidyConsumption(int applicationId);
        /// <summary>
        /// دریافت یارانه بر اساس گروه دسته بندی
        /// </summary>
        /// <param name="categoryGroupId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Subsidy> GetByCategoryGroupId(int? categoryGroupId, bool readOnly = true);
    }
}
