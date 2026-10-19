using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Repositories
{
    public interface IHybridPaymentRepository : IGenericRepository<HybridPayment, int>
    {
        IQueryable<HybridPayment> GetByCustomFilter(int? hybridPaymentRequestId, int? hybridPaymentStatusType,
            int? paymentMethodType, int? applicationId);


        IQueryable<HybridPayment> FindById(int id, bool readOnly = false);
        IQueryable<HybridPayment> FindByReservationNumber(string reservationNumber, bool readOnly = false);
    }
}