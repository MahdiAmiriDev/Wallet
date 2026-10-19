using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    [ApiController]
    [Area("ECommerceAdminApi")]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletProgramController : ControllerBase
    {
        private readonly IWalletProgramService _service;

        /// <summary>سرویس Program را برای عملیات مدیریتی دارای مجوز دریافت می‌کند.</summary>
        public WalletProgramController(IWalletProgramService service) { _service = service; }

        /// <summary>فهرست Programهای Application جاری را برای Grid مدیریتی بارگذاری می‌کند.</summary>
        [HttpGet]
        [YashilAuthorization(ActionName = "GetEntities")]
        public Task<LoadResult> GetEntities(CustomDataSourceLoadOptions loadOptions)
            => DataSourceLoader.LoadAsync(_service.GetAll(), loadOptions);

        /// <summary>جزئیات یک Program مجاز را بر اساس شناسه می‌خواند.</summary>
        [HttpGet("{id:int}")]
        [YashilAuthorization(ActionName = "GetEntity")]
        public async Task<ActionResult<WalletProgramDto>> GetEntity(int id)
        {
            var program = await _service.GetAsync(id);
            if (program == null) return NotFound();
            return Ok(program);
        }

        /// <summary>Program را با مجوز PostEntity و قواعد ایجاد Service ثبت می‌کند.</summary>
        [HttpPost]
        [YashilAuthorization(ActionName = "PostEntity")]
        public Task<WalletProgramResultDto> PostEntity([FromBody] WalletProgramEditDto request)
            => _service.CreateAsync(request);

        /// <summary>ویرایش را با مجوز UpdateEntity به کنترل مالی و Lifecycle در Service می‌سپارد.</summary>
        [HttpPost("UpdateEntity")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public Task<WalletProgramResultDto> UpdateEntity([FromBody] WalletProgramEditDto request)
            => _service.UpdateAsync(request);

        /// <summary>تغییر وضعیت را با مجوز UpdateEntity و گذارهای مصوب Service انجام می‌دهد.</summary>
        [HttpPost("ChangeStatus")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public Task<WalletProgramResultDto> ChangeStatus([FromBody] WalletProgramStatusDto request)
            => _service.ChangeStatusAsync(request);
    }
}
