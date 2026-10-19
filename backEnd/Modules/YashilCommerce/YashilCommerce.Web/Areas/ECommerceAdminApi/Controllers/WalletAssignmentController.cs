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
    public class WalletAssignmentController : ControllerBase
    {
        private readonly IWalletAssignmentService _service;

        /// <summary>سرویس Assignment را با دسترسی مدیریتی WalletProgram دریافت می‌کند.</summary>
        public WalletAssignmentController(IWalletAssignmentService service) { _service = service; }

        /// <summary>Assignmentهای Program را با مجوز مشاهده همان Program برمی‌گرداند.</summary>
        [HttpGet]
        [YashilAuthorization("GetEntities", nameof(WalletProgram))]
        public Task<LoadResult> GetEntities(int walletProgramId, CustomDataSourceLoadOptions loadOptions)
            => DataSourceLoader.LoadAsync(_service.GetAll(walletProgramId), loadOptions);

        /// <summary>کاربران مجاز را برای انتخاب Target با مجوز مشاهده Program برمی‌گرداند.</summary>
        [HttpGet("GetUsers")]
        [YashilAuthorization("GetEntities", nameof(WalletProgram))]
        public Task<LoadResult> GetUsers(CustomDataSourceLoadOptions loadOptions)
            => DataSourceLoader.LoadAsync(_service.GetUsers(), loadOptions);

        /// <summary>جزئیات Assignment را با مجوز مشاهده Program می‌خواند.</summary>
        [HttpGet("{id:int}")]
        [YashilAuthorization("GetEntity", nameof(WalletProgram))]
        public async Task<ActionResult<WalletAssignmentDto>> GetEntity(int id)
        {
            var assignment = await _service.GetAsync(id);
            return assignment == null ? NotFound() : Ok(assignment);
        }

        /// <summary>عضویت دستیِ صفرمانده را با مجوز ایجاد و Ruleهای Service ثبت می‌کند.</summary>
        [HttpPost]
        [YashilAuthorization("PostEntity", nameof(WalletProgram))]
        public Task<WalletAssignmentResultDto> PostEntity([FromBody] WalletAssignmentCreateDto request)
            => _service.CreateAsync(request);
    }
}
