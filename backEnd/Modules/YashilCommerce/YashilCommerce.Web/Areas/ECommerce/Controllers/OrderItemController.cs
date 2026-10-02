using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;


namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(SessionFixationFilterAsync))]
    public class OrderItemController : YashilMvcBaseController<OrderItem>
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;

        // GET: /<controller>/

        public OrderItemController(ApplicationConfiguration applicationConfiguration, IMapper mapper, IYashilLogger<OrderItem> logger, IPermissionService permissionService, IOrderItemService orderItemService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _orderItemService = orderItemService;
        }
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
