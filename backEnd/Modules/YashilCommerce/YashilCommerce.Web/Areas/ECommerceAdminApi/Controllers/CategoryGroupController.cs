using System.Threading.Tasks;
using AutoMapper;
using Yashil.Common.Core.AppEvents;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using  YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class CategoryGroupController : BaseController<CategoryGroup ,int,CategoryGroupListViewModel, CategoryGroupEditModel,CategoryGroupSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryGroupService _categoryGroupService;
		private readonly IYashilLogger<CategoryGroup> _logger;
        private readonly IPermissionService _permissionService;
        public CategoryGroupController(ICategoryGroupService categoryGroupService, 
        IMapper mapper, 
        IYashilLogger<CategoryGroup> logger,
        IPermissionService permissionService) : base(categoryGroupService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _categoryGroupService=categoryGroupService;
			_logger = logger;
			_permissionService = permissionService;
        }

    }
}      