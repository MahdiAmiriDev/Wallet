using AutoMapper;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class WarehouseController : BaseController<Warehouse ,int,WarehouseListViewModel, WarehouseEditModel,WarehouseSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IWarehouseService _warehouseService;
        private readonly IYashilLogger<Warehouse> _logger;  private readonly IPermissionService _permissionService;
        public WarehouseController(IWarehouseService warehouseService, IMapper mapper, IYashilLogger<Warehouse> logger,IPermissionService permissionService): base(warehouseService, mapper,logger,permissionService)
        {
            _mapper=mapper;
            _logger = logger; _permissionService = permissionService;
            _warehouseService=warehouseService;
        }
        
    }
}      