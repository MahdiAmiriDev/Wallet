using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data; 
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
	public class HekmatModifiedTransactionRepository : GenericRepository<HekmatModifiedTransaction,int>, IHekmatModifiedTransactionRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
		public HekmatModifiedTransactionRepository (YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,userPrincipal)
            {
                _context = context;
                _userPrincipal = userPrincipal;
            }
        /// <summary>
        /// دریافت تراکنش عودت بر اساس refundDetailId
        /// </summary>
        /// <param name="refundDetailId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public Task<HekmatModifiedTransaction> GetByRefundDetailIdAsync(int refundDetailId, bool readOnly = true)
        {
            if (readOnly)
            {
                return DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.RefundDetailId == refundDetailId);
            }
            return DbSet.FirstOrDefaultAsync(x => x.RefundDetailId == refundDetailId);
        }
    }
}      
