using System.Threading.Tasks;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface IHumaxService
    {
        /// <summary>
        /// برای کاربر جاری 
        /// register
        /// در سایت هیومکس  انجام می دهد و وارد داشبود کاربر می شود 
        /// </summary>
        /// <returns></returns>
        Task<HumaxRegisterUserResponseDto> Register();

        /// <summary>
        /// این متد جهت ثبت نام کاربر جاری در سرویس
        /// humax
        ///کاربرد دارد همچنین عملیات بروزرسانی
        /// AddtionalUserProp , PrticipatedInHumaxTest
        /// را نیز انجام می دهد و در نهایت
        /// url
        /// هدایت کاربر به سامانه ی Humax
        /// را دریافت می کند و برمیگرداند 
        /// </summary>
        /// <returns></returns>
        Task<HumaxRegisterUserResponseDto> DoRegisterAndGetRedirectUrlToHumaxServiceForCurrentUserAsync();
    }
}
