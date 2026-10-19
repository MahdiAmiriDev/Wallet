using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class CustomerOrderFundingService : ICustomerOrderFundingService
    {
        private readonly YashilAppDbContext _context;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        public CustomerOrderFundingService(YashilAppDbContext context, IUserPrincipal principal,
            ApplicationConfiguration application)
        {
            _context = context;
            _principal = principal;
            _application = application;
        }

        /// <summary>پرداخت‌های ثبت‌شده و خالص Ledger کیف پول را فقط برای سفارش‌های کاربر جاری می‌خواند.</summary>
        public async Task<IReadOnlyDictionary<int, CustomerOrderFundingDto>> GetCurrentUserOrdersAsync(
            IEnumerable<int> orderIds)
        {
            var ids = orderIds?.Where(x => x > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
            var userId = _principal?.Id ?? 0;
            var applicationId = _application.ApplicationId;
            if (userId <= 0 || applicationId <= 0 || ids.Length == 0)
                return new Dictionary<int, CustomerOrderFundingDto>();

            var allowedIds = await _context.Order.AsNoTracking()
                .Where(x => ids.Contains(x.Id) && x.UserId == userId
                    && x.ApplicationId == applicationId && !x.Deleted)
                .Select(x => x.Id).ToArrayAsync();
            var result = allowedIds.ToDictionary(x => x, x => new CustomerOrderFundingDto { OrderId = x });
            if (allowedIds.Length == 0)
                return result;

            var payments = await _context.Payment.AsNoTracking()
                .Where(x => allowedIds.Contains(x.OrderId) && x.ApplicationId == applicationId
                    && !x.Deleted && x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
                .GroupBy(x => x.OrderId)
                .Select(x => new { OrderId = x.Key, Amount = x.Sum(p => p.Total) })
                .ToListAsync();
            foreach (var payment in payments)
                result[payment.OrderId].RecordedPaidAmount = payment.Amount;

            // مبلغ‌های تلاش باز فقط برای نمایش‌اند و در پرداخت قطعی Order جمع نمی‌شوند.
            var openAttempts = await _context.WalletReservation.AsNoTracking()
                .Where(x => x.PaymentId.HasValue && !x.Deleted && x.ApplicationId == applicationId
                    && x.UserId == userId && allowedIds.Contains(x.Payment.OrderId)
                    && x.Payment.ApplicationId == applicationId && !x.Payment.Deleted
                    && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                    && (x.Payment.PaymentMethodType == (int)PaymentMethodTypesEnum.Vibe
                        || x.Payment.PaymentMethodType == (int)PaymentMethodTypesEnum.HekmatOnline))
                .Select(x => new { x.Payment.OrderId, WalletAmount = x.Amount,
                    GatewayAmount = x.Payment.Total })
                .ToListAsync();
            foreach (var group in openAttempts.GroupBy(x => x.OrderId))
                if (group.Count() == 1)
                {
                    result[group.Key].OpenWalletReservedAmount = group.Single().WalletAmount;
                    result[group.Key].OpenGatewayPaymentAmount = group.Single().GatewayAmount;
                }

            var pendingCaptures = await _context.WalletReservation.AsNoTracking()
                .Where(x => x.PaymentId.HasValue && !x.Deleted && x.ApplicationId == applicationId
                    && x.UserId == userId
                    && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                    && allowedIds.Contains(x.Payment.OrderId)
                    && x.Payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid
                    && (x.Payment.PaymentMethodType == (int)PaymentMethodTypesEnum.Vibe
                        || x.Payment.PaymentMethodType == (int)PaymentMethodTypesEnum.HekmatOnline))
                .Select(x => new { x.Payment.OrderId, PaymentId = x.PaymentId.Value,
                    x.Payment.PaymentMethodType })
                .ToListAsync();
            foreach (var pending in pendingCaptures.GroupBy(x => new { x.OrderId, x.PaymentMethodType }))
                if (pending.Count() == 1)
                {
                    if (pending.Key.PaymentMethodType == (int)PaymentMethodTypesEnum.Vibe)
                        result[pending.Key.OrderId].PendingVibeCapturePaymentId = pending.Single().PaymentId;
                    else
                        result[pending.Key.OrderId].PendingHekmatOnlineCapturePaymentId = pending.Single().PaymentId;
                }

            var wallet = await _context.WalletTransaction.AsNoTracking()
                .Where(x => x.PaymentId.HasValue && allowedIds.Contains(x.Payment.OrderId)
                    && x.ApplicationId == applicationId && !x.Deleted
                    && x.UserId == userId && x.Payment.ApplicationId == applicationId && !x.Payment.Deleted
                    && x.WalletReservationId.HasValue
                    && ((x.WalletTransactionType == (int)WalletTransactionTypeEnum.Capture
                            && x.WalletTransactionDirectionType == (int)WalletTransactionDirectionTypeEnum.Debit
                            && x.Amount < 0m)
                        || (x.WalletTransactionType == (int)WalletTransactionTypeEnum.Refund
                            && x.WalletTransactionDirectionType == (int)WalletTransactionDirectionTypeEnum.Credit
                            && x.Amount > 0m)))
                .GroupBy(x => new { x.Payment.OrderId, x.WalletTransactionType })
                .Select(x => new { x.Key.OrderId, x.Key.WalletTransactionType, Amount = x.Sum(t => t.Amount) })
                .ToListAsync();
            foreach (var item in wallet)
            {
                if (item.WalletTransactionType == (int)WalletTransactionTypeEnum.Capture)
                    result[item.OrderId].WalletCapturedAmount = -item.Amount;
                else
                    result[item.OrderId].WalletRefundedAmount = item.Amount;
            }
            return result;
        }
    }
}
