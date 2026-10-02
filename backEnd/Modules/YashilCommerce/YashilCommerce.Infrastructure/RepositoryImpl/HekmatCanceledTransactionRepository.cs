using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class HekmatCanceledTransactionRepository : GenericRepository<HekmatCanceledTransaction, int>, IHekmatCanceledTransactionRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public HekmatCanceledTransactionRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public Task<HekmatCanceledTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true)
        {
            if (readOnly)
            {
                return DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.RefundDetailId == refundDetailId);
            }
            return DbSet.FirstOrDefaultAsync(x => x.RefundDetailId == refundDetailId);

        }
    }
}
