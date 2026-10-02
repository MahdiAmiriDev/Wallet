using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IItemGroupService : IGenericService<ItemGroup, int>
    {
        IQueryable<ItemGroup> GetByCustomFilter(int? parentId, int appEntityId, int? itemGroupTypeSubId);
        /// <summary>
        /// بررسی می کند که آیا این گرو فرزند دارد یا نه
        /// </summary>
        /// <param name="itemGroup"></param>
        /// <returns></returns>
        bool CheckItemGroupIsParent(int itemGroup);
        Task<string> ConfirmItemGroup(int id, bool saveAfterConfirm);

        /// <summary>
        /// لیست نود هایی از گروه بندی که می توانند پدر باشند
        /// </summary>
        /// <param name="appEntityId"></param>
        /// <param name="itemGroupTypeSubId"></param>
        /// <returns></returns>
        IQueryable<ItemGroup> GetPaymentItemGroupThatCanBeParent(int appEntityId, int itemGroupTypeSubId);

        IQueryable<ItemGroup> GetRefundDetailItemGroupThatCanBeParent(int appEntityId, int? itemGroupTypeSubId);
        IQueryable<ItemGroup> GetRefundDetailForPaymentItemGroupThatCanBeParent(int v, int? itemGroupTypeSubId);

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
        IQueryable<ItemGroup> GetAdditionalUserPropItemGroupThatCanBeParent(int appEntityId,
            int? itemGroupTypeSubId);

        /// <summary>
        /// دریافت آیتم گروه ها برای انتخاب گروه بعدی
        /// این آیتم ها شامل گروه فعلی نمی باشد
        /// </summary>
        /// <param name="appEntityId">موجودیت</param>
        /// <param name="currentItemGroupId">شناسه گروه فعلی</param>
        /// <returns></returns>
        IQueryable<ItemGroup> GetAdditionalUserPropItemGroupForNextGroup(int appEntityId, int? currentItemGroupId);
    }
}