using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDtos.Payment;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilPayment.Core.Services
{
    public interface IHybridPaymentRequestService : IGenericService<HybridPaymentRequest, int>
    {
        IQueryable<HybridPaymentRequest> GetByCustomFilter(int? merchantId, int? yashilHekmatTerminalId,
            int? hybridPaymentRequestStatusType, string localReservationNumber, int? applicationId);


        /// <summary>
        /// محاسبه قیمت نقدی
        /// </summary>
        /// <param name="basePrice"></param>
        /// <param name="cashPaymentPercentage"></param>
        /// <param name="cashInstituteWage"></param>
        /// <param name="tax"></param>
        /// <param name="shippingAmount"></param>
        /// <param name="useFormula">برای محاسبه قیمت از فرمول استفاده شود؟</param>
        /// <returns></returns>
        public long CalculateCashPrice(long basePrice,
            int cashPaymentPercentage,
            decimal cashInstituteWage,
            int tax,
            int? shippingAmount,
            bool useFormula
            );

        /// <summary>
        /// محاسبه قیمت اقساطی
        /// </summary>
        /// <param name="basePrice"></param>
        /// <param name="cashPaymentPercentage"></param>
        /// <param name="installmentCount"></param>
        /// <param name="instituteWage"></param>
        /// <param name="supplierWage"></param>
        /// <param name="tax"></param>
        /// <param name="shippingAmount"></param>
        /// <param name="useFormula">برای محاسبه قیمت از فرمول استفاده شود؟</param>
        /// <returns></returns>
        public long CalculateHekmatPrice(long basePrice,
            int cashPaymentPercentage,
            int installmentCount,
            decimal instituteWage,
            decimal supplierWage,
            int tax,
            int? shippingAmount,
            bool useFormula);

        /// <summary>
        /// در صورتی که برای
        /// ReservationNumber و Terminald
        /// ارسالی از سمت سایت پذیرنده رکوردی
        /// در جدول
        /// hybridPaymentRequest
        /// بود
        ///  اطلاعات
        /// hybridPaymentRequest
        /// بهمراه قیمت باقیمانده حکمت و آنلاین
        /// برگردانده می شود در غیر اینصورت مقادیر
        ///  0 ,0 Null
        /// برگشت داده می شود
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="yashilHekmatTerminalId"></param>
        /// <param name="localReservationNumber"></param>
        /// <param name="applicationId"></param>
        /// <param name="useFormula">برای محاسبه قیمت از فرمول استفاده شود؟</param>
        /// <returns></returns>
        public Task<(long remainingHekmatPrice, long remainingCashPrice,
            HybridPaymentRequestDto hybridPaymentRequest)> GetHybridPaymentRequestDtoWithRemainingPricesAsync(int merchantId,
            int yashilHekmatTerminalId, string localReservationNumber,bool useFormula,int? applicationId);

        /// <summary>
        /// دریافت اطلاعات  hybridPayment
        /// بهمراه قیمت باقیمانده حکمت و آنلاین
        /// </summary>
        /// <param name="hybridPaymentRequestId"></param>
        /// <param name="useFormula">برای محاسبه قیمت از فرمول استفاده شود؟</param>
        /// <returns></returns>
        public Task<(long remainingHekmatPrice, long remainingCashPrice, HybridPaymentRequestDto
            hybridPaymentRequest)> GetHybridPaymentRequestDtoWithRemainingPricesAsync(int hybridPaymentRequestId,bool useFormula = true);

        IQueryable<HybridPaymentRequest> FindById(int id, bool readOnly = false);
        /// <summary>
        /// بروزرسانی وضعیت hybridPaymentRequest
        /// </summary>
        /// <param name="hybridPaymentHybridPaymentRequestId"></param>
        /// <param name="hybridPaymentRequestStatusTypeEnum"></param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>
        [ItemCanBeNull] Task<HybridPaymentRequest> UpdateHybridPaymentRequestStatusAsync(int hybridPaymentHybridPaymentRequestId, HybridPaymentRequestStatusTypeEnum hybridPaymentRequestStatusTypeEnum, bool saveAfterUpdate = false);
    }
}