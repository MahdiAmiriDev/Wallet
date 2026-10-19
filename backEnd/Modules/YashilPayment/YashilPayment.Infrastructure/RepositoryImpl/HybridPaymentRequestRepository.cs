using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    public class HybridPaymentRequestRepository : GenericApplicationBasedRepository<HybridPaymentRequest, int>,
        IHybridPaymentRequestRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public HybridPaymentRequestRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<HybridPaymentRequest> GetByCustomFilter(int? merchantId, int? yashilHekmatTerminalId,
            int? hybridPaymentRequestStatusType, string localReservationNumber, int? applicationId)
        {
             var query = GetAll(true, applicationId);
            
            if (merchantId.HasValue)
            {
                query = query.Where(x => x.MerchantId == merchantId.Value);
            }

            if (yashilHekmatTerminalId.HasValue)
            {
                query = query.Where(x => x.YashilHekmatTerminalId == yashilHekmatTerminalId.Value);
            }

            if (hybridPaymentRequestStatusType.HasValue)
            {
                query = query.Where(x => x.HybridPaymentRequestStatusType == hybridPaymentRequestStatusType.Value);
            }

            if (!string.IsNullOrEmpty(localReservationNumber))
            {
                query = query.Where(x => x.LocalReservationNumber == localReservationNumber);
            }


            return query;
        }

        public IQueryable<HybridPaymentRequest> FindById(int id, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.Id == id);
            return readOnly ? query.AsNoTracking() : query;
        }
    }
}