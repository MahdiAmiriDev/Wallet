using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;

namespace YashilCommerce.Core.Repositories
{
	public interface IModifiedOrderItemRepository : IGenericRepository<ModifiedOrderItem,int>
    {
		          
        IQueryable<ModifiedOrderItem> GetByCustomFilter( int? oldOrderItemId, int? newOrderId, int? modifiedOrderItemType, int? modifiedOrderItemStatusType, int? assignee);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldOrderItemId">شناسه آیتم سفارش قبلی</param>
        /// <param name="newProductSalePlanningId">شناسه آیتم سفارش جدید</param>
        /// <param name="representationId">شناسه نمایندگی</param>
        /// <param name="subsidyBased">یارانه می باشد></param>
        /// <param name="calcMaxSubsidy">سقف یارانه محاسبه شود</param>
        /// <param name="ChangeToItSlef">سفارش قبلی به خودش تغییر یابد؟</param>
        /// <returns></returns>
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
        /// بررسی می کند که آیتم سفارش قبلی در وضعیت اصلاح آیتم سفارش ارسالی وجود دارد یا خیر ؟
        /// </summary>
        /// <param name="oldOrderItemId">شناسه آیتم سفارش قبلی</param>
        /// <param name="modifiedOrderItemStatusType">وضعیت اصلاح آیتم سفارش</param>
        /// <returns></returns>
        Task<bool> CheckExistOldOrderItemByModifyOrderItemStatusType(int oldOrderItemId, int modifiedOrderItemStatusType);

        IQueryable<ModifiedOrderItem> FindById(int id, bool readOnly);

        /// <summary>
        /// تغییر
        /// ModifiedOrderItemType
        /// مربوط به یک ایتم سفارش تغییر کرده به حالت جدید
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newStatusType"></param>
        /// <returns></returns>
        Task<int> UpdateModifiedOrderItemStatusTypeAsync(int id, ModifiedOrderItemStatusTypeEnum newStatusType);
    }
}      
 