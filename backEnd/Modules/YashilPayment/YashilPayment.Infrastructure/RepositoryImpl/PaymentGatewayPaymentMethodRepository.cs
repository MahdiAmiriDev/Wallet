			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 

using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
	public class PaymentGatewayPaymentMethodRepository : GenericRepository<PaymentGatewayPaymentMethod,int>, IPaymentGatewayPaymentMethodRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public PaymentGatewayPaymentMethodRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<PaymentGatewayPaymentMethod> GetByCustomFilter( int? paymentGatewayId, int? paymentMethodType)
        {
            var query= GetAll(true);
                         if ( paymentGatewayId.HasValue)
                {
                    query = query.Where(x => x.PaymentGatewayId == paymentGatewayId.Value);
                }
                          if ( paymentMethodType.HasValue)
                {
                    query = query.Where(x => x.PaymentMethodType == paymentMethodType.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
