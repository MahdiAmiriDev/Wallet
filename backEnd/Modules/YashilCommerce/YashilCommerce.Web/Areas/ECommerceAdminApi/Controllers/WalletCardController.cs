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
    public class WalletCardController : ControllerBase
    {
        private readonly IWalletBenefitsService _service;
        public WalletCardController(IWalletBenefitsService service) => _service = service;

        [HttpGet("Groups")]
        [YashilAuthorization("GetEntities", nameof(WalletProgram))]
        public Task<LoadResult> Groups(int walletProgramId, CustomDataSourceLoadOptions options)
            => DataSourceLoader.LoadAsync(_service.GetCardGroups(walletProgramId), options);

        [HttpGet("Members")]
        [YashilAuthorization("GetEntities", nameof(WalletProgram))]
        public Task<LoadResult> Members(int groupId, CustomDataSourceLoadOptions options)
            => DataSourceLoader.LoadAsync(_service.GetCardMembers(groupId), options);

        /// <summary>گروه کارت Draft را با مجوز مدیریت Program می‌سازد.</summary>
        [HttpPost("CreateGroup")]
        [YashilAuthorization("PostEntity", nameof(WalletProgram))]
        public Task<WalletBenefitResultDto> CreateGroup([FromBody] WalletCardGroupCreateDto request)
            => _service.CreateCardGroupAsync(request);

        /// <summary>اثر انگشت کارت را بدون نگهداری شماره خام ثبت می‌کند.</summary>
        [HttpPost("AddCard")]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public Task<WalletBenefitResultDto> AddCard([FromBody] WalletCardMemberCreateDto request)
            => _service.AddCardAsync(request);

        /// <summary>گروه دارای کارت را برای Match فعال می‌کند.</summary>
        [HttpPost("ActivateGroup")]
        [YashilAuthorization("UpdateEntity", nameof(WalletProgram))]
        public Task<WalletBenefitResultDto> ActivateGroup([FromBody] int groupId)
            => _service.ActivateCardGroupAsync(groupId);
    }
}
