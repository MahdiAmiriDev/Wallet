using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Services
{
    public interface IMerchantInfoService : IGenericService<MerchantInfo, int>
    {

        IQueryable<MerchantInfo> GetByCustomFilter(int? cityId, int? userId);

        MerchantInfo GetMerchantInfoByIdAndIp(int merchantId, int terminalId, string ip);
        /// <summary>
        /// دریافت پذیرنده کاربر
        /// درضورتی که کاربر لاگین کرده دارای یک پذیرنده باشد آن را بر میگرداند 
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        /// <returns></returns>
        IQueryable<MerchantInfo> GetUserMerchant(int userId);

       
    }
}
