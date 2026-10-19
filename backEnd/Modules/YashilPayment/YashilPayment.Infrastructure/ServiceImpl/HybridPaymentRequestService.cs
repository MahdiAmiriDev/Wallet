using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core.CustomDtos.Payment;
using Yashil.Core.Enums;
using YashilPayment.Core.Repositories;
using YashilPayment.Core.Services;

namespace YashilPayment.Infrastructure.ServiceImpl
{
    public class HybridPaymentRequestService : GenericService<HybridPaymentRequest, int>, IHybridPaymentRequestService
    {
        private readonly IHybridPaymentRequestRepository _hybridPaymentRequestRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public HybridPaymentRequestService(IUnitOfWork unitOfWork,
            IHybridPaymentRequestRepository hybridPaymentRequestRepository, IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher, ApplicationConfiguration applicationConfiguration)
            : base(unitOfWork, hybridPaymentRequestRepository, userPrincipal, eventPublisher)
        {
            _hybridPaymentRequestRepository = hybridPaymentRequestRepository;
            _userPrincipal = userPrincipal;
            _applicationConfiguration = applicationConfiguration;
        }

        public IQueryable<HybridPaymentRequest> GetByCustomFilter(int? merchantId, int? yashilHekmatTerminalId,
            int? hybridPaymentRequestStatusType, string localReservationNumber, int? applicationId)
        {
            return _hybridPaymentRequestRepository.GetByCustomFilter(merchantId, yashilHekmatTerminalId,
                hybridPaymentRequestStatusType, localReservationNumber, applicationId);
        }


        public long CalculateCashPrice(long basePrice,
            int cashPaymentPercentage,
            decimal cashInstituteWage,
            int tax,
            int? shippingAmount,
            bool useFormula = true)
        {
            long cashBasePrice = default(long);

            // درصد پرداخت نقدی روی قیمت پایه محصول اعمال می شود و مبلغ پایه نقدی بدست می آید
            cashBasePrice = basePrice * cashPaymentPercentage / 100;

            if (useFormula)
            {
                // سود نقدی : این مبلغ سود برای سایت یا شرکت می باشد
                var cashInstituteWagePriceExlTax =
                    (cashBasePrice) * cashInstituteWage / 100;
                // مبلغ مالیات برای بخش نقدی
                var cashTotalTax =
                    (cashBasePrice +
                     cashInstituteWagePriceExlTax) *
                    tax / 100;
                // مبلغ نهایی 
                return (long)(cashBasePrice + cashInstituteWagePriceExlTax +
                              cashTotalTax +
                              (shippingAmount.HasValue &&
                               cashPaymentPercentage == 100 ?
                                  shippingAmount.Value
                                  : 0));
            }

            return cashBasePrice;

        }

        public long CalculateHekmatPrice(long basePrice,
            int cashPaymentPercentage,
            int installmentCount,
            decimal instituteWage,
            decimal supplierWage,
            int tax,
            int? shippingAmount,
            bool useFormula = true)
        {
            // درصد پرداخت اقساطی روی قیمت پایه محصول اعمال می شود و مبلغ پایه اقساطی بدست می آید
            long hekmatBasePrice = basePrice * (100 - cashPaymentPercentage) / 100;

            if (useFormula)
            {

                // سود اقساطی سایت یا شرکت : این مبلغ سود برای سایت یا شرکت می باشد
                var hekmatInstituteWagePriceExlTax =
                    (hekmatBasePrice) * instituteWage / 100;

                // سود تامین کننده : این مبلغ سود تامیین کننده : درصد سود اقساطی تامیین کننده در تعداد قسط تقسیم بر 100
                var hekmatSupplierWagePriceExlTax = (installmentCount * supplierWage / 100) * (hekmatBasePrice + hekmatInstituteWagePriceExlTax);


                // مبلغ مالیات برای بخش اقساطی
                var hekmatTotalTax =
                    (hekmatBasePrice +
                     hekmatInstituteWagePriceExlTax +
                     hekmatSupplierWagePriceExlTax) *
                    tax / 100;
                //قیمت نهایی 
                return (long)(hekmatBasePrice + hekmatInstituteWagePriceExlTax +
                              hekmatSupplierWagePriceExlTax + hekmatTotalTax +
                              (shippingAmount.HasValue &&
                               cashPaymentPercentage is < 100 and >= 0 ?
                                  shippingAmount.Value
                                  : 0));
            }

            return hekmatBasePrice;
        }

