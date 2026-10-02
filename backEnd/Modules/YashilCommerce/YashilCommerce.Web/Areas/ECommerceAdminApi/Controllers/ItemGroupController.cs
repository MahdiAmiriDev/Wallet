using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Report;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomDomainModels.Report;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilReport.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public abstract class ItemGroupController : BaseController<ItemGroup, int, ItemGroupListViewModel, ItemGroupEditModel,
        ItemGroupSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IItemGroupService _itemGroupService;
        private readonly IReportStoreService _reportStoreService;
        protected ItemGroupController(IItemGroupService itemGroupService, IMapper mapper, IYashilLogger<ItemGroup> logger,
            IPermissionService permissionService, IReportStoreService reportStoreService) : base(itemGroupService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _itemGroupService = itemGroupService;
            _reportStoreService = reportStoreService;
        }

        /// <summary>
        /// دریافت لیست گروه بندی بر اساس itemGroupType, itemGroupTypeSubId
        /// این لیست میتواند لیست گروه بندی کیف پول پرداخت باشگاه مشتریان باشد
        /// و یا گروه بندی های دیگر که بر اساس ورودی ها تعیین میگردد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="parentId"></param>
        /// <param name="itemGroupTypeSubId"></param>
        /// <returns></returns>
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? parentId, int? itemGroupTypeSubId)
        {
            var itemGroups = _itemGroupService.GetByCustomFilter(parentId, GetAppEntityId(), itemGroupTypeSubId);
            return await MapQueryToListViewModel(loadOptions, itemGroups);
        }
        /// <summary>
        /// لیست نود هایی که می توانند به عنوان پدر انتخاب شوند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="itemGroupSubId"></param>
        /// <returns></returns>
        [HttpGet("GetParentItemGroups")]
        public async Task<LoadResult> GetParentItemGroups(CustomDataSourceLoadOptions loadOptions, int? itemGroupSubId)
        {
            var itemGroups = GetParentItemGroups(itemGroupSubId);
            return await DataSourceLoader.LoadAsync(
                itemGroups.ProjectTo<ItemGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task CustomMapBeforeInsertAsync(ItemGroupEditModel editModel, ItemGroup entity)
        {
            // هنگام ایجاد گروه گروه در حالت ثبت اولیه قرار میگیرد
            entity.ItemGroupStatusType = (int)ItemGroupStatusTypeEnum.Initial;
            entity.AppEntityId = GetAppEntityId();
            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(ItemGroupEditModel editModel)
        {
            if (!editModel.ParentId.HasValue)
            {
                return await base.CustomValidateBeforeInsertAsync(editModel);
            }
            // اگر گروه پدری انتخاب شده باشد
            // چک میشود که گروه پدر مجاز است که بعنوان گروه پدر انتخاب شود یا خیر
            var parentGroup = _itemGroupService.Get(editModel.ParentId, true);
            if (parentGroup != null)
            {
                var parentHasAnyItem = await HasAnyItemAsync(editModel.ParentId.Value);

                if (!parentHasAnyItem)
                {
                    return await base.CustomValidateBeforeInsertAsync(editModel);
                }
                return "کاربر گرامی گروه انتخاب شده بعنوان پدر دارای آیتم میباشد و نمیتواند بعنوان گروه پدر انتخاب شود";
            }
            return "کاربر گرامی گروه پدر انتخاب شده صحیح نیست";
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(ItemGroupEditModel editModel)
        {
            var itemGroup = await _itemGroupService.GetAsync(editModel.Id, true);
            // اگر گروه در حالت ثبت نهایی باشد امکان ویرایش ندارد
            if (itemGroup.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Confirmed)
            {
                return "کاربر گرامی این گروه نهایی شده است و امکان ویرایش ندارد";
            }
            // در صورتی که گروه پدر خودش باشد دچار لوپ میشویم و خطا به کاربر نشان داده میشود
            if (editModel.ParentId.HasValue && editModel.Id == editModel.ParentId.Value)
            {
                return "کاربر گرامی یک گروه نمی تواند پدر خود باشد";
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var itemGroup = _itemGroupService.Get(id, false);
            // در صورتی که گروه در حالت ثبت نهایی باشد اجازه حذف کردن ندارد
            if (itemGroup.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Confirmed)
            {
                return "کاربر گرامی این گروه ثبت نهایی شده است و امکان ویرایش یا بروزرسانی ندارد";
            }
            // اگر گروه پدر باشد نمی تواند حذف شود و باید ابتدا گروه های فرزند آن را ویرایش یا حذف کنیم
            bool hasAnyItem = await HasAnyItemAsync(itemGroup.Id);
            if (!hasAnyItem)
            {
                if (!_itemGroupService.CheckItemGroupIsParent(itemGroup.Id))
                {
                    return await base.CustomValidateBeforeDeleteAsync(id);
                }
                return "کاربر گرامی این گروه نمی تواند حذف شود";
            }
            else
            {
                return "با توجه به اینکه به این گروه آیتم تخصیص داده شده است، امکان حذف وجود ندارد";
            }
        }

        // ثبت نهایی گروه
        [HttpPost("ConfirmItemGroup")]
        [YashilAuthorization(ActionName = "PostEntity")]
        public async Task<IActionResult> ConfirmItemGroup(ItemGroupSimpleViewModel itemGroupSimpleViewModel)
        {
            string message;
            // ثبت نهایی گروه
            bool hasAnyItem = await HasAnyItemAsync(itemGroupSimpleViewModel.Id);
            if (hasAnyItem)
            {
                message = await _itemGroupService.ConfirmItemGroup(itemGroupSimpleViewModel.Id, true);
                if (!string.IsNullOrEmpty(message))
                {
                    return Accepted(new ResultStatus<int>()
                    {
                        IsSuccess = true,
                        Message = message
                    });
                }
            }
            else
            {
                message = "گروه انتخاب شده دارای هیچ آیتمی نمی باشد";
            }

            return Accepted(new ResultStatus<int>()
            {
                IsSuccess = false,
                Message = message
            });

        }

        /// <summary>
        /// آیا دارای آیتم می باشد یا نه
        /// </summary>
        /// <param name="itemGroupId"></param>
        /// <returns></returns>
        [NonAction]
        public abstract Task<bool> HasAnyItemAsync(int itemGroupId);
        /// <summary>
        /// مپ کردن کووری و ویو مدل اختصاصی هر گروه بندی
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="itemGroups"></param>
        /// <returns></returns>
        protected abstract Task<LoadResult> MapQueryToListViewModel(CustomDataSourceLoadOptions loadOptions, IQueryable<ItemGroup> itemGroups);

        protected abstract int GetAppEntityId();
        protected abstract IQueryable<ItemGroup> GetParentItemGroups(int? itemGroupTypeSubId);

        [HttpGet("GetItemsAssignedToGroup")]
        public abstract Task<LoadResult> GetItemsAssignedToGroup(CustomDataSourceLoadOptions loadOptions, int groupId);

        [HttpGet("GetNotGroupedItems")]
        public abstract Task<LoadResult> GetNotGroupedItems(CustomDataSourceLoadOptions loadOptions, int itemGroupSubId);
        /// <summary>
        /// ایتم انتخاب شده را به گروه اساین میکند و
        /// گروه را در حالت ثبت نهایی قرار میدهد.
        /// </summary>
        /// <param name="itemGroupSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("AssignItemGroupForEntityAndConfirm")]
        public abstract Task<bool> AssignItemGroupForEntityAndConfirm(ItemGroupSimpleViewModel itemGroupSimpleViewModel);

        /// <summary>
        /// ایجاد و دانلود گزارش آیتم های گروه بندی شده گروه بندی 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("GenerateGroupedItemsReport")]
        public async Task<FileResult> GenerateGroupedItemsReport(int groupId)
        {
            int itemGroupReportId = await GetItemGroupReportIdAsync(groupId);
            var paymentGroup = await _itemGroupService.GetAsync(groupId, true);
            if (paymentGroup.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Confirmed)
            {
                var reportStoreWithParam = new ReportStoreWithParam
                {
                    Params = new Dictionary<string, string> { { "groupId", paymentGroup.Id.ToString() } }
                };

                var reportFile = await _reportStoreService.GetReportFileAsync(itemGroupReportId, StiExportFormat.Excel2007, reportStoreWithParam);
                if (reportFile != null)
                {
                    return File(reportFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{paymentGroup.Title}.xlsx");
                }
            }
            return null;
        }
        [NonAction]
        public abstract Task<int> GetItemGroupReportIdAsync(int groupId);

        protected abstract IQueryable<ItemGroup> GetItemGroupForNextGroup(int? currentItemGroupId);
        /// <summary>
        /// دریافت آیتم گروه ها برای انتخاب گروه بعدی
        /// این آیتم ها شامل گروه فعلی نمی باشد
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="currentItemGroupId"></param>
        /// <returns></returns>
        [HttpGet("GetItemGroupForNextGroupAsync")]
        [YashilAuthorization("GetForSelect")]
        public async Task<LoadResult> GetItemGroupForNextGroupAsync(CustomDataSourceLoadOptions loadOptions, int? currentItemGroupId)
        {
            var itemGroups = GetItemGroupForNextGroup(currentItemGroupId);
            return await DataSourceLoader.LoadAsync(
                itemGroups.ProjectTo<ItemGroupSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
    }
}