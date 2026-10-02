using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;


namespace YashilCommerce.Core.Services
{
    public interface IUploadedProductItemService : IGenericService<UploadedProductItem, int>
    {
        /// <summary>
        /// رکورد های مربوط به کاربر را بر می گرداند
        /// </summary>
        /// <param name="userId">کاربر درج کننده</param>
        /// <param name="showInvalidItem"></param>
        /// <returns></returns>
        IQueryable<UploadedProductItem> GetByUserId(int userId, bool showInvalidItem);
        /// <summary>
        /// حذف رکورد های مربوط به کاربر
        /// </summary>
        /// <param name="userId">کاربر درج کننده</param>
        /// <returns></returns>
        Task DeleteByUserIdAsync(int userId);

        /// <summary>
        /// فایل ارسال شده کاربر را در مسیر مستند ریخته 
        /// و نام مسیر و شماره اندیس شیت فایل اکسل و نام جدول برای درج دیتا به همراه نام ستون های جدول را به متد درج دیتا به صورت 
        /// Bulk
        /// ارسال میکند
        /// </summary>
        /// <param name="productTransactionFile">فایل اراسل شده</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Task<ResultStatus<int>> BulkInsertFromExcelAsync(IFormFile productTransactionFile, int companyId);
        /// <summary>
        /// معتبر بودن مقادیر مربوط به قیمت آیتم های محصول بارگذاری شده را بررسی میکند
        /// </summary>
        /// <returns></returns>
        Task<int> GetInvalidUploadedProductItemsCountAsync(int userId,int companyId);
        /// <summary>
        /// بررسی میکند که کاربر آیتم نامعتبر دارد یا خیر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        bool ExistsAnyInvalidItemByUserId(int userId);

        /// <summary>
        /// لیست سورس را در 
        /// MemoryStream 
        /// ریخته و بر می گرداند
        /// </summary>
        /// <param name="source">سورس دیتا که قرار است در اکسل قرار بگیرد</param>
        /// <param name="companyId"></param>
        /// <param name="categoryGroupId"></param>
        /// <returns></returns>
        Task<MemoryStream> CopyProductListToWorkBook(List<ProductDetailViewModel> source, int companyId,int? categoryGroupId);
        /// <summary>
        /// فایل اکسل بارگذاری شده کاربر و سند آن را حذف می کند  
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="deleteInvalidItem">حذف آیتم نامعتبر</param>
        void DeleteUploadedProductItemWithAppDocument(int userId, bool deleteInvalidItem);
        /// <summary>
        ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
        ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاری جدید تغییر می کند این متد مقدار
        ///  true
        ///  بر می گرداند در غیر اینصورت مقدار
        ///  false
        ///  بر می گرداند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        Task<bool> CheckExistsProductsSoldAndRenamedInUploadedProductsAsync(int userId, int applicationId, int companyId);
        /// <summary>
        ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
        ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاری جدید تغییر می کند .
        ///  آن محصولات را بر می  گرداند
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <param name="companyId">شناسه مرکز</param>
        /// <returns></returns>
        IQueryable<UploadedProductItem> GetProductsSoldAndRenamedInUploadedProducts(int userId, int applicationId, int companyId);

        /// <summary>
        ///دریافت تعداد ایتم های برنامه ریزی دوره نامعتبر که
        /// از طریق اکسل بارگذاری شده اند مربوط به سامانه ی
        /// Lms
        /// </summary>
        /// <returns></returns>
        Task<int> GetInvalidUploadedSimpleProductItemsCountAsync(int userId, int companyId);

        /// <summary>
        /// افزودن برنامه ریزی دوره از ایتم های آپلود شده با استفاده از اکسل
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<int> AddDataFromSimpleUploadedProductItemAsync(int applicationId, int userId, int companyId);
    }
}
