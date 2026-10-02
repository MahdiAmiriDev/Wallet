using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface ISubsidyService : IGenericService<Subsidy, int>
    {
        /// <summary>
        /// یارانه ساتا تایید یا رد میشود
        /// </summary>
        /// <param name="nationalCode">کد ملی خریدار</param>
        /// <param name="identificationCode"></param>
        /// <returns></returns>
        Task<SataSubsidyApprovalResult> TrySataSubsidyApprovalAsync(string nationalCode, string identificationCode = null);
        /// <summary>
        /// سوبسید مربوط به برنامه بر اساس کد اپلیکیشن کاربر
        /// </summary>
        /// <returns></returns>
        Subsidy GetCurrentSubsidy();

        /// <summary>
        /// سوبسید مربوط به برنامه بر اساس کد اپلیکیشن سیستم
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<Subsidy> GetApplicationCurrentSubsidyAsync(int applicationId);
        /// <summary>
        /// تنظیم یارانه قبلی برنامه را غیر فعال می کند 
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        Task DeactivateOldSubsidyByApplicationIdAsync(int applicationId);

        IQueryable<Subsidy> GetByCustomFilter(int? categoryGroupId, int? subsidyGroupId);
        /// <summary>
        /// بررسی می کند که آیا برای گروه دسته بندی در گروه یارانه ، یارانه فعالی وجود دارد یا خیر
        /// </summary>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <returns></returns>
        Task<bool> CheckExistCategoryGroupActiveSubsidyAsync(int categoryGroupId);

        IQueryable<Subsidy> FindById(int id, bool readOnly = true);
        /// <summary>
        ///  بر اساس مصرف یارانه سفارشات تایید شده یا منتظر تایید یا پرداخت ناقص
        ///  اگر یارانه مصرف شده از سقف یارانه در ساتا و وزارت بیشتر مساوی باشد
        ///  یارانه ساتا و وزارت برای هر گروه دسته بندی یارانه
        ///  غیر فعال می گردد
        /// </summary>
        /// <returns></returns>
        Task UpdateSubsidyStatusBySubsidyConsumption();

        Task<Subsidy> GetCategoryGroupActiveSubsidyInfoAsync(int employmentCategoryGroupId);
    }

}
