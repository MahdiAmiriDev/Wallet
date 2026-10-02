using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Common;
using Newtonsoft.Json;
using Yashil.Common.Core.Classes;
using Yashil.Core.Configurations;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class HumaxService : IHumaxService
    {
        private readonly IHumaxApiConfiguration _humaxApiConfiguration;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAdditionalUserPropService _additionalUserPropService;

        public HumaxService(IHumaxApiConfiguration humaxApiConfiguration, IUserPrincipal userPrincipal, IHttpClientFactory httpClientFactory, IAdditionalUserPropService additionalUserPropService)
        {
            _humaxApiConfiguration = humaxApiConfiguration;
            _userPrincipal = userPrincipal;
            _httpClientFactory = httpClientFactory;
            _additionalUserPropService = additionalUserPropService;
        }
        /// <summary>
        /// برای کاربر جاری 
        /// register
        /// در سایت هیومکس  انجام می دهد و وارد داشبود کاربر می شود 
        /// </summary>
        /// <returns></returns>
        public async Task<HumaxRegisterUserResponseDto> Register()
        {

            var url = _humaxApiConfiguration.Url;
            if (_userPrincipal.Id > 0)
            {
                var humaxRegisterUserRequestDto = new HumaxRegisterUserRequestDto()
                {
                    APIKey = _humaxApiConfiguration.APIKey,
                    PartnerName = _humaxApiConfiguration.PartnerName,
                    PhoneNumber = _userPrincipal.UserName,
                    FirstName = _userPrincipal.Id.ToString(),
                    LastName = _userPrincipal.Id.ToString()
                };
                var httpClient = _httpClientFactory.CreateClient();
                var content = new FormUrlEncodedContent(humaxRegisterUserRequestDto.ToDictionary());
                using (var response = await httpClient.PostAsync(url, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<HumaxRegisterUserResponseDto>(result);

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

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
        public async Task<HumaxRegisterUserResponseDto> DoRegisterAndGetRedirectUrlToHumaxServiceForCurrentUserAsync()
        {

            var humaxRegisterResult = await Register();

            if (humaxRegisterResult == null || !humaxRegisterResult.Success)
                return new HumaxRegisterUserResponseDto()
                {
                    Message = "کاربر گرامی، سایت آزمون استعداد سنجی در دسترسی نمی باشد لطفا در زمان دیگری مجددا تلاس نمایید",
                    Url = string.Empty,
                    Success = false
                };

            var isSuccess = await _additionalUserPropService.UpdateParticipatedInHumaxTestValueAsync(_userPrincipal.Id);

            if (!isSuccess)
                return new HumaxRegisterUserResponseDto()
                {
                    Message = "کاربر گرامی فرآیند ورود به آزمون به خطا مواجه شد لطفا در زمان دیگر مجددا تلاش نمایید.",
                    Url = string.Empty,
                    Success = false
                };


            return new HumaxRegisterUserResponseDto()
            {
                Message = string.Empty,
                Url = humaxRegisterResult.Url,
                Success = true
            };

        }
    }

}