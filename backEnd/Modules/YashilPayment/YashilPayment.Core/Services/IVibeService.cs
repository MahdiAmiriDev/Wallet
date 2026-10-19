
using System.Threading.Tasks;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using YashilPayment.Core.DTO;

namespace YashilPayment.Core.Services
{
    public interface IVibeService
    {
        /// <summary>
        /// این متد برای ساخت سفارش در درگاه وایب و دریافت لینک پرداخت کاربرد دارد
        /// </summary>
        /// <param name="paymentInformation"></param>
        /// <param name="orderData"></param>
        /// <returns></returns>
        Task<CreateVibeOrderResponseDto> CreateOrderAsync(PaymentInformation paymentInformation,Order orderData);

        /// <summary>
        /// این متد برای ثبت نهایی سفارش فراخوانی می شود
        /// و به عنوان ورودی 
        /// شناسه پیگیری که همان 
        /// </summary>
        /// <param name="refNumber"></param>
        /// <param name="status">وضعیت پرداخت</param>
        /// <returns></returns>
        Task<VerifyVibeOrderResponseDto> VerifyVibeOrderAsync(string refNumber, string status);
    }
}
