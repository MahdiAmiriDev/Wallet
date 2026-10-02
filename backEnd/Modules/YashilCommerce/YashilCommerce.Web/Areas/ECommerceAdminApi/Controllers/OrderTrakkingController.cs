using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class OrderTrakkingController : BaseController<OrderTrakking ,int,OrderTrakkingListViewModel, OrderTrakkingEditModel,OrderTrakkingSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IOrderTrakkingService _orderTrakkingService;
		private readonly IYashilLogger<OrderTrakking> _logger;
        private readonly IPermissionService _permissionService;
        public OrderTrakkingController(IOrderTrakkingService orderTrakkingService, IMapper mapper, IYashilLogger<OrderTrakking> logger, IPermissionService permissionService) : base(orderTrakkingService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _orderTrakkingService=orderTrakkingService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? referenceTo, int? orderId, int? orderItemId)
        {
            var orderTrakkings = _orderTrakkingService.GetByCustomFilter(referenceTo,orderId,orderItemId);
            return await DataSourceLoader.LoadAsync(orderTrakkings.ProjectTo<OrderTrakkingListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? referenceTo, int? orderId, int? orderItemId)
        {
            var orderTrakkings = _orderTrakkingService.GetByCustomFilter(referenceTo,orderId,orderItemId);
            return await DataSourceLoader.LoadAsync(orderTrakkings.ProjectTo<OrderTrakkingSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      