using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core.Enums;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Helpers;
using Microsoft.EntityFrameworkCore;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class PaymentService : GenericService<Payment, int>, IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IItemGroupService _itemGroupService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderItemService _orderItemService;
        private readonly IRefundDetailService _refundDetailService;
        private readonly IYashilLogger<Payment> _yashilLogger;
        private readonly IProductSalesPlanningService _productSalesPlanningService;

        public PaymentService(IUnitOfWork unitOfWork, IOrderItemService orderItemService,
            IPaymentRepository paymentRepository, IUserPrincipal userPrincipal,
            ApplicationConfiguration applicationConfiguration, IEventPublisher eventPublisher,
            IItemGroupService itemGroupService,
            IRefundDetailService refundDetailService, IYashilLogger<Payment> logger, IProductSalesPlanningService productSalesPlanningService)
            : base(unitOfWork, paymentRepository, userPrincipal, eventPublisher)
        {
            _paymentRepository = paymentRepository;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
            _itemGroupService = itemGroupService;
            _unitOfWork = unitOfWork;
            _orderItemService = orderItemService;
            _refundDetailService = refundDetailService;
            _yashilLogger = logger;
            _productSalesPlanningService = productSalesPlanningService;
        }

        public IQueryable<Payment> GetByCustomFilter(int? cardType, int? saleType, int? paymentStatusType, int? paymentMethodType, int? orderId)
        {
            return _paymentRepository.GetByCustomFilter(cardType, saleType, paymentStatusType, paymentMethodType, orderId);
        }

        public async Task<Payment> UpdatePaymentStatusAsync(int paymentId, PaymentStatusTypesEnum paymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, bool saveAfterUpdate = false)
        {
            return await UpdatePaymentStatusAsync(paymentId, paymentStatusType, paymentMethodType, message, null, saveAfterUpdate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="paymentStatusType"></param>
        /// <param name="paymentMethodType"></param>
        /// <param name="message"></param>
        /// <param name="referenceNumber">شماره پیگیری تراکنش انجامشده است. (این مقدار یکتا است و به ازای هر تراکنش از سوی فنآوا کارت به پذیرنده ارسال میگردد.) </param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>
        public async Task<Payment> UpdatePaymentStatusAsync(int paymentId, PaymentStatusTypesEnum paymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, string referenceNumber,
            bool saveAfterUpdate = false)
        {
            return await UpdatePaymentStatusAsync(paymentId, paymentStatusType, paymentMethodType, message, referenceNumber, null, null, null, saveAfterUpdate);
        }

        public async Task<Payment> UpdatePaymentStatusAsync(int paymentId, PaymentStatusTypesEnum paymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, string referenceNumber, string customerRefNum,
            string goodReferenceId, string merchantData, 
            bool saveAfterUpdate = false,
            string token = null,
            string retrievalReferenceNumber = null,
            string systemTraceAuditNumber = default
        )
        {
            var payment = await _paymentRepository.GetAsync(paymentId);
            if (payment.PaymentMethodType != (int)PaymentMethodTypesEnum.Wallet && payment.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
            {
                _yashilLogger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error, (int)Events.PaymentException, "Payment", $"خطا در بروز رسانی پرداخت موفق.یعنی این پرداخت قبلا با موفقیت انجام شده و نباید بروز رسانی شود کد پرداخت: {payment.Id}", null);
                return payment;
            }
            if (paymentMethodType == PaymentMethodTypesEnum.Ansar ||
                paymentMethodType == PaymentMethodTypesEnum.Saman ||
                paymentMethodType == PaymentMethodTypesEnum.Samin ||
                paymentMethodType == PaymentMethodTypesEnum.Wallet ||
                 paymentMethodType == PaymentMethodTypesEnum.Saderat
                )
            {
                payment.PaidDateUtcInt = DateFunction.ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            payment.PaymentMethodType = (int)paymentMethodType;
            payment.PaymentStatusType = (int)paymentStatusType;
            payment.CustomerRefNum = customerRefNum;
            payment.GoodReferenceId = goodReferenceId;
            payment.MerchantData = merchantData;
            payment.ModifyBy = _userPrincipal.Id == 0 ? _applicationConfiguration.SysUserId : _userPrincipal.Id;
            payment.PaidDateUtc = DateTime.Now;
            payment.AuthorizationTransactionResult = message;
            payment.RetrievalReferenceNumber =
                !string.IsNullOrEmpty(retrievalReferenceNumber) ? retrievalReferenceNumber : payment.RetrievalReferenceNumber;

            payment.Token = !string.IsNullOrEmpty(token) ? token : payment.Token;
            payment.SystemTraceAuditNumber = systemTraceAuditNumber;

            if (!string.IsNullOrEmpty(referenceNumber))
            {
                payment.AuthorizationTransactionId = referenceNumber;
            }
            var ret = await UpdateAsync(payment, paymentId, saveAfterUpdate);
            // تخصیص سهم پرداخت آیتم سفارش برای پرداخت تایید شده
            if (paymentStatusType == PaymentStatusTypesEnum.Paid)
            {
                await _orderItemService.SetOrderItemPriceRightShareAsync(payment.OrderId);
            }
            await AfterUpdatePaymentAsync(payment, saveAfterUpdate);
            return ret;
        }

        public override async Task<Payment> UpdateAsync(Payment t, object key, bool saveAfterUpdate = false, bool publishEvent = true)
        {
            var result = await base.UpdateAsync(t, key, saveAfterUpdate, publishEvent);
            await AfterUpdatePaymentAsync(t, saveAfterUpdate);
            return result;
        }

        public override async Task<Payment> UpdateAsync(Payment t, object key, List<string> props, bool modifyProps, bool saveAfterUpdate = false,
            bool publishEvent = true)
        {
            var result = await base.UpdateAsync(t, key, props, modifyProps, saveAfterUpdate, publishEvent);
            await AfterUpdatePaymentAsync(t, saveAfterUpdate);
            return result;
        }
        /// <summary>
        /// بعد از به روز رسانی پرداخت
        /// این متد برای صدا زدن سرویس هایی اجرا می شود
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>
        private async Task AfterUpdatePaymentAsync(
            Payment payment,
            bool saveAfterUpdate = false)
        {
            // درصورتی که پرداخت ناموفق بود
            // ظرفیت برنامه ریزی دوره باید به روز رسانی شود
            if (payment.PaymentStatusType != (int)PaymentStatusTypesEnum.Paid)
            {
                var order = await FindById(payment.Id)
                    .Select(x => x.Order)
                    .FirstOrDefaultAsync();
                await _productSalesPlanningService
                    .UpdateOrderProductSalesPlanningsCapacityAsync(order);
            }
            if (saveAfterUpdate)
            {
                await SaveChangeAsync();
            }
        }

        public Task<Payment> GetByReservationNumberIncludeOrderAsync(string reservationNumber, bool readOnly = false)
        {
            return _paymentRepository.GetByReservationNumberIncludeOrderAsync(reservationNumber, readOnly);
        }

        public IQueryable<Payment> GetOrderPaymentsByStatusType(int orderId, PaymentStatusTypesEnum paymentStatusType)
        {
            return _paymentRepository.GetOrderPaymentsByStatusType(orderId, paymentStatusType);
        }
        /// <summary>
        /// دریافت آخرین رکورد پرداخت برای  سفارش
        /// </summary>
        /// <param name="oId"></param>
        /// <returns></returns>
        public Payment GetOrderLastPayment(int oId)
        {
            return _paymentRepository.GetOrderLastPayment(oId);
        }
        /// <summary>
        /// Get Payments Order By OrderId
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        public IQueryable<Payment> GetPaymentsByOrderId(int orderId)
        {
            return _paymentRepository.GetPaymentsByOrderId(orderId);
        }

        //public Task<Payment> UpdateAuthorizationTransactionIdAsync(int paymentId, string authorizationTransactionId,
        //    bool saveAfterUpdate)
        //{
        //    var payment = _paymentRepository.Get(paymentId);
        //    int.TryParse(authorizationTransactionId, out int authorizationTransactionIdInt);
        //    payment.AuthorizationTransactionId = authorizationTransactionIdInt == 0 ? null : authorizationTransactionIdInt.ToString();
        //    payment.ModifyBy = _userPrincipal.Id == 0 ? _applicationConfiguration.SysUserId : _userPrincipal.Id;
        //    payment.ModificationDate = DateTime.Now;

        //    return UpdateAsync(payment, paymentId, saveAfterUpdate);
        //}

        public IQueryable<Payment> GetUserPendingHekmatPayment(int userId)
        {
            return _paymentRepository.GetUserPendingHekmatPayment(userId);
        }

        public IQueryable<Payment> GetCustomerClubWalletPaymentsAssignedToPaymentGroup(int paymentGroupId)
        {
            return _paymentRepository.GetCustomerClubPaymentsAssignedToPaymentGroup(paymentGroupId);
        }

        public async Task<bool> AssignAndConfirmCustomerClubWalletPaymentGroup(int groupId)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            var paymentGroup = await _itemGroupService.GetAsync(groupId, false);
            try
            {
                if (paymentGroup.ItemGroupSubId != null)
                {
                    int count = await _paymentRepository.AssignAndConfirmCustomerClubWalletPaymentGroup(paymentGroup.ItemGroupSubId.Value, groupId);
                    if (count > 0)
                    {
                        paymentGroup.ItemGroupStatusType = (int)ItemGroupStatusTypeEnum.Confirmed;
                        await _itemGroupService.UpdateAsync(paymentGroup, paymentGroup.Id, true);
                        await _paymentRepository.InsertCustomerClubPaymentInstallmentAsync();
                        await transaction.CommitAsync();
                        return true;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _yashilLogger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error,
                    (int)Events.UpdateException,
                    "CustomerClubWalletPayment",
                    $"خطا در  گروه بندی ثبت نامی های ناجا ", e);
                throw;
            }
            return false;
        }

        public IQueryable<Payment> GetCustomerClubWalletPaymentsNotAssignedToPaymentGroup(int? customerClubId, bool readOnly = false)
        {
            return _paymentRepository.GetCustomerClubWalletPaymentsNotAssignedToPaymentGroup(customerClubId, readOnly);
        }


        public decimal GetOrderTotalPayment(int orderId, PaymentStatusTypesEnum paid)
        {
            var paymentRefunds = _refundDetailService.GetByOrderId(orderId)
                .Select(x => new { x.PaymentId, x.NewPaymentAmount, x.RefundStatusType,
                    PaymentMethodType = x.Payment.PaymentMethodType })
                .ToList()
                .Where(x => !RequiresFinalizedRefundStatus(x.PaymentMethodType)
                    || x.RefundStatusType == (int)RefundStatusTypeEnum.SendForModifyPrice
                    || x.RefundStatusType == (int)RefundStatusTypeEnum.Done)
                .GroupBy(x => x.PaymentId)
                .Select(x => new { Paymentid = x.Key, NewPaymentAmount = x.Min(c => c.NewPaymentAmount) })
                .ToList();

            var completePayments = _paymentRepository
                .GetOrderPaymentsByStatusType(orderId, PaymentStatusTypesEnum.Paid)
                .Where(x => x.Order.OrderStatusType != (int)OrderStatusTypesEnum.IncompleteCashAndInstallmentPay)
                .Select(x => new { x.Id, x.Total }).ToList();

            decimal sumTotal = 0;
            foreach (var payment in completePayments)
            {
                var refundDetail = paymentRefunds.FirstOrDefault(x => x.Paymentid == payment.Id);
                if (refundDetail != null)
                {
                    if (refundDetail.NewPaymentAmount < 0m || refundDetail.NewPaymentAmount > payment.Total)
                        throw new InvalidOperationException("Final refund amount is inconsistent with the paid Payment.");
                    sumTotal += refundDetail.NewPaymentAmount;
                }
                else
                {
                    sumTotal += payment.Total;
                }

            }
            return sumTotal;
        }

        /// <summary>برای پرداخت‌های شتابی و حکمت آنلاین فقط Refund قطعی در مانده اثر می‌گذارد.</summary>
        private static bool RequiresFinalizedRefundStatus(int? paymentMethodType)
            => paymentMethodType == (int)PaymentMethodTypesEnum.BankInternetPaymentGateway
                || paymentMethodType == (int)PaymentMethodTypesEnum.Ansar
                || paymentMethodType == (int)PaymentMethodTypesEnum.Saman
                || paymentMethodType == (int)PaymentMethodTypesEnum.Saderat
                || paymentMethodType == (int)PaymentMethodTypesEnum.IranKish
                || paymentMethodType == (int)PaymentMethodTypesEnum.HekmatOnline;

        public Task<bool> ExistsAnyPaymentWithThisItemGroupIdAsync(int itemGroupId)
        {
            return _paymentRepository.ExistsAnyPaymentWithThisItemGroupIdAsync(itemGroupId);
        }

        public Task<bool> CheckExitsByRefNumberAsync(PaymentMethodTypesEnum paymentMethodType, string refNum)
        {
            return _paymentRepository.CheckExitsByRefNumberAsync(paymentMethodType, refNum);
        }

        public Task<bool> CheckExitsByRetrievalReferenceNumberAsync(
            PaymentMethodTypesEnum paymentMethodType,
            string retrievalReferenceNumber)
        {
            return _paymentRepository.CheckExitsByRetrievalReferenceNumberAsync(
                paymentMethodType,
                retrievalReferenceNumber);
        }

        public Task<Payment> GetByTokenAsync(string token, int paymentMethodType, bool readOnly = false,
            int? applicationId = null)
        {
            return _paymentRepository.GetByTokenAsync(token, paymentMethodType, readOnly, applicationId);
        }

        public Task<bool> CheckExistByTokenAsync(string token, int paymentMethodType)
        {
            return _paymentRepository.CheckExistByTokenAsync(token, paymentMethodType);
        }

        public IQueryable<Payment> FindById(int paymentId)
        {
            return _paymentRepository.FindById(paymentId);
        }


        public IQueryable<Payment> FindByToken(string token,PaymentMethodTypesEnum paymentMethodTypesEnum ,bool readOnly)
            => readOnly ? _paymentRepository.FindByToken(token)
                .Where(x => x.PaymentMethodType == (int)paymentMethodTypesEnum).AsNoTracking() : _paymentRepository.FindByToken(token);

        public Task<int> UpdateTokenAsync(string token, int id)
            => GetAll(true).Where(x => x.Id == id)
                .ExecuteUpdateAsync(e => e.SetProperty(s => s.Token, token));

    }
}
