using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using  YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class SubsidyGroupController : BaseController<SubsidyGroup ,int,SubsidyGroupListViewModel, SubsidyGroupEditModel,SubsidyGroupSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISubsidyGroupService _subsidyGroupService;
		private readonly IYashilLogger<SubsidyGroup> _logger;
        private readonly IPermissionService _permissionService;
        public SubsidyGroupController(ISubsidyGroupService subsidyGroupService, 
        IMapper mapper, 
        IYashilLogger<SubsidyGroup> logger,
        IPermissionService permissionService) : base(subsidyGroupService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _subsidyGroupService=subsidyGroupService;
			_logger = logger;
			_permissionService = permissionService;
        }
        
    }
}      