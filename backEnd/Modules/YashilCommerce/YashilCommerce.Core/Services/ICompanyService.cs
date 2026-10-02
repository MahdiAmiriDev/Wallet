using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;
using YashilCommerce.Core.DTO;

namespace YashilCommerce.Core.Services
{
    public interface ICompanyService : IGenericService<Company, int>
    {
        /// <summary>
        /// دریافت کمپانی بر اساس پارامتر های فیلتر
        /// </summary>
        /// <param name="searchTitle">عنوان محصول</param>
        /// <param name="applicationId">برنامه</param>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <param name="cityId">شهر</param>
        /// <param name="categoryId">دسته بندی</param>
        /// <param name="categoryGroupId">گروه دسته بندی</param>
        /// <returns></returns>
        Task<IQueryable<Company>> GetApplicationCompaniesAsync(
            string searchTitle,
            int applicationId,
            int customerClubId,
            int? cityId,
            int? categoryId,
            int? categoryGroupId,
            bool orderByCompanyDisplayOrder = true);
        IQueryable<City> GetCityByCompanyId(int? companyId);

        Company GetUserCompany(int userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="categoryId">شناسه دسته بندی</param>
        /// <param name="categoryGroupId">شناسه گروه دسته بندی</param>
        /// <returns></returns>
        Task<IQueryable<City>> GetCityByFilterAsync(int? companyId, int? categoryId,int? categoryGroupId);
        string GetTitle(int id);
        IQueryable<Company> GetCompaniesByCityId(int? cityId);
        /// <summary>
        /// لیست کمپانی هایی را برمیگرداند که
        /// voucherType داشته باشند.
        /// </summary>
        /// <returns></returns>
        IQueryable<Company> GetCompaniesHaveVoucherType(bool readOnly = true);
        IQueryable<Company> GetAllActiveCompanies(bool readOnly = true, int? applicationGroupId = null, int? applicationId = null);
        /// <summary>
        /// دریافت مرکز بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<Company> GetByProductSalePlanningAsync(int productSalePlanningId);
        /// <summary>
        /// آیا اجازه حذف مرکز وجود دارد
        /// بررسی می کند برای مرکز چه وابستگی های تعریف شده و با توجه به این وابستگی اجازه حذف وجود دارد یا خیر
        /// یعنی برای مرکز محصولی تعریف نشده باشد
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        Task<(bool isAllowed, string message)> IsAllowedDeleteCompanyAsync(int companyId);
        /// <summary>
        /// حذف بر اساس شناسه مرکز
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task DeleteCompanyMainCategoryByCompanyIdAsync(int companyId);
        IQueryable<Company> FindById(int id, bool readOnly = true);
        Task UpdateCodePathAsync();
        /// <summary>
        /// بررسی می کند نمایندگی های مرکز دارای برنامه ریزی محصول هستند
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <returns></returns>
        Task<bool> CheckCompanyRepresentationsHasProductSalesPlanning(int companyId);
        /// <summary>
        /// دریافت تمامی کمپانی ها بر اساس شناسه اپلیکیشن
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Company> GetAllCompaniesByApplicationId(int applicationId, bool readOnly = false);
        /// <summary>
        /// این سرویس عملیات بروزرسانی فید CanUserSeeOtherCompany مربوط به یک مرکز را انجام می دهد
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="canUserOtherCompany"></param>
        /// <returns></returns>
        Task<Company> UpdateCanUserSeeOtherCompanyAsync(int companyId, bool canUserOtherCompany);

        /// <summary>
        /// دریافت مراکز به همراه فرزندان
        /// </summary>
        /// <param name="codePath"> codePath مرکز</param>
        /// <param name="applicationId">برنامه</param>
        /// <returns></returns>
        IQueryable<Company> GetCompaniesWithChildren(string codePath, int applicationId);
        /// <summary>
        /// دریافت مراکز به همراه زیر مجموعه های خود،
        /// در صورتی که کاربر لاگین کرده دارای مرکز باشد
        ///  مرکز کاربر لاگین کرده را به همراه زیر مجموعه ها آن
        ///  و codePath
        ///  مرکز کاربر لاگین کرده را
        /// بر می گرداند
        /// اگر مرکز تنظیم "بتواند سایر مراکز را ببیند" را داشته باشد
        /// آنگاه همه مراکز را به همراه زیر مجموعه های بر می گرداند
        /// </summary>
        /// <param name="possibleSeeOtherCompanies"> در صورتی که امکان دارد سایر مراکز را ببیند</param>
        /// <returns></returns>
        Task<(IQueryable<Company> companies, string currentUserCompanyCodePath)> GetCurrentUserCompanyWithChildrenAsync(bool possibleSeeOtherCompanies);
        /// <summary>
        /// دریافت
        /// codePath
        /// و تنظیم نمایش سایر مراکز برای مرکز کاربر جاری
        /// </summary>
        /// <returns></returns>
        Task<(string CodePath, bool CanUserSeeOtherCompany)> GetCurrentUserCompanyInfo();
        /// <summary>
        /// بررسی می کنیم
        /// Parent
        /// امتخابی مرکز معتبر باشد یعنی
        /// comapny Parent CodePath
        /// ارسالی جز
        /// codePath
        /// های مرکز کاربر جاری با زیر مجموعه های آن
        /// باشد
        /// </summary>
        /// <param name="parentId">شناسه پدر مرکز</param>
        /// <param name="currentUserCompanyCodePath">CodePath مرکز کاربر جاری</param>
        /// <returns></returns>
        Task<bool> CheckCompanyParentAsync(int parentId, string currentUserCompanyCodePath);
        /// <summary>
        /// در صورتی که تنظیم اضافه شدن دسته بندی ها به مرکز جدید فعال باشد
        /// این متد دسته بندی ها را به مرکز جدید اضافه می کند
        /// دسته بندی های کاربر جاری را بر میگرداند
        /// و آن ها را به مرکز اساین می کند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        Task AddCategoriesForCompanyAsync(int companyId);
        /// <summary>
        /// در صورتی که کاربر دارای مرکز باشد
        /// شناسه مرکز کاربر  بر می گرداند
        /// در غیر اینصورت شناسه کل مراکز را بر می گرداند
        /// </summary>
        /// <returns></returns>
        Task<List<int>> GetCompanyIds();


        /// <summary>
        /// این متد عملیات حستجو براساس کلید در بین عنوان مراکز را انجام می دهد
        /// نکته مرکز باید جزو مراکز فعال باشد
        /// </summary>
        /// <param name="searchKey">کلید جستجو</param>
        /// <param name="customerClubId">شناسه باشگاه مشتری</param>
        /// <returns>IQueryable<CategorySearchDto></returns>
        Task<IQueryable<CompanySearchDto>> SearchInCompanyTitleAsync(string searchKey, int customerClubId);

        /// <summary>
        /// این متد برای دریافت اطلاعات یک مرکز شامل این که
        /// آیا برنامه ریزی دوره فعال دارد یا نه
        /// آیا گزینه نمایش در صفحه اصلی مرکز فعال است
        /// آیا تصویر حالت موبایل و دسکتاپ دارد یا نه
        /// آیا قرار داد فعال دارد یا نه
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="customerClubId">شناسه باشگاه مشتریان</param>
        /// <returns>CompanyInformationDto</returns>
        Task<CompanyInformationDto> GetCompanyInformation(int companyId, int customerClubId);

        /// <summary>
        /// دریافت اطلاعات مراکز فعالی که دارای تصویر برای حالت موبایل و دسکتاپ باشند
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<CompanySimpleDto>> GetActiveCompaniesIfHasPictureAsync();

        /// <summary>
        /// این متد بررسی می کند که آیا کاربر به شناسه مرکز ارسال شده
        /// دسترسی دارد یا نه ؟
        /// </summary>
        /// <param name="sentCompanyId">شناسه مرکز ارسال شده توسط کاربر</param>
        /// <returns>bool</returns>
        Task<bool> HasCurrentUserAccessToSentCompanyIdAsync(int sentCompanyId);
    }
}
