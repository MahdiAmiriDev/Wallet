using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Core.Enums;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    [ApiController]
    [Area("ECommerceAdminApi")]
    [Route("ECommerceAdminApi/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletOrderPreparationService _preparation;

        public WalletController(IWalletOrderPreparationService preparation) => _preparation = preparation;

        /// <summary>تلاش Mixed را با Payment و Reservation یکسان آماده می‌کند.</summary>
        [HttpPost("Reserve")]
        public async Task<ActionResult<WalletOrderPreparationResultDto>> Reserve(
            [FromBody] WalletOrderPreparationRequestDto request)
        {
            if (request == null || (request.PaymentMethodType != (int)PaymentMethodTypesEnum.Vibe
                && request.PaymentMethodType != (int)PaymentMethodTypesEnum.HekmatOnline))
                return Conflict(WalletOrderPreparationResultDto.Failure(
                    WalletReserveErrorCode.Conflict, "Mixed برای این روش پرداخت فعال نیست."));
            var result = await _preparation.PrepareAsync(request);
            if (!result.IsSuccess)
                return result.ErrorCode == WalletReserveErrorCode.Conflict
                    ? Conflict(result) : BadRequest(result);
            if (result.CanRedirect)
                result.RedirectUrl = request.PaymentMethodType == (int)PaymentMethodTypesEnum.Vibe
                    ? Url.Action("Index", "VibePayment",
                        new { oId = result.OrderId, paymentId = result.PaymentId })
                    : Url.Action("Index", "HekmatOnlinePaymentGateway",
                        new { orderId = result.OrderId, paymentId = result.PaymentId });
            return Ok(result);
        }
    }
}
