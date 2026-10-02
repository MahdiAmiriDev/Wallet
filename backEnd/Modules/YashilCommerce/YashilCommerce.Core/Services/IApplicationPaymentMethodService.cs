using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IApplicationPaymentMethodService : IGenericService<ApplicationPaymentMethod, int>
    {

        IQueryable<ApplicationPaymentMethod> GetByCustomFilter(int? paymentMethodType);
        Task<IQueryable<PaymentMethodType>> GetApplicationPaymentMethodsAsync(int applicationId, bool readOnly);
        /// <summary>
        /// لیست مدل های پرداخت آنلاین و حکمت را در صورتی که به برنامه کاربر وصل بود را بر می گرداند
        /// </summary>
        /// <returns></returns>
        IQueryable<ApplicationPaymentMethod> GetOnlineAndHekmatPaymentMethodsForSelect();

        /// <summary>
        /// روش های پرداخت حکمت و ناجا را بر می گرداند
        /// </summary>
        /// <returns></returns>
        IQueryable<PaymentMethodType> GetNajaAndHekmatPaymentMethodsForSelect();
    }
}
