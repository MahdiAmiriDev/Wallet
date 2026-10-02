using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Core.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.Filters;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class ModifiedOrderItemController : BaseController<ModifiedOrderItem, int, ModifiedOrderItemListViewModel, ModifiedOrderItemEditModel, ModifiedOrderItemSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IModifiedOrderItemService _modifiedOrderItemService;
        private readonly IYashilLogger<ModifiedOrderItem> _logger;
        private readonly IOrderService _orderService;
        private readonly IFactorItemService _factorItemService;

        public ModifiedOrderItemController(IModifiedOrderItemService modifiedOrderItemService, IMapper mapper,
            IYashilLogger<ModifiedOrderItem> logger, IPermissionService permissionService, IOrderService orderService, IFactorItemService factorItemService) : base(modifiedOrderItemService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _modifiedOrderItemService = modifiedOrderItemService;
            _logger = logger;
            _orderService = orderService;
            _factorItemService = factorItemService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? oldOrderItemId, int? newOrderId, int? modifiedOrderItemType, int? modifiedOrderItemStatusType, int? assignee)
        {
            var modifiedOrderItems = _modifiedOrderItemService.GetByCustomFilter(oldOrderItemId, newOrderId, modifiedOrderItemType, modifiedOrderItemStatusType, assignee);
            return await DataSourceLoader.LoadAsync(modifiedOrderItems.ProjectTo<ModifiedOrderItemListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomFilterForModify")]
        [YashilAuthorization(ActionName = "ModifiedOrderItemByCustomFilterForModify")]
        public async Task<LoadResult> GetByCustomFilterForModify(CustomDataSourceLoadOptions loadOptions, int? oldOrderItemId, int? newOrderId, int? modifiedOrderItemType, int? modifiedOrderItemStatusType, int? assignee)
        {
            if (loadOptions.Sort == null)
            {
                loadOptions.Sort = new[] { new SortingInfo { Selector = "Id", Desc = true } };
            }
            var modifiedOrderItems = _modifiedOrderItemService.GetByCustomFilter(oldOrderItemId, newOrderId, modifiedOrderItemType, modifiedOrderItemStatusType, assignee);
            return await DataSourceLoader.LoadAsync(modifiedOrderItems.ProjectTo<ModifiedOrderItemListForModifyViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomFilterForModifyAction")]
        [YashilAuthorization(ActionName = "ModifiedOrderItemByCustomFilterForModifyAction")]
        public async Task<LoadResult> GetByCustomFilterForModifyAction(CustomDataSourceLoadOptions loadOptions, int? oldOrderItemId, int? newOrderId, int? modifiedOrderItemType, int? modifiedOrderItemStatusType, int? assignee)
        {
            if (loadOptions.Sort == null)
            {
                loadOptions.Sort = new[] { new SortingInfo { Selector = "Id", Desc = true } };
            }
            var modifiedOrderItems = _modifiedOrderItemService.GetByCustomFilter(oldOrderItemId, newOrderId, modifiedOrderItemType, modifiedOrderItemStatusType, assignee);
            return await DataSourceLoader.LoadAsync(modifiedOrderItems.ProjectTo<ModifiedOrderItemListForModifyActionViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? oldOrderItemId, int? newOrderId, int? modifiedOrderItemType, int? modifiedOrderItemStatusType, int? assignee)
        {
            var modifiedOrderItems = _modifiedOrderItemService.GetByCustomFilter(oldOrderItemId, newOrderId, modifiedOrderItemType, modifiedOrderItemStatusType, assignee);
            return await DataSourceLoader.LoadAsync(modifiedOrderItems.ProjectTo<ModifiedOrderItemSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task CustomMapBeforeInsertAsync(ModifiedOrderItemEditModel editModel, ModifiedOrderItem entity)
        {
            entity.ModifiedOrderItemStatusType = (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier;


            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var modifiedOrderItem = await _modifiedOrderItemService.GetAsync(id, true);

            await CheckUserHasAccessForOrderItem(modifiedOrderItem.OldOrderItemId);

            if (modifiedOrderItem.ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier || modifiedOrderItem.ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.RejectedByCrmExpert)
                return await base.CustomValidateBeforeDeleteAsync(id);
            else
            {
                return "کاربر گرامی  شما مجاز به حذف این رکورد نمی باشید.";
            }
        }

        [HttpPost("UpdateNewOrderItem")]
        [YashilAuthorization(ActionName = "PostEntity")]
        public async Task<IActionResult> UpdateNewOrderItem(ModifiedOrderItemSimpleEditModel editModel)
        {
            var modifiedOrderItem = await _modifiedOrderItemService.GetAsync(editModel.Id, true);
            await CheckUserHasAccessForOrderItem(modifiedOrderItem.OldOrderItemId);

            var result = await _modifiedOrderItemService.ApplyModificationAsync(editModel.Id, editModel.ProductSalePlanningId, editModel.RepresentationId, editModel.RejectRequest,editModel.ExpertDescription);
            if (!string.IsNullOrEmpty(result))
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = result
                });
            }
            else
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "اعمال تغییر سفارش با موفقیت انجام شد"
                });
            }

        }

        [HttpGet("CheckOrderItemHasBeenFactored")]
        [YashilAuthorization(ActionName = "PostEntity")]
        public async Task<IActionResult> CheckOrderItemHasBeenFactored(int modifiedOrderItemId)
        {
            var oldOrderItemId = await 
                 _modifiedOrderItemService.FindById(modifiedOrderItemId, true)
                     .Select(x=>x.OldOrderItemId)
                     .FirstOrDefaultAsync();

            var orderItemHasBeenFactored = await _factorItemService
                .CheckOrderItemHasBeenFactored(oldOrderItemId);


            if (orderItemHasBeenFactored)
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "کاربر گرامی سفارش انتخاب شده فاکتور شده می باشد، آیا از لغو سفارش اطمینان دارید؟"
                });
            }
            else
            {
                return Accepted(new ResultStatus<int>
                {
                    IsSuccess = false
                });
            }

        }


        /// <summary>
        ///  لغو اصلاح انجام شده
        /// </summary>
        /// <param name="modifiedOrderItemSimpleViewModel"></param>
        /// <returns></returns>
        [HttpPost("CancelModification")]
        [YashilAuthorization(ActionName = "CancelModification")]
        public async Task<ResultStatus<int>> CancelModification(ModifiedOrderItemSimpleViewModel modifiedOrderItemSimpleViewModel)
        {

            var modifiedOrderItem = await _modifiedOrderItemService.GetAsync(modifiedOrderItemSimpleViewModel.Id, true);

            //  اجازه اینکه می تواند آیتم سفارش اصلاح شده را به قبل از اصلاح بر گرداند
            var result = await _modifiedOrderItemService.AllowCancelModification(modifiedOrderItemSimpleViewModel.Id);
            if (!result.IsSuccess)
            {
                return new ResultStatus<int>(false, result.Message);
            }

            await _modifiedOrderItemService.CancelModification(modifiedOrderItemSimpleViewModel.Id);

            return new ResultStatus<int>(true, "کاربر گرامی آیتم سفارش اصلاح شده را به قبل برگشت داده شد.");
        }



        /// <summary>
        /// قبل از درج باید بررسی کنیم که آیتم سفارشی که برای اصلاح فرستاده می شود قبلا در وضعیت ثبت توسط تامین کننده رکوردی نداشته باشد
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeInsertAsync(ModifiedOrderItemEditModel editModel)
        {

            await CheckUserHasAccessForOrderItem(editModel.OldOrderItemId);
            //  بررسی می کند که رکوردی برای آیتم سفارشی قبلی در وضعیت ثبت تامین کننده وجود دارد؟ 
            var existOldOrderItemInRegisteredBySupplierStatus = await _modifiedOrderItemService.
                CheckExistOldOrderItemByModifyOrderItemStatusType(editModel.OldOrderItemId, (int)ModifiedOrderItemStatusTypeEnum.RegisteredBySupplier);
            if (existOldOrderItemInRegisteredBySupplierStatus)
            {
                return "بروز خطا در ثبت درخواست اصلاح سفارش";
            }
            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        /// <summary>
        /// اصلاح پرداختی اضافی سفارش :
        /// در صورتی که سفارشی پرداخت اضافی داست باید یکی از آیتم های آن سفارش را به خود آن آیتم سفارش 
        /// تغییر بدهیم با این شرط که قیمت های آیتم جدید همان قیمت های آیتم قبلی باشد
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <returns></returns>
        [HttpPost("ChangeOrderItemToItSelfAsync")]
        [YashilAuthorization(ActionName = "ChangeOrderItemToItSelf")]
        public async Task<ResultStatus<int>> ChangeOrderItemToItSelfAsync(int orderItemId)
        {
            await CheckUserHasAccessForOrderItem(orderItemId);

            var isSuccessfulChangeToItSelf = await _modifiedOrderItemService.ApplyChangeOrderItemToItSelfAsync(orderItemId);
            if (isSuccessfulChangeToItSelf)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = true,
                    Message = "تغییر آیتم سفارش به خود آیتم با موفقیت انجام شد"
                };
            }
            return new ResultStatus<int>
            {
                IsSuccess = false,
                Message = "خطا در انجام تغییر آیتم سفارش به خود آیتم"
            };
        }

        async Task CheckUserHasAccessForOrder(int orderId)
        {
            var userHasAccessForOrder = await _orderService.CheckUserHasAccessForOrder(orderId);
            if (!userHasAccessForOrder)
            {
                _logger.InsertLog(LogLevel.Error, new EventId(28, "CustomValidateBeforeInsertAsync"),
                    nameof(ModifiedOrderItem), "CheckUserHasAccessForOrder Error", null);
                throw new Exception($"{Events.AccessDeniedGetEntities} : {nameof(ModifiedOrderItem)}");
            }
        }
        private async Task CheckUserHasAccessForOrderItem(int orderItemId)
        {
            Order order = await _orderService.GetByOrderItemIdAsync(orderItemId);
            await CheckUserHasAccessForOrder(order.Id);
        }
    }
}