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
    public class WalletRefundController : ControllerBase
    {
        private readonly IWalletRefundService _service;

        /// <summary>Service مالی Refund را با مجوز مدیریتی WalletProgram دریافت می‌کند.</summary>
        public WalletRefundController(IWalletRefundService service) { _service = service; }

        /// <summary>Refund جزئی یا کامل را بدون دریافت Assignment از Client به Service اتمیک می‌سپارد.</summary>
        [HttpPost]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public async Task<ActionResult<WalletRefundResultDto>> PostEntity([FromBody] WalletRefundRequestDto request)
        {
            var result = await _service.RefundAsync(request);
            return result.ErrorCode switch
            {
                WalletRefundErrorCode.None => Ok(result),
                WalletRefundErrorCode.NotFound => NotFound(result),
                WalletRefundErrorCode.Conflict or WalletRefundErrorCode.ConcurrentConflict => Conflict(result),
                _ => BadRequest(result)
            };
        }
    }
}
