using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Enums;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Infrastructure.Data;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilPayment.Core.Services;
using YashilPayment.Core.Enums;

namespace YashilPayment.Web.Areas.Payment.Controllers
{
    [Authorize]
    public class VibePaymentController : YashilMvcBaseController<object>
    {
        private readonly IOrderService _orderService;
        private readonly IVibeService _vibeService;
        private readonly YashilAppDbContext _walletContext;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;
        public VibePaymentController(ApplicationConfiguration applicationConfiguration, IMapper mapper,
            IYashilLogger<object> logger, IPermissionService permissionService, IOrderService orderService,
            IVibeService vibeService, YashilAppDbContext walletContext, IUserPrincipal principal)
            : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _orderService = orderService;
            _vibeService = vibeService;
            _walletContext = walletContext;
            _principal = principal;
            _application = applicationConfiguration;
        }

        /// <summary>
        /// زمانی که کاربر درگاه پرداخت را از نوع 
        /// vibe 
        /// انتخاب می کند این متد فراخوانی می شود و کاربر را به صفحه پرداخت هدایت می کند.
        /// </summary>
        /// <param name="oId">شناسه سفارش</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? oId, int? paymentId = null)
        {
            if (!oId.HasValue)
            {
                _logger.InsertLog(LogLevel.Error,
                        (int)Events.InvalidModel, nameof(Index), $"order id is null", null);

                return RedirectToAction("Index", "Error");
            }

            var order = await _orderService.GetOrderIncludeOrderItemAndProduct(oId.Value, true).FirstOrDefaultAsync();

            if (order == null)
            {
                _logger.InsertLog(LogLevel.Error,
                        (int)Events.InvalidModel, nameof(Index), $"سفارش مرتبط با شناسه ارسالی یافت نشد. شناسه سفارش {oId.Value}", null);

                return RedirectToAction("Index", "Error");
            }

            PaymentInformation paymentInformation;
            if (paymentId.HasValue)
            {
                // تلاش Mixed تنها از Payment متصل به Reservation خود کاربر استفاده می‌کند.
                if (order.UserId != _principal.Id || order.ApplicationId != _application.ApplicationId)
                    return RedirectToAction("Index", "Error");
                var payment = await _walletContext.Payment.AsNoTracking().FirstOrDefaultAsync(x =>
                    x.Id == paymentId.Value && x.OrderId == order.Id
                    && x.ApplicationId == _application.ApplicationId && !x.Deleted
                    && x.PaymentMethodType == (int)PaymentMethodTypesEnum.Vibe
                    && x.PaymentStatusType == (int)PaymentStatusTypesEnum.SendToPaymentGateway
                    && x.Total > 0m && (x.Token == null || x.Token == ""));
                var reservation = payment == null ? null : await _walletContext.WalletReservation
                    .AsNoTracking().FirstOrDefaultAsync(x => x.PaymentId == payment.Id
                        && x.ApplicationId == _application.ApplicationId && x.UserId == _principal.Id
                        && !x.Deleted && x.WalletReservationStatusType == (int)WalletReservationStatusType.Reserved
                        && x.ExpiresAt > DateTime.UtcNow);
                if (reservation == null)
                    return RedirectToAction("Index", "Error");
                paymentInformation = new PaymentInformation
                {
                    PaymentId = payment.Id, Payment = payment, Total = payment.Total,
                    ReservationNumber = payment.ReservationNumber,
                    InstallmentCount = payment.InstallmentCount
                };
            }
            else
            {
                paymentInformation = await _orderService.CreateNewPaymentAndReturnAsync(
                    oId.Value, PaymentMethodTypesEnum.Vibe, null, null);
            }

            if (paymentInformation is { Total: { } } && paymentInformation.Total.Value != 0)
            {

                var createVibeOrderResponse = await _vibeService.CreateOrderAsync(paymentInformation,order);

                if (string.IsNullOrEmpty(createVibeOrderResponse.ErrorMessage) && !string.IsNullOrEmpty(createVibeOrderResponse.PaymentUrl))
                {
                    return Redirect(createVibeOrderResponse.PaymentUrl);
                }
            }
            return RedirectToAction("Index", "Error");
        }


        /// <summary>
        /// این متد زمانی فراخوانی می شود که کاربر در گاه وایب
        /// اقدام به پرداخت یا انصراف کرده باشد و درگاه برای اعلام نتیجه تراکنش این متد را فراخوانی می کند
        /// </summary>
        /// <param name="ref_id">شماره پیگیری</param>
        /// <param name="result">نتیجه تراکنش</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPaymentTransactionAsync(string ref_id, string result)
        {
            if (string.IsNullOrEmpty(ref_id) || string.IsNullOrEmpty(result))
            {
                _logger.InsertLog(LogLevel.Error,
                    (int)Events.InvalidModel, nameof(VerifyPaymentTransactionAsync), $"ref_id value or result value is null or empty", null);

                return RedirectToAction("Index", "Error");
            }


            var verifyResult = await _vibeService.VerifyVibeOrderAsync(ref_id, result);

            if (!verifyResult.IsSuccess)
                return RedirectToAction("Index", "Error");

            return View("VerifyPaymentTransaction", verifyResult.verifyPaymentViewModel);
        }

        /// <summary>Capture پرداخت قطعی Mixed را بدون فراخوانی دوباره درگاه بازیابی می‌کند.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverMixed(int paymentId)
        {
            var payment = await _walletContext.Payment.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == paymentId && x.ApplicationId == _application.ApplicationId
                && x.PaymentMethodType == (int)PaymentMethodTypesEnum.Vibe
                && x.PaymentStatusType == (int)PaymentStatusTypesEnum.Paid && !x.Deleted);
            if (payment == null || string.IsNullOrWhiteSpace(payment.Token)
                || !await _walletContext.Order.AsNoTracking().AnyAsync(x =>
                    x.Id == payment.OrderId && x.UserId == _principal.Id
                    && x.ApplicationId == _application.ApplicationId && !x.Deleted)
                || !await _walletContext.WalletReservation.AsNoTracking().AnyAsync(x =>
                    x.PaymentId == paymentId && x.UserId == _principal.Id
                    && x.ApplicationId == _application.ApplicationId && !x.Deleted))
                return RedirectToAction("Index", "Error");
            var result = await _vibeService.VerifyVibeOrderAsync(payment.Token,
                VerifyVibePaymentStatusEnum.Ok.ToString());
            return result.IsSuccess
                ? View("VerifyPaymentTransaction", result.verifyPaymentViewModel)
                : RedirectToAction("Index", "Error");
        }
    }
}
