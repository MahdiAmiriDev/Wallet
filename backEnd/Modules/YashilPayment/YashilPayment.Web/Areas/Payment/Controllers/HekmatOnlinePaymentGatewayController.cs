using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Configurations;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Infrastructure.ServiceImpl;
using YashilPayment.Core.Const;
using YashilPayment.Core.DTO;
using YashilPayment.Core.Services;



namespace YashilPayment.Web.Areas.Payment.Controllers
{
    [Authorize]
    [Route("HekmatOnlinePaymentGateway")]
    public class HekmatOnlinePaymentGatewayController : YashilMvcBaseController<object>
    {
        private readonly IOrderService _orderService;
        private readonly IHekmatOnlinePaymentConfiguration _hekmatOnlinePaymentConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHekmatOnlinePaymentGatewayService _hekmatOnlinePaymentGatewayService;
        private readonly YashilAppDbContext _walletContext;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        public HekmatOnlinePaymentGatewayController(ApplicationConfiguration applicationConfiguration, IMapper mapper,
            IYashilLogger<object> logger,
            IPermissionService permissionService, IOrderService orderService,
            IHekmatOnlinePaymentConfiguration hekmatOnlinePaymentConfiguration, IHttpContextAccessor httpContextAccessor,
            IHekmatOnlinePaymentGatewayService hekmatOnlinePaymentGatewayService, IPaymentService paymentService,
            YashilAppDbContext walletContext, IUserPrincipal principal) :
            base(applicationConfiguration, mapper, logger, permissionService)
        {
            _orderService = orderService;
            _hekmatOnlinePaymentConfiguration = hekmatOnlinePaymentConfiguration;
            _httpContextAccessor = httpContextAccessor;
            _hekmatOnlinePaymentGatewayService = hekmatOnlinePaymentGatewayService;
            _walletContext = walletContext;
            _principal = principal;
            _application = applicationConfiguration;
        }
        /// <summary>
        /// هدایت کاربر یه صفحه درگاه به همراه توکن
        /// </summary>
        /// <param name="generatedTokenToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task RedirectToPaymentPageAsync(string generatedTokenToken, string signature)
        {
            if (string.IsNullOrWhiteSpace(generatedTokenToken))
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"توکن برای ارسال به درگاه مقدار ندارد" +
                    $"In RedirectToPaymentPage Method generatedTokenToken In Null Or Empty ",
                    null);
                throw new Exception("In RedirectToPaymentPage Method generatedTokenToken In Null Or Empty");
            }
            var remotePost = new RemotePost(_httpContextAccessor)
            {
                Url = _hekmatOnlinePaymentConfiguration.RedirectToIpgUrl
            };
            // اضافه کردن اطلاعات توکن و پست کردن آن به صفحه درگاه
            remotePost.Add("token", generatedTokenToken);
            remotePost.Add("signature", signature);
            remotePost.Add("language", HekmatOnlinePaymentGatewayLanguageConst.Persian);
            await remotePost.PostAsync();

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GoPayment")]
        public IActionResult GoPayment(string token)
        {
            var paymentUrl = _hekmatOnlinePaymentGatewayService.CreatePaymentUrl(token);
            return Redirect(paymentUrl);
        }

        public async Task<IActionResult> Index(int? orderId, int? paymentId = null)
        {
            if (!orderId.HasValue)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    "Payment",
                    $"شناسه سفارش برای درج پرداخت و ارسال به درگاه مقدار ندارد",
                    null);
                return RedirectToAction("Index", "Error");
            }
            var order = await _orderService.GetAsync(orderId.Value, true);
            if (order == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var requestPayment = paymentId.HasValue
                ? await _hekmatOnlinePaymentGatewayService.GenerateMixedTokenAsync(orderId.Value, paymentId.Value)
                : await _hekmatOnlinePaymentGatewayService.GenerateTokenAsync(orderId.Value);
            if (requestPayment.isSucces)
            {

                await RedirectToPaymentPageAsync(requestPayment.token, requestPayment.signature);
                //return GoPayment(requestPayment.token);
            }
            return RedirectToAction("Index", "Error");
        }

        [HttpPost("VerifyPaymentTransaction")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaymentTransactionAsync()
        {

            var callbackParameter = GetCallbackParameters();
            if (callbackParameter == null)
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.PaymentException,
                    nameof(VerifyPaymentTransactionAsync),
                    HekmatOnlinePaymentGatewayVerificationMessageConst.CallBackParamIsNull, null);
                return RedirectToAction("Index", "Error");
            }

            var result = await _hekmatOnlinePaymentGatewayService
                .VerifyPaymentTransactionAsync(callbackParameter);

            return View("VerifyPaymentTransaction", result.VerifyPaymentViewModel);
        }


        [HttpGet("VerifyPaymentTransaction")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaymentTransactionAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var res = await _hekmatOnlinePaymentGatewayService.VerifyPaymentTransactionByTokenAsync(token);
                return View("VerifyPaymentTransaction", res.VerifyPaymentViewModel);

            }
            _logger.InsertLog(LogLevel.Error,
                (int)Events.PaymentException,
                nameof(VerifyPaymentTransactionAsync),
                " توکن برای بررسی تراکنش  مقدار ندارد.", null);
            return RedirectToAction("Index", "Error");
        }

        /// <summary>پرداخت قطعی HekmatOnline را برای Capture کیف پول بدون تماس Confirm دوباره بازیابی می‌کند.</summary>
        [HttpPost("RecoverMixed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverMixed(int paymentId)
        {
            var payment = await _walletContext.Payment.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == paymentId && x.ApplicationId == _application.ApplicationId
                && x.PaymentMethodType == (int)PaymentMethodTypesEnum.HekmatOnline
                && x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid && !x.Deleted);
            if (payment == null || string.IsNullOrWhiteSpace(payment.Token)
                || !await _walletContext.Order.AsNoTracking().AnyAsync(x =>
                    x.Id == payment.OrderId && x.UserId == _principal.Id
                    && x.ApplicationId == _application.ApplicationId && !x.Deleted)
                || !await _walletContext.WalletReservation.AsNoTracking().AnyAsync(x =>
                    x.PaymentId == paymentId && x.UserId == _principal.Id
                    && x.ApplicationId == _application.ApplicationId && !x.Deleted
                    && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved))
                return RedirectToAction("Index", "Error");
            var result = await _hekmatOnlinePaymentGatewayService.VerifyPaymentTransactionByTokenAsync(payment.Token);
            return View("VerifyPaymentTransaction", result.VerifyPaymentViewModel);
        }
        /// <summary>
        /// مپ کردن
        /// پارامتر های ارسالی از صفحه پرداخت به
        /// Callback Method
        /// به Dto
        /// </summary>
        /// <returns></returns>
        private HekmatOnlinePaymentGatewayCallbackDto? GetCallbackParameters()
        {
            var redirectData = Request.Form["redirectData"].ToString();

            var jsonBytes = Convert.FromBase64String(redirectData);

            var callbackData = Encoding.UTF8.GetString(jsonBytes);

            var redirectDataModel =
                JsonSerializer.Deserialize<HekmatOnlinePaymentGatewayCallbackDto>(callbackData);

            return redirectDataModel;

        }


    }
}
