using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilReport.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class AdditionalUserPropGroupController : ItemGroupController
    {
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IMapper _mapper;
        private readonly IItemGroupService _itemGroupService;
        public AdditionalUserPropGroupController(IItemGroupService itemGroupService, IMapper mapper,
            IYashilLogger<ItemGroup> logger, IPermissionService permissionService, IReportStoreService reportStoreService,
            IAdditionalUserPropService additionalUserPropService) : base(itemGroupService, mapper, logger, permissionService, reportStoreService)
        {
            _itemGroupService = itemGroupService;
            _mapper = mapper;
            _additionalUserPropService = additionalUserPropService;
        }

        public override async Task<bool> HasAnyItemAsync(int itemGroupId)
        {
            return await _additionalUserPropService.ExistsAnyAdditionalUserPropServiceByItemGroupIdAsync(itemGroupId);
        }

        protected override async Task<LoadResult> MapQueryToListViewModel(CustomDataSourceLoadOptions loadOptions, IQueryable<ItemGroup> itemGroups)
        {
            return await DataSourceLoader.LoadAsync(
                itemGroups.ProjectTo<AdditionalUserPropGroupListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override int GetAppEntityId()
        {
            return (int)AppEntityEnum.AdditionalUserProp;
        }

        protected override IQueryable<ItemGroup> GetParentItemGroups(int? itemGroupTypeSubId)
        {
            return _itemGroupService.GetAdditionalUserPropItemGroupThatCanBeParent(GetAppEntityId(),
                itemGroupTypeSubId);
        }
        /// <summary>
        /// لیست کاربران اساسن شده در این گروه
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [HttpGet("GetItemsAssignedToGroup")]
        public override async Task<LoadResult> GetItemsAssignedToGroup(CustomDataSourceLoadOptions loadOptions, int groupId)
        {
            var users = _additionalUserPropService.GetUsersAssignToGroup(groupId);
            return await DataSourceLoader.LoadAsync(users
                .ProjectTo<AdditionalUserPropCustomListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// لیست کاربرانی که گروه بندی نشده اند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="itemGroupSubId"></param>
        /// <returns></returns>
        [HttpGet("GetNotGroupedItems")]
        public override async Task<LoadResult> GetNotGroupedItems(CustomDataSourceLoadOptions loadOptions, int itemGroupSubId)
        {
            var users = _additionalUserPropService.GetUsersForNewGroup();
            return await DataSourceLoader.LoadAsync(users
                .ProjectTo<AdditionalUserPropCustomListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpPost("AssignItemGroupForEntityAndConfirm")]
        [YashilAuthorization(ActionName = "AssignItemGroupForEntityAndConfirm")]
        public override Task<bool> AssignItemGroupForEntityAndConfirm(ItemGroupSimpleViewModel itemGroupSimpleViewModel)
        {
            throw new System.NotImplementedException();
        }

        public override Task<int> GetItemGroupReportIdAsync(int groupId)
        {
            throw new System.NotImplementedException();
        }

        protected override IQueryable<ItemGroup> GetItemGroupForNextGroup(int? currentItemGroupId)
        {
            return _itemGroupService.GetAdditionalUserPropItemGroupForNextGroup(GetAppEntityId(), currentItemGroupId);
        }
    }
}
