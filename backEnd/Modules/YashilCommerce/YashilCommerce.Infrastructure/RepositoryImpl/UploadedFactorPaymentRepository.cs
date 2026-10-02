using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class UploadedFactorPaymentRepository : GenericRepository<UploadedFactorPayment, int>,
        IUploadedFactorPaymentRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public UploadedFactorPaymentRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<UploadedFactorPayment> GetByCustomFilter(int? appDocumentId)
        {
            var query = GetAll(true).Where(x=>x.CreateBy == _userPrincipal.Id);
            if (appDocumentId.HasValue)
            {
                query = query.Where(x => x.AppDocumentId == appDocumentId.Value);
            }
            return query;
        }

        public Task<int> ChecksInvalidUploadedFactorPaymentFilesForLoggedInUser()
        {
            return _context.Database.ExecuteSqlRawAsync(
                $"[shp].[CheckInvalidUploadedFactorPayment] {_userPrincipal.Id}");
        }

        public IEnumerable<UploadedFactorPayment> GetAllUploadedFactorPaymentsUploadedByUser(int userId)
        {
            return DbSet.Where(x => x.CreateBy == userId);
        }

        public Task<int> TransferDataFromUploadedFactorPaymentsToFactorPaymentForLoggedInUserAsync()
        {
            return _context.Database.ExecuteSqlRawAsync($" [shp].[TransferDataFromUploadedFactorPaymentsToFactorPayment] {_userPrincipal.Id} ");
        }
    }
}