using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IItemGroupRepository : IGenericRepository<ItemGroup, int>
    {
        IQueryable<ItemGroup> GetByCustomFilter(int? parentId, int appEntityId, int? itemGroupTypeSubId);
        /// <summary>
        /// بررسی میکند که ابا آبدب گروه آیتم داده شده پدر است یا خیر
        /// </summary>
        /// <param name="itemGroupId"></param>
        /// <returns></returns>
        bool CheckItemGroupIsParent(int itemGroupId);

        IQueryable<ItemGroup> GetPaymentItemGroupThatCanBeParent(int appEntityId, int customerClubId);
        IQueryable<ItemGroup> GetRefundDetailItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId);
        IQueryable<ItemGroup> GetRefundDetailForPaymentItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId);

        /// <summary>
        /// لیست نود هایی از گروه بندی که می توانند پدر برای مطالبات ناجا باشند
        /// </summary>
        /// <param name="appEntityId"></param>
        /// <param name="customerClubId"></param>
        /// <returns></returns>
        IQueryable<ItemGroup> GetCustomerClubPaymentInstallmentItemGroupThatCanBeParent(int appEntityId, int customerClubId);
        /// <summary>
        /// لیست نود هایی از گوره بندی که می توانند برای کاربران باشند
        /// </summary>
        /// <param name="appEntityId"></param>
        /// <param name="itemGroupTypeSubId"></param>
        /// <returns></returns>
        IQueryable<ItemGroup> GetAdditionalUserPropItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId);

        /// <summary>
        /// دریافت آیتم گروه ها برای انتخاب گروه بعدی
        /// این آیتم ها شامل گروه فعلی نمی باشد
        /// </summary>
        /// <param name="appEntityId">موجودیت</param>
        /// <param name="currentItemGroupId"></param>
        /// <returns></returns>
        IQueryable<ItemGroup> GetAdditionalUserPropItemGroupForNextGroup(int appEntityId, int? currentItemGroupId);
    }
}