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
    public class WalletAuditController : ControllerBase
    {
        private readonly IWalletAuditService _service;

        /// <summary>سرویس ممیزی خواندنی Wallet را برای API مدیریتی دریافت می‌کند.</summary>
        public WalletAuditController(IWalletAuditService service) { _service = service; }

        /// <summary>مغایرت‌های Program مجاز را گزارش می‌کند و هیچ مانده یا Ledger را تغییر نمی‌دهد.</summary>
        [HttpGet("{walletProgramId:int}")]
        [YashilAuthorization("GetEntity", nameof(WalletProgram))]
        public async Task<ActionResult<WalletAuditResultDto>> GetEntity(int walletProgramId)
        {
            if (walletProgramId <= 0) return BadRequest();
            var result = await _service.AuditProgramAsync(walletProgramId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
