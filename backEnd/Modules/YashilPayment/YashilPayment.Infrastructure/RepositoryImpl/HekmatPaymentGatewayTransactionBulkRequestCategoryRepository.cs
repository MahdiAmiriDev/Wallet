using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    
    public class HekmatPaymentGatewayTransactionBulkRequestCategoryRepository : GenericApplicationBasedRepository<HekmatPaymentGatewayTransactionBulkRequestCategory, int>, IHekmatPaymentGatewayTransactionBulkRequestCategoryRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		
        public HekmatPaymentGatewayTransactionBulkRequestCategoryRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        
        
        public IQueryable<HekmatPaymentGatewayTransactionBulkRequestCategory> GetByCustomFilter( int? parentId, int? yashilHekmatTerminalId)
        {
            var query = GetAll(true);
            if(parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }
            
            if(yashilHekmatTerminalId.HasValue)
            {
                query = query.Where(x => x.YashilHekmatTerminalId == yashilHekmatTerminalId.Value);
            }
            
            return query;
        }
         
    }
}      
