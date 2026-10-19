using AutoMapper;
using System.Text;
using System.Text.Json;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.CustomViewModels.SaderatPaymentModels;

namespace YashilPayment.Web.Areas.Payment.Controllers
{
    public class BaseSaderatPaymentController<T> : YashilMvcBaseController<T>
    {
        private readonly ISaderatPaymentConfiguration _saderatPaymentConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseSaderatPaymentController(ApplicationConfiguration applicationConfiguration, 
            IMapper mapper, IYashilLogger<T> logger, IPermissionService permissionService, 
            ISaderatPaymentConfiguration saderatPaymentConfiguration, IHttpContextAccessor httpContextAccessor) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _saderatPaymentConfiguration = saderatPaymentConfiguration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// متد ایجاد توکن درگاه پرداخت
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="reservationNumber"></param>
        /// <returns></returns>
        public async Task<string> GenerateTokenAsync(long amount, string reservationNumber)
        {
            var httpHandler = new HttpClientHandler();

            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var tokenRequestModel = new SaderatGenerateTokenRequestModel
            {
                Amount = amount,
                callbackURL = _saderatPaymentConfiguration.CallBackUrl,
                invoiceID = reservationNumber,
                terminalID = (long)Convert.ToDouble(_saderatPaymentConfiguration.TerminalId)
            };

            var client = new HttpClient(httpHandler);

            client.DefaultRequestHeaders.Host =
                ApplicationConfiguration.BaseUrl
                    .Replace("https://", "")
                    .Replace("/", "");

            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://sepehr.shaparak.ir:8081/V1/PeymentApi/GetToken");

            var data = JsonSerializer.Serialize(tokenRequestModel);
            request.Content = new StringContent(data, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var generateTokenResponseModel =
                    JsonSerializer.Deserialize<SaderatGenerateTokenResponseModel>
                        (result);

                if (generateTokenResponseModel?.Status == 0)
                {
                    return generateTokenResponseModel.Accesstoken;
                }
                else
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                        "Payment", $"خطا در ساخت توکن برای سفارش با کد: {reservationNumber} --" +
                                   $"{generateTokenResponseModel?.Status}", null);
                }
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                      "Payment", $"خطا در ساخت توکن برای سفارش با کد: {reservationNumber}", e);

            }

            return string.Empty;
        }
        /// <summary>
        /// متد هدایت کاربر به درگاه پرداخت
        /// </summary>
        /// <param name="token"></param>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        public async Task RedirectToIpg(string token, long terminalId)
        {
            // return Redirect($"https://sepehr.shaparak.ir:8080/Pay?TerminalID={terminalId}&token={token}&getMethod=1");
            var remotePost = new RemotePost(_httpContextAccessor)
            {
                Url = "https://sepehr.shaparak.ir:8080/Pay"
            };
            // اضافه کردن اطلاعات توکن و پست کردن آن به صفحه درگاه
            remotePost.Add("terminalID", terminalId.ToString());
            remotePost.Add("token", token);
            remotePost.Add("getMethod", "1");
            await remotePost.PostAsync();
        }
        /// <summary>
        /// وضعیت موفقیت آمیز بودن یا عدم موفقیت تراکنش را برمی گرداند
        /// </summary>
        /// <param name="state">وضعیت تراکنش</param>
        /// <returns></returns>
        public bool RequestUnpack(out int state)
        {
            state = 1;

            if (!CheckRequestField().IsSuccess)
            {
                state = 1;
                return false;
            }

            if (int.TryParse(Convert.ToString(Request.Form["respcode"]), out state))
            {
                if (!(state == 0))
                {
                    state = 1;
                    return false;
                }
                state = 0;
                return true;
            }

            return false;
        }
        /// <summary>
        /// برسی فیلد های موجود در پاسخ درگاه
        /// </summary>
        /// <returns></returns>
        public ResultStatus<int> CheckRequestField()
        {
            if (!Request.HasFormContentType)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "بروز خطا"
                };
            }

            if (string.IsNullOrEmpty(Request.Form["respcode"]))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خرید شما توسط بانک تایید شده است اما رسید دیجیتالی شما تایید نگشت مشکلی در فرایند رزرو خرید شما پیش آمده"
                };
            }

            if (!string.IsNullOrEmpty(Request.Form["TracenNmber"]))
            {
                if (Request.Form["TracenNmber"].ToString().Equals(string.Empty) &&
                    Request.Form["Respcode"].ToString().Equals(string.Empty))
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message =
                            "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت"
                    };
                }
            }

            if (Request.Form["InvoiceId"].ToString().Equals(string.Empty) &&
                Request.Form["Respcode"].ToString().Equals(string.Empty))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خطا در برقرار ارتباط با بانک"
                };
            }

            return new ResultStatus<int> { IsSuccess = true };
        }
        /// <summary>
        /// متد تایید تراکنش  توسط درگاه 
        /// با ارسال رسید دیجیتال و شناسه ترمینال 
        /// </summary>
        /// <param name="digitalReceipt">رسید دیجیتال</param>
        /// <param name="terminalId">شناسه ترمینال</param>
        /// <param name="hostUrl"></param>
        /// <returns></returns>
        public async Task<bool> VerifyMerchantTransAsync(string digitalReceipt, long terminalId, string hostUrl)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var verifyMerchantTransModel = new SaderatVerifyMerchantTransRequestModel
            {
                digitalreceipt = digitalReceipt,
                Tid = terminalId,
            };

            var client = new HttpClient(httpHandler);

            if (string.IsNullOrEmpty(hostUrl))
            {
                client.DefaultRequestHeaders.Host = hostUrl;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "https://sepehr.shaparak.ir:8081/V1/PeymentApi/Advice");

            var data = JsonSerializer.Serialize(verifyMerchantTransModel);

            request.Content = new StringContent(data, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var verifyMerchantTransResponseModel = JsonSerializer.Deserialize<SaderatVerifyMerchantTransResponseModel>(await response.Content.ReadAsStringAsync());

                if (verifyMerchantTransResponseModel?.Status == "Ok")
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", "درخواست موفق", null);
                    return true;
                }
                else if (verifyMerchantTransResponseModel?.Status == "Duplicate")
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", " درخواست تکراری است ", null);
                    return false;
                }
                else if (verifyMerchantTransResponseModel?.Status == "NOK")
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException, "Payment", "خطا درخواست ناموفق", null);
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return false;
        }
    }
}
