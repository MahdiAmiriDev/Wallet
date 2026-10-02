using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomViewModels.UserManagement;
using Yashil.Core.Entities;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class IssueController : BaseController<Issue, int, IssueListViewModel, IssueEditModel, IssueSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IIssueService _issueService;
        private readonly IUserPrincipal _userPrincipal;
        public IssueController(IIssueService issueService, IMapper mapper, IYashilLogger<Issue> logger,
           IUserPrincipal userPrincipal, IPermissionService permissionService) : base(issueService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _issueService = issueService;
            _userPrincipal = userPrincipal;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? orderItemId, int? orderId, int? paymentId, int? issueType, int? resolutionType, int? priorityType,
                                                                int? issueStateType, int? assignee, int? issueOwner, string nonRegisteredUser = null)
        {
            var issues = _issueService.GetByCustomFilter(orderItemId, orderId, paymentId, issueType, resolutionType, priorityType, issueStateType, assignee, issueOwner, nonRegisteredUser);
            return await DataSourceLoader.LoadAsync(issues.ProjectTo<IssueListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? orderItemId, int? orderId, int? paymentId, int? issueType, int? resolutionType, int? priorityType, int? issueStateType, int? assignee)
        {
            var issues = _issueService.GetByCustomFilter(orderItemId, orderId, paymentId, issueType, resolutionType, priorityType, issueStateType, assignee, null, null);
            return await DataSourceLoader.LoadAsync(issues.ProjectTo<IssueSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetAssigneeByFiler")]
        [YashilAuthorization(ActionName = "IssueAssigneeByFiler")]
        public async Task<LoadResult> GetAssigneeByFiler(CustomDataSourceLoadOptions loadOptions, int? orderItemId, string recipientNationalCode = null, string mobileNumber = null)
        {
            var assignee = _issueService.GetAssigneeByFiler(orderItemId, recipientNationalCode, mobileNumber);
            return await DataSourceLoader.LoadAsync(assignee.ProjectTo<UserListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(IssueEditModel editModel)
        {
            if (editModel.Assignee != _userPrincipal.Id)
            {
                return "کاربر گرامی شما مجاز به تغییر این رکورد نمی باشید.";
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var issue = _issueService.Get(id, true);
            if (issue == null)
            {
                return "کاربر گرامی چنین رکوردی وجود ندارد.";
            }
            else
            {
                if (issue.Assignee != _userPrincipal.Id)
                {
                    return "کاربر گرامی شما مجاز به حذف این رکورد نمی باشید.";
                }
                return await base.CustomValidateBeforeDeleteAsync(id);
            }
        }


    }
}