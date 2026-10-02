using System;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class IssueLogWorkController : BaseController<IssueLogWork, int, IssueLogWorkListViewModel,
        IssueLogWorkEditModel, IssueLogWorkSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IIssueLogWorkService _issueLogWorkService;
        private readonly IYashilLogger<IssueLogWork> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IIssueService _issueService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IOrderService _orderService;

        public IssueLogWorkController(IIssueLogWorkService issueLogWorkService, IMapper mapper,
            IYashilLogger<IssueLogWork> logger,
            IPermissionService permissionService, IUserPrincipal userPrincipal, IIssueService issueService,
            ApplicationConfiguration applicationConfiguration, IOrderService orderService) : base(
            issueLogWorkService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _issueLogWorkService = issueLogWorkService;
            _logger = logger;
            _permissionService = permissionService;
            _userPrincipal = userPrincipal;
            _issueService = issueService;
            _applicationConfiguration = applicationConfiguration;
            _orderService = orderService;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? issueId,
            int? issueType, int? orderId, int? orderItemId, int? paymentId)
        {
            if (orderId.HasValue)
            {
                var userHasAccessForOrder = await _orderService.CheckUserHasAccessForOrder(orderId.Value);
                if (!userHasAccessForOrder)
                {
                    _logger.InsertLog(LogLevel.Error, new EventId(28, "GetByCustomFilterForList"),
                        nameof(Factor), "GetByCustomFilterForList Error", null);
                    throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(Factor)}");
                }
            }

            var issueLogWorks =
                _issueLogWorkService.GetByCustomFilter(issueId, issueType, orderId, orderItemId, paymentId);
            return await DataSourceLoader.LoadAsync(
                issueLogWorks.ProjectTo<IssueLogWorkListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? issueId,
            int? issueType, int? orderId, int? orderItemId, int? paymentId)
        {
            var issueLogWorks =
                _issueLogWorkService.GetByCustomFilter(issueId, issueType, orderId, orderItemId, paymentId);
            return await DataSourceLoader.LoadAsync(
                issueLogWorks.ProjectTo<IssueLogWorkSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(IssueLogWorkEditModel editModel)
        {
            var issueLogWork = await _issueLogWorkService.GetAsync(editModel.Id, true);
            if (issueLogWork.CreateBy != _userPrincipal.Id)
            {
                return "کابر گرامی شما مجاز به تغییر این رکورد نمی باشد.";
            }

            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var issueLogWork = _issueLogWorkService.Get(id, true);
            if (issueLogWork == null)
            {
                return "کابر گرامی چنین رکوردی وجود ندارد.";
            }
            else
            {
                if (issueLogWork.CreateBy != _userPrincipal.Id)
                {
                    return "کابر گرامی شما مجاز به حذف این رکورد نمی باشید";
                }

                return await base.CustomValidateBeforeDeleteAsync(id);
            }
        }

        protected override async Task CustomMapBeforeInsertAsync(IssueLogWorkEditModel editModel, IssueLogWork entity)
        {
            // درصورتی که هنگام ثبت لاگ
            // issue مشخصی وجود نداشته باشد
            // باید بر اساس اطلاعات پاس داده شده یک issue
            // برای ان ایجاد کنیم
            // کاربرد این بیشتر در فرمهایی مثل ثبت اطلاعات حساب برای عودت وجه میباشد که میخواهیم توضیحاتی را ذخیره کنیم
            if (editModel.IssueId == null && (editModel.OrderId.HasValue || editModel.OrderItemId.HasValue ||
                                              editModel.PaymentId.HasValue || editModel.IssueType.HasValue))
            {
                var issue = await _issueService.GetByCustomFilter(editModel.OrderItemId, editModel.OrderId,
                    editModel.PaymentId,
                    editModel.IssueType, null, null, null, null, null, null).FirstOrDefaultAsync();
                // اگر issue وجود داشته باشد
                // پس لاگی که میزنیم برای همان issue خواهد بود.
                if (issue != null)
                {
                    editModel.IssueId = issue.Id;
                    entity.IssueId = issue.Id;
                }
                else
                {
                    entity.Issue = new Issue
                    {
                        OrderId = editModel.OrderId,
                        OrderItemId = editModel.OrderItemId,
                        PaymentId = editModel.PaymentId,
                        CreateBy = _userPrincipal.Id,
                        ApplicationId = _userPrincipal.ApplicationId,
                        IssueType = editModel.IssueType ?? (int)IssueTypeEnum.Refund,
                        Title = editModel.CustomIssueTitle,
                        Estimate = 5,
                        PriorityType = (int)PriorityTypeEnum.Medium,
                        Assignee = _userPrincipal.Id,
                        CreationDate = DateTime.Now,
                        AccessLevelId = 1,
                        CreatorOrganizationId = 1
                    };
                }
            }
        }
    }
}