using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilPayment.Core.Repositories
{
    public interface IHybridPaymentRequestRepository : IGenericRepository<HybridPaymentRequest, int>
    {
        IQueryable<HybridPaymentRequest> GetByCustomFilter(int? merchantId, int? yashilHekmatTerminalId,
            int? hybridPaymentRequestStatusType, string localReservationNumber, int? applicationId);

        IQueryable<HybridPaymentRequest> FindById(int id, bool readOnly= false);
    }
}