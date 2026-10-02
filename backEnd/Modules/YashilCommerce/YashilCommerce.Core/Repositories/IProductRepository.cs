using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {

        IQueryable<Product> GetByCustomFilter(string title, string code, int? categoryId, int? companyId, int? appEntityId, int? productTemplateId, int? giftCardTypeId, int? downloadActivationType, int? recurringCyclePeriodType, int? taxCategoryId, int? basePriceUnitType, int? basePriceBaseUnitType, bool? showArchives);


        int GetProductCompanyId(int productId);

        IQueryable<Product> GetCategoryProducts(int applicationId, int? categoryId, int? companyId, int? cityId, bool loadSubCategories, int count = 20);
        IQueryable<Product> GetByCategoryIdAndCompanyId(int categoryId, int? companyId, bool hierarchical = true);

        IQueryable<Product> GetRepresentationProductsByCategoryId(int? companyId, int? representationId, int? categoryId, bool hierarchical);
        /// <summary>
        /// این متد برای ذخیره و یا به روز رسانی اطلاعات محصول بارگذاری شده از جدول 
        /// UploadedProductItem
        /// به جدول های 
        /// product  - ProductSalePlaning - DependentProductSale - ProductCategory
        /// میباشد
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        Task<int> AddDataFromUploadedProductItemAsync(int applicationId, int userId, int companyId);
        /// <summary>
        /// اطلاعات کامل محصول مربوط به مرکز را بر میگرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        IEnumerable<ProductDetailDomainModel> GetCompanyConfirmedAndPublishedProducts(int companyId);
        /// <summary>
        /// بررسی می کند که کد محصول تکراری است یا خیر 
        /// </summary>
        /// <param name="code">کد محصول</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        bool IsProductCodeDuplicate(string code, int applicationId);
        /// <summary>
        /// دریافت اطلاعات محصول براساس شناسه برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<Product> GetByproductSalesPlanningIdAsync(int productSalesPlanningId);

        IQueryable<Product> FindById(int productId, bool readOnly= false);

        /// <summary>
        /// محصولات تعریف شده یک مرکز را بر می گرداند
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <param name="readOnly">آیا فقط خواندنی است</param>
        /// <returns></returns>
        IQueryable<Product> GetByCompanyId(int companyId, bool readOnly = true);

        Task<bool> ExistProductByCategoryIdAsync(int categoryId);

        /// <summary>
        /// این متد مقدار حذف برنامه ریزی دوره را برابر با صحیح می کند. رکورد در دریتابیس موجود است
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        Task<int> ArchiveAsync(int id);

        /// <summary>
        /// می کند false این متد مقدار حذف برنامه ریزی دوره را برابر با 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> RestoreFromArchiveAsync(int id);

        /// <summary>
        /// دریافت ویو
        /// ProductMainCategory
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductMainCategoryView> GetProductMainCategoryView();

        /// <summary>
        /// دریافت اطلاعات دوره های تعریف شده بر اساس شناسه مرکز
        /// دوره های که منتشر شده اند و تایید شده اند
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<List<ProductSimpleDomainModel>> GetCompanyConfirmedAndPublishedProductSimpleDomainModelAsync(
            int companyId);
    }
}
