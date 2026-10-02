using Yashil.Common.Core.Classes;
using AutoMapper;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
	public class VocherTypeController : YashilMvcBaseController<VocherType>
    {
        private readonly IMapper _mapper;
        private readonly IVocherTypeService _vocherTypeService;
		private readonly IYashilLogger<VocherType> _logger;
        private readonly IPermissionService _permissionService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        public VocherTypeController(
             ApplicationConfiguration applicationConfiguration,
            IVocherTypeService vocherTypeService, IMapper mapper, IYashilLogger<VocherType> logger,
            IPermissionService permissionService) : base(applicationConfiguration, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _vocherTypeService=vocherTypeService;
			_logger = logger;
			_permissionService = permissionService;
            _applicationConfiguration = applicationConfiguration;
        }
        
    }
}      