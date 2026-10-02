using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDtos.Representation;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IRepresentationRepository : IGenericRepository<Representation, int>
    {
       // IQueryable<Representation> GetByCompanyId(int companyId, bool readOnly = false, int? applicationId = null);
        IQueryable<Representation> GetByCompanyMainCategory(int companyId);
        IQueryable<Representation> GetByCustomFilter(int? addressId, int? companyId, int? founderId, int? licenseType, int? ownershipType, bool showConfirmRepresentation = false);
        /// <summary>
        /// Get Representation By City Id
        /// </summary>
        /// <param name="cityId">شناسه شهر</param>
        /// <returns></returns>
        IQueryable<Representation> GetByCityId(int cityId);

        /// <summary>
        /// نمایندگی های مربوط به مرکز در هر شهر
        /// </summary>
        /// <param name="cityId"> شهر</param>
        /// <param name="companyId">مرکز</param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Representation> GetByCityIdAndCompanyId(int? cityId, int? companyId, bool readOnly = true);
        /// <summary>
        /// get Representation of a user
        /// the user must be the preresentation otherwisae return null
        /// </summary>
        /// <param name="userPrincipalId"></param>
        /// <returns></returns>
        Representation GetUserRepresentation(int userPrincipalId);

        /// <summary>
        /// دریافت نمایندگی ها بر اساس پارمتر های فیلتر
        /// </summary>
        /// <param name="customerClubId">باشگاه مشتریان</param>
        /// <param name="applicationId">برنامه</param>
        /// <param name="company">مرکز</param>
        /// <param name="cityId">شهر</param>
        /// <param name="categoryId">دسته بندی</param>
        /// <param name="categoryGroup">گروه دسته بندی</param>
        /// <param name="isEmploymentCategoryGroup">آیا گروه دسته بندی  اشتغال می باشد</param>
        /// <returns></returns>
        IQueryable<Representation> GetCompanyRepresentationByFilter(
            int customerClubId,
            int applicationId,
            List<int> company,
            int? cityId,
            int? categoryId,
            int? categoryGroup,
            bool isEmploymentCategoryGroup);
        string GetTitle(int id);
        /// <summary>
        /// دریافت تنظیمات کلاس مجازی بر اساس برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalePlanningId">برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<Representation> GetByProductSalePlanningASync(int productSalePlanningId);

        /// <summary>
        /// دریافت نمایندگی های مرکز
        /// </summary>
        /// <param name="companyId">مرکز</param>
        /// <param name="readOnly">خواندنی</param>
        /// <returns></returns>
        IQueryable<Representation> GetByCompanyId(int companyId ,bool readOnly = true);

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
        /// دریافت نمایندگی های مرکز برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId">شنساه برنامه ریزی دوره</param>
        /// <returns></returns>
        IQueryable<Representation> GetByProductSalesPlanningCompany(int productSalesPlanningId);
    }
}
