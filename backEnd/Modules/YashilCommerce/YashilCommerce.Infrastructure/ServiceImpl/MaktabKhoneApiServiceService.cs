using System;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Infrastructure.Extensions;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Interfaces;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class MaktabKhoneApiServiceService : IMaktabKhoneApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IYashilLogger<MaktabKhoneApiServiceService> _logger;
        private readonly IConfiguration _configuration;
        public MaktabKhoneApiServiceService(IHttpClientFactory httpClientFactory, IYashilLogger<MaktabKhoneApiServiceService> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<(bool isSuccess, string message, int subscriptionId)> AddUserToSubscriptionPlanAsync(string userMobileNumber)
        {
            if (string.IsNullOrEmpty(userMobileNumber) || userMobileNumber == "0" || !userMobileNumber.IsValidMobileNumber())
                return (false, $"شماره موبایل ارسالی به ای پی آی معتبر نمی باشد -  {userMobileNumber}.", 0);

            GetValeFromAppSetting("MaktabKhoneApiConfiguration:SubscriptionUrl", out var apiUrl);

            if (string.IsNullOrEmpty(apiUrl))
                return (false, $"آدرس ای پی آی  معتبر نمی باشد - MaktabKhoneApiConfiguration:SubscriptionUrl -   {apiUrl}.", 0);

            GetValeFromAppSetting("MaktabKhoneApiConfiguration:ApiKey", out var header);

            if (string.IsNullOrEmpty(header))
                return (false, $"هدر ای پی آی معتبر نمی باشد - MaktabKhoneApiConfiguration:ApiKey -  {header}.", 0);

            GetValeFromAppSetting("MaktabKhoneApiConfiguration:PlanId", out var planId);

            if (string.IsNullOrEmpty(planId))
                return (false, $"پلن ای پی آی معتبر نمی باشد - MaktabKhoneApiConfiguration:PlanId -  {planId}.", 0);

            var hasUserPurchaseSubscriptionBefore = await CheckHasUserPurchaseSubscriptionBeforeAsync(userMobileNumber, planId, apiUrl, header);

            if (!hasUserPurchaseSubscriptionBefore.isSuccess)
                return (false, hasUserPurchaseSubscriptionBefore.message, 0);

            var apiResponse = await SendHttpRequestForAddUserToSubscriptionPlanAsync(userMobileNumber, planId, apiUrl, header);


            return (apiResponse.IsSuccessStatusCode, apiResponse.Message, apiResponse.SubscriptionId);
        }

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر قبلا اشتراک را خریداری کرده است یا نه؟
        /// </summary>
        /// <param name="userMobileNumber"></param>
        /// <param name="planId"></param>
        /// <param name="url"></param>
        /// <param name="apiHeader"></param>
        /// <returns></returns>
        private async Task<(bool isSuccess, string message)> CheckHasUserPurchaseSubscriptionBeforeAsync(string userMobileNumber, string planId, string url, string apiHeader)
        {
            var response = await SendHttpRequestForAddUserToSubscriptionPlanAsync(userMobileNumber, planId, url + "?dry_run=true", apiHeader, true);

            if (!response.IsSuccessStatusCode)
                return (false, "کاربر گرامی شما اشتراک مورد نظر را از قبل خریداری کرده اید.");

            return (true, string.Empty);
        }

        /// <summary>
        /// این متد برای ارسال درخواست http
        /// جهت افزودن کاربر به اشتراک مکتب خونه کابرد دارد
        /// </summary>
        /// <param name="mobileNumber">شماره موبایل کاربر برای اشتراک </param>
        /// <param name="planId"></param>
        /// <param name="url"></param>
        /// <param name="apiHeader">api key </param>
        /// <param name="dryRun">چک برای اینکه کاربر قبلا اشتراک خریده است یا خیر</param>
        /// <returns></returns>
        private async Task<MaktabKhoneAddToSubscriptionApiResponseDto> SendHttpRequestForAddUserToSubscriptionPlanAsync(
            string mobileNumber,
            string planId,
            string url,
            string apiHeader,
            bool dryRun = false
            )
        {
            var defaultErrorMessage = new MaktabKhoneAddToSubscriptionApiResponseDto()
            {
                IsSuccessStatusCode = false,
                Message = "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.",
                SubscriptionId = default(int)

            };

            var requestModel = new
            {
                tessera = mobileNumber,
                plan_id = int.Parse(planId),
            };

            if (string.IsNullOrEmpty(apiHeader))
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                    "مقدار header در appSetting تنظیم نشده است.", null);

                return defaultErrorMessage;
            }
            string jsonBody;
            try
            {
                jsonBody = JsonConvert.SerializeObject(requestModel);

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                    "بروز خطا به هنگام تبدیل مدل به json برای ارسال درخواست http", e);

                return defaultErrorMessage;
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                httpClient.DefaultRequestHeaders.Add("API-Key", apiHeader);

                var apiResponse = await httpClient.PostAsync(new Uri(url), new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                if (!apiResponse.IsSuccessStatusCode)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                        $"بروز خطا statusCode بازگشتی از api مکتب خونه موفقیت آمیز نبود.{await apiResponse.Content.ReadAsStringAsync()}", null);

                    return defaultErrorMessage;
                }

                var response = JsonConvert.DeserializeObject<MaktabKhoneAddToSubscriptionApiResponseDto>(await apiResponse.Content.ReadAsStringAsync());

                if (response == null ||
                     (dryRun && response.Error &&  !response.Message.Trim().ToLower().Contains("ok")) ||
                     (!dryRun && response.Error)
                    )
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                        "پاسخ دریافتی از api مکتب خونه معتبر نمی باشد.", null);

                    return defaultErrorMessage;
                }

                response.IsSuccessStatusCode = true;
                response.Message = "عملیات با موفقیت انجام شد.";
                return response;

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                    "بروز خطا به هنگام ارسال درخواست به api مکتب خونه", e);

                return defaultErrorMessage;
            }

        }

        public async Task<(bool isSuccess, string message, string redirectUrl)> GetAuthenticationToken(string userMobileNumber)
        {
            if (string.IsNullOrEmpty(userMobileNumber) || userMobileNumber == "0" || !userMobileNumber.IsValidMobileNumber())
                return (false, $"شماره موبایل ارسالی به ای پی آی معتبر نمی باشد -  {userMobileNumber}.", string.Empty);

            GetValeFromAppSetting("MaktabKhoneApiConfiguration:AuthenticationUrl", out var apiUrl);

            if (string.IsNullOrEmpty(apiUrl))
                return (false, $"آدرس ای پی آی  معتبر نمی باشد - MaktabKhoneApiConfiguration:AuthenticationUrl -   {apiUrl}.", string.Empty);

            GetValeFromAppSetting("MaktabKhoneApiConfiguration:ApiKey", out var apiKey);

            if (string.IsNullOrEmpty(apiKey))
                return (false, $"هدر ای پی آی معتبر نمی باشد - MaktabKhoneApiConfiguration:ApiKey -  {apiKey}.", string.Empty);


            var apiResponse = await SendHttpRequestForGetAuthenticationTokenAsync(userMobileNumber, apiKey, apiUrl);

            return (apiResponse.IsSuccess, apiResponse.Message, apiResponse.RedirectUrl);
        }

        /// <summary>
        /// این متد جهت ارسال درخواست http
        /// جهت دریافت توکن احراز هویت کاربر از سامانه مکت خونه کاربرد دارد
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <param name="apiKey"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<MaktabKhoneAuthenticationApiResponseDto> SendHttpRequestForGetAuthenticationTokenAsync(
            string mobileNumber,
            string apiKey,
            string url)
        {
            var defaultErrorMessage = new MaktabKhoneAuthenticationApiResponseDto()
            {
                IsSuccess = false,
                Message = "بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.",
                Token = string.Empty

            };

            var requestModel = new
            {
                tessera = mobileNumber,
                api_key = apiKey,
            };

            if (string.IsNullOrEmpty(url))
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                    "مقدار Url در appSetting تنظیم نشده است.", null);

                return defaultErrorMessage;
            }

            string jsonBody;

            try
            {
                jsonBody = JsonConvert.SerializeObject(requestModel);

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                    "بروز خطا به هنگام تبدیل مدل به json برای ارسال درخواست http", e);

                return defaultErrorMessage;
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var apiResponse = await httpClient.PostAsync(new Uri(url), new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                if (!apiResponse.IsSuccessStatusCode)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                        $"بروز خطا statusCode بازگشتی از api مکتب خونه موفقیت آمیز نبود.{await apiResponse.Content.ReadAsStringAsync()}",
                        null);

                    return defaultErrorMessage;
                }
                var response = JsonConvert.DeserializeObject<MaktabKhoneAuthenticationApiResponseDto>(await apiResponse.Content.ReadAsStringAsync());

                if (response == null)
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                        "پاسخ دریافتی از api مکتب خونه معتبر نمی باشد.", null);

                    return defaultErrorMessage;
                }

                if (response.Message.Trim().ToLower().Contains("invalid"))
                {
                    response.IsSuccess = false;
                    response.Message = "بروز خطا به نگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.";
                    return response;
                }

                GetValeFromAppSetting("MaktabKhoneApiConfiguration:RedirectToSiteUrl", out var authenticationUrl);

                if (string.IsNullOrEmpty(authenticationUrl))
                    return defaultErrorMessage;

                response.IsSuccess = true;
                response.Message = "عملیات با موفقیت انجام شد.";
                var urlPostfix = "auth_token=" + response.Token;
                response.RedirectUrl = authenticationUrl += urlPostfix;
                return response;

            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                    "بروز خطا به هنگام ارسال درخواست به api مکتب خونه", e);

                return defaultErrorMessage;
            }

        }

        /// <summary>
        /// این متد جهت دریافت مقدار از appSetting
        /// با استفاده از کلید ارسالی 
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="url"></param>
        private void GetValeFromAppSetting(string keyName, out string url)
        {
            url = null;

            try
            {
                url = _configuration.GetValue<string>(keyName);

                if (string.IsNullOrEmpty(url))
                    throw new ArgumentNullException(nameof(url));
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidModel, nameof(MaktabKhoneApiServiceService),
                    $"بروز خطا به هنگام دریافت آدرس api مربوط به مکت خونه از appSetting - {keyName} ", e);
            }
        }
    }
}
