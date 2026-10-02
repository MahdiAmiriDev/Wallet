using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;

using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class CustomerClubPaymentInstallmentRepository : GenericApplicationBasedRepository<CustomerClubPaymentInstallment, int>, ICustomerClubPaymentInstallmentRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;
        public CustomerClubPaymentInstallmentRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public async Task DeleteByPaymentIdAsync(int paymentId, bool saveAfterDelete)
        {
            await _context.Database.ExecuteSqlRawAsync($"DELETE  FROM shp.CustomerClubPaymentInstallment WHERE PaymentId = {paymentId}");
        }

        public async Task DeleteRecordsAfterYearMonthByPaymentIdAsync(int paymentId, int currentYearAndMonth, bool saveAfterDelete)
        {
            await _context.Database.ExecuteSqlRawAsync($"DELETE  FROM  shp.CustomerClubPaymentInstallment WHERE PaymentId = {paymentId}  AND YearMonth >= {currentYearAndMonth}");
        }
        /// <summary>
        /// رکورد های اقساط پرداخت ناجا که برای مطالبه اقساط ناجا (پرداخت اقساط ناجا به شرکت) به ناجا ارسال نشده است
        /// یعنی DemandGroupId 
        /// برابر با
        /// Null
        /// باشد
        /// </summary>
        /// <param name="paymentId">شناسه پرداخت ناجا</param>
        /// <returns></returns>
        public async Task DeleteRecordsNotSentForDemandByPaymentIdAsync(int paymentId)
        {
            await _context.Database.ExecuteSqlRawAsync($"DELETE  FROM  shp.CustomerClubPaymentInstallment WHERE PaymentId = {paymentId}  AND DemandGroupId Is Null");
        }

        public Task<bool> ExistsAnyCustomerClubPaymentInstallmentWithThisItemGroupIdAsync(int itemGroupId)
        {
            return DbSet.AnyAsync(x => x.DemandGroupId == itemGroupId);
        }

        public IQueryable<CustomerClubPaymentInstallment> GetCustomerClubPaymentInstallmentsAssignedToDemandGroup(int groupId)
        {
            return GetUserAccessedCustomerClubPaymentInstallment().Where(x => x.DemandGroupId == groupId).AsNoTracking();
        }

        public IQueryable<CustomerClubPaymentInstallment> GetCustomerClubPaymentInstallmentsNotAssignedToDemandGroupAsync(int itemGroupSubId, bool readOnly = false)
        {
            var currentDateYearMonth = DateFunction.TodayShamsiYearAndMonthInt();
            var query = GetUserAccessedCustomerClubPaymentInstallment()
                .Where(x=> 
                !x.DemandGroupId.HasValue && x.YearMonth < currentDateYearMonth &&
                !x.Payment.RefundDetail.Any(r=>r.PaymentId == x.PaymentId));
            if (readOnly)
            {
                return query.AsNoTracking();
            }
            return query;
        }

        public Task<int> AssignCustomerClubPaymentInstallmentDemandGroup(int customerClubId, int groupId)
        {
            return GetCustomerClubPaymentInstallmentsNotAssignedToDemandGroupAsync(customerClubId)
                .UpdateAsync(x => new CustomerClubPaymentInstallment { DemandGroupId = groupId });
        }

        public Task<int> AssignCustomerClubPaymentInstallmentOldSiteDemandGroup(int customerClubId, int groupId)
        {
            return GetCustomerClubPaymentInstallmentsOldSiteNotAssignedToDemandGroupAsync(customerClubId)
                .UpdateAsync(x => new CustomerClubPaymentInstallmentOldSite { DemandGroupId = groupId });
        }
        /// <summary>
        /// دریافت لیست مطالباتی قدیمی که تاکنون گروه بندی نشده است
        /// </summary>
        /// <param name="customerClubId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        private IQueryable<CustomerClubPaymentInstallmentOldSite> GetCustomerClubPaymentInstallmentsOldSiteNotAssignedToDemandGroupAsync(int customerClubId, bool readOnly= false)
        {
            var currentDateYearMonth = DateFunction.TodayShamsiYearAndMonthInt();
            var query = _context.CustomerClubPaymentInstallmentOldSite.Where(x => !x.DemandGroupId.HasValue && x.YearMonth < currentDateYearMonth);
            if (readOnly)
            {
                return query.AsNoTracking();
            }
            return query;
        }

        public IQueryable<CustomerClubPaymentInstallment> GetByCustomFilter(int? paymentId, int? yearMonth)
        {
            var query = GetAll(true);
            if (paymentId.HasValue)
            {
                query = query.Where(x => x.PaymentId == paymentId.Value);
            }
            if (yearMonth.HasValue)
            {
                query = query.Where(x => x.YearMonth == yearMonth.Value);
            }


            return query;
        }

        /// <summary>
        /// مطالباتی که  کاربر جاری به آنها دسترسی دارد
        /// </summary>
        /// <returns></returns>
        private IQueryable<CustomerClubPaymentInstallment> GetUserAccessedCustomerClubPaymentInstallment()
        {
            return DbSet.Where(x => _context.UserApplication.Any(ua => ua.UserId == _userPrincipal.Id && ua.AssignedApplicationId == x.ApplicationId));
        }


    }
}
