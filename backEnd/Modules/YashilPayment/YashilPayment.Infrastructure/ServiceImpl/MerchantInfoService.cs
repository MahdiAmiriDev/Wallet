using System.Linq;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class MerchantInfoService : GenericService<MerchantInfo, int>, IMerchantInfoService
    {
        private readonly IMerchantInfoRepository _merchantInfoRepository;

        public MerchantInfoService(IUnitOfWork unitOfWork, IMerchantInfoRepository merchantInfoRepository, IUserPrincipal userPrincipal, IEventPublisher eventPublisher)
            : base(unitOfWork, merchantInfoRepository, userPrincipal, eventPublisher)
        {
            _merchantInfoRepository = merchantInfoRepository;
        }

        public IQueryable<MerchantInfo> GetByCustomFilter(int? cityId, int? userId)
        {
            return _merchantInfoRepository.GetByCustomFilter(cityId, userId);
        }

        public MerchantInfo GetMerchantInfoByIdAndIp(int merchantId, int terminalId, string ip)
        {
            return _merchantInfoRepository.GetMerchantInfoByIdAndIp(merchantId, terminalId, ip).SingleOrDefault();
        }
        /// <summary>
        /// دریافت پذیرنده کاربر
        /// درضورتی که کاربر لاگین کرده دارای یک پذیرنده باشد آن را بر میگرداند 
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        public IQueryable<MerchantInfo> GetUserMerchant(int userId)
        {
            return _merchantInfoRepository.GetUserMerchant(userId);
        }

       
    }
}
