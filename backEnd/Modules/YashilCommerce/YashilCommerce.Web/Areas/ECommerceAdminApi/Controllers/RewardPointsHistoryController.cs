using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using  YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class RewardPointsHistoryController : BaseController<RewardPointsHistory ,int,RewardPointsHistoryListViewModel, RewardPointsHistoryEditModel,RewardPointsHistorySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRewardPointsHistoryService _rewardPointsHistoryService;
		private readonly IYashilLogger<RewardPointsHistory> _logger;
        private readonly IPermissionService _permissionService;
        public RewardPointsHistoryController(IRewardPointsHistoryService rewardPointsHistoryService, IMapper mapper, IYashilLogger<RewardPointsHistory> logger, IPermissionService permissionService) : base(rewardPointsHistoryService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _rewardPointsHistoryService=rewardPointsHistoryService;
			_logger = logger;
			_permissionService = permissionService;
        }
        
    }
}      