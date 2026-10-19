using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;
using Z.EntityFramework.Plus;

namespace YashilPayment.Infrastructure.RepositoryImpl
{
    public class HekmatPaymentGatewayTransactionRepository :
        GenericApplicationBasedRepository<HekmatPaymentGatewayTransaction, int>,
        IHekmatPaymentGatewayTransactionRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public HekmatPaymentGatewayTransactionRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) :
            base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<HekmatPaymentGatewayTransaction> GetByCustomFilter(int? merchantId,
            int? hekmatTerminalId, int? fromDate, int? toDate, int? statusCode, string orderId = null)
        {
            var query = GetAll(true);
            if (merchantId.HasValue)
            {
                query = query.Where(x => x.YashilHekmatTerminal.MerchantId == merchantId.Value);
            }

            if (hekmatTerminalId.HasValue)
            {
                query = query.Where(x => x.YashilHekmatTerminalId == hekmatTerminalId.Value);
            }

            if (!string.IsNullOrWhiteSpace(orderId))
            {
                query = query.Where(x => x.LocalReservationNumber == orderId);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.CreationDateInt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.CreationDateInt <= toDate.Value);
            }

            if (statusCode.HasValue)
            {
                query = query.Where(x => x.StatusCode == statusCode.Value);
            }

            return query;
        }

        /// <summary>
        /// دریافت اطلاعات خرید مربوط به پرداخت حکمت بر اساس کد رهگیری مشتری و شناسه ترمینال 
        /// </summary>
        /// <param name="terminalId">شناسه ترمینال</param>
        /// <param name="reservationNumber">کد رهگیری مشتری</param>
        /// <returns></returns>
        public Task<HekmatPaymentGatewayTransaction> GetByReservationNumberAndTerminalIdAsync(string reservationNumber,
            int terminalId)
        {
            var query = DbSet.AsNoTracking().Where(x => x.LocalReservationNumber == reservationNumber
                                                        && x.YashilHekmatTerminalId == terminalId);
            return query.FirstOrDefaultAsync();
        }

        /// <summary>
        ///  دریافت تراکنش ها برای ارسال به حکمت یعنی تراکنش هایی که در وضعیت ارسال به حکمت می باشند
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public IQueryable<HekmatPaymentGatewayTransaction> GetTransactionsThatAreInStatusSendToHekmat(int applicationId)
        {
            return GetAll(true, applicationId).Where(x => x.StatusCode == 0 && x.ErrorCode == 99);
        }

        /// <summary>
        /// تراکنش های که در وضعیت ثبت اولیه می باشند
        ///  یعنی هنوز به وضعیت ارسال به حمکمت نرفته اند را بر می گرداند 
        /// یا در وضعیت
        /// StatusCode = 50 , ErrorCode = 50
        /// می باشند
        /// (یعنی آنها به سمت حکمت رفته اند اما وضعیت آنها مشخص نیست)
        /// </summary>
        public IQueryable<HekmatPaymentGatewayTransaction> GetTransactionsThatAreInStatusInitialRegistration()
        {
            return DbSet.AsNoTracking().Where(x => (x.StatusCode == 0 && x.ErrorCode == 0)
                                                   || (x.StatusCode == 50 && x.ErrorCode == 50));
        }

        /// <summary>
        /// تغییر وضعیت تراکنش ها به وضعیت ارسال به حکمت
        /// </summary>
        /// <returns></returns>
        public Task<int> UpdateTransactionsStatusToSendToHekmatAsync()
        {
            return GetTransactionsThatAreInStatusInitialRegistration().UpdateAsync(
                x => new HekmatPaymentGatewayTransaction
                {
                    StatusCode = 0,
                    ErrorCode = 99
                }
            );
        }

        /// <summary>
        /// بررسی می کند که آیا تراکنشی در وضعیت ارسال به حکمت وجود دارد یا خیر
        /// </summary>
        /// <returns></returns>
        public Task<bool> CheckExistTransactionsThatAreInStatusSendToHekmat()
        {
            return DbSet.AnyAsync(x => x.StatusCode == 0 && x.ErrorCode == 99);
        }

        public string ValidateBeforeHekmatPaymentPurchase(
            HekmatPaymentGatewayTransaction hekmatPaymentGatewayTransaction, int merchantId)
        {
            var resultMessage = _context.ResultMessage.FromSqlRaw($"EXEC [shp].[ValidateBeforeHekmatPaymentPurchase] " +
                                                                  $"@MerchantId = {merchantId}," +
                                                                  $"@YashilHekmatTerminalId = {hekmatPaymentGatewayTransaction.YashilHekmatTerminalId}," +
                                                                  $"@HekmatTerminalId ={hekmatPaymentGatewayTransaction.HekmatTerminalId}," +
                                                                  $"@Price = {hekmatPaymentGatewayTransaction.Price}," +
                                                                  $"@LocalReservationNumber = N'{hekmatPaymentGatewayTransaction.LocalReservationNumber}'," +
                                                                  $"@InstallmentCount = {hekmatPaymentGatewayTransaction.InstallmentCount}," +
                                                                  $"@HekmatCardNumber =N'{hekmatPaymentGatewayTransaction.HekmatCardNumber}'," +
                                                                  $"@NationalCode =N'{hekmatPaymentGatewayTransaction.NationalCode}'," +
                                                                  $"@Mobile = N'{hekmatPaymentGatewayTransaction.Mobile}'," +
                                                                  $"@Tell = N'{hekmatPaymentGatewayTransaction.Tell}'"
            ).AsEnumerable().FirstOrDefault();
            return resultMessage?.Message;
        }

        public Task<HekmatPaymentGatewayTransaction> GetTerminalHekmatTransactionAsync(int yashilHekmatTerminalId,
            string reservationNumber, bool readOnly = true)
        {
            return GetAll(readOnly).Where(x =>
                    x.YashilHekmatTerminalId == yashilHekmatTerminalId &&
                    x.LocalReservationNumber == reservationNumber)
                .FirstOrDefaultAsync();
        }

        public Task<HekmatPaymentGatewayTransaction> GetTerminalHekmatTransactionAsync(int yashilHekmatTerminalId,
            string reservationNumber, bool fromDbSet, bool readOnly = true)
        {
            var query = fromDbSet ?  DbSet: GetAll();

            query = query.Where(x =>
                         x.YashilHekmatTerminalId == yashilHekmatTerminalId &&
                         x.LocalReservationNumber == reservationNumber);

            return readOnly ? query.AsNoTracking().FirstOrDefaultAsync() : query.FirstOrDefaultAsync();
        }
    }
}