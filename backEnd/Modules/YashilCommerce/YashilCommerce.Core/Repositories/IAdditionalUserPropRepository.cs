using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IAdditionalUserPropRepository : IGenericRepository<AdditionalUserProp, int>
    {
        AdditionalUserProp GetByUserId(int userId, bool readOnly = false);
        IQueryable<AdditionalUserProp> FindByUserId(int id);

        /// <summary>
        /// چک میکند کاربری با شناسه باشگاه و کد باشگاه وجود دارد یا نه
        /// </summary>
        /// <param name="customerClubId"></param>
        /// <param name="customerClubCode"></param>
        /// <returns></returns>
        bool CheckExistenceByFilter(int? customerClubId, string customerClubCode);

        /// <summary>
        /// دریافت اطلاعات اضافی کاربر بر اساس یوزرآیدی و اپیکیلیکشن
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        AdditionalUserProp GetByUserIdAndApplicationId(int userId, int applicationId);

        IQueryable<AdditionalUserProp> GetByCustomFilter(int? userId, int? applicationId);
        /// <summary>
        /// کاربرانی که به مرکز اختصاص داده شده اند را بر می گرداند
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        IQueryable<AdditionalUserProp> GetByCompanyId(int companyId, bool readOnly = true);
        /// <summary>
        /// بررسی می کند آیا برای دسته بندی ارسالی در پارمتر
        ///  دسته بندی نمایندگی در جدول
        /// AddtionalUserProrp
        /// وجود دارد یا خیر
        /// </summary>
        /// <param name="categoryId">دسته بندی</param>
        /// <returns></returns>
        Task<bool> CheckExistRepresentationCategoryByCategoryIdAsync(int categoryId);
        /// <summary>
        /// بررسی می کند با این شناسه گروه کاربری وجود دارد یا خیر
        /// </summary>
        /// <param name="itemGroupId">شناسه گروه</param>
        /// <returns></returns>
        Task<bool> ExistsAnyAdditionalUserPropServiceByItemGroupId(int itemGroupId);

        /// <summary>
        /// دریافت کاربران اساین شده در گروه
        /// </summary>
        /// <param name="groupId">شنایه گروه</param>
        /// <param name="readOnly">فقط خواندنی</param>
        /// <returns></returns>
        IQueryable<AdditionalUserProp> GetUsersAssignToGroup(int groupId, bool readOnly = false);
        /// <summary>
        /// لیست کاربرانی که گروه بندی نشده اند
        /// </summary>
        /// <returns></returns>
        IQueryable<AdditionalUserProp> GetUsersForNewGroup(bool readOnly = true);
        /// <summary>
        /// دریافت کاربران اساین شده به گروه
        /// </summary>
        /// <param name="groupId">شناسه گروه</param>
        /// <param name="companyId">مرکز</param>
        /// <param name="representationId">نمایندگی</param>
        /// <param name="categoryId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        Task<IQueryable<AdditionalUserProp>> GetAdditionalUserPropNotAssignedToGroupAsync(
            int groupId,
            int? companyId,
            int? representationId,
            int? categoryId,
            int? userGroupId);
        /// <summary>
        /// لیست کاربران موجود در گروه ارسالی در پارامتر
        /// </summary>
        /// <param name="itemGroupId">شناسه گروه </param>
        /// <returns></returns>
        IQueryable<AdditionalUserProp> GetAdditionalUsersPropAssignedToGroup(int itemGroupId);
        /// <summary>
        /// اساین کردن کاربران به گروه ارسالی در پارامتر
        /// یا خارج کردن کاربران از گروه ارسالی در پارامتر 
        /// </summary>
        /// <param name="additionalUserPropIds">لیست شناسه کاربران</param>
        /// <param name="groupId">شناسه گروه</param>
        /// <param name="assign">اساین شود یا از اساین خارج شود</param>
        /// <returns></returns>
        Task AssignSelectedItemsToGroupAsync(List<int> additionalUserPropIds, int groupId, bool assign);
    }
}