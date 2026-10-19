using System.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Data;
using YashilPayment.Core.Repositories;

namespace YashilPayment.Infrastructure.RepositoryImpl
{

    public class RefundPaymentTransactionItemRequestRepository :
        GenericApplicationBasedRepository<RefundPaymentTransactionItemRequest, int>,
        IRefundPaymentTransactionItemRequestRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal;

        public RefundPaymentTransactionItemRequestRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) :
            base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }


        public IQueryable<RefundPaymentTransactionItemRequest> GetByCustomFilter(int? parentId,
            int? yashilHekmatTerminalId, int? refundPaymentTransactionItemRequestType,
            int? refundPaymentTransactionItemRequestStatus)
        {
            var query = GetAll(true);
            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }

            if (yashilHekmatTerminalId.HasValue)
            {
                query = query.Where(x => x.YashilHekmatTerminalId == yashilHekmatTerminalId.Value);
            }

            if (refundPaymentTransactionItemRequestType.HasValue)
            {
                query = query.Where(x =>
                    x.RefundPaymentTransactionItemRequestType == refundPaymentTransactionItemRequestType.Value);
            }

            if (refundPaymentTransactionItemRequestStatus.HasValue)
            {
                query = query.Where(x =>
                    x.RefundPaymentTransactionItemRequestStatus == refundPaymentTransactionItemRequestStatus.Value);
            }

            return query;
        }


        public IQueryable<IsiBookCompleteOrderDomainModel> GetIsiBookCompleteOrder(int vendorId, int startDateInt,
            int endDateInt)
        {
            return _context.IsiBookCompleteOrderDomainModels
                .FromSqlInterpolated(
                    $"exec [pay].[GetIsiBookCompeletedOrders] @StartDateInt = {startDateInt},@EndDateInt = {endDateInt},@VendorId={vendorId}");
        }

        /// <param name="yashilHekmatTerminalId">ترمینال های درگاه پرداخت</param>
        /// <param name="reservationNumber">شناسه تراکنش پرداخت یا سفارش سمت ترمینال پذیرنده</param>
        public async Task<string> RefundTerminalPaymentTransaction(int yashilHekmatTerminalId, string reservationNumber)
        {
            try
            {

                var result = new SqlParameter
                {
                    ParameterName = "@Result",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Output,
                    Size = 300
                };

                var yashilHekmatTerminalIdParam = new SqlParameter
                {
                    ParameterName = "@YashilHekmatTerminalId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = yashilHekmatTerminalId
                };

                var reservationNumberParam = new SqlParameter
                {
                    ParameterName = "@ReservationNumber",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Value = reservationNumber
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC [pay].[RefundTerminalPaymentTransaction] @YashilHekmatTerminalId,@ReservationNumber,@Result OUT",
                    yashilHekmatTerminalIdParam, reservationNumberParam, result);
                return result.Value.ToString();


            }
            catch (Exception)
            {
                return "کاربر گرامی،عملیات عودت تراکنش های سایت ترمینال پذیرنده خطایی رخ است.لطفا مجددا تلاش نمایید";
            }
        }
    }
}
