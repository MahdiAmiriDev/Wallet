using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    [ApiController]
    [Area("ECommerceAdminApi")]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletCouponBatchController : ControllerBase
    {
        private readonly IWalletBenefitsService _service;
        public WalletCouponBatchController(IWalletBenefitsService service) => _service = service;

        [HttpGet]
        [YashilAuthorization("GetEntities", nameof(WalletProgram))]
        public Task<LoadResult> GetEntities(int walletProgramId, CustomDataSourceLoadOptions options)
            => DataSourceLoader.LoadAsync(_service.GetCouponBatches(walletProgramId), options);

        /// <summary>کدهای خام Batch را فقط در پاسخ نخست به مدیر مجاز تحویل می‌دهد.</summary>
        [HttpPost]
        [YashilAuthorization("PostEntity", nameof(WalletProgram))]
        public async Task<WalletCouponBatchResultDto> Create([FromBody] WalletCouponBatchCreateDto request)
        {
            Response.Headers["Cache-Control"] = "no-store";
            return await _service.CreateCouponBatchAsync(request);
        }

        /// <summary>تأیید دریافت کدها را با فعال‌سازی Batch ثبت می‌کند.</summary>
        [HttpPost("Activate")]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public Task<WalletBenefitResultDto> Activate([FromBody] int batchId)
            => _service.ActivateCouponBatchAsync(batchId);

        /// <summary>Batch با پاسخ گم‌شده را در Draft با کلید همان درخواست لغو می‌کند.</summary>
        [HttpPost("Cancel")]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public Task<WalletBenefitResultDto> Cancel([FromBody] string attemptKey)
            => _service.CancelCouponBatchAsync(attemptKey);
    }
}