        public Task<(long remainingHekmatPrice, long remainingCashPrice, HybridPaymentRequestDto
            hybridPaymentRequest)> GetHybridPaymentRequestDtoWithRemainingPricesAsync(int hybridPaymentRequestId, bool useFormula = true)
        {
            var query = FindById(hybridPaymentRequestId, true);
            return GetHybridPaymentRequestDtoWithRemainingPricesAsync(query, useFormula);
        }


        public Task<(long remainingHekmatPrice, long remainingCashPrice, HybridPaymentRequestDto
            hybridPaymentRequest)> GetHybridPaymentRequestDtoWithRemainingPricesAsync(
            int merchantId, int yashilHekmatTerminalId,
            string reservationNumber, bool useFormula, int? applicationId)
        {
            var query = GetByCustomFilter(merchantId, yashilHekmatTerminalId, null,
                reservationNumber, applicationId);
            return GetHybridPaymentRequestDtoWithRemainingPricesAsync(query, useFormula);
        }

        /// <summary>
        /// دریافت اطلاعات hybridPaymentRequest
        /// بهمراه قیمت باقیمانده اقساطی و آنلاین
        /// </summary>
        /// <param name="query"></param>
        /// <param name="useFormula">در پرداخت نقدی اقساطی طرح باقرالعلوم مشخص می کند که از فرمول استفاده بشود یا نه</param>
        /// <returns></returns>
        private async Task<(long remainingHekmatPrice, long remainingCashPrice,
            HybridPaymentRequestDto hybridPaymentRequest)> GetHybridPaymentRequestDtoWithRemainingPricesAsync(
            IQueryable<HybridPaymentRequest> query, bool useFormula = true)
        {
            var hybridPaymentRequest = await query
                .Select(x => new HybridPaymentRequestDto
                {
                    Id = x.Id,
                    BasePrice = x.BasePrice,
                    InstallmentCount = x.InstallmentCount.Value,
                    InstituteWage = x.InstituteWage,
                    SupplierWage = x.SupplierWage,
                    CashInstituteWage = x.CashInstituteWage,
                    CashSupplierWage = x.CashSupplierWage,
                    CashPaymentPercentage = x.CashPaymentPercentage,
                    AdditionalData = x.AdditionalDate,
                    HybridPaymentRequestStatusType = x.HybridPaymentRequestStatusType,
                    ReferenceNumber = x.Title,
                    ReservationNumber = x.LocalReservationNumber,
                    ShippingAmount = x.ShippingAmount.Value,
                    Tax = x.Tax,
                    HybridPayments = x.HybridPayment.Select(c => new HybridPaymentDto
                    {
                        Id = c.Id,
                        SaleType = c.SaleType,
                        PaymentMethodType = c.PaymentMethodType,
                        HybridPaymentStatusType = c.HybridPaymentStatusType,
                        Price = c.Price,
                        AuthorizationTransactionResult = c.AuthorizationTransactionResult,
                        PaidDateInt = c.PaidDateInt,
                        ReservationNumber = c.ReservationNumber
                    })
                }).FirstOrDefaultAsync();
            if (hybridPaymentRequest != null && hybridPaymentRequest.CashPaymentPercentage.HasValue)
            {
                // حال مبلغ باقیمانده اقساطی را محاسبه میکنیم
                // ممکن است قبلا پرداخت اقساطی داشته باشد و بعدا مبلغ اصلاح شود
                // که البته الان این موضوع کاربردی ندارد ولی بعدا برای اصلاح تراکنشات این مورد درست است
                var hekmatPrice = CalculateHekmatPrice(hybridPaymentRequest.BasePrice,
                    hybridPaymentRequest.CashPaymentPercentage.Value,
                    hybridPaymentRequest.InstallmentCount,
                    hybridPaymentRequest.InstituteWage,
                    hybridPaymentRequest.SupplierWage,
                    hybridPaymentRequest.Tax,
                    hybridPaymentRequest.ShippingAmount, useFormula);

                var hekmatPricePaid = hybridPaymentRequest.HybridPayments
                    .Where(x => x.SaleType == (int)SaleTypesEnum.InstallmentPayment &&
                                x.HybridPaymentStatusType == (int)HybridPaymentStatusTypeEnum.Paid)
                    .Sum(x => x.Price);
                var remainingHekmatPaymentPrice = hekmatPrice - hekmatPricePaid;

                var cashPrice = CalculateCashPrice(hybridPaymentRequest.BasePrice,
                    hybridPaymentRequest.CashPaymentPercentage.Value,
                    hybridPaymentRequest.CashInstituteWage,
                    hybridPaymentRequest.Tax,
                    hybridPaymentRequest.ShippingAmount, useFormula);

                var cashPricePaid = hybridPaymentRequest.HybridPayments
                    .Where(x => x.SaleType == (int)SaleTypesEnum.CashPayment &&
                                x.HybridPaymentStatusType == (int)HybridPaymentStatusTypeEnum.Paid)
                    .Sum(x => x.Price);
                var remainingCashPaymentPrice = cashPrice - cashPricePaid;

                return (remainingHekmatPaymentPrice, remainingCashPaymentPrice,
                    hybridPaymentRequest);
            }

            return (0, 0, null);
        }

