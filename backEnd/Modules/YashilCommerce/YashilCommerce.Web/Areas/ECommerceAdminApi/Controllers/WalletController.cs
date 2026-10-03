using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IWalletReservationService _walletReservationService;

        public WalletController(IWalletReservationService walletReservationService)
        {
            _walletReservationService = walletReservationService;
        }

        [HttpPost("Reserve")]
        public async Task<ActionResult<WalletReserveResultDto>> Reserve([FromBody] WalletReserveRequestDto request)
        {
            var result = await _walletReservationService.ReserveAsync(request);
            return result.ErrorCode switch
            {
                WalletReserveErrorCode.None => Ok(result),
                WalletReserveErrorCode.InvalidRequest => BadRequest(result),
                WalletReserveErrorCode.OrderNotFound => NotFound(result),
                WalletReserveErrorCode.UnauthorizedOrder => StatusCode(403, result),
                WalletReserveErrorCode.AssignmentMismatch => BadRequest(result),
                WalletReserveErrorCode.InsufficientBalance => BadRequest(result),
                WalletReserveErrorCode.Conflict => Conflict(result),
                _ => StatusCode(500, result)
            };
        }
    }
}
