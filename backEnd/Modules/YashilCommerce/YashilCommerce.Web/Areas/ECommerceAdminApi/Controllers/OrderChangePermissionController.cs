using AutoMapper;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class OrderChangePermissionController : BaseController<OrderChangePermission ,int,OrderChangePermissionListViewModel, OrderChangePermissionEditModel,OrderChangePermissionSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IOrderChangePermissionService _orderChangePermissionService;
		private readonly IYashilLogger<OrderChangePermission> _logger;
        private readonly IPermissionService _permissionService;
        public OrderChangePermissionController(IOrderChangePermissionService orderChangePermissionService, IMapper mapper, IYashilLogger<OrderChangePermission> logger, IPermissionService permissionService) : base(orderChangePermissionService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _orderChangePermissionService=orderChangePermissionService;
			_logger = logger;
			_permissionService = permissionService;
        }
        
    }
}      