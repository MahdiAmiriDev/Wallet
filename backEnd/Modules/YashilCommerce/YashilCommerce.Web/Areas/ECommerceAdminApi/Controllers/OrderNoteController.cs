	 
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using  YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
	public class OrderNoteController : BaseController<OrderNote ,int,OrderNoteListViewModel, OrderNoteEditModel,OrderNoteSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IOrderNoteService _orderNoteService;
		private readonly IYashilLogger<OrderNote> _logger;
        private readonly IPermissionService _permissionService;
        public OrderNoteController(IOrderNoteService orderNoteService, IMapper mapper, IYashilLogger<OrderNote> logger, IPermissionService permissionService) : base(orderNoteService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _orderNoteService=orderNoteService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? orderId)
        {
            var orderNotes = _orderNoteService.GetByCustomFilter(orderId);
            return await DataSourceLoader.LoadAsync(orderNotes.ProjectTo<OrderNoteListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? orderId)
        {
            var orderNotes = _orderNoteService.GetByCustomFilter(orderId);
            return await DataSourceLoader.LoadAsync(orderNotes.ProjectTo<OrderNoteSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      