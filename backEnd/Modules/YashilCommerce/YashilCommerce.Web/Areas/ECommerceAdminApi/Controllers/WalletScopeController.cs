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
    public class WalletScopeController : ControllerBase
    {
        private readonly IWalletScopeService _service;

        /// <summary>سرویس Scope را برای مدیریت زیرمجموعه WalletProgram دریافت می‌کند.</summary>
        public WalletScopeController(IWalletScopeService service) { _service = service; }

        /// <summary>Scopeهای Program را با مجوز مشاهده همان Program برمی‌گرداند.</summary>
        [HttpGet]
        [YashilAuthorization("GetEntities", nameof(WalletProgram))]
        public Task<LoadResult> GetEntities(int walletProgramId, CustomDataSourceLoadOptions loadOptions)
            => DataSourceLoader.LoadAsync(_service.GetAll(walletProgramId), loadOptions);

        /// <summary>Targetهای Scope را با همان مجوز مشاهده WalletProgram برای انتخاب در فرم می‌خواند.</summary>
        [HttpGet("GetTargets")]
        [YashilAuthorization("GetEntities", nameof(WalletProgram))]
        public Task<LoadResult> GetTargets(int scopeType, CustomDataSourceLoadOptions loadOptions)
            => DataSourceLoader.LoadAsync(_service.GetTargets(scopeType), loadOptions);

        /// <summary>جزئیات Scope را با مجوز مشاهده Program می‌خواند.</summary>
        [HttpGet("{id:long}")]
        [YashilAuthorization("GetEntity", nameof(WalletProgram))]
        public async Task<ActionResult<WalletScopeDto>> GetEntity(long id)
        {
            var scope = await _service.GetAsync(id);
            return scope == null ? NotFound() : Ok(scope);
        }

        /// <summary>Scope جدید را با مجوز ایجاد Program و Rule وضعیت Draft ثبت می‌کند.</summary>
        [HttpPost]
        [YashilAuthorization("PostEntity", nameof(WalletProgram))]
        public Task<WalletScopeResultDto> PostEntity([FromBody] WalletScopeEditDto request)
            => _service.CreateAsync(request);

        /// <summary>ویرایش Scope را با مجوز ویرایش Program و کنترل Service انجام می‌دهد.</summary>
        [HttpPost("UpdateEntity")]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public Task<WalletScopeResultDto> UpdateEntity([FromBody] WalletScopeEditDto request)
            => _service.UpdateAsync(request);

        /// <summary>حذف نرم Scope را با مجوز ویرایش Program و کنترل Draft انجام می‌دهد.</summary>
        [HttpPost("DeleteEntity")]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public Task<WalletScopeResultDto> DeleteEntity([FromBody] WalletScopeDeleteDto request)
            => _service.DeleteAsync(request);
    }
}
