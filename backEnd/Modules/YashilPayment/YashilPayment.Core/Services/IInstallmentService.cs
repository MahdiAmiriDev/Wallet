
			using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
            using System.Collections.Generic;

namespace YashilPayment.Core.Services
{
	public interface IInstallmentService : IGenericService<Installment,int>
    {
                 
        IQueryable<Installment> GetByCustomFilter( int? paymentId, int? paymentStatus);

        List<Installment> GetInstallmentByPaymentId(int paymentId);
    }
}      
 