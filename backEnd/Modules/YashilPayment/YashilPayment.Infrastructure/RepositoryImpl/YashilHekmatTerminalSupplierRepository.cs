using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    
    public class YashilHekmatTerminalSupplierRepository : GenericApplicationBasedRepository<YashilHekmatTerminalSupplier, int>, IYashilHekmatTerminalSupplierRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		
        public YashilHekmatTerminalSupplierRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }
        
        
        public IQueryable<YashilHekmatTerminalSupplier> GetByCustomFilter( int? parentId, int? userId)
        {
            var query = GetAll(true);
            if(parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }
            
            if(userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId.Value);
            }
            
            return query;
        }
         
    }
}      
