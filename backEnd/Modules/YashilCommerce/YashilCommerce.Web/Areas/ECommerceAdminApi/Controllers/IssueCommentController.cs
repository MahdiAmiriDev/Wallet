	 
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
	public class IssueCommentController : BaseController<IssueComment ,int,IssueCommentListViewModel, IssueCommentEditModel,IssueCommentSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IIssueCommentService _issueCommentService;
		private readonly IYashilLogger<IssueComment> _logger;
        private readonly IPermissionService _permissionService;
        public IssueCommentController(IIssueCommentService issueCommentService, IMapper mapper, IYashilLogger<IssueComment> logger, IPermissionService permissionService) : base(issueCommentService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _issueCommentService=issueCommentService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? issueId)
        {
            var issueComments = _issueCommentService.GetByCustomFilter(issueId);
            return await DataSourceLoader.LoadAsync(issueComments.ProjectTo<IssueCommentListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? issueId)
        {
            var issueComments = _issueCommentService.GetByCustomFilter(issueId);
            return await DataSourceLoader.LoadAsync(issueComments.ProjectTo<IssueCommentSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      