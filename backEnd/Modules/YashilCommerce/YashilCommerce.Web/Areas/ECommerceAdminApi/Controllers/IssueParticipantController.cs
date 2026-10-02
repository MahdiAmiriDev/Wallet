	 
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
	public class IssueParticipantController : BaseController<IssueParticipant ,int,IssueParticipantListViewModel, IssueParticipantEditModel,IssueParticipantSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IIssueParticipantService _issueParticipantService;
		private readonly IYashilLogger<IssueParticipant> _logger;
        private readonly IPermissionService _permissionService;
        public IssueParticipantController(IIssueParticipantService issueParticipantService, IMapper mapper, IYashilLogger<IssueParticipant> logger, IPermissionService permissionService) : base(issueParticipantService, mapper,logger, permissionService)
        {
            _mapper=mapper;
            _issueParticipantService=issueParticipantService;
			_logger = logger;
			_permissionService = permissionService;
        }
              [HttpGet("GetByCustomFilterForList")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions,  int? issueId, int? userId)
        {
            var issueParticipants = _issueParticipantService.GetByCustomFilter(issueId,userId);
            return await DataSourceLoader.LoadAsync(issueParticipants.ProjectTo<IssueParticipantListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")] [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions,  int? issueId, int? userId)
        {
            var issueParticipants = _issueParticipantService.GetByCustomFilter(issueId,userId);
            return await DataSourceLoader.LoadAsync(issueParticipants.ProjectTo<IssueParticipantSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
  
    }
}      