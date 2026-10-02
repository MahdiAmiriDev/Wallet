using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels.UserManagement;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Model;

namespace YashilCommerce.Core.Services
{
    public interface IAdditionalUserPropService : IGenericService<AdditionalUserProp, int>
    {
        AdditionalUserProp GetByUserId(int userId, bool readOnly = true);
        /// <summary>
        /// دریافت اطلاعات کاربر بر اساس یوزرآیدی و اپیلیکیشن داده شده
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        AdditionalUserProp GetByUserIdAndApplicationId(int userId, int applicationId);
        IQueryable<AdditionalUserProp> FindByUserId(int id);
        /// <summary>
        /// چک میکند کاربری با شناسه باشگاه و کد باشگاه وجود دارد یا نه
        /// </summary>
        /// <param name="customerClubId"></param>
        /// <param name="customerClubCode"></param>
        /// <returns></returns>
        bool CheckExistenceByFilter(int? customerClubId, string customerClubCode);

        /// <summary>
        /// لیست کاربران را بر اساس ویژگی های کاربر فیلتر می کند
        /// اگر کاربر لاگین شده را در نظر بگیرد
        /// اطلاعات مرکز و نمایندگی و دسته بندی نمایندگی را از کاربر لاگین شده می خواند
        /// <param name="considerCurrentUser">آیا کاربر لاگین شده را در نظر بگیرد</param>
        /// </summary>
        /// <returns></returns>
        IQueryable<User> GetAllUsersByFilter(int? companyId, 
            int? representationId,
            int? representationCategoryId,
            int? personId,
            List<int> itemsGroupId = null,
            bool considerCurrentUser = true);

        IQueryable<AdditionalUserProp> GetByCustomFilter(int? userId, int? applicationId);
        /// <summary>
        /// بررسی می کند آیا کاربرانی که به مرکز اختصاص داده شده اند یا خیر
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <returns></returns>
        Task<bool> IsExistAdditionalUserPropForCompanyAsync(int companyId);

        /// <summary>
        /// بروزرسانی مقدار
        /// ParticipatedInHumaxTest
        /// به مقدار
        /// True
        /// برای کاربرانی که در آزمون استعداد سنجی شرکت کرده اند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>bool</returns>
        Task<bool> UpdateParticipatedInHumaxTestValueAsync(int userId);
        Task<AdditionalUserPropsDto> GetUserAdditionalUserPropAsync(int userId);

        void TryRemoveUserAdditionalUserProp(int userId);

        /// <summary>
        /// کش مربوط به اطلاعات باشگاه مشتریان کاربر را برمیگرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RefreshAdditionalUserPropAsync(int userId);
        /// <summary>
        /// بررسی می کند که آیا باشگاه مشتریان کاربر جاری حکمتی می باشد یا خیر
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckIsHekmatCurrentUserCustomerClub();
        /// <summary>
        /// این متد کار
        /// بروزرسانی مقدار
        /// HasUserPurchasedEmploymentExam
        /// به
        /// true
        /// را انجام می دهد و در صورتی فراخوانی می شود که کاربر آزمون استعدادیابی شغلی را خریده باشد 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>bool</returns>
        Task<bool> UpdateHasUserPurchasedEmploymentExamAsync(int userId);

        /// <summary>
        /// این متد عملیات بروزرسانی مقدار
        /// IsResumeFillOutEnough  = true
        /// را انجام می دهد
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>bool</returns>
        Task<bool> UpdateIsResumeFillOutEnoughAsync(int userId);

        /// <summary>
        /// این متد عملیات تبدیل رشته رزومه ارسالی کاربر به مدل رزومه را انجام می دهد
        /// </summary>
        /// <param name="resume">رشته رزومه کاربر</param>
        /// <returns>UserResume | Null</returns>
        Resume DeserializeResumeStringToResumeModel(string resume);

        /// <summary>
        /// این متد عملیات مربوط به
        /// تبدیل مدل رزومه کاربر به رشته جی سون
        /// را انجام می دهد
        /// </summary>
        /// <param name="resume">رزومه کاربر</param>
        /// <returns>string</returns>
        string SerializeResumeToJson(Resume resume);

        /// <summary>
        /// بررسی می کند که آیا کاربر به مقدار کافی رزومه را پر کرده است یا نه؟
        /// </summary>
        /// <param name="userResume">رزومه کاربر</param>
        /// <returns>bool</returns>
        bool HasUserFillOutResumeEnough(Resume userResume);

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر رزومه را به اندازه ی کافی پر کرده است یا نه؟
        /// همچنین بررسی می کند که سن کاربر برای شرکت در آزمون استعداد یابی در باز مجاز باشد
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns>(bool isSuccess, string message)</returns>
        Task<(bool isSuccess, string message)> ValidateUserResumeAndUserAgeAsync(int userId);
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
        /// این متد بررسی می کند که آیا کاربر رزومه را پر کرده است یا نه 
        /// اگر پر نکرده باشد مقدار 0 بر می گرداند یعنی در حالتی که روزمه کاربر مقدار 
        /// null داشته باشد
        /// در غیر این صورت محاسبه می کند که چند در صد از رزومه را پر کرده است و سپس خروجی را به صورت عدد می دهد
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        Task<int> GetResumeCompletionPercentageByUserIdAsync(int userId);

        /// <summary>
        /// دریافت رزومه کاربر بر اساس شناسه کاربری
        /// در صورتی که کاربر رزومه را پر نکرده باشد مقدار 
        /// null
        /// بر میگرداند همجنین اگر در هنگام 
        /// deserialize
        /// کردن resume به مدل
        /// خطای رخ دهد نیز مقدار null 
        /// بر میگرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Resume || null</returns>
        Task<Resume> GetResumeByUserIdAsync(int userId);
        /// <summary>
        /// بررسی می کند با این شناسه گروه کاربری وجود دارد یا خیر
        /// </summary>
        /// <param name="itemGroupId">شناسه گروه</param>
        /// <returns></returns>
        Task<bool> ExistsAnyAdditionalUserPropServiceByItemGroupIdAsync(int itemGroupId);
        /// <summary>
        /// دریافت کاربران اساین شده در گروه
        /// </summary>
        /// <param name="groupId">شنایه گروه</param>
        /// <returns></returns>
        IQueryable<AdditionalUserProp> GetUsersAssignToGroup(int groupId);
        /// <summary>
        /// لیست کاربرانی که گروه بندی نشده اند
        /// </summary>
        /// <returns></returns>
        IQueryable<AdditionalUserProp> GetUsersForNewGroup(bool readOnly = false);



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
        /// دریافت کاربران گروه بندی شده
        /// </summary>
        /// <param name="itemGroupId">شناسه گروه</param>
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
        Task<bool> AssignSelectedItemsToGroupAsync(List<int> additionalUserPropIds, int groupId, bool assign);

        /// <summary>
        /// این متد جهت بروزرسانی باشگاه مشتریان کاربر جاری به
        /// نوع حکمت کارت کاربرد دارد
        /// در صورت بروز خطا Exception
        /// می دهد که باید توسط متد فراخواننده مدیریت شود
        /// </summary>
        /// <param name="hekamtCardNumber">شماره کارت حکمت</param>
        /// <returns></returns>
        Task UpdateCurrentUserCustomerClubToHekmatAsync(string hekamtCardNumber = null);
    }
}