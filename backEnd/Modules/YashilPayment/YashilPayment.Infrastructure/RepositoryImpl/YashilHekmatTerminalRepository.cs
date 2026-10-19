using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    public class YashilHekmatTerminalRepository : GenericApplicationBasedRepository<YashilHekmatTerminal, int>, IYashilHekmatTerminalRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public YashilHekmatTerminalRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<YashilHekmatTerminal> GetByCustomFilter(int? hekmatTerminalId, int? merchantId)
        {
            var query = GetAll(true);
            if (hekmatTerminalId.HasValue)
            {
                query = query.Where(x => x.HekmatTerminalId == hekmatTerminalId.Value);
            }
            if (merchantId.HasValue)
            {
                query = query.Where(x => x.MerchantId == merchantId.Value);
            }
            return query;
        }

        public IQueryable<YashilHekmatTerminal> GetYashilHekmatTerminalByIdAndIp(int merchantId, int terminalId, string ipAddress, int applicationId)
        {
            return GetAll(true, applicationId).Where(x =>
                x.MerchantId == merchantId &&
                x.Id == terminalId &&
                (string.IsNullOrEmpty(x.IpAddress) || x.IpAddress == ipAddress));
        }
    }
}
