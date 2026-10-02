	 
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
	public class IssueTypeItemController : BaseController<IssueTypeItem ,int,IssueTypeItemListViewModel, IssueTypeItemEditModel,IssueTypeItemSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IIssueTypeItemService _issueTypeItemService;
		private readonly IYashilLogger<IssueTypeItem> _logger;
        private readonly IPermissionService _permissionService;
        public IssueTypeItemController(IIssueTypeItemService issueTypeItemService, IMapper mapper, IYashilLogger<IssueTypeItem> logger, IPermissionService permissionService) : base(issueTypeItemService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _issueTypeItemService=issueTypeItemService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? issueId, int? issueType)
        {
            var issueTypeItems = _issueTypeItemService.GetByCustomFilter(issueId,issueType);
            return await DataSourceLoader.LoadAsync(issueTypeItems.ProjectTo<IssueTypeItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? issueId, int? issueType)
        {
            var issueTypeItems = _issueTypeItemService.GetByCustomFilter(issueId,issueType);
            return await DataSourceLoader.LoadAsync(issueTypeItems.ProjectTo<IssueTypeItemSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      