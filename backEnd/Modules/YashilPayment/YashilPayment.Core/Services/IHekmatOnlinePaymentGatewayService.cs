using System.Threading.Tasks;



using YashilPayment.Core.DTO;

namespace YashilPayment.Core.Services
{
    public interface IHekmatOnlinePaymentGatewayService
    {
        /// <summary>
        /// درخواست پرداخت
        /// </summary>
        /// <param name="orderId">شناسه سفارش</param>
        /// <returns></returns>
        Task<(string token, string signature, string errorMessae, bool isSucces)> GenerateTokenAsync(int orderId);
        /// <summary>برای تلاش Mixed فقط از Payment و Reservation آماده‌شده توکن می‌سازد.</summary>
        Task<(string token, string signature, string errorMessae, bool isSucces)> GenerateMixedTokenAsync(int orderId, int paymentId);
        /// <summary>
        /// بررسی  و تایید 
        /// تراکنش پرداخت کاربر وقتی از سمت درگاه بعد از پرداخت کاربر
        /// به متد
        /// CallBackUrl
        /// بر می گردد
        /// </summary>
        /// <returns></returns>
        Task<HekmatOnlinePaymentGatewayTransactionVerificationResponseDto> VerifyPaymentTransactionAsync(HekmatOnlinePaymentGatewayCallbackDto callbackDto);
        /// <summary>
        /// ساخت آدرس پرداخت
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        string CreatePaymentUrl(string token);

        /// <summary>
        /// بررسی وضعیت تراکنش بر اساس توکن دریافتی از درگاه
        /// </summary>
        /// <param name="token">توکن دریافتی از درگاه</param>
        /// <returns></returns>
        Task<HekmatOnlinePaymentGatewayInquiryPayResponseDto> InquiryPayAsync(string token);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        Task<HekmatOnlinePaymentGatewayTransactionVerificationResponseDto> VerifyPaymentTransactionByTokenAsync(string token);
        /// <summary>
        /// ابطال تراکنش بر اساس توکن
        /// </summary>
        /// <param name="token">توکن</param>
        /// <returns></returns>
        Task<HekmatOnlinePaymentGatewayVoidPayResponseDto> VoidPayAsync(string token);
    }
}
