using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Web.Infrastructure.Classes;
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
    public class WalletGrantController : ControllerBase
    {
        private readonly IWalletGrantService _service;

        /// <summary>Service مالی Grant را برای API مدیریتی دریافت می‌کند.</summary>
        public WalletGrantController(IWalletGrantService service) { _service = service; }

        /// <summary>Grantهای Program را با مجوز مشاهده WalletProgram برمی‌گرداند.</summary>
        [HttpGet]
        [YashilAuthorization("GetEntities", nameof(WalletProgram))]
        public Task<LoadResult> GetEntities(int walletProgramId, CustomDataSourceLoadOptions loadOptions)
            => DataSourceLoader.LoadAsync(_service.GetAll(walletProgramId), loadOptions);

        /// <summary>با مجوز ویرایش Program، Grant را به Ruleهای مالی Service می‌سپارد.</summary>
        [HttpPost]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public async Task<ActionResult<WalletGrantResultDto>> PostEntity([FromBody] WalletGrantCreateDto request)
        {
            var result = await _service.CreateAsync(request);
            return result.IsConflict ? Conflict(result) : Ok(result);
        }
    }
}
