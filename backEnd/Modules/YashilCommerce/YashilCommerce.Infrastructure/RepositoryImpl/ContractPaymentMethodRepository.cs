			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class ContractPaymentMethodRepository : GenericApplicationBasedRepository<ContractPaymentMethod,int>, IContractPaymentMethodRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public ContractPaymentMethodRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<ContractPaymentMethod> GetByCustomFilter( int? contractId, int? paymentMethodType)
        {
            var query= GetAll(true);
                         if ( contractId.HasValue)
                {
                    query = query.Where(x => x.ContractId == contractId.Value);
                }
                          if ( paymentMethodType.HasValue)
                {
                    query = query.Where(x => x.PaymentMethodType == paymentMethodType.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
