using System;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using Microsoft.Extensions.Logging;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilReport.Core.Services;
using YashilUserManagement.Core.Services;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class PaymentGroupController : ItemGroupController
    {

        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        private readonly IItemGroupService _itemGroupService;
        private readonly IYashilLogger<RefundDetail> _logger;
        private readonly IPermissionService _permissionService;
        private readonly IAppConfigService _appConfigService;

        public PaymentGroupController(IItemGroupService itemGroupService, IMapper mapper, IYashilLogger<ItemGroup> logger, IPermissionService permissionService, IPaymentService paymentService, IItemGroupService groupService, IYashilLogger<RefundDetail> logger1, IAppConfigService appConfigService, IReportStoreService reportStoreService) : base(itemGroupService, mapper, logger, permissionService, reportStoreService)
        {
            _mapper = mapper;
            _permissionService = permissionService;
            _paymentService = paymentService;
            _itemGroupService = groupService;
            _logger = logger1;
            _appConfigService = appConfigService;
        }

        public override Task<bool> HasAnyItemAsync(int itemGroupId)
        {
            return _paymentService.ExistsAnyPaymentWithThisItemGroupIdAsync(itemGroupId);
        }

        protected override async Task<LoadResult> MapQueryToListViewModel(CustomDataSourceLoadOptions loadOptions,
            IQueryable<ItemGroup> itemGroups)
        {
            return await DataSourceLoader.LoadAsync(itemGroups.ProjectTo<WalletPaymentGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override int GetAppEntityId()
        {
            return (int)AppEntityEnum.Payment;
        }

        protected override IQueryable<ItemGroup> GetParentItemGroups(int? itemGroupSubId)
        {
            if (itemGroupSubId.HasValue)
            {
                return _itemGroupService.GetPaymentItemGroupThatCanBeParent(GetAppEntityId(), itemGroupSubId.Value);
            }
            throw new Exception("کد باشگاه مشتریان مقدار دهی نشده است");
        }

        public override async Task<LoadResult> GetItemsAssignedToGroup(CustomDataSourceLoadOptions loadOptions, int groupId)
        {
            var payments = _paymentService.GetCustomerClubWalletPaymentsAssignedToPaymentGroup(groupId);
            return await DataSourceLoader.LoadAsync(payments.ProjectTo<CustomerClubPaymentGroupItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [YashilAuthorization(ActionName = "GetNotGroupedItems")]
        public override async Task<LoadResult> GetNotGroupedItems(CustomDataSourceLoadOptions loadOptions, int itemGroupSubId)
        {
            var entities = _paymentService.GetCustomerClubWalletPaymentsNotAssignedToPaymentGroup(itemGroupSubId, true);
            return await DataSourceLoader.LoadAsync(entities.ProjectTo<CustomerClubPaymentGroupItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [YashilAuthorization(ActionName = "AssignItemGroupForEntityAndConfirm")]
        public override async Task<bool> AssignItemGroupForEntityAndConfirm(ItemGroupSimpleViewModel itemGroupSimpleViewModel)
        {
            return await _paymentService.AssignAndConfirmCustomerClubWalletPaymentGroup(itemGroupSimpleViewModel.Id);
        }

        [YashilAuthorization(ActionName = "GetItemGroupReportIdAsync")]
        public override async Task<int> GetItemGroupReportIdAsync(int groupId)
        {
            _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(Payment), "GetItemGroupReportId Record", null);

            return await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommercePaymentGroupReportId);
        }

        protected override IQueryable<ItemGroup> GetItemGroupForNextGroup(int? currentItemGroupId)
        {
            throw new System.NotImplementedException();
        }
    }
}
