using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.Entities;

namespace YashilCommerce.Core.Repositories
{
    public interface IUploadedProductItemRepository : IGenericRepository<UploadedProductItem, int>
    {
        /// <summary>
        ///   رکورد های مرب.ط به کاربر لاگین کرده را بر می گرداند
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="showInvalidItem">فقط رکورد های نامعتبر را نمایش دهد</param>
        /// <returns></returns>
        IQueryable<UploadedProductItem> GetByUserId(int userId, bool showInvalidItem);

        /// <summary>
        /// معتبر بودن مقادیر مربوط به قیمت آیتم های محص.ل بارگذاری شده را بررسی میکند
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="applicationId"></param>
        /// <param name="numberSeparator"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<int> GetInvalidUploadedProductItemsCountAsync(int userId,int applicationId, char numberSeparator, int companyId);

        Task DeleteByUserIdAsync(int userId);
        /// <summary>
        ///  بررسی می کند که محصولاتی که توسط کاربر بارگذاری شده و  قرار است به روز رسانی شود 
        ///  اگر از این محصولات، محصولی توسط مشتری خریداری شده باشد و نام یا توضیحات محسول نیز در بارگذاذی جدید تغییر می کند این متد مقدار
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
