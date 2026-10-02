

using System.Threading.Tasks;

namespace YashilCommerce.Core.Interfaces
{
    public interface IMaktabKhoneApiService
    {
        /// <summary>
        /// این متد برای افزودن کابر به یکی از پلن های مکتب خونه کاربرد دارد
        /// </summary>
        /// <param name="userMobileNumber">شماره موبایل کاربر</param>
        /// <param name="planId">شناسه برنامه</param>
        Task<(bool isSuccess, string message, int subscriptionId)> AddUserToSubscriptionPlanAsync(string userMobileNumber);

        /// <summary>
        /// این متد جهت دریافت
        /// AuthenticationToken
        /// مربوط به وبسایت مکتب خونه کاربرد دارد
        /// </summary>
        /// <param name="userMobileNumber"></param>
        /// <returns></returns>

        Task<(bool isSuccess, string message, string redirectUrl)> GetAuthenticationToken(string userMobileNumber);
    }
}
