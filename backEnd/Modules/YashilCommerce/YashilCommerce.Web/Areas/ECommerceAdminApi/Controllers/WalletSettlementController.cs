using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    [ApiController]
    [Area("ECommerceAdminApi")]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletSettlementController : ControllerBase
    {
        private readonly IWalletSettlementService _service;

        /// <summary>Service مالی Settlement را برای API مدیریتی دارای مجوز دریافت می‌کند.</summary>
        public WalletSettlementController(IWalletSettlementService service) { _service = service; }

        /// <summary>Capture را فقط از مسیر مدیریتی و با Payment موفق به Service می‌سپارد.</summary>
        [HttpPost("Capture")]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public async Task<ActionResult<WalletSettlementResultDto>> Capture([FromBody] WalletCaptureRequestDto request)
            => ToActionResult(await _service.CaptureAsync(request));

        /// <summary>Release را فقط از مسیر مدیریتی و با کنترل وضعیت اتمیک انجام می‌دهد.</summary>
        [HttpPost("Release")]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public async Task<ActionResult<WalletSettlementResultDto>> Release([FromBody] WalletReleaseRequestDto request)
            => ToActionResult(await _service.ReleaseAsync(request));

        /// <summary>نتیجه Business و تعارض هم‌زمان را به وضعیت مناسب HTTP تبدیل می‌کند.</summary>
        private ActionResult<WalletSettlementResultDto> ToActionResult(WalletSettlementResultDto result)
            => result.ErrorCode switch
            {
                WalletSettlementErrorCode.None => Ok(result),
                WalletSettlementErrorCode.NotFound => NotFound(result),
                WalletSettlementErrorCode.Conflict or WalletSettlementErrorCode.ConcurrentConflict => Conflict(result),
                _ => BadRequest(result)
            };
    }
}
