using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilReport.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class RefundDetailGroupController : ItemGroupController
    {
        private readonly IRefundDetailService _refundDetailService;
        private readonly IMapper _mapper;
        private readonly IItemGroupService _itemGroupService;
        private readonly IPermissionService _permissionService;
        private readonly IYashilLogger<RefundDetail> _logger;

        public RefundDetailGroupController(IItemGroupService itemGroupService, IMapper mapper, 
            IYashilLogger<ItemGroup> logger, IPermissionService permissionService,
            IRefundDetailService refundDetailService, IYashilLogger<RefundDetail> logger1,
            IReportStoreService reportStoreService) : base(itemGroupService, mapper, logger, permissionService, reportStoreService)
        {
            _itemGroupService = itemGroupService;
            _mapper = mapper;
            _permissionService = permissionService;
            _refundDetailService = refundDetailService;
            _logger = logger1;
        }

        public override Task<bool> HasAnyItemAsync(int itemGroupId)
        {
            return _refundDetailService.ExistsAnyRefundDetailWithThisItemGroupIdAsync(itemGroupId);
        }

        protected override async Task<LoadResult> MapQueryToListViewModel(CustomDataSourceLoadOptions loadOptions, IQueryable<ItemGroup> itemGroups)
        {
            return await DataSourceLoader.LoadAsync(itemGroups.ProjectTo<RefundDetailGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override int GetAppEntityId()
        {
            return (int)AppEntityEnum.RefundDetail;
        }

        protected override IQueryable<ItemGroup> GetParentItemGroups(int? itemGroupTypeSubId)
        {
            return _itemGroupService.GetRefundDetailItemGroupThatCanBeParent(GetAppEntityId(), itemGroupTypeSubId);
        }
        /// <summary>
        /// جهت قراردادن پرداختی هایی که تاکنون گروه بندی نشده اند به گروه انتخابی و ثبت نهایی کردن آن گروه
        /// </summary>
        /// <param name="itemGroupSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("AssignItemGroupForEntityAndConfirm")]
        [YashilAuthorization(ActionName = "AssignItemGroupForEntityAndConfirm")]
        public override Task<bool> AssignItemGroupForEntityAndConfirm(ItemGroupSimpleViewModel itemGroupSimpleViewModel)
        {
            return _refundDetailService.AssignItemGroupForRefundDetailAndConfirm(itemGroupSimpleViewModel.Id);
        }

        [YashilAuthorization(ActionName = "GetItemGroupReportIdAsync")]
        public override async Task<int> GetItemGroupReportIdAsync(int groupId)
        {
            _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(RefundDetail), "GetItemGroupReportId Record", null);

            return await _refundDetailService.GetRefundDetailGroupReportIdAsync(groupId);
        }

        protected override IQueryable<ItemGroup> GetItemGroupForNextGroup(int? currentItemGroupId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// لیست عودت وجه های این گروه
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("GetItemsAssignedToGroup")]
        public override async Task<LoadResult> GetItemsAssignedToGroup(CustomDataSourceLoadOptions loadOptions, int groupId)
        {
            IQueryable<RefundDetail> refundDetails = _refundDetailService.GetRefundDetailsAssignedToGroup(groupId);
            return await DataSourceLoader.LoadAsync(refundDetails.ProjectTo<RefundDetailListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// لیست عودت وجه های گروه بندی نشده
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="itemGroupSubId">paymentMethodId</param>
        /// <returns></returns>
        [HttpGet("GetNotGroupedItems")]
        public override async Task<LoadResult> GetNotGroupedItems(CustomDataSourceLoadOptions loadOptions, int itemGroupSubId)
        {
            var refundDetails = _refundDetailService.GetRefundDetailsForNewGroup(itemGroupSubId);
            return await DataSourceLoader.LoadAsync(refundDetails.ProjectTo<RefundDetailListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
     
    }
}
