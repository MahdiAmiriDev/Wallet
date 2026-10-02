using AutoMapper;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Common.Core.Classes;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class VocherController : YashilMvcBaseController<Vocher>
    {
        private readonly IMapper _mapper;
        private readonly IVocherService _vocherService;
        private readonly IYashilLogger<Vocher> _logger;
        private readonly IPermissionService _permissionService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        public VocherController(
             ApplicationConfiguration applicationConfiguration,
            IVocherService vocherService, IMapper mapper, IYashilLogger<Vocher> logger,
            IPermissionService permissionService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _vocherService = vocherService;
            _logger = logger;
            _permissionService = permissionService;
            _applicationConfiguration = applicationConfiguration;
        }

    }
}