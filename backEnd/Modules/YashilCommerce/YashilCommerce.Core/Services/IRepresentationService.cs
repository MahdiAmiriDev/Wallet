using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDtos.Representation;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Services
{
    public interface IRepresentationService : IGenericService<Representation, int>
    {
        IQueryable<Representation> GetByCompanyMainCategory(int companyId);
        IQueryable<Representation> GetByCustomFilter(int? addressId, int? companyId, int? founderId, int? licenseType, int? ownershipType, bool showConfirmRepresentation = false);
        /// <summary>
        /// Get Representation By City Id
        /// </summary>
        /// <param name="CityId">شناسه شهر</param>
        /// <returns></returns>
        IQueryable<Representation> GetByCityId(int cityId);
        /// <summary>
        /// get user Representation
        /// </summary>
        /// <param name="userPrincipalId"></param>
        /// <returns></returns>
        Representation GetUserRepresentation(int userPrincipalId);

        /// <summary>
        /// دریافت نمایندگی ها بر اساس پارمتر های فیلتر
        /// </summary>
        /// <param name="applicationId">برنامه</param>
        /// <param name="company">مرکز</param>
        /// <param name="cityId">شهر</param>
        /// <param name="categoryId">دسته بندی</param>
        /// <param name="categoryGroupId">گروه دسته بندی</param>
        /// <returns></returns>
        Task<IQueryable<Representation>> GetCompanyRepresentationByFilterAsync(
            int applicationId,
            List<int> company,
            int? cityId,
            int? categoryId,
            int? categoryGroupId);
        string GetTitle(int value);
        /// <summary>
        /// نمایندگی های مربوط به مرکز در هر شهر
        /// </summary>
        /// <param name="cityId"> شهر</param>
        /// <param name="companyId">مرکز</param>
        /// <returns></returns>
        IQueryable<Representation> GetByCityIdAndCompanyId(int? cityId, int? companyId, bool readOnly = true);

        /// <summary>
        /// دریافت نمایندگی بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId">برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<Representation> GetByProductSalePlanningAsync(int productSalePlanningId);

        /// <summary>
        /// آیا کاربر به این مرکز دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CheckUserAccessForCompanyAsync(int? companyId, int? userId = null);

        /// <summary>
        /// آیا کاربر به این نمایندگی و مرکز دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CheckUserAccessForCompanyAndRepresentationAsync(int? companyId, int? representationId, int? userId = null);

        /// <summary>
        /// آیا نمایندگی برای مرکز وجود دارد
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <returns></returns>
        Task<bool> IsExistRepresentationForCompanyAsync(int companyId);

        /// <summary>
        /// لیست نمایندگی ها را بر اساس مرکز بر می گرداند
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Representation> GetByCompanyId(int companyId, bool readOnly);

        /// <summary>
        /// در صورتی که تنظیم اضافه شدن دسته بندی ها به نمایندگی جدید فعال باشد
        /// این متد دسته بندی ها را به نمایندگی جدید اضافه می کند
        /// دسته بندی های کاربر جاری را بر میگرداند
        /// و آن ها را به نمایندگی اساین می کند
        /// </summary>
        /// <param name="representationId"></param>
        Task AddCategoriesForRepresentationAsync(int representationId);

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر به جاری به شناسه مرکز ارسالی دسترسی دارد یا نه؟
        /// </summary>
        /// <param name="sentRepresentationId">شناسه نمایندگی ارسالی</param>
        /// <returns>bool</returns>
        Task<bool> HasCurrentUserAccessToSentRepresentationIdAsync(int sentRepresentationId);

        /// <summary>
        ///  دریافت لیست نمایندگی ها بر اساس پارامترهای ارسالی
        /// </summary>
        /// <param name="companyIds"></param>
        /// <returns></returns>
        IQueryable<Representation> GetRepresentationByParams(List<int> companyIds);


        /// <summary>
        ///  id دریافت جزییات نمایندگی بر اساس 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<Representation> GetCompanyDetails(int id);

        /// <summary>
        /// بررسی می کند که آیا نمایندگی 
        /// hub
        /// است
        /// یعنی نمایندگی مرکزی مربوط به یک استان
        /// </summary>
        /// <param name="representationId"></param>
        /// <returns></returns>
        Task<bool> IsHubRepresentationAsync(int representationId);
        /// <summary>
        /// بررسی می کند که آیا امکان حذف نمایندگی مورد نظر وجود دارد یا نه
        /// تمام وابستگی های مربوط به نمایندگی بررسی می شود
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool isAllowed, string message)> AllowDeleteRepresntationAsync(int id);
        /// <summary>
        /// حذف دسته بندی نمایندگی بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteRepresentationCategoryByIdAsync(int id);
        /// <summary>
        /// دریافت نمایندگی های مرکز برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId">شنساه برنامه ریزی دوره</param>
        /// <returns></returns>
        IQueryable<Representation> GetByProductSalesPlanningCompany(int productSalesPlanningId);
    }
}