using AutoMapper;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
	public class HekmatTerminalController : BaseController<HekmatTerminal ,int,HekmatTerminalListViewModel, HekmatTerminalEditModel,HekmatTerminalSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IHekmatTerminalService _hekmatTerminalService;
		private readonly IYashilLogger<HekmatTerminal> _logger;
        private readonly IPermissionService _permissionService;
        public HekmatTerminalController(IHekmatTerminalService hekmatTerminalService, 
        IMapper mapper, 
        IYashilLogger<HekmatTerminal> logger,
        IPermissionService permissionService) : base(hekmatTerminalService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _hekmatTerminalService=hekmatTerminalService;
			_logger = logger;
			_permissionService = permissionService;
        }
        
    }
}      