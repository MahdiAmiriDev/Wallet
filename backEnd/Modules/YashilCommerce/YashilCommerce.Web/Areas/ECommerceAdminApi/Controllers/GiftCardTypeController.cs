using AutoMapper;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class GiftCardTypeController : BaseController<GiftCardType ,int,GiftCardTypeListViewModel, GiftCardTypeEditModel,GiftCardTypeSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IGiftCardTypeService _giftCardTypeService;
        private readonly IYashilLogger<GiftCardType> _logger;  private readonly IPermissionService _permissionService;
        public GiftCardTypeController(IGiftCardTypeService giftCardTypeService, IMapper mapper, IYashilLogger<GiftCardType> logger,IPermissionService permissionService): base(giftCardTypeService, mapper,logger,permissionService)
        {
            _mapper=mapper;
            _logger = logger; _permissionService = permissionService;
            _giftCardTypeService=giftCardTypeService;
        }
        
    }
}      