using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Repositories;
using Z.EntityFramework.Plus;


namespace YashilCommerce.Infrastructure.RepositoryImpl
{
    public class PaymentRepository : GenericApplicationBasedRepository<Payment, int>, IPaymentRepository
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _userPrincipal; 
        public PaymentRepository(YashilAppDbContext context, IUserPrincipal userPrincipal) : base(context, userPrincipal)
        {
            _context = context;
            _userPrincipal = userPrincipal;
        }

        public IQueryable<Payment> GetByCustomFilter(int? cardType, int? saleType, int? paymentStatusType, int? paymentMethodType, int? orderId)
        {
            var query = GetAll(true);
            if (cardType.HasValue)
            {
                query = query.Where(x => x.CardType == cardType.Value);
            }
            if (saleType.HasValue)
            {
                query = query.Where(x => x.SaleType == saleType.Value);
            }
            if (paymentStatusType.HasValue)
            {
                query = query.Where(x => x.PaymentStatusType == paymentStatusType.Value);
            }
            if (paymentMethodType.HasValue)
            {
                query = query.Where(x => x.PaymentMethodType == paymentMethodType.Value);
            }
            if (orderId.HasValue)
            {
                query = query.Where(x => x.OrderId == orderId.Value);
            }

            return query;
        }

        public Task<Payment> GetByReservationNumberIncludeOrderAsync(string reservationNumber, bool readOnly = false)
        {
            if (readOnly)
            {
                return DbSet.Include(x => x.Order).AsNoTracking().FirstOrDefaultAsync(x => x.ReservationNumber == reservationNumber);

            }
            return DbSet.Include(x => x.Order).FirstOrDefaultAsync(x => x.ReservationNumber == reservationNumber);

        }

        public IQueryable<Payment> GetOrderPaymentsByStatusType(int orderId, PaymentStatusTypesEnum paymentStatusType)
        {
            return DbSet.Where(x => x.OrderId == orderId && x.PaymentStatusType == (int)paymentStatusType);
        }

        public Payment GetOrderLastPayment(int oId)
        {
            return DbSet.Where(x => x.OrderId == oId).OrderByDescending(x => x.Id).FirstOrDefault();
        }
        /// <summary>
        /// Get Payments Order By OrderId
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        public IQueryable<Payment> GetPaymentsByOrderId(int orderId)
        {
            return DbSet.Where(x => x.OrderId == orderId);
        }

        public IQueryable<Payment> GetUserPendingHekmatPayment(int userId)
        {
            return DbSet.Where(x => x.Order.UserId == userId &&
                                    x.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat &&
                                    (x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway ||
                                     x.PaymentStatusType ==
                                     (int)PaymentStatusTypesEnum.WaitingResponseFromPaymentGateway));
        }


        public IQueryable<Payment> GetCustomerClubPaymentsAssignedToPaymentGroup(int paymentGroupId)
        {
            // شرط x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid
            // در نظر گرفته نشود.چون به هر دلیلی ممکن است تغییر کرده باشد
            return GetUserAccessedPayment().Where(x => x.PaymentGroupId == paymentGroupId).AsNoTracking();
        }

        public Task<int> InsertCustomerClubPaymentInstallmentAsync()
        {
            object[] param = {
                        new SqlParameter
                        {
                            ParameterName = "@userId",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = _userPrincipal.Id
                        }
            };
            return _context.Database.ExecuteSqlRawAsync("Shp.InsertCustomerClubPaymentInstallment @userId", param);
        }
        public IQueryable<Payment> GetCustomerClubWalletPaymentsNotAssignedToPaymentGroup(int? customerClubId, bool readOnly = false)
        {
            var query = GetUserAccessedPayment();
            query = query.Where(x =>
                 x.RefundDetail.All(r => r.NewPaymentAmount != 0) &&
                 (x.Order.OrderStatusType == (int)OrderStatusTypesEnum.Complete ||
                  x.Order.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment) &&
                 x.PaymentGroupId == null && x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid &&
                 x.PaymentMethodType == (int)PaymentMethodTypesEnum.Wallet &&
                 x.Order.CustomerClubCard.CustomerClubId == customerClubId);
            if (readOnly)
            {
                return query.AsNoTracking();
            }
            return query;

        }

        public Task<bool> ExistsAnyPaymentWithThisItemGroupIdAsync(int itemGroupId)
        {
            return _context.Payment.AnyAsync(x => x.PaymentGroupId == itemGroupId);
        }

        public Task<int> AssignAndConfirmCustomerClubWalletPaymentGroup(int customerClubId, int paymentGroupId)
        {
            return GetCustomerClubWalletPaymentsNotAssignedToPaymentGroup(customerClubId, false)
                 .UpdateAsync(x => new Payment { PaymentGroupId = paymentGroupId });
        }

        /// <summary>
        /// جستجوی پرداخت با رسید دیجیتالی ارسالی از سمت بانک
        /// با توجه به متد پرداخت
        /// وجود دارد یا خیر؟
        /// </summary>
        /// <param name="paymentMethodType"></param>
        /// <param name="refNum">رسید دیجیتالی</param>
        public Task<bool> CheckExitsByRefNumberAsync(PaymentMethodTypesEnum paymentMethodType, string refNum)
        {
            return DbSet.AnyAsync(x =>
                x.PaymentMethodType == (int)paymentMethodType
                && x.AuthorizationTransactionId == refNum);
        }

        public IQueryable<Payment> FindById(int paymentId)
        {
            return DbSet.Where(x => x.Id == paymentId)
                .AsNoTracking();
        }

        public IQueryable<Payment> FindByToken(string token)
            => DbSet.Where(x => x.Token == token);
        

        public Task<bool> CheckExitsByRetrievalReferenceNumberAsync(PaymentMethodTypesEnum paymentMethodType,
            string retrievalReferenceNumber)
        {
            return DbSet.AnyAsync(x =>
                x.RetrievalReferenceNumber == retrievalReferenceNumber &&
                x.PaymentMethodType == (int)paymentMethodType
            );
        }

        public Task<Payment> GetByTokenAsync(string token, int paymentMethodType, bool readOnly = false, int? applicationId = null)
        {
            return GetAll(readOnly, applicationId).Where(
                x => x.Token == token &&
                     x.PaymentMethodType == paymentMethodType)
                .FirstOrDefaultAsync();
        }

        public Task<bool> CheckExistByTokenAsync(string token, int paymentMethodType)
        {
            return DbSet.AnyAsync(x => x.Token == token && x.PaymentMethodType == paymentMethodType);
        }

        /// <summary>
        /// پرداختی هایی که کاربر جاری به آنها دسترسی دارد
        /// </summary>
        /// <returns></returns>
        private IQueryable<Payment> GetUserAccessedPayment()
        {
            return _context.Payment.Where(x => _context.UserApplication.Any(ua => ua.UserId == _userPrincipal.Id && ua.AssignedApplicationId == x.ApplicationId));
        }
    }
}
