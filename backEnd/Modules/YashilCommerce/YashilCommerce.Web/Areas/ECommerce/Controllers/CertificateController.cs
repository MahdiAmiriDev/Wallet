using Microsoft.AspNetCore.Mvc;
using Yashil.Core.Entities;
using Yashil.Core;
using Yashil.Common.Core.Classes;
using AutoMapper;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class CertificateController : YashilMvcBaseController<Certificate>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private new readonly IPermissionService _permissionService;
        public CertificateController(ApplicationConfiguration applicationConfiguration, IMapper mapper, IYashilLogger<Certificate> logger, IPermissionService permissionService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _permissionService = permissionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CertificateSearch()
        {
            return View();
        }
    }
}
