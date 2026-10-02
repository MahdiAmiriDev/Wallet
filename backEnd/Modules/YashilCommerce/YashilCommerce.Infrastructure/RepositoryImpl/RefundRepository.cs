using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class RefundRepository : GenericApplicationBasedRepository<Refund, int>, IRefundRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public RefundRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<Refund> GetByCustomFilter(int? orderId)
        {
            var query = GetAll(true);
            if (orderId.HasValue)
            {
                query = query.Where(x => x.OrderId == orderId.Value);
            }


            return query;
        }



    }
}
