using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class HekmatApprovedTransactionRepository : GenericRepository<HekmatApprovedTransaction, int>, IHekmatApprovedTransactionRepository
    {
        private readonly YashilAppDbContext _context;
        public HekmatApprovedTransactionRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
        }
        public async Task DeleteDuplicateHekmatApprovedTransactionsAsync()
        {
            await _context.Database.ExecuteSqlRawAsync($"Execute Shp.DeleteDuplicateHekmatApprovedTransactions");
        }
        /// <summary>
        /// به روز رسانی تراکنش های حکمتی که اصلاح قیمت برای آنها رخ داده است
        /// </summary>
        /// <returns></returns>
        public async Task UpdateModifiedHekmatTransactionAsync()
        {
            await _context.Database.ExecuteSqlRawAsync($"Execute Shp.UpdateModifiedHekmatTransaction");
        }
    }
}
