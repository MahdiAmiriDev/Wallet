
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.Entities;
using YashilCommerce.Core.Enums;

namespace YashilCommerce.Core.Services
{
    public interface IProductService : IGenericService<Product, int>
    {

        IQueryable<Product> GetByCustomFilter(string title, string code, int? categoryId, int? companyId, int? appEntityId, int? productTemplateId, int? giftCardTypeId, int? downloadActivationType, int? recurringCyclePeriodType, int? taxCategoryId, int? basePriceUnitType, int? basePriceBaseUnitType, bool? showArchives);
        IQueryable<Product> GetByCategoryIdAndCompanyId(int categoryId, int? companyId, bool hierarchical = true);

        int GetProductCompanyId(int editModelProductId);
        IQueryable<Product> GetCategoryProducts(int applicationId, int? categoryId, int? companyId, int? cityId, bool loadSubCategories, int count = 20);

        IQueryable<Product> GetRepresentationProductsByCategoryId(int? companyId, int? representationId, int? categoryId, bool hierarchical);
        /// <summary>
        /// این متد برای ذخیره و یا به روز رسانی اطلاعات محصول بارگذاری شده از جدول 
        /// UploadedProductItem
        /// به جدول های 
        /// product  - ProductSalePlaning - DependentProductSale - ProductCategory
        /// میباشد
        /// </summary>
        /// <param name="applicationId">شناسه اپلیکیشن</param>
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
        /// جهت ذخیره سازی فایل زیپ صفحات توضیحات محصول و کپی کردن آن در آدرس توضیحات
        /// </summary>
        /// <param name="productDetailPagesFile"></param>
        /// <returns></returns>
        Task<string> SaveProductDetailPagesFileAndExtractToPath(IFormFile productDetailPagesFile);

        Task<AppDocument> GetProductPicture(int productId, int applicationId);

        /// <summary>
        /// دریافت اطلاعات محصول براساس شناسه برنامه ریزی دوره
        /// </summary>
        /// <param name="productSalesPlanningId">شناسه برنامه ریزی دوره</param>
        /// <returns></returns>
        Task<Product> GetByproductSalesPlanningIdAsync(int productSalesPlanningId);

        /// <summary>
        /// جهت دریافت محصول با تمام ریلیشن های مربوط
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IQueryable<Product> FindById(int productId, bool readOnly = false);


        /// <summary>
        /// بررسی کد محصول هنگام درج در پایگاه داده
        /// درر این متد بررسی میکنیم که کد خالی نباشد و همچنین تکراری نباشد 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string ValidateProductCodeBeforeInsert(string code);
        /// <summary>
        /// ست کردن مقادیر دیفالت برای Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="taxCategoryId"></param>
        /// <param name="isTaxExempt"></param>
        /// <returns></returns>
        Task SetEntityDefaultPropsAsync(Product entity, int taxCategoryId, bool isTaxExempt);

        void SetProductDefaultCategory(Product entity, List<int> categories);
        void SetProductCategories(Product entity, List<int> categories);
        void SetProductSpecificationOption(Product entity, List<int> categories);
        /// <summary>
        /// set sales planing model in inserting new simple or grouped product
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="salesModelType"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        void SetDependentProductSalePlanning(Product entity, int salesModelType, decimal minPrice, decimal maxPrice);

        ProductSalesPlanning CreateProductSalesPlanningObject(int? productId, int price);


        Task CustomMapBeforeInsertAsync(Product entity, List<int> categories, int taxCategoryId, bool isTaxExempt, int salesModelType, decimal minPrice, decimal maxPrice, int? appEntityId);
        //Task<bool> UserHasAccessForProduct(int productId);
        /// <summary>
        /// آیا محصولی برای مرکز تعریف شده است یا خیر ؟؟ 
        /// </summary>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        Task<bool> IsExistProductForCompanyAsync(int companyId);

        /// <summary>
        /// آیا اجازه حذف محصول وجود دارد
        /// بررسی می کند برای محصول چه وابستگی های تعریف شده و با توجه به این محصول اجازه حذف وجود دارد یا خیر
        /// </summary>
        /// <param name="productId">محصول</param>
        /// <returns></returns>
        Task<(bool isAllowed, string message)> IsAllowedDeleteProductAsync(int productId);
        /// <summary>
        /// دریافت محصولات دوره
        /// </summary>
        /// <param name="categoryId">دوره</param>
        /// <returns></returns>
        Task<bool> ExistProductByCategoryIdAsync(int categoryId);

        /// <summary>
        /// دریافت شناسه
        /// salesModeType
        /// مربوط به یک Product
        /// </summary>
        /// <param name="productId">شناسه مرکز</param>
        /// <returns></returns>
        Task<int> GetSalesModeTypeIdAsync(int productId);

        /// <summary>
        /// این متد مقدار حذف محصول را برابر با صحیح می کند. رکورد در دریتابیس موجود است
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> Archive(int id);

        /// <summary>
        /// می کند false این متد مقدار حذف برنامه ریزی دوره را برابر با 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> RestoreFromArchiveAsync(int id);

        /// <summary>
        /// بروزرسانی نوع فروش محصول به نوع انتخابی
        /// </summary>
        /// <param name="newSalesModelType">نوع فروش جدید محصول</param>
        /// <param name="productId">شناسه محصول</param>
        /// <returns></returns>
        Task UpdateProductSalesModeTypeAsync(int productId,SalesModelTypeEnum newSalesModelType);

        /// <summary>
        /// دریافت ویو
        /// ProductMainCategory
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductMainCategoryView> GetProductMainCategoryView();

        /// <summary>
        /// دریافت تعدد دوره های تولید شده
        /// </summary>
        /// <returns></returns>
        public Task<int> GetProductCountBasedOnCurrentUserCompanyAndRepresentationAsync();

        /// <summary>
        /// دریافت اطلاعات دوره های منتشر و تایید شده
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Task<List<ProductSimpleDomainModel>> GetCompanyConfirmedAndPublishedProductSimpleDomainModelAsync(
            int companyId);

        /// <summary>
        /// دریافت لیست محصولات مرکز
        /// برای نمایش به عنوان داده های قابل انتخاب در اکسل بارگذاری محصول
        /// محصولاتی که مربوط به مرکز بوده و
        /// VisibleIndividually = true
        /// باشد
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<List<string>> GetCompanyProductListForFillInUploadedProductItemXlsValidationAsync(int companyId);
    }
}