        public IQueryable<HybridPaymentRequest> FindById(int id, bool readOnly = false)
        {
            return _hybridPaymentRequestRepository.FindById(id, readOnly);
        }

        public async Task<HybridPaymentRequest> UpdateHybridPaymentRequestStatusAsync(int hybridPaymentRequestId,
            HybridPaymentRequestStatusTypeEnum hybridPaymentRequestStatusTypeEnum, bool saveAfterUpdate = false)
        {
            var hybridPaymentRequest = await GetAsync(hybridPaymentRequestId, false);
            if (hybridPaymentRequest.HybridPaymentRequestStatusType ==
                (int)HybridPaymentRequestStatusTypeEnum.IncompletePayment
                && hybridPaymentRequestStatusTypeEnum != HybridPaymentRequestStatusTypeEnum.Complete)
            {
                //فقط در صورتی وضعیت آن تغییر می کند که 
                //در صورتی که وضعیت تراکنش پرداخت ناقص باشد،
                return hybridPaymentRequest;
            }

            if (hybridPaymentRequest.HybridPaymentRequestStatusType == (int)HybridPaymentRequestStatusTypeEnum.Complete &&
                hybridPaymentRequestStatusTypeEnum != HybridPaymentRequestStatusTypeEnum.IncompletePayment &&
                hybridPaymentRequestStatusTypeEnum != HybridPaymentRequestStatusTypeEnum.Refund)
            {
                /*
                 *فقط در صورتی می توان درخواست پرداخت  کامل را تغییر وضعیت داد که وضعیت جدید
                 * پرداخت ناقص یا انصراف بعد از تعیید نهایی باشد
                 */
                return hybridPaymentRequest;
            }

            hybridPaymentRequest.ModificationDate = DateTime.Now;
            hybridPaymentRequest.ModifyBy =
                _userPrincipal.Id == 0 ? _applicationConfiguration.SysUserId : _userPrincipal.Id;
            hybridPaymentRequest.HybridPaymentRequestStatusType = (int)hybridPaymentRequestStatusTypeEnum;
            await UpdateAsync(hybridPaymentRequest, hybridPaymentRequestId, saveAfterUpdate);
            return hybridPaymentRequest;
        }
    }
}