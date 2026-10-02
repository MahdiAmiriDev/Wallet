
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IPreRegistrationService : IGenericService<PreRegistration, int>
    {

        IQueryable<PreRegistration> GetByCustomFilter(int? userId, int? productSalesPlanningId);

        /// <summary>
        /// بررسی میکند که برای ایدی یوزر داده شده و برنامه ریزی دوره داده شده
        /// کاربر قبلا پیش ثبت نام انجام داده است یا خیر
        /// </summary>
        /// <param name="editModelProductSalesPlanningId"></param>
        /// <param name="userPrincipalId"></param>
        /// <returns></returns>
        Task<bool> CheckUserIsPreRegisteredAsync(int editModelProductSalesPlanningId, int userPrincipalId);
        Task<string> PublicUserPreRegisterAsync(int productSalesPlanningId, int userId,int nationalCode);
    }
}
