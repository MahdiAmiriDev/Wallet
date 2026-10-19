using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Extensions;
using YashilPayment.Core.Services;
using YashilPayment.Infrastructure.Helper;
using YashilPayment.Web.Areas.Payment.ViewModels;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.Payment.Controllers
{
    public class HekmatIpgController : YashilMvcBaseController<HekmatPaymentGatewayTransaction>
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IMapper _mapper;
        private readonly IYashilHekmatTerminalService _hekmatTerminalService;
        private IWebHostEnvironment _environment;
        private readonly IPaymentGatewayService _paymentGatewayService;

        public HekmatIpgController(ApplicationConfiguration applicationConfiguration, IMapper mapper,
            IYashilLogger<HekmatPaymentGatewayTransaction> logger,
            IPermissionService permissionService, IYashilHekmatTerminalService hekmatTerminalService,
            IWebHostEnvironment environment,
            IPaymentGatewayService paymentGatewayService)
            : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _applicationConfiguration = applicationConfiguration;
            _hekmatTerminalService = hekmatTerminalService;
            _environment = environment;
            _paymentGatewayService = paymentGatewayService;
        }
        /// <summary>
        /// پذیرنده توکن را به صفحه اصلی درگاه پرداخت ارسال و کاربر را به این صفحه هدایت می نماید
        /// </summary>
        /// <param name="encryptedToken">توکن ارتباطی بین پذیرنده و دارنده</param>
        /// <returns></returns>
        public async Task<IActionResult> Index([FromForm] string encryptedToken)
        {
            var resultModel = new ResultModel
            {
                Succeeded = false,
                Status = 0
            };
            if (string.IsNullOrEmpty(encryptedToken))
            {
                resultModel.Message = "مقدار توکن ارسال نشده است";
                resultModel.Status = (int)HekmatIpgPageErrorEnum.InvalidToken;
                return View("IpgError", resultModel);
            }
            // استخراج اطلاعات توکن و پذیرنده و بررسی معتبر بودن توکن
            var (succeeded, message, _, ipgToken, terminal) =
                await _paymentGatewayService.ValidateAndExtractEncryptedTokenData(encryptedToken);

            if (!succeeded || ipgToken == null || terminal == null)
            {
                resultModel.Message = "توکن معتبر نمی باشد";
                resultModel.Status = (int)HekmatIpgPageErrorEnum.InvalidToken;
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction),
                    message, null);
                return View("IpgError", resultModel);
            }
            // بررسی معتبر بودن شماره کارت حکمت
            if (!string.IsNullOrEmpty(ipgToken.CardNumber) && !ipgToken.CardNumber.IsValidValidateHekmatCard())
            {
                resultModel.Message = "شماره کارت ارسالی معتبر نمی باشد";
                resultModel.Status = (int)HekmatIpgPageErrorEnum.CardNumberInvalid;
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"card is not valid in HekmatIpgController index with cardNumber={ipgToken.CardNumber} and merchant={ipgToken.MerchantId} and terminalId={ipgToken.TerminalId}", null);

                return View("IpgError", resultModel);
            }
            var tokenIpAddress = ipgToken.IpAddress;
            var terminalId = ipgToken.TerminalId;
            var merchantId = ipgToken.MerchantId;
            var terminalInfoQuery = _hekmatTerminalService.GetYashilHekmatTerminalByIdAndIp(merchantId, terminalId, tokenIpAddress, _applicationConfiguration.ApplicationId);
            var terminalInfo = await terminalInfoQuery.ProjectTo<YashilHekmatTerminalWithMerchantViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (terminalInfo == null)
            {
                resultModel.Message = "اطلاعات ترمینال برای این پذیرنده یافت نشد.";
                resultModel.Status = (int)HekmatIpgPageErrorEnum.TerminalNotFound;
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert, nameof(HekmatPaymentGatewayTransaction), $"terminal info is not valid : Terminal not Found by terminalId and MerchantId in HekmatIpgController index with ip={ipgToken.IpAddress} and merchant={ipgToken.MerchantId} and terminalId={ipgToken.TerminalId}", null);

                return View("IpgError", resultModel);
            }
            bool tokenIsExpired = PaymentHelper.IsTokenExpired(ipgToken.TokenExpireDateTime);
            if (tokenIsExpired)
            {
                resultModel.Message = "زمان اعتبار توکن منقضی شده است";
                resultModel.Status = (int)HekmatIpgPageErrorEnum.TokenExpired;
                _logger.InsertLog(LogLevel.Error, (int)Events.InvalidParamsInsert,
                    nameof(HekmatPaymentGatewayTransaction), $"token is expired in HekmatIpgController index ", null);
                return View("IpgError", resultModel);
            }

            var amount = ipgToken.Amount;
            int installmentCount = ipgToken.InstallmentCount;

            var hekmatIpgViewModel = new HekmatIpgViewModel
            {
                CardNumber = ipgToken.CardNumber,
                PaymentRequestToken = encryptedToken,
                Price = amount,
                InstallmentCount = installmentCount,
                MerchantName = terminalInfo.MerchantName,
                MerchantUrl = terminalInfo.MerchantUrl,
                TerminalId = terminalInfo.Id,
                AdditionalDate = ipgToken.AdditionalData,
                CallBackUrl = ipgToken.ReturnUrl,
                ReservationNumber = ipgToken.ReservationNumber,
                MerchantId = ipgToken.MerchantId,
                //اگر پرداخت نقدی اقساطی از طرح باقرالعلوم باشد مقدار
                //useFormula == true است
                UseFormula = ipgToken.UseFormula
            };
            return View(hekmatIpgViewModel);
        }
        /// <summary>
        ///  بعد از اینکه کاربر دکمه پرداخت را کلیک کرد
        ///  و متد مربوط به ثبت تراکنش صدا زده شد و تراکنش کاربر با موفقت درج شد
        ///  متد زیر با  
        ///  نتیجه پرداخت ثبت شده کاربر صدا زده می شود
        ///  صدا زده می شود، تا کاربر را به صفحه تکمیل اطلاعات پرداخت هدایت کرده  
        ///  و کاربر با مشاهده اطلاعات پرداخت 
        ///  با زدن دکمه تکمیل اطلاعات پرداخت یا به صورت اتوماتیک بعد از 10 ثانیه به سایت پذیرنده هدایت شود 
        /// </summary>
        /// <returns></returns>
        public IActionResult CompletePurchase(PurchaseResult purchaseResult)
        {
            return View(purchaseResult);
        }
        /// <summary>
        /// دریافت فایل proto  مربوط به درگاه
        /// </summary>
        /// <returns></returns>
        public IActionResult GetHekmatIpgFile()
        {
            var filePath = Path.Combine(_environment.WebRootPath, "hekmatIPG.proto");
            if (System.IO.File.Exists(filePath))
            {
                return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), "hekmatIPG.proto");
            }
            // هدایت به صفحه خطا
            return RedirectToAction("Index", "Error");
        }

    }
}
