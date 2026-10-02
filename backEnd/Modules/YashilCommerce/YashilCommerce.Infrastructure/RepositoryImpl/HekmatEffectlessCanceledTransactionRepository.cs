using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class HekmatEffectlessCanceledTransactionRepository :
        GenericRepository<HekmatEffectlessCanceledTransaction, int>, IHekmatEffectlessCanceledTransactionRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public HekmatEffectlessCanceledTransactionRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) :
            base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public Task<HekmatEffectlessCanceledTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly)
        {
            if (readOnly)
            {
                return DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.RefundDetailId == refundDetailId);
            }

            return DbSet.FirstOrDefaultAsync(x => x.RefundDetailId == refundDetailId);
        }
    }
}