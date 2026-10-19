using Microsoft.EntityFrameworkCore;
using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    public class MerchantInfoRepository : GenericApplicationBasedRepository<MerchantInfo, int>, IMerchantInfoRepository
    {
        public MerchantInfoRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
        }


        public IQueryable<MerchantInfo> GetByCustomFilter(int? cityId, int? userId)
        {
            var query = GetAll(true).AsNoTracking();
            if (cityId.HasValue)
            {
                query = query.Where(x => x.CityId == cityId.Value);
            }

            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId);
            }
            return query;
        }

        public IQueryable<MerchantInfo> GetMerchantInfoByIdAndIp(int merchantId, int terminalId, string ip)
        {
            return DbSet.Where(x => x.Id == merchantId && x.MerchantTerminalInfo.Any(u => u.Id == terminalId && u.Ip == ip)).Include(x => x.MerchantTerminalInfo);
        }
        /// <summary>
        /// دریافت پذیرنده کاربر
        /// درضورتی که کاربر لاگین کرده دارای یک پذیرنده باشد آن را بر میگرداند 
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        public IQueryable<MerchantInfo> GetUserMerchant(int userId)
        {
            return DbSet.Where(x => x.UserId == userId);
        }
    }
}
