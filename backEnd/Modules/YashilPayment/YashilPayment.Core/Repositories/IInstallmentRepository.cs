			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities; 

namespace YashilPayment.Core.Repositories
{
	public interface IInstallmentRepository : IGenericRepository<Installment,int>
    {
        IQueryable<Installment> GetByCustomFilter( int? paymentId, int? paymentStatus);

        IQueryable<Installment> GetInstallmentByPaymentId(int paymentId);
    }
}      
 