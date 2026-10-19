using System.Linq;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    public class HybridPaymentRepository : GenericApplicationBasedRepository<HybridPayment, int>,
        IHybridPaymentRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public HybridPaymentRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<HybridPayment> GetByCustomFilter(int? hybridPaymentRequestId, int? hybridPaymentStatusType,
            int? paymentMethodType, int? applicationId)
        {
            var query = GetAll(true, applicationId);
            if (hybridPaymentRequestId.HasValue)
            {
                query = query.Where(x => x.HybridPaymentRequestId == hybridPaymentRequestId.Value);
            }

            if (hybridPaymentStatusType.HasValue)
            {
                query = query.Where(x => x.HybridPaymentStatusType == hybridPaymentStatusType.Value);
            }

            if (paymentMethodType.HasValue)
            {
                query = query.Where(x => x.PaymentMethodType == paymentMethodType.Value);
            }


            return query;
        }

        public IQueryable<HybridPayment> FindById(int id, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.Id == id);
            return readOnly ? query.AsNoTracking() : query;
        } 
        public IQueryable<HybridPayment> FindByReservationNumber(string reservationNumber, bool readOnly = false)
        {
            var query = DbSet.Where(x => x.ReservationNumber == reservationNumber);
            return readOnly ? query.AsNoTracking() : query;
        }
    }
}