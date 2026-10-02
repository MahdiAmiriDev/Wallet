using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class VocherRepository : GenericApplicationBasedRepository<Vocher, int>, IVocherRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public VocherRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public void AssignVoucherForOrderItems(int applicationId)
        {
            _context.Database.ExecuteSqlRaw($" [shp].[UpdateOrderItemVocher] {_userPrincipal.Id},{_userPrincipal.Id}");
        }

        public IQueryable<Vocher> GetByCustomFilter(int? voucherTypeId)
        {
            var query = GetAll(true);
            if (voucherTypeId.HasValue)
            {
                query = query.Where(x => x.VocherTypeId == voucherTypeId.Value);
            }
            return query;
        }

        public Vocher GetNotUsedVoucherByOrderItemId(int orderItemId)
        {
            var query = _context.OrderItem.Where(x => x.Id == orderItemId)
                 .Include(c => c.ProductSalesPlanning.Product.VocherType.Vocher)
                  .Select(c => c.Vocher).FirstOrDefault();
            return query;
        }

        /// <summary>
        /// دریافت وچر استفاده نشده بر اساس نوع وچر
        /// </summary>
        /// <param name="voucherTypeId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public Vocher GetNotUsedVoucherByVoucherTypeId(int voucherTypeId, int applicationId)
        {
            var voucher = DbSet.Where(x => x.VocherTypeId == voucherTypeId && x.OrderItemId == null && x.ApplicationId == applicationId).Take(1).FirstOrDefault();
            return voucher;
        }

        /// <summary>
        /// دریافت ووچر بر اساس آیتم سفارش
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        Vocher IVocherRepository.GetByOrderItemId(int orderItemId)
        {
            return DbSet.FirstOrDefault(x => x.OrderItemId == orderItemId);

        }
        /// <summary>
        /// درج کد کوپن رمز شده از روی جدول
        /// UploadedVoucher
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="voucherTypeId">شناسه نوع کوپن</param>
        /// <returns></returns>
        public Task<int> AddVoucherFromUploadedVoucher(int applicationId, int userId, int companyId, int voucherTypeId)
        {
            try
            {
                return _context.Database.ExecuteSqlRawAsync($"[shp].[AddVoucherFromUploadedVoucher] {applicationId},{userId},{companyId},{voucherTypeId},{_userPrincipal.OrganizationId} ");
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// حذف کوپن های تخصیص نیافته یک نوع مشخص 
        /// </summary>
        /// <param name="vocherTypeId"></param>
        /// <returns></returns>
        public async Task<int> DeleteNotAssignedVochersAsync(int vocherTypeId)
        {
            return await DbSet.Where(x => x.VocherTypeId == vocherTypeId 
            && x.OrderItemId == null 
            && x.Deleted == false).UpdateAsync(
                x => new()
                {
                    Deleted = true
                });
        }
    }
}
