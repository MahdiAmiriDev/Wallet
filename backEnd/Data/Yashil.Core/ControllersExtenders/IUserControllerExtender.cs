using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;

namespace Yashil.Core.ControllersExtenders
{
    public interface IUserControllerExtender
    {
        Task BeforeUpdateAsync(Dictionary<string, string> editModelKeyValues, int userId);
        Dictionary<string, string> GetCustomProps(int id);
        Task BeforeInsert(User modelKeyValues, Dictionary<string, string> editModelKeyValues, int userId);
        ActionRouteInfo GetCustomRegisterViewInfo();
        Task<string> CustomValidateBeforeInsertAsync(Dictionary<string, string> additionalInfoList);
        bool CustomValidateBeforeUpdate(Dictionary<string, string> additionalInfoList, int id);

        Task<string> BeforeLogin(Dictionary<string, string> additionalInfoList, int userId);
        /// <summary>
        /// بعد از لاگین کردن کاربر این متد فراخوانی می شود
        /// </summary>
        /// <param name="userId">شناسه کاربر لاگین کرده</param>
        /// <returns></returns>
        Task<string> AfterLogin(int userId);

        IQueryable<User> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="representationCategoryId"></param>
        /// <param name="personId"></param>
        /// <param name="considerCurrentUserAdditionalData">اطلاعات اضافی کاربر مانند مرکز و نمایندگی و دپارتمان نمایندگی را در نظر بگیرد</param>
        /// <returns></returns>
        IQueryable<User> GetForSelectByFilter(int? companyId, int? representationId, int? representationCategoryId, int? personId, bool considerCurrentUserAdditionalData = true);
    }
}