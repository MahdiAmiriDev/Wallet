using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.Extensions.Logging;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilReport.Core.Services;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    /// <summary>
    ///
    /// گروه بندی مطالبات ناجا
    /// </summary>
    public class CustomerClubPaymentInstallmentGroupController : ItemGroupController
    {
        private readonly ICustomerClubPaymentInstallmentService _customerClubPaymentInstallmentService;
        private readonly IItemGroupService _itemGroupService;
        private readonly IPermissionService _permissionService;
        private readonly IAppConfigService _appConfigService;
        private readonly IYashilLogger<CustomerClubPaymentInstallment> _logger;
        private readonly IMapper _mapper;


        public CustomerClubPaymentInstallmentGroupController(IItemGroupService itemGroupService, IMapper mapper, IYashilLogger<ItemGroup> logger, IPermissionService permissionService, IYashilLogger<RefundDetail> logger1, IAppConfigService appConfigService, IReportStoreService reportStoreService, ICustomerClubPaymentInstallmentService customerClubPaymentInstallmentService, IYashilLogger<CustomerClubPaymentInstallment> logger2) : base(itemGroupService, mapper, logger, permissionService, reportStoreService)
        {
            _itemGroupService = itemGroupService;
            _mapper = mapper;
            _permissionService = permissionService;
            _appConfigService = appConfigService;
            _customerClubPaymentInstallmentService = customerClubPaymentInstallmentService;
            _logger = logger2;
        }
        /// <summary>
        /// بررسی اینکه گروه داده شده آیتمی دارد یا خیر
        /// </summary>
        /// <param name="itemGroupId"></param>
        /// <returns></returns>
        public override Task<bool> HasAnyItemAsync(int itemGroupId)
        {
            return _customerClubPaymentInstallmentService.ExistsAnyCustomerClubPaymentInstallmentWithThisItemGroupIdAsync(itemGroupId);
        }
        /// <summary>
        /// مپ کردن به لیست ویو مدل جهت نمایش
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="itemGroups"></param>
        /// <returns></returns>
        protected override async Task<LoadResult> MapQueryToListViewModel(CustomDataSourceLoadOptions loadOptions, IQueryable<ItemGroup> itemGroups)
        {
            return await DataSourceLoader.LoadAsync(itemGroups.ProjectTo<CustomerClubPaymentInstallmentGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);

        }
        /// <summary>
        /// دریافت آیدی appentity
        /// </summary>
        /// <returns></returns>
        protected override int GetAppEntityId()
        {
            return (int)AppEntityEnum.CustomerClubPaymentInstallment;
        }

        protected override IQueryable<ItemGroup> GetParentItemGroups(int? itemGroupTypeSubId)
        {
            if (itemGroupTypeSubId.HasValue)
            {
                return _itemGroupService.GetCustomerClubPaymentInstallmentItemGroupThatCanBeParent(GetAppEntityId(), itemGroupTypeSubId.Value);
            }
            throw new Exception("کد باشگاه مشتریان مقدار دهی نشده است");
        }
        /// <summary>
        /// دریافت لیست آیتم های اساین شده به گروه یا متعلق به گروه
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public override async Task<LoadResult> GetItemsAssignedToGroup(CustomDataSourceLoadOptions loadOptions, int groupId)
        {
            var customerClubPaymentInstallments = _customerClubPaymentInstallmentService.GetCustomerClubPaymentInstallmentsAssignedToDemandGroup(groupId);
            return await DataSourceLoader.LoadAsync(customerClubPaymentInstallments.ProjectTo<CustomerClubPaymentInstallmentGroupItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// دریافت مطالبات گروه بندی نشده
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="itemGroupSubId"></param>
        /// <returns></returns>
        [YashilAuthorization(ActionName = "GetNotGroupedItems")]
        public override async Task<LoadResult> GetNotGroupedItems(CustomDataSourceLoadOptions loadOptions, int itemGroupSubId)
        {
            var entities = _customerClubPaymentInstallmentService.GetCustomerClubPaymentInstallmentsNotAssignedToDemandGroupAsync(itemGroupSubId, true);
            return await DataSourceLoader.LoadAsync(entities.ProjectTo<CustomerClubPaymentInstallmentGroupItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// گروه بندی مطالیات ناجا
        /// </summary>
        /// <param name="itemGroupSimpleViewModel"></param>
        /// <returns></returns>
        [YashilAuthorization(ActionName = "AssignItemGroupForEntityAndConfirm")]
        public override Task<bool> AssignItemGroupForEntityAndConfirm(ItemGroupSimpleViewModel itemGroupSimpleViewModel)
        {
            return _customerClubPaymentInstallmentService.AssignAndConfirmCustomerClubPaymentInstallmentDemandGroup(itemGroupSimpleViewModel.Id);
        }
        /// <summary>
        /// دریافت گزارش مطالبات
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [YashilAuthorization(ActionName = "GetItemGroupReportIdAsync")]
        public override async Task<int> GetItemGroupReportIdAsync(int groupId)
        {
            _logger.InsertLog(LogLevel.Information, new EventId((int)ECommerceEvents.ExportToExcel, Events.Insert.ToString()), nameof(CustomerClubPaymentInstallment), "GetItemGroupReportId Record", null);

            return await _appConfigService.GetIntValueAsync(ApplicationConfigKey.YashilCommerceCustomerClubPaymentInstallmentDemandGroupReportId);
        }

        protected override IQueryable<ItemGroup> GetItemGroupForNextGroup(int? currentItemGroupId)
        {
            throw new System.NotImplementedException();
        }
    }
}
