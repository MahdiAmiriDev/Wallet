using AutoMapper;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class SpecificationController : BaseController<Specification ,int,SpecificationListViewModel, SpecificationEditModel,SpecificationSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISpecificationService _specificationService;
        private readonly IYashilLogger<Specification> _logger;  private readonly IPermissionService _permissionService;

        public SpecificationController(ISpecificationService specificationService, IMapper mapper, IYashilLogger<Specification> logger,IPermissionService permissionService): base(specificationService, mapper,logger,permissionService)
        {
            _mapper=mapper;
            _logger = logger; _permissionService = permissionService;
            _specificationService=specificationService;
        }
        
    }
}      