using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilPayment.Core.Classes;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class HybridPaymentService : GenericService<HybridPayment, int>, IHybridPaymentService
    {
        private readonly IHybridPaymentRepository _hybridPaymentRepository;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IYashilLogger<HybridPayment> _yashilLogger;
        private readonly IAppConfigService _appConfigService;
        private readonly ITaxCategoryService _taxCategoryService;

        public HybridPaymentService(IUnitOfWork unitOfWork, IHybridPaymentRepository hybridPaymentRepository,
            IUserPrincipal userPrincipal, IEventPublisher eventPublisher,
            ApplicationConfiguration applicationConfiguration, IYashilLogger<HybridPayment> yashilLogger, IAppConfigService appConfigService, ITaxCategoryService taxCategoryService)
            : base(unitOfWork, hybridPaymentRepository, userPrincipal, eventPublisher)
        {
            _hybridPaymentRepository = hybridPaymentRepository;
            _applicationConfiguration = applicationConfiguration;
            _yashilLogger = yashilLogger;
            _appConfigService = appConfigService;
            _taxCategoryService = taxCategoryService;
        }

        public IQueryable<HybridPayment> GetByCustomFilter(int? hybridPaymentRequestId, int? hybridPaymentStatusType, int? paymentMethodType, int? applicationId)
        {
            return _hybridPaymentRepository.GetByCustomFilter(hybridPaymentRequestId, hybridPaymentStatusType, paymentMethodType, applicationId);
        }

        public Task<HybridPayment> UpdatePaymentStatusAsync(int hybridPaymentId, HybridPaymentStatusTypeEnum hybridPaymentStatusTypeEnum,
            PaymentMethodTypesEnum paymentMethodType, string message, bool saveAfterUpdate = false)
        {
            return UpdatePaymentStatusAsync(hybridPaymentId, hybridPaymentStatusTypeEnum, paymentMethodType, message, null, saveAfterUpdate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hybridPaymentId"></param>
        /// <param name="hybridPaymentStatusTypeEnum"></param>
        /// <param name="paymentMethodType"></param>
        /// <param name="message"></param>
        /// <param name="referenceNumber">شماره پیگیری تراکنش انجامشده است. (این مقدار یکتا است و به ازای هر تراکنش از سوی فنآوا کارت به پذیرنده ارسال میگردد.) </param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>
        public Task<HybridPayment> UpdatePaymentStatusAsync(int hybridPaymentId, HybridPaymentStatusTypeEnum hybridPaymentStatusTypeEnum,
            PaymentMethodTypesEnum paymentMethodType, string message, string referenceNumber,
            bool saveAfterUpdate = false)
        {
            return UpdatePaymentStatusAsync(hybridPaymentId, hybridPaymentStatusTypeEnum, paymentMethodType, message, referenceNumber, null, null, null, saveAfterUpdate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hybridPaymentId"></param>
        /// <param name="hybridPaymentStatusTypeEnum"></param>
        /// <param name="paymentMethodType"></param>
        /// <param name="message"></param>
        /// <param name="referenceNumber"></param>
        /// <param name="customerRefNum"></param>
        /// <param name="goodReferenceId"></param>
        /// <param name="merchantData"></param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>
        public async Task<HybridPayment> UpdatePaymentStatusAsync(int hybridPaymentId, HybridPaymentStatusTypeEnum hybridPaymentStatusTypeEnum,
            PaymentMethodTypesEnum paymentMethodType, string message, string referenceNumber, string customerRefNum,
            string goodReferenceId, string merchantData, bool saveAfterUpdate = false)
        {

            var hybridPayment = await GetAsync(hybridPaymentId, false);
            if (hybridPayment.PaymentMethodType != (int)PaymentMethodTypesEnum.Wallet &&
                hybridPayment.HybridPaymentStatusType == (int)PaymentStatusTypesEnum.Paid)
            {
                _yashilLogger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error, (int)Events.PaymentException, nameof(HybridPayment), $"خطا در بروز رسانی پرداخت موفق.یعنی این پرداخت قبلا با موفقیت انجام شده و نباید بروز رسانی شود کد پرداخت: {hybridPayment.Id}", null);
                return hybridPayment;
            }
            if (paymentMethodType == PaymentMethodTypesEnum.Ansar)
            {
                hybridPayment.PaidDateUtcInt = DateFunction.ToShamsiInt(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            hybridPayment.PaymentMethodType = (int)paymentMethodType;
            hybridPayment.HybridPaymentStatusType = (int)hybridPaymentStatusTypeEnum;
            hybridPayment.ModifyBy = _applicationConfiguration.SysUserId;
            hybridPayment.PaidDateUtc = DateTime.Now;
            hybridPayment.AuthorizationTransactionResult = message;

            if (!string.IsNullOrEmpty(referenceNumber))
            {
                hybridPayment.AuthorizationTransactionId = referenceNumber;
            }
            return await UpdateAsync(hybridPayment, hybridPaymentId, saveAfterUpdate);

        }

        public IQueryable<HybridPayment> FindById(int id, bool readOnly = false)
        {
            return _hybridPaymentRepository.FindById(id, readOnly);
        }
        public IQueryable<HybridPayment> FindByReservationNumber(string reservationNumber, bool readOnly = false)
        {
            return _hybridPaymentRepository.FindByReservationNumber(reservationNumber, readOnly);
        }
        /// <summary>
        /// دریافت درصد مالیات بر ارزش افزوده
        /// </summary>
        /// <returns></returns>
        public async Task<decimal?> GetTaxPercentageAsync()
        {
            try
            {
                var taxCategoryId = await _appConfigService.GetStringValueByApplicationIdAsync(
                    ApplicationConfigKey.YashilPaymentTaxDefaultTaxCategoryId,
                    _applicationConfiguration.ApplicationId);
                var canConvertedTaxCategoryIdToInt = Int32.TryParse(taxCategoryId, out int taxCategoryIdInt);
                if (!canConvertedTaxCategoryIdToInt)
                {
                    return null;
                }
                return await _taxCategoryService.FindById(taxCategoryIdInt)
                    .Select(x => x.Percentage)
                    .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
           
        }

        /// <summary>
        /// دریافت قیمت پایه بر اساس شناسه
        /// hybridPayment
        /// </summary>
        /// <param name="hybridPaymentId"></param>
        /// <returns></returns>
        public Task<long> GetBasePriceAsync(int hybridPaymentId)
        {
            return _hybridPaymentRepository.GetAll(true, _applicationConfiguration.ApplicationId)
                .Where(hp => hp.Id == hybridPaymentId)
                .Select(hp => hp.HybridPaymentRequest.BasePrice)
                .FirstOrDefaultAsync();
        }
    }
}
