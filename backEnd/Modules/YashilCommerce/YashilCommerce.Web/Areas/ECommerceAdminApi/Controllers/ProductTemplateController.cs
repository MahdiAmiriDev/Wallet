using AutoMapper;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class ProductTemplateController : BaseController<ProductTemplate ,int,ProductTemplateListViewModel, ProductTemplateEditModel,ProductTemplateSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IProductTemplateService _productTemplateService;
        private readonly IYashilLogger<ProductTemplate> _logger;  private readonly IPermissionService _permissionService;
        public ProductTemplateController(IProductTemplateService productTemplateService, IMapper mapper, IYashilLogger<ProductTemplate> logger,IPermissionService permissionService): base(productTemplateService, mapper,logger,permissionService)
        {
            _mapper=mapper;
            _logger = logger; _permissionService = permissionService;
            _productTemplateService=productTemplateService;
        }
        
    }
}      