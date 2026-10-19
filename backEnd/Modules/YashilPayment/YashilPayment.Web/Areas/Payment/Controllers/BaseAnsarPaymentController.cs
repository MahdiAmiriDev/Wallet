using System.Net;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.CustomViewModels.AnsarPaymentModels;
using YashilCommerce.Core.Enums;

namespace YashilPayment.Web.Areas.Payment.Controllers
{
    public class BaseAnsarPaymentController<T> : YashilMvcBaseController<T>
    {
        private readonly IAnsarPaymentConfiguration _ansarPaymentConfiguration;
        private readonly IHostEnvironment _environment;
        public BaseAnsarPaymentController(ApplicationConfiguration applicationConfiguration, IMapper mapper, IYashilLogger<T> logger, IPermissionService permissionService, IAnsarPaymentConfiguration ansarPaymentConfiguration, IHostEnvironment environment) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _ansarPaymentConfiguration = ansarPaymentConfiguration;
            _environment = environment;
        }

        /// <summary>
        /// check post pack is fill or not
        /// check the parameter is gotten from ipg page or not
        /// </summary>
        /// <returns></returns>
        public bool RequestUnpack(out string state)
        {
            state = string.Empty;
            if (!CheckRequestField().IsSuccess)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(Request.Form[AnsarResultParam.RefNum]))
            {
                // var refrenceNumber = Request.Form[AnsarResultParam.RefNum].ToString();
            }

            state = Request.Form[AnsarResultParam.State].ToString();
            return true;
        }

        /// <summary>
        /// check the field gotten from ipg page is empty or not
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

            if (string.IsNullOrEmpty(Request.Form["State"]))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message =
                        "خريد شما توسط بانک تاييد شده است اما رسيد ديجيتالي شما تاييد نگشت! مشکلي در فرايند رزرو خريد شما پيش آمده است",
                };
            }

            if (!string.IsNullOrEmpty(Request.Form["RefNum"]))
            {
                if (Request.Form["RefNum"].ToString().Equals(string.Empty) &&
                    Request.Form["State"].ToString().Equals(string.Empty))
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message =
                            "فرايند انتقال وجه با موفقيت انجام شده است اما فرايند تاييد رسيد ديجيتالي با خطا مواجه گشت"
                    };
                }
            }

            if (Request.Form["ResNum"].ToString().Equals(string.Empty) &&
                Request.Form["State"].ToString().Equals(string.Empty))
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "خطا در برقرار ارتباط با بانک"
                };
            }

            return new ResultStatus<int> { IsSuccess = true };
        }

        public async Task<bool> VerifyMerchantTransAsync(string token, string refNumber, string hostUrl)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var verifyMerchantTransModel = new VerifyMerchantTransRequestModel
            {
                Token = token,
                RefNum = refNumber,
                WSContext = new WSContext
                {
                    Password = _ansarPaymentConfiguration.Password,
                    UserId = _ansarPaymentConfiguration.UserName
                }
            };

            var client = new HttpClient(httpHandler);
            if (string.IsNullOrEmpty(hostUrl))
            {
                client.DefaultRequestHeaders.Host = hostUrl;
            }
            // client.DefaultRequestHeaders.Host = "fanavaran.tbao.ir";

            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcp.shaparak.ir/ref-payment/RestServices/mts/verifyMerchantTrans/");

            var data = JsonSerializer.Serialize(verifyMerchantTransModel);

            request.Content = new StringContent(data, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var verifyMerchantTransResponseModel = JsonSerializer.Deserialize<VerifyMerchantTransResponseModel>(await response.Content.ReadAsStringAsync());

                if (verifyMerchantTransResponseModel?.Result == "erSucceed")
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return false;
        }



        #region Ipg New Implimantation
        public virtual RedirectResult RedirectToIpg(string token)
        {
            return Redirect($"https://fcp.shaparak.ir/_ipgw_/payment/?token={token}&lang=fa");
        }

        public async Task<string> GenerateTokenAsync(string amount, string resNum, string mobileNumber)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var tokenRequestModel = new GenerateTokenRequestModel
            {
                Amount = amount,
                WSContext = new WSContext
                {
                    Password = _ansarPaymentConfiguration.Password,
                    UserId = _ansarPaymentConfiguration.UserName
                },
                TransType = "EN_GOODS",
                ReserveNum = resNum,
                RedirectUrl = _ansarPaymentConfiguration.CallBackUrl,
                UserId = mobileNumber,
                MobileNo = mobileNumber,
                Email = ""
            };


            //برای عبور از پراکسی این قطعه کد را فعال می کنیم در حالت توسعه
            //نیاز است نام کاربری و رمز عبور مربوط به پراکسی شخص وارد شود
            if (_environment.IsDevelopment())
            {
                var proxy = new WebProxy("http://192.168.55.2:8080");
                proxy.Credentials = new NetworkCredential("ir94", "artin");
                httpHandler.Proxy = proxy;
                httpHandler.UseProxy = true;
            }
            


        var client = new HttpClient(httpHandler);
            client.DefaultRequestHeaders.Host =
                ApplicationConfiguration.BaseUrl
                .Replace("https://", "")
                .Replace("/", "");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcp.shaparak.ir/ref-payment/RestServices/mts/generateTokenWithNoSign/");

            var data = JsonSerializer.Serialize(tokenRequestModel);
            request.Content = new StringContent(data,
                Encoding.UTF8, "application/json");

            try
            {

                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var stringResponse = await response.Content.ReadAsStringAsync();
                var generateTokenResponseModel = JsonSerializer.Deserialize<GenerateTokenResponseModel>(stringResponse);

                if (generateTokenResponseModel?.Result == "erSucceed")
                {
                    return generateTokenResponseModel.Token;
                }
                else
                {
                    _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                        "Payment", $"خطا در ساخت توکن برای سفارش با کد: {resNum} --" +
                                   $"{generateTokenResponseModel?.Result}", null);
                }
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.PaymentException,
                    "Payment", $"خطا در ساخت توکن برای سفارش با کد: {resNum}", e);
            }

            return string.Empty;
        }
        #endregion


    }
}
