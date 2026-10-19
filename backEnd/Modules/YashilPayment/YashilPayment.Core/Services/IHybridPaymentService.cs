using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilPayment.Core.Services
{
    public interface IHybridPaymentService : IGenericService<HybridPayment, int>
    {
        IQueryable<HybridPayment> GetByCustomFilter(int? hybridPaymentRequestId, int? hybridPaymentStatusType,
            int? paymentMethodType, int? applicationId);


        Task<HybridPayment> UpdatePaymentStatusAsync(int hybridPaymentId, HybridPaymentStatusTypeEnum hybridPaymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, bool saveAfterUpdate = false);

        Task<HybridPayment> UpdatePaymentStatusAsync(int hybridPaymentId, HybridPaymentStatusTypeEnum hybridPaymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, string referenceNumber,
            bool saveAfterUpdate = false);

        /// <summary>
        /// این متد عمدتا در هنگام پرداخت فن اوا کارت یا انصار مورد استفاده قرار میگیرد
        /// در هنگام پرداخت با فن آوا کارت
        /// پارامترهای بیشتری توسط بانک ارسال میشود که این پارامتر ها نیز ذخیره می شوند
        /// </summary>
        /// <param name="hybridPaymentId"></param>
        /// <param name="hybridPaymentStatusType"></param>
        /// <param name="paymentMethodType"></param>
        /// <param name="message"></param>
        /// <param name="referenceNumber"></param>
        /// <param name="customerRefNum"></param>
        /// <param name="goodReferenceId"></param>
        /// <param name="merchantData"></param>
        /// <param name="saveAfterUpdate"></param>
        /// <returns></returns>
        Task<HybridPayment> UpdatePaymentStatusAsync(int hybridPaymentId, HybridPaymentStatusTypeEnum hybridPaymentStatusType,
            PaymentMethodTypesEnum paymentMethodType, string message, string referenceNumber, string customerRefNum,
            string goodReferenceId, string merchantData, bool saveAfterUpdate = false);

        IQueryable<HybridPayment> FindById(int id, bool readOnly = false);
        IQueryable<HybridPayment> FindByReservationNumber(string reservationNumber, bool readOnly = false);

        /// <summary>
        /// دریافت درصد مالیات بر ارزش افزوده
        /// </summary>
        /// <returns></returns>
        Task<decimal?> GetTaxPercentageAsync();

        /// <summary>
        /// دریافت قیمت پایه بر اساس شناسه
        /// hybridPayment
        /// </summary>
        /// <param name="hybridPaymentId"></param>
        /// <returns></returns>
        Task<long> GetBasePriceAsync(int hybridPaymentId);
    }
}