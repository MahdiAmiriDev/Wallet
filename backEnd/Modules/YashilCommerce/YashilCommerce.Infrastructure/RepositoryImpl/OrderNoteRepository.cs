			using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class OrderNoteRepository : GenericApplicationBasedRepository<OrderNote,int>, IOrderNoteRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public OrderNoteRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
                 
   
    public IQueryable<OrderNote> GetByCustomFilter( int? orderId)
        {
            var query= GetAll(true);
                         if ( orderId.HasValue)
                {
                    query = query.Where(x => x.OrderId == orderId.Value);
                }
                  
          
            return query;
        }
           

       
    }
}      
