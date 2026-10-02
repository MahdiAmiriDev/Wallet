using System.Linq;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class UploadedVoucherRepository : GenericRepository<UploadedVoucher, int>,
        IUploadedVoucherRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public UploadedVoucherRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context,
            userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<UploadedVoucher> GetByCustomFilter(int? appDocumentId)
        {
            var query = GetAll(true);
            if (appDocumentId.HasValue)
            {
                query = query.Where(x => x.AppDocumentId == appDocumentId.Value);
            }


            return query;
        }
        /// <summary>
        /// حذف کوپن های کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        public void DeleteByUserId(int userId)
        {
            _context.UploadedVoucher.Where(x => x.CreateBy == userId).DeleteFromQuery();
        }
        /// <summary>
        /// دریافت کد کوپن های بارگذاری شده توسط کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        public IQueryable<UploadedVoucher> GetByUserId(int userId)
        {
            return DbSet.Where(x => x.CreateBy == userId);
        }
    }
}