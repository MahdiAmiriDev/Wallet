using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;

namespace Yashil.Core
{
    public partial class YashilMvcBaseController<T> : Controller
    {
        protected readonly ApplicationConfiguration ApplicationConfiguration;
        protected readonly IMapper Mapper;
        protected readonly IYashilLogger<T> _logger;
        protected readonly IPermissionService _permissionService;

        public YashilMvcBaseController(ApplicationConfiguration applicationConfiguration, IMapper mapper, IYashilLogger<T> logger, IPermissionService permissionService)
        {
            ApplicationConfiguration = applicationConfiguration;
            Mapper = mapper;
            _logger = logger;
            _permissionService = permissionService;
        }
    }
}
