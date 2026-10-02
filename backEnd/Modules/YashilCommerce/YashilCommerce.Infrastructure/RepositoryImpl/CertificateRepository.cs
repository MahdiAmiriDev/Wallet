using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CertificateRepository : GenericRepository<Certificate, int>, ICertificateRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public CertificateRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public IQueryable<Certificate> GetByCustomFilter(string nationalCode)
        {
            var query = GetAll(true);
            if(!string.IsNullOrEmpty(nationalCode))
            {
                query = query.Where(x => x.NationalCode == nationalCode);
            }
            return query.AsNoTracking();
        }

        /// <summary>
        /// اطلاعات گواهینامه را بر اساس شماره گواهینامه دریافت می کند
        /// </summary>
        /// <param name="certificateNumber">شماره گواهینامه</param>
        /// <returns></returns>
        public Task<Certificate> GetByNumberAsync(string certificateNumber)
        {
            return DbSet.Where(x => x.Number == certificateNumber).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
