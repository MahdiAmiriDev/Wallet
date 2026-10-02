using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilCommerce.Core.Services
{
    public interface IModifiedOrderItemService : IGenericService<ModifiedOrderItem, int>
    {

        IQueryable<ModifiedOrderItem> GetByCustomFilter(int? oldOrderItemId, int? newOrderId, int? modifiedOrderItemType, int? modifiedOrderItemStatusType, int? assignee);
        Task<string> ApplyModificationAsync(int id, int? newProductSalePlanningId, int? representationId, bool? rejectRequest, string expertDescription = null, bool changeToItSlef = false, int? quantity = 1, OrderStatusTypesEnum newOrderStatusType = 0);
        List<CalcedOrderItemPrice> ModifyOrderItemPreview(
                int oldOrderItemId,
                int? newProductSalePlanningId,
                int? representationId,
                bool subsidyBased,
                bool? calcMaxSubsidy,
                bool changeToItSlef = false
        );
        /// <summary>
        ///  آیتم سفارش اصلاح شده را به قبل برمی گرداند
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        /// <returns></returns>
        Task<int> CancelModification(int modifiedOrderItemId);
        /// <summary>
        /// بررسی می کند که modifiedOrderItemId
        /// آخرین رکورد برای درج شده در 
        /// modifiedOrderItem
        /// برای order 
        /// می باشد 
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        /// <returns></returns>
        Task<bool> IsLastModifiedOrderItemRecord(int modifiedOrderItemId);
        /// <summary>
        /// اجازه لغو تغییر سفارش 
        /// </summary>
        /// <param name="modifiedOrderItemId"></param>
        /// <returns></returns>
        Task<ResultStatus<int>> AllowCancelModification(int modifiedOrderItemId);
        /// <summary>
        /// بررسی می کند که آیتم سفارش قبلی در وضعیت اصلاح آیتم سفارش ارسالی وجود دارد یا خیر ؟
        /// </summary>
        /// <param name="oldOrderItemId">شناسه آیتم سفارش قبلی</param>
        /// <param name="modifiedOrderItemStatusType">وضعیت اصلاح آیتم سفارش</param>
        /// <returns></returns>
        Task<bool> CheckExistOldOrderItemByModifyOrderItemStatusType(int oldOrderItemId, int modifiedOrderItemStatusType);
        /// <summary>
        /// اصلاح پرداختی اضافی سفارش :
        /// در صورتی که سفارشی پرداخت اضافی داست باید یکی از آیتم های آن سفارش را به خود آن آیتم سفارش 
        /// تغییر بدهیم با این شرط که قیمت های آیتم سفارش جدید همان قیمت های آیتم قبلی باشد
        /// </summary>
        /// <param name="orderItemId">شناسه آیتم سفارش</param>
        Task<bool> ApplyChangeOrderItemToItSelfAsync(int orderItemId);
        /// <summary>
        /// دریافت رکورد اصلاح آیتم سفارش
        /// </summary>
        /// <param name="id"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<ModifiedOrderItem> FindById(int id, bool readOnly);

        /// <summary>
        /// این متد عملیات تغییر دوره برای سفارشی که در وضعیت
        /// منتظر تایید تامین کننده است را انجام می دهد.
        /// </summary>
        /// <param name="orderItemId"></param>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<(bool isSuccess, string message)>
            ChangeOrderItemProductSalesPlanningForOrderWithWaitingForSupplierApprovalStatusAsync(int orderItemId,
                int productSalesPlanningId, string description);
    }
}
