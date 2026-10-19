using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using YashilPayment.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class HekmatPaymentGatewayTransactionService : GenericService<HekmatPaymentGatewayTransaction, int>,
        IHekmatPaymentGatewayTransactionService
    {
        private readonly IHekmatPaymentGatewayTransactionRepository _hekmatPaymentGatewayTransactionRepository;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IYashilLogger<HekmatPaymentGatewayTransaction> _logger;

        public HekmatPaymentGatewayTransactionService(IUnitOfWork unitOfWork,
            IHekmatPaymentGatewayTransactionRepository hekmatPaymentGatewayTransactionRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
            ApplicationConfiguration applicationConfiguration, IYashilLogger<HekmatPaymentGatewayTransaction> logger)
            : base(unitOfWork, hekmatPaymentGatewayTransactionRepository, userPrincipal, eventPublisher)
        {
            _hekmatPaymentGatewayTransactionRepository = hekmatPaymentGatewayTransactionRepository;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
        }


        public IQueryable<HekmatPaymentGatewayTransaction> GetByCustomFilter(int? merchantId, int? hekmatTerminalId,
            int? fromDate, int? toDate, int? statusCode, string orderId = null)
        {
            return _hekmatPaymentGatewayTransactionRepository.GetByCustomFilter(merchantId, hekmatTerminalId, fromDate, toDate, statusCode, orderId);
        }

        /// <summary>
        /// دریافت تراکنش ها برای ارسال به حکمت:
        ///   ابتدا  بررسی می کند که آیا تراکنش هایی در وضعیت ارسال به حکمت وجود دارد یا خیر
        /// 1 - اگر وجود داشت
        /// تراکنش هایی که در وضعیت ارسال به حکمت می باشند را بر می گرداند.
        /// 2 - در غیر اینصورت
        /// تراکنش هایی که در وضعیت ثبت اولیه می باشند را به وضعیت ارسال به حکمت تغییر می دهد، 
        /// سپس تراکنش هایی که در وضعیت ارسال به حکمت می باشند را بر می گرداند. 
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        public async Task<IQueryable<HekmatPaymentGatewayTransaction>> GetTransactionsForSendToHekmatAsync(
            int applicationId)
        {
            // بررسی می کند که آیا تراکنش هایی در وضعیت ارسال به حکمت وجود دارد یا خیر 
            var existTransactionsForSendToHekmat = await
                _hekmatPaymentGatewayTransactionRepository.CheckExistTransactionsThatAreInStatusSendToHekmat();

            // اگر تراکنش هایی در وضعیت ارسال به حکمت وجود نداشت می تواند تراکنش های ثبت اولیه را به وضعیت ارسال به حکمت تغییر بدهد
            if (!existTransactionsForSendToHekmat)
            {
                await _hekmatPaymentGatewayTransactionRepository.UpdateTransactionsStatusToSendToHekmatAsync();
            }

            // تراکنش هایی که در وضعیت ارسال به حکمت می باشند را بر میگرداند
            return _hekmatPaymentGatewayTransactionRepository.GetTransactionsThatAreInStatusSendToHekmat(applicationId);
        }

        public string ValidateBeforeHekmatPaymentPurchase(
            HekmatPaymentGatewayTransaction hekmatPaymentGatewayTransaction, int merchantId)
        {
            return _hekmatPaymentGatewayTransactionRepository.ValidateBeforeHekmatPaymentPurchase(
                hekmatPaymentGatewayTransaction, merchantId);
        }

        public Task<HekmatPaymentGatewayTransaction> GetByReservationNumberAndTerminalIdAsync(string reservationNumber, int terminalId)
        {
            return _hekmatPaymentGatewayTransactionRepository.GetByReservationNumberAndTerminalIdAsync(reservationNumber, terminalId);
        }

        public async Task<(bool isSucces, string message)> RefundHekmatTransactionAsync(int yashilHekmatTerminalId, string reservationNumber)
        {
            var hekmatTransaction = await GetTerminalHekmatTransaction(yashilHekmatTerminalId, reservationNumber, false);
            if (hekmatTransaction == null)
            {

                return (false, ".تراکنشی یافت نشد");
            }

            if (hekmatTransaction.StatusCode == (int)HekmatStatusCodeEnum.Unsuccessful &&
                hekmatTransaction.ErrorCode == (int)HekmatErrorCodeEnum.Refunded)
            {
                return (true, "این تراکنش قبلا عودت زده شده است.");
            }
            if (hekmatTransaction.StatusCode == (int)HekmatStatusCodeEnum.Success)
            {
                hekmatTransaction.StatusCode = (int)HekmatStatusCodeEnum.Unsuccessful;
                hekmatTransaction.ErrorCode = (int)HekmatErrorCodeEnum.Refunded;
                var res = await UpdateAsync(hekmatTransaction, hekmatTransaction.Id, true);
                if (res.StatusCode == (int)HekmatStatusCodeEnum.Unsuccessful &&
                    res.ErrorCode == (int)HekmatErrorCodeEnum.Refunded)
                {
                    return (true, "با موفقیت عودت تراکنش حکمت درخواستی انجام شد.");
                }
            }
            return (false, "عودت تراکنش انجام نشد");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<HekmatPaymentGatewayTransaction> GetTerminalHekmatTransaction(int yashilHekmatTerminalId, string reservationNumber, bool readOnly = true)
        {
            return _hekmatPaymentGatewayTransactionRepository.GetTerminalHekmatTransactionAsync(
                yashilHekmatTerminalId,
                reservationNumber,
                readOnly);
        }

        public Task<HekmatPaymentGatewayTransaction> GetTerminalHekmatTransactionAsync(int yashilHekmatTerminalId,
            string reservationNumber, bool readOnly = true)
        {
            return GetAll(readOnly).Where(x =>
                    x.YashilHekmatTerminalId == yashilHekmatTerminalId &&
                    x.LocalReservationNumber == reservationNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<(bool isSuccess, string message)> CancelHekmatTransactionAsync(int yashilHekmatTerminalId, 
            string reservationNumber,long amount)
        {
            var hekmatTransaction = await _hekmatPaymentGatewayTransactionRepository.GetTerminalHekmatTransactionAsync
                (yashilHekmatTerminalId, reservationNumber, true, false);

            if (hekmatTransaction == null)
            {

                return (false, ".تراکنشی یافت نشد");
            }

            if (amount != hekmatTransaction.Price)
                return (false,"مبلغ تراکنش ارسالی معتبر نمی باشد.");

            if (hekmatTransaction.StatusCode == (int)HekmatStatusCodeEnum.Waiting &&
                hekmatTransaction.ErrorCode == (int)HekmatErrorCodeEnum.Waiting)
            {
                hekmatTransaction.StatusCode = (int)HekmatStatusCodeEnum.Unsuccessful;
                hekmatTransaction.ErrorCode = (int)HekmatErrorCodeEnum.Cancel;
                hekmatTransaction.HekmatTransactionResultMessage = "انصراف از پرداخت";
                var res = await UpdateAsync(hekmatTransaction, hekmatTransaction.Id, true);

                if (res.StatusCode == (int)HekmatStatusCodeEnum.Unsuccessful &&
                    res.ErrorCode == (int)HekmatErrorCodeEnum.Cancel)
                {
                    return (true, "با موفقیت عودت تراکنش حکمت درخواستی انجام شد.");
                }
            }

            return (false, "امکان تغییر وضعیت تراکنش درخواستی به حالت لغو شده وجود ندارد");
        }

    }
}