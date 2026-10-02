using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
	public interface IAdditionalUserPropCompanyGroupService : IGenericService<AdditionalUserPropCompanyGroup, int>
    {
        IQueryable<AdditionalUserPropCompanyGroup> GetByCustomFilter( int? additionalUserPropId, int? companyGroupId);
        /// <summary>
        /// این متد در فرم تخصیص کاربر به گروه مرکز کاربرد دارد
        /// گروه مرکزهایی را که به کاربر اختصاص داده نشده اند یا گروه مرکزهایی را که به کاربر اختصاص داده شده اند،را باتوجه به پارامتر اساین لیست میکند 
        /// اگر پارامتر اساین ترو باشد، با توجه به ای دی کاربر از جدول
        /// AdditionalUserPropCompanyGroup
        /// AssignedCompanyGroup
        /// ها را لیست میکند
        /// اگر پارامتر اساین فالس باشد
        /// از جدول AdditionalUserPropCompanyGroup 
        /// گروه مرکزهایی که به این کاربر اختصاص داده شده اند را سلکت کرده و و در جدول 
        /// CompanyGroup
        /// رکوردهایی که شامل لیست اپلیکیشن های بالا نیست را برمیگرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="assigned"></param>
        /// <returns></returns>
        IQueryable<CompanyGroup> GetAdditionalUserPropCompanyGroups(int userId, bool assigned);
        /// <summary>
        /// این متد برای تخصیص گروه مرکز به کاربر و خارج کردن تخصیص کاربرد دارد و در زمان کلیک بر روی دکمه های فلش
        /// بین باکس های سمت چپ و راست فرم فراخوانی می شود
        /// </summary>
        /// <param name="companyGroupsId">لیست گروه های کاربر مورد نظر</param>
        /// <param name="userId">ای دی کاربر مورد نظر</param>
        /// <param name="assign">تخصیص داده شده یا نشده ، مقدار true یا false برمی گرداند</param>
        /// 
        /// <returns></returns>
        Task<bool> AssignSelectedCompanyGroupesToUser(List<int> companyGroupsId, int userId, bool assign);
    }
}
